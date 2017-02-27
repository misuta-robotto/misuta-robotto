##
# Copyright (c) 2011 Aldebaran Robotics. All Rights Reserved.

cmake_minimum_required(VERSION 2.6.4 FATAL_ERROR)
# Give a name to the project.
project(bridge)
# This include enable you to use our CMake framework
find_package(qibuild)

# Create an executable named movehead,
# with the source file : movehead.cpp
qi_create_lib(bridge SHARED SRC alvalue.cpp almotionproxy.cpp)

# Tell CMake that movehead depends on ALCOMMON and ALPROXIES
# This will set the libraries to link movehead with,
# the include paths, and so on
qi_use_lib(bridge ALCOMMON ALPROXIES)
