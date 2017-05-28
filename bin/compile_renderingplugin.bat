@ECHO OFF

:: NOTE: Requires Visual Studio 2013
call "bin/mkdir.bat" "src\RenderingPlugin\projects\VisualStudio2015\lib\x86\"
copy /Y "build\extra\opencv\build_world\lib\Release\opencv_world320.lib" "src\RenderingPlugin\projects\VisualStudio2015\lib\x86\opencv_world320.lib"
cd "src/RenderingPlugin/projects/VisualStudio2015/"

set MSBuildPath=
for /f "tokens=2*" %%a in ('REG QUERY "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\12.0" /v MSBuildToolsPath') do set "MSBuildPath=%%~b"
echo %MSBuildPath%

"%MSBuildPath%\MSBuild.exe" RenderingPlugin.sln