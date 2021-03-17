using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PupilLabs;
using PupilLabs.Demos;

public class CustomGazeVisaulizer : GazeVisualizer
{

    private GazeMode mode;

    public void SetGazeMode(GazeMode _gazeMode)
    {
        mode = _gazeMode;
    }

    protected override void ReceiveGaze(GazeData gazeData)
    {
        switch (mode)
        {
            case GazeMode.Right:
                if (gazeData.MappingContext != GazeData.GazeMappingContext.Monocular_0)
                {
                    return;
                }

                lastConfidence = gazeData.Confidence;

                if (gazeData.Confidence < confidenceThreshold)
                {
                    return;
                }

                localGazeDirection = gazeData.GazeNormal1;
                gazeDistance = gazeData.GazeDistance;
                break;

            case GazeMode.Left:
                if (gazeData.MappingContext != GazeData.GazeMappingContext.Monocular_1)
                {
                    return;
                }

                lastConfidence = gazeData.Confidence;

                if (gazeData.Confidence < confidenceThreshold)
                {
                    return;
                }

                localGazeDirection = gazeData.GazeNormal1;
                gazeDistance = gazeData.GazeDistance;
                break;

            case GazeMode.Both:
                if (gazeData.MappingContext != GazeData.GazeMappingContext.Binocular)
                {
                    return;
                }

                lastConfidence = gazeData.Confidence;

                if (gazeData.Confidence < confidenceThreshold)
                {
                    return;
                }
                localGazeDirection = gazeData.GazeDirection;
                gazeDistance = gazeData.GazeDistance;
                break;
        }
    }

}
