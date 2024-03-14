#region 注释
/*
 *     @File:          DownloadHandlerScript
 *     @NameSpace:     DownloadUtils
 *     @Description:   DES
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-9:39:19
 *     @Copyright  Copyright (c) 2023
 */
#endregion


using System.IO;
using UnityEngine.Networking;

namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadHandlerScript : UnityEngine.Networking.DownloadHandlerScript
    {
        public DownloadHandlerScript(DownloaderManager owner, UnityWebRequest request, string fullSavePath, string fullUrl, string hashCode)
        {
            this.m_Owner = owner;
            this.m_Request = request;
            this.m_FullSavePath = fullSavePath;
            this.m_FullUrl = fullUrl;
            this.m_HashCode = hashCode;
            FileInfo fileInfo = new FileInfo(fullSavePath);
            DirectoryInfo dirInfo = fileInfo.Directory;
            if (dirInfo is { Exists: false })
            {
                dirInfo.Create();
            }
        }

        public DownloadHandlerScript(DownloaderManager owner, UnityWebRequest request, int buffSize, string fullUrl, string hashCode)
        {
            this.m_Owner = owner;
            this.m_Request = request;
            this.m_FullSavePath = "";
            this.m_FullUrl = fullUrl;
            this.m_HashCode = hashCode;
            this.m_DownloadStream = new MemoryStream(buffSize);
        }

        ~DownloadHandlerScript()
        {
            this.Close();
        }


        public void OpenStream()
        {
            if (this.IsDownloadFile)
            {
                this.m_DownloadStream = System.IO.File.Create(this.m_FullSavePath);
            }
        }

        public void Close()
        {
            try
            {
                if (this.m_DownloadStream is { CanWrite: true })
                {
                    this.m_DownloadStream.Close();
                }
            }
            catch (System.Exception ex)
            {
                DownloadLog.LogError(ex);
            }
            finally
            {
                this.m_DownloadStream?.Close();
            }
        }

        public bool IsDownloadFile => !string.IsNullOrEmpty(this.m_FullSavePath);

        public bool IsDownloadMemory => string.IsNullOrEmpty(this.m_FullSavePath);

        // 请求对象
        public UnityWebRequest m_Request;

        // 管理器
        public DownloaderManager m_Owner;

        // 起始下载索引
        private static int _startDownloadSerialId = 1;

        // 下载索引
        public int m_DownloadSerialId = _startDownloadSerialId++;

        // 完整保存路径
        public string m_FullUrl;

        // 文件哈希校验码(默认MD5,建议CRC)
        public string m_HashCode;

        // 完整下载地址
        public string m_FullSavePath;

        // 下载流
        public Stream m_DownloadStream;

        // 是否检查hash值
        private bool m_CheckHashCode;

        public bool IsCheckHashCode
        {
            get
            {
                m_CheckHashCode = !string.IsNullOrEmpty(this.m_HashCode);
                return m_CheckHashCode;
            }
        }

        private async void AsReceiveFile(byte[] downloadData, int receivingLength)
        {
            try
            {
                await this.m_DownloadStream.WriteAsync(downloadData, 0, receivingLength, System.Threading.CancellationToken.None);

                // 记录下载尺寸(统计下载速度使用)
                this.m_Owner.m_LastSumBytes += receivingLength;

                // 通知下载更新
                this.m_Owner.ManualNoticeDownloadUpdate(this);
            }
            catch (System.Exception ex)
            {
                this.m_Owner.ManualNoticeDownloadFailure(this, ex.ToString());
            }
        }

        private async void AsReceiveData(byte[] downloadData, int receivingLength)
        {
            try
            {
                await this.m_DownloadStream.WriteAsync(downloadData, 0, receivingLength, System.Threading.CancellationToken.None);
                // 记录下载尺寸(统计下载速度使用)
                this.m_Owner.m_LastSumBytes += receivingLength;
                // 通知下载更新
                this.m_Owner.ManualNoticeDownloadUpdate(this);
            }
            catch (System.Exception ex)
            {
                this.m_Owner.ManualNoticeDownloadFailure(this, ex.ToString());
            }
        }

        // 接收数据后的回调
        protected override bool ReceiveData(byte[] downloadData, int receivingLength)
        {
            // 触发异步处理(减少主逻辑占用)
            if (this.m_FullSavePath != null)
            {
                this.AsReceiveFile(downloadData, receivingLength);
            }
            else
            {
                this.AsReceiveData(downloadData, receivingLength);
            }

            // 返回基类调用
            return base.ReceiveData(downloadData, receivingLength);
        }
    }
}