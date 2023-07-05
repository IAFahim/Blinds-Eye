using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;
using CJM.BBox2DToolkit;

namespace Audio
{
    public class AudioSourcePool : MonoBehaviour
    {
        public PlayAudioGroup[] audioGroupObjects;
        private static ObjectPool<PlayAudioGroup>[] _pools = new ObjectPool<PlayAudioGroup>[2];
        public static int languageIndex = 0;
        public float fov=1.2f;

        public enum PlaybackMode
        {
            Sequential,
            Random
        }

        public void SwitchLanguage(int index)
        {
            languageIndex = index;
        }

        [Button]
        public void CycleLanguage()
        {
            languageIndex = (languageIndex + 1) % audioGroupObjects.Length;
        }


        private void Start()
        {
            for (var i = 0; i < audioGroupObjects.Length; i++)
            {
                var audioGroupObject = audioGroupObjects[i];
                _pools[i] = new ObjectPool<PlayAudioGroup>(() => Instantiate(audioGroupObject),
                    (go) =>
                    {
                        if (go != null) go.gameObject.SetActive(true);
                    },
                    (go) => go.gameObject.SetActive(false),
                    Destroy,
                    true, 6, 10);
            }
        }

        public Vector3 dir;

        [Button]
        public void Get(float distance = 2, float degreeZ = 0,
            string text = "person", float x = 640, float y = 360,
            float width = 1280, float height = 720, bool inverse = true)
        {
            var audioGroup = _pools[languageIndex].Get();
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

            dir = new Vector3(xRatiio * fov, yRatiio * fov, 1);

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
                        var audioGroup = _pools[languageIndex].Get();
                        audioGroup.Play(bbox2DInfo.label,
                            CalculatePosition((int)(bbox2DInfos[i].bbox.x0 + (bbox2DInfos[i].bbox.width / 2f)), (int)(bbox2DInfos[i].bbox.y0 - (bbox2DInfos[i].bbox.height / 2f)), width, height, 2, 0,
                                false));
                        yield return new WaitForSeconds(audioGroup.audioSource.clip.length - audioGroup.length);
                    }

                    break;

                case PlaybackMode.Random:
                    int[] randomIndexes = GenerateRandomIndexes(length);

                    for (int i = 0; i < length; i++)
                    {
                        int randomIndex = randomIndexes[i];
                        var bbox2DInfo = bbox2DInfos[randomIndex];
                        PlayAudioGroup audioGroup = _pools[languageIndex].Get();
                        audioGroup.Play(bbox2DInfo.label, CalculatePosition(bbox2DInfo.bbox.x0, bbox2DInfo.bbox.y0,
                            width, height, 2, 0,
                            true));
                        yield return new WaitForSeconds(audioGroup.audioSource.clip.length - audioGroup.cutOff);
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
                _pools[languageIndex].Release(audioGroup);
            }
        }
    }
}