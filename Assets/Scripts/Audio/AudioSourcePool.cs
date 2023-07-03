using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;
using CJM.BBox2DToolkit;

namespace Audio
{
    public class AudioSourcePool : MonoBehaviour
    {
        public PlayAudioGroup audioGroupObject;
        private static ObjectPool<PlayAudioGroup> _pool;

        public enum PlaybackMode
        {
            Sequential,
            Random
        }

        private void Start()
        {
            _pool = new ObjectPool<PlayAudioGroup>(() => Instantiate(audioGroupObject),
                (go) =>
                {
                    if (go != null) go.gameObject.SetActive(true);
                },
                (go) => go.gameObject.SetActive(false),
                Destroy,
                true, 6, 10);
        }

        public Vector3 dir;

        [Button]
        public void Get(float distance = 2, float degreeZ = 0,
            string text = "person", float x = 640, float y = 360,
            float width = 1280, float height = 720, bool inverse = true)
        {
            var audioGroup = _pool.Get();
            audioGroup.Play(text, CalculatePosition(x, y, width, height, distance, degreeZ, inverse));
        }

        public Vector3 CalculatePosition(float x, float y, float width, float height, float distance, float degreeZ,
            bool inverse)
        {
            float xRatiio = 2 * (x / width) - 1;
            float yRatiio = 2 * (y / height) - 1;
            Debug.Log(xRatiio);
            if (inverse)
            {
                xRatiio = -xRatiio;
            }

            dir = new Vector3(xRatiio, yRatiio, 1);

            Vector3 position = dir * distance;


            Quaternion targetRotation = Quaternion.Euler(0, degreeZ, 0);
            return targetRotation * position;
        }


        public void Get(BBox2DInfo[] bbox2DInfos, PlaybackMode playbackMode,
            float distance = 2, float degreeZ = 0,
            float width = 1280, float height = 720, bool inverse = true)
        {
            StartCoroutine(PlayBBox2DInfosSequentially(bbox2DInfos, playbackMode, width, height));
        }

        private IEnumerator PlayBBox2DInfosSequentially(BBox2DInfo[] bbox2DInfos, PlaybackMode playbackMode,
            float width,
            float height)

        {
            int length = bbox2DInfos.Length;

            switch (playbackMode)
            {
                case PlaybackMode.Sequential:
                    for (int i = 0; i < length; i++)
                    {
                        var bbox2DInfo = bbox2DInfos[i];
                        var audioGroup = _pool.Get();
                        audioGroup.Play(bbox2DInfo.label,
                            CalculatePosition(bbox2DInfo.bbox.x0, bbox2DInfo.bbox.y0, width, height, 2, 0,
                                true));
                        yield return new WaitForSeconds(audioGroup.audioSource.clip.length - audioGroup.endCutoff);
                    }

                    break;

                case PlaybackMode.Random:
                    int[] randomIndexes = GenerateRandomIndexes(length);

                    for (int i = 0; i < length; i++)
                    {
                        int randomIndex = randomIndexes[i];
                        var bbox2DInfo = bbox2DInfos[randomIndex];
                        var audioGroup = _pool.Get();
                        audioGroup.Play(bbox2DInfo.label, CalculatePosition(bbox2DInfo.bbox.x0, bbox2DInfo.bbox.y0,
                            width, height, 2, 0,
                            true));
                        yield return new WaitForSeconds(audioGroup.audioSource.clip.length - audioGroup.endCutoff);
                    }

                    break;
            }
            yield return null;
        }

        private int[] GenerateRandomIndexes(int length)
        {
            int[] indexes = new int[length];
            for (int i = 0; i < length; i++)
            {
                indexes[i] = i;
            }

            // Shuffle the indexes array using Fisher-Yates algorithm
            for (int i = length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
            }

            return indexes;
        }


        public static void Release(PlayAudioGroup audioGroup)
        {
            //check if object that has already been released to the pool.
            if (audioGroup.gameObject.activeSelf)
            {
                _pool.Release(audioGroup);
            }
        }
    }
}