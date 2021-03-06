@ECHO OFF

SET SDK_DIR="C:\tddd96-grupp3\SDK"
echo "Where is your SDK located? (default: %SDK_DIR%)"
echo.

SET /p NEW_DIR=">"

IF "%NEW_DIR%"=="" GOTO sdk_dir_configured

SET SDK_DIR="%NEW_DIR%"

:sdk_dir_configured

SET CURRENTDIR="%cd%"

cd "%SDK_DIR%"

qibuild config --wizard
qitoolchain create default "%SDK_DIR%\naoqi-sdk-2.5.5.5-win32-vs2013\toolchain.xml"
qibuild add-config default -t default --default

cd "%CURRENTDIR%"
cd cpp
CALL configure %SDK_DIR%
CALL build %SDK_DIR%

cd %CURRENTDIR%
