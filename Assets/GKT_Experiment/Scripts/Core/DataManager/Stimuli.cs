using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
}
public partial class DataManager
{
    public class Stimuli
    {
        private static Stimuli _instance = null;
        public static Stimuli Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Stimuli();
                }
                return _instance;
            }
        }
        string[] mondrianVideoPath;

        Sprite[] weakeyeImage;
        public Sprite[] getWeakEyeImage { get { return weakeyeImage; } }
        //random return one of them 
        public Sprite[] getTrialImage(int trial)
        {
            Sprite[] trial_image = weakeyeImage.SubArray(trial * 4, 4);
            return trial_image;
        }
        public string getMondrianVideoPath
        {
            get
            {
                int length = mondrianVideoPath.Length;
                int rnd_index = UnityEngine.Random.Range(0, length - 1);
                return mondrianVideoPath[rnd_index];
            }
        }
        public void ReadSpriteName()
        {
            foreach (var item in weakeyeImage)
            {
                Debug.Log(item.name);
            }
        }

        public void ReadFile(string _imageFolderPath, string _videoPath)
        {
            ReadVideoPath(_videoPath);
            ReadImageFile(_imageFolderPath);
        }

        void ReadVideoPath(string _videoPath)
        {
            DirectoryInfo dir = new DirectoryInfo(_videoPath);
            var videos = dir.GetFiles("*.mov");
            mondrianVideoPath = new string[videos.Length];
            for (int i = 0; i < videos.Length; i++)
            {
                mondrianVideoPath[i] = videos[i].FullName;
            }
            Debug.Log(string.Format("Get {0} video from {1}", videos.Length, _videoPath));

        }


        void ReadImageFile(string _ImageFolderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(_ImageFolderPath);
            try
            {
                var folders = dir.GetDirectories("Trial?");
                Debug.Log(string.Format("ReadImg : Get {0} Folders from {1}", folders.Length, _ImageFolderPath));
                //every trial have four image
                weakeyeImage = new Sprite[folders.Length * 4];
                // Debug.Log("Read img : Image Length " + folders.Length * 4);
                int index = 0;
                foreach (var dChild in folders)
                {

                    //minus 1 is because the folder naming index is starting from 1 not 0
                    // int index = Convert.ToInt32(dChild.Name.Substring(dChild.Name.Length - 1)) - 1;

                    DirectoryInfo trialFolder = new DirectoryInfo(dChild.FullName);
                    var files = trialFolder.GetFiles("*.png");
                    foreach (var png in files)
                    {
                        // Debug.Log(png.Name);
                        byte[] bytes = File.ReadAllBytes(png.FullName);

                        Texture2D texture = new Texture2D(1, 1);
                        texture.LoadImage(bytes);
                        weakeyeImage[index] = Sprite.Create(
                            texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f));
                        weakeyeImage[index++].name = png.Name;
                        // if(weakeyeImage == null){
                        //     Debug.Log(texture.ToString());
                        //     Debug.Log("Texture width : " + texture.width + " , Texture hieght : " + texture.height);
                        // }

                    }

                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.LogError("Image Read Fail");
            }


        }

    }

}
