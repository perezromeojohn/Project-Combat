using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;

    void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartState state)
    {
        switch (state)
        {
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartState.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartState
{
    Empty = 0,
    Half = 1,
    Full = 2
}
