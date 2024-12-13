using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FantasyRpg.Combat;

public class DeathScreen : MonoBehaviour
{
    public Canvas deathCanvas;
    public Button restartButton;
    public GameObject player;

    private AttributesManager playerAttributes;

    void Start()
    {
        deathCanvas.enabled = false;

        if (player != null)
        {
            playerAttributes = player.GetComponent<AttributesManager>();

            if (playerAttributes == null)
            {
                Debug.LogError("AttributesManager component not found on the assigned player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not assigned in the DeathScreen script.");
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("Restart Button not assigned in the DeathScreen script.");
        }
    }

    void Update()
    {
        if (playerAttributes != null && playerAttributes.currentHealth <= 0 && !deathCanvas.enabled)
        {
            Debug.Log("Player health is 0. Triggering death screen.");
            TriggerDeathScreen();
        }
    }

    public void TriggerDeathScreen()
    {
        deathCanvas.enabled = true;
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
