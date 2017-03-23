// Example low level rendering Unity plugin

#include "PlatformBase.h"
#include "RenderAPI.h"

#include <assert.h>
#include <math.h>
#include <vector>

#include <opencv2/opencv.hpp>


// --------------------------------------------------------------------------
// SetTimeFromUnity, an example function we export which is called by one of the scripts.

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetDevice(int dev);

static cv::Mat frame;
static cv::VideoCapture cap;
static int device_count;

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API InitOpenCV()
{
	device_count = 0;
	while (cap.open(device_count)) {
		device_count++;
	}
	SetDevice(0);
}

extern "C" int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API GetDeviceCount()
{
	return device_count;
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetDevice(int dev)
{
	if (cap.isOpened()) {
		cap.release();
	}
	cap.open(dev);
	cap.set(cv::CAP_PROP_FRAME_WIDTH, 1920);
	cap.set(cv::CAP_PROP_FRAME_HEIGHT, 1080);
}

// --------------------------------------------------------------------------
// SetTextureFromUnity, an example function we export which is called by one of the scripts.

static void* g_TextureHandle = NULL;
static int   g_TextureWidth  = 0;
static int   g_TextureHeight = 0;

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetTextureFromUnity(void* textureHandle, int w, int h)
{
	// A script calls this at initialization time; just remember the texture pointer here.
	// Will update texture pixels each frame from the plugin rendering event (texture update
	// needs to happen on the rendering thread).
	g_TextureHandle = textureHandle;
	g_TextureWidth = w;
	g_TextureHeight = h;
}



// --------------------------------------------------------------------------
// UnitySetInterfaces

static void UNITY_INTERFACE_API OnGraphicsDeviceEvent(UnityGfxDeviceEventType eventType);

static IUnityInterfaces* s_UnityInterfaces = NULL;
static IUnityGraphics* s_Graphics = NULL;

extern "C" void	UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces* unityInterfaces)
{
	s_UnityInterfaces = unityInterfaces;
	s_Graphics = s_UnityInterfaces->Get<IUnityGraphics>();
	s_Graphics->RegisterDeviceEventCallback(OnGraphicsDeviceEvent);

	// Run OnGraphicsDeviceEvent(initialize) manually on plugin load
	OnGraphicsDeviceEvent(kUnityGfxDeviceEventInitialize);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload()
{
	s_Graphics->UnregisterDeviceEventCallback(OnGraphicsDeviceEvent);
}

#if UNITY_WEBGL
typedef void	(UNITY_INTERFACE_API * PluginLoadFunc)(IUnityInterfaces* unityInterfaces);
typedef void	(UNITY_INTERFACE_API * PluginUnloadFunc)();

extern "C" void	UnityRegisterRenderingPlugin(PluginLoadFunc loadPlugin, PluginUnloadFunc unloadPlugin);

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API RegisterPlugin()
{
	UnityRegisterRenderingPlugin(UnityPluginLoad, UnityPluginUnload);
}
#endif

// --------------------------------------------------------------------------
// GraphicsDeviceEvent


static RenderAPI* s_CurrentAPI = NULL;
static UnityGfxRenderer s_DeviceType = kUnityGfxRendererNull;


static void UNITY_INTERFACE_API OnGraphicsDeviceEvent(UnityGfxDeviceEventType eventType)
{
	// Create graphics API implementation upon initialization
	if (eventType == kUnityGfxDeviceEventInitialize)
	{
		assert(s_CurrentAPI == NULL);
		s_DeviceType = s_Graphics->GetRenderer();
		s_CurrentAPI = CreateRenderAPI(s_DeviceType);
	}

	// Let the implementation process the device related events
	if (s_CurrentAPI)
	{
		s_CurrentAPI->ProcessDeviceEvent(eventType, s_UnityInterfaces);
	}

	// Cleanup graphics API implementation upon shutdown
	if (eventType == kUnityGfxDeviceEventShutdown)
	{
		delete s_CurrentAPI;
		s_CurrentAPI = NULL;
		s_DeviceType = kUnityGfxRendererNull;
	}
}



// --------------------------------------------------------------------------
// OnRenderEvent
// This will be called for GL.IssuePluginEvent script calls; eventID will
// be the integer passed to IssuePluginEvent. In this example, we just ignore
// that value.

static void CopyCamPicture(void* textureDataPtr, int textureRowPitch)
{
    cap.read(frame);
    if (frame.empty()) {
        // TODO: report error
        // cerr << "ERROR: blank frame grabbed\n";
        // return -1;
    }

	int width = g_TextureWidth;
	int height = g_TextureHeight;

    const int opencv_bpp = 3;

    unsigned char* camera_data = frame.ptr();
	unsigned char* dst = (unsigned char*)textureDataPtr;
	for (int y = 0; y < height && y < frame.rows; ++y)
	{
		unsigned char* ptr = dst;
		for (int x = 0; x < width && x < frame.cols; ++x)
		{
            int pixel_index = opencv_bpp * (y * frame.cols + x);
			// Write the texture pixel
			ptr[0] = camera_data[pixel_index + 2];
			ptr[1] = camera_data[pixel_index + 1];
			ptr[2] = camera_data[pixel_index];
			ptr[3] = 255;

			// To next pixel (our pixels are 4 bpp)
			ptr += 4;
		}

		// To next image row
		dst += textureRowPitch;
	}
}

static void ModifyTexturePixels()
{
	void* textureHandle = g_TextureHandle;
	int width = g_TextureWidth;
	int height = g_TextureHeight;
	if (!textureHandle)
		return;

	int textureRowPitch;
	void* textureDataPtr = s_CurrentAPI->BeginModifyTexture(textureHandle, width, height, &textureRowPitch);
	if (!textureDataPtr)
		return;

    CopyCamPicture(textureDataPtr, textureRowPitch);

	s_CurrentAPI->EndModifyTexture(textureHandle, width, height, textureRowPitch, textureDataPtr);
}


static void UNITY_INTERFACE_API OnRenderEvent(int eventID)
{
	// Unknown / unsupported graphics device type? Do nothing
	if (s_CurrentAPI == NULL)
		return;

	ModifyTexturePixels();
}


// --------------------------------------------------------------------------
// GetRenderEventFunc, an example function we export which is used to get a rendering event callback function.

extern "C" UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API GetRenderEventFunc()
{
	return OnRenderEvent;
}

