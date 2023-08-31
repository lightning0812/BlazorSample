namespace Blazor_DynamicComponent.Model
{
    /// <summary>
    /// コンポーネントの情報クラス
    /// </summary>
    public class ComponentMetadata
    {
        /// <summary>
        /// UIに表示する名前
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// コンポーネントに渡すパラメータ
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } =
            new Dictionary<string, object>();
    }
}
