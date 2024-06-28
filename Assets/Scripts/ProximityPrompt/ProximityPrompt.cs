using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ProximityPrompt : MonoBehaviour
{

    [SerializeField] private string message;

    private SphereCollider interactHitbox;
    // Start is called before the first frame update
    private void Start()
    {
        interactHitbox= GetComponent<SphereCollider>();
        if (!interactHitbox.isTrigger)
            interactHitbox.isTrigger = true;   
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_PROXIMITY_PROMPT_ENTER, WrapObject());
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            EventBroadcaster.Instance.PostEvent(EventNames.UI_EVENTS.ON_PROXIMITY_PROMPT_EXIT, WrapObject());
    }

    Parameters WrapObject()
    {
        Parameters p = new Parameters();
        p.PutObjectExtra("PROMPT_SUBJECT", gameObject);
        return p;
    }

}
