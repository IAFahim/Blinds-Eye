using Audio;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ToggleLanguage : MonoBehaviour
    {
        public string[] language;
        public TextMeshProUGUI text;

        public void Toggle()
        {
            text.text = language[AudioSourcePool.languageIndex % 2];
        }
    }
}