// cv::Mat to Texture | Jan 2012 | r3dux
// Library dependencies: OpenCV core, video and highgui libs, GL, glew, glfw
// We do NOT need to link in libOpenNI.so (it comes with building OpenCV with OpenNI support)
// Kinect with OpenCV usage guide: http://opencv.itseez.com/doc/user_guide/ug_highgui.html

#include <iostream>
using std::cout;
using std::endl;

extern "C" { FILE __iob_func[3] = { *stdin,*stdout,*stderr }; }

#include "opencv.hpp"
#include "glew.h"
#include "glfw.h"

GLint   windowWidth  = 1920;     // Define our window width
GLint   windowHeight = 1080;     // Define our window height
GLfloat fieldOfView  = 45.0f;   // FoV
GLfloat zNear        = 0.1f;    // Near clip plane
GLfloat zFar         = 200.0f;  // Far clip plane

// Frame counting and limiting
int    frameCount = 0;
double frameStartTime, frameEndTime, frameDrawTime;

bool quit = false;

// Function turn a cv::Mat into a texture, and return the texture ID as a GLuint for use
GLuint matToTexture(cv::Mat &mat, GLenum minFilter, GLenum magFilter, GLenum wrapFilter)
{
	// Generate a number for our textureID's unique handle
	GLuint textureID;
	glGenTextures(1, &textureID);

	// Bind to our texture handle
	glBindTexture(GL_TEXTURE_2D, textureID);

	// Catch silly-mistake texture interpolation method for magnification
	if (magFilter == GL_LINEAR_MIPMAP_LINEAR  ||
	    magFilter == GL_LINEAR_MIPMAP_NEAREST ||
	    magFilter == GL_NEAREST_MIPMAP_LINEAR ||
	    magFilter == GL_NEAREST_MIPMAP_NEAREST)
	{
		cout << "You can't use MIPMAPs for magnification - setting filter to GL_LINEAR" << endl;
		magFilter = GL_LINEAR;
	}

	// Set texture interpolation methods for minification and magnification
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, minFilter);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, magFilter);

	// Set texture clamping method
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapFilter);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapFilter);

	// Set incoming texture format to:
	// GL_BGR       for CV_CAP_OPENNI_BGR_IMAGE,
	// GL_LUMINANCE for CV_CAP_OPENNI_DISPARITY_MAP,
	// Work out other mappings as required ( there's a list in comments in main() )
	GLenum inputColourFormat = GL_BGR;
	if (mat.channels() == 1)
	{
		inputColourFormat = GL_LUMINANCE;
	}

	// Create the texture
	glTexImage2D(GL_TEXTURE_2D,     // Type of texture
	             0,                 // Pyramid level (for mip-mapping) - 0 is the top level
	             GL_RGB,            // Internal colour format to convert to
	             mat.cols,          // Image width  i.e. 640 for Kinect in standard mode
	             mat.rows,          // Image height i.e. 480 for Kinect in standard mode
	             0,                 // Border width in pixels (can either be 1 or 0)
	             inputColourFormat, // Input image format (i.e. GL_RGB, GL_RGBA, GL_BGR etc.)
	             GL_UNSIGNED_BYTE,  // Image data type
	             mat.ptr());        // The actual image data itself

	// If we're using mipmaps then generate them. Note: This requires OpenGL 3.0 or higher
	if (minFilter == GL_LINEAR_MIPMAP_LINEAR  ||
	    minFilter == GL_LINEAR_MIPMAP_NEAREST ||
	    minFilter == GL_NEAREST_MIPMAP_LINEAR ||
	    minFilter == GL_NEAREST_MIPMAP_NEAREST)
	{
		glGenerateMipmap(GL_TEXTURE_2D);
	}

	return textureID;
}

