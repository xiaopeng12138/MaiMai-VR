using System;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine;

    public class RegisterTouchSerial : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int Area;

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("trigger Touch");
            Serial.ChangeTouch((int)Area, true);
            Serial.SendTouch();
        }

        private void OnTriggerExit(Collider other)
        {
            Serial.ChangeTouch((int)Area, false);
            Serial.ResetTouch();
        }

    }
