using System.Collections.Generic;
using Objects.Classes;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    [DeclareHorizontalGroup("vars")]
    public class PlayAudioGroup : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioGroup[] audioGroups;

        [Group("vars")] public int currentIndex;
        [Group("vars")] public int nextIndex;
        [Group("vars")] public int length;

        public List<AudioClip> audioClips;

        [Button]
        public void Play(string text, Vector3 position)
        {
            if (string.IsNullOrEmpty(text))
            {
                length = 0;
                return;
            }

            audioClips = new List<AudioClip>();
            transform.position = position;
            string[] words = text.Split(' ');

            foreach (var word in words)
            {
                foreach (var audioGroup in audioGroups)
                {
                    audioGroup.LabelClipDictionary.TryGetValue(word, out AudioClip clip);
                    if (clip != null)
                    {
                        audioClips.Add(clip);
                    }
                }
            }

            if (!gameObject.activeInHierarchy || audioClips.Count == 0)
            {
                gameObject.SetActive(true);
            }

            audioSource.Play();
        }

        public void Update()
        {
            if (!audioSource.isPlaying)
            {
                if (nextIndex < audioClips.Count)
                {
                    currentIndex = nextIndex++;
                    audioSource.clip = audioClips[currentIndex];
                    audioSource.Play();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void End()
        {
            currentIndex = 0;
            nextIndex = 0;
            length = 0;
            audioClips.Clear();
            audioSource.Stop();
            audioSource.clip = null;
            AudioSourcePool.Release(this);
        }

        private void OnDisable()
        {
            End();
        }
    }
}