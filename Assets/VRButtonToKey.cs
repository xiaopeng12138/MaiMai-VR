using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using WindowsInput;

public class VRButtonToKey : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    public static class WinAPI
    {
        public const uint MAPVK_VK_TO_SC = 0;
        public const uint MAPVK_SC_TO_VK = 1;

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2;

        [StructLayout(LayoutKind.Explicit)]
        public struct Input
        {
            [FieldOffset(0)]
            public int Type;

            [FieldOffset(4)]
            public KeyboardInput Ki;

            [FieldOffset(4)]
            public MouseInput Mi;

            [FieldOffset(4)]
            public HardwareInput Hi;

            public static Input Keyboard(KeyboardInput input)
            {
                return new Input { Type = INPUT_KEYBOARD, Ki = input };
            }

            public static Input Mouse(MouseInput input)
            {
                return new Input { Type = INPUT_MOUSE, Mi = input };
            }

            public static Input Hardware(HardwareInput input)
            {
                return new Input { Type = INPUT_HARDWARE, Hi = input };
            }
        }

        public const uint KEYEVENTF_EXTENDED = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_SCANCODE = 0x0008;

        public struct KeyboardInput
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;

            public KeyboardInput(ushort Vk = 0, ushort Scan = 0, uint Flags = 0, uint Time = 0)
            {
                this.Vk = Vk;
                this.Scan = Scan;
                this.Flags = Flags;
                this.Time = Time;
                ExtraInfo = GetMessageExtraInfo();
            }
        }

        public struct MouseInput
        {
            public int Dx;
            public int Dy;
            public uint Data;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;

            public MouseInput(int Dx = 0, int Dy = 0, uint Data = 0, uint Flags = 0, uint Time = 0)
            {
                this.Dx = Dx;
                this.Dy = Dy;
                this.Data = Data;
                this.Flags = Flags;
                this.Time = Time;
                ExtraInfo = GetMessageExtraInfo();
            }
        }

        public struct HardwareInput
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;

            public HardwareInput(uint Msg = 0, ushort ParamL = 0, ushort ParamH = 0)
            {
                this.Msg = Msg;
                this.ParamL = ParamL;
                this.ParamH = ParamH;
            }
        }

        public static uint SendInput(Input[] inputs)
        {
            return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<Input>());
        }
    }
    public VirtualKeyCode keyToPress;
    public AudioClip click;
    public Light lightTarget;
    SteamVR_Action_Vibration haptic = new SteamVR_Action_Vibration();
    //AudioSource audio;
    // Start is called before the first frame update
        void Start()
        {
        //Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //audio = transform.gameObject.AddComponent<AudioSource>();
        //audio.clip = click;
        //audio.volume = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.LogWarning("Button down " + ((uint)keyToPress).ToString());
            keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 0, UIntPtr.Zero);  
            //SendKeyEvent((uint)keyToPress, true);
            Hand hand = other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>();
        //hand.TriggerHapticPulse(1000, (other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>().GetTrackedObjectVelocity().z * -100), other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>().GetTrackedObjectVelocity().z * -10000);
        hand.TriggerHapticPulse(0.1f, 1, 0.5f);
        if (lightTarget != null) {
            //lightTarget.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Hand hand = other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>();
        //hand.TriggerHapticPulse(0.1f, 1, 0.1f);
    }

    private void OnTriggerExit(Collider other)
        {
        //Debug.LogWarning("Button up " + ((uint)keyToPress).ToString());
        //SetForegroundWindow(System.Diagnostics.Process.GetProcessesByName("maimai_dump_")[0].MainWindowHandle);
        keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 2, UIntPtr.Zero);
        //SendKeyEvent((uint)keyToPress, false);
        Hand hand = other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>();
        //hand.TriggerHapticPulse(10, (other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>().GetTrackedObjectVelocity().z), other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Hand>().GetTrackedObjectVelocity().z * 100);
        hand.TriggerHapticPulse(0.1f, 1, 0.3f);
        //lightTarget.gameObject.SetActive(false);
    }

        public static void SendKeyEvent(uint keyCode, bool isDown)
        {
            uint flags = WinAPI.KEYEVENTF_SCANCODE;

            if (!isDown) flags |= WinAPI.KEYEVENTF_KEYUP;
            if ((keyCode & 0x100) > 0) flags |= WinAPI.KEYEVENTF_EXTENDED;

            ushort scan = (ushort)WinAPI.MapVirtualKey(keyCode & 0xFF, WinAPI.MAPVK_VK_TO_SC);

            WinAPI.SendInput(new WinAPI.Input[] {
                    WinAPI.Input.Keyboard(new WinAPI.KeyboardInput(Scan: scan, Flags: flags)),
                });
        }
}
