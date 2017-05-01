using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace scripts.entities {
    [RequireComponent(typeof(Entity))]
    public class CharacterStateMachine : MonoBehaviour {
        private delegate void StateUpdate(float deltaTime);
        private StateUpdate currentState;
        private Entity entity;
        // Use this for initialization
        void Start() {
            entity = GetComponent<Entity>();
            currentState = idleState;
        }

        // Update is called once per frame
        void Update() {
            currentState(Time.deltaTime);
        }

        private void idleState(float deltaTime) {

        }
    }
}