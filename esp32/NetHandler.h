#ifndef NET_HANDLER_H
#define NET_HANDLER_H

void serverStart();
void serverCleanup();
void serverHTML();
void JSONUpdate();
void serverHandleWiFi();
void recieveWiFiCredentials();
void storeWiFiCredentials(String ssid, String password);
void getWiFiCredentials();
void connectToStoredWiFi();

void initWiFi();
void initAPWiFi();
#endif