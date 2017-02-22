#include "naobridge_impl.h"
#include <alproxies/almotionproxy.h>
#include <alvalue/alvalue.h>

#define THIS ((AL::ALMotionProxy*)self)
#define ALVALUE(x) *((AL::ALValue*) x)

EXTERN void* ALMotionProxy(const std::string &server, int port)
{
    return new AL::ALMotionProxy(server, port);
}

EXTERN void ALMotionProxyAngleInterpolation(void* self, void* names, void* angleLists, void* timeLists, const bool& isAbsolute)
{
    THIS->angleInterpolation(ALVALUE(names), ALVALUE(angleLists), ALVALUE(timeLists), isAbsolute);
}

EXTERN void ALMotionProxyAngleInterpolationWithSpeed(void* self, void* names, void* targetAngles, const float& maxSpeedFraction)
{
    THIS->angleInterpolationWithSpeed(ALVALUE(names), ALVALUE(targetAngles), maxSpeedFraction);
}

EXTERN void ALMotionProxyAngleInterpolationBezier(void* self, void* jointNames, void* times, void* controlPoints)
{
    THIS->angleInterpolationBezier(ALVALUE(jointNames), ALVALUE(times), ALVALUE(controlPoints));
}

EXTERN void ALMotionProxySetAngles(void* self, void* names, void* angles, const float& fractionMaxSpeed)
{
    THIS->setAngles(ALVALUE(names), ALVALUE(angles), fractionMaxSpeed);
}

EXTERN void ALMotionProxyChangeAngles(void* self, void* names, void* changes, const float& fractionMaxSpeed)
{
    THIS->changeAngles(ALVALUE(names), ALVALUE(changes), fractionMaxSpeed);
}

EXTERN void* ALMotionProxyGetAngles(void* self, void* names, const bool& useSensors)
{
    // TODO: Implement
    return 0;
}

EXTERN void ALMotionProxyCloseHand(void* self, const std::string& handName)
{
    THIS->closeHand(handName);
}

EXTERN void ALMotionProxyOpenHand(void* self, const std::string& handName)
{
    THIS->openHand(handName);
}

EXTERN void AlMotionProxyFree(void* self)
{
    delete THIS;
}
