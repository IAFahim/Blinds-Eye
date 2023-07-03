using System;
using Objects.Classes;
using TriInspector;
using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "NumberObject", menuName = "ScriptableObjects/NumberObject", order = 1)]
    public class NumberObject : AudioGroup
    {
        private void OnEnable()
        {
            Populate();
        }

        [Button]
        public void Populate(int count = 30)
        {
            labelClipList = new LabelClipList(count + 1);

            for (int i = 0; i <= count; i++)
            {
                string label = i.ToString();
                AudioClip clip = LoadSingleClip(i.ToString());
                var audioResource = new LabelClip
                {
                    label = label,
                    clip = clip
                };
                labelClipList.Set(i, audioResource);
                LabelClipDictionary.Add(label, clip);
            }
        }
    }
}