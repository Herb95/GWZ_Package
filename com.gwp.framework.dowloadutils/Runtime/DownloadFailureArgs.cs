#region 注释
/*
 *     @File:          DownloadFailureArgs
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:  下载失败参数
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-11:47:29
 *     @Copyright  Copyright (c) 2023
 */
#endregion


namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadFailureArgs : DownloadArgs
    {
        public DownloadFailureArgs(DownloadHandlerScript handler, ulong downloadedBytes, string errMsg) : base(handler, downloadedBytes)
        {
            this.ErrorMsg = errMsg;
        }
    }
}