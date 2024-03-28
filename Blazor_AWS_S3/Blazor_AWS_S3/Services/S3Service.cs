using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Blazor_AWS_S3.Services
{
    /// <summary>
    /// AWS S3アクセス用サービス
    /// </summary>
    public class S3Service
    {
        /// <summary>
        /// アクセスキー
        /// </summary>
        private readonly string _accessKey;

        /// <summary>
        /// シークレットキー
        /// </summary>
        private readonly string _secretKey;

        /// <summary>
        /// バケット名
        /// </summary>
        private readonly string _bucketName;

        /// <summary>
        /// リージョン
        /// </summary>
        private readonly string _region;

        /// <summary>
        /// アクセスベースパス
        /// </summary>
        private readonly string _accessRoutePath;

        /// <summary>
        /// リージョンエンドポイント
        /// </summary>
        private readonly RegionEndpoint _regionEndpoint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public S3Service()
        {
            _accessKey = "xxxxxxxxxxxxxxxxxx";
            _secretKey = "xxxxxxxxxxxxxxxxxx";
            _bucketName = "xxxxxxxxxxxxxxxxxx";
            _region = "ap-northeast-xxxx";
            _accessRoutePath = "xxxxxxxxxxxxxxxxxx";

            _regionEndpoint = RegionEndpoint.GetBySystemName(_region);
        }

        /// <summary>
        /// S3からファイルを取得する
        /// </summary>
        /// <typeparam name="T">デシリアライズしたいクラス</typeparam>
        /// <param name="fileName">ファイル名</param>
        /// <returns>デシリアライズ後のオブジェクト</returns>
        public async Task<T?> ReadObjectDataAsync<T>(string fileName)
        {
            // S3クライアントの設定
            var s3Client = new AmazonS3Client(_accessKey, _secretKey, _regionEndpoint);

            // JSONファイルのキーを生成
            var fileKey = $"{_accessRoutePath}/{fileName}";

            // S3からファイルを取得
            using (var fileStream = await ReadObjectDataAsync(s3Client, _bucketName, fileKey))
            {
                if (fileStream != null)
                {
                    // JSONをデシリアライズしてオブジェクトに変換
                    using (var sr = new StreamReader(fileStream))
                    {
                        var jsonContent = await sr.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<T>(jsonContent);
                    }
                }
                else
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// S3からファイルを取得する
        /// </summary>
        /// <param name="client">S3クライアント</param>
        /// <param name="bucketName">バケット名</param>
        /// <param name="key">ファイルキー</param>
        /// <returns>取得データ</returns>
        public static async Task<MemoryStream?> ReadObjectDataAsync(AmazonS3Client client, string bucketName, string key)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                {
                    var memoryStream = new MemoryStream();
                    responseStream.CopyTo(memoryStream);
                    memoryStream.Position = 0; // メモリストリームの位置をリセット
                    return memoryStream;
                }
            }
            catch (AmazonS3Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
