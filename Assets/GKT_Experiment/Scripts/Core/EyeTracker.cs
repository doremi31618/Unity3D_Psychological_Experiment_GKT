using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PupilLabs;
public enum GazeMode
{
    Right,
    Left,
    Both
}


public class EyeTracker : MonoBehaviour
{
    public int pupilCalibrationRunTime = 0;
    public bool IsUsePupilCalibration{get{return pupilCalibrationRunTime < 1;}}
    public GazeMode mode;
    [Header("Pupil lab scripts")]
    public RequestController requestController;
    public CustomRecordingController recordingController;
    public FrameVisualizer frameVisualizer;
    public CustomGazeVisaulizer gazeVisualizer;
    public CustomCalibrationController calibrationController;
    public OptimizeCalibrationController customCalibrationController;

    // [Header("Recording Data")]
    public string savingPath
    {
        get
        {
            return recordingController.GetRecordingPath();
        }
        set
        {
            recordingController.SetCustomPath(value);
        }
    }

    [Header("Annotations")]
    public AnnotationPublisher annotationPub;
    public Transform eyePosition;

    [Header("Render Eye Frame Visualizer")]
    [Tooltip("below Eye Tracker's canvas ")]
    public RawImage[] eyeFrameVisaulizer;
    public Texture2D[] getEyeTexture
    {
        get
        {
            return frameVisualizer.getEyeTexture;
        }
    }

    bool isConnected
    {
        get
        {
            return requestController.IsConnected;
        }
    }

    bool isCalibrationRoutineDone = false;
    public bool isCalibrationDone
    {
        get { return isCalibrationRoutineDone; }
    }
    int calibrationIndex { get { return customCalibrationController.getTargetIndex; } }
    CalibrationTargets targets { get { return customCalibrationController.targets; } }
    public Vector3 getCurrentCalibrationPoint { get { return (IsUsePupilCalibration)?Vector3.zero:targets.GetLocalTargetPosAt(calibrationIndex); } }

    void Awake()
    {
        Setup();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// assigning attribute
    /// </summary>
    public void Setup()
    {
        recordingController.useCustomPath = true;
        frameVisualizer.InitEyeTexture += new InitTextureHandler(UpdateEyeTexture);

        calibrationController.OnCalibrationRoutineDone += OnCalibrationRoutineDone;
    }

    /// <summary>
    /// assign setting value to pupil labs
    /// </summary>
    public void InitEyeCamera(GazeMode _mode)
    {

        // calibrationController.camera = cam;
        // gazeVisualizer.gazeOrigin = cam.transform;
        mode = _mode;
        //update gaze mode
        recordingController.SetGazeMode(mode);
        gazeVisualizer.SetGazeMode(mode);

    }

    /// <summary>
    /// manage Eye texture
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void UpdateEyeTexture(object sender, PupilLabsEventArgs e)
    {
        Debug.Log("Update Eye Texture (eye index) : " + e.eyeIndex);
        Texture2D eyeTexture = frameVisualizer.getEyeTexture[e.eyeIndex];
        eyeFrameVisaulizer[e.eyeIndex].texture = eyeTexture;
    }

    /// <summary>
    /// Start pupil calibration
    /// </summary>
    public void StartCalibration()
    {

        calibrationController.StartCalibration();
        pupilCalibrationRunTime += 1;
        isCalibrationRoutineDone = false;
    }

    /// <summary>
    /// start data optimize calibration
    /// </summary>
    public void StartCustomCalibration(){
        customCalibrationController.StartCalibration();
        pupilCalibrationRunTime += 1;
        isCalibrationRoutineDone = false;
    }
    void OnCalibrationRoutineDone()
    {
        isCalibrationRoutineDone = true;
    }

    /// <summary>
    /// manage recording 
    /// </summary>
    public void StartRecording()
    {
        recordingController.StartRecording();
        SendTimeStamp("start record", new Dictionary<string, object>());
    }

    /// <summary>
    /// process when subject see the target and press button
    /// </summary>
    public void PressButton()
    {
        SendTimeStamp("press button", new Dictionary<string, object>());
    }
    void SendTimeStamp(string labelName, Dictionary<string, object> userInfo)
    {
        annotationPub.SendAnnotation(label: labelName, customData: userInfo);
    }

    /// <summary>
    /// stop recording 
    /// </summary>
    public void StopRecording()
    {
        recordingController.StopRecording();
        SendTimeStamp("stop record", new Dictionary<string, object>());
    }

    public void SetEyeTrackerSavinglPath(string path)
    {
        recordingController.useCustomPath = true;
        recordingController.SetCustomPath(path);
    }

    public void ResetCalibrationRunTime(){
        pupilCalibrationRunTime = 0;
    }

    

}
