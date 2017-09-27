using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInput : MonoBehaviour
{

    public enum SwipeDirection
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
    };

    public event Action<SwipeDirection> OnSwipe; //在touchPad上滑动  
    public event Action OnClick; //点击touchPad  
    public event Action OnDown; //按下  
    public event Action OnUp;//抬起  
    public event Action OnDoubleClick;//双击  
    public event Action OnCancel;//点击touchPad的返回键  

    [SerializeField] private float m_DoubleClickTime = 0.3f;//有效双击的最大时间间隔  
    [SerializeField] private float m_SwipeWidth = 0.3f; //有效滑动的最小位移  

    private Vector2 m_MouseDownPosition; //记录手指按下的位置  
    private Vector2 m_MouseUpPosition; //记录手指抬起的位置  
    private float m_LastMouseUpTime; //上一次手指抬起的时间，用来检测双击  
    private float m_LastHorizontalValue;
    private float m_LastVerticalValue;

    public float DoubleClickTime { get { return m_DoubleClickTime; } }
    // Use this for initialization  
    void Start()
    {

    }

    // Update is called once per frame  
    private void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        SwipeDirection swipe = SwipeDirection.NONE;
        if (Input.GetButtonDown("Fire1"))//手指触碰到touchPad  
        {
            m_MouseDownPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (OnDown != null)
            {
                OnDown();
            }
        }

        if (Input.GetButtonUp("Fire1"))//手指离开touchPad  
        {
            m_MouseUpPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            swipe = DetectSwipe();//  
        }
        if (swipe == SwipeDirection.NONE)
        {
            swipe = DetectKeyboardEmulatedSwipe();
        }

        if (OnSwipe != null)
        {
            OnSwipe(swipe);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (OnUp != null)
            {
                OnUp();
            }

            if (Time.time - m_LastMouseUpTime < m_DoubleClickTime)
            {
                if (OnDoubleClick != null)
                {
                    OnDoubleClick();
                }
            }
            else
            {
                if (OnClick != null)
                {
                    OnClick();
                }
            }
            m_LastMouseUpTime = Time.time;
        }
        if (Input.GetButtonDown("Cancel")) //点击头显上的返回键  
        {
            if (OnCancel != null)
            {
                OnCancel();
            }
        }
    }
    /// <summary>  
            /// 检测滑动方向  
            /// </summary>  
            /// <returns> 滑动方向，上下左右</returns>  
    private SwipeDirection DetectSwipe()
    {
        Vector2 swipeData = (m_MouseUpPosition - m_MouseDownPosition).normalized;
        bool swipeIsVertical = Mathf.Abs(swipeData.x) < m_SwipeWidth;
        bool swipeIsHorizontal = Mathf.Abs(swipeData.y) < m_SwipeWidth;

        if (swipeData.y > 0f && swipeIsVertical)
        {
            return SwipeDirection.UP;
        }

        if (swipeData.y < 0 && swipeIsVertical)
        {
            return SwipeDirection.DOWN;
        }

        if (swipeData.x > 0 && swipeIsHorizontal)
        {
            return SwipeDirection.RIGHT;
        }
        if (swipeData.x < 0 && swipeIsHorizontal)
        {
            return SwipeDirection.LEFT;
        }
        return SwipeDirection.NONE;
    }

    //检测键盘或者手柄模拟的晃动方向  
    private SwipeDirection DetectKeyboardEmulatedSwipe()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool noHorizontalInputPreviously = Mathf.Abs(m_LastHorizontalValue) < float.Epsilon;
        bool noVerticalInputPreviously = Mathf.Abs(m_LastVerticalValue) < float.Epsilon;
        m_LastHorizontalValue = horizontal;
        m_LastVerticalValue = vertical;
        if (vertical > 0f && noVerticalInputPreviously)
            return SwipeDirection.UP;
        if (vertical < 0f && noVerticalInputPreviously)
            return SwipeDirection.DOWN;
        if (horizontal > 0f && noHorizontalInputPreviously)
            return SwipeDirection.RIGHT;
        if (horizontal < 0f && noHorizontalInputPreviously)
            return SwipeDirection.LEFT;

        return SwipeDirection.NONE;
    }

    private void OnDestroy()
    {
        OnSwipe = null;
        OnClick = null;
        OnDoubleClick = null;
        OnDown = null;
        OnUp = null;
    }
}
