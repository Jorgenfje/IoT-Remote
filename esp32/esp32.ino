#include <Arduino.h>
#include "NetHandler.h"
#include "HueHandler.h"
#include "InputHandler.h"

void setup() {
  Serial.begin(115200);

  inputSetup(); // Setting up inputs from InputHandler
  initWiFi(); // Connect to Wi-Fi from WiFiHandler

  getHueID(); // Get Hue Lights status from HueHandler 

  serverStart(); // Start Async server from ServerHandler
  serverHTML(); // Serve HTML
  fetchAndToggleAllLights();

}

void loop() {
  serverCleanup();

  if (button1Pressed()){
    fetchAndToggleAllLights();
  }

  delay(10);
}
