#if UNITY_EDITOR
using System;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    [Serializable]
    public class HierarchyColorPreset
    {
        public string keyChar;

        public Color textColor = Color.white;

        public Color backgroundColor = Color.black;

        public TextAnchor textAlignment = TextAnchor.MiddleCenter;

        public FontStyle fontStyle = FontStyle.Bold;

        public bool autoUpperLetters = true;
    }
}
#endif
