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
    
    public event PageSwitchingHandler pageSwitch;
    private void Start() {

        version.text = GKT_Experiment.version.ToString();

        startButton.onClick.AddListener(StartExperiment);
        settingButton.onClick.AddListener(ShowSettingPage);

    }
    void StartExperiment(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.Experiment;
        pageSwitch(this, page_args);
        EndPage();
    }
    void ShowSettingPage(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.Setting;
        pageSwitch(this, page_args);
    }
}
