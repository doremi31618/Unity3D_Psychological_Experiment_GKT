using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;
using PupilLabs;

public partial class DataManager
{
    
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
    /// <summary>
    /// Read setting from custom path
    /// </summary>
    /// <param name="path"></param>
    public void ReadSettingFromInput(string path){
        setting.ReadSetting(path);
    }

    /// <summary>
    /// Read setting through windows dialog
    /// </summary>
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

    public void StartRecordingGazeData(GazeData gazeData){

    }

    public void StopRecordingGazeData(){
        
    }
}
