using GRisk.Data;
using UnityEngine;

namespace GRisk.Interface
{
    public class GRSound : MonoBehaviour
    {
        private AudioSource soundSource; 
        
        private void Awake()
        {
            soundSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }

        public void playsound(string name, Transform location, float volume = 1.0f)
        {
            if (!GRData.soundLibrary.ContainsKey(name))
            {
                Debug.LogError($"Sound '{name}' not found in the sound library.");
                return;
            }

            AudioClip clip = GRData.soundLibrary[name];
            
            soundSource.volume = volume;
            soundSource.spatialBlend = 0.8f; 
            
            Vector3 originalPosition = transform.position;
            transform.position = location.position;
            
            soundSource.PlayOneShot(clip);
            
            transform.position = originalPosition;
        }
    }
}