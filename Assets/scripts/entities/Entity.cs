using scripts.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        public Sprite Sprite { get; private set; }
        private float currentHealth;

        public float Damage { get; private set; }

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
            Damage = entityType.Damage;
            Sprite = entityType.EntitySprite;

            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityCreate, id, element : this);
        }

        public void TakeDamage(float damage) {
            
        }


        // Update is called once per frame
        void Update() {
            //TODO: remove after testing
            transform.position = transform.position + movementSpeed * Time.deltaTime * Vector3.right;
        }

        public void Attack(int targetId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityAttack, id, targetId);
        }


        private void OnValidate() {
            GetComponent<SpriteRenderer>().sprite = entityType.EntitySprite;
        }
    }
}