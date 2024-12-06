using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class GoblinAttack : MonoBehaviour
    {
        private GameObject player; // Reference to the player object
        private AttributesManager playerAttributes;

        private void Start()
        {
            player = GameObject.Find("PlayerArmature");
            // Ensure we find the player's AttributesManager
            if (player != null)
            {
                playerAttributes = player.GetComponent<AttributesManager>();
            }
            else
            {
                Debug.LogError("Player reference is missing in GoblinAttack!");
            }
        }

        // This method will be called by the animation event
        public void TriggerAttack()
        {
            if (playerAttributes == null) return;

            // Call the TakeDamage method on the player's AttributesManager
            playerAttributes.TakeDamage(GetComponent<AttributesManager>().attack);
        }
    }
}
