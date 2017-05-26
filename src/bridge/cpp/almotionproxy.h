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

#ifndef ALMOTIONPROXY_H
#define ALMOTIONPROXY_H

#include <string>

EXTERN void* ALMotionProxyNew(char* server, int port);
EXTERN void ALMotionProxyAngleInterpolation(void* self, void* names, void* angleLists, void* timeLists, bool isAbsolute);
EXTERN void ALMotionProxyAngleInterpolationWithSpeed(void* self, void* names, void* targetAngles, float maxSpeedFraction);
EXTERN void ALMotionProxyAngleInterpolationBezier(void* self, void* jointNames, void* times, void* controlPoints);
EXTERN void ALMotionProxySetAngles(void* self, void* names, void* angles, float fractionMaxSpeed);
EXTERN void ALMotionProxyChangeAngles(void* self, void* names, void* changes, float fractionMaxSpeed);
EXTERN void* ALMotionProxyGetAngles(void* self, void* names, bool useSensors);
EXTERN void ALMotionProxyCloseHand(void* self, char* handName);
EXTERN void ALMotionProxyOpenHand(void* self, char* handName);
EXTERN void ALMotionProxyFree(void* self);
EXTERN void ALMotionProxyMoveInit(void* self);
EXTERN void ALMotionProxyMove(void* self, float x, float y, float theta);
EXTERN void ALMotionProxyMoveTo(void* self, float x, float y, float theta);
EXTERN void ALMotionProxyMoveToAsync(void* self, float x, float y, float theta);
EXTERN void ALMotionProxyGetRobotPosition(void* self, bool useSensors, float* buffer);
EXTERN void ALMotionProxyStopMove(void* self);
EXTERN void ALMotionProxyKillMove(void* self);

#endif
