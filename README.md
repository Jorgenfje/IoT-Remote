Brukermanual for prototype - FullKontroll.
- Denne veiledningen forklarer hvordan du setter opp og kjører prototypen til FullKontroll-prosjektet, inkludert backend, frontend og GUI-simulatoren. Følg trinnene nedenfor for å sikre en vellykket oppstart.


Hva trenger du?
- .NET SDK versjon 6.0 og 8.0:
    - 6.0 brukes for GUI.
    - 8.0 brukes for backend og frontend.

- Last ned .NET 6.0: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Last ned .NET 8.0: https://dotnet.microsoft.com/en-us/download/dotnet/8.0


Åpne prosjektet:
- Gå til zip-mappen for prosjektet "Gruppe4_Prosjektinnlevering" og undermappen "Kildekode".
- Kun hvis du bruker GitHub: 
    - Gå til repositoriet: https://github.com/Arvoz/Gruppe4_V2. Klon repositoriet fra main-grenen.


Starte prosjektet:
- Start prosjektet ved å dobbeltklikke på filen "start_all.bat".
- En nettleser vil åpne seg med to faner:
    - Frontend
    - Backend (kan ignoreres hvis du ikke er teknisk interessert).
- Et vindu åpnes, som simulerer enkle funksjoner på ESP32-fjernkontrollen.


Bruke applikasjonen:
Navigasjon i frontend - bruk frontend-siden til å:
- Opprette grupper
- Legge til og fjerne enheter fra grupper
- Navigere mellom ulike funksjoner
- Formålet er å simulere smarthjem-enheter i forskjellige rom, slik at flere enheter kan styres samtidig (f.eks. skru av alle lysene i stua med ett klikk).

Simulator (GUI):
- Gå til Hjem-fanen på frontend-siden.
- I GUI:
    - Velg en forhåndsdefinert gruppe i dropdownmenyen oppe til høyre.
    - Skru av og på lys i den valgte gruppen.
    - Bruk "Oppdater Grupper (GET)" for å hente liste fra backend med gruppeinformasjon.
    - En slider simulerer et vrihjul for justering av funksjoner som lydvolum eller lysfarge (dette er foreløpig ikke funksjonelt).
- Alternativt kan du opprette egne grupper og legge til enheter (lys). En enhet kan tilhøre flere grupper samtidig (med vilje).


Vanlige problemer og løsninger:
- Hvis porter allerede er i bruk:
    - Endre portene i launchSettings.json i prosjektmappen (både for frontend og backend).
    - Alternativt, stopp andre programmer som bruker de samme portene.
- Manglende avhengigheter:
    - Hvis nødvendig programvare eller SDK-er mangler, vil feilmeldinger vanligvis angi hva som må installeres (f.eks. .NET SDK 6.0 og 8.0).
