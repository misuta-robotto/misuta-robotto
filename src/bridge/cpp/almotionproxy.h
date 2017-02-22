#ifndef ALMOTIONPROXY_H
#define ALMOTIONPROXY_H

#include <string>

EXTERN void* ALMotionProxy(const std::string &server, int port);
EXTERN void ALMotionProxyAngleInterpolation(void* self, void* names, void* angleLists, void* timeLists, const bool& isAbsolute);
EXTERN void ALMotionProxyAngleInterpolationWithSpeed(void* self, void* names, void* targetAngles, const float& maxSpeedFraction);
EXTERN void ALMotionProxyAngleInterpolationBezier(void* self, void* jointNames, void* times, void* controlPoints);
EXTERN void ALMotionProxySetAngles(void* self, void* names, void* angles, const float& fractionMaxSpeed);
EXTERN void ALMotionProxyChangeAngles(void* self, void* names, void* changes, const float& fractionMaxSpeed);
EXTERN void* ALMotionProxyGetAngles(void* self, void* names, const bool& useSensors);
EXTERN void ALMotionProxyCloseHand(void* self, const std::string& handName);
EXTERN void ALMotionProxyOpenHand(void* self, const std::string& handName);
EXTERN void AlMotionProxyFree(void* self);

#endif