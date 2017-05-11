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

    // We'll pass native pointer to the mesh vertex buffer.
    // Also passing source unmodified mesh data.
    // The plugin will fill vertex data from native code.
    [DllImport("RenderingPlugin")]
    private static extern void SetMeshBuffersFromUnity(IntPtr vertexBuffer, int vertexCount, IntPtr sourceVertices, IntPtr sourceNormals, IntPtr sourceUVs);

    [DllImport("RenderingPlugin")]
    private static extern IntPtr GetRenderEventFunc();

    [DllImport("RenderingPlugin")]
    private static extern void InitOpenCV();

    [DllImport("RenderingPlugin")]
    private static extern void GetDeviceCount();

    [DllImport("RenderingPlugin")]
    private static extern int FreeResources();	
	
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
        // Create a texture
        tex = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        // Set point filtering just so we can see the pixels clearly
        tex.filterMode = FilterMode.Trilinear;
        // Call Apply() so it's actually uploaded to the GPU
        tex.Apply();

        // Set texture onto our material
        blurredTexture = RenderTexture.GetTemporary(tex.width, tex.height);
        targetMaterial = blurredPlane.GetComponent<Renderer>().material;
        targetMaterial.mainTexture = blurredTexture;
        GetComponent<Renderer>().material.mainTexture = tex;

        // Pass texture pointer to the plugin
        SetTextureFromUnity(tex.GetNativeTexturePtr(), tex.width, tex.height);
    }

    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (isRunning)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            // Issue a plugin event with arbitrary integer identifier.
            // The plugin can distinguish between different
            // things it needs to do based on this ID.
            // For our simple plugin, it does not matter which ID we pass here.
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            BlurTexture(tex, blurredTexture);
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
        Graphics.CopyTexture(source, buffer);

        // Blur the small texture
        for (int i = 0; i < iterations; i++)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            FourTapCone(buffer, buffer2, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }
        Graphics.Blit(buffer, destination);

        RenderTexture.ReleaseTemporary(buffer);
    }
}
