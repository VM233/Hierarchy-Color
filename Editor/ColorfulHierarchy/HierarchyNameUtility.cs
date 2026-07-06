#if UNITY_EDITOR
namespace VMFramework.HierarchyColor
{
    internal static class HierarchyNameUtility
    {
        public static bool TryGetPreset(string objectName, out HierarchyColorPreset preset)
        {
            preset = null;
            if (string.IsNullOrEmpty(objectName))
            {
                return false;
            }

            foreach (var candidate in HierarchyColorSettings.instance.ColorPresets)
            {
                if (string.IsNullOrEmpty(candidate.keyChar))
                {
                    continue;
                }

                if (StartsWithKeyIgnoringLeadingWhitespace(objectName, candidate.keyChar))
                {
                    preset = candidate;
                    return true;
                }
            }

            return false;
        }

        public static int GetStartIndexIgnoringWhitespace(string value)
        {
            int index = 0;
            while (index < value.Length && char.IsWhiteSpace(value[index]))
            {
                index++;
            }

            return index;
        }

        public static bool StartsWithKeyIgnoringLeadingWhitespace(string value, string key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return false;
            }

            int startIndex = GetStartIndexIgnoringWhitespace(value);
            if (startIndex + key.Length > value.Length)
            {
                return false;
            }

            return string.CompareOrdinal(value, startIndex, key, 0, key.Length) == 0;
        }
    }
}
#endif
