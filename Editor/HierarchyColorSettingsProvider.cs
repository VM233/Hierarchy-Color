#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    public static class HierarchyColorSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/Hierarchy Color", SettingsScope.Project)
            {
                label = "Hierarchy Color",
                keywords = new HashSet<string>
                {
                    "Hierarchy",
                    "Color",
                    "Icon",
                    "Component"
                },
                guiHandler = _ =>
                {
                    var settings = HierarchyColorSettings.instance;
                    settings.EnsureInitialized();

                    var serializedSettings = new SerializedObject(settings);
                    serializedSettings.Update();

                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("colorPresets"), true);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("maxIconNum"),
                        new GUIContent("Max Icon Num"));
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("iconSize"),
                        new GUIContent("Icon Size"));

                    if (serializedSettings.ApplyModifiedProperties())
                    {
                        settings.SaveSettings();
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }
            };
        }
    }
}
#endif
