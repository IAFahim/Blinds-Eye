using System.Collections;
using System.Collections.Generic;
using Objects.Classes;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Audio
{
    [DeclareHorizontalGroup("vars")]
    public class PlayAudioGroup : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioGroup[] audioGroups;
        public AudioGroup[] audioGroupsEnglish;
        public AudioGroup[] audioGroupsBangla;
        public float[] cutoffMap;

        [Group("vars")] public int currentIndex;
        [Group("vars")] public int nextIndex;

        public float length;


        public List<AudioClip> audioClips;

        private Coroutine playCoroutine;

        [Button]
        public void Play(string text, Vector3 position)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            audioClips = new List<AudioClip>();
            transform.position = position;
            string[] words = text.Split(' ');
            cutoffMap = new float[words.Length];
            foreach (var word in words)
            {
                audioGroups = audioGroupsEnglish;

                if (AudioSourcePool.languageIndex == 1)
                {
                    audioGroups = audioGroupsBangla;
                }

                foreach (var audioGroup in audioGroups)
                {
                    audioGroup.LabelClipDictionary.TryGetValue(word, out AudioClip clip);
                    if (clip != null)
                    {
                        cutoffMap[audioClips.Count] = audioGroup.cutoff;
                        audioClips.Add(clip);
                        length += clip.length - audioGroup.cutoff;
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
                yield return new WaitForSeconds(audioSource.clip.length - cutoffMap[currentIndex]);
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
            cutoffMap = null;
            AudioSourcePool.Release(this);
        }

        private void OnDisable()
        {
            End();
        }
    }
}