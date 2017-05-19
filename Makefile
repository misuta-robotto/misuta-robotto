CMAKE_URL = "https://cmake.org/files/v3.8/cmake-3.8.1-win64-x64.zip"
OPENCV_URL = "https://sourceforge.net/projects/opencvlibrary/files/opencv-win/3.2.0/opencv-3.2.0-vc14.exe/download"
UNZIP_URL = "http://stahlworks.com/dev/unzip.exe"

product: folders opencv;

.PHONY: opencv
opencv: build/extra/opencv/build_world/lib/Release/opencv_world320.lib;

build/extra/opencv/build_world/lib/Release/opencv_world320.lib: build/extra/opencv build/extra/cmake
	@echo Building OpenCV...
	@bin\compile_opencv.bat

build/extra/opencv: build/extra/opencv.exe
	@echo Extracting OpenCV...
	@build/extra/opencv.exe -y

build/extra/cmake: build/extra/cmake.zip build/extra/unzip.exe
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

