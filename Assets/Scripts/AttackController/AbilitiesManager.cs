using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class AbilitiesManager : MonoBehaviour
    {
        public int basicAttackOneLevel = 1;
        public int meleeAttackLevel = 1;
        public int specialAttackOneLevel = 1;
        public int specialAttackTwoLevel = 1;

        public int basicAttackOneManaCost = 10;
        public int specialAttackOneManaCost = 25;
        public int specialAttackTwoManaCost = 35;

        public int basicAttackOneCooldown = 1;
        public int specialAttackOneCooldown = 2;
        public int specialAttackTwoCooldown = 5;
    }
}