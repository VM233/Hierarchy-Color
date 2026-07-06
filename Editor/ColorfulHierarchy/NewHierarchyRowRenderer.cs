#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.HierarchyColor
{
    internal static class NewHierarchyRowRenderer
    {
        private struct RowStyleState
        {
            public long ObjectID;
            public string ObjectName;
            public string DisplayName;
            public int SettingsHash;
            public bool ActiveInHierarchy;
            public HierarchyColorPreset Preset;
        }

        private static readonly Dictionary<VisualElement, RowStyleState> rowStyleStates = new();

        public static void ClearCache()
        {
            rowStyleStates.Clear();
        }

        public static void RemoveFromCache(VisualElement row)
        {
            rowStyleStates.Remove(row);
        }

        public static void Apply(VisualElement row)
        {
            var gameObject = NewHierarchyReflection.GetGameObject(row);
            var label = FindNameLabel(row);

            if (label == null)
            {
                Clear(row, null, null);
                return;
            }

            string objectName = gameObject != null ? gameObject.name : label.text;
            int settingsHash = GetSettingsHash();
            if (TrySkipUnchanged(row, gameObject, label, objectName, settingsHash))
            {
                return;
            }

            if (!HierarchyColorSettings.instance.EnableHighlight)
            {
                string originalName = objectName;
                if (gameObject == null && rowStyleStates.TryGetValue(row, out var state))
                {
                    originalName = state.ObjectName;
                }

                Clear(row, label, originalName);
                NewHierarchyIconRenderer.Draw(row, gameObject);
                Cache(row, gameObject, originalName, originalName, settingsHash, null);
                return;
            }

            if (gameObject == null)
            {
                if (!TryApplyCached(row, label))
                {
                    Clear(row, label, label.text);
                }

                return;
            }

            if (!HierarchyNameUtility.TryGetPreset(objectName, out var preset))
            {
                Clear(row, label, objectName);
                NewHierarchyIconRenderer.Draw(row, gameObject);
                Cache(row, gameObject, objectName, objectName, settingsHash, null);
                return;
            }

            ApplyPreset(row, label, objectName, preset, out var displayName);
            NewHierarchyIconRenderer.Draw(row, gameObject);
            Cache(row, gameObject, objectName, displayName, settingsHash, preset);
        }

        private static bool TrySkipUnchanged(VisualElement row, GameObject gameObject, Label label,
            string objectName, int settingsHash)
        {
            if (!rowStyleStates.TryGetValue(row, out var state))
            {
                return false;
            }

            if (state.ObjectID != HierarchyEditorObjectUtility.GetObjectID(gameObject) ||
                state.ObjectName != objectName ||
                state.SettingsHash != settingsHash ||
                state.ActiveInHierarchy != (gameObject == null || gameObject.activeInHierarchy))
            {
                return false;
            }

            return label.text == state.ObjectName || label.text == state.DisplayName;
        }

        private static void Cache(VisualElement row, GameObject gameObject, string objectName,
            string displayName, int settingsHash, HierarchyColorPreset preset)
        {
            rowStyleStates[row] = new()
            {
                ObjectID = HierarchyEditorObjectUtility.GetObjectID(gameObject),
                ObjectName = objectName,
                DisplayName = displayName,
                SettingsHash = settingsHash,
                ActiveInHierarchy = gameObject == null || gameObject.activeInHierarchy,
                Preset = preset
            };
        }

        private static bool TryApplyCached(VisualElement row, Label label)
        {
            if (!rowStyleStates.TryGetValue(row, out var state))
            {
                return false;
            }

            if (label.text != state.ObjectName && label.text != state.DisplayName)
            {
                return false;
            }

            if (state.Preset == null)
            {
                row.style.backgroundColor = StyleKeyword.Null;
                if (!string.IsNullOrEmpty(state.ObjectName))
                {
                    label.text = state.ObjectName;
                }

                label.style.color = StyleKeyword.Null;
                label.style.unityFontStyleAndWeight = StyleKeyword.Null;
                label.style.unityTextAlign = StyleKeyword.Null;
                NewHierarchyIconRenderer.Clear(row);
                return true;
            }

            ApplyPreset(row, label, state.ObjectName, state.Preset, out _);
            return true;
        }

        private static void ApplyPreset(VisualElement row, Label label, string objectName,
            HierarchyColorPreset preset, out string displayName)
        {
            displayName = objectName[preset.keyChar.Length..];
            if (preset.autoUpperLetters)
            {
                displayName = displayName.ToUpper();
            }

            row.style.backgroundColor = preset.backgroundColor;
            label.text = displayName;
            label.style.color = preset.textColor;
            label.style.unityFontStyleAndWeight = preset.fontStyle;
            label.style.unityTextAlign = preset.textAlignment;
        }

        private static void Clear(VisualElement row, Label label, string objectName)
        {
            rowStyleStates.Remove(row);
            row.style.backgroundColor = StyleKeyword.Null;

            if (label != null)
            {
                if (!string.IsNullOrEmpty(objectName))
                {
                    label.text = objectName;
                }

                label.style.color = StyleKeyword.Null;
                label.style.unityFontStyleAndWeight = StyleKeyword.Null;
                label.style.unityTextAlign = StyleKeyword.Null;
            }

            NewHierarchyIconRenderer.Clear(row);
        }

        private static int GetSettingsHash()
        {
            var settings = HierarchyColorSettings.instance;
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + settings.EnableHighlight.GetHashCode();
                hash = hash * 31 + settings.MaxIconNum;
                hash = hash * 31 + settings.IconSize;
                hash = hash * 31 + settings.ShowMainComponentIcon.GetHashCode();
                hash = hash * 31 + settings.EnableHierarchyIconTooltips.GetHashCode();

                foreach (var preset in settings.ColorPresets)
                {
                    hash = hash * 31 + (preset.keyChar != null ? preset.keyChar.GetHashCode() : 0);
                    hash = hash * 31 + preset.textColor.GetHashCode();
                    hash = hash * 31 + preset.backgroundColor.GetHashCode();
                    hash = hash * 31 + preset.textAlignment.GetHashCode();
                    hash = hash * 31 + preset.fontStyle.GetHashCode();
                    hash = hash * 31 + preset.autoUpperLetters.GetHashCode();
                }

                return hash;
            }
        }

        private static Label FindNameLabel(VisualElement row)
        {
            var nameElement = VisualElementSearchUtility.FindFirst(row,
                element => element.ClassListContains(NewHierarchyConstants.NameClass));
            return VisualElementSearchUtility.FindFirst(nameElement,
                element => element is Label label && !string.IsNullOrEmpty(label.text)) as Label;
        }
    }
}
#endif
