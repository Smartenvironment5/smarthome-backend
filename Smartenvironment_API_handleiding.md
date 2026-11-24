**Handleiding voor integratie in het Smart Environment
Dashboard**

**Doel:** gebruik de aangeboden sensor-API veilig en correct vanuit
jullie eigen applicatie. De IoT-infrastructuur is al ingericht; jullie
werken op API-niveau.

| **Base URL**                   | {{URBAN_SENSE_BASE_URL}}  | = https://urban-sense-api.azure-api.net/urban-sense haal de {{}} weg. Test eens met CURL, zie verderop in het document.
|--------------------------------|---------------------------|
| **API-key / subscription key** | {{URBAN_SENSE_API_KEY}}   | = zie de mail. 
| **HTTP-header**                | Ocp-Apim-Subscription-Key |

| **Belangrijk —** Iedere groep gebruikt de eigen API-key. Deel deze niet met andere groepen en zet de key niet in een openbaar Git-repository. |
|-----------------------------------------------------------------------------------------------------------------------------------------------|

# 1. Wat is de Urban Sense API?

De Urban Sense API levert actuele en historische omgevingsmetingen van
meetpunten in Rijswijk/Den Haag. Jullie hoeven de ESP32-devices, Azure IoT Hub,
Cosmos DB, Azure Functions of API Management niet zelf te bouwen.

De huidige meetwaarden zijn:

- temperatuur (°C)

- relatieve luchtvochtigheid (%)

- lokale luchtdruk (hPa)

Globaal ziet de keten er zo uit:

ESP32 sensoren  
↓  
Azure IoT Hub  
↓  
Azure Cosmos DB  
↓  
Azure Function API  
↓  
Azure API Management  
↓  
Jullie applicatie

# 2. Toegang tot de API

Iedere groep krijgt van de opdrachtgever een base URL en een eigen
subscription key. Stuur de key bij iedere request mee via deze header:

Ocp-Apim-Subscription-Key: {{URBAN_SENSE_API_KEY}}

Gebruik in jullie configuratie bijvoorbeeld:

URBAN_SENSE_BASE_URL={{URBAN_SENSE_BASE_URL}}  
URBAN_SENSE_API_KEY={{URBAN_SENSE_API_KEY}}

| **Aanbevolen —** Bewaar de API-key server-side in een environment variable. Zet de key niet hardcoded in frontend-JavaScript als die code door gebruikers kan worden bekeken. |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

# 3. Beschikbare endpoints

## 3.1 Beschikbare meetpunten

GET {{URBAN_SENSE_BASE_URL}}/devices

Gebruik dit endpoint om devices dynamisch op te halen. Ga er niet vanuit
dat er altijd exact drie meetpunten zijn.

## 3.2 Laatste meting van een device

GET {{URBAN_SENSE_BASE_URL}}/devices/{deviceId}/latest

Voorbeeld:

GET {{URBAN_SENSE_BASE_URL}}/devices/urban-sense-rijswijk-01/latest

Een response kan er bijvoorbeeld zo uitzien:

{  
"deviceId": "urban-sense-rijswijk-01",  
"deviceName": "Meetpunt 01",  
"msgCount": 42,  
"timestamp": "2026-09-11T14:59:14.000Z",  
"temperature": 22.04,  
"temperatureUnit": "C",  
"pressure": 1021.32,  
"pressureUnit": "hPa",  
"humidity": 67.36,  
"humidityUnit": "percent"  
}

## 3.3 Historische metingen

GET {{URBAN_SENSE_BASE_URL}}/devices/{deviceId}/telemetry?limit=100

Voorbeeld:

GET
{{URBAN_SENSE_BASE_URL}}/devices/urban-sense-rijswijk-01/telemetry?limit=100

De queryparameter **limit** bepaalt hoeveel metingen worden
teruggegeven. De server accepteert maximaal 500 resultaten per request.

# 4. Requests uitvoeren

## 4.1 JavaScript / Node.js

Goed om uit te zoeken!

## 4.2 C#

Goed om uit te zoeken!

## 4.3 curl

curl \\  
-H "Ocp-Apim-Subscription-Key: {{URBAN_SENSE_API_KEY}}" \\  
"{{URBAN_SENSE_BASE_URL}}/devices"

# 5. Aanbevolen architectuur in jullie project

Gebruik bij voorkeur jullie eigen backend als tussenlaag:

Browser / frontend  
↓  
Jullie eigen backend  
↓  
Urban Sense API

Zo blijft de subscription key buiten de browser en kunnen jullie
foutafhandeling, caching en eigen datamodellen centraal regelen.

# 6. Rate limit

Iedere groep heeft een eigen limiet van:

| **Limiet —** 60 requests per 60 seconden per groep. |
|-----------------------------------------------------|

Als de limiet wordt overschreden, retourneert de API:

