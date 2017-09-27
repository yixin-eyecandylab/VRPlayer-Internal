using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BtnStatu
{
    Free = 0,
    Pressing = 1,
    Pressed = 2,

}

public class GazeControl : MonoBehaviour
{
    public float rotateSpeed = 100f;

    public GameObject PlayControl;

    AutoPlayVideos PlayCtrl;

    private BtnStatu leftStatu = BtnStatu.Free;
    private BtnStatu rightStatu = BtnStatu.Free;
      
    private DateTime leftTime;
    private DateTime rightTime;

    public GameObject Left;
    public GameObject Right;

    RaycastHit hit;
    int mask ;
    public GameObject target;

    //private Vector3 Direction;
    void Start()
    {
#if UNITY_ANDROID
        PlayCtrl = PlayControl.GetComponent<AutoPlayVideos>();
#endif
       
        mask = LayerMask.GetMask("Btn");
    }
    void Update()
    { 
        testRay();
        
        //allRay();
        
    }

    void testRay()
    {
        //Direction = transform.TransformDirection(target.transform.localPosition);
        Ray ray = new Ray(transform.position, target.transform.position);
        //var p = ray.GetPoint(100);
        //Debug.DrawLine(transform.position, p, Color.red, 1);
        var hitted = Physics.Raycast(ray, out hit,100, mask);
        //Debug.Log("Hitted ? " +hitted + mask);
        if (hitted)
        {
            //Debug.Log("Triger "+hit.collider.gameObject.tag);
            var tag = hit.collider.gameObject.tag;
            if (tag == "LeftBtn")
            {
                if (leftStatu == BtnStatu.Free)
                {
                    leftTime = DateTime.Now;

                }
                var dtime = DateTime.Now - leftTime;
                if (dtime < TimeSpan.FromSeconds(1))
                {
                    leftStatu = BtnStatu.Pressing;
                    //Material m = new Material();
                    Left.GetComponent<Renderer>().material.color = Color.gray;
                }
                if (dtime > TimeSpan.FromSeconds(1) && leftStatu == BtnStatu.Pressing)
                {
                    Debug.Log("Left Button Clicked!");
                    leftStatu = BtnStatu.Pressed;
                    PlayCtrl.PreItem();
                }

            }
            else if (tag == "RightBtn")
            {
                if (rightStatu == BtnStatu.Free)
                {
                    rightTime = DateTime.Now;
                }
                var dtime = DateTime.Now - rightTime;
                if (dtime < TimeSpan.FromSeconds(1))
                {
                    rightStatu = BtnStatu.Pressing;
                    Right.GetComponent<Renderer>().material.color = Color.gray;
                }

                if (dtime > TimeSpan.FromSeconds(1) && rightStatu == BtnStatu.Pressing)
                {
                    Debug.Log("Right Button Clicked!");
                    rightStatu = BtnStatu.Pressed;
                    PlayCtrl.NextItem();
                }
            }
            else
            {
                leftStatu = rightStatu = BtnStatu.Free;
                leftTime = rightTime = DateTime.Now;
                Right.GetComponent<Renderer>().material.color = Color.white;
                Left.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            leftStatu = rightStatu = BtnStatu.Free;
            leftTime = rightTime = DateTime.Now;
            Right.GetComponent<Renderer>().material.color = Color.white;
            Left.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    void allRay()
    {
        var Hits = Physics.RaycastAll(new Ray(transform.position,target.transform.position));
        Debug.Log(Hits.Length);
        var hit = Hits.Where(p => p.collider.gameObject.tag == "LeftBtn" || p.collider.gameObject.tag=="RightBtn");
        if(hit.Count()>0)
        {
            var tag = hit.First().collider.gameObject.tag;
            if (tag == "LeftBtn")
            {
                if (leftStatu == BtnStatu.Free)
                {
                    leftTime = DateTime.Now;

                }
                var dtime = DateTime.Now - leftTime;
                if (dtime < TimeSpan.FromSeconds(1))
                {
                    leftStatu = BtnStatu.Pressing;
                    //Material m = new Material();
                    Left.GetComponent<Renderer>().material.color = Color.gray;
                }
                if (dtime > TimeSpan.FromSeconds(1) && leftStatu == BtnStatu.Pressing)
                {
                    Debug.Log("Left Button Clicked!");
                    leftStatu = BtnStatu.Pressed;
                    //PlayCtrl.PreItem();
                }

            }
            else if (tag == "RightBtn")
            {
                if (rightStatu == BtnStatu.Free)
                {
                    rightTime = DateTime.Now;
                }
                var dtime = DateTime.Now - rightTime;
                if (dtime < TimeSpan.FromSeconds(1))
                {
                    rightStatu = BtnStatu.Pressing;
                    Right.GetComponent<Renderer>().material.color = Color.gray;
                }

                if (dtime > TimeSpan.FromSeconds(1) && rightStatu == BtnStatu.Pressing)
                {
                    Debug.Log("Right Button Clicked!");
                    rightStatu = BtnStatu.Pressed;
                    //PlayCtrl.NextItem();
                }
            }
            else
            {
                leftStatu = rightStatu = BtnStatu.Free;
                leftTime = rightTime = DateTime.Now;
                Right.GetComponent<Renderer>().material.color = Color.white;
                Left.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            leftStatu = rightStatu = BtnStatu.Free;
            leftTime = rightTime = DateTime.Now;
            Right.GetComponent<Renderer>().material.color = Color.white;
            Left.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
