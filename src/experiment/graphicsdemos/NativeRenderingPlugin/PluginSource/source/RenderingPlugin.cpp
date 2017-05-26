// Example low level rendering Unity plugin

#include "PlatformBase.h"
#include "RenderAPI.h"

#include <assert.h>
#include <math.h>
#include <vector>
#include <mutex>

#include <opencv2/opencv.hpp>

// --------------------------------------------------------------------------
// SetTextureFromUnity, an example function we export which is called by one of the scripts.

static void* g_TextureHandle = NULL;
static int   g_TextureWidth  = 0;
static int   g_TextureHeight = 0;

// Variables and data used to perform camera feed capture on another thread.
// Please note that access to this buffer is not synchronized as it does not have
// serious consequences despite being accessed from multiple threads simultaniously.
int textureRowPitch;
int bufferSize;
static void* cameraDataBuffer;
static void* writeBuffer;
std::mutex bufferMutex;

// --------------------------------------------------------------------------
// SetTimeFromUnity, an example function we export which is called by one of the scripts.

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetDevice(int dev);

static cv::Mat tempFrame;
static cv::Mat frame;
static cv::VideoCapture cap;
static int device_count = 0;

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API InitOpenCV()
{
	SetDevice(0);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API FreeResources()
{
	cap.release();

	bufferMutex.lock();
		free(cameraDataBuffer);
		free(writeBuffer);

		cameraDataBuffer = NULL;
		writeBuffer = NULL;
	bufferMutex.unlock();
}

extern "C" int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API GetDeviceCount()
{
	return device_count;
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetDevice(int dev)
{
	cap.open(dev);
	cap.set(cv::CAP_PROP_FRAME_WIDTH, g_TextureWidth);
	cap.set(cv::CAP_PROP_FRAME_HEIGHT, g_TextureHeight);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API SetTextureFromUnity(void* textureHandle, int w, int h)
{
	// A script calls this at initialization time; just remember the texture pointer here.
	// Will update texture pixels each frame from the plugin rendering event (texture update
	// needs to happen on the rendering thread).
	g_TextureHandle = textureHandle;
	g_TextureWidth = w;
	g_TextureHeight = h;

	// The camera data seems to consist of h rows, each with w pixels containing RGBA data (4 bytes).
	// The above comment is assumed and may not be true, but it works at the moment.
	textureRowPitch = w * 4;
	bufferSize = textureRowPitch * h;
	cameraDataBuffer = malloc(bufferSize);
	writeBuffer = malloc(bufferSize);
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

/*
static void ReprojectImage()
{
	

	cv::undi

	cvInitUndistortMap(intrinsics, )

	cvCreateImage(cv::GetSize(frame.data), frame.depth, frame.channels);
	
}
*/

static void ReprojectImage() 
{
	/*

	[
		674.2915897025306, 0, 999.5024387021914;
		0, 676.5783895711185, 516.4438814568073;
		0, 0, 1
	]


	[
		0.06994405576339403;
		-0.2661662886285399;
		0.8183055256305204;
		-0.8159052611368692
	]
	
	*/

	double fx = 674.2915897025306;
	double fy = 676.5783895711185;
	double cx = 999.5024387021914;
	double cy = 516.4438814568073;

	double k1 = 0.06994405576339403;
	double k2 = -0.2661662886285399;
	double k3 = 0.8183055256305204;
	double k4 = -0.8159052611368692;

	cv::Mat K = cv::Mat::zeros(3, 3, CV_64FC1);
	K.at<double>(0, 0) = fx;
	K.at<double>(1, 1) = fy;
	K.at<double>(2, 2) = 1.0;
	K.at<double>(0, 2) = cx;
	K.at<double>(1, 2) = cy;

	cv::Mat D = cv::Mat::zeros(1, 4, CV_64FC1);
	D.at<double>(0) = k1;
	D.at<double>(1) = k2;
	D.at<double>(2) = k3;
	D.at<double>(3) = k4;

	cv::Mat output;
	cv::Mat newK;
	cv::Mat view, map1, map2;

	cv::Size newSize(1920, 1080);
	cv::Mat rview(newSize, frame.type());
	//resize(rview, rview, newSize);

	cv::fisheye::estimateNewCameraMatrixForUndistortRectify(K, D, frame.size(), cv::Matx33d::eye(), newK, 1);

	cv::fisheye::initUndistortRectifyMap(K, D, cv::Matx33d::eye(), newK, frame.size(), CV_16SC2, map1, map2);
	
	remap(frame, rview, map1, map2, cv::INTER_LINEAR);
	frame = rview;
}


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

static void ReadCamPictureToBuffer(void* textureDataPtr, int textureRowPitch)
{
	bool success = cap.read(frame);
	if (success)
	{
		ReprojectImage();
	}
	else {
		return;
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

	// Copy camera data from buffer to achieve minimal execution time.
	int localTextureRowPitch;
	void* textureDataPtr = s_CurrentAPI->BeginModifyTexture(textureHandle, width, height, &localTextureRowPitch);
	if (!textureDataPtr)
		return;

	bufferMutex.lock();
		if (cameraDataBuffer != NULL)
		{
			memcpy(textureDataPtr, cameraDataBuffer, bufferSize);
		}
	bufferMutex.unlock();

	s_CurrentAPI->EndModifyTexture(textureHandle, width, height, localTextureRowPitch, textureDataPtr);
}


// NOTE: This function is executed on the rendering thread and MUST terminate as quickly as possible!
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

// Used to update the camera data buffer.
// Call this function as often as possible from a non-blocking thread in Unity.
extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API ReadFromCamera()
{
	ReadCamPictureToBuffer(writeBuffer, textureRowPitch);

	bufferMutex.lock();
		void* temp = cameraDataBuffer;
		cameraDataBuffer = writeBuffer;
		writeBuffer = temp;
	bufferMutex.unlock();
}

