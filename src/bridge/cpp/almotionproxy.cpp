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
#include <alproxies/almotionproxy.h>
#include <alvalue/alvalue.h>
#include <algorithm>
#include <alerror/alerror.h>

#define THIS ((AL::ALMotionProxy*)self)
#define ALVALUE(x) *((AL::ALValue*) x)

void* ALMotionProxyNew(char* server_, int port)
{
    std::string server(server_);
	try
	{
		return new AL::ALMotionProxy(server, port);
	}
	catch (const AL::ALError &e)
	{
		return NULL;
	}
}

void ALMotionProxyAngleInterpolation(void* self, void* names, void* angleLists, void* timeLists, bool isAbsolute)
{
    THIS->angleInterpolation(ALVALUE(names), ALVALUE(angleLists), ALVALUE(timeLists), isAbsolute);
}

void ALMotionProxyAngleInterpolationWithSpeed(void* self, void* names, void* targetAngles, float maxSpeedFraction)
{
    THIS->angleInterpolationWithSpeed(ALVALUE(names), ALVALUE(targetAngles), maxSpeedFraction);
}

void ALMotionProxyAngleInterpolationBezier(void* self, void* jointNames, void* times, void* controlPoints)
{
    THIS->angleInterpolationBezier(ALVALUE(jointNames), ALVALUE(times), ALVALUE(controlPoints));
}

void ALMotionProxySetAngles(void* self, void* names, void* angles, float fractionMaxSpeed)
{
    THIS->setAngles(ALVALUE(names), ALVALUE(angles), fractionMaxSpeed);
}

void ALMotionProxyChangeAngles(void* self, void* names, void* changes, float fractionMaxSpeed)
{
    THIS->changeAngles(ALVALUE(names), ALVALUE(changes), fractionMaxSpeed);
}

void* ALMotionProxyGetAngles(void* self, void* names, bool useSensors)
{
    std::vector<float> angles = THIS->getAngles(ALVALUE(names), useSensors);
	std::vector<float>* anglesPointer = new std::vector<float>(angles);

    return &anglesPointer[0];
}

void ALMotionProxyCloseHand(void* self, char* handName_)
{
    std::string handName(handName_);
    THIS->closeHand(handName);
}

void ALMotionProxyOpenHand(void* self, char* handName_)
{
    std::string handName(handName_);
    THIS->openHand(handName);
}

void ALMotionProxyFree(void* self)
{
    delete THIS;
}

void ALMotionProxyMoveInit(void* self)
{
    THIS->moveInit();
}

void ALMotionProxyMove(void* self, float x, float y, float theta)
{
    THIS->move(x, y, theta);
}

void ALMotionProxyMoveTo(void* self, float x, float y, float theta)
{
    THIS->moveTo(x, y, theta);
}

void ALMotionProxyMoveToAsync(void* self, float x, float y, float theta)
{
    THIS->post.moveTo(x, y, theta);
}

void ALMotionProxyGetRobotPosition(void* self, bool useSensors, float* buffer)
{
    std::vector<float> robotPosition = THIS->getRobotPosition(useSensors);
    std::copy(robotPosition.begin(), robotPosition.end(), buffer);
}

void ALMotionProxyStopMove(void* self)
{
    THIS->stopMove();
}

void ALMotionProxyKillMove(void* self)
{
    THIS->killMove();
}
