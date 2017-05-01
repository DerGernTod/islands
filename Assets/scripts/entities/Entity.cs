using System;
using scripts.services;
using UnityEngine;

namespace scripts.entities {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Entity : MonoBehaviour {
        private static int curId = 0;

        [SerializeField]
        private EntityType entityType;

        private SpriteRenderer spriteRenderer;

        public int ID { get; private set; }
        private float maxHealth;
        private float range;
        private float movementSpeed;
        private float attackSpeed;
        private float currentHealth;
        private bool isDead;

        public int ownerID;

        public float Damage { get; private set; }
        public Sprite Sprite { get; private set; }

        private void Awake() {
            ID = curId++;
        }
        // Use this for initialization
        void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = entityType.EntitySprite;

            maxHealth = entityType.MaxHealth;
            currentHealth = maxHealth;
            range = entityType.Range;
            movementSpeed = entityType.MovementSpeed;
            attackSpeed = entityType.AttackSpeed;
            Damage = entityType.Damage;
            Sprite = entityType.EntitySprite;

            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityCreate, ID, element : this);
        }

        public void Deselect() {
            Debug.Log("Entity deselected: ", gameObject);
        }

        public void Select() {
            Debug.Log("Entity selected: ", gameObject);
        }

        public void TakeDamage(float damage) {
            currentHealth -= damage;
            Debug.Log("Entity " + ID + " got damage, is on " + currentHealth);
            if (currentHealth <= 0) {
                isDead = true;
                ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityDeath, ID);
            }
        }

        public void Remove() {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update() {
            //TODO: remove after testing
            if (ID == 0) {
                if (Input.GetKeyDown(KeyCode.Alpha1)) Attack(1);
                if (Input.GetKeyDown(KeyCode.Alpha2)) Attack(2);
            }
        }

        public void Attack(int targetId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityAttack, ID, targetId);
        }


        private void OnValidate() {
            GetComponent<SpriteRenderer>().sprite = entityType.EntitySprite;
        }
    }
}