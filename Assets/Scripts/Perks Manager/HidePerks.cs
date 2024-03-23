using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePerks : MonoBehaviour
{
    public PerkManager perkManager;

    void Hide()
    {
        perkManager.HideAllPerkButtons();
    }
}
