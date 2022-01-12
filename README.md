# Disaster-Tracker-App [![.NET](https://github.com/FairyFox5700/Disaster-Tracker-App/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FairyFox5700/Disaster-Tracker-App/actions/workflows/dotnet.yml)
Disaster-Tracker-App-2022

### Implemented by PHAOND team:
PHAOND: People who  are afraid of natural disasters!
<img src="https://user-images.githubusercontent.com/46414904/149190082-b4975aa1-3128-487c-b24a-b737d04cda05.png" width=10% height=10%>

## Branching strategy
_________________________________________
1. Crate own branch with name as
fix-short-description
2. Create PR
3. Wait for approve and merge

## Architecrure overview
_________________________________________
![image](https://user-images.githubusercontent.com/46414904/149184756-887074c6-9e86-4b2e-b374-1bf2ce77df9a.png)

## REST API (Swagger)

![image](https://user-images.githubusercontent.com/46414904/149184451-d8855ba4-7cf7-4f16-bee8-8c2c098eaf26.png)

## Hangfire dasboard

Url: https:localhost:7200/hangfire
![image](https://user-images.githubusercontent.com/46414904/149184665-51679ca9-b96b-4a23-abd8-e1808185f22a.png)

## Techologies used:

### 1. <img src="https://user-images.githubusercontent.com/46414904/149185243-cc4bc418-cabc-4483-a7db-32920f87f2a0.png" width=10% height=10%>  Redis - In-memory  processing and  cache 
### 2. <img src="https://user-images.githubusercontent.com/46414904/149185307-bde69b96-1674-435e-be3e-3163debf72e6.png" width=10% height=10%>  Hangfire - Scheduled long-running  jobs
### 3. <img src="https://user-images.githubusercontent.com/46414904/149185350-178254b2-c209-49a6-b04a-f84bebdb79d6.png" width=10% height=10%>  PostGIS - Spatial data types for  PostgreSQL
### 4. <img src="https://user-images.githubusercontent.com/46414904/149185460-a59fe0f4-5c4b-461e-865e-6d6cceb46e6a.png" width=8% height=8%>  RX.NET - Reactive extensions
### 5. <img src="https://user-images.githubusercontent.com/46414904/149185522-90015143-02c0-4129-aeca-0f3e0c6769eb.png" width=10% height=10%>   NgRock - Ngrok exposes local  servers behind NATs and  firewalls to the public  internet over secure  tunnels.
### 6. <img src="https://user-images.githubusercontent.com/46414904/149185635-2b275229-6937-4fa9-a800-8e9db38f1019.png" width=10% height=10%> NET Core 6 - .NET 6 — The Fastest .NET Yet.
### 7. <img src="https://user-images.githubusercontent.com/46414904/149185754-cb088735-e981-4a81-92fe-11966594ba0f.png" width=10% height=10%> Polly - Polly helps you  navigate the unreliable  network.

## External APIs:

### 1.Google Calendar API  <img src="https://user-images.githubusercontent.com/46414904/149185950-2faf51ed-c9cf-4433-b7cf-946e9bfe9b2d.png" width=10% height=10%> 
The Google Calendar API is a RESTful API that can be  accessed through explicit HTTP calls or via the Google Client  Libraries. The API exposes most of the features available in  the Google Calendar Web interface.
### 2.Google Geocoding API <img src="https://user-images.githubusercontent.com/46414904/149186017-f42f43dd-c957-4dd6-b0eb-12ba744ee41e.png" width=10% height=10%> 
The Geocoding API is a service that provides geocoding and  reverse geocoding of addresses.
### 3.EONET API <img src="https://user-images.githubusercontent.com/46414904/149186108-2dc98d5d-f52d-4bb9-b007-d42e79282733.png" width=10% height=10%> 
An API that provided a curated list of natural events and  provided a way to link those events to event-related NRT  image layers
### 4.Google OAuth 2.0 API <img src="https://user-images.githubusercontent.com/46414904/149186151-6d26948c-3b6f-4806-8b99-2422ee3dc37b.png" width=10% height=10%> 
Google APIs use the OAuth 2.0 protocol for authentication  and authorization. Google supports common OAuth 2.0  scenarios such as those for web server, client-side, installed,  and limited-input device applications.

### Google auth sequence flow
![image](https://user-images.githubusercontent.com/46414904/149189472-5ae03f2e-b1f5-4830-af63-db238a4b5911.png)


### Auth sequence flow from user perspective
![image](https://user-images.githubusercontent.com/46414904/149189505-7859b6a0-b1e7-43fa-bf5b-e90a0d4426cf.png)











