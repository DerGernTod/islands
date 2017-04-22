using scripts.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.entities {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Entity : MonoBehaviour {
        private static int curId = 0;
        [SerializeField]
        private EntityType entityType;

        private SpriteRenderer spriteRenderer;

        private int id;
        private float maxHealth;
        private float range;
        private float movementSpeed;
        private float attackSpeed;
        private float damage;
        private Sprite sprite;
        private float currentHealth;

        private void Awake() {
            id = curId++;
        }
        // Use this for initialization
        void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = entityType.EntitySprite;

            maxHealth = entityType.MaxHealth;
            range = entityType.Range;
            movementSpeed = entityType.MovementSpeed;
            attackSpeed = entityType.AttackSpeed;
            damage = entityType.Damage;
            sprite = entityType.EntitySprite;

        }
        

        // Update is called once per frame
        void Update() {
            //TODO: remove after testing
            transform.position = transform.position + movementSpeed * Time.deltaTime * Vector3.right;
        }

        public void Attack(int targetId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityEventAttack, targetId);
        }
    }
}