void draw(cv::Mat &camFrame)
{
	// Clear the screen and depth buffer, and reset the ModelView matrix to identity
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();

	// Move things back into the screen
	glTranslatef(0.0f, 0.0f, -8.0f);

	// Rotate around the y-axis
	glRotatef(180, 0.0f, 1.0f, 0.0f);

	// Rotate around the x-axis
	static float rateOfChange = 0.01f;
	static float degreesToMoveThrough = 180.0f;
	//glRotatef(sin(frameCount * rateOfChange) * degreesToMoveThrough, 1.0f, 0.0f, 0.0f);

	// Rotate around the z-axis
	//glRotatef(cos(frameCount * rateOfChange) * degreesToMoveThrough, 0.0f, 1.0f, 0.0f);
	glRotatef(180, 0.0f, 0.0f, 1.0f);

	glEnable(GL_TEXTURE_2D);

	// Quad width and height
	float w = 6.4f;
	float h = 4.8f;

	// Convert image and depth data to OpenGL textures
	GLuint imageTex = matToTexture(camFrame,   GL_LINEAR_MIPMAP_LINEAR,   GL_LINEAR, GL_CLAMP);

	// Draw the textures
	// Note: Window co-ordinates origin is top left, texture co-ordinate origin is bottom left.

	// Front facing texture
	glBindTexture(GL_TEXTURE_2D, imageTex);
	glBegin(GL_QUADS);
		glTexCoord2f(1, 1);
		glVertex2f(-w/2,  h/2);
		glTexCoord2f(0, 1);
		glVertex2f( w/2,  h/2);
		glTexCoord2f(0, 0);
		glVertex2f( w/2, -h/2);
		glTexCoord2f(1, 0);
		glVertex2f(-w/2, -h/2);
	glEnd();

	// Free the texture memory
	glDeleteTextures(1, &imageTex);

	glDisable(GL_TEXTURE_2D);
}

void handleKeypress(int theKey, int theAction)
{
	// If a key was pressed...
	if (theAction == GLFW_PRESS)
	{
		// ...act accordingly dependant on what key it was!
		switch (theKey)
		{
			case GLFW_KEY_ESC:
				quit = true;
				break;

			default:
				break;

		} // End of switch statement

	} // End of GLFW_PRESS
}

void initGL()
{
	// Define our buffer settings
	int redBits    = 8, greenBits = 8,  blueBits    = 8;
	int alphaBits  = 8, depthBits = 24, stencilBits = 8;

	// Initialise glfw
	glfwInit();

	// Create a window
	if(!glfwOpenWindow(windowWidth, windowHeight, redBits, greenBits, blueBits, alphaBits, depthBits, stencilBits, GLFW_WINDOW))
	{
		cout << "Failed to open window!" << endl;
		glfwTerminate();
		exit(-1);
	}

	glfwSetWindowTitle("OpenCV/OpenNI Sensor Data to Texture | r3dux");

	// Specify the callback function for key presses/releases
	glfwSetKeyCallback(handleKeypress);

	//  Initialise glew (must occur AFTER window creation or glew will error)
	GLenum err = glewInit();
	if (GLEW_OK != err)
	{
		cout << "GLEW initialisation error: " << glewGetErrorString(err) << endl;
		exit(-1);
	}
	cout << "GLEW okay - using version: " << glewGetString(GLEW_VERSION) << endl;

	// Setup our viewport to be the entire size of the window
	glViewport(0, 0, (GLsizei)windowWidth, (GLsizei)windowHeight);

	// Change to the projection matrix and set our viewing volume
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	// The following code is a fancy bit of math that is equivilant to calling:
	// gluPerspective(fieldOfView/2.0f, width/height , near, far);
	// We do it this way simply to avoid requiring glu.h
	GLfloat aspectRatio = (windowWidth > windowHeight)? float(windowWidth)/float(windowHeight) : float(windowHeight)/float(windowWidth);
	GLfloat fH = tan( float(fieldOfView / 360.0f * 3.14159f) ) * zNear;
	GLfloat fW = fH * aspectRatio;
	glFrustum(-fW, fW, -fH, fH, zNear, zFar);

	// ----- OpenGL settings -----

	glDepthFunc(GL_LEQUAL);		// Specify depth function to use

	glEnable(GL_DEPTH_TEST);    // Enable the depth buffer

	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST); // Ask for nicest perspective correction

	glEnable(GL_CULL_FACE);     // Cull back facing polygons

	glfwSwapInterval(1);        // Lock screen updates to vertical refresh

	// Switch to ModelView matrix and reset
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Set our clear colour to black
}

void lockFramerate(double framerate)
{
	// Note: frameStartTime is called first thing in the main loop

	// Our allowed frame time is 1 second divided by the desired FPS
	static double allowedFrameTime = 1.0 / framerate;

	// Get current time
	frameEndTime = glfwGetTime();

	// Calc frame draw time
	frameDrawTime = frameEndTime - frameStartTime;

	double sleepTime = 0.0;

	// Sleep if we've got time to kill before the next frame
	if (frameDrawTime < allowedFrameTime)
	{
		sleepTime = allowedFrameTime - frameDrawTime;
		glfwSleep(sleepTime);
	}

	// Debug stuff
	double potentialFPS = 1.0 / frameDrawTime;
	double lockedFPS    = 1.0 / (glfwGetTime() - frameStartTime);
	cout << "Draw: " << frameDrawTime << " Sleep: " << sleepTime;
	cout << " Pot. FPS: " << potentialFPS << " Locked FPS: " << lockedFPS << endl;
}

