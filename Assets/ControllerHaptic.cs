using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHaptic : MonoBehaviour
{
    public float frequency;
    public float amplitude;
    public OVRInput.Controller controllerMask = new OVRInput.Controller();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

             
    }
    private void OnTriggerStay(Collider other)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controllerMask);
    }
    private void OnTriggerEnter(Collider other)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controllerMask);
        ///Debug.Log("Haptic On");
    }
    private void OnTriggerExit(Collider other)
    {
        OVRInput.SetControllerVibration(0f, 0f, controllerMask);
        //Debug.Log("Haptic Off");
    }

}
