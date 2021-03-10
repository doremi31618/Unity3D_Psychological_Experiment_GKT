using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PupilLabs;



public class EyeTracker : MonoBehaviour
{
    [Header("Pupil lab scripts")]
    public RequestController requestController;
    public RecordingController recordingController;
    public FrameVisualizer frameVisualizer;
    public GazeVisualizer gazeVisualizer;
    public CalibrationController calibrationController;
    // [Header("Recording Data")]
    public string savingPath{
        get{
            return recordingController.GetRecordingPath();
        }
        set{
            recordingController.SetCustomPath(value);
        }
    }

    [Header("Annotations")]
    public AnnotationPublisher annotationPub;
    public Transform eyePosition;

    [Header("Render Eye Frame Visualizer")]
    [Tooltip("below Eye Tracker's canvas ")]
    public RawImage[] eyeFrameVisaulizer;
    public Texture2D[] getEyeTexture{
        get{
           return frameVisualizer.getEyeTexture;
        }
    }

    bool isConnected {
        get{
            return requestController.IsConnected;
        }
    }

    bool isCalibrationRoutineDone = false;
    public bool isCalibrationDone {
        get{return isCalibrationRoutineDone;}
    }

    void Awake(){
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
    void Setup()
    {
        recordingController.useCustomPath = true;
        frameVisualizer.InitEyeTexture += new InitTextureHandler(UpdateEyeTexture);

        calibrationController.OnCalibrationRoutineDone += CalibrationStatus;
    }

    /// <summary>
    /// assign setting value to pupil labs
    /// </summary>
    void InitEyeCamera(Camera cam){

        calibrationController.camera = cam;
        gazeVisualizer.gazeOrigin = cam.transform;

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
    /// Start calibration
    /// </summary>
    public void StartCalibration(){
        
        calibrationController.StartCalibration();
        isCalibrationRoutineDone = false;
    }
    void CalibrationStatus(){
        isCalibrationRoutineDone = true;
    }

    /// <summary>
    /// manage recording 
    /// </summary>
    public void StartRecording(){
        recordingController.StartRecording();
        SendTimeStamp("start record", new Dictionary<string, object>());
    }

    /// <summary>
    /// process when subject see the target and press button
    /// </summary>
    public void PressButton(){
        SendTimeStamp("press button", new Dictionary<string, object>());
    }
    void SendTimeStamp(string labelName, Dictionary<string, object> userInfo)
    {
        annotationPub.SendAnnotation(label: labelName, customData: userInfo);
    }

    /// <summary>
    /// stop recording 
    /// </summary>
    public void StopRecording(){
        recordingController.StopRecording();
        SendTimeStamp("stop record", new Dictionary<string, object>());
    }
    
    public void SetEyeTrackerSavinglPath(string path)
    {
        recordingController.useCustomPath = true;
        recordingController.SetCustomPath(path);
    }

}
