using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WinCapture;

public class ExampleUsage : MonoBehaviour {

    [DllImport("coredll.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    Shader windowShader;
    Shader desktopShader;
    Shader chromiumShader;
    WindowCaptureManager captureManager;

    public Dictionary<IntPtr, WindowCapture> windowsRendering;
    public Dictionary<IntPtr, GameObject> windowObjects;


    //DesktopCapture desktopCapture1;
    //GameObject desktopObject;

    bool windowFound = false;
    bool actualWindowFound = false;

    int count = 0;

    // Use this for initialization
    void Start () {

        windowShader = Shader.Find("WinCapture/WindowShader");
        desktopShader = Shader.Find("WinCapture/DesktopShader");
        chromiumShader = Shader.Find("WinCapture/ChromiumShader");

        windowsRendering = new Dictionary<IntPtr, WindowCapture>();
        windowObjects = new Dictionary<IntPtr, GameObject>();
        captureManager = new WindowCaptureManager();
        captureManager.OnAddWindow += OnAddWindow;
        captureManager.OnRemoveWindow += OnRemoveWindow;
        lastUpdateTime = Time.time;
        lastPollWindowsTime = Time.time;



        //int displayNum = 0;
        //desktopCapture1 = new DesktopCapture(displayNum);

        //desktopObject = transform.gameObject;
        //desktopObject.name = "desktop" + displayNum;
        //desktopObject.transform.GetComponent<Renderer>().material = new Material(desktopShader);
    }

    // You need to do this because the desktop capture API will only work if we are on the graphics thread
    //void OnPostRender()
    //{
    //    desktopCapture1.OnPostRender();
    //}


    bool IsGoodWindow(WindowCapture window)
    {
        // You can stick whatever logic or names you want here for windows you want to keep to render

        string windowLowerTitle = window.windowInfo.title.ToLower();
        if (windowLowerTitle == "teagfx directx release")
        {
            Debug.Log("Saw window: " + window.windowInfo.title);
            return true;
        }
        return false;
    }

    void OnAddWindow(WindowCapture window)
    {
        if (!windowsRendering.ContainsKey(window.hwnd) && IsGoodWindow(window))
        {
            GameObject windowObject = transform.gameObject;
            windowObject.name = window.windowInfo.title;
            //windowObject.transform.GetComponent<Renderer>().material = new Material(windowShader);
            windowsRendering[window.hwnd] = window;
            windowObjects[window.hwnd] = windowObject;
            windowFound = true;
            actualWindowFound = true;
            count = 0;
        }
    }

    void OnRemoveWindow(WindowCapture window)
    {
        Debug.Log("removed " + window.windowInfo.title);
        if (windowsRendering.ContainsKey(window.hwnd))
        {
            GameObject windowObjectRemoving = windowObjects[window.hwnd];
            Destroy(windowObjectRemoving);
            windowObjects.Remove(window.hwnd);
            windowsRendering.Remove(window.hwnd);
        }
    }

    float lastUpdateTime;
    float lastPollWindowsTime;

    public float captureRateFps = 60;
    public float windowPollPerSecond = 2;
    public float windowScale = 0.001f;

    int currentRateCount = 0;

    public Material[] mats;
    public Texture noSignalTex;

    Texture2D windowTexture;

    // Update is called once per frame
    void Update()
    {
            bool didChange;
            // Capture each window
            foreach (IntPtr key in windowsRendering.Keys)
            {
                WindowCapture window = windowsRendering[key];
                GameObject windowObject = windowObjects[key];

                if (windowObject == null)
                {
                    continue;
                }

                windowTexture = window.GetWindowTexture(out didChange);
                if (didChange)
                {
                    //windowObject.GetComponent<Renderer>().material.mainTexture = windowTexture;
                    foreach (Material mat in mats)
                    {
                        mat.mainTexture = windowTexture;
                    }
                }
                windowTexture = null;
                //windowObject.transform.localScale = new Vector3(window.windowWidth * windowScale, 0.1f, window.windowHeight * windowScale);
            }

            // Poll for new windows
            if (windowFound == false)
            {
                if (count >= 5)
                {
                    windowFound = true;
                }
                if (Time.time - lastPollWindowsTime < 1.0f / windowPollPerSecond)
                {
                    return;
                }
                else
                {
                    count++;
                    lastPollWindowsTime = Time.time;
                    // calls OnAddWindow or OnRemoveWindow above if any windows have been added or removed
                    captureManager.Poll();
                }
            }
            if (!actualWindowFound)
            {
                foreach (Material mat in mats)
                {
                    mat.mainTexture = noSignalTex;
                }
            }
        }
    }
