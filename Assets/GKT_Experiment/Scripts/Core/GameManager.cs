using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeviceStatus))]
public class GameManager : MonoBehaviour
{

    [Header("Page")]
    public Page settingPage;
    public Page previewPage;
    public Page mainMenuPage;
    public Page experimentPage;
    public Page deviceCheckingPage;

    GKT_Experiment gkt_experiment;
    DeviceStatus deviceStatus;

    // Start is called before the first frame update
    void Start()
    {
        if (gkt_experiment == null)
            gkt_experiment = GameObject.Find("GKT_Experiment").GetComponent<GKT_Experiment>();


        // init Page
        if (settingPage.gameObject.activeSelf)
            settingPage.gameObject.SetActive(false);

        if (!mainMenuPage.gameObject.activeSelf)
            mainMenuPage.gameObject.SetActive(true);

        if(!deviceCheckingPage.gameObject.activeSelf)
            deviceCheckingPage.gameObject.SetActive(true);
        
        deviceStatus = GetComponent<DeviceStatus>();
        InitializePage();
    }
    void InitializePage()
    {
        ((SettingPage)settingPage).dataEvent += new DataEventHandler(gkt_experiment.DataEvent);
        ((SettingPage)settingPage).dataEvent += new DataEventHandler(DataEvent);
        ((SettingPage)settingPage).pageSwitch += new PageSwitchingHandler(PageEvent);

        ((MainMenuPage)mainMenuPage).pageSwitch += new PageSwitchingHandler(PageEvent);
        ((MainMenuPage)mainMenuPage).dataEvent += new DataEventHandler(gkt_experiment.DataEvent);
        ((MainMenuPage)mainMenuPage).dataEvent += new DataEventHandler(DataEvent);
        
        deviceStatus.deviceEvent += new DeviceEventHandler(OnDeviceStatusChange);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDeviceStatusChange(DeviceEventArgs e){
       ((EyeTrackerCheckingPage)deviceCheckingPage).UpdateDeviceStatus(e.isVRConnected, e.isEyeTrackerConnected);
    }

    void PageEvent(object sender, PageEventArgs e)
    {
        switch (e.SwitchToPage)
        {
            case PageEventArgs.PageIndex.Setting:
                settingPage.InitPage();
                settingPage.UpdatePage(gkt_experiment.GetDataManager);
                break;

            case PageEventArgs.PageIndex.MainMenu:
                gkt_experiment.ResetExperiment();
                break;

            case PageEventArgs.PageIndex.Preview:
                previewPage.InitPage(gkt_experiment.GetDataManager);
                break;

            case PageEventArgs.PageIndex.Experiment_start:
                experimentPage.InitPage(gkt_experiment.trialIndex);
                gkt_experiment.StartExperiment();
                break;

            case PageEventArgs.PageIndex.Experiment_initTrial:
                experimentPage.InitPage(gkt_experiment.trialIndex);
                break;

            case PageEventArgs.PageIndex.Experiment_ending:
                experimentPage.EndPage();
                mainMenuPage.InitPage();
                settingPage.InitPage();
                break;

        }
    }

    public void DataEvent(object sender, DataEventArgs e)
    {
        switch (e.eventType)
        {
            case (DataEventArgs.EventType.SaveSetting):
                break;

            case (DataEventArgs.EventType.SaveAsSetting):
                break;

            case (DataEventArgs.EventType.ReadSetting):
                //read setting from custom directory
                settingPage.UpdatePage(gkt_experiment.GetDataManager);
                break;

            case (DataEventArgs.EventType.ReadDefaultSetting):
                //read setting form streammingAssets/ExperimentSetting_Default
                settingPage.UpdatePage(gkt_experiment.GetDataManager);
                break;

            case (DataEventArgs.EventType.SaveTrial):
                //save setting from streammingAssets/ExperimentSetting
                break;

            case (DataEventArgs.EventType.CreateNewExperiment):
                //create new folder and save Current setting to it

                break;
            case (DataEventArgs.EventType.LoadResource):
                //create new folder and save Current setting to it
                break;
        }
    }


}
