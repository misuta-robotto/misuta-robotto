@ECHO OFF

:: NOTE: Requires Visual Studio 2015
copy /Y "build\extra\opencv\build_world\lib\Release\opencv_world320.lib" "src\experiment\graphicsdemos\NativeRenderingPlugin\PluginSource\projects\VisualStudio2015\lib\x86\opencv_world320.lib"
cd "src/experiment/graphicsdemos/NativeRenderingPlugin/PluginSource/projects/VisualStudio2015/"

set MSBuildPath=
for /f "tokens=2*" %%a in ('REG QUERY "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath') do set "MSBuildPath=%%~b"
echo %MSBuildPath%

:: Exit code is surpressed as the project contains an invalid post-build script
"%MSBuildPath%\MSBuild.exe" RenderingPlugin.sln & exit 0