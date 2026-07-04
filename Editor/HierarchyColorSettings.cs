#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    [FilePath("ProjectSettings/HierarchyColorSettings.asset",
        FilePathAttribute.Location.ProjectFolder)]
    public sealed class HierarchyColorSettings : ScriptableSingleton<HierarchyColorSettings>
    {
        private const string SETTINGS_PATH = "ProjectSettings/HierarchyColorSettings.asset";
        private const int MIN_ICON_NUM = 1;
        private const int MAX_ICON_NUM = 10;
        private const int MIN_ICON_SIZE = 1;
        private const int MAX_ICON_SIZE = 24;

        [SerializeField]
        private List<HierarchyColorPreset> colorPresets = new();

        [SerializeField]
        private int maxIconNum = 5;

        [SerializeField]
        private int iconSize = 16;

        [InitializeOnLoadMethod]
        private static void EnsureSettingsFile()
        {
            if (File.Exists(SETTINGS_PATH))
            {
                return;
            }

            instance.SaveSettings();
        }

        public IReadOnlyList<HierarchyColorPreset> ColorPresets
        {
            get
            {
                EnsureInitialized();
                return colorPresets;
            }
        }

        public int MaxIconNum
        {
            get
            {
                EnsureInitialized();
                return maxIconNum;
            }
        }

        public int IconSize
        {
            get
            {
                EnsureInitialized();
                return iconSize;
            }
        }

        private void OnEnable()
        {
            EnsureInitialized();
        }

        private void OnValidate()
        {
            NormalizeValues();
        }

        internal void EnsureInitialized()
        {
            colorPresets ??= new();

            if (colorPresets.Count == 0)
            {
                AddDefaultColorPresets();
            }

            NormalizeValues();
        }

        internal void SaveSettings()
        {
            EnsureInitialized();
            EditorUtility.SetDirty(this);
            Save(true);
        }

        private void AddDefaultColorPresets()
        {
            colorPresets.Add(new()
            {
                keyChar = "$",
                textColor = Color.white,
                backgroundColor = new(1f, 0.6470588f, 0f, 1f)
            });

            colorPresets.Add(new()
            {
                keyChar = "^",
                textColor = Color.white,
                backgroundColor = Color.green
            });

            colorPresets.Add(new()
            {
                keyChar = "#",
                textColor = new(0f, 0.7490196f, 1f, 1f),
                backgroundColor = Color.yellow
            });

            colorPresets.Add(new()
            {
                keyChar = "@",
                textColor = new(1f, 0.4117647f, 0.7058824f, 1f),
                backgroundColor = new(0.4980392f, 1f, 0.8313726f, 1f)
            });

            colorPresets.Add(new()
            {
                keyChar = "/",
                textColor = Color.white,
                backgroundColor = Color.magenta
            });

            colorPresets.Add(new()
            {
                keyChar = "%",
                textColor = Color.white,
                backgroundColor = new(0.5f, 0f, 0.5f, 1f)
            });

            colorPresets.Add(new()
            {
                keyChar = "!",
                textColor = Color.white,
                backgroundColor = Color.red
            });

            colorPresets.Add(new()
            {
                keyChar = "&",
                textColor = Color.white,
                backgroundColor = Color.black
            });

            colorPresets.Add(new()
            {
                keyChar = "*",
                textColor = Color.white,
                backgroundColor = new(0f, 1f, 0.617f, 1f)
            });
        }

        private void NormalizeValues()
        {
            maxIconNum = Mathf.Clamp(maxIconNum, MIN_ICON_NUM, MAX_ICON_NUM);
            iconSize = Mathf.Clamp(iconSize, MIN_ICON_SIZE, MAX_ICON_SIZE);
        }
    }
}
#endif
