#include <Arduino.h> 
#include "InputHandler.h"

// Input Variables
const int button1Pin = 12;
const int button2Pin = 14;
const int potPin = 34;
int modeSelect = 0;
bool buttonPressed = false; 

void inputSetup(){
  pinMode(button1Pin, INPUT_PULLUP);
  pinMode(button2Pin, INPUT_PULLUP);
  pinMode(potPin, INPUT);
}

bool button1Pressed(){
  if (digitalRead(button1Pin) == 0 && !buttonPressed) {
  buttonPressed = true;
  return false;
  } else if (digitalRead(button1Pin) == 1 && buttonPressed) {
  buttonPressed = false;

  modeSelect ++;
  if (modeSelect == 4){modeSelect = 0;}

  return true;
  }
  else {return false;}
}