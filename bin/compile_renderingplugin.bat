@ECHO OFF

:: NOTE: Requires Visual Studio 2015
cd "src/experiment/graphicsdemos/NativeRenderingPlugin/PluginSource/projects/VisualStudio2015/"

set MSBuildPath=
for /f "tokens=2*" %%a in ('REG QUERY "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath') do set "MSBuildPath=%%~b"
echo %MSBuildPath%

"%MSBuildPath%\MSBuild.exe" RenderingPlugin.sln & exit 0