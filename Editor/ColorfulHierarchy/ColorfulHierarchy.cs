#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    [InitializeOnLoad]
    public class ColorfulHierarchy
    {
        static ColorfulHierarchy()
        {
            EditorApplication.hierarchyWindowItemByEntityIdOnGUI += OnHierarchyWindow;
        }

        private static void OnHierarchyWindow(EntityId instanceID, Rect selectionRect)
        {
            var instance = EditorUtility.EntityIdToObject(instanceID);

            if (instance == null)
            {
                return;
            }

            foreach (var preset in HierarchyColorSettings.instance.ColorPresets)
            {
                if (string.IsNullOrEmpty(preset.keyChar))
                {
                    continue;
                }

                if (instance.name.TrimStart().StartsWith(preset.keyChar))
                {
                    string newName = instance.name[preset.keyChar.Length..];

                    EditorGUI.DrawRect(selectionRect, preset.backgroundColor);

                    GUIStyle newStyle = new()
                    {
                        alignment = preset.textAlignment,
                        fontStyle = preset.fontStyle,
                        normal = new GUIStyleState()
                        {
                            textColor = preset.textColor,
                        }
                    };

                    if (preset.autoUpperLetters)
                    {
                        newName = newName.ToUpper();
                    }

                    EditorGUI.LabelField(selectionRect, newName, newStyle);
                }
            }
        }
    }
}
#endif
