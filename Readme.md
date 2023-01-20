# ASP .Net Blinky Backend

Backend support for desk booking Blinky TS Rest Project

## API Reference

#### Example

```http
  GET /example
```

| Parameter | Type     | Description            |
| :-------- | :------- | :--------------------- |
| `api_key` | `string` | This is an example api |

## Running Locally

To run this application locally.

```bash
  dotnet run
```

This will run the api service on http://localhost:5127.

## Running on Docker

To run this application on docker:

Create a release of the project

```bash
  dotnet public -c Release
```

Build Docker image

```bash
  docker build -t blinky-back-end-image -f Dockerfile .
```

Spinning up premade docker image

```bash
  docker compose up
```

This will run the api service on http://localhost:5000.
