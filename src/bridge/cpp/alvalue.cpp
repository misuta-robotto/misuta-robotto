#include "naobridge_impl.h"
#include <alvalue/alvalue.h>

EXTERN void* ALValue_f(const float &value)
{
    return new AL::ALValue(value);
}

EXTERN void* ALValue_s(const std::string &value)
{
    return new AL::ALValue(value);
}

EXTERN void* ALValue_fv(const std::vector<float> &pListFloat)
{
    return new AL::ALValue(pListFloat);
}

EXTERN void* ALValue_sv(const std::vector<std::string> &pListString)
{
    return new AL::ALValue(pListString);
}

EXTERN void ALValueFree(void* alvalue_)
{
    AL::ALValue* alvalue = (AL::ALValue*) alvalue;
    delete alvalue;
}
