using System.Collections.Generic;
using UnityEngine;

namespace Objects.Classes
{
    public abstract class AudioGroup : ScriptableObject
    {
        public LabelClipList labelClipList;

        public string folderPath = "Audio/";
        public readonly Dictionary<string, AudioClip> LabelClipDictionary = new();
        public float cutoff=0.8f;
        
        protected AudioClip LoadSingleClip(string label)
        {
            string audioClipAddress = System.IO.Path.Combine(folderPath, $"{label}");

            Resources.Load<AudioClip>(audioClipAddress);
            AudioClip audioClip = Resources.Load<AudioClip>(audioClipAddress);

            if (audioClip == null)
            {
                Debug.LogWarning("Audio clip not found at path: " + audioClipAddress);
            }

            return audioClip;
        }

        public AudioClip this[string label] => LoadSingleClip(label);
    }
}