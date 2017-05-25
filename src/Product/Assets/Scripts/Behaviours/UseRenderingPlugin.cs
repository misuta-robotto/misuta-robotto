using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

public class UseRenderingPlugin : MonoBehaviour
{
    public Calibration calibration;
    public Shader blurShader;
    public GameObject blurredPlane;

    Texture2D tex;
    RenderTexture blurredTexture;
    private Material targetMaterial;
    
    private bool isRunning = true;

    private int number_of_devices;
    
    private int current_device;

    static Material modelMaterial = null;
    
    protected Material material
    {
        get
        {
            if (modelMaterial == null)
            {
                modelMaterial = new Material(blurShader);
                modelMaterial.hideFlags = HideFlags.DontSave;
            }
            return modelMaterial;
        }
    }

    // We'll also pass native pointer to a texture in Unity.
    // The plugin will fill texture data from native code.
    [DllImport("RenderingPlugin")]
    private static extern void SetTextureFromUnity(System.IntPtr texture, int w, int h);

    [DllImport("RenderingPlugin")]
    private static extern IntPtr GetRenderEventFunc();

    [DllImport("RenderingPlugin")]
    private static extern void InitOpenCV();

    [DllImport("RenderingPlugin")]
    private static extern int GetDeviceCount();

    [DllImport("RenderingPlugin")]
    private static extern void FreeResources();	
	
    [DllImport("RenderingPlugin")]
    private static extern void SetDevice(int dev);

    [DllImport("RenderingPlugin")]
    private static extern int ReadFromCamera();

    IEnumerator Start()
    {
        SetEnabled(false);
        calibration.ToggleMode += SetEnabled;
        CreateTextureAndPassToPlugin();
        InitOpenCV();
    
    	number_of_devices = GetDeviceCount();
        current_device = 0;

        BeginUpdatingCameraData();
        yield return StartCoroutine("CallPluginAtEndOfFrames");
    }

    void SetEnabled(bool b)
    {
        GetComponent<Renderer>().enabled = b;

	blurredPlane.GetComponent<Renderer>().enabled = b;
    }

    private void OnEnable()
    {
        ControllerEventManager.OnClicked += cycleDevice;
    }

    private void OnDisable()
    {
        ControllerEventManager.OnClicked -= cycleDevice;
        isRunning = false;
    }

    private void cycleDevice()
    {
        if (++current_device > number_of_devices)
        {
            current_device = 0;
        }
        SetDevice(current_device);
    }

    private void CreateTextureAndPassToPlugin()
    {
        tex = new Texture2D(1920, 1920, TextureFormat.ARGB32, false);
        for (int x = 0; x < 1920; x++)
        {
            for (int y = 0; y < 1920; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }
        tex.Apply();

        tex.filterMode = FilterMode.Trilinear;
        // uploaded to the GPU
        tex.Apply();

        // Set texture onto our material
        blurredTexture = RenderTexture.GetTemporary(tex.width, tex.height);
        targetMaterial = blurredPlane.GetComponent<Renderer>().material;
        targetMaterial.mainTexture = blurredTexture;
        GetComponent<Renderer>().material.mainTexture = tex;

        // Pass texture pointer to the plugin
        SetTextureFromUnity(tex.GetNativeTexturePtr(), 1920, 1080);
    }

    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (isRunning)
        {
            yield return new WaitForEndOfFrame();

            // Tell plugin to copy camera data to texture
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            //BlurTexture(tex, blurredTexture);
        }
    }

    // Executed on a non-blocking thread.
    private void UpdateCameraData()
    {
        while (isRunning)
        {
            ReadFromCamera();
        }

        // Perform clean up and close camera feed
        FreeResources();
    }

    // Starts updating camera data in another thread.
    private void BeginUpdatingCameraData()
    {
        new Thread(new ThreadStart(UpdateCameraData)).Start();
    }

    public void FourTapCone(Texture source, RenderTexture dest, int iteration)
    {
        float blurSpread = 2f;
        float off = 0.5f + (iteration * blurSpread);
        Graphics.BlitMultiTap(source, dest, material,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off)
            );
    }

    // Called by the camera to apply the image effect
    void BlurTexture(Texture source, RenderTexture destination)
    {
        int rtW = source.width;
        int rtH = source.height;
        int iterations = 3;
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
        if (!buffer.IsCreated())
        {
            buffer.Create();
        }
        Graphics.CopyTexture(source, buffer);

        // Blur the small texture
        for (int i = 0; i < iterations; i++) {
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            FourTapCone(buffer, buffer2, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }
        Graphics.Blit(buffer, destination);

        RenderTexture.ReleaseTemporary(buffer);
    }
}
