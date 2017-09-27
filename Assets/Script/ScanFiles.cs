using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ScanFiles : MonoBehaviour
{
    
    public static Texture2D LoadTextureFromFile(string filename)
    { 
        FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];

        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 1;
        int height = 1;  
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        return texture;
    }

    public static List<MediaItem> GetFileNames(string path)
    {
        List<MediaItem> files = new List<MediaItem>();
        var folders = Directory.GetDirectories(path);
        foreach (var folder in folders)
        {
            var filenames = Directory.GetFiles(folder);
            foreach (var name in filenames)
            {
                var item = new MediaItem();
                var filetype = name.Substring(name.Length - 4);
                if (filetype == ".jpg" || filetype == ".png" || filetype == "jpeg")
                {
                    item.Path = name;
                    if (folder.Contains("Para"))
                    {
                        item.Type = MediaType.ParaPhoto;
                        //item.PicTexture = LoadTextureFromFile(name);
                        files.Add(item);
                    }
                    else if (folder.Contains("OverU"))
                    {
                        item.Type = MediaType.OverunderPhoto;
                        //item.PicTexture = LoadTextureFromFile(name);
                        files.Add(item);
                    }
                }
                if (filetype == ".mp4" || filetype == ".mov")
                {
                    item.Path = "file://" + name;
                    if (folder.Contains("Para"))
                    {
                        item.Type = MediaType.ParaVideo;
                        files.Add(item);
                    }
                    else if (folder.Contains("OverU"))
                    {
                        item.Type = MediaType.OverunderVideo;
                        files.Add(item);
                    }
                    else if (folder.Contains("SbS"))
                    {
                        item.Type = MediaType.SideToVideo;
                        files.Add(item);
                    } 
                } 
            }
        } 
        return files;
    }
    private void LoadByWWW(string path)
    {
        StartCoroutine(Load(path));
    }

    IEnumerator Load(string path)
    {
        double startTime = (double)Time.time;
        //请求WWW
        WWW www = new WWW(path);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
           var texture = www.texture;
        }
    }

}
public class MediaItem
{
    public string Path { get; set; }
    public MediaType Type { get; set; }
    public Texture PicTexture { get; set; }
}
public enum MediaType
{
    ParaPhoto = 0,
    OverunderPhoto = 1,
    ParaVideo = 2,
    SideToVideo = 3,
    OverunderVideo = 4

}