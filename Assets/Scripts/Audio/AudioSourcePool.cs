using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    public class AudioSourcePool : MonoBehaviour
    {
        public PlayAudioGroup audioGroupObject;
        private static ObjectPool<PlayAudioGroup> pool;
        private AudioClip[] audioClips;

        [SerializeField] private int index = 0;

        private void Start()
        {
            pool = new ObjectPool<PlayAudioGroup>(() => Instantiate(audioGroupObject),
                (go) => go.gameObject.SetActive(true),
                (go) => go.gameObject.SetActive(false),
                Destroy,
                false, 6, 10);
        }

        [Button]
        public PlayAudioGroup Get(string text, Vector3 position)
        {
            var audioGroup = pool.Get();
            audioGroup.Play(text, position);
            return audioGroup;
        }

        [Button]
        public Vector2 Get(string text = "person", float distance = 4, float degreeZ = 45, float x = 640,
            float y = 480, float width = 1280, float height = 720, float xFov = 60, float yFov = 40)
        {
            Vector3 position = Vector3.forward * distance;

            float wRatio = x / width;
            float hRatio = y / height;

            float xOffsetDegree = ((180 - xFov) / 2) + wRatio * xFov;
            float yOffsetDegree = ((180 - yFov) / 2) + hRatio * yFov;


            Quaternion targetRotation = Quaternion.Euler(0, degreeZ, 0);
            position = targetRotation * position;

            xOffsetDegree = (xOffsetDegree - 90);
            yOffsetDegree = (yOffsetDegree - 90);
            position.x = xOffsetDegree / 10;
            position.y = yOffsetDegree / 10;

            var audioGroup = pool.Get();
            audioGroup.Play(text, position);
            Debug.Log($"xOffsetDegree: {xOffsetDegree}, yOffsetDegree: {yOffsetDegree}");
            return new Vector2(xOffsetDegree, yOffsetDegree);
        }

        IEnumerator PlayAfterDelay(PlayAudioGroup audioGroup, string text, Vector3 position)
        {
            yield return new WaitForSeconds(((float)index) * 0.8f);
            index++;
            audioGroup.Play(text, position);
        }


        public static void Release(PlayAudioGroup audioGroup)
        {
            pool.Release(audioGroup);
        }
    }
}