#region 注释
/*
 *     @File:          DownloadDef
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   下载参数定义
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月21日-17:06:26
 *     @Copyright  Copyright (c) 2023
 */
#endregion


namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadDef
    {
        /// <summary>
        /// 最大下载通道数量
        /// </summary>
        public static int MAX_CHANNEL_COUNT_LIMIT = 8;

        /// <summary>
        /// 统计下载速度间隔时间(秒)
        /// </summary>
        public static float STATISTICS_DOWNLOAD_SPEED_INTERVAL = 1;

        /// <summary>
        /// 任务迟滞检测时长
        /// </summary>
        public static float TASK_HYSTERESIS_CHECK_INTERVAL = 6;

        /// <summary>
        /// 任务迟滞流量阈值
        /// </summary>
        public static int TASK_HYSTERESIS_CHECK_BYTES = 10 * 1024;

        public static string GetMD5HashFromStream(System.IO.Stream stream)
        {
            try
            {
                stream.Position = 0;
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("GetMD5HashFromStream() fail,error:" + ex.Message);
            }
        }
    }
}