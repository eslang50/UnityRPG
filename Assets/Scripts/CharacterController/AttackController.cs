using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.SetTrigger("BasicAttackOne");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.SetTrigger("SpecialAttackOne");
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("MeleeAttack");
            }
        }
    }
}