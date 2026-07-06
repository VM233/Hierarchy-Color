#if UNITY_EDITOR
namespace VMFramework.HierarchyColor
{
    internal static class NewHierarchyConstants
    {
        public const double WindowScanIntervalSeconds = 0.5;
        public const int RefreshIntervalMs = 16;

        public const string WindowTypeName = "Unity.Hierarchy.Editor.HierarchyWindow";
        public const string RowName = "unity-multi-column-view__row-container";
        public const string ItemContainerTypeName = "Unity.Hierarchy.HierarchyViewItemContainer";

        public const string NameClass = "hierarchy-item__name";
        public const string DefaultIconClass = "hierarchy-item__icon";
        public const string LeftCustomSectionClass = "hierarchy-item__left-custom-section";

        public const string IconRootClass = "hierarchy-color-component-icons";
        public const string MainIconClass = "hierarchy-color-main-component-icon";
        public const string MainIconHostClass = "hierarchy-color-main-component-icon-host";
    }
}
#endif
