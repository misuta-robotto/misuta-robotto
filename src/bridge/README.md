# Aldebaran C# Bridge
This library exports a subsample of the Aldebaran C++ SDK for use in C# on the Windows platform. It is designed to be lightweight, extendable and easy to use.

## Architecture
The library consist of main three separate parts in different languages*:
- Aldebaran C++ SDK
- C-like exports
- C# implementation

*The library is written in C++ and C# exclusively but uses C-like syntax for exporting functions.

### Aldebaran C++ SDK
Since the library is based upon the Aldebaran C++ SDK it must also be included in the final project composition. The Aldebaran SDK is not modified in any way but is instead used to compile and run the project.

### C-like exports
Because of restrictions in how functions are exported and imported from programs it is not possible to directly export the C++ functions which builds up the Aldebaran SDK. Therefore there exist a C-like wrapper around the Aldebaran functions, referred to as "C-like exports" (also the name of this section).

The C-like exports basically works with objects by passing around points. This have the following inpact on different functions:
- Constructors returns pointers allocated on the heap
- Member functions accepts a pointer as their first argument

**As objects are allocated on the heap they must be freed manually, therefore each constructor should have a matching function for deallocating the memory!**

### C# implementation
For the final functionality of the library it is implemented in various C# classes which import the C-like functions and call them at the correct situation in an objects life cycle. This means that the C# garbage collector automatically frees unmanaged memory via the C-like exported functions, **provided that the C# implementation is correct**.

#### Implementation guide
The following should be followed in order to guarantee a correct C# implementation.
- Every exported C++ class should have a matching C# class
- Every C# class should forward their constructor arguments to the SDK via the C-like exports
- Every C# class should store a pointer to their unmanaged memory internally
- Every C# class should call C-like exported functions using the internal pointer and the arguments that are passed to it
- Every C# class should free the unmanaged memory by overriding the Finalize method

## Configuration
### Built files
Edit `cpp/CMakeLists.txt` and append the `.cpp`-file to the definition `qi_create_lib`.

### Exported headers
Edit `cpp/naobridge.h` and modify the included headers. Make sure that they are placed below the `EXTERN` definition.

## Compiling
### Pre-conditions
In order to build the library you must first install qibuild and configure the Aldebaran C++ SDK.

1. Install CMake from [here](https://cmake.org/download/), **make sure to add it to your PATH**
2. Install Microsoft Visual Studio Express 2013 from [here](https://www.microsoft.com/en-us/download/details.aspx?id=48131)
3. Install qibuild by following the instructions [here](https://github.com/aldebaran/qibuild)
4. Create a new directory and name it to something like "SDK"
5. Execute `qibuild init` from a terminal with its current working directory set to the newly created directory.
6. Download "C++ SDK 2.5.5 Win 32 (Beta)" from [here](https://developer.softbankrobotics.com/us-en/downloads/pepper)
7. Unpack the SDK to a subfolder in your previously created directory

### Building
Building is as simple as executing the file `build.bat` in this directory and following the instructions. For the library to compile successfully you should configure the build tool to "Microsoft Visual Studio 2013" (usually option #7). Other configurations options can be defaulted by pressing enter.

**NOTE: Later versions of Visual Studio will find C3860 errors in the Aldebaran SDK.**

### Deploying to Unity
Deployment is done manually by copying **all** built dll files from `build-default/sdk/bin` to `Assets/Plugins` in your Unity project.

## Extending
Extension of new functionality can be made by following the official Aldebaran C++ SDK documentation and looking at existing implementations. The rather un-complicated nature of this library should allow for much copy-pasting through-out the code base. Make sure to follow the recommendations mentioned previously in this document to ensure correct implementation in C#.
