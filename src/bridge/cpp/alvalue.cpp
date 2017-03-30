#include "naobridge_impl.h"
#include <alvalue/alvalue.h>

#include <stdio.h>

void* ALValue_f(float value)
{
    return new AL::ALValue(value);
}

void* ALValue_s(char* value)
{
	std::string value_s(value);
    return new AL::ALValue(value_s);
}

void* ALValue_fv(float* pListFloat, int numValues)
{
	std::vector<float> values;
	for (int i = 0; i < numValues; ++i)
	{
		values.push_back(pListFloat[i]);
	}
    return new AL::ALValue(values);
}

void* ALValue_sv(char** pListString, int numValues)
{
	std::vector<std::string> values;
	for (int i = 0; i < numValues; ++i)
	{
		values.push_back(std::string(pListString[i]));
	}
    return new AL::ALValue(values);
}

void ALValueFree(void* alvalue_)
{
    AL::ALValue* alvalue = (AL::ALValue*) alvalue_;
    delete alvalue;
}
