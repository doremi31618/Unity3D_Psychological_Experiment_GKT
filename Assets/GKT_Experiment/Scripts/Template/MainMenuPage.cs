using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : Page
{
    [Header("Button")]
    public Button startButton;
    public Button settingButton;
    [Header("Text")]
    public Text version;
    public event DataEventHandler dataEvent;
    public event PageSwitchingHandler pageSwitch;
    private void Start() {

        version.text = "version : " + GKT_Experiment.version.ToString();

        startButton.onClick.AddListener(StartExperiment);
        settingButton.onClick.AddListener(ShowSettingPage);

    }

    public void enableStartButton(){
        
    }
    public void CreateNewExperiment()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.CreateNewExperiment;
        dataEvent(this, save_setting_args);
    }

    public void LoadResource()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.LoadResource;
        dataEvent(this, save_setting_args);
    }

    void StartExperiment(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.Experiment_start;
        pageSwitch(this, page_args);
        CreateNewExperiment();
        LoadResource();
        EndPage();
    }
    void ShowSettingPage(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.Setting;
        pageSwitch(this, page_args);
    }
}
