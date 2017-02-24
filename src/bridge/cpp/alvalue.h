#ifndef ALVALUE_H
#define ALVALUE_H

#include <string>
#include <vector>

EXTERN void* ALValue_f(float value);
EXTERN void* ALValue_s(char* value);
EXTERN void* ALValue_fv(std::vector<float> pListFloat);
EXTERN void* ALValue_sv(std::vector<char*> pListString);
EXTERN void ALValueFree(void* alvalue);

#endif