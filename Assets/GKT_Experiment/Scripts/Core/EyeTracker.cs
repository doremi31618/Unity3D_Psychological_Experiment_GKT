using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
public class EyeTracker : MonoBehaviour
{
    public RecordingController recordingController;
    [Header("Recording Data")]
    public string savingPath;

    [Header("Annotations")]
    public AnnotationPublisher annotationPub;
    public Transform eyePosition;

    void Start()
    {

    }

    void Setup()
    {
        recordingController.useCustomPath = true;
    }
    public void SendTimeStamp()
    {
        Dictionary<string, object> userInfo = new Dictionary<string, object>();

        // userInfo["head_world_x"] = head.position.x;
        // userInfo["head_world_y"] = head.position.y;
        // userInfo["head_world_z"] = head.position.z;

        annotationPub.SendAnnotation(label: "Notice Target", customData: userInfo);
    }
    public void SavingTrialData(int index, string path)
    {
        recordingController.SetCustomPath(path);

    }

}
