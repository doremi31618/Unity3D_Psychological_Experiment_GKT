using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Video;
public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
}
public class DataManager
{

    [System.Serializable]
    public class Trial
    {
        public int index;
        public string top_left;
        public string top_right;
        public string bottom_right;
        public string bottom_left;
        public float finishTime;
        public float finalAlpha;

        public Trial()
        {
            index = 0;
            top_left = "";
            top_right = "";
            bottom_right = "";
            bottom_left = "";
            finishTime = 0;
            finalAlpha = 0;
        }
        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
    [System.Serializable]
    public class ExperimentRecord
    {
        private static ExperimentRecord _instance = null;
        public static ExperimentRecord Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExperimentRecord();
                }
                return _instance;
            }
        }
        [SerializeField] string name;
        [SerializeField] string date;
        [SerializeField] float version;
        [SerializeField] Trial[] trialCollection;
        public string Name { set { name = value; } }

        public void Initialize(string recordname, int trialNumber, float _version)
        {
            date = System.DateTime.Now.ToString();
            version = _version;
            trialCollection = new Trial[trialNumber];
            name = recordname;

        }


        public void addTrialRecord(int index, Trial _data)
        {
            if (trialCollection[index] == null)
                trialCollection[index] = new Trial();
            trialCollection[index] = _data;
            // Debug.Log(_data);
            // Debug.Log(trialCollection[index]);
        }
        public void SaveAllRecord(string _path)
        {
            StreamWriter sw = new StreamWriter(_path);
            string jsonFormat = JsonUtility.ToJson(this);
            sw.Write(jsonFormat);
            sw.Close();
        }
    }
    [System.Serializable]
    public struct SettingFormat
    {
        public float version { get; }
        public int trialNumber;

        public int mode;
        public string recordName;
        public string recordPath;
        public string mondrianVideoPath;
        public string visualTargetPath;
        public string recordFolderPath;

        public float delayTime;
        public float maxAlpha;
        public float maxTime;

        // The Initial value
        public SettingFormat(float _version)
        {
            version = _version;
            trialNumber = 7;

            mode = 0;
            recordName = "Subject_1";
            recordPath = "";
            mondrianVideoPath = "";
            visualTargetPath = "";

            delayTime = 3f;
            maxAlpha = 0.5f;
            maxTime = 60f;

            mondrianVideoPath = "";
            visualTargetPath = "";
            recordFolderPath = "";
            recordPath = "";
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
            ;
        }
    }

    [System.Serializable]
    public class ExperimentSetting
    {
        private static ExperimentSetting _instance;
        public static ExperimentSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExperimentSetting("Default");
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        [SerializeField] public SettingFormat format;

        public float version { get { return format.version; } }
        public int trialNumber { get { return format.trialNumber; } }

        public int mode { get { return format.mode; } }
        public string recordName { get { return format.recordName; } }
        public string recordPath { get { return format.recordPath; } }
        public string mondrianVideoPath { get { return format.mondrianVideoPath; } }
        public string visualTargetPath { get { return format.visualTargetPath; } }
        public string recordFolderPath { get { return format.recordFolderPath; } }

        public float delayTime { get { return format.delayTime; } }
        public float maxAlpha { get { return format.maxAlpha; } }
        public float maxTime { get { return format.maxTime; } }

        //alpha rate = maxAlpha/maxTime

        // setting default value
        public ExperimentSetting(string _filePath)
        {
            if (_filePath == "Default")
                _filePath = UnityEngine.Application.streamingAssetsPath;

            format = new SettingFormat(GKT_Experiment.version);

            format.mondrianVideoPath = Path.Combine(_filePath, "mondrian");
            format.visualTargetPath = Path.Combine(_filePath, "Visual_target");

            format.recordFolderPath = Path.Combine(_filePath, "Record");
            Directory.CreateDirectory(format.recordFolderPath);
            
            format.recordName = "Subject_1";
            format.recordPath = Path.Combine(format.recordFolderPath, format.recordName);

        }
        string GetLatestRecordFolderPath(){
            string path = recordFolderPath;
            DirectoryInfo dir = new DirectoryInfo(path);
            var dir_folders = dir.GetDirectories();
            string _recordName = recordName.Split('_')[0];
            int total = 0;
            foreach (var folder in dir_folders){
                if (folder.Name.Split('_')[0] == _recordName){
                    int index = int.Parse(folder.Name.Split('_')[1]);
                    total += 1;
                }
            }
            if (total == 0)return null;

            _recordName = _recordName + "_" + total;
            return Path.Combine( path, _recordName);
        }
        
        public void CreateRecordFolder(){
            string latest_record_path = GetLatestRecordFolderPath();
            Debug.Log("latest_record_path : " + latest_record_path);
            if (latest_record_path == null)
                latest_record_path = recordPath;
            DirectoryInfo dir = new DirectoryInfo(latest_record_path);
            
            if (dir.Exists)
            {
                Debug.Log("record Directory exist");
                FileInfo file = new FileInfo(Path.Combine(latest_record_path, "ExperimentSetting.json"));

                //if there is a experiment setting file in the folder , create a new folder and save new setting to it
                if ( file.Exists){
                    Debug.Log("Experiment setting exist");
                    string[] record_name_split = dir.Name.Split('_');
                    int record_index = int.Parse(record_name_split[1])+ 1;
                    string new_record_name = record_name_split[0] + "_" + record_index;
                    format.recordName = new_record_name;
                    format.recordPath = Path.Combine(format.recordFolderPath, new_record_name);
                    Directory.CreateDirectory(format.recordPath);
                }
                
            }else{
                Directory.CreateDirectory(format.recordPath);
            }
            Debug.Log("record_path : " + format.recordPath);
        }
        

        public void ChangeSetting(SettingFormat _format)
        {
            format = _format;
            // Debug.Log(_format);
        }

        public void WriteSetting(string _filePath)
        {

            string fileName = "ExperimentSetting.json";
            if (_filePath == "")
            {
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.ShowDialog();
                _filePath = path.SelectedPath;

            }
            else if (_filePath == "Default")
            {
                _filePath = UnityEngine.Application.streamingAssetsPath;

            }

            _filePath = Path.Combine(_filePath, fileName);
            string json = JsonUtility.ToJson(this);

            try
            {
                StreamWriter setting_json_file = new StreamWriter(_filePath);
                setting_json_file.Write(json);
                setting_json_file.Close();
                Debug.Log("Write Experiment Setting Success");
                // Debug.Log(json);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception Message: " + ex.Message);
            }


        }
        public void ReadSetting(string _filePath)
        {
            try
            {
                if (_filePath == "")
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "json files (*.json)|*.json";
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _filePath = dialog.FileName;
                    }

                }
                else if (_filePath == "Default")
                {
                    _filePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, "ExperimentSetting.json");
                }
                StreamReader sr = new StreamReader(_filePath);
                string json_format_data = sr.ReadToEnd();

                Debug.Log("Read Experiment Setting Success from path: " + _filePath);
                _instance.format = JsonUtility.FromJson<ExperimentSetting>(json_format_data).format;
            }
            catch (Exception ex)
            {
                //if not find file , just create one
                WriteSetting("Default");
                Debug.Log("Exception Message: " + ex.Message);
            }

        }
    }
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

    public ExperimentSetting setting { get; }
    public Stimuli resouce { get; }
    public ExperimentRecord record { get; }

    public DataManager()
    {
        setting = ExperimentSetting.Instance;
        setting.ReadSetting("Default");

        resouce = Stimuli.Instance;
        record = ExperimentRecord.Instance;
        InitiateValue();
    }
    public void InitiateValue()
    {
        record.Initialize(setting.recordName, setting.trialNumber, setting.version);
        LoadResource();
    }
    public void SaveTrial(int index, Trial trial)
    {
        record.addTrialRecord(index, trial);
    }

    /// <summary>
    /// Save setting to Another Path
    /// </summary>
    public void SaveAsNewSetting(SettingFormat format)
    {
        SaveSettingChange(format);
        setting.WriteSetting("");
        LoadResource();
    }

    /// <summary>
    /// call from GKT_Experiment data event
    /// </summary>
    public void ReadDefaultSetting(){
        ReadDefault();
        LoadResource();
    }

    /// <summary>
    /// Read setting from custom path and load resource
    /// </summary>
    public void ReadSetting(){
       ReadSettingFrom();
       LoadResource();
    }
    public void ReadSettingFromInput(string path){
        setting.ReadSetting(path);
    }
    public void ReadSettingFrom()
    {
        setting.ReadSetting("");
    }

    /// <summary>
    /// Read unchangeable setting from /StreammingAssets/ExperimentSetting_Default
    /// </summary>
    public void ReadDefault()
    {
        setting.ReadSetting(Path.Combine(UnityEngine.Application.streamingAssetsPath, "ExperimentSetting_Default.json"));
    }

    /// <summary>
    /// Load Stimuli resource like visual target and mondrian video
    /// </summary>
    public void LoadResource()
    {
        resouce.ReadFile(setting.visualTargetPath, setting.mondrianVideoPath);
    }

    /// <summary>
    /// Create new record folder and save setting to it and set recordPath to new folder directories
    /// </summary>
    public void CreateNewExperiment(){
        setting.CreateRecordFolder();
        setting.WriteSetting(setting.format.recordPath);
    }
    
    /// <summary>
    /// save setting to streammingAssets/ ExperimentSetting
    /// </summary>
    /// <param name="format"></param>
    public void SaveSettingChange(SettingFormat format)
    {
        setting.ChangeSetting(format);
        setting.WriteSetting("Default");
    }
}
