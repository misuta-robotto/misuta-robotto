#ifndef ALVALUE_H
#define ALVALUE_H

#include <string>
#include <vector>

EXTERN void* ALValue_f(const float &value);
EXTERN void* ALValue_s(const std::string &value);
EXTERN void* ALValue_fv(const std::vector<float> &pListFloat);
EXTERN void* ALValue_sv(const std::vector<std::string> &pListString);
EXTERN void ALValueFree(void* alvalue);

#endif