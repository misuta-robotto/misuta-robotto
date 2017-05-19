call "bin/mkdir.bat" "build/extra/opencv/build_world"
cd "build/extra/opencv/build_world"
"../../cmake/bin/cmake.exe" -G "Visual Studio 12" -DBUILD_opencv_world=1 -DBUILD_opencv_ts=0 -DWITH_IPP=0 ../sources

set MSBuildPath=
for /f "tokens=2*" %%a in ('REG QUERY "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\12.0" /v MSBuildToolsPath') do set "MSBuildPath=%%~b"
echo %MSBuildPath%

"%MSBuildPath%\MSBuild.exe" OpenCV.sln /p:Configuration=Release
