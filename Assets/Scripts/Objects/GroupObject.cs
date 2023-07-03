using Newtonsoft.Json;
using Objects.Classes;
using TriInspector;
using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "LabelsLoader", menuName = "ScriptableObjects/LabelObject", order = 1)]
    public class GroupObject : AudioGroup
    {
        public TextAsset colorMapFile;
        
        private void OnEnable()
        {
            LoadLabels();
        }
        
        [Button]
        private void LoadLabels()
        {
            string jsonContent = colorMapFile.text;
            labelClipList = JsonConvert.DeserializeObject<LabelClipList>(jsonContent);
            foreach (var labelData in labelClipList.items)
            {
                labelData.clip = LoadSingleClip(labelData.label);
                LabelClipDictionary.Add(labelData.label, labelData.clip);
            }
        }
    }
}