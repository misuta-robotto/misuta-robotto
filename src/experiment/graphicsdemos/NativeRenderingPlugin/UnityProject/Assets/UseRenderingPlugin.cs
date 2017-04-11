using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public class UseRenderingPlugin : MonoBehaviour
{

    private int number_of_devices;
    private int current_device;
    // Native plugin rendering events are only called if a plugin is used
    // by some script. This means we have to DllImport at least
    // one function in some active script.
    // For this example, we'll call into plugin's SetTimeFromUnity
    // function and pass the current time so the plugin can animate.

    [DllImport ("RenderingPlugin")]
	private static extern void SetTimeFromUnity(float t);


	// We'll also pass native pointer to a texture in Unity.
	// The plugin will fill texture data from native code.
	[DllImport ("RenderingPlugin")]
	private static extern void SetTextureFromUnity(System.IntPtr texture, int w, int h);

	// We'll pass native pointer to the mesh vertex buffer.
	// Also passing source unmodified mesh data.
	// The plugin will fill vertex data from native code.
	[DllImport ("RenderingPlugin")]
	private static extern void SetMeshBuffersFromUnity (IntPtr vertexBuffer, int vertexCount, IntPtr sourceVertices, IntPtr sourceNormals, IntPtr sourceUVs);

	[DllImport("RenderingPlugin")]
	private static extern IntPtr GetRenderEventFunc();

    [DllImport("RenderingPlugin")]
    private static extern void InitOpenCV();

    [DllImport("RenderingPlugin")]
    private static extern int GetDeviceCount();

    [DllImport("RenderingPlugin")]
    private static extern void SetDevice(int dev);

    IEnumerator Start()
	{
		CreateTextureAndPassToPlugin();
        InitOpenCV();
        number_of_devices = GetDeviceCount();
        current_device = 0;
		//yield return StartCoroutine("CallPluginAtEndOfFrames");
        
	}

    public static void Update()
    {
        GL.IssuePluginEvent(GetRenderEventFunc(), 1);
    }

    private void OnEnable()
    {
        ControllerEventManager.OnClicked += cycleDevice;
    }

    private void OnDisable()
    {
        ControllerEventManager.OnClicked -= cycleDevice;
    }

    private void cycleDevice()
    {
        Debug.Log(number_of_devices);
        if (current_device++ > number_of_devices) {
            current_device = 0;
        }
        Debug.Log(current_device);
        SetDevice(current_device);
    }

    private void CreateTextureAndPassToPlugin()
	{
		// Create a texture
		Texture2D tex = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
		// Set point filtering just so we can see the pixels clearly
		tex.filterMode = FilterMode.Point;
		// Call Apply() so it's actually uploaded to the GPU
		tex.Apply();

		// Set texture onto our material
		GetComponent<Renderer>().material.mainTexture = tex;

		// Pass texture pointer to the plugin
		SetTextureFromUnity(tex.GetNativeTexturePtr(), tex.width, tex.height);
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame();

			// Issue a plugin event with arbitrary integer identifier.
			// The plugin can distinguish between different
			// things it needs to do based on this ID.
			// For our simple plugin, it does not matter which ID we pass here.
			GL.IssuePluginEvent(GetRenderEventFunc(), 1);
		}
	}
}
