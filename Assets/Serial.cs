using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO.Ports;
using System;
//using System.Linq;
using System.Text;
    public class Serial : MonoBehaviour
    {
        static SerialPort p1Serial = new SerialPort ("COM5", 9600);
        int packleng = 0;
        byte[] incomPacket = new byte[6];
        byte[] settingPacket = new byte[6];
        static byte[] touchPacket = new byte[9];
        
        static bool startUp = false;
        float timer = 0; 
        bool failed = false;
        byte recivData;
        
        // Start is called before the first frame update
        void Start()
        {
            SerialStartUp();
        }

        // Update is called once per frame
        void Update()
        {
            ReadPack();
            if (!failed)
                TouchSetUp();
                
        }

        private void SerialStartUp()
        {
            settingPacket[0] = 40;
            settingPacket[5] = 41;
            touchPacket[0] = 40;
            touchPacket[8] = 41;
            p1Serial.Open();
            Debug.Log("Serial Started");
        }
        private void TouchSetUp()
        {
            switch (incomPacket[3])
            {
                case 76:
                case 69:
                    startUp = false;
                    break;
                case 114:
                case 107:
                    for (int i=1; i<5; i++)
                        settingPacket[i] = incomPacket[i];    
                    p1Serial.Write(settingPacket, 0, settingPacket.Length);
                    //Debug.Log("Send Package: "+settingPacket[0]+"-"+settingPacket[1]+"-"+settingPacket[2]+"-"+settingPacket[3]+"-"+settingPacket[4] +"-"+ settingPacket[5]);
                    //Debug.Log("Package String: "+Encoding.UTF8.GetString(settingPacket, 0, settingPacket.Length));
                    Array.Clear(incomPacket, 0, incomPacket.Length);
                    break;
                case 65:
                    startUp = true;
                    //Debug.Log("Now Serial State: "+Encoding.UTF8.GetString(incomPacket, 0, incomPacket.Length));
                    break;
            }
            
        }

        private void ReadPack()
        {
            timer = 0f;
            if (p1Serial.BytesToRead == 6)
            {
                packleng = 0;
                while (packleng < 6) 
                {
                    recivData = Convert.ToByte(p1Serial.ReadByte());
                    if (recivData == 123) 
                    {
                        packleng = 0;
                    }
                    incomPacket[packleng++] = recivData;
                    //Debug.Log(recivData);
                    if(timer > 20f){ failed = true; break; }
                    timer += Time.deltaTime;
                }
               //Debug.Log("RecivePackage: "+incomPacket[0]+"-"+incomPacket[1]+"-"+incomPacket[2]+"-"+incomPacket[3]+"-"+incomPacket[4] +"-"+ incomPacket[5]);
            }
        }
        private static void SendTouch()
        {
            if (startUp)
            {
                //Debug.Log("trigger serial 2");
                //Debug.Log(BitConverter.ToString(touchPacket));
                p1Serial.Write(touchPacket, 0, 9);
            }

        }
        public static void ChangeTouch(int Area, bool State)
        {
            if (startUp)
            {
                ByteArrayExt.SetBit(touchPacket, Area+8, State);
                p1Serial.Write(touchPacket, 0, 9);
                //Debug.Log("Send:" + BitConverter.ToString(touchPacket));
                //Debug.Log("trigger serial");
            }

        }
        
    }

    public static class ByteArrayExt
    {
        public static byte[] SetBit(this byte[] self, int index, bool value)
        {
            
            var bitArray = new BitArray(self);
            bitArray.Set(index, value);
            bitArray.CopyTo(self, 0);
            return self;
        }
    }
