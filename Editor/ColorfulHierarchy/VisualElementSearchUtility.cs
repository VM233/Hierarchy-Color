#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace VMFramework.HierarchyColor
{
    internal static class VisualElementSearchUtility
    {
        public static VisualElement FindFirst(VisualElement root, Func<VisualElement, bool> predicate)
        {
            if (root == null)
            {
                return null;
            }

            if (predicate(root))
            {
                return root;
            }

            for (int i = 0; i < root.hierarchy.childCount; i++)
            {
                var result = FindFirst(root.hierarchy.ElementAt(i), predicate);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static IEnumerable<VisualElement> FindAll(VisualElement root, Func<VisualElement, bool> predicate)
        {
            if (root == null)
            {
                yield break;
            }

            if (predicate(root))
            {
                yield return root;
            }

            for (int i = 0; i < root.hierarchy.childCount; i++)
            {
                foreach (var child in FindAll(root.hierarchy.ElementAt(i), predicate))
                {
                    yield return child;
                }
            }
        }
    }
}
#endif
