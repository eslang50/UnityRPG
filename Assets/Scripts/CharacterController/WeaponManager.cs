using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class WeaponManager : MonoBehaviour
    {
        public GameObject weapon;

        public void EnableWeaponCollider(bool isEnable)
        {
            if (weapon == null) return;

            var col = weapon.GetComponent<Collider>();  

            if (col == null) return;

            col.enabled = isEnable;
        }
    }
}