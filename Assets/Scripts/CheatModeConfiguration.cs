using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatModeConfiguration : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        PlayerPrefs.SetInt("CheatModeActive", 0);
        toggle = GetComponent<Toggle>();
    }

    public void setCheatMode()
    {
        PlayerPrefs.SetInt("CheatModeActive", toggle.isOn ? 1 : 0);
    }
}