int main()
{
	// Set up our OpenGL window, projection and options
	initGL();

	// Create a our video capture using the Kinect and OpenNI
	// Note: To use the cv::VideoCapture class you MUST link in the highgui lib (libopencv_highgui.so)
	cout << "Opening Kinect device ..." << endl;
	cv::VideoCapture capture( 0 );
	capture.set(CV_CAP_PROP_BUFFERSIZE, 3);

	// Set sensor to 640x480@30Hz mode as opposed to 1024x768@15Hz mode (which is available for image sensor only!)
	// Note: CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE = CV_CAP_OPENNI_IMAGE_GENERATOR + CV_CAP_PROP_OPENNI_OUTPUT_MODE
	//capture.set( CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE, CV_CAP_OPENNI_VGA_30HZ ); // default
	cout << "done." << endl;

	// Check that we have actually opened a connection to the sensor
	if( !capture.isOpened() )
	{
		cout << "Can not open a capture object." << endl;
		return -1;
	}

	// Create our cv::Mat objects
	cv::Mat camFrame;

	do
	{
		frameStartTime = glfwGetTime(); // Grab the time at the beginning of the frame

		// Grab a frame from the sensor
		// Correct procedure is to grab once per frame, then retrieve as many fields as required.
		// *****************************************************************************************
		// IMPORTANT NOTE: There appears to be a threading issue with the OpenCV grab() function
		// where if you try to grab the device before it's ready to provide the next frame it takes
		// up to 2 seconds to provide the frame, which it might do for a little while before crashing
		// the XnSensorServer process & then you can't get any more frames without restarting the
		// application. This results in horrible, stuttery framerates and garbled sensor data.
		//
		// I've found that this can be worked around by playing an mp3 in the background. No, really.
		// I'm guessing the threading of the mp3 player introduces some kind of latency which
		// prevents the grab() function being called too soon. Try it if you don't believe me!
		//
		// Config: Linux x64 LMDE, Kernel 3.1.0-5.dmz.1-liquorix-amd64, Nvidia 290.10 drivers,
		// OpenCV 2.3.2 (from git, built without TBB [same occurs with!]), openni-bin-x64-v1.5.2.23,
		// avin2-SensorKinect-git-unstable-branch-2011-01-04, NITE-bin-unstable-x64-v1.5.2.21.
		//******************************************************************************************
		if (!capture.grab())
		{
			cout << "Could not grab kinect... Skipping frame." << endl;
		}
		else
		{
			/*
			Frame retrieval formats:
				data given from depth generator:
					OPENNI_DEPTH_MAP         - depth values in mm (CV_16UC1)
					OPENNI_POINT_CLOUD_MAP   - XYZ in meters (CV_32FC3)
					OPENNI_DISPARITY_MAP     - disparity in pixels (CV_8UC1)
					OPENNI_DISPARITY_MAP_32F - disparity in pixels (CV_32FC1)
					OPENNI_VALID_DEPTH_MASK  - mask of valid pixels (not occluded, not shaded etc.) (CV_8UC1)

				data given from RGB image generator:
					OPENNI_BGR_IMAGE - color image (CV_8UC3)
					OPENNI_GRAY_IMAGE - gray image (CV_8UC1)
			*/

			// Retrieve desired sensor data
			capture.retrieve(camFrame, 0);
			//capture.retrieve(depthFrame, CV_CAP_OPENNI_DISPARITY_MAP);

			// Draw texture contents
			draw(camFrame);

			// Swap the active and visual pages
			glfwSwapBuffers();
		}

		// Quit out if the OpenGL window was closed
		if ( !glfwGetWindowParam( GLFW_OPENED) )
		{
			quit = true;
		}

		frameCount++;

		// Lock our main loop to 30fps
		lockFramerate(60.0);

		//if( cv::waitKey( 30 ) >= 0 )
		//break;

	} while (quit == false);

	capture.release();

	glfwTerminate();

	return 0;
}
