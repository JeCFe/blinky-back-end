# ASP .Net Blinky Backend

Backend support for desk booking Blinky TS Rest Project

## API Reference

#### Generate Room

Takes a room name and amount of desks to generate a new room which is saved in the database.
Returns status code 200 if successful.
Return 400 for a bad request.

```http
  POST /GenerateRoom
```

| Parameter       | Type     | Description                                            |
| :-------------- | :------- | :----------------------------------------------------- |
| `RoomName`      | `string` | Define the name of the room to be generated            |
| `AmountOfDesks` | `int`    | Define the amount of desk to be added to added to room |

#### Generate Room

Returns all room stored in the database.

```http
  GET /Room
```

#### View room bookings on specified dates

Returns all desks with their booked status, and any relevent assigned names on the specified date.

```http
  GET /Room/{roomId}
```

| Parameter | Type       | Description                                                     |
| :-------- | :--------- | :-------------------------------------------------------------- |
| `roomId`  | `Guid`     | ID of the room                                                  |
| `date`    | `DateOnly` | ISO 8601 Date Only, used for getting bookings on specified date |

#### Book desk on specified date

Books specified desk on specified date

```http
  POST /book
```

| Parameter  | Type       | Description                                                     |
| :--------- | :--------- | :-------------------------------------------------------------- |
| `deskId`   | `Guid`     | ID of the room                                                  |
| `userName` | `string`   | Name of person booking desk                                     |
| `date`     | `DateOnly` | ISO 8601 Date Only, used for getting bookings on specified date |

#### Update desk display position

Books specified desk on specified date

```http
  POST /book
```

| Parameter | Type   | Description                |
| :-------- | :----- | :------------------------- |
| `deskId`  | `Guid` | ID of the room             |
| `x`       | `int`  | Position of desk in x axis |
| `y`       | `int`  | Position of desk in y axis |

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

## Update datebase migrations

To update the database migrations, run the follow code snippits

```bash
  dotnet ef migrations add <Descriptive name of changes>
  dotnet ef database update
```
