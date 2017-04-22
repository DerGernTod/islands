using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.entities {
    [CreateAssetMenu(fileName = "Data", menuName = "Entity/EntityType", order = 1)]
    public class EntityType : ScriptableObject {

        public float MaxHealth = 1;
        public float Range = 1;
        public float MovementSpeed = 1;
        public float AttackSpeed = 1;
        public float Damage = 1;
        public Sprite EntitySprite;
    }
}