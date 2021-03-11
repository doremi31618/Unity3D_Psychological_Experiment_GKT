﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
using System.IO;
public class CustomRecordingController : RecordingController
{
    [System.Serializable]
    struct GazeDataFormat
    {
        public float confidence;
        public double pupilTimeStamp;
        public Vector3 gazePoint3d;
        public Vector3 eyeCenter0, eyeCenter1;
        public Vector3 gazeNormal0, gazeNormal1;
    }

    public SubscriptionsController subscriptionsController;
    public GazeVisualizer gazeVisualizer;
    public GKT_Experiment gkt_experiment_data;
    public GazeController gazeController;
    [SerializeField] List<GazeDataFormat> gazeDataCollection;
    protected override void CustomOnEnable()
    {

        gazeDataCollection = new List<GazeDataFormat>();
    }
    // Start is called before the first frame update
    public override void StartRecording()
    {
        if (!enabled)
        {
            Debug.LogWarning("Component not enabled");
            return;
        }

        if (!requestCtrl.IsConnected)
        {
            Debug.LogWarning("Not connected");
            return;
        }

        if (IsRecording)
        {
            Debug.Log("Recording is already running.");
            return;
        }


        var path = GetRecordingPath();

        requestCtrl.Send(new Dictionary<string, object>
            {
                { "subject","recording.should_start" }
                , { "session_name", path }
                , { "record_eye",recordEyeFrames}
            });

        IsRecording = true;

        //abort process on disconnecting
        requestCtrl.OnDisconnecting += StopRecording;

        gazeDataCollection.Clear();
        gazeController.OnReceive3dGaze += RecordGaze;

    }

    public void RecordGaze(GazeData gazeData)
    {
        GazeDataFormat new_GazeData = new GazeDataFormat();
        new_GazeData.eyeCenter0 = gazeData.EyeCenter0;
        new_GazeData.eyeCenter1 = gazeData.EyeCenter1;
        new_GazeData.gazePoint3d = gazeVisualizer.gazePoint3d;
        new_GazeData.gazeNormal0 = gazeData.GazeNormal0;
        new_GazeData.gazeNormal1 = gazeData.GazeNormal1;
        new_GazeData.confidence = gazeData.Confidence;
        new_GazeData.pupilTimeStamp = gazeData.PupilTimestamp;

        gazeDataCollection.Add(new_GazeData);
    }

    public void ExtractGazeData()
    {
        string csv_content = "eyeCenter0_x,eyeCenter0_y,eyeCenter0_z,eyeCenter1_x,eyeCenter1_y,eyeCenter1_z,gazePoint3d_x,gazePoint3d_y,gazePoint3d_z,gazeNormal0_x,gazeNormal0_y,gazeNormal0_z,gazeNormal1_x,gazeNormal1_y,gazeNormal1_z,confidence,pupilTimeStamp\n";
        StreamWriter sw = new StreamWriter(Path.Combine(GetRecordingPath(), "Trial" + gkt_experiment_data.trialIndex + "_GazePositions.csv"));
        foreach (var gaze in gazeDataCollection)
        {
            csv_content += string.Format("{0},{1},{2},{3},{4},{5},{6}\n",
                 gaze.eyeCenter0,
                 gaze.eyeCenter1,
                 gaze.gazePoint3d,
                 gaze.gazeNormal0,
                 gaze.gazeNormal1,
                 gaze.confidence,
                 gaze.pupilTimeStamp
            );
        }
        sw.Write(csv_content);
        sw.Close();
    }

    public override void StopRecording()
    {
        if (!IsRecording)
        {
            Debug.Log("Recording is not running, nothing to stop.");
            return;
        }

        requestCtrl.Send(new Dictionary<string, object>
            {
                { "subject", "recording.should_stop" }
            });

        IsRecording = false;
        gazeController.OnReceive3dGaze -= RecordGaze;
        requestCtrl.OnDisconnecting -= StopRecording;
        ExtractGazeData();
    }
}
