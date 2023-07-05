using System.Collections;
using System.Collections.Generic;
using Objects.Classes;
using TriInspector;
using UnityEngine;

namespace Audio
{
    [DeclareHorizontalGroup("vars")]
    public class PlayAudioGroup : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioGroup[] audioGroups;
        public int languageIndex = 0;
        
        public float cutOff=0.8f;

        [Group("vars")]
        public int currentIndex;
        [Group("vars")]
        public int nextIndex;
        [Group("vars")]
        public int length;
        
        
        public List<AudioClip> audioClips;

        private Coroutine playCoroutine;

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

            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }

            playCoroutine = StartCoroutine(PlayAudioCoroutine());
        }

        private IEnumerator PlayAudioCoroutine()
        {
            nextIndex = 0;

            while (nextIndex < audioClips.Count)
            {
                currentIndex = nextIndex++;
                audioSource.clip = audioClips[currentIndex];
                audioSource.Play();

                yield return new WaitForSeconds(audioSource.clip.length - audioGroups[0].cutoff);
            }
            End();
            yield return null;
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
