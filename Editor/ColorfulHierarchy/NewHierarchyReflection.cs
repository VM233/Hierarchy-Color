#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.HierarchyColor
{
    internal static class NewHierarchyReflection
    {
        private static FieldInfo viewItemField;
        private static FieldInfo nodeField;
        private static FieldInfo handlerField;
        private static MethodInfo getGameObjectMethod;
        private static Type getGameObjectMethodHandlerType;

        public static GameObject GetGameObject(VisualElement row)
        {
            try
            {
                var itemContainer = VisualElementSearchUtility.FindFirst(row,
                    element => element.GetType().FullName == NewHierarchyConstants.ItemContainerTypeName);
                if (itemContainer == null)
                {
                    return null;
                }

                EnsureItemContainerReflection(itemContainer.GetType());

                var viewItem = viewItemField?.GetValue(itemContainer);
                if (viewItem == null)
                {
                    return null;
                }

                var node = nodeField?.GetValue(viewItem);
                var handler = handlerField?.GetValue(viewItem);
                if (node == null || handler == null)
                {
                    return null;
                }

                EnsureHandlerReflection(handler.GetType());
                if (getGameObjectMethod == null)
                {
                    return null;
                }

                object[] args = { node };
                return getGameObjectMethod.Invoke(handler, args) as GameObject;
            }
            catch
            {
                return null;
            }
        }

        private static void EnsureItemContainerReflection(Type itemContainerType)
        {
            viewItemField ??= itemContainerType.GetField("m_ViewItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var viewItemType = viewItemField?.FieldType;
            if (viewItemType == null)
            {
                return;
            }

            nodeField ??= viewItemType.GetField("m_Node",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            handlerField ??= viewItemType.GetField("m_Handler",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private static void EnsureHandlerReflection(Type handlerType)
        {
            if (getGameObjectMethodHandlerType == handlerType)
            {
                return;
            }

            getGameObjectMethodHandlerType = handlerType;
            getGameObjectMethod = handlerType.GetMethod("GetGameObject",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}
#endif
