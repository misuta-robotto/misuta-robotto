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
EXTERN void AlMotionProxyFree(void* self);
EXTERN void AlMotionProxyMoveInit(void* self);
EXTERN void AlMotionProxyMove(void* self, float x, float y, float theta);
EXTERN void AlMotionProxyStopMove(void* self);
EXTERN void AlMotionProxyKillMove(void* self);

#endif
