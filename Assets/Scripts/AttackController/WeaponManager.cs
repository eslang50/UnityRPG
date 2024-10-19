using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class WeaponManager : MonoBehaviour
    {
        public GameObject weapon;
        public vThirdPersonController tpc;

        public void EnableWeaponCollider(int isEnable)
        {
            if (weapon == null) return;

            var col = weapon.GetComponent<Collider>();

            if (col == null) return;

            col.enabled = isEnable == 1 ? true : false;
        }

        public void EnableMovement(bool isEnable)
        {
            if (tpc == null) return;

            tpc.lockMovement = !isEnable;
        }

        public void EnableRotation(bool isEnable)
        {
            if (tpc == null) return;

            tpc.lockRotation = !isEnable;
        }
    }
}