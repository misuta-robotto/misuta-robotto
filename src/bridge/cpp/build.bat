qibuild make --work-tree "%1%" -c default
mkdir out
copy "build-default\sdk\bin\bridge_d.dll" "out\bridge.dll"
