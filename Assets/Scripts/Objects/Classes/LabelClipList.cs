using UnityEngine;

namespace Objects.Classes
{
    [System.Serializable]
    public class LabelClipList
    {
        public LabelClip[] items;

        public LabelClipList(int size)
        {
            items = new LabelClip[size];
        }

        public string GetLabel(int index)
        {
            return items[index].label;
        }

        public AudioClip GetClip(int index)
        {
            return items[index].clip;
        }

        public int GetLength()
        {
            return items.Length;
        }

        public void Set(int index, LabelClip labelClip)
        {
            items[index] = labelClip;
        }
    }
}