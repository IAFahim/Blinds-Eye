using Objects.Classes;
using TriInspector;
using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "LabelFolderObject", menuName = "ScriptableObjects/LabelFolderObject", order = 1)]
    public class GroupFolderObject: AudioGroup
    {
        private void OnEnable()
        {
            LoadAllClip();
        }

        [Button]
        public void LoadAllClip()
        {
            var list= Resources.LoadAll<AudioClip>(folderPath);
            labelClipList = new LabelClipList(list.Length);
            for (var i = 0; i < list.Length; i++)
            {
                var audioClip = list[i];
                var audioResource = new LabelClip
                {
                    label = audioClip.name,
                    clip = audioClip
                };
                labelClipList.Set(i, audioResource);
                LabelClipDictionary.Add(audioClip.name, audioClip);
            }
        }
    }
}