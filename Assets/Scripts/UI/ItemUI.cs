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
    [SerializeField] private string ItemName;
    [SerializeField] private KeyCode keybind;
    public KeyCode KeyBind { get { return keybind; } }

    [Header("Stats")]
    [SerializeField] private float cooldown = 1f;
    private float currentCooldown;
    [SerializeField] private bool initiateCooldownOnUse = true;
    private bool willTickDown = false;


    [Header("Events")]
    public UnityEvent onActivation;


    

    private void Start()
    {
        iconUI.sprite = icon;
        keybindUI.text = keybind.ToString();
        cooldownUI.fillAmount = 0;
        currentCooldown = 0f;

        EventBroadcaster.Instance.AddObserver(EventNames.UI_EVENTS.ON_COOLDOWN_INVOCATION, (Parameters p) => {

            if (p.GetStringExtra("ItemName", "") == ItemName && currentCooldown == cooldown)
                willTickDown = true;
        
        });

    }

    private void Update()
    {

        cooldownUI.fillAmount = currentCooldown / cooldown;

        if (currentCooldown > 0)
        {
            if(willTickDown)
                currentCooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(keybind))
        {
            print("Invoked");


            onActivation?.Invoke();
            currentCooldown = cooldown;

            willTickDown = initiateCooldownOnUse;
        }
    }

 
    

}
