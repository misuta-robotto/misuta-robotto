#include "naobridge_impl.h"
#include <alproxies/almotionproxy.h>
#include <alvalue/alvalue.h>

#define THIS ((AL::ALMotionProxy*)self)
#define ALVALUE(x) *((AL::ALValue*) x)

void* ALMotionProxyNew(char* server_, int port)
{
    std::string server(server_);
    return new AL::ALMotionProxy(server, port);
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

void* ALMotionProxyGetRobotPosition(void* self, bool useSensors)
{
    std::vector<float> robotPosition = THIS->getRobotPosition(useSensors);
    std::vector<float>* positionsPointer = new std::vector<float>(robotPosition);

    return positionsPointer;
}

void ALMotionProxyStopMove(void* self)
{
    THIS->stopMove();
}

void ALMotionProxyKillMove(void* self)
{
    THIS->killMove();
}
