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
