﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using scripts.entities;

namespace scripts.services {
    public class EntityService : Service {
        public Dictionary<int, Entity> entities;
        public EntityService() {
            entities = new Dictionary<int, Entity>();
        }

        public override void Initialize() {
            
        }

        public override void PostInitialize() {
            ServiceLocator.Service<EventService>().EntityAttack += OnEntityAttack;
            ServiceLocator.Service<EventService>().EntityCreate += OnEntityCreate;
        }

        private void OnEntityCreate(int id, Entity entity) {
            if (entities.ContainsKey(id)) {
                Debug.LogWarning("Trying to create an entity that's already registered: " + id);
            } else {
                entities[id] = entity;
            }
        }

        private void OnEntityAttack(int source, int target) {
            if (!entities.ContainsKey(target) || !entities.ContainsKey(source)) {
                Debug.LogWarning("Trying to attack an entity that is not registered! Source: " + source + ", Target: " + target);
            } else {
                entities[target].TakeDamage(entities[source].Damage);
            }
        }
    }
}