#region 注释
/*
 *     @File:          IgnoreHttps
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   Https 校验执行跳过
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-13:47:12
 *     @Copyright  Copyright (c) 2023
 */
#endregion


using UnityEngine.Networking;

namespace Gwp.FrameCode.DownloadUtils
{
    public class IgnoreHttps : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}