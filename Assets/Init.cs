using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    public int displays = 3;
    // Start is called before the first frame update
    void Start()
    {
         Display.displays[1].Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
