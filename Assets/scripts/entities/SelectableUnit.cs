using System;
using scripts.services;
using UnityEngine;
using System.Collections.Generic;

namespace scripts.entities {
    [RequireComponent(typeof(Entity))]
    public class SelectableUnit : MonoBehaviour {
        public GameObject selectionCircle;
        private Entity entity;
        private int entityId;
        private Dictionary<PlayerManager.RELATIONTYPE, GameObject> selectionPrefabs;
        private GameObject mySelectionCirclePrefab;
        private GameObject enemySelectionCirclePrefab;
        private GameObject allySelectionCirclePrefab;
        private GameObject neutralSelectionCirclePrefab;
        private void Awake() {
            var prefabs = Resources.LoadAll<GameObject>("prefabs/selection");
            selectionPrefabs = new Dictionary<PlayerManager.RELATIONTYPE, GameObject>() {
                { PlayerManager.RELATIONTYPE.ALLY, prefabs[0] },
                { PlayerManager.RELATIONTYPE.ENEMY, prefabs[1] },
                { PlayerManager.RELATIONTYPE.OWN, prefabs[2] },
                { PlayerManager.RELATIONTYPE.NEUTRAL, prefabs[3] }
            };
            
        }

        private void Start() {
            entity = GetComponent<Entity>();
            entityId = entity.ID;
        }

        public void Select(int selectorId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntitySelect, entityId, selectorId);
        }

        public void Deselect(int selectorId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityDeselect, entityId, selectorId);
        }

        public void DestroySelection() {

            if (selectionCircle != null) {
                Destroy(selectionCircle.gameObject);
                selectionCircle = null;
            }
        }

        public void CreateSelection(int ownerId) {
            if (selectionCircle == null) {
                PlayerManager.RELATIONTYPE relation = PlayerManager.getRelationBetween(entity.ownerID, ownerId);
                selectionCircle = Instantiate(selectionPrefabs[relation]);
                selectionCircle.transform.SetParent(transform, false);
                selectionCircle.transform.eulerAngles = Vector3.zero;
            }
        }

    }
}