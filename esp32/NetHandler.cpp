#include <AsyncTCP.h>
#include <ESPAsyncWebServer.h>
#include <Arduino_JSON.h>
#include "SPIFFS.h"
#include <HTTPClient.h>
#include <WiFi.h>
#include <Arduino_JSON.h>
#include <EEPROM.h>
#include <NetHandler.h>

#define EEPROM_SIZE 96 

AsyncWebServer server(80);
AsyncWebSocket ws("/ws");

String ssid = "";
String pwd = "";

void initWiFi() {
  EEPROM.begin(EEPROM_SIZE);
  // Hvis den siste byten i EEPROM er flagget 1, koble til med lagrede kredentialer:
  if (EEPROM.read(95) == 1){
    getWiFiCredentials();
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, pwd);
    Serial.print("Connecting to WiFi ..");
    int attempts = 0;
    while (WiFi.status() != WL_CONNECTED) {
      Serial.print('.');
      delay(1000);
      attempts++;
      if (attempts > 20) {
        Serial.println("Can't connect! Starting AP Router!");
        initAPWiFi();
        return;
      }
    }
    Serial.println(WiFi.localIP()); 
  }
  else {
    initAPWiFi();
  }
}
  
// Hvis ikke startes Wi-Fi tilkoblingsprosessen:
const char* apSSID = "ESP32";
void initAPWiFi(){
  WiFi.softAP(apSSID);
  Serial.print("AP IP adresse: ");
  Serial.println(WiFi.softAPIP());

  server.serveStatic("/", SPIFFS, "/").setDefaultFile("index.html");
  server.serveStatic("/styles.css", SPIFFS, "/styles.css");
  server.serveStatic("/script.js", SPIFFS, "/script.js");

  recieveWiFiCredentials();
}

// Funksjon for å motta kredentialer fra nettsiden:
void recieveWiFiCredentials() {
  server.on("/wifi", HTTP_POST, [](AsyncWebServerRequest *request){}, NULL,
    [](AsyncWebServerRequest *request, uint8_t *data, size_t len, size_t index, size_t total) {
      String jsonData = "";
      
      for (size_t i = 0; i < len; i++) {
        jsonData += (char)data[i];
      }

      JSONVar parsedData = JSON.parse(jsonData);

      if (JSON.typeof(parsedData) == "undefined") {
        Serial.println("Failed to parse JSON");
        request->send(400, "text/plain", "Invalid input");
        return;
      }

      String ssid = parsedData["ssid"];
      String password = parsedData["password"];

      // DEBUG:
      // Serial.printf("Received SSID: %s, Password: %s\n", ssid.c_str(), password.c_str());

      request->send(200, "text/plain", "WiFi credentials received");
      storeWiFiCredentials(ssid.c_str(), password.c_str());
      WiFi.softAPdisconnect(true);
      initWiFi();
  });
}

void storeWiFiCredentials(String ssid, String password) {
  // Clear EEPROM:
  for (int i = 0; i < EEPROM_SIZE; i++) {
    EEPROM.write(i, 0);
  }
  // Store SSID:
  for (int i = 0; i < ssid.length(); i++) {
    EEPROM.write(i, ssid[i]);
  }
  // Store Password:
  for (int i = 0; i < password.length(); i++) {
    EEPROM.write(32 + i, password[i]);
  }
  // Flagger den siste byten:
  uint8_t x = 1;
  EEPROM.write(95, x); //
  EEPROM.commit();
  Serial.println("Credentials saved in EEPROM.");
}

void serverStart(){
  server.addHandler(&ws);
  server.begin();

  if (!SPIFFS.begin(true)) {
    Serial.println("An error has occurred while mounting SPIFFS");
    return;
  }
}


void getWiFiCredentials() {
  char ssidBuffer[33]; // SSID er max 32 chars + null terminator
  char passwordBuffer[64]; // Pwd er max 63 chars + null terminator

  for (int i = 0; i < 32; i++) {
    ssidBuffer[i] = char(EEPROM.read(i));
  }
  ssidBuffer[32] = '\0'; // Null-terminator
  ssid = String(ssidBuffer);

  for (int i = 0; i < 63; i++) {
    passwordBuffer[i] = char(EEPROM.read(32 + i));
  }

  passwordBuffer[63] = '\0'; // Null-terminator
  pwd = String(passwordBuffer);
}

void serverHTML(){
  server.on("/", HTTP_GET, [](AsyncWebServerRequest *request){
    request->send(SPIFFS, "/index.html", "text/html");
  });
  server.on("/styles.css", HTTP_GET, [](AsyncWebServerRequest *request){
    request->send(SPIFFS, "/style.css", "text/css");
  });
  server.on("/script.js", HTTP_GET, [](AsyncWebServerRequest *request){
    request->send(SPIFFS, "/script.js", "application/javascript");
  });
}

void serverCleanup(){
  ws.cleanupClients();
}