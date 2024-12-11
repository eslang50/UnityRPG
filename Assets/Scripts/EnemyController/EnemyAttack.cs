using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class GoblinAttack : MonoBehaviour
    {
        private GameObject player; // Reference to the player object
        private AttributesManager playerAttributes;

        [SerializeField] private float attackRange = 1f; // Range within which the attack can hit the player
        [SerializeField] private LayerMask playerLayer;   // Layer for detecting the player

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

            // Check if the player is within attack range
            if (IsPlayerInAttackRange())
            {
                playerAttributes.TakeDamage(GetComponent<AttributesManager>().attack);
                Debug.Log("Player hit!");
            }
            else
            {
                Debug.Log("Player dodged the attack!");
            }
        }

        // Check if the player is in range
        private bool IsPlayerInAttackRange()
        {
            // Perform a sphere cast to detect the player within the attack range
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
            Debug.Log(hits);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject == player)
                {
                    return true; // Player is within range
                }
            }

            return false; // Player is out of range
        }


    }
}
