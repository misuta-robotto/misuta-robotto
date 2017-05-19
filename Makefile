CMAKE_URL = "https://cmake.org/files/v3.8/cmake-3.8.1-win64-x64.zip"
OPENCV_URL = "https://sourceforge.net/projects/opencvlibrary/files/opencv-win/3.2.0/opencv-3.2.0-vc14.exe/download"
UNZIP_URL = "http://stahlworks.com/dev/unzip.exe"
ALDEBARAN_URL = "https://developer.softbankrobotics.com/Software/C%2B%2B/2.5.5/Windows/naoqi-sdk-2.5.5.5-win32-vs2013.zip"

product: folders src/experiment/graphicsdemos/NativeRenderingPlugin/PluginSource/build/Win32/Debug/RenderingPlugin.dll build/extra/qibuild/sdk;

build/extra/qibuild/sdk: build/extra/aldebaran.zip
	@echo Extracting Aldebaran C++ SDK...
	@build/extra/unzip.exe build/extra/aldebaran.zip -d build/extra/qibuild/sdk.tmp
	@mv build/extra/qibuild/sdk.tmp/* build/extra/qibuild/sdk
	@bin\rmdir.bat build\extra\qibuild\sdk.tmp

build/extra/aldebaran.zip: build\extra\qibuild\.qi
	@bin/download.bat $(ALDEBARAN_URL) build/extra/aldebaran.zip

build\extra\qibuild\.qi:
	@pip install qibuild
	@bin/mkdir.bat "build\extra\qibuild"
	@bin/rmdir.bat "build\extra\qibuild\.qi"
	@cd "build\extra\qibuild"; \
	qibuild init;

src/experiment/graphicsdemos/NativeRenderingPlugin/PluginSource/build/Win32/Debug/RenderingPlugin.dll: build/extra/opencv/build_world/lib/Release/opencv_world320.lib
	@echo Building RenderingPlugin...
	@bin\compile_renderingplugin.bat

build/extra/opencv/build_world/lib/Release/opencv_world320.lib: build/extra/opencv build/extra/cmake
	@echo Building OpenCV...
	@bin\compile_opencv.bat

build/extra/opencv: build/extra/opencv.exe
	@echo Extracting OpenCV...
	@build/extra/opencv.exe -y

build/extra/cmake: build/extra/cmake.zip build/extra/unzip.exe
	@echo Extracting CMake...
	@build/extra/unzip.exe build/extra/cmake.zip -d build/extra/cmake.tmp/
	@mv build/extra/cmake.tmp/* build/extra/cmake
	@bin\rmdir.bat build\extra\cmake.tmp

build/extra/opencv.exe:
	@bin/download.bat $(OPENCV_URL) build/extra/opencv.exe

build/extra/cmake.zip:
	@bin/download.bat $(CMAKE_URL) build/extra/cmake.zip

build/extra/unzip.exe:
	@bin/download.bat $(UNZIP_URL) build/extra/unzip.exe

.PHONY: folders
folders:
	@bin/mkdir.bat "build\extra"

.PHONY: clean
clean:
	@bin\rmdir.bat build

