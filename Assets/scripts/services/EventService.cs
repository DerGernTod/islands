using scripts.entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace scripts.services {
    public class EventService : Service {
        public event OnEntityAttack EntityAttack;
        public event OnEntityDamage EntityDamage;
        public event OnEntityCreate EntityCreate;
        public event OnEntityDeath EntityDeath;
        public event OnEntityRemove EntityRemove;

        public delegate void OnEntityDamage(int sourceId, int targetId);
        public delegate void OnEntityAttack(int sourceId, int targetId);
        public delegate void OnEntityCreate(int entityId, Entity entity);
        public delegate void OnEntityRemove(int entityId);
        public delegate void OnEntityDeath(int entityId);

        public enum EventType {
            EntityAttack,
            EntityDamage,
            EntityCreate,
            EntityDeath,
            EntityRemove
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
            }
        }

    }
}
