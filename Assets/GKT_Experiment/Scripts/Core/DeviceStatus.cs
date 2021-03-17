using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
using Valve.VR;
public class DeviceStatus : MonoBehaviour
{
    public RequestController requestController;
    public event DeviceEventHandler deviceEvent;
    private bool vr_status;
    private bool eyeTracker_status;
    public bool isVRConnect{get {return vr_status;}}
    public bool isEyeTrackerConnect{get {return eyeTracker_status;}}

    void Start(){
        SteamVR_Events.DeviceConnected.AddListener(OnViveConnect);
        requestController.OnConnected += OnPupilLabsConnected;
        requestController.OnDisconnecting += OnPupilLabsDisconected;
    }
    void OnViveConnect(int index, bool status){
        if (index == 0){
            vr_status =status;
        }
        DeviceStatusChange();
    }

    void OnPupilLabsConnected(){
        eyeTracker_status = true;
        DeviceStatusChange();
    }
    void OnPupilLabsDisconected(){
        eyeTracker_status = false;
        DeviceStatusChange();
    }

    void DeviceStatusChange(){
        DeviceEventArgs device_event = new DeviceEventArgs(vr_status, eyeTracker_status);
        deviceEvent(device_event);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
