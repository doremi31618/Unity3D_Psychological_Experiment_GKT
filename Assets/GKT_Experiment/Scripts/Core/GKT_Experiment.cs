using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public delegate void PageSwitchingHandler(object sender, PageEventArgs page);
public delegate void DataEventHandler(object sender, DataEventArgs e);
public delegate void DeviceEventHandler(DeviceEventArgs e);
public class DeviceEventArgs
{
    public DeviceEventArgs(bool vr, bool eyeTracker)
    {
        isVRConnected = vr;
        isEyeTrackerConnected = eyeTracker;
    }
    public bool isVRConnected;
    public bool isEyeTrackerConnected;
}

public class PageEventArgs
{
    public enum PageIndex
    {
        Title,
        MainMenu,
        Setting,
        Preview,
        Experiment_start,
        Experiment_initTrial,
        Experiment_ending
    }
    public PageIndex SwitchToPage;

    public string info;
}
public class DataEventArgs
{
    public enum EventType
    {
        SaveSetting,
        SaveAsSetting,
        ReadSetting,
        ReadDefaultSetting,
        SaveTrial,
        CreateNewExperiment,
        LoadResource,

    }
    public string info;
    public EventType eventType;
    public DataManager.SettingFormat settingFormat;
    public DataManager.Trial trialFormat;

    public int trialIndex;

}


public class GKT_Experiment : MonoBehaviour
{
    public enum ExperimentStage
    {
        Calibration,
        DelayStart,
        Processing,
        DelayEnd,
        None
    }

    public ExperimentStage stage = ExperimentStage.None;
    public static float version = 1.1f;
    DataManager dataManager;
    public VisualTarget visualTarget;
    public EyeTracker eyeTracker;
    public Page experimentPage;

    #region Get Setting Data
    float gapTime { get { return dataManager.setting.gapTime; } }
    float maxTime { get { return dataManager.setting.maxTime; } }
    string recordPath { get { return dataManager.setting.recordPath; } }
    float maxAlpha { get { return dataManager.setting.maxAlpha; } }
    float delayTime { get { return dataManager.setting.delayTime; } }
    int trialNumber { get { return dataManager.setting.trialNumber; } }
    int mode { get { return dataManager.setting.mode; } }
    #endregion
    #region Get Stimuli Resource
    string videoUrl { get { return dataManager.resouce.getMondrianVideoPath; } }
    #endregion
    #region GKT Experiment Attribute
    int currentTrial = 0;
    public int trialIndex { get { return currentTrial; } }
    bool isSeeingVisualTarget = false;
    public DataManager.Trial trialRecord;
    public DataManager GetDataManager { get { return dataManager; } }
    public event PageSwitchingHandler pageSwitch;
    Coroutine experimentProcessing = null;

