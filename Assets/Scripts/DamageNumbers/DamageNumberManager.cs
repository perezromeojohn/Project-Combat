using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public static class DamageNumberManager
{
    private static DamageNumbers _damageNumbers;

    public static DamageNumbers Instance
    {
        get
        {
            if (_damageNumbers == null)
            {
                _damageNumbers = GameObject.Find("On Enemy Hit").GetComponent<DamageNumbers>();
            }
            return _damageNumbers;
        }
    }
}
