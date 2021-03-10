using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class VisualTarget : MonoBehaviour
{
    [Header("Camera Setting")]
    public GameObject RightEyeCamera;
    public GameObject LeftEyeCamera;
    public LayerMask WeakEyeEyeLayer;
    public LayerMask StrongEyeLayer;

    [Header("Strong eye setting")]
    public VideoPlayer strongEye_view;
    RawImage strongEye_view_image;
    // the order depends on coounter clockwised
    // index 0 : top-left
    // index 1 : top-right
    // index 2 : bottom-Right
    // index 3 : bottom-Left
    [Header("Weak eye setting")] public Image[] weakEye_view;

    GameObject strongEye_view_parent;
    GameObject weakEye_view_parent;
    //mode 0 : left eye is weakeye
    //mode 1 : right eye is weakeye

    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {

    }

    //Initialize will process at start 
    public void Initialize()
    {
        if (weakEye_view_parent == null)
            weakEye_view_parent = transform.Find("WeakEye").gameObject;
        if (strongEye_view_parent == null)
            strongEye_view_parent = transform.Find("StrongEye").gameObject;

        if (RightEyeCamera == null)
            RightEyeCamera = GameObject.Find("RightEye");
        ChangeCameraView(RightEyeCamera.transform, StrongEyeLayer);


        if (LeftEyeCamera == null)
            LeftEyeCamera = GameObject.Find("LeftEye");
        ChangeCameraView(LeftEyeCamera.transform, WeakEyeEyeLayer);

        if (weakEye_view.Length != 4)
        {
            weakEye_view[0] = weakEye_view_parent.transform.Find("top-left").GetComponent<Image>();
            weakEye_view[1] = weakEye_view_parent.transform.Find("top-right").GetComponent<Image>();
            weakEye_view[2] = weakEye_view_parent.transform.Find("bottom-right").GetComponent<Image>();
            weakEye_view[3] = weakEye_view_parent.transform.Find("bottom-left").GetComponent<Image>();
        }

        if (strongEye_view == null)
            strongEye_view = strongEye_view_parent.GetComponentInChildren<VideoPlayer>();
        strongEye_view_image = strongEye_view.GetComponent<RawImage>();
        InitStrongEyeVideoAlpha(0);
    }

    public void InitStrongEyeVideoAlpha(float alpha)
    {
        Color video_color = strongEye_view_image.color;
        strongEye_view_image.color = new Vector4(video_color.r, video_color.g, video_color.b, alpha);

    }

    public void InitBeforeTrialStart()
    {
        InitStrongEyeVideoAlpha(1);

    }

    public void SetVisaulTargetInvisible()
    {
        SetImageAlpha(0);
        InitStrongEyeVideoAlpha(0);
    }

    /// <summary>
    /// /// Process funtion when start next trial
    /// </summary>
    /// <param name="_weakEyeSprite"></param>
    /// <param name="_videoPath"></param>
    public void InitTrialSetting(Sprite[] _weakEyeSprite, string _videoPath)
    {
        //set image color before calibration
        SetVisaulTargetInvisible();

        //set image and video content before calibration
        SetStrongView(_videoPath);
        SetWeakEyeView(_weakEyeSprite);
    }

    /// <summary>
    /// Process function when finishing experiment setting
    /// </summary>
    /// <param name="mode"></param>
    public void SetEyeCameraView(int mode)
    {

        if (mode == 0)
        {
            ChangeCameraView(LeftEyeCamera.transform, StrongEyeLayer);
            ChangeCameraView(RightEyeCamera.transform, WeakEyeEyeLayer);
            Debug.Log("mode 0 ; weakeye : righteye ; strongeye : lefteye");
        }
        else if (mode == 1)
        {
            ChangeCameraView(LeftEyeCamera.transform, WeakEyeEyeLayer);
            ChangeCameraView(RightEyeCamera.transform, StrongEyeLayer);
            Debug.Log("mode 1 ; weakeye : lefteye ; strongeye : righteye");
        }
    }
    public void SetImageAlpha(float alpha)
    {
        foreach (var img in weakEye_view)
        {
            img.color = new Vector4(
                img.color.r,
                img.color.g,
                img.color.b,
                alpha);
        }
    }
    void SetStrongView(string video_url)
    {
        strongEye_view.url = video_url;
    }

    void SetWeakEyeView(Sprite[] _view)
    {
        for (int i = 0; i < _view.Length; i++)
            weakEye_view[i].sprite = _view[i];

    }
    void ChangeCameraView(Transform parent, LayerMask layerIndex)
    {
        foreach (var cam in parent.GetComponentsInChildren<Camera>())
        {
            cam.cullingMask = layerIndex;
        }
    }

    void ChangeChildLayer(Transform parent, int layerIndex)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            child.gameObject.layer = layerIndex;
        }
    }

}
