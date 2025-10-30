#include "HueHandler.h"
#include <WiFiClientSecure.h>
#include <WiFiClient.h>
#include <HTTPClient.h>
#include <vector>
#include <Arduino_JSON.h>


String serverIP = "192.168.1.71"; // Change this to the IP of ASP.NET backend server

JSONVar jsonData;  
bool toggleState = true; 


// Functions for interfacing with the simulator:
void fetchAndToggleAllLights() {
    WiFiClient client;
    HTTPClient http;

    String url = "http://" + serverIP + ":5048/api/v1/Group/getAll";
    http.begin(client, url);

    int httpResponseCode = http.GET();
    if (httpResponseCode > 0) {
        String response = http.getString();
        Serial.println("Fetched JSON: " + response);

        // Parse JSON
        jsonData = JSON.parse(response);

        // Check if parsing succeeded
        if (JSON.typeof(jsonData) == "undefined") {
            Serial.println("Parsing failed.");
            return;
        }

        // Update all devices:
        toggleDeviceState();
    } else {
        Serial.print("Error on GET request: ");
        Serial.println(httpResponseCode);
    }

    http.end();
}
void toggleDeviceState() {
    WiFiClient client;
    HTTPClient http;

    String url = "http://" + serverIP + ":5048/api/v1/Group/updateDevices";

    // Looping through each group:
    for (int i = 0; i < jsonData.length(); i++) {
        JSONVar group = jsonData[i];
        JSONVar devices = group["devices"];  

        // Looping through devices in the groups and updating state:
        for (int j = 0; j < devices.length(); j++) {
            int deviceId = (int)devices[j]["id"];  // Get the device ID

            String payload = "{\"Id\": " + String(deviceId) + ", \"State\": " + (toggleState ? "true" : "false") + "}";
            
            http.begin(client, url);
            http.addHeader("Content-Type", "application/json");

            int httpResponseCode = http.POST(payload);

            if (httpResponseCode > 0) {
                Serial.print("State updated for device ");
                Serial.print(deviceId);
                Serial.print(" to ");
                Serial.print(toggleState ? "on" : "off");
                Serial.print(" with response code: ");
                Serial.println(httpResponseCode);
            } else {
                Serial.print("Failed to update state for device ");
                Serial.print(deviceId);
                Serial.print(" with error code: ");
                Serial.println(httpResponseCode);
            }

            http.end();
        }
    }

    // Toggle the state for the next call
    toggleState = !toggleState;
}



// The rest is Philips Hue API stuff:



// BRIDGE IP AND USERNAME NEEDS TO BE DYNAMIC, WILL FIX LATER:
const char* hueBridgeIP = "192.168.1.181";
const char* apiUsername = "ez-4DhpG03fVStwV0S3Xkf4KskaaMqdPK9ewDW2N";
int numberOfLights;
bool lightsOn;
std::vector<int> hueID;

void getHueID() {
    WiFiClientSecure client;
    HTTPClient http;
    String url = String("https://") + hueBridgeIP + "/api/" + apiUsername + "/lights";
    client.setInsecure();
    http.begin(client, url);
    http.addHeader("Content-Type", "application/json");

    int httpResponseCode = http.GET();
    if (httpResponseCode > 0) {
        String response = http.getString();
        JSONVar jsonData = JSON.parse(response);

        Serial.println(jsonData);

        if (JSON.typeof(jsonData) == "undefined") {
            Serial.println("Parsing JSON failed!");
        }

        numberOfLights = jsonData.keys().length();
        hueID.resize(numberOfLights);
        int lightsOnCounter = 0;

        for (int i = 0; i < numberOfLights; i++) {
            String lightID = jsonData.keys()[i];
            hueID[i] = lightID.toInt();
            JSONVar lightState = jsonData[lightID]["state"];

            if (lightState.hasOwnProperty("on") && bool(lightState["on"])) {
                lightsOnCounter++;
            }
        }

        lightsOn = (lightsOnCounter > numberOfLights / 2);
        http.end();
    } else {
        Serial.print("GET ERROR: ");
        Serial.println(httpResponseCode);
    }
}

void lightsDefault() {
    WiFiClientSecure client;
    HTTPClient http;

    for (int i = 0; i < numberOfLights; i++) {
        String url = String("https://") + hueBridgeIP + "/api/" + apiUsername + "/lights/" + String(hueID[i]) + "/state";
        http.begin(client, url);
        http.addHeader("Content-Type", "application/json");
        client.setInsecure();

        String payload = lightsOn ? "{\"on\": false}" : "{\"on\": true, \"bri\": 254, \"ct\": 370}";

        int httpResponseCode = http.PUT(payload);

        if (httpResponseCode > 0) {
            Serial.print("PUT request sent for light ");
            Serial.print(i);
            Serial.println(httpResponseCode);
            String response = http.getString();
            Serial.println(response);
        } else {
            Serial.print("Error on PUT request for light ");
            Serial.print(i);
            Serial.print(": ");
            Serial.println(httpResponseCode);
        }

        http.end();
    }
  lightsOn = !lightsOn;
}
