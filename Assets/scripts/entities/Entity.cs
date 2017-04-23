using scripts.services;
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
        private float currentHealth;
        private bool isDead;

        public int ownerID;

        public float Damage { get; private set; }
        public Sprite Sprite { get; private set; }

        private void Awake() {
            id = curId++;
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

            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityCreate, id, element : this);
        }

        public void TakeDamage(float damage) {
            currentHealth -= damage;
            Debug.Log("Entity " + id + " got damage, is on " + currentHealth);
            if (currentHealth <= 0) {
                isDead = true;
                ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityDeath, id);
            }
        }

        public void Remove() {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update() {
            //TODO: remove after testing
            transform.position = transform.position + movementSpeed * Time.deltaTime * Vector3.right;
            if (id == 0) {
                if (Input.GetKeyDown(KeyCode.Alpha1)) Attack(1);
                if (Input.GetKeyDown(KeyCode.Alpha2)) Attack(2);
            }
        }

        public void Attack(int targetId) {
            ServiceLocator.Service<EventService>().Dispatch(EventService.EventType.EntityAttack, id, targetId);
        }


        private void OnValidate() {
            GetComponent<SpriteRenderer>().sprite = entityType.EntitySprite;
        }
    }
}