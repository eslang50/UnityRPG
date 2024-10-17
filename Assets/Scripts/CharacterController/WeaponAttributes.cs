using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class WeaponAttributes : MonoBehaviour
    {
        public AttributesManager atm;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            other.GetComponent<AttributesManager>().TakeDamage(atm.attack);
        }
    }
}