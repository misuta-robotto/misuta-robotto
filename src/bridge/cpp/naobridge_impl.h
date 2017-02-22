#include <string>

#ifdef WINBUILD
    #define EXTERN extern "C" __declspec(dllimport)
#else
    #define EXTERN extern "C"
#endif

#include "naobridge.h"