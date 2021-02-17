using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using WindowsInput;

public class RegisterTouch : MonoBehaviour
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
    public float moveDistance = 0.5f;
    public int frameRefreshDelay = 120;
    int currentRefresh = 0;
    public bool isPlayer1 = true;
    bool allowPress = true;
    Vector3 lastTouchPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 0, UIntPtr.Zero);
        //if (allowPress == true)
        //{
        //allowPress = true;
        //}       
        //lastTouchPos = other.gameObject.transform.position;


    }

    private void OnTriggerStay(Collider other)
    {
        //if (allowPress == true)
        //{
            

        //}
        //allowPress = false;
        //if (allowPress == true)
        //{
        //keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 2, UIntPtr.Zero);
        //allowPress = false;
        //}
        //else
        //{
        //    if (currentRefresh >= frameRefreshDelay)
        //    {
        //        allowPress = true;
        //    }
        //    currentRefresh++;
        //}        
        //keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 2, UIntPtr.Zero);
        // float distanceFromLastPos = Vector3.Distance(other.gameObject.transform.position, lastTouchPos);
        // if (distanceFromLastPos >= moveDistance)
        // {
        //    keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 0, UIntPtr.Zero);
        //    lastTouchPos = other.gameObject.transform.position;
        //    notPressing = false;
        // }
        //else if (notPressing == false)
        //{
        //    keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 2, UIntPtr.Zero);
        //    notPressing = true;
        //}
        //else
        //{
        //    if (currentRefresh >= frameRefreshDelay)
        //    {
        //        lastTouchPos = other.gameObject.transform.position;
        //        currentRefresh = 0;
        //    }
        //    currentRefresh++;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        keybd_event(System.Convert.ToByte(keyToPress), (byte)MapVirtualKey((uint)keyToPress, 0), 2, UIntPtr.Zero);
        //allowPress = true;
    }

    IEnumerator KeyPress()
    {
        yield return new WaitForSeconds(0.015f);
        
        yield return null;
    }
}
