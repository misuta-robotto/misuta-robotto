# Compiling
This project consist of many different dependencies and multiple build systems. In order to ease setting up a development environment and building the product, the compilation process has been condensed to a single automated process.

## Required tools
Most of the dependencies and tools needed to compile the project are automatically downloaded during the build process, but there are some exceptions. In order to produce minimal footprint on the host operating system the build system is designed to not install any software, and can thus only utilize tools without installers. The required software which must be installed beforehand is listed below:

* GNU/Make for Windows
* Microsoft Visual Studio 2013
* Microsoft Visual Studio 2015
* Python 2.7 with pip (32-bit)
* Unity Editor (32-bit)

**NOTE:** The project can only be built for Windows, using Windows. GNU/Make and Python 2.7 (including pip) must be available on the path, this includes the binaries: make.exe, python.exe and pip.exe. Unity Editor must be installed in the default install location and authenticated with an account.

## How to build
Building is as simple as running the command "make" from either cmd (Windows Command Prompt) or Git Bash with the working directory set to the project root. **Building from another shell is untested and may not work correctly.**

### Building the application
Building the application for the first time is a time consuming task and will require at least 20 minutes. **IMPORTANT: Do not build the application with Unity running.**
```bash
make
```

There is an error in one of the final build steps and it will look like the build failed. This is completely normal and the build should still complete successfully with the message "Executable built (found in build/Product.exe)".

### Cleaning the application
If you need to reset the build environment you should use the command below, which will ensure that all built Unity plugins are removed. **This will require a re-download of all dependencies.**
```bash
make clean
```

## Development
With the project built you are also up and running for continued development. The project should appear in the recent projects list in the Unity Editor, otherwise it may be opened manually.

# Running
After compiling the project an executable will be created at `build/Product.exe`. This executable starts the application and must be distributed together with the generated folder `build/Product_Data`.
