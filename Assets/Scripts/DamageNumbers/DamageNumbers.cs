using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DamageNumbers : MonoBehaviour
{
    public MMF_Player feedbacks;
    private MMF_FloatingText floatingText;

    public void DamageValues(Transform hit, float damage)
    {
        if (damage <= 0) return;
        floatingText = feedbacks.GetFeedbackOfType<MMF_FloatingText>();
        floatingText.PositionMode = MMF_FloatingText.PositionModes.TargetTransform;
        floatingText.TargetTransform = hit;
        floatingText.Value = damage.ToString();
        feedbacks.PlayFeedbacks();
    }
}
