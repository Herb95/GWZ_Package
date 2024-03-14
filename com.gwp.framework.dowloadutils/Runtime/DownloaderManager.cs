#region 注释
/*
 *     @File:          DownloaderManager
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   下载管理器
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-9:57:39
 *     @Copyright  Copyright (c) 2023
 */
#endregion


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Gwp.FrameCode.DownloadUtils
{
    public class DownloaderManager
    {
        public Dictionary<int, DownloadHandlerScript> m_Dict_ReadyDownload_Requests =
            new Dictionary<int, DownloadHandlerScript>();

        public Dictionary<int, UnityWebRequest> m_Dict_Downloading_Requests = new Dictionary<int, UnityWebRequest>();

        private bool _mIsUpdating = false;
        private ulong _mSumDownloadBytes = 0L;

        private int _mNeedGCCount = 0;

        /// <summary>
        /// 下载速度记录
        /// </summary>
        private float _mLastCalculationSpeedTime = Time.realtimeSinceStartup;

        /// <summary>
        /// 最后迟滞检测时间
        /// </summary>
        public float m_LastTaskHysteresisCheckTime = Time.realtimeSinceStartup;

        private float _mLastCalculationSpeed = 0;
        public float m_LastSumBytes = 0;
        public bool MIsNewSpeed = false;
        private bool _mNeedRestartAll = false;

        public delegate void OnDownloadStartDelegate(DownloadArgs param);

        public delegate void OnDownloadUpdateDelegate(DownloadArgs param);

        public delegate void OnDownloadSuccessDelegate(DownloadArgs param);

        public delegate void OnDownloadFailureDelegate(DownloadArgs param);

        /// <summary>
        /// 下载开始事件
        /// </summary>
        public event OnDownloadStartDelegate OnDownloadStart;

        /// <summary>
        /// 下载更新事件
        /// </summary>
        public event OnDownloadUpdateDelegate OnDownloadUpdate;

        /// <summary>
        /// 下载成功事事件
        /// </summary>
        public event OnDownloadSuccessDelegate OnDownloadSuccess;

        /// <summary>
        /// 下载失败事件
        /// </summary>
        public event OnDownloadFailureDelegate OnDownloadFailure;

        public void Reset()
        {
            this.RemoveAllDownloads();
            this._mSumDownloadBytes = 0L;
            _mIsUpdating = false;
        }

        public ulong GetCurrentSumDownloadLength()
        {
            return this._mSumDownloadBytes;
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="fullSavePath">保存地址</param>
        /// <param name="fullUrl">下载完整地址</param>
        /// <param name="hashCode">校验的hash</param>
        /// <returns></returns>
        public int AddDownload(string fullSavePath, string fullUrl, string hashCode)
        {
            DownloadHandlerScript handler = new DownloadHandlerScript(this, null, fullSavePath, fullUrl, hashCode);
            try
            {
                this.m_Dict_ReadyDownload_Requests.Add(handler.m_DownloadSerialId, handler);
                if (_mIsUpdating == false)
                {
                    _mIsUpdating = true;
                    AsUpdate();
                }
                return handler.m_DownloadSerialId;
            }
            catch (Exception ex)
            {
                OnDownloadFailHandler(handler, ex.ToString());
            }
            return -1;
        }

        // 增加下载任务。
        public int AddDownload(int buffSize, string fullUrl, string hashCode)
        {
            DownloadHandlerScript handler = new DownloadHandlerScript(this, null, buffSize, fullUrl, hashCode);
            try
            {
                this.m_Dict_ReadyDownload_Requests.Add(handler.m_DownloadSerialId, handler);
                if (_mIsUpdating)
                {
                    return handler.m_DownloadSerialId;
                }
                _mIsUpdating = true;
                AsUpdate();
                return handler.m_DownloadSerialId;
            }
            catch (Exception ex)
            {
                OnDownloadFailHandler(handler, ex.ToString());
            }
            return -1;
        }


        // 根据下载任务的序列编号移除下载任务。
        public bool RemoveDownload(int serialId)
        {
            try
            {
                if (this.m_Dict_ReadyDownload_Requests.ContainsKey(serialId))
                {
                    this.m_Dict_ReadyDownload_Requests.Remove(serialId);
                    return true;
                }
                if (this.m_Dict_Downloading_Requests.ContainsKey(serialId))
                {
                    var request = this.m_Dict_Downloading_Requests[serialId];
                    var handler = (DownloadHandlerScript)request.downloadHandler;
                    handler.Close();
                    request.Abort();
                    request.Dispose();
                    this.m_Dict_Downloading_Requests.Remove(serialId);
                    return true;
                }
                _mNeedGCCount++;
                if (_mNeedGCCount > DownloadDef.MAX_CHANNEL_COUNT_LIMIT)
                {
                    _mNeedGCCount = 0;
                    //GC.Collect(-1, GCCollectionMode.Forced);
                }
            }
            catch (Exception ex)
            {
                DownloadLog.LogWarn(ex);
            }
            return false;
        }

        // 移除所有下载任务。
        public int RemoveAllDownloads()
        {
            int length = 0;
            var keys = new int[this.m_Dict_Downloading_Requests.Count];
            this.m_Dict_Downloading_Requests.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                this.RemoveDownload(key);
            }
            length += keys.Length;
            keys = new int[this.m_Dict_ReadyDownload_Requests.Count];
            this.m_Dict_ReadyDownload_Requests.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                this.RemoveDownload(key);
            }
            length += keys.Length;
            return length;
        }

        // 重置所有下载任务。
        public int RestartAllDownloads()
        {
            var keys = new int[this.m_Dict_Downloading_Requests.Count];
            this.m_Dict_Downloading_Requests.Keys.CopyTo(keys, 0);
            foreach (var k in keys)
            {
                DownloadHandlerScript handler = (DownloadHandlerScript)m_Dict_Downloading_Requests[k].downloadHandler;
                this.RemoveDownload(k);
                if (handler.IsDownloadFile)
                {
                    this.AddDownload(handler.m_FullSavePath, handler.m_FullUrl, handler.m_HashCode);
                }
                else
                {
                    this.AddDownload((int)handler.m_DownloadStream.Length, handler.m_FullUrl, handler.m_HashCode);
                }
            }
            var length = m_Dict_Downloading_Requests.Count;
            m_Dict_Downloading_Requests.Clear();
            return length;
        }


        // 获取当前下载速度
        public float GetCurrentDownloadSpeed()
        {
            var interval = Time.realtimeSinceStartup - this._mLastCalculationSpeedTime;
            if (interval > DownloadDef.STATISTICS_DOWNLOAD_SPEED_INTERVAL)
            {
                _mLastCalculationSpeed = m_LastSumBytes / interval;
                this._mLastCalculationSpeedTime = Time.realtimeSinceStartup;
                m_LastSumBytes = 0;
                MIsNewSpeed = true;
            }
            else
            {
                MIsNewSpeed = false;
            }
            if (this.GetCurrentDownloadingTaskCount() > 0 && _mLastCalculationSpeed < DownloadDef.TASK_HYSTERESIS_CHECK_BYTES)
            {
                interval = Time.realtimeSinceStartup - this.m_LastTaskHysteresisCheckTime;
                if (!(interval > DownloadDef.TASK_HYSTERESIS_CHECK_INTERVAL))
                {
                    return _mLastCalculationSpeed;
                }
                if (_mIsUpdating == false)
                {
                    this.RestartAllDownloads();
                }
                else
                {
                    this._mNeedRestartAll = true;
                }
                this.m_LastTaskHysteresisCheckTime = Time.realtimeSinceStartup;
            }
            else
            {
                this.m_LastTaskHysteresisCheckTime = Time.realtimeSinceStartup;
            }
            return _mLastCalculationSpeed;
        }

        /// <summary>
        /// 获取正在下载任务的数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentDownloadingTaskCount()
        {
            return this.m_Dict_Downloading_Requests.Count;
        }

        /// <summary>
        /// 获取等待下载任务的数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentWaitingTaskCount()
        {
            return this.m_Dict_ReadyDownload_Requests.Count;
        }

        /// <summary>
        /// 外部手动通知下载更新
        /// </summary>
        /// <param name="handler"></param>
        public void ManualNoticeDownloadUpdate(DownloadHandlerScript handler)
        {
            if (this.m_Dict_Downloading_Requests.ContainsKey(handler.m_DownloadSerialId))
            {
                this.OnDownloadUpdate?.Invoke(new DownloadArgs(handler, handler.m_Request.downloadedBytes));
            }
        }

        /// <summary>
        /// 外部手动通知下载失败
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="errMeg"></param>
        public void ManualNoticeDownloadFailure(DownloadHandlerScript handler, string errMeg)
        {
            if (!this.m_Dict_Downloading_Requests.ContainsKey(handler.m_DownloadSerialId)) return;
            var p = new DownloadFailureArgs(handler, 0, errMeg);
            this.RemoveDownload(handler.m_DownloadSerialId);
            this.OnDownloadFailure?.Invoke(p);
        }

        private async void AsUpdate()
        {
            await Task.Delay(1);
            DownloadHandlerScript handler = null;
            try
            {
                while (this.m_Dict_Downloading_Requests.Count > 0 || this.m_Dict_ReadyDownload_Requests.Count > 0)
                {
                    if (this._mNeedRestartAll)
                    {
                        DownloadLog.LogWarn("网络迟滞,正在重试3");
                        return;
                    }

                    // 遍历所有正在下载的任务
                    var keys = new int[m_Dict_Downloading_Requests.Count];
                    m_Dict_Downloading_Requests.Keys.CopyTo(keys, 0);
                    foreach (var k in keys)
                    {
                        var value = m_Dict_Downloading_Requests[k];
                        handler = (DownloadHandlerScript)value.downloadHandler;
                        if (!value.isDone)
                        {
                            continue;
                        }
                        if (value.result == UnityWebRequest.Result.Success)
                        {
                            OnSuccessHandler(handler, k);
                        }
                        // 判断是否下载错误
                        else if (value.result == UnityWebRequest.Result.ConnectionError ||
                                 value.result == UnityWebRequest.Result.ProtocolError ||
                                 value.result == UnityWebRequest.Result.DataProcessingError)
                        {
                            OnDownloadFailHandler(handler, value.error);
                        }
                    }

                    // 等待队列中有任务, 将任务开始执行,直到上限
                    while (this.m_Dict_ReadyDownload_Requests.Count > 0 && this.m_Dict_Downloading_Requests.Count < DownloadDef.MAX_CHANNEL_COUNT_LIMIT)
                    {
                        using var it = this.m_Dict_ReadyDownload_Requests.GetEnumerator();
                        if (!it.MoveNext())
                        {
                            continue;
                        }
                        try
                        {
                            handler = it.Current.Value;
                            this.OnDownloadStart?.Invoke(new DownloadArgs(handler));
                            this.m_Dict_ReadyDownload_Requests.Remove(it.Current.Key);
                            handler.OpenStream();
                            it.Current.Value.m_Request = UnityWebRequest.Get(handler.m_FullUrl);
                            it.Current.Value.m_Request.downloadHandler = handler;
                            if (it.Current.Value.m_FullUrl.StartsWith("https"))
                            {
                                it.Current.Value.m_Request.certificateHandler = new IgnoreHttps();
                            }
                            it.Current.Value.m_Request.SendWebRequest();
                            this.m_Dict_Downloading_Requests.Add(it.Current.Key, it.Current.Value.m_Request);
                        }
                        catch (Exception ex)
                        {
                            OnDownloadFailHandler(handler, ex.ToString());
                        }
                    }
                    await Task.Delay(1);
                }
            }
            catch (Exception ex)
            {
                OnDownloadFailHandler(handler, ex.ToString());
            }
            finally
            {
                if (this._mNeedRestartAll)
                {
                    this.RestartAllDownloads();
                    this._mNeedRestartAll = false;
                    AsUpdate();
                }
                else
                {
                    _mIsUpdating = false;
                }
            }
        }

        /// <summary>
        /// 下载失败处理
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="errMsg"></param>
        private void OnDownloadFailHandler(DownloadHandlerScript handler, string errMsg)
        {
            DownloadFailureArgs p = new DownloadFailureArgs(handler, 0, errMsg);
            this.RemoveDownload(handler.m_DownloadSerialId);
            this.OnDownloadFailure?.Invoke(p);
        }


        /// <summary>
        /// 下载失败处理
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="serialId"></param>
        private void OnSuccessHandler(DownloadHandlerScript handler, int serialId)
        {
            if (!handler.IsCheckHashCode)
            {
                this._mSumDownloadBytes += handler.m_Request.downloadedBytes;
                if (handler.IsDownloadFile)
                {
                    handler.Close();
                }
                this.OnDownloadSuccess?.Invoke(new DownloadSuccessArgs(handler, handler.m_Request.downloadedBytes));
                this.RemoveDownload(serialId);
                return;
            }
            var mainThreadSynContext = SynchronizationContext.Current;
            Task t = new Task(taskResultObjData =>
            {
                CheckHashResult hashCheckResult = (CheckHashResult)taskResultObjData;
                hashCheckResult.UpdateCheckResult();
                mainThreadSynContext.Post(mainThreadTaskResultObjData =>
                {
                    var checkHashResult = (CheckHashResult)mainThreadTaskResultObjData;
                    if (checkHashResult.Handler.IsDownloadFile)
                    {
                        checkHashResult.Handler.Close();
                    }
                    if (checkHashResult.CheckHashCode())
                    {
                        this._mSumDownloadBytes += checkHashResult.Handler.m_Request.downloadedBytes;
                        OnDownloadSuccess?.Invoke(new DownloadSuccessArgs(checkHashResult.Handler, checkHashResult.Handler.m_Request.downloadedBytes));
                        this.RemoveDownload(serialId);
                    }
                    else
                    {
                        OnDownloadFailHandler(checkHashResult.Handler, checkHashResult.ErrorMsg);
                    }
                }, hashCheckResult);
            }, new CheckHashResult(handler));
            this.m_Dict_Downloading_Requests.Remove(serialId);
            t.Start();
        }
    }
}