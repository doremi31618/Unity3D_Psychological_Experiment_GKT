using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Video;

public partial class DataManager
{
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
        public float gapTime;

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

            gapTime = 3f;
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
        public float gapTime { get { return format.gapTime; } }

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
            // Debug.Log("record_path : " + format.recordPath);
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
}
