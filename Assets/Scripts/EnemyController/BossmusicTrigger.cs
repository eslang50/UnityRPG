using System.Collections;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class BossmusicTrigger : MonoBehaviour
    {
        public AudioSource bossMusic; 
        public AudioClip bossMusicClip;
        public AudioSource backgroundMusic;
        public AudioClip backgroundMusicClip;
        public AudioClip dragonRoar;
        public GameObject player;
        public GameObject boss;
        private bool musicPlaying = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player && !musicPlaying) 
            {
                StartBossMusic();
            }
        }

        // Start playing the boss music
        private void StartBossMusic()
        {
            if (bossMusic != null && bossMusicClip != null && dragonRoar != null)
            {

                StartCoroutine(PlaySoundsInSequence());
                musicPlaying = true;
            }
        }

        private IEnumerator PlaySoundsInSequence()
        {
            backgroundMusic.Pause();

            bossMusic.clip = dragonRoar;
            bossMusic.Play();
            yield return new WaitForSeconds(dragonRoar.length);  // Wait for roar to finish
            bossMusic.clip = bossMusicClip;
            bossMusic.Play();
        }

        // Call this method when the boss dies to stop the music with a fade-out effect
        public void StopBossMusic()
        {
            StartCoroutine(FadeOutMusic(2f)); // 2 seconds fade-out duration
            musicPlaying = false;
            backgroundMusic.Play();
        }

        // Coroutine to fade the music out over time
        private IEnumerator FadeOutMusic(float fadeDuration)
        {
            float startVolume = bossMusic.volume;

            // Gradually decrease the volume
            while (bossMusic.volume > 0)
            {
                bossMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            // Ensure the volume is exactly 0 before stopping
            bossMusic.Stop();
            bossMusic.volume = startVolume;  // Reset volume to initial value
        }


    }
}