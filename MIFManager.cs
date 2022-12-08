using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OpenMIF
{
    /// <summary>
    /// MIF 파일 관리 클래스
    /// </summary>
    class MIFManager
    {
        #region Constants
        /// <summary>
        /// 미션명 최대 글자 수
        /// </summary>
        public const int MAX_MISSION_TITLE = 32;
        /// <summary>
        /// 정식 미션명 최대 글자 수
        /// </summary>
        public const int MAX_OFFICIAL_MISSION_TITLE = 128;
        /// <summary>
        /// 블럭 데이터 경로 최대 글자 수
        /// </summary>
        public const int MAX_BLOCKDATA_PATH = 128;
        /// <summary>
        /// 포인트 데이터 경로 최대 글자 수
        /// </summary>
        public const int MAX_POINTDATA_PATH = 128;
        /// <summary>
        /// 브리핑 이미지 1 경로 최대 글자 수
        /// </summary>
        public const int MAX_BRIEFING_IMAGE1_PATH = 256;
        /// <summary>
        /// 브리핑 이미지 2 경로 최대 글자 수
        /// </summary>
        public const int MAX_BRIEFING_IMAGE2_PATH = 256;
        /// <summary>
        /// 추가 소품 경로 최대 글자 수
        /// </summary>
        public const int MAX_ADD_SMALLOBEJCT_PATH = 256;
        /// <summary>
        /// 브리핑 메시지 최대 글자 수
        /// </summary>
        public const int MAX_BRIEFING_MESSAGE = 800;
        #endregion

        #region Enums
        /// <summary>
        /// 하늘 플래그
        /// </summary>
        public enum SkyFlag
        {
            NONE = 0,
            SUNNY,
            CLOUDY,
            NIGHT,
            DINNER,
            SUNSET,
        }

        /// <summary>
        /// 옵션 플래그
        /// </summary>
        public enum OptionFlag
        {
            NONE = 0,
            ADD_COLLISION = 1,
            DARK_SCREEN = 2,
            ALL = (ADD_COLLISION + DARK_SCREEN),
        }
        #endregion

        #region Member Variables

        #endregion

        #region Properties
        /// <summary>
        /// 미션명을 취득하거나 설정합니다.
        /// </summary>
        public string MissionTitle { get; set; }
        /// <summary>
        /// 정식 미션명을 취득하거나 설정합니다.
        /// </summary>
        public string OfficialMissionTitle { get; set; }
        /// <summary>
        /// 블럭 데이터 파일 경로를 취득하거나 설정합니다.
        /// </summary>
        public string BlockDataPath { get; set; }
        /// <summary>
        /// 포인트 데이터 파일 경로를 취득하거나 설정합니다.
        /// </summary>
        public string PointDataPath { get; set; }
        /// <summary>
        /// 하늘 유형을 취득하거나 설정합니다.
        /// </summary>
        public SkyFlag SkyType { get; set; }
        /// <summary>
        /// 옵션 유형을 취득하거나 설정합니다.
        /// </summary>
        public OptionFlag Options { get; set; }
        /// <summary>
        /// 브리핑 이미지 1 경로를 취득하거나 설정합니다.
        /// </summary>
        public string BriefingImage1Path { get; set; }
        /// <summary>
        /// 브리핑 이미지 2 경로를 취득하거나 설정합니다.
        /// </summary>
        public string BriefingImage2Path { get; set; }
        /// <summary>
        /// 추가 소품 경로를 취득하거나 설정합니다.
        /// </summary>
        public string AddSmallObjectPath { get; set; }
        /// <summary>
        /// 브리핑 메시지를 취득하거나 설정합니다.
        /// </summary>
        public string BriefingMessage { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public MIFManager() {
            this.MissionTitle = string.Empty;
            this.OfficialMissionTitle = string.Empty;
            this.BlockDataPath = string.Empty;
            this.PointDataPath = string.Empty;
            this.BriefingImage1Path = string.Empty;
            this.BriefingImage2Path = string.Empty;
            this.AddSmallObjectPath = string.Empty;
            this.BriefingMessage = string.Empty;
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        /// <summary>
        /// MIF 파일을 읽어옵니다.
        /// </summary>
        /// <param name="filepath">MIF 파일의 경로</param>
        /// <returns>성공(true), 실패(false)</returns>
        public bool Load(string filepath, int codepage = 0) {
            if (string.IsNullOrEmpty(filepath) == true) { return false; }
            if (string.Equals(Path.GetExtension(filepath), ".mif") == false) { return false; }

            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read)) {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(codepage))) {
                    // 미션명
                    this.MissionTitle = sr.ReadLine();

                    // 정식 미션명
                    this.OfficialMissionTitle = sr.ReadLine();

                    // 블럭 데이터 경로
                    this.BlockDataPath = sr.ReadLine();

                    // 포인트 데이터 경로
                    this.PointDataPath = sr.ReadLine();

                    // 하늘 유형
                    this.SkyType = (SkyFlag)Enum.Parse(typeof(SkyFlag), sr.ReadLine());

                    // 옵션 유형
                    this.Options = (OptionFlag)Enum.Parse(typeof(OptionFlag), sr.ReadLine());

                    // 추가 소품 경로
                    this.AddSmallObjectPath = sr.ReadLine();

                    // 브리핑 이미지 1 경로
                    this.BriefingImage1Path = sr.ReadLine();

                    // 브리핑 이미지 2 경로
                    this.BriefingImage2Path = sr.ReadLine();

                    // 브리핑 메시지
                    this.BriefingMessage = sr.ReadToEnd();
                }
            }

            return true;
        }

        /// <summary>
        /// MIF 파일을 저장합니다.
        /// </summary>
        /// <param name="filepath">MIF 파일의 경로</param>
        /// <returns>성공(true), 실패(false)</returns>
        public bool Save(string filepath) {
            if (string.IsNullOrEmpty(filepath) == true) { return false; }
            if (string.Equals(Path.GetExtension(filepath), ".mif") == false) { return false; }

            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write)) {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(0))) {
                    // 미션명
                    sw.WriteLine(this.MissionTitle);

                    // 정식 미션명
                    sw.WriteLine(this.OfficialMissionTitle);

                    // 블럭 데이터 경로
                    sw.WriteLine(this.BlockDataPath);

                    // 포인트 데이터 경로
                    sw.WriteLine(this.PointDataPath);

                    // 하늘 유형
                    sw.WriteLine((int)this.SkyType);

                    // 옵션 유형
                    sw.WriteLine((int)this.Options);

                    // 추가 소품 경로
                    if (string.Equals(this.AddSmallObjectPath, "") == true) { sw.WriteLine("!"); }
                    else { sw.WriteLine(this.AddSmallObjectPath); }

                    // 브리핑 이미지 1 경로
                    sw.WriteLine(this.BriefingImage1Path);

                    // 브리핑 이미지 2 경로
                    if (string.Equals(this.BriefingImage2Path, "") == true) { sw.WriteLine("!"); }
                    else { sw.WriteLine(this.BriefingImage2Path); }

                    // 브리핑 메시지
                    sw.Write(this.BriefingMessage);
                }
            }

            return true;
        }
        #endregion
    }
}
