# Base64Diff

## Build

1. Download the solution from git
2. Build the solution __Caliel.Base64Diff.sln__ using Visual Studio 2017
3. Execute the main project _Caliel.Base64Diff.Api_ either on IIS Express or Self hosted
4. Open your favorite web browser and go to http://localhost:63634/docs. 

> There are two configurations for port: 63634 to IIS Express and 63635 to Self hosted. 
> 
> But you can change in project configuration

### Pre-requistes:
- Visual Studio 2017
- .NET Core SDK 2.0

## Architecture

The solution was split into 3 layers, and there are 4 test projects.

<img src="https://raw.githubusercontent.com/calielc/Base64Diff/6fe5942c7ac241032c2c002e5ef9dd5afd36be0e/Architecture.png" width="500" />


* Caliel.Base64.API

This is the layer wich users do requests to save left or right side.

* Caliel.Base64.Data

Here is where data persistence is read and write. Right now the project is using File System do write binaries (in folder _c:\temp_) but in the future this way could be changed.


* Caliel.Base64.Domain

This is where real application works, the API just receive HTTP request and sendo to Domain to validate and persist. 


* Caliel.Base64.API.Tests, 
* Caliel.Base64.Domain.Tests
* Caliel.Base64.Data.Tests

Unit tests projects to referred project.

* Caliel.Base64.Tests

Integration teste, from end to end. All tests post to API and wait for response, just like a real enviromnent.


## Usage

The API has 5 endpoints as specified below:

|Endpoint|Description|
|---------|-----------|
|__GET /docs__|Documentation auto-generated be swagger|
|__GET /v1/ping__|Just a ping to check if service is running|
|__POST /v1/diff/{id}/left__|Post left side of Diff (should be as base64 form)|
|__POST /v1/diff/{id}/right__|Post Right side of Diff (should be as base64 form)|
|__GET /v1/diff/{id}__|Compare and returns similarity between left and right side|

### POST Request
```json
{
    "data": "Q2FsaWVsIENvc3Rh"
}
```

### GET Response
```json
{
  "similarity": "AreEquals",
  "diffs": [
    {
      "offset": 0,
      "length": 0
    }
  ]
}
```

If you hant no more about endpoints and parameters you can check http://localhost:63634/docs and see online documentation.
