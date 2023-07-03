using TriInspector;
using UnityEngine;

namespace Objects
{
    [DeclareHorizontalGroup("vars")]
    [System.Serializable]
    public class LabelClip
    {
        [Group("vars")] public string label;
        [Group("vars")] public AudioClip clip;
    }
}