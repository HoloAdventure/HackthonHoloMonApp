using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    /// <summary>
    /// ホロモンの睡眠設定
    /// </summary>
    [Serializable]
    public class HoloMonSleepSetting
    {
        [SerializeField, Tooltip("睡眠開始時間設定テキスト(初期化時のみ有効)")]
        private string p_StartTimeText;

        /// <summary>
        /// 睡眠開始時間帯(private)
        /// </summary>
        private TimeSpan? p_StartTime;

        /// <summary>
        /// 睡眠開始時間帯
        /// </summary>
        public TimeSpan StartTime => GetStartTime();

        [SerializeField, Tooltip("睡眠終了時間設定テキスト(初期化時のみ有効)")]
        private string p_EndTimeText;

        /// <summary>
        /// 睡眠終了時間帯(private)
        /// </summary>
        private TimeSpan? p_EndTime;

        /// <summary>
        /// 睡眠終了時間帯
        /// </summary>
        public TimeSpan EndTime => GetEndTime();

        public HoloMonSleepSetting()
        {
            // 引数無しの場合時間帯は null のまま
            // Inspectorビューのテキストから設定読込を行う
        }

        public HoloMonSleepSetting(TimeSpan a_StartTime, TimeSpan a_EndTime)
        {
            p_StartTime = a_StartTime;
            p_EndTime = a_EndTime;
        }

        private TimeSpan GetStartTime()
        {
            if (p_StartTime == null)
            {
                TimeSpan parceTime = new TimeSpan();
                // 初期化時のみテキストから時刻指定の取得を試みる
                if (TimeSpan.TryParse(p_StartTimeText, out parceTime))
                {
                    p_StartTime = parceTime;
                    Debug.Log("SleepStartTimeSpan : " + p_StartTime.ToString());
                }
                else
                {
                    p_EndTime = new TimeSpan();
                    Debug.LogError("Parse Error : SleepStartTimeText");
                }
            }
            return p_StartTime ?? new TimeSpan();
        }

        private TimeSpan GetEndTime()
        {
            if (p_EndTime == null)
            {
                TimeSpan parceTime = new TimeSpan();
                // 初期化時のみテキストから時刻指定の取得を試みる
                if (TimeSpan.TryParse(p_EndTimeText, out parceTime))
                {
                    p_EndTime = parceTime;
                    Debug.Log("SleepEndTimeSpan : " + p_EndTime.ToString());
                }
                else
                {
                    p_EndTime = new TimeSpan();
                    Debug.LogError("Parse Error : SleepEndTimeText");
                }
            }
            return p_EndTime ?? new TimeSpan();
        }
    }
}