    #endregion
    void Start()
    {
        InitializeExperiment();
    }
    void Update()
    {
        if (stage == ExperimentStage.Processing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSeeingVisualTarget = true;
            }
        }
    }
    /// <summary>
    /// Processing when the whole program start 
    /// </summary>
    void InitializeExperiment()
    {
        dataManager = new DataManager();
        dataManager.ReadSettingFromInput("Default");
    }

    /// <summary>
    /// Processing when each trail start 
    /// </summary>
    /// <param name="index"></param>
    void InitTrial(int index)
    {
        PageEventArgs page_event_args = new PageEventArgs();
        page_event_args.SwitchToPage = PageEventArgs.PageIndex.Experiment_initTrial;
        pageSwitch(this, page_event_args);

        Sprite[] target_sprite = dataManager.resouce.getTrialImage(index);
        visualTarget.InitTrialSetting(target_sprite, videoUrl);
        trialRecord = new DataManager.Trial();
        trialRecord.index = currentTrial;

        // assign trial record
        trialRecord.top_left = target_sprite[0].name;
        trialRecord.top_right = target_sprite[1].name;
        trialRecord.bottom_right = target_sprite[2].name;
        trialRecord.bottom_left = target_sprite[3].name;

        trialRecord.finalAlpha = 0;
        trialRecord.finishTime = 0;

    }

    public void InitExperiment()
    {
        visualTarget.SetEyeCameraView(mode);
        eyeTracker.InitEyeCamera((GazeMode)mode);
        eyeTracker.SetEyeTrackerSavinglPath(dataManager.setting.recordPath);
    }
    public void StartExperiment()
    {
        //create a new record folder

        dataManager.CreateNewExperiment();
        InitExperiment();
        experimentProcessing = StartCoroutine(TrialProcess());

    }
    public void StopExperiment()
    {
        StopCoroutine(experimentProcessing);
        PageEventArgs _page_event_args = new PageEventArgs();
        _page_event_args.SwitchToPage = PageEventArgs.PageIndex.Experiment_ending;
        pageSwitch(this, _page_event_args);
    }

    void OnExperimentEnd()
    {
        Debug.Log("save record : " + dataManager.record.ToString());

        PageEventArgs _page_event_args = new PageEventArgs();
        _page_event_args.SwitchToPage = PageEventArgs.PageIndex.Experiment_ending;
        pageSwitch(this, _page_event_args);

        eyeTracker.ResetCalibrationRunTime();
        visualTarget.InitBeforeTrialStart();
        dataManager.record.SaveAllRecord(Path.Combine(recordPath, "record.json"));
    }

    IEnumerator TrialProcess()
    {
        while (currentTrial < trialNumber)
        {
            //process 
            eyeTracker.StartRecording();

            // start - press button

            InitTrial(currentTrial);
            stage = ExperimentStage.Calibration;
            if (eyeTracker.IsUsePupilCalibration)
            {
                eyeTracker.StartCalibration();

                while (!eyeTracker.isCalibrationDone)
                {
                    yield return null;
                }
            }


            //start data optimze calibration
            eyeTracker.StartCustomCalibration();
            while (!eyeTracker.isCalibrationDone)
            {
                yield return null;
            }

            visualTarget.InitBeforeTrialStart();
            float gapCounter = Time.time + gapTime;
            stage = ExperimentStage.DelayStart;

            float experimentCounter = gapCounter + maxTime;

            //normalize time value 0~1 value
            float normalize_time = (1 - ((experimentCounter - Time.time) / maxTime));
            while (!isSeeingVisualTarget)
            {
                //wait for gap time
                if (Time.time <= gapCounter)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                //start fadin process  
                if (stage == ExperimentStage.DelayStart)
                    stage = ExperimentStage.Processing;

                if (Time.time >= experimentCounter) break;

                normalize_time = (1 - ((experimentCounter - Time.time) / maxTime));
                visualTarget.SetImageAlpha(maxAlpha * normalize_time);
                ((MainExperimentPage)experimentPage).UpdateTimebar(normalize_time, normalize_time * maxTime);
                yield return new WaitForEndOfFrame();
            }

            //Trial End : save current trial record
            normalize_time = (1 - ((experimentCounter - Time.time) / maxTime));
            trialRecord.finishTime = normalize_time * maxTime;
            trialRecord.finalAlpha = maxAlpha * normalize_time;
            isSeeingVisualTarget = false;
            dataManager.record.addTrialRecord(currentTrial, trialRecord);
            eyeTracker.PressButton();
            stage = ExperimentStage.DelayEnd;

            //Delay Time : wait for next trial
            yield return new WaitForSeconds(delayTime);
            eyeTracker.StopRecording();
            currentTrial++;
        }

        stage = ExperimentStage.None;
        OnExperimentEnd();


    }

    public void DataEvent(object sender, DataEventArgs e)
    {
        switch (e.eventType)
        {
            case (DataEventArgs.EventType.SaveSetting):
                dataManager.SaveSettingChange(e.settingFormat);
                break;

            case (DataEventArgs.EventType.SaveAsSetting):
                dataManager.SaveAsNewSetting(e.settingFormat);
                break;

            case (DataEventArgs.EventType.ReadSetting):
                //read setting from custom directory
                dataManager.ReadSetting();
                break;

            case (DataEventArgs.EventType.ReadDefaultSetting):
                //read setting form streammingAssets/ExperimentSetting_Default
                dataManager.ReadDefaultSetting();
                break;

            case (DataEventArgs.EventType.SaveTrial):
                //save setting from streammingAssets/ExperimentSetting
                dataManager.SaveTrial(e.trialIndex, e.trialFormat);
                break;

            case (DataEventArgs.EventType.CreateNewExperiment):
                //create new folder and save Current setting to it

                break;
            case (DataEventArgs.EventType.LoadResource):
                //create new folder and save Current setting to it
                dataManager.LoadResource();
                break;
        }
    }
    /// <summary>
    /// Debug use 
    /// </summary>
    private void OnGUI()
    {
        // string info = JsonUtility.ToJson(dataManager.setting);
        // Rect SettingInfo = new Rect(0, 0, 1920, 1080);
        // GUI.Label(SettingInfo, info);
    }

}
