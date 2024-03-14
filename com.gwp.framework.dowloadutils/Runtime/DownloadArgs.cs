#region 注释
/*
 *     @File:          DownloadArgs
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   下载参数
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-9:38:04
 *     @Copyright  Copyright (c) 2023
 */
#endregion


namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadArgs
    {
        private DownloadHandlerScript _handler;
        public DownloadHandlerScript Handler => this._handler;
        public string MCheckingResult { get; set; } = string.Empty;
        public string ErrorMsg { get; protected set; } = string.Empty;
        public int DownloadSerialId { get; } = 0;
        public string FullSavePath { get; }
        public string FullUrl { get; }
        public string HashMD5 { get; }
        private ulong _mDownloadBytes = 0;

        public DownloadArgs(DownloadHandlerScript handler, ulong downloadedBytes = 0)
        {
            this._handler = handler;
            this.DownloadSerialId = handler.m_DownloadSerialId;
            this.FullSavePath = handler.m_FullSavePath;
            this.FullUrl = handler.m_FullUrl;
            this.HashMD5 = handler.m_HashCode;
            this._mDownloadBytes = downloadedBytes;
        }

        public byte[] DownloadBytes
        {
            get
            {
                long originalPos = _handler.m_DownloadStream.Position;
                _handler.m_DownloadStream.Position = 0;
                byte[] data = new byte[_handler.m_DownloadStream.Length];
                int readData = _handler.m_DownloadStream.Read(data, 0, data.Length);
                _handler.m_DownloadStream.Position = originalPos;
                return data;
            }
        }

        public long GetLength => _handler.m_DownloadStream.Length;
        public long GetDownloadSize => (long)this._mDownloadBytes;
        
        public static string FormatDateSize(float s, int round = 2)
        {
            string suffix = "B";
            if (s > 1024)
            {
                s /= 1024;
                suffix = "KB";
            }
            if (s > 1024)
            {
                s /= 1024;
                suffix = "MB";
            }
            if (s > 1024)
            {
                s /= 1024;
                suffix = "GB";
            }
            return System.Math.Round(s, round).ToString("F2") + suffix;
        }
    }
}