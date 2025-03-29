# Unity APIBuilder Documentation

## Overview
The `APIBuilder` class is a fluent-style API request builder for Unity, using `UnityWebRequest` to handle HTTP requests. It allows developers to create, configure, and send requests in an organized and structured manner.

## Installation
Ensure that your Unity project has access to `UnityEngine.Networking` before using `APIBuilder`.

```csharp
using Maphatar.UnityAPI;
```

## Usage

### Creating a Request
To create a new request, use the static `Make` method and provide an endpoint URL:

```csharp
var request = APIBuilder.Make("https://api.example.com/data");
```

### Setting Headers
Headers can be added using the `SetHeaderRequest` method:

```csharp
request.SetHeaderRequest("Authorization", "Bearer YOUR_TOKEN");
```

### Setting Timeout
You can set a timeout duration (in seconds) for the request:

```csharp
request.SetTimeout(10);
```

### Choosing HTTP Methods
Specify the HTTP method using one of the following:

```csharp
request.Get();   // For GET requests
request.Post();  // For POST requests
request.Put();   // For PUT requests
```
Alternatively, use `SetMethod` for custom methods:

```csharp
request.SetMethod("DELETE");
```

### Adding a Request Body
For requests that require a body (e.g., POST, PUT), use:

```csharp
request.BodyData("{\"key\":\"value\"}");
```

### Handling Responses
Define success and error handlers using `OnSuccess` and `OnError`:

```csharp
request
    .OnSuccess(response => Debug.Log("Success: " + response.downloadHandler.text))
    .OnError(response => Debug.LogError("Error: " + response.error));
```

### Preventing Duplicate Requests
To prevent duplicate requests, use the `StackPrevention` method with a unique key:

```csharp
request.StackPrevention("UniqueKey");
```
If a request with the same key is still in progress, subsequent requests will not be sent.

### Sending the Request
Once the request is configured, send it using:

```csharp
request.SendRequest();
```

## Example Usage

```csharp
APIBuilder.Make("https://api.example.com/data")
    .SetHeaderRequest("Content-Type", "application/json")
    .Post()
    .BodyData("{\"name\":\"John\"}")
    .OnSuccess(response => Debug.Log("Response: " + response.downloadHandler.text))
    .OnError(response => Debug.LogError("Request failed: " + response.error))
    .StackPrevention("UserCreation")
    .SendRequest();
``` 

Send request with IEnumerator. Suitable for scenarios that requires to run multiple coroutines in proper order. 

```csharp
public IEnumerator StandardRequest(){
     yield return APIBuilder.Make("https://mynedpoint.com/endpoint")
    .Get() 
    .OnSuccess((res) =>
    {
        Debug.Log("message: " + res.downloadHandler.text); 
    })
    .OnError(res => {
        Debug.LogError($"Error: {res.error}"); 
    }).SendRequestRoutine();
}
```

Send request without IEnumerator or invoking yield. Can be called in Non-MonoBehavior classes. 

```csharp
APIBuilder.Make("https://mynedpoint.com/endpoint")
    .Get() 
    .OnSuccess((res) =>
    {
        Debug.Log("message: " + res.downloadHandler.text); 
    })
    .OnError(res => {
        Debug.LogError($"Error: {res.error}"); 
    }).SendRequest();
```
