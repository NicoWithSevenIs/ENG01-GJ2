using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPromptWorld : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject proximityPrompt;
    private ProximityPromptUI ui;

  
    private void Start()
    {
        ui = proximityPrompt.GetComponent<ProximityPromptUI>();
        ui.setPlayerInRange(false);

    }
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //canvas.enabled = true;
            ui.setPlayerInRange(true);
            canvas.enabled = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(delayedDisable());
        }
            
     
    }

    private IEnumerator delayedDisable()
    {
        ui.setPlayerInRange(false);
        yield return new WaitForSeconds(0.15f);
        canvas.enabled = false;
    }


}
