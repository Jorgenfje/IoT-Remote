start /min cmd /c "cd IoT-Prosjekt/Backend && dotnet run && exit"
start /min cmd /c "cd IoT-Prosjekt/Frontend && dotnet run && exit"
start /min cmd /c "cd gui/SimpleGUIApp && dotnet run && exit"

timeout /t 2
start http://localhost:5048/swagger
start http://localhost:5117/Device/LightSimulator
