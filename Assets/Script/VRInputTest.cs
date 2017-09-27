using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRInputTest : MonoBehaviour
{
    [SerializeField] private VRInput m_VRInput;

    public Text Te;
    //该脚本激活时，注册对玩家操作的监听  
    void OnEnable()
    {
        m_VRInput.OnCancel += OnCalcel;
        m_VRInput.OnClick += OnClick;
        m_VRInput.OnDown += OnDown;
        m_VRInput.OnUp += OnUp;
        m_VRInput.OnDoubleClick += OnDoubleClick;
        m_VRInput.OnSwipe += OnSwip; 
    }


    void OnSwip(VRInput.SwipeDirection swipeDirection)
    {
        if (swipeDirection != VRInput.SwipeDirection.NONE)
        {
            Debug.Log("Unity+OnSwipe" + swipeDirection);
        }

    }

    //取消对玩家输入的监听  
    void OnDisable()
    {
        m_VRInput.OnCancel -= OnCalcel;
        m_VRInput.OnClick -= OnClick;
        m_VRInput.OnDown -= OnDown;
        m_VRInput.OnUp -= OnUp;
        m_VRInput.OnDoubleClick -= OnDoubleClick;
        m_VRInput.OnSwipe -= OnSwip;
    }

    void OnCalcel()
    {
        Debug.Log("Unity+OnCancel");
        Te.text = "Unity+OnCancel";
    }

    void OnClick()
    {
        Debug.Log("Unity+OnClick"); Te.text = "Unity+OnCancel";
    }
    void OnDown()
    {
        Debug.Log("Unity+OnDown"); Te.text = "Unity+OnCancel";
    }
    void OnUp()
    {
        Debug.Log("Unity+OnUp"); Te.text = "Unity+OnCancel";
    }
    void OnDoubleClick()
    {
        Debug.Log("Unity+OnDoubleClick"); Te.text = "Unity+OnCancel";
    }

    // Use this for initialization  
    void Start()
    {

    }
}  
