using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
using System.IO;
public class CustomRecordingController : RecordingController
{
    [System.Serializable]
    struct GazeDataFormat
    {
        public GKT_Experiment.ExperimentStage stage;
        public GazeMode mode;
        public float confidence;
        public double pupilTimeStamp;
        public Vector3 gazePoint3d;
        public Vector3 standardCalibrationPoint;
        // public Vector3 eyeCenter0, eyeCenter1;
        // public Vector3 gazeNormal0, gazeNormal1;
        
    }
    public EyeTracker eyeTracker;
    [HideInInspector]public GazeMode mode;
    public SubscriptionsController subscriptionsController;
    public GazeVisualizer gazeVisualizer;
    public GKT_Experiment gkt_experiment_data;
    public GazeController gazeController;
    [SerializeField] List<GazeDataFormat> gazeDataCollection;
    protected override void CustomOnEnable()
    {

        gazeDataCollection = new List<GazeDataFormat>();
    }
    public void SetGazeMode(GazeMode _gazeMode){
        mode = _gazeMode;
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

        new_GazeData.mode = mode;
        new_GazeData.stage = gkt_experiment_data.stage;
        new_GazeData.confidence = gazeData.Confidence;
        new_GazeData.standardCalibrationPoint = eyeTracker.getCurrentCalibrationPoint;
        new_GazeData.gazePoint3d = gazeVisualizer.gazePoint3d;
        new_GazeData.pupilTimeStamp = gazeData.PupilTimestamp;

        // new_GazeData.eyeCenter0 = gazeData.EyeCenter0;
        // new_GazeData.eyeCenter1 = gazeData.EyeCenter1;
        
        // new_GazeData.gazeNormal0 = gazeData.GazeNormal0;
        // new_GazeData.gazeNormal1 = gazeData.GazeNormal1;

        gazeDataCollection.Add(new_GazeData);
    }

    public void ExtractGazeData()
    {
        string csv_content = "mode,stage,Confidence,gazePoint3d,standardCalibrationPoint,PupilTimestamp\n";
        string path = Path.Combine(GetRecordingPath(), "Trial" + gkt_experiment_data.trialIndex + "_GazePositions.csv");
        StreamWriter sw = new StreamWriter(path);
        foreach (var gaze in gazeDataCollection)
        {
            csv_content += string.Format("{0},{1},{2},{3},{4},{5}\n",
                 gaze.mode,
                 gaze.stage,
                 gaze.confidence,
                 gaze.gazePoint3d.x+" "+gaze.gazePoint3d.y+" "+gaze.gazePoint3d.z,
                 gaze.standardCalibrationPoint.x+" "+gaze.standardCalibrationPoint.y+" "+gaze.standardCalibrationPoint.z,
                 gaze.pupilTimeStamp
            );
        }
        sw.Write(csv_content);
        sw.Close();
        Debug.Log("Extract GazeData to "+path);
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
