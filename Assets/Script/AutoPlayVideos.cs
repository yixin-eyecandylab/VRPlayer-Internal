using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlayVideos : MonoBehaviour
{
    private string androidpath = "/storage/emulated/0/VisardPlayer";
    private string pcpath = @"E:\Pics";
    private string path;
    //public MediaPlayerCtrl LeftMediaCtrl;
    //public MediaPlayerCtrl RightMediaCtrl;
    private List<MediaItem> files;
    private int PlayIndex = 0;
    private int Left = 8;
    private int Right = 9;
    private int Para = 10;

    public GameObject LeftEye;
    public GameObject RightEye;
    public GameObject MainCamera;

    private GameObject TopPhoto;
    private GameObject BottomPhoto;
    private GameObject LeftVideo;
    private GameObject RightVideo;
    private GameObject TopVideo;
    private GameObject BottomVideo;
    private GameObject ParaPhoto;
    private GameObject ParaVideo;

    public GameObject PreTopPhoto;
    public GameObject PreBottomPhoto;
    public GameObject PreLeftVideo;
    public GameObject PreRightVideo;
    public GameObject PreTopVideo;
    public GameObject PreBottomVideo;
    public GameObject PreParaPhoto;
    public GameObject PreParaVideo;

    public GameObject Videomanager;
    private MediaPlayerCtrl mediaCtrl;

    public GameObject ItemType;

    [SerializeField] private VRInput m_VRInput;
    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        path = pcpath;
#elif UNITY_ANDROID
        path = androidpath;
#else
        path = pcpath;
#endif

        files = ScanFiles.GetFileNames(path);
        mediaCtrl = Videomanager.GetComponent<MediaPlayerCtrl>();
        LoadDefault();
        Debug.Log("There is " + files.Count + " files in " + path + " folder");
        PlayItem();
      

#if UNITY_ANDROID
        m_VRInput = this.GetComponent<VRInput>();
        m_VRInput.OnCancel += M_VRInput_OnCancel;
        m_VRInput.OnSwipe += M_VRInput_OnSwipe;
        m_VRInput.OnClick += M_VRInput_OnClick;
#endif

    }

    private void M_VRInput_OnClick()
    {
        NextItem();
    }

    private void M_VRInput_OnSwipe(VRInput.SwipeDirection obj)
    {
        switch (obj)
        {
            case VRInput.SwipeDirection.LEFT | VRInput.SwipeDirection.UP:
                PreItem();
                break;
            case VRInput.SwipeDirection.RIGHT | VRInput.SwipeDirection.DOWN:
                NextItem();
                break;
        }
    }

    private void M_VRInput_OnCancel()
    {
        PreItem();
    }

    private void LoadSideItem(int destory, int load)
    {
        files[destory].PicTexture = null;
        if (files[load].Type == MediaType.ParaPhoto || files[load].Type == MediaType.OverunderPhoto)
        {
            LoadByWWW(files[load].Path, load);
        } 
    }
    private void LoadDefault()
    { 
        if (files[0].Type == MediaType.ParaPhoto || files[0].Type == MediaType.OverunderPhoto)
        {
           files[0].PicTexture = ScanFiles.LoadTextureFromFile(files[0].Path);
        }
        if (files[1].Type == MediaType.ParaPhoto || files[1].Type == MediaType.OverunderPhoto)
        {
            LoadByWWW(files[1].Path, 1);
        }
        if (files[files.Count - 1].Type == MediaType.ParaPhoto || files[files.Count - 1].Type == MediaType.OverunderPhoto)
        {
            LoadByWWW(files[files.Count - 1].Path, files.Count - 1);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
            PreItem();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NextItem();
        }
    }
    public void PreItem()
    {
        int des, load = 0;
        if (PlayIndex + 1 < files.Count)
        {
            des = PlayIndex + 1;
        }
        else
        {
            des = 0;
        }
        if (PlayIndex - 2 >= 0)
        {
            load = PlayIndex - 2;
        }
        else
        {
            if (PlayIndex - 2 == -1)
            {
                load = files.Count - 1;
            }
            else
            {
                load = files.Count - 2;
            }
        }
        LoadSideItem(des, load);
        DestroyItem();
        PlayIndex--;
        if (PlayIndex >= 0)
            PlayItem();
        else
        {
            PlayIndex = files.Count - 1;
            PlayItem();
        }
    }
    public void NextItem()
    { 
        int des, load = 0;
        if (PlayIndex - 1 >= 0)
        {
            des = PlayIndex - 1;
        }
        else
        {
            des = files.Count - 1;
        }
        if (PlayIndex + 2 < files.Count)
        {
            load = PlayIndex + 2;
        }
        else
        {
            if (PlayIndex + 2 == files.Count)
            {
                load = 0;
            }
            else
            {
                load = 1;
            }
        }
        LoadSideItem(des, load);
        DestroyItem();
        PlayIndex++;
        if (PlayIndex < files.Count)
            PlayItem();
        else
        {
            PlayIndex = 0;
            PlayItem();
        }
    }
    public void DestroyItem()
    {
        switch (files[PlayIndex].Type)
        {
            case MediaType.OverunderPhoto:
                Destroy(TopPhoto);
                Destroy(BottomPhoto);
                break;
            case MediaType.OverunderVideo:
                Destroy(TopVideo);
                Destroy(BottomVideo);
                break;
            case MediaType.ParaPhoto:
                Destroy(ParaPhoto);
                break;
            case MediaType.ParaVideo:
                Destroy(ParaVideo);
                break;
            case MediaType.SideToVideo:
                Destroy(LeftVideo);
                Destroy(RightVideo);
                break;
        }
    }
    public void PlayItem()
    {
      
        mediaCtrl.Stop();
        mediaCtrl.UnLoad();
        var file = files[PlayIndex];
        var name = file.Path;
        ItemType.GetComponent<TextMesh>().text = "Type: " + file.Type.ToString();
        //Debug.Log("now item's index is  " + PlayIndex + "name: " + name);
        switch (file.Type)
        {
            case MediaType.ParaPhoto:
                ShowParaPhoto(file);
               
                break;
            case MediaType.ParaVideo:
                PlayParaVideo(name);
                break;
            case MediaType.OverunderPhoto:
                ShowOverUnderPhoto(file);
                break;
            case MediaType.OverunderVideo:
                PlayOverUnderVideo(name);
                break;
            case MediaType.SideToVideo:
                PlaySbSVideo(name);
                break;
        }
    }

    public void ShowParaPhoto(MediaItem file)
    { 
        ParaPhoto = GetPrefab(PreParaPhoto);
        ParaPhoto.layer = Para;
        SetTexture(ParaPhoto, file);
        SwitchToMainCamera();
    }
    public void ShowOverUnderPhoto(MediaItem file)
    { 
        TopPhoto = GetPrefab(PreTopPhoto);
        BottomPhoto = GetPrefab(PreBottomPhoto);
        TopPhoto.layer = Left;
        BottomPhoto.layer = Right;
        SetTexture(TopPhoto, file);
        SetTexture(BottomPhoto, file);
        SwitchToDoubleEyes();
    }
    public void PlayParaVideo(string file)
    {
        mediaCtrl.Load(file);
        mediaCtrl.Play();
        ParaVideo = GetPrefab(PreParaVideo);
        ParaVideo.layer = Para;
        mediaCtrl.m_TargetMaterial[0] = ParaVideo;
        mediaCtrl.m_TargetMaterial[1] = null;
        SwitchToMainCamera();
    }

    public void PlayOverUnderVideo(string file)
    {
        mediaCtrl.Load(file);
        mediaCtrl.Play();
        TopVideo = GetPrefab(PreTopVideo);
        BottomVideo = GetPrefab(PreBottomVideo);
        TopVideo.layer = Left;
        BottomVideo.layer = Right;
        mediaCtrl.m_TargetMaterial[0] = TopVideo;
        mediaCtrl.m_TargetMaterial[1] = BottomVideo;
        SwitchToDoubleEyes();
    }
    public void PlaySbSVideo(string file)
    { 
        mediaCtrl.Load(file);
        mediaCtrl.Play();
        LeftVideo = GetPrefab(PreLeftVideo);
        RightVideo = GetPrefab(PreRightVideo);
        LeftVideo.layer = Left;
        RightVideo.layer = Right;
        mediaCtrl.m_TargetMaterial[0] = LeftVideo;
        mediaCtrl.m_TargetMaterial[1] = RightVideo;
        SwitchToDoubleEyes();
    }
    public void SetTexture(GameObject obj, MediaItem file)
    {
        obj.GetComponent<Renderer>().material.mainTexture = file.PicTexture;
    }
    
    public void SwitchToMainCamera()
    {
        if (!MainCamera.activeSelf)
        {
            MainCamera.SetActive(true);
        }
        if (LeftEye.activeSelf || RightEye.activeSelf)
        {
            LeftEye.SetActive(false);
            RightEye.SetActive(false);
        }
    }
    public void SwitchToDoubleEyes()
    {
        if (MainCamera.activeSelf)
        {
            MainCamera.SetActive(false);
        }
        if (!LeftEye.activeSelf)
        {
            LeftEye.SetActive(true);
            RightEye.SetActive(true);
        }
    }
    public GameObject GetPrefab(GameObject objPrefab)
    {
        //GameObject objPrefab = (GameObject)Resources.Load("Prefab/" + name); 
        return MonoBehaviour.Instantiate(objPrefab);

    }
    private void LoadByWWW(string path,int index)
    {
        StartCoroutine(Load(path,index));
        Debug.Log("loading..."+ path);
    }

    IEnumerator Load(string path,int index)
    {
        double startTime = (double)Time.time;
        //请求WWW
        WWW www = new WWW("file://"+path);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            files[index].PicTexture = www.texture;
            Debug.Log(files[index].Path + " is loaded");
        }
    }
}
