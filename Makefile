CMAKE_URL = "https://cmake.org/files/v3.8/cmake-3.8.1-win64-x64.zip"
OPENCV_URL = "https://sourceforge.net/projects/opencvlibrary/files/opencv-win/3.2.0/opencv-3.2.0-vc14.exe/download"
UNZIP_URL = "http://stahlworks.com/dev/unzip.exe"

# When running under Git Bash we are required to not double-escape percentage tokens
ifdef HOME
	ALDEBARAN_URL = "https://developer.softbankrobotics.com/Software/C%2B%2B/2.5.5/Windows/naoqi-sdk-2.5.5.5-win32-vs2013.zip"
else
	ALDEBARAN_URL = "https://developer.softbankrobotics.com/Software/C%%2B%%2B/2.5.5/Windows/naoqi-sdk-2.5.5.5-win32-vs2013.zip"
endif

build/Product.exe: build/extra src/Product/Assets/Plugins/bridge_d.dll src/Product/Assets/Plugins/RenderingPlugin.dll
	@echo Building exectuable...
	@bin\build_executable.bat
	@bin/touch.bat "build\Product.exe"
	@echo "Executable built (found in build/Product.exe)"

src/Product/Assets/Plugins/bridge_d.dll: build/extra/cmake build/extra/qibuild/sdk
	@bin\build_qiconfig.bat
	@bin/mkdir.bat "%userprofile%\.config\qi"
	@bin/copy.bat "%userprofile%\.config\qi\qibuild.xml" "build\extra\config.bak"
	@bin/copy.bat "build\extra\config.tmp" "%userprofile%\.config\qi\qibuild.xml"
	@cd build/extra/qibuild && \
	qitoolchain create default sdk/toolchain.xml && \
	qibuild add-config default -t default --default && \
	cd ../../../src/bridge/cpp && \
	qibuild configure --work-tree "..\..\..\build\extra\qibuild" -c default && \
	qibuild make --work-tree "..\..\..\build\extra\qibuild" -c default
	@bin/copy.bat "build\extra\config.bak" "%userprofile%\.config\qi\qibuild.xml"
	@bin/mkdir.bat "src\Product\Assets\Plugins"
	@bin/copy.bat "src\bridge\cpp\build-default\sdk\bin\*" "src\Product\Assets\Plugins"
	@bin/touch.bat "src\Product\Assets\Plugins\bridge_d.dll"

build/extra/qibuild/sdk: build/extra/unzip.exe build/extra/aldebaran.zip
	@echo Extracting Aldebaran C++ SDK...
	@bin\rmdir.bat build\extra\qibuild\sdk.tmp
	@build/extra/unzip.exe build/extra/aldebaran.zip -d build/extra/qibuild/sdk.tmp
	@cmd.exe /C "move build\extra\qibuild\sdk.tmp\naoqi-sdk-2.5.5.5-win32-vs2013 build\extra\qibuild\sdk"
	@bin\rmdir.bat build\extra\qibuild\sdk.tmp

build/extra/aldebaran.zip: build/extra/qibuild/.qi
	@bin/download.bat $(ALDEBARAN_URL) build/extra/aldebaran.zip

build/extra/qibuild/.qi:
	@pip install qibuild
	@bin/mkdir.bat "build\extra\qibuild"
	@bin/rmdir.bat "build\extra\qibuild\.qi"
	@cd "build\extra\qibuild" && \
	qibuild init

src/Product/Assets/Plugins/RenderingPlugin.dll: build/extra/opencv/build_world/lib/Release/opencv_world320.lib
	@echo Building RenderingPlugin...
	@bin\compile_renderingplugin.bat
	@bin/mkdir.bat "src\Product\Assets\Plugins"
	@bin/copy.bat "src\experiment\graphicsdemos\NativeRenderingPlugin\PluginSource\build\Win32\Debug\RenderingPlugin.dll" "src\Product\Assets\Plugins"
	@bin/copy.bat "build\extra\opencv\build_world\bin\Release\opencv_world320.dll" "src\Product\Assets\Plugins"

build/extra/opencv/build_world/lib/Release/opencv_world320.lib: build/extra/opencv build/extra/cmake
	@echo Building OpenCV...
	@bin\compile_opencv.bat

build/extra/opencv: build/extra/opencv.exe
	@echo Extracting OpenCV...
	@build/extra/opencv.exe -y

build/extra/cmake: build/extra/cmake.zip build/extra/unzip.exe
	@echo Extracting CMake...
	@bin\rmdir.bat build\extra\cmake.tmp
	@build/extra/unzip.exe build/extra/cmake.zip -d build/extra/cmake.tmp/
	@cmd.exe "/C move build\extra\cmake.tmp\cmake-3.8.1-win64-x64 build\extra\cmake"
	@bin\rmdir.bat build\extra\cmake.tmp

build/extra/opencv.exe:
	@bin/download.bat $(OPENCV_URL) build/extra/opencv.exe

build/extra/cmake.zip:
	@bin/download.bat $(CMAKE_URL) build/extra/cmake.zip

build/extra/unzip.exe:
	@bin/download.bat $(UNZIP_URL) build/extra/unzip.exe

build/extra:
	@bin/mkdir.bat "build\extra"

.PHONY: clean
clean:
	@bin\rmdir.bat build
	@bin\rmdir.bat src\Product\Assets\Plugins

