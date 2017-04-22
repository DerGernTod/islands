using UnityEngine;

namespace scripts.services {
    public abstract class Service {
        public abstract void Initialize();
        public abstract void PostInitialize();
    }
}