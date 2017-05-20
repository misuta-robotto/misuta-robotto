@ECHO OFF

cd "build/extra/cmake/bin"
set Level1=%cd%
cd ..
set Level2=%cd%
cd ..
set Level3=%cd%

set EnvPath=%Level1%;%Level2%;%Level3%
set WorkTree=%Level3%\qibuild

echo ^<qibuild version="1"^> > config.tmp
echo   ^<defaults^> >> config.tmp
echo     ^<env path="%EnvPath%" /^> >> config.tmp
echo     ^<cmake generator="Visual Studio 12 2013" /^> >> config.tmp
echo   ^</defaults^> >> config.tmp
echo   ^<worktree path="%WorkTree%"^> >> config.tmp
echo     ^<defaults /^> >> config.tmp
echo   ^</worktree^> >> config.tmp
echo ^</qibuild^> >> config.tmp
