using GRisk.Data;
using UnityEngine;

namespace GRisk.Interface
{
    public class GRSound : MonoBehaviour
    {
        public void playsound(string name, Transform location, float volume = 1.0f)
        {
            if (!GRData.soundLibrary.ContainsKey(name))
            {
                Debug.LogError($"Sound '{name}' not found in the sound library.");
                return;
            }

            AudioClip clip = GRData.soundLibrary[name];

            playClipAtPoint(clip, location.position, volume);
        }

        private static void playClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f,
            float spatialBlend = 0.95f)
        {
            // ripped from AudioSource.PlayClipAtPoint with some modifications

            GameObject gameObject = new("One shot audio");
            gameObject.transform.position = position;

            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSource.clip = clip;
            audioSource.spatialBlend = spatialBlend;
            audioSource.volume = volume;
            audioSource.Play();

            Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
        }
    }
}