/*
Copyright (c) 2017, Misuta Robotto Group

The contents of this file are subject to the Common Public Attribution License Version 1.0 (the “License”); 
you may not use this file except in compliance with the License. You may obtain a copy of the License at

    https://github.com/Emiluren/misuta-robotto/blob/master/LICENSE.md
    
The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover
use of software over a computer network and provide for limited attribution for the Original Developer. In 
addition, Exhibit A has been modified to be consistent with Exhibit B.

Software distributed under the License is distributed on an “AS IS” basis, WITHOUT WARRANTY OF ANY KIND, 
either express or implied. See the License  for the specific language governing rights and limitations 
under the License.

The Original Code is Misuta Robotto.

The Initial Developer of the Original Code is Misuta Robotto Group. 
All portions of the code written by Misuta Robotto Group are Copyright (c) 2017. All Rights Reserved.

Misuta Robotto Group includes Robin Christensen, Jacob Lundberg, Ylva Lundegård, Emil Segerbäck,
Patrik Sletmo, Teo Tiefenbacher, Jon Vik and David Wajngot.
*/

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
