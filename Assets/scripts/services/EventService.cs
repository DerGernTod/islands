using scripts.entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace scripts.services {
    public class EventService : Service {
        public event Action<int, int> EntityAttack;
        public event Action<int, int> EntityDamage;
        public event Action<int, Entity> EntityCreate;
        public event Action<int> EntityDeath;
        public event Action<int> EntityRemove;

        //entityId, selectionOwnerId
        public event Action<int, int> EntitySelected;
        public event Action<int, int> EntityDeselected;


        public enum EventType {
            EntityAttack,
            EntityDamage,
            EntityCreate,
            EntityDeath,
            EntityRemove,
            EntitySelect,
            EntityDeselect
        }

        public override void Initialize() {
            Debug.Log("EventService initialized");
        }

        public override void PostInitialize() {

        }

        public void Dispatch(EventType eventType, int sourceId, int targetId = -1, object element = null) {
            switch(eventType) {
                case EventType.EntityAttack:
                    if (EntityAttack != null) EntityAttack(sourceId, targetId);
                    break;
                case EventType.EntityDamage:
                    if (EntityDamage != null) EntityDamage(sourceId, targetId);
                    break;
                case EventType.EntityCreate:
                    if (EntityCreate != null) EntityCreate(sourceId, (Entity)element);
                    break;
                case EventType.EntityDeath:
                    if (EntityDeath != null) EntityDeath(sourceId);
                    break;
                case EventType.EntityRemove:
                    if (EntityRemove != null) EntityRemove(sourceId);
                    break;
                case EventType.EntitySelect:
                    if (EntitySelected != null) EntitySelected(sourceId, targetId);
                    break;
                case EventType.EntityDeselect:
                    if (EntityDeselected != null) EntityDeselected(sourceId, targetId);
                    break;
                default:
                    Debug.LogWarning("No event specified for " + eventType);
                    break;
            }
        }

    }
}
