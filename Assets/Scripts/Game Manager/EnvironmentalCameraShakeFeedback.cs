using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EnvironmentalCameraShakeFeedback
{
    private static MMF_Player environmentalCameraShakeFeedback;

    public static MMF_Player Instance
    {
        get
        {
            if (environmentalCameraShakeFeedback == null)
            {
                environmentalCameraShakeFeedback = GameObject.Find("Environmental Camera").GetComponent<MMF_Player>();
            }
            return environmentalCameraShakeFeedback;
        }
    }
}
