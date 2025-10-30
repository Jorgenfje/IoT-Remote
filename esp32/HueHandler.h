#ifndef HUE_HANDLER_H
#define HUE_HANDLER_H

#include <vector>
#include <Arduino_JSON.h>

void getHueID();
void lightsDefault();
void fetchAndToggleAllLights();
void toggleDeviceState();

#endif
