using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;

public class CustomCalibrationController : CalibrationController
{
    public GameObject customPreviewMarkers;
    public override void UpdatePosition()
    {
        currLocalTargetPos = targets.GetLocalTargetPosAt(targetIdx);

        targetIdx++;
        tLastTarget = Time.time;
    }

    public override void UpdateMarker()
    {
        marker.position = camera.transform.localToWorldMatrix.MultiplyPoint(currLocalTargetPos);
        marker.LookAt(camera.transform.position);
    }

    protected override void InitPreviewMarker(){
        customPreviewMarkers.SetActive(true);
        previewMarkersActive = true;
    }

    protected override void SetPreviewMarkers(bool value){
        customPreviewMarkers.SetActive(value);
        previewMarkersActive = value;
    }
}
