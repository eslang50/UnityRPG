using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class DamageTester : MonoBehaviour
    {
        public AttributesManager playerAtm;
        public AttributesManager enemyAtm;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                playerAtm.Attack(enemyAtm.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                enemyAtm.Attack(playerAtm.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                playerAtm.GainExperience(10);
            }
        }
    }
}