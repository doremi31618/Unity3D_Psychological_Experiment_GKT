using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public partial class DataManager
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
}