HTTP 429 Too Many Requests

Maak daarom geen onnodige polling-loop. Voor een dashboard is
bijvoorbeeld iedere 10, 30 of 60 seconden verversen vaak voldoende,
afhankelijk van jullie toepassing en wat de klant wilt. 

# 7. HTTP-statuscodes en foutafhandeling

| **Status** | **Betekenis**                                      |
|------------|----------------------------------------------------|
| 200        | Request gelukt                                     |
| 400        | Ongeldige invoer, bijvoorbeeld een ongeldige limit |
| 401        | Subscription key ontbreekt of is ongeldig          |
| 404        | Device of data niet gevonden                       |
| 429        | Te veel requests binnen de rate limit              |
| 500        | Interne fout bij de API                            |

Controleer in code altijd eerst of de response succesvol is voordat je
JSON verwerkt.

if (!response.ok) {  
if (response.status === 429) {  
// wacht en probeer later opnieuw  
}  
  
throw new Error(\`HTTP \${response.status}\`);  
}

# 8. Timestamps

De API retourneert timestamps in UTC. Een waarde als:

2026-09-11T14:59:14.000Z

heeft een **Z** aan het einde. Dit betekent UTC. Zet de tijd pas voor
weergave om naar de lokale tijdzone.

const local = new Date(timestamp).toLocaleString("nl-NL", {  
timeZone: "Europe/Amsterdam"  
});

| **Advies —** Bewaar of verwerk de originele UTC-timestamp ongewijzigd in jullie datalaag. Converteer alleen voor presentatie. |
|-------------------------------------------------------------------------------------------------------------------------------|

# 9. Betekenis van de meetwaarden

## Temperatuur

Gemeten omgevingstemperatuur.

"temperature": 22.04, "temperatureUnit": "C"

## Luchtvochtigheid

Relatieve luchtvochtigheid.

"humidity": 67.36, "humidityUnit": "percent"

## Luchtdruk

Lokale luchtdruk zoals gemeten door de sensor. Deze waarde hoeft niet
exact gelijk te zijn aan luchtdruk op een weerwebsite die naar zeeniveau
is omgerekend.

"pressure": 1021.32, "pressureUnit": "hPa"

# 10. Werk met dynamische devices

Op dit moment kunnen bijvoorbeeld deze device-ID’s voorkomen:

urban-sense-rijswijk-01  
urban-sense-rijswijk-02  
urban-sense-rijswijk-03

De lijst kan later wijzigen. Gebruik daarom altijd **GET /devices** om
beschikbare devices op te halen. Hardcode niet één device als enige
mogelijkheid.

# 11. Aanbevolen dashboard-flow

1.  Vraag beschikbare meetpunten op met GET /devices.

2.  Laat de gebruiker een meetpunt kiezen of vergelijk meerdere
    meetpunten.

3.  Gebruik /latest voor de meest recente waarden.

4.  Gebruik /telemetry?limit=... voor historische grafieken of tabellen.

5.  Verwerk timestamps in UTC en zet ze alleen voor de UI om naar lokale
    tijd.

6.  Ververs actuele data met een redelijk interval.

7.  Handel 400-, 401-, 404-, 429- en 500-responses netjes af.

# 12. Minimale eisen voor een nette integratie

- Gebruik /devices en werk dus niet met slechts één hardcoded device.

- Gebruik /latest voor actuele sensordata.

- Gebruik /telemetry voor historische sensordata.

- Verwerk temperatuur, luchtvochtigheid en luchtdruk.

- Verwerk UTC-timestamps correct.

- Bewaar de API-key niet in een openbaar repository.

- Handel fouten zoals 404 en 429 af.

- Voorkom onnodige of agressieve request-loops.

- Combineer Urban Sense met minimaal één andere databron zoals vereist
  in de projectopdracht.

- Plaats de externe API-integratie bij voorkeur achter jullie eigen
  backend/data-laag.

# 13. Quick reference

| **Onderdeel**      | **Waarde**                                  |
|--------------------|---------------------------------------------|
| Base URL           | {{URBAN_SENSE_BASE_URL}}                    | zie hierboven. 
| Subscription key   | {{URBAN_SENSE_API_KEY}}                     |
| Header             | Ocp-Apim-Subscription-Key                   |
| Devices            | GET /devices                                |
| Laatste meting     | GET /devices/{deviceId}/latest              |
| Historie           | GET /devices/{deviceId}/telemetry?limit=100 |
| Max. history limit | 500                                         |
| Rate limit         | 60 requests per 60 seconden per groep       |
| Timestamp-formaat  | ISO 8601 UTC (Z)                            |

**Vragen over toegang of een ongeldige key?** Neem contact op met de
opdrachtgever/docent en vermeld jullie groepsnummer en de
HTTP-statuscode die jullie ontvangen.
