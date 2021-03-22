using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
public class SettingPage : Page
{
    [System.Serializable]
    public class PathGUIObject
    {
        public InputField path;
        public UnityEngine.UI.Button folderDialog;

        public void Initialize()
        {
            folderDialog.onClick.AddListener(OpenFolderDialog);
        }

        public void OpenFolderDialog()
        {
            FolderBrowserDialog _path = new FolderBrowserDialog();
            _path.ShowDialog();
            path.text = _path.SelectedPath;
        }
    }
    [Header("Trial")]
    public InputField delayTime;
    public InputField trialNumber;
    public InputField maxTime;
    public InputField gapTime;
    public UnityEngine.UI.Scrollbar maxAlpha;
    public Dropdown mode;

    [Header("Path")]
    public PathGUIObject mondrian_video_path;
    public PathGUIObject visual_target_path;
    public PathGUIObject record_path;
    public InputField record_name;

    [Header("Control Button")]
    public UnityEngine.UI.Button preview;
    public UnityEngine.UI.Button back;
    public UnityEngine.UI.Button save;
    public UnityEngine.UI.Button saveAs;
    public UnityEngine.UI.Button read;
    public UnityEngine.UI.Button readDefault;

    [Header("Preview")]
    public GameObject eyePreview;
    public Text alphaValue;
    public Image alphaPreview;

    //EventHandler 
    public event DataEventHandler dataEvent;
    
    public event PageSwitchingHandler pageSwitch;
    private void Start()
    {
        //Initialize PathGUIObject
        mondrian_video_path.Initialize();
        visual_target_path.Initialize();
        record_path.Initialize();

        //Initiate button event
        preview.onClick.AddListener(PreviewVisaulTarget);
        back.onClick.AddListener(BackToMainMenu);
        save.onClick.AddListener(SaveSetting);
        saveAs.onClick.AddListener(SaveAsSetting);
        read.onClick.AddListener(ReadSetting);
        readDefault.onClick.AddListener(ReadDefaultSetting);

        maxAlpha.onValueChanged.AddListener(UpdateAlphaPreview);
        mode.onValueChanged.AddListener(UpdateModePreviewImage);
    }
    public void UpdateAlphaPreview(float alpha){
        alphaValue.text = alpha.ToString("0.00");
        Color img_color = alphaPreview.color;
        alphaPreview.color = new Vector4(img_color.r, img_color.g, img_color.b, alpha);
    }

    public void UpdateModePreviewImage(int value){
        if(value==0)
            eyePreview.transform.localScale = new Vector3(1, 1, 1);
        else
            eyePreview.transform.localScale = new Vector3(-1, 1, 1);
        
    }

    public override void InitPage()
    {
        base.InitPage();
    }

    //only process at the gkt experiment class 
    public override void UpdatePage(DataManager dataManager)
    {
        DataManager.ExperimentSetting setting = dataManager.setting;

        //trial
        delayTime.text = setting.delayTime.ToString();
        trialNumber.text = setting.trialNumber.ToString();
        maxTime.text = setting.maxTime.ToString();
        gapTime.text = setting.gapTime.ToString();
        maxAlpha.value = setting.maxAlpha;
        mode.value = setting.mode;

        //path
        record_name.text = setting.recordName;
        mondrian_video_path.path.text = setting.mondrianVideoPath;
        visual_target_path.path.text = setting.visualTargetPath;
        record_path.path.text = setting.recordFolderPath;

        UpdateAlphaPreview(setting.maxAlpha);
        UpdateModePreviewImage(setting.mode);
    }

    

    DataManager.SettingFormat UIToFormat()
    {
        DataManager.SettingFormat format = new DataManager.SettingFormat("");

        //trial
        format.delayTime = float.Parse(delayTime.text);
        format.trialNumber = int.Parse(trialNumber.text);
        format.maxTime = float.Parse(maxTime.text);
        format.gapTime = float.Parse(gapTime.text);
        format.maxAlpha = maxAlpha.value;
        format.gapTime = 3f;
        format.mode = mode.value;

        //path 
        format.recordName = record_name.text;
        format.mondrianVideoPath = mondrian_video_path.path.text;
        format.visualTargetPath = visual_target_path.path.text;
        format.recordFolderPath = record_path.path.text;
        format.recordPath = Path.Combine(format.recordFolderPath, format.recordName);
        return format;
    }

    //call gkt experiment class 
    public void SaveSetting()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.SaveSetting;
        save_setting_args.settingFormat = UIToFormat();
        dataEvent(this, save_setting_args);
    }

    public void SaveAsSetting()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.SaveAsSetting;
        save_setting_args.settingFormat = UIToFormat();
        dataEvent(this, save_setting_args);
    }

    public void ReadSetting()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.ReadSetting;
        dataEvent(this, save_setting_args);
    }

    public void ReadDefaultSetting()
    {
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.ReadDefaultSetting;
        dataEvent(this, save_setting_args);
    }

    public void PreviewVisaulTarget(){
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.Preview;
        page_args.info = visual_target_path.path.text;
        LoadResource();
        pageSwitch(this, page_args);
    }

    public void LoadResource(){
        DataEventArgs save_setting_args = new DataEventArgs();
        save_setting_args.eventType = DataEventArgs.EventType.LoadResource;
        dataEvent(this, save_setting_args);
    }

    public void BackToMainMenu(){
        this.gameObject.SetActive(false);
        PageEventArgs page_args = new PageEventArgs();
        page_args.SwitchToPage = PageEventArgs.PageIndex.MainMenu;
        // SaveSetting();
        pageSwitch(this, page_args);
    }

}
