@ECHO OFF
"%PROGRAMFILES(x86)%\Unity\Editor\Unity.exe" -batchmode -quit -projectPath "%cd%\src\Product" -executeMethod BuildScript.PerformBuild