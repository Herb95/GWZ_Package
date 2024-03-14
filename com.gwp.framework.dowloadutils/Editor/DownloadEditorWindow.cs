#region 注释
/*
 *     @File:          DownloadEditorWindow
 *     @NameSpace:     Gwp.FrameCode.DownloadUtils.Editor
 *     @Description:   下载编辑器窗口查看器
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月23日-15:51:54
 *     @Copyright  Copyright (c) 2023
 */
#endregion

using System;
using UnityEditor;
using UnityEngine;

namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloadEditorWindow : EditorWindow
    {
        [MenuItem("Tools/EditorTools/DownloadEditorWindow", false, 1)]
        static void ShowWindow()
        {
            DownloadEditorWindow window = EditorWindow.GetWindow<DownloadEditorWindow>(false, "Download Window");
            window.minSize = new Vector2(270.0f, 300.0f);
            window.name = "Download Window";
            window.Show();
        }

        private static DownloaderManager _downloadMgr;
        private static Action<DownloadArgs> _successAction;

        public static DownloaderManager CreateDownloadLoader()
        {
            DownloaderManager downloadMgr = new DownloaderManager();
            downloadMgr.OnDownloadFailure += OnDownloadFailureHandler;
            downloadMgr.OnDownloadSuccess += OnDownloadSuccessHandler;
            return downloadMgr;
        }

        private static void OnDownloadFailureHandler(DownloadArgs param)
        {
            if (param is DownloadFailureArgs)
            {
                Debug.LogError(param.ErrorMsg + $" FullUrl: {param.FullUrl}");
            }
        }

        private static void OnDownloadSuccessHandler(DownloadArgs param)
        {
            if (param is DownloadSuccessArgs)
            {
                // Log.Info("[下载成功] 保存位置: " + param.FullUrl);
                _successAction?.Invoke(param);
            }
        }


        public static void StartDownloadFile(string savePath, string downloadUri, string hashCode, Action<DownloadArgs> successAction = null, Action<DownloadArgs> errorAction = null)
        {
            _successAction = successAction;
            int id = AddDownload(savePath, downloadUri, hashCode);
            if (id != -1) return;
            errorAction?.Invoke(new DownloadFailureArgs(null, 0, "没有正确分配下载的队列id!!!!!!!"));
        }

        private static int AddDownload(string downloadPath, string downloadUri, string hashCode)
        {
            _downloadMgr ??= CreateDownloadLoader();
            return _downloadMgr.AddDownload(downloadPath, downloadUri, hashCode);
        }
    }
}