using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    //ermm im gonna merge the ui and item effects bc i dont feel like creating an item manager rn

    [Header("References")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Image iconUI;
    [SerializeField] private TextMeshProUGUI keybindUI;
    [SerializeField] private Image cooldownUI;


    [Header("Settings")]
    [SerializeField] private KeyCode keybind;

    [Header("Stats")]
    [SerializeField] private float cooldown = 1f;
    private float currentCooldown;

    [Header("Events")]
    public UnityEvent onActivation;

    private void Start()
    {
        iconUI.sprite = icon;
        keybindUI.text = keybind.ToString();
        cooldownUI.fillAmount = 0;
        currentCooldown = 0f;
    }

    private void Update()
    {

        cooldownUI.fillAmount = currentCooldown / cooldown;
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(keybind))
        {
            onActivation?.Invoke();
            currentCooldown = cooldown;
        }
    }

    

}
