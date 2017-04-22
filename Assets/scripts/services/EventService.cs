using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace scripts.services {
    public class EventService : Service {
        public event OnEntityAttack EntityAttack;
        public event OnEntityDamage EntityDamage;

        public delegate void OnEntityDamage(int id);
        public delegate void OnEntityAttack(int targetId);

        public enum EventType {
            EntityEventAttack,
            EntityEventDamage
        }
        public override void Initialize() {
            Debug.Log("EventService initialized");
        }
        public void Dispatch(EventType eventType, int argument) {
            switch(eventType) {
                case EventType.EntityEventAttack:
                    if (EntityAttack != null) EntityAttack(argument);
                    break;
                case EventType.EntityEventDamage:
                    if (EntityDamage != null) EntityDamage(argument);
                    break;
            }
        }
    }
}
