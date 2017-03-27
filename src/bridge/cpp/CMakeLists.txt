cmake_minimum_required(VERSION 2.6.4 FATAL_ERROR)

# Set up project name and qibuild dependency
project(bridge)
find_package(qibuild)

# Add another filename before the end paranthesis to include it in the compiled library
qi_create_lib(bridge SHARED SRC alvalue.cpp almotionproxy.cpp)

# Tell CMake that bridge depends on ALCOMMON and ALPROXIES
# If you want to extend a completely different functionality in the SDK you may have to include it here
qi_use_lib(bridge ALCOMMON ALPROXIES)
