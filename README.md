# IoT Remote – Smart Device Controller

IoT Remote is a .NET-based IoT controller prototype that simulates remote management of smart home devices through an ESP32-based system. 
The solution includes a backend API, web frontend, and desktop GUI simulator for real-time control and visualization. 
The goal is to demonstrate an extendable and modular IoT architecture that enables communication between multiple layers and devices.

---------------------------------------------------------------------------

## 🚀 Tech Stack
- .NET 6.0 (GUI) and .NET 8.0 (Backend + Frontend)
- C# (ASP.NET Core)
- MQTT protocol for IoT communication
- HTML, CSS, JavaScript (Frontend)
- WinForms/WPF (GUI Simulator)

---------------------------------------------------------------------------

## ⚙️ Setup

### 1. Requirements
Install the following SDKs before running the project:
- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


### 2. Clone the repository

git clone https://github.com/Jorgenfje/IoT-Remote.git


### 3. Run the project
Launch all components automatically:

start_all.bat

This opens:
- Frontend in your browser
- Backend API (ASP.NET Core)
- GUI simulator for ESP32 functionality
  
---------------------------------------------------------------------------

### 💡 Features
- Create and manage device groups
- Add or remove smart devices dynamically
- Simulate multi-room smart home control
- Real-time updates between GUI, backend, and frontend
- Built-in MQTT support for IoT message handling

---------------------------------------------------------------------------

### 🧩 Common Issues
- Port in use:
Update ports in launchSettings.json or close other apps using the same ports.

- Missing dependencies:
Install the required .NET SDKs (6.0 and 8.0).

---------------------------------------------------------------------------

### 📄 Overview
This project demonstrates scalable IoT architecture and communication through MQTT, designed for modularity and easy expansion.
Created as part of a software engineering project at Høgskolen i Østfold.
