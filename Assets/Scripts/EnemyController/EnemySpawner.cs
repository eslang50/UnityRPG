using FantasyRpg.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private float _minSpawnTime;

    [SerializeField]
    private float _maxSpawnTime;

    [SerializeField]
    private Vector3 _spawnAreaSize; // Defines the size of the spawn area

    [SerializeField]
    private float _minSpawnDistance = 1.5f; // Minimum distance between enemies to avoid overlaps

    [SerializeField]
    private int _maxEnemies = 5; // Maximum number of enemies allowed on the map at once

    private float _timeUntilSpawn;

    void Start()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        // Only spawn if there are fewer than the maximum number of enemies
        if (_timeUntilSpawn <= 0 && GetCurrentEnemyCount() < _maxEnemies)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition != Vector3.zero)
            {
                GameObject enemyInstance = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                enemyInstance.tag = "Enemy";
                enemyInstance.layer = LayerMask.NameToLayer("Enemy");

                // Attach AttributesManager component and initialize stats
                AttributesManager attributesManager = enemyInstance.AddComponent<AttributesManager>();
                InitializeEnemyAttributes(attributesManager);
            }

            SetTimeUntilSpawn();
        }
    }

    private void InitializeEnemyAttributes(AttributesManager attributesManager)
    {
        // Set the stats for the enemy
        attributesManager.characterName = "Enemy";
        attributesManager.maxHealth = 100;
        attributesManager.currentHealth = 100;
        attributesManager.maxMana = 50;
        attributesManager.currentMana = 50;
        attributesManager.maxXp = 100;
        attributesManager.currentXp = 0;
        attributesManager.currentLevel = 1;
        attributesManager.attack = 10;
        attributesManager.armor = 5;
        attributesManager.critDamage = 1.5f;
        attributesManager.critChance = 0.1f;
        attributesManager.healthRegen = 1;
        attributesManager.manaRegen = 1;
        attributesManager.xpValue = 20;
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int attempts = 10;
        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-_spawnAreaSize.x / 2, _spawnAreaSize.x / 2),
                0,
                Random.Range(-_spawnAreaSize.z / 2, _spawnAreaSize.z / 2)
            );

            Vector3 spawnPosition = transform.position + randomPosition;

            RaycastHit rayHit;

            if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out rayHit, Mathf.Infinity) && !Physics.CheckSphere(spawnPosition, _minSpawnDistance))
            {
                // Update spawn position with the ground y-coordinate
                spawnPosition.y = rayHit.point.y;
                return spawnPosition;
            }
        }

        return Vector3.zero;
    }

    private int GetCurrentEnemyCount()
    {
        // Get all enemies in the scene by searching for objects with the "Enemy" tag or use your own method
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _spawnAreaSize);
    }
}
