using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalTimeManager
{
    private static TimeManager _timeManager;

    public static TimeManager Instance
    {
        get
        {
            if (_timeManager == null)
            {
                _timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
            }
            return _timeManager;
        }
    }
}
