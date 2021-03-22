using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
using Valve.VR;
public class DeviceStatus : MonoBehaviour
{
    public RequestController requestController;
    public event DeviceEventHandler deviceEvent;
    [SerializeField]private bool vr_status;
    [SerializeField]private bool eyeTracker_status;
    public bool isVRConnect { get { return vr_status; } }
    public bool isEyeTrackerConnect { get { return eyeTracker_status; } }

    void Awake()
    {
        SteamVR_Events.Initialized.AddListener(OnViveConnect);
        requestController.OnConnected += OnPupilLabsConnected;
        requestController.OnDisconnecting += OnPupilLabsDisconected;
    }
    void Update()
    {
        if (!vr_status)
        {
            vr_status = OpenVR.System.GetTrackedDeviceActivityLevel(0) == EDeviceActivityLevel.k_EDeviceActivityLevel_Standby ||
                        OpenVR.System.GetTrackedDeviceActivityLevel(0) == EDeviceActivityLevel.k_EDeviceActivityLevel_UserInteraction||
                        OpenVR.System.GetTrackedDeviceActivityLevel(0) == EDeviceActivityLevel.k_EDeviceActivityLevel_Idle;
            DeviceStatusChange();
        }
    }

    
    void OnViveConnect(bool status)
    {
        vr_status = status;
        DeviceStatusChange();
    }

    void OnPupilLabsConnected()
    {
        eyeTracker_status = true;
        DeviceStatusChange();
    }
    void OnPupilLabsDisconected()
    {
        eyeTracker_status = false;
        DeviceStatusChange();
    }

    void DeviceStatusChange()
    {
        DeviceEventArgs device_event = new DeviceEventArgs(isVRConnect, isEyeTrackerConnect);
        deviceEvent(device_event);
    }
}
