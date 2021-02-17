using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

public class WashingMachineLightController : MonoBehaviour
{
    public Light[] lights;
    public float maxIntensity = 4f;
    public float minIntensity = 2f;
    public float time = 1f;
    float currentIntensity = 0f;
    float progress = 0;
    bool increasing = true;

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
        currentIntensity = minIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        IntPtr[] ptrArray = { ptrtoread, (IntPtr)0x3CD };
        if (proc != null && procBase != null) {
            readBtnLight("maimai_dump_", ptrArray);
        }
            //IntPtr[] ptrArray = { ptrtoread, (IntPtr)0x408, (IntPtr)0x84, (IntPtr)0x34, (IntPtr)0xC, (IntPtr)0x5A4 };
            //IntPtr[] ptrArray2 = { ptrtoread, (IntPtr)0x408, (IntPtr)0x84, (IntPtr)0x34, (IntPtr)0xC, (IntPtr)0x5A5 };
            
            //IntPtr bodyLEDValue1 = readPtr("maimai_dump_", ptrArray);
            //IntPtr bodyLEDValue2 = readPtr("maimai_dump_", ptrArray2);
            //UnityEngine.Debug.LogWarning("P1 raw intensity:" + (int)bodyLEDValue1 + " P2 raw intensity:" + (int)bodyLEDValue2);
            //float[] intensity = new float[2];
            //lights[0].intensity = (int)bodyLEDValue1 / 60;
            //lights[1].intensity = (int)bodyLEDValue2 / 60;
            
            //if (progress > 1)
            //{
            //    progress = 0;
            //    increasing = !increasing;
            //}
            //if (increasing == true)
            //{
            //    currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, progress);
            //}
            //else
            //{
            //    currentIntensity = Mathf.Lerp(maxIntensity, minIntensity, progress);
            //}
            //foreach (Light l in lights)
            //{
            //    l.intensity = currentIntensity;
            //}
            //progress += Time.deltaTime / time;  
    }

    public void readBtnLight(string pname, IntPtr[] offsets, bool debug = true, string module = null) {
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

            for (int i = 0; i < 8; i ++) {
                Byte[] rgb_bytes = ReadBytes((IntPtr)handle.Handle, ptr2 + 1 + (i*4), 3);
                //IntPtr rgb_int = (IntPtr)ReadInt64(ptr2 + (i*4), 4, false, handle.Handle);
                //Console.WriteLine("[Light " + (i+1) + "] " + BitConverter.ToString(rgb_bytes));
                //ptr2 = IntPtr.Add(tmpptr, 8);
                string hex = "#" + BitConverter.ToString(rgb_bytes).Replace("-", string.Empty);
                Color btnColor;
                if (ColorUtility.TryParseHtmlString(hex, out btnColor)) {
                    lights[i].color = btnColor;
                } else {
                    UnityEngine.Debug.LogWarning("WTF: " + hex + " failed to parse");
                }
            }

        }

    public IntPtr readPtr(string pname, IntPtr[] offsets, bool debug = false, string module = null)
    {

        IntPtr tmpptr = (IntPtr)0;
        Process handle = proc;
        IntPtr Base = procBase;

        for (int i = 0; i <= offsets.Length - 1; i++)
        {
            if (i == 0)
            {
                if (debug)
                    UnityEngine.Debug.LogWarning(Base + "[Base] + " + offsets[i] + "[OFFSET 0]");
                IntPtr ptr = IntPtr.Add(Base, (int)offsets[i]);
                tmpptr = (IntPtr)ReadInt64(ptr, 8, false, handle.Handle);
                if (debug)
                    UnityEngine.Debug.LogWarning(" is " + tmpptr);
            }
            else
            {
                if (debug)
                    UnityEngine.Debug.LogWarning(tmpptr + " + " + offsets[i] + "[OFFSET " + i + "]");
                IntPtr ptr2 = IntPtr.Add(tmpptr, (int)offsets[i]);
                if (i == offsets.Length - 1)
                {
                    tmpptr = (IntPtr)ReadInt64(ptr2, 1, true, handle.Handle);
                }
                else
                {
                    tmpptr = (IntPtr)ReadInt64(ptr2, 8, false, handle.Handle);
                }
                if (debug)
                    UnityEngine.Debug.LogWarning(" is " + tmpptr);
            }
        }
        return tmpptr;
    }

    //static IntPtr getBase(Process handle, string module = null)
    //{
    //    ProcessModuleCollection modules = handle.Modules;
    //    if (module != null)
    //    {
    //        for (int i = 0; i <= modules.Count - 1; i++)
    //        {
    //            if (modules[i].ModuleName == module)
    //            {
    //                return (IntPtr)modules[i].BaseAddress;
    //            }
    //        }
    //        Console.WriteLine("Module Not Found");

    //    }
    //    else
    //    {
    //        return (IntPtr)handle.MainModule.BaseAddress;
    //    }
    //    return (IntPtr)0;
    //}

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
