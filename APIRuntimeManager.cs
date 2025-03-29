using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Maphatar.UnityAPI
{
    public class APIRuntimeManager : MonoBehaviour
    {
        //it's only here to handle coroutines.  

        static APIRuntimeManager instance;

        public static APIRuntimeManager Instance
        {
            get
            {

                if (instance == null)
                {
                    GameObject gameObject = new GameObject(nameof(APIRuntimeManager));
                    instance = gameObject.AddComponent<APIRuntimeManager>();

                    DontDestroyOnLoad(gameObject);
                }

                return instance;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }

    }
}
