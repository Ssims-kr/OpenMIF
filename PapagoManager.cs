using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace OpenMIF
{
    /// <summary>
    /// 파파고 API 관리 클래스
    /// </summary>
    class PapagoManager
    {
        #region Properties
        /// <summary>
        /// 발급받은 파파고 API ID를 취득하거나 설정합니다.
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 발급받은 파파고 API Secret를 취득하거나 설정합니다.
        /// </summary>
        public string ClientSecret { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public PapagoManager() {
            this.ClientID = string.Empty;
            this.ClientSecret = string.Empty;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="client_id">발급받은 파파고 API ID</param>
        /// <param name="client_secret">발급받은 파파고 API Secret</param>
        public PapagoManager(string client_id, string client_secret) {
            this.ClientID = client_id;
            this.ClientSecret = client_secret;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 파파고 API 관련 내용을 불러옵니다.
        /// </summary>
        /// <param name="filepath">파일 경로</param>
        /// <returns>성공(true), 실패(false)</returns>
        public bool Load(string filepath) {
            if (string.IsNullOrEmpty(filepath) == true) { return false; }
            if (File.Exists(filepath) == false) { return false; }

            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sr = new StreamReader(fs)) {
                    this.ClientID = sr.ReadLine();
                    this.ClientSecret = sr.ReadLine();
                }
            }

            return true;
        }

        /// <summary>
        /// 파파고 API 관련 내용을 저장합니다.
        /// </summary>
        /// <param name="filepath">파일 경로</param>
        /// <returns>성공(true), 실패(false)</returns>
        public bool Save(string filepath) {
            if (string.IsNullOrEmpty(filepath) == true) { return false; }

            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write)) {
                using (StreamWriter sw = new StreamWriter(fs)) {
                    sw.WriteLine(this.ClientID);
                    sw.WriteLine(this.ClientSecret);
                }
            }

            return true;
        }

        /// <summary>
        /// 일본어를 한국어로 번역합니다.
        /// </summary>
        /// <param name="text">번역할 텍스트</param>
        /// <returns>성공(<see cref="string"/>), 실패(<see cref="Nullable"/>)</returns>
        public string? TranslateJA2KO(string text) {
            if (string.IsNullOrEmpty(this.ClientID) == true) { return null; }
            if (string.IsNullOrEmpty(this.ClientSecret) == true) { return null; }

            const string url = "https://openapi.naver.com/v1/papago/n2mt";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", this.ClientID);
            request.Headers.Add("X-Naver-Client-Secret", this.ClientSecret);
            request.Method = "POST";

            byte[] byteDataParams = Encoding.UTF8.GetBytes($"source=ja&target=ko&text={text.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\v", "")}");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;

            using (Stream st = request.GetRequestStream()) {
                st.Write(byteDataParams, 0, byteDataParams.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string json;
            using (Stream st = response.GetResponseStream()) {
                using (StreamReader sr = new StreamReader(st, Encoding.UTF8)) {
                    json = sr.ReadToEnd();
                }
            }

            // Json Parse
            var j = JObject.Parse(json);
            return j["message"]["result"]["translatedText"].ToString();
        }
        #endregion
    }
}
