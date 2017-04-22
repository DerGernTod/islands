using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.entities {
    public class Entity : MonoBehaviour {
        [SerializeField]
        private EntityType entityType;
        private float maxHealth;
        private float range;
        private float movementSpeed;
        private float attackSpeed;
        private float damage;
        private Sprite sprite;
        // Use this for initialization
        void Start() {
            maxHealth = entityType.MaxHealth;
            range = entityType.Range;
            movementSpeed = entityType.MovementSpeed;
            attackSpeed = entityType.AttackSpeed;
            damage = entityType.Damage;
            sprite = entityType.EntitySprite;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}