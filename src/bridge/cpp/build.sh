qibuild make --work-tree ~/Downloads/naoqi-sdk-2.5.5.5-mac64/ -c mytoolchain
mkdir -p out
cp build-mytoolchain/sdk/lib/libbridge.dylib out/libbridge.dylib