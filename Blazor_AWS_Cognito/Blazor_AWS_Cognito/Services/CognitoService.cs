using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Amazon;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using System.Security.Cryptography;
using System.Text;

namespace Blazor_AWS_Cognito.Services
{
    /// <summary>
    /// AWS Cognito用のサービス
    /// </summary>
    public class CognitoService
    {
        /// <summary>
        /// アプリクライアントID
        /// </summary>
        private readonly string clientId;
        /// <summary>
        /// ユーザープールID
        /// </summary>
        private readonly string userPoolId;
        /// <summary>
        /// アプリクライアントのシークレット
        /// </summary>
        private readonly string clientSecret;
        /// <summary>
        /// リージョン
        /// </summary>
        private readonly string region;
        /// <summary>
        /// 認証クライアント
        /// </summary>
        private readonly AmazonCognitoIdentityProviderClient providerClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CognitoService()
        {
            clientId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            userPoolId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            clientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            region = "ap-northeast-1_xxxxxxxxxxxxxxxxxxxxx";
            providerClient = new AmazonCognitoIdentityProviderClient(
                    new AnonymousAWSCredentials(),
                    RegionEndpoint.GetBySystemName(region));
        }

        /// <summary>
        /// 新規アカウントのサインアップ
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <param name="password">パスワード</param>
        /// <param name="email">メールアドレス</param>
        public async Task SignUpAsync(string userName, string password, string email)
        {
            try
            {
                // クライアントシークレットを生成が有効時はハッシュ化する
                var secretHash = CreateSecretHash(userName, clientId, clientSecret);

                var signUpRequest = new SignUpRequest
                {
                    ClientId = clientId,
                    SecretHash = secretHash,
                    Username = userName,
                    Password = password,
                    UserAttributes = { new AttributeType { Name = "email", Value = email } }
                };

                var signUpResponse = await providerClient.SignUpAsync(signUpRequest);
            }
            catch (Exception e)
            {
                // エラー処理
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// サインイン
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <param name="password">パスワード</param>
        /// <returns>アクセストークン</returns>
        public async Task<string> SignInAsync(string userName, string password)
        {
            try
            {
                var cognitoUserPool = new CognitoUserPool(userPoolId, clientId, providerClient);
                var cognitoUser = new CognitoUser(userName, clientId, cognitoUserPool, providerClient, clientSecret);

                // SRP (Secure Remote Password) 認証
                var response = await cognitoUser.StartWithSrpAuthAsync(new InitiateSrpAuthRequest { Password = password });
                return response.AuthenticationResult.AccessToken;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"エラーが発生しました: {e.Message}");
                return "";
            }
        }

        /// <summary>
        /// ユーザープールからユーザー削除
        /// </summary>
        /// <param name="accessToken">アクセストークン</param>
        public async Task DeleteUserAsync(string accessToken)
        {
            try
            {
                var request = new DeleteUserRequest
                {
                    AccessToken = accessToken
                };
                var response = await providerClient.DeleteUserAsync(request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// ユーザーのカスタム属性に値を設定
        /// </summary>
        /// <param name="accessToken">アクセストークン</param>
        /// <param name="attributeName">カスタム属性名（custom:〇〇 という命名規則）</param>
        /// <param name="attributeValue">カスタム属性値</param>
        public async Task UpdateUserAsync(string accessToken, string attributeName, string attributeValue)
        {
            try
            {
                // ユーザーの属性を更新するリクエストを作成
                var updateUserAttributesRequest = new UpdateUserAttributesRequest
                {
                    AccessToken = accessToken,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType
                        {
                            Name = attributeName,
                            Value = attributeValue
                        }
                    }
                };

                // 属性を更新
                var response = await providerClient.UpdateUserAttributesAsync(updateUserAttributesRequest);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"エラーが発生しました: {e.Message}");
            }
        }

        /// <summary>
        /// カスタム属性を取得
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <param name="attributeName">カスタム属性名（custom:〇〇 という命名規則）</param>
        /// <returns>カスタム属性</returns>
        public async Task<AttributeType?> GetAttributeAsync(string userName, string accessToken, string attributeName)
        {
            try
            {
                var cognitoUserPool = new CognitoUserPool(userPoolId, clientId, providerClient);
                var cognitoUser = new CognitoUser(userName, clientId, cognitoUserPool, providerClient, clientSecret);

                // アクセストークンを使用してユーザーを認証する
                cognitoUser.SessionTokens = new CognitoUserSession(null, accessToken, null, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

                var getUserResponse = await cognitoUser.GetUserDetailsAsync();
                var userAttribute = getUserResponse.UserAttributes.Where(x => x.Name == attributeName).SingleOrDefault();
                return userAttribute;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"{e.Message}");
                return null;
            }
        }

        /// <summary>
        /// SECRET_HASHを作成
        /// </summary>
        /// <param name="username">ユーザー名</param>
        /// <param name="clientId">アプリクライアントID</param>
        /// <param name="clientSecret">アプリクライアントのシークレット</param>
        /// <returns>ハッシュ値</returns>
        private string CreateSecretHash(string username, string clientId, string clientSecret)
        {
            var data = username + clientId;
            var key = Encoding.UTF8.GetBytes(clientSecret);

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
