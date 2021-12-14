﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers.DebugHandler
{
    /// <summary>
    /// 負責Debug.Log相關處理
    /// </summary>
    public class DebugManager
    {
        #region {========== Singleton: Instance ==========}
        private static DebugManager _instance;
        public static DebugManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DebugManager();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 是否啟動Log
        /// </summary>
        public bool IsActivated = true;
        /// <summary>
        /// 每當Log訊息時觸發
        /// </summary>
        public UnityAction<string> onLogEvent;

        string separater = "========== Separater ==========";

        #region {========== Log歷史訊息: LogHistoryList ==========}
        private List<string> logList = new List<string>();
        public List<string> LogHistoryList
        {
            get { return logList; }
        }
        public string LogHistory
        {
            get
            {
                string result = "";
                foreach (string str in logList)
                {
                    result += str + "\n";
                }
                return result;
            }
        }
        #endregion

        /// <summary>
        /// Debug.Log 指定訊息
        /// </summary>
        public void Log(string msg, TextColor? color = null, bool timeStamp = true)
        {
            if (!IsActivated) return;
            msg = SetTextColor(msg, color);
            Debug.Log(msg);
            if (!msg.Contains(separater))
            {
                RecordMsg(msg, timeStamp);
            }
        }

        /// <summary>
        /// Debug.Log 分隔行
        /// </summary>
        public void LogSeparater()
        {
            if (!IsActivated) return;
            separater = SetTextSize(separater, 16);
            separater = SetTextBold(separater);
            separater = SetTextItalics(separater);
            Log(separater, TextColor.orange, false);
        }

        #region {========== 設定RicthText樣式標籤，可供外部直接調用來設定文字樣式  ==========}
        /// <summary>
        /// RichText設定文字顏色
        /// </summary>
        public static string SetTextColor(string msg, TextColor? color)
        {
            string colorStr = (color ?? TextColor.cyan).ToString();
            return $"<color={colorStr}>{msg}</color>";
        }
        /// <summary>
        /// RichText設定文字大小
        /// </summary>
        public static string SetTextSize(string msg, int size = 14)
        {
            return $"<size={size}>{msg}</size>";
        }
        /// <summary>
        /// RichText設定文字粗體
        /// </summary>
        public static string SetTextBold(string msg)
        {
            return $"<b>{msg}</b>";
        }
        /// <summary>
        /// RichText設定文字斜體
        /// </summary>
        public static string SetTextItalics(string msg)
        {
            return $"<i>{msg}</i>";
        }
        #endregion

        /// <summary>
        /// 記錄Log訊息
        /// </summary>
        private void RecordMsg(string msg, bool isTimeStamp)
        {
            if (isTimeStamp) msg = TimeStamp + msg;
            logList.Add(msg);
            onLogEvent?.Invoke(msg);
        }

        /// <summary>
        /// 目前時間點
        /// </summary>
        private string TimeStamp
        {
            get
            {
                return "[" + DateTime.Now.ToString("HH:mm:ss") + "] => ";
            }
        }

        /// <summary>
        /// 文字顏色列表
        /// </summary>
        public enum TextColor
        {
            cyan, blac, blue, green, lime, olive, orange, purple, red, teal, white, yellow
        }
    }
}

