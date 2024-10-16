using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class AttributesManager : MonoBehaviour
    {
        public int health;
        public int attack;
        public int armor;
        public float critDamage = 1.5f;
        public float critChance = 0.1f;

        public void TakeDamage(int amount)
        {
            health -= amount - (amount * armor / 100);
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            StatusPopUpGenerator.instance.CreatePopUp(transform.position + randomOffset, amount.ToString(), Color.yellow);
        }

        public void Attack(GameObject target)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm == null) return;
            atm.TakeDamage((int)(Random.Range(0f, 1f) < critChance ? attack * critDamage : attack));
        }
    }
}