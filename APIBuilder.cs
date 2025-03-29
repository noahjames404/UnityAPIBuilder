using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking; 

namespace Maphatar.UnityAPI
{
    public class APIBuilder
    {
        UnityWebRequest request;
        Action<UnityWebRequest> OnSuccessCallback, OnErrorCallback;
        static APIRuntimeManager runtime => APIRuntimeManager.Instance;

        static Dictionary<string, bool> requestHistory = new Dictionary<string, bool>();

        string stackKey = null;

        public static APIBuilder Make(string endpoint)
        {
            var builder = new APIBuilder();
            builder.request = new UnityWebRequest();
            builder.request.url = endpoint;
            builder.request.downloadHandler = new DownloadHandlerBuffer();

            return builder;
        }

        public APIBuilder SetHeaderRequest(string name, string value)
        {
            request.SetRequestHeader(name, value);
            return this;
        }

        public APIBuilder SetTimeout(int timeout)
        {
            request.timeout = timeout;
            return this;
        }


        public APIBuilder Put()
        {
            request.method = "PUT";
            return this;
        }

        public APIBuilder Get()
        {
            request.method = "GET";
            return this;
        }


        public APIBuilder Post()
        {
            request.method = "POST";
            return this;
        }

        public APIBuilder BodyData(string data)
        {
            request.uploadHandler = new UploadHandlerRaw(
                System.Text.Encoding.UTF8.GetBytes(data)
            );
            return this;
        }

        public APIBuilder SetMethod(string method)
        {
            request.method = method;
            return this;
        }

        public APIBuilder OnSuccess(Action<UnityWebRequest> callback)
        {
            OnSuccessCallback = callback;
            return this;
        }

        public APIBuilder OnError(Action<UnityWebRequest> callback)
        {
            OnErrorCallback = callback;
            return this;
        }

        public void SendRequest()
        {
            runtime.StartCoroutine(SendRequestRoutine());
        }

        public IEnumerator SendRequestRoutine()
        {
            if (HasPendingRequest())
            {
                UnityEngine.Debug.LogError($"Stack Prevention is active on key {stackKey}, you won't be able to request another while you're still sending");
                yield break;
            }

            SetRequestState(true);
            yield return request.SendWebRequest();
            SetRequestState(false);

            
            if (request.result == UnityWebRequest.Result.Success)
            {
                OnSuccessCallback?.Invoke(request);
            }
            else
            {
                OnErrorCallback?.Invoke(request);
            }

        }


        void SetRequestState(bool state)
        {
            if (stackKey == null) return;

            requestHistory[stackKey] = state;
        }

        bool HasPendingRequest()
        {
            if (stackKey == null) return false;
            return requestHistory.TryGetValue(stackKey, out var pending) ? pending : false;
        }

        public APIBuilder StackPrevention(string key)
        {
            if (!requestHistory.ContainsKey(key))
            {
                requestHistory.Add(key, false);
            }

            stackKey = key;

            return this;
        }
    }
}
