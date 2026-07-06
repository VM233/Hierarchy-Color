#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    [InitializeOnLoad]
    public class ColorfulHierarchy
    {
        private static readonly GUIStyle hierarchyLabelStyle = new();

        static ColorfulHierarchy()
        {
#if UNITY_6000_4_OR_NEWER
            EditorApplication.hierarchyWindowItemByEntityIdOnGUI += OnHierarchyWindow;
#else
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
#endif
            EditorApplication.update += NewHierarchyWindowController.UpdateWhenDue;
            EditorApplication.hierarchyChanged += NewHierarchyWindowController.RepaintAll;
        }

#if UNITY_6000_4_OR_NEWER
        private static void OnHierarchyWindow(EntityId entityID, Rect selectionRect)
#else
        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
#endif
        {
            var settings = HierarchyColorSettings.instance;
            if (!settings.EnableHighlight)
            {
                return;
            }

#if UNITY_6000_4_OR_NEWER
            var instance = EditorUtility.EntityIdToObject(entityID);
#else
            var instance = EditorUtility.InstanceIDToObject(instanceID);
#endif

            if (instance == null)
            {
                return;
            }

            foreach (var preset in settings.ColorPresets)
            {
                if (string.IsNullOrEmpty(preset.keyChar))
                {
                    continue;
                }

                if (!HierarchyNameUtility.StartsWithKeyIgnoringLeadingWhitespace(instance.name, preset.keyChar))
                {
                    continue;
                }

                int nameStartIndex =
                    HierarchyNameUtility.GetStartIndexIgnoringWhitespace(instance.name) + preset.keyChar.Length;
                string newName = instance.name[nameStartIndex..];

                EditorGUI.DrawRect(selectionRect, preset.backgroundColor);

                hierarchyLabelStyle.alignment = preset.textAlignment;
                hierarchyLabelStyle.fontStyle = preset.fontStyle;
                hierarchyLabelStyle.normal.textColor = preset.textColor;

                if (preset.autoUpperLetters)
                {
                    newName = newName.ToUpper();
                }

                EditorGUI.LabelField(selectionRect, newName, hierarchyLabelStyle);
            }
        }
    }
}
#endif
