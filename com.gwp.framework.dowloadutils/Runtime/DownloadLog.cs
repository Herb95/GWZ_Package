#region 注释
/*
 *     @File:          DownloadLog
 *     @NameSpace:     Gwp.FrameWork.DownloadUtils
 *     @Description:   日志输出
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年3月14日-13:37:10
 *     @Copyright  Copyright (c) 2023
 */
#endregion


using UnityEngine;

namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadLog
    {
        public static void LogInfo(object message)
        {
            Debug.unityLogger.Log(LogType.Error, message);
        }

        public static void LogWarn(object message)
        {
            Debug.unityLogger.Log(LogType.Warning, message);
        }

        public static void LogError(object message)
        {
            Debug.unityLogger.Log(LogType.Error, message);
        }
    }
}