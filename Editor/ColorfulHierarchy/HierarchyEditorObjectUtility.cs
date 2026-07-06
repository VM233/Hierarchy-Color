#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VMFramework.HierarchyColor
{
    internal static class HierarchyEditorObjectUtility
    {
        public static long GetObjectID(Object obj)
        {
            if (obj == null)
            {
                return 0;
            }

#if UNITY_6000_5_OR_NEWER
            return (long)EntityId.ToULong(obj.GetEntityId());
#else
            return obj.GetInstanceID();
#endif
        }
    }
}
#endif
