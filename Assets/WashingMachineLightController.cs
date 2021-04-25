using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

public class WashingMachineLightController : MonoBehaviour
{
    public Light[] lights;

    int count = 0;

    const int PROCESS_WM_READ = 0x0010;

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern IntPtr ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

    Process proc;
    IntPtr procBase;

    IntPtr ptrtoread = (IntPtr)0xB471CC; 

    // Start is called before the first frame update
    void Start()
    {
        proc = Process.GetProcessesByName("maimai_dump_")[0];
        procBase = proc.MainModule.BaseAddress;
    }

    // Update is called once per frame
    void Update()
    {
        IntPtr[] ptrArray = { ptrtoread, (IntPtr)0x3CD };
        if (proc != null && procBase != null) {
            readLight("maimai_dump_", ptrArray);
        }
    }

    public void readLight(string pname, IntPtr[] offsets, bool debug = true, string module = null) {
            IntPtr tmpptr = (IntPtr)0;
            Process handle = proc;
            IntPtr Base = procBase;

            //Console.WriteLine("Program memory base: " + Base);
            //Console.WriteLine("First offset: " + offsets[0]);

            IntPtr ptr = IntPtr.Add(Base, (int)offsets[0]);
            tmpptr = (IntPtr)ReadInt64(ptr, 8, false, handle.Handle);

            //Console.WriteLine("Program memory base + first offset: " + ptr);
            //Console.WriteLine("Data (Int32-pointer): " + tmpptr);

            IntPtr ptr2 = IntPtr.Add(tmpptr, (int)offsets[1]);
            //Console.WriteLine("Light offset: " + ptr2);

            //Yeah, it's shitstorm but it works anyway
            //P1
            for (int i = 0; i < 8; i ++) {
                Byte[] rgb_bytes = ReadBytes((IntPtr)handle.Handle, ptr2 + 1 + (i*4), 3);
                string hex = "#" + BitConverter.ToString(rgb_bytes).Replace("-", string.Empty);
                Color btnColor;
                if (ColorUtility.TryParseHtmlString(hex, out btnColor)) {
                    lights[i].color = btnColor;
                } else {
                    UnityEngine.Debug.LogWarning("WTF: " + hex + " failed to parse");
                }
            }
            
            //P2
            for (int i = 0; i < 8; i ++) {
                Byte[] rgb_bytes = ReadBytes((IntPtr)handle.Handle, ptr2 + 69 + (i*4), 3);
                string hex = "#" + BitConverter.ToString(rgb_bytes).Replace("-", string.Empty);
                Color btnColor;
                if (ColorUtility.TryParseHtmlString(hex, out btnColor)) {
                    lights[i+8].color = btnColor;
                } else {
                    UnityEngine.Debug.LogWarning("WTF: " + hex + " failed to parse");
                }
            }
            
            //Body LEDs
            Byte[] led_intensity_p1 = ReadBytes((IntPtr)handle.Handle, ptr2 + 49, 1);
            byte led_p1 = led_intensity_p1[0];
            Byte[] led_intensity_p2 = ReadBytes((IntPtr)handle.Handle, ptr2 + 117, 1);
            byte led_p2 = led_intensity_p2[0];
            
            lights[16].intensity = (int)led_p1 / 70;
            lights[17].intensity = (int)led_p2 / 70;
        }

    public Int64 ReadInt64(IntPtr Address, uint length = 4, bool isFinal = false, IntPtr? Handle = null)
    {
        if (isFinal)
        {
            return (int)ReadBytes((IntPtr)Handle, Address, length)[0];
        }
        else
        {
            return BitConverter.ToInt32(ReadBytes((IntPtr)Handle, Address, length), 0);
        }
    }

    public static byte[] ReadBytes(IntPtr Handle, IntPtr Address, uint BytesToRead)
    {
        IntPtr ptrBytesRead;
        byte[] buffer = new byte[BytesToRead];
        ReadProcessMemory(Handle, Address, buffer, BytesToRead, out ptrBytesRead);
        return buffer;
    }
}
