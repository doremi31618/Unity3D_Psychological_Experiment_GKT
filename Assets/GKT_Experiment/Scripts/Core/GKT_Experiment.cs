using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public delegate void PageSwitchingHandler(object sender, PageEventArgs page);
public delegate void DataEventHandler(object sender, DataEventArgs e);

public class PageEventArgs
{
    public enum PageIndex
    {
        Title,
        MainMenu,
        Setting,
        Preview,
        Experiment
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
    public static float version = 1.0f;
    DataManager dataManager;
    public VisualTarget visualTarget;
    public EyeTracker eyeTracker;
    [Header("Page")]
    public Page settingPage;
    public Page previewPage;
    public Page mainMenuPage;

    public Page experimentPage;
    #region Get Setting Data
    float gapTime { get { return dataManager.setting.gapTime; } }
    float maxTime { get { return dataManager.setting.maxTime; } }
    string recordPath{ get { return dataManager.setting.recordPath; } }
    float maxAlpha  { get { return dataManager.setting.maxAlpha; } }
    float delayTime { get { return dataManager.setting.delayTime; } }
    int trialNumber { get { return dataManager.setting.trialNumber; } }
    int mode { get { return dataManager.setting.mode; } }
    #endregion
    #region Get Stimuli Resource
    string videoUrl { get { return dataManager.resouce.getMondrianVideoPath; } }
    #endregion
    #region GKT Experiment Attribute
    int currentTrial = 0;
    public int trialIndex {get{return currentTrial;}}
    bool isSeeingVisualTarget = false;
    public DataManager.Trial trialRecord;

    #endregion
    void Start()
    {
        InitializeExperiment();
    }
    /// <summary>
    /// Processing when the whole program start 
    /// </summary>
    void InitializeExperiment()
    {
        // internal value
        ((SettingPage)settingPage).dataEvent += new DataEventHandler(DataEvent);
        ((SettingPage)settingPage).pageSwitch += new PageSwitchingHandler(PageEvent);
        ((MainMenuPage)mainMenuPage).pageSwitch += new PageSwitchingHandler(PageEvent);
        ((MainMenuPage)mainMenuPage).dataEvent += new DataEventHandler(DataEvent);
        
        dataManager = new DataManager();
        dataManager.ReadSettingFromInput("Default");
    }

    /// <summary>
    /// Processing when each trail start 
    /// </summary>
    /// <param name="index"></param>
    void InitTrial(int index)
    {
        experimentPage.InitPage(currentTrial);
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

    void StartExperiment(){
        //create a new record folder
        dataManager.CreateNewExperiment();
        visualTarget.SetEyeCameraView(mode);
        eyeTracker.SetEyeTrackerSavinglPath(dataManager.setting.recordPath);
        Debug.Log("record path : " + dataManager.setting.recordPath);
        StartCoroutine(TrialProcess());
    }
    void EndExperiment()
    {
        Debug.Log("save record : " + dataManager.record.ToString());
        experimentPage.EndPage();
        mainMenuPage.InitPage();
        settingPage.InitPage();
        visualTarget.InitBeforeTrialStart();
        dataManager.record.SaveAllRecord(Path.Combine(recordPath, "record.json"));
    }

    IEnumerator TrialProcess()
    {
        while (currentTrial < trialNumber)
        {

            // start - press button
            float time = 0;
            InitTrial(currentTrial);
            eyeTracker.StartCalibration();
            while(!eyeTracker.isCalibrationDone){
                yield return null;
            }

            //process 
            eyeTracker.StartRecording();
            visualTarget.InitBeforeTrialStart();
            float gapCounter = gapTime;
            Debug.Log("Time : " + time+" maxTime : "+ maxTime);
            while (!isSeeingVisualTarget)
            {
                //wait for gap time
                if (gapCounter > 0){
                    gapCounter -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                //start fadin process  
                if (time >= maxTime) break;
                else time += Time.deltaTime;
                

                visualTarget.SetImageAlpha(maxAlpha * (time / maxTime));
                ((MainExperimentPage)experimentPage).UpdateTimebar((time / maxTime));
                yield return new WaitForEndOfFrame();
            }

            //Trial End : save current trial record
            trialRecord.finishTime = time;
            trialRecord.finalAlpha = maxAlpha * (time / maxTime);
            dataManager.record.addTrialRecord(currentTrial, trialRecord);
            eyeTracker.PressButton();

            //Delay Time : wait for next trial
            yield return new WaitForSeconds(delayTime);
            eyeTracker.StopRecording();
            currentTrial++;
        }

        EndExperiment();


    }

    void PageEvent(object sender, PageEventArgs e)
    {
        switch (e.SwitchToPage)
        {
            case PageEventArgs.PageIndex.Setting:
                settingPage.InitPage();
                settingPage.UpdatePage(dataManager);
                break;
            case PageEventArgs.PageIndex.MainMenu:
                visualTarget.SetEyeCameraView(mode);
                break;
            case PageEventArgs.PageIndex.Preview:
                previewPage.InitPage(dataManager);
                break;
            case PageEventArgs.PageIndex.Experiment:
                experimentPage.InitPage(currentTrial);
                StartExperiment();
                break;
        }
    }

    void DataEvent(object sender, DataEventArgs e)
    {
        switch (e.eventType)
        {
            case (DataEventArgs.EventType.SaveSetting):
                dataManager.SaveSettingChange(e.settingFormat);
                dataManager.LoadResource();
                break;
            case (DataEventArgs.EventType.SaveAsSetting):
                dataManager.SaveAsNewSetting(e.settingFormat);
                break;
            case (DataEventArgs.EventType.ReadSetting):
                //read setting from custom directory
                dataManager.ReadSetting();
                settingPage.UpdatePage(dataManager);
                break;
            case (DataEventArgs.EventType.ReadDefaultSetting):
                //read setting form streammingAssets/ExperimentSetting_Default
                dataManager.ReadDefaultSetting();
                settingPage.UpdatePage(dataManager);
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
        string info = JsonUtility.ToJson(dataManager.setting);
        Rect SettingInfo = new Rect(0, 0, 1920, 1080);
        GUI.Label(SettingInfo, info);
    }

}
