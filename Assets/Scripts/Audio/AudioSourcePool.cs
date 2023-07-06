using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;
using CJM.BBox2DToolkit;
using UnityEngine.Events;

namespace Audio
{
    public class AudioSourcePool : MonoBehaviour
    {
        public PlayAudioGroup audioGroupObject;
        private static ObjectPool<PlayAudioGroup> _pool;
        public static int languageIndex = 0;
        public float fov = 1.2f;
        public static bool busy;

        public enum PlaybackMode
        {
            Sequential,
            Random
        }

        public void SwitchLanguage(int index)
        {
            languageIndex = index;
        }
        
        public UnityEvent onToggleLanguage;

        [Button]
        public void CycleLanguage()
        {
            languageIndex = (languageIndex + 1) % 2;
            onToggleLanguage?.Invoke();
        }


        private void Start()
        {
            _pool= new ObjectPool<PlayAudioGroup>(() => Instantiate(audioGroupObject),
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
            float xRatio = Balence(x, width, inverse) * fov;
            float yRatio = Balence(y, height, false) * fov;
            Debug.Log($"width: {width}, height: {height}, x: {x}, y: {y}, xRatio: {xRatio}, yRatio: {yRatio}");
            dir = new Vector3(xRatio, yRatio, 1);
            Vector3 position = dir * distance;
            Quaternion targetRotation = Quaternion.Euler(0, degreeZ, 0);
            return targetRotation * position;
        }

        public float Balence(float x, float size, bool inverse)
        {
            float ratio = 2 * (x / size) - 1;
            bool negative = ratio < 0;
            if (negative)
            {
                ratio = -ratio;
            }

            ratio = Mathf.Sqrt(ratio) * (negative ? -1 : 1);

            if (inverse)
            {
                ratio = -ratio;
            }

            return ratio;
        }


        public void Get(BBox2DInfo[] bbox2DInfos, PlaybackMode playbackMode,
            float distance = 2, float degreeZ = 0,
            float width = 1280, float height = 720, bool inverse = true)
        {
            busy = true;
            StartCoroutine(PlayBBox2DInfosSequentially(bbox2DInfos, playbackMode, width, height, inverse));
        }

        private IEnumerator PlayBBox2DInfosSequentially(BBox2DInfo[] bbox2DInfos, PlaybackMode playbackMode,
            float width,
            float height, bool inverse)

        {
            int length = bbox2DInfos.Length;

            switch (playbackMode)
            {
                case PlaybackMode.Sequential:
                    for (int i = 0; i < length; i++)
                    {
                        int x = (int)(bbox2DInfos[i].bbox.x0 + (bbox2DInfos[i].bbox.width / 2f));
                        int y = (int)(bbox2DInfos[i].bbox.y0 - (bbox2DInfos[i].bbox.height / 2f));
                        var bbox2DInfo = bbox2DInfos[i];
                        var audioGroup = _pool.Get();
                        audioGroup.Play(bbox2DInfo.label,
                            CalculatePosition(x, y, width, height, 2, 0, inverse));
                        yield return new WaitForSeconds(audioGroup.length);
                    }

                    break;

                case PlaybackMode.Random:
                    int[] randomIndexes = GenerateRandomIndexes(length);

                    for (int i = 0; i < length; i++)
                    {
                        int randomIndex = randomIndexes[i];
                        var bbox2DInfo = bbox2DInfos[randomIndex];
                        PlayAudioGroup audioGroup = _pool.Get();
                        audioGroup.Play(bbox2DInfo.label, CalculatePosition(bbox2DInfo.bbox.x0, bbox2DInfo.bbox.y0,
                            width, height, 2, 0,
                            true));
                        yield return new WaitForSeconds(audioGroup.length);
                    }

                    break;
            }
            busy = false;
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