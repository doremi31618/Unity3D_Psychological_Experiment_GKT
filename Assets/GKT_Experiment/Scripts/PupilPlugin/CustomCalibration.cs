using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;

[CreateAssetMenu(fileName = "Custom Calibration Targets", menuName = "Pupil/CustomCalibrationTargets", order = 3)]
public class CustomCalibration : CalibrationTargets
{
    [System.Serializable]
    public struct SquareTarget
    {
        public Rect target;
        public float zDepth;
    }
    
    public List<SquareTarget> squares = new List<SquareTarget>();
    int cornerIdx;
    int squareIdx;

    public override int GetTargetCount()
    {
        return 4 * squares.Count;
    }

    public override Vector3 GetLocalTargetPosAt(int idx)
    {
        squareIdx = idx / squares.Count;
        cornerIdx = idx % 4;
        return UpdateCalibrationPoint();
    }

    private Vector3 UpdateCalibrationPoint()
    {
        SquareTarget square = squares[squareIdx];
        Vector3 center = new Vector3(square.target.x, square.target.y, square.zDepth);

        float width = square.target.width;
        float height = square.target.height;

        float x = (cornerIdx % 3 == 0) ? -width / 2 : width / 2;
        float y = (cornerIdx < 2) ? height / 2 : -height / 2;

        

        Vector3 position = new Vector3(x + center.x, y + center.y, center.z);

        // Debug.Log("squareIdx : " + squareIdx + " ; cornerIdx : " + cornerIdx);
        // Debug.Log("x : " + position.x + " y : " + position.y + " z " + position.z);

        return position;
    }

}
