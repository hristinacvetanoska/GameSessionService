# GameSessionService

A simple .NET 8 Web API to manage game sessions with caching and concurrency safety.

## Features

- Start a game session (`POST /sessions/start`)
- Get session by ID (`GET /sessions/{sessionId}`) with in-memory cache
- Concurrency-safe session creation using `SemaphoreSlim`
- Idempotent session creation (returns existing session if exists)
- Structured logging with Correlation ID
- Performance testing endpoint (`GET /diagnostics/perf-test`)

---

## Endpoints

### Start Session

**POST** `/sessions/start`  

**Request Body:**

```json
{
  "playerId": "P123",
  "gameId": "G100"
}
```
**Response Body:**
```json
{
  "sessionId": "generated-id",
  "startedAt": "2026-03-11T18:00:00Z",
  "status": "Active"
}
```

**Performance Test**

**GET** ```/diagnostics/perf-test?iterations=1000```

**Sample Response:**
```json
{
  "iterations": 1000,
  "totalMs": 5319,
  "avgMs": 5.319
}
```
**Tech Stack**

- .NET 8

- PostgreSQL

- MemoryCache for caching

- xUnit & Moq for unit tests

- ILogger for structured logging

**Concurrency & Idempotency**

- SemaphoreSlim ensures only one session can be created for the same player+game at a time

- Existing sessions are returned to guarantee idempotency

- Structured logging includes CorrelationId to trace requests

## Getting Started

1. Clone the repository: ````git clone https://github.com/hristinacvetanoska/GameSessionService.git````
2. Configure the database in appsettings.json: ````"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=gamesessionsdb;Username=yourusername;Password=yourpassword"
}````
3. Run migrations and start the API
4. Use Swagger at `https://localhost:7272/swagger/index.html` to test endpoints

## Notes

Cache TTL is 60 seconds

Logging is structured with CorrelationId for traceability

StartSession is safe under concurrent requests

GetSession endpoint provides X-Cache header for client-side awareness   
