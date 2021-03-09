using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainExperimentPage : Page
{
    [Header("Experiment Relate")]
    public Text Title;
    public Image StrongEyeView;
    public Image WeakEyeView;
    public Scrollbar timeBar;
    [Header("Control Button")]
    public Button pause;
    public Button back;

    public event PageSwitchingHandler pageSwitch;

    void Start(){

    }

    public override void InitPage(int trialIndex)
    {
        base.InitPage();
        Title.text = "Trial " + trialIndex;
    }

    public override void UpdatePage(DataManager dataManager)
    {
        
    }
    /// <summary>
    /// input 0~1 value and change this by realtime
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTimebar(float time){
        timeBar.value = time;
    }

    public override void EndPage(){
        base.EndPage();
    }

    public void Back(){
        
    }

    public void SwitchTOMainPage(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.MainMenu;
        pageSwitch(this, page_args);
    }

    

}
