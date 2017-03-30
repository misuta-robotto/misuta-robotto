#ifndef ALVALUE_H
#define ALVALUE_H

#include <string>
#include <vector>

EXTERN void* ALValue_f(float value);
EXTERN void* ALValue_s(char* value);
EXTERN void* ALValue_fv(float* pListFloat, int numValues);
EXTERN void* ALValue_sv(char** pListString, int numValues);
EXTERN void ALValueFree(void* alvalue);

#endif