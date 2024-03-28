namespace Blazor_AWS_Cognito.Data.Models
{
    /// <summary>
    /// Cognito の接続設定
    /// </summary>
    public class CognitoSettings
    {
        /// <summary>
        /// クライアントシークレット
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// リージョン
        /// </summary>
        public string? Region { get; set; }
    }
}
