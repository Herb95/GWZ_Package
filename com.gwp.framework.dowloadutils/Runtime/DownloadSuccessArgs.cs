#region 注释
/*
 *     @File:          DownloadSuccessArgs
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   DES
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-12:00:16
 *     @Copyright  Copyright (c) 2023
 */
#endregion
using System.IO;

namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadSuccessArgs : DownloadArgs
    {
        public DownloadSuccessArgs(DownloadHandlerScript handler, ulong downloadedBytes) : base(handler, downloadedBytes)
        {
        }

        public string GetSuccessMsg()
        {
            string formatLen = FormatDateSize(GetDownloadSize);
            FileInfo f = new FileInfo(FullSavePath);
            string fName = f.Exists ? f.Name : string.Empty;
            return $"[下载成功]: {fName} --- Size:{formatLen}";
        }
    }
}