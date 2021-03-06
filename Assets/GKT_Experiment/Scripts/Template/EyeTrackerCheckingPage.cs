﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTrackerCheckingPage : Page
{
    public Text vr_status;
    public Text eyeTracker_status;
    public Button exit;
    // public event PageSwitchingHandler pageEvent;


    void Start()
    {
        exit.interactable = false;
        exit.onClick.AddListener(CloseCheckingWindow);
        vr_status.text = "VR status : not connect";
        eyeTracker_status.text = "Eye Tracker status : not connected";
        
    }

    private void OnEnable()
    {
       
    }
    void CloseCheckingWindow(){
        this.gameObject.SetActive(false);
    }
    
    public void UpdateDeviceStatus(bool vr_connect, bool eyeTracker_connect)
    {
        if (vr_connect)vr_status.text = "VR status : connect";
        else vr_status.text = "VR status : not connect";

        if (eyeTracker_connect) eyeTracker_status.text = "Eye Tracker status : connect";
        else eyeTracker_status.text = "Eye Tracker status : not connected";

        if (vr_connect && eyeTracker_connect)
            exit.interactable = true;
        else
            exit.interactable = false;
        
    }


}
