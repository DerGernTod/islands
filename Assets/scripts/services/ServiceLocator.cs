using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.services {
    public class ServiceLocator : MonoBehaviour {
        private Dictionary<System.Type, Service> services = new Dictionary<System.Type, Service>();
        private static ServiceLocator singleton;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            if (singleton != null) {
                Debug.LogWarning("Trying to instantiate another service locator. This is not allowed!");
                DestroyImmediate(this);
            } else {
                singleton = this;
            }

            services[typeof(EventService)] = new EventService();
            services[typeof(EntityService)] = new EntityService();

            foreach (Service s in services.Values) {
                s.Initialize();
            }

            foreach (Service s in services.Values) {
                s.PostInitialize();
            }
        }
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {

        }

        public static T Service<T>() where T : Service {
            return (T) singleton.services[typeof(T)];
        }
    }

}
