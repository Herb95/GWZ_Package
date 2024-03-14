#region 注释
/*
 *     @File:          CheckHashResult
 *     @NameSpace:     Cxx.DownloadUtils
 *     @Description:   DES
 *     @Author:        GrayWolfZ
 *     @Version:       0.1版本
 *     @Time:          2024年2月22日-13:22:11
 *     @Copyright  Copyright (c) 2023
 */
#endregion


namespace Gwp.FrameCode.DownloadUtils
{
    public class CheckHashResult
    {
        private string _mCheckingResult = "";
        public string CheckingResult => this._mCheckingResult;

        private readonly DownloadHandlerScript _mHandler;
        public DownloadHandlerScript Handler => this._mHandler;

        public string ErrorMsg => "HashError: Url: " + this._mHandler.m_FullUrl + " Hash: " + this.CheckingResult + "," + this._mHandler.m_HashCode;

        public CheckHashResult()
        {
        }

        public CheckHashResult(DownloadHandlerScript handler, string checkResult = "")
        {
            this._mCheckingResult = checkResult;
            this._mHandler = handler;
        }

        public void UpdateCheckResult()
        {
            this._mCheckingResult = DownloadDef.GetMD5HashFromStream(this._mHandler.m_DownloadStream);
        }

        public bool CheckHashCode()
        {
            return this._mCheckingResult.Equals(this._mHandler.m_HashCode);
        }
    }
}