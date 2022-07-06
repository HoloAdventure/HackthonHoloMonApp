using System.Collections;
using System.Collections.Generic;
using System;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    public class CalculationActivityTime
    {
        /// <summary>
        /// 睡眠開始時刻
        /// </summary>
        private TimeSpan p_StartSleepTime;

        /// <summary>
        /// 睡眠終了時刻
        /// </summary>
        private TimeSpan p_EndSleepTime;

        public CalculationActivityTime(TimeSpan a_StartSleepTime, TimeSpan a_EndSleepTime)
        {
            p_StartSleepTime = a_StartSleepTime;
            p_EndSleepTime = a_EndSleepTime;
        }

        #region "Public Method"
        /// <summary>
        /// 指定時刻から指定時刻までの睡眠時間を考慮しない全ての経過時間(TimeSpan)を返す
        /// </summary>
        /// <param name="a_StartTime"></param>
        /// <param name="a_EndTime"></param>
        /// <returns></returns>
        public TimeSpan AllElapsedTimeSpan(DateTime a_StartTime, DateTime a_EndTime)
        {
            return CalculateAllElapsedTimeSpan(a_StartTime, a_EndTime);
        }

        /// <summary>
        /// 指定時刻から指定時刻までの睡眠時間を考慮した経過時間(TimeSpan)を返す
        /// </summary>
        /// <param name="a_StartTime"></param>
        /// <param name="a_EndTime"></param>
        /// <returns></returns>
        public TimeSpan AwakeElapsedTimeSpan(DateTime a_StartTime, DateTime a_EndTime)
        {
            return CalculateAwakeElapsedTimeSpan(a_StartTime, a_EndTime);
        }

        /// <summary>
        /// 指定の時刻が睡眠時刻に含まれるか否か
        /// </summary>
        /// <param name="a_DataTime"></param>
        public bool IsSleepTime(DateTime a_DataTime)
        {
            return CheckSleepTime(a_DataTime);
        }
        #endregion

        #region "経過時間の計算"
        /// <summary>
        /// 指定時刻から指定時刻までの睡眠時間を考慮しない全ての経過時間(TimeSpan)を返す
        /// </summary>
        /// <param name="a_StartTime"></param>
        /// <param name="a_EndTime"></param>
        /// <returns></returns>
        private TimeSpan CalculateAllElapsedTimeSpan(DateTime a_StartTime, DateTime a_EndTime)
        {
            // 経過時間を計算する
            TimeSpan subtractionTime = a_EndTime - a_StartTime;

            // 計算の結果、0 未満になっていた場合は 0 分で返却する
            if (subtractionTime < TimeSpan.Zero) return TimeSpan.Zero;

            return subtractionTime;
        }

        /// <summary>
        /// 指定時刻から指定時刻までの睡眠時間を考慮した経過時間(TimeSpan)を返す
        /// </summary>
        /// <param name="a_StartTime"></param>
        /// <param name="a_EndTime"></param>
        /// <returns></returns>
        private TimeSpan CalculateAwakeElapsedTimeSpan(DateTime a_StartTime, DateTime a_EndTime)
        {
            // 経過時間の計算結果を格納する変数
            TimeSpan subtractionTime = TimeSpan.Zero;

            // 睡眠時間を考慮した指定開始時刻を取得する
            // 開始時刻が睡眠時刻に含まれる場合は開始時刻から睡眠終了時刻を計算して開始時刻とする
            DateTime startTime = a_StartTime;
            if (CheckSleepTime(startTime)) startTime = CalcStartSleepTime(startTime) ?? startTime;

            // 睡眠時間を考慮した指定終了時刻を取得する
            // 終了時刻が睡眠時刻に含まれる場合は終了時刻から睡眠開始時刻を計算して終了時刻とする
            DateTime endTime = a_EndTime;
            if (CheckSleepTime(endTime)) endTime = CalcEndSleepTime(endTime) ?? endTime;

            // 最初に次の睡眠時間までの経過時刻を計算する
            // 事前に睡眠時刻チェックをしているので実際には null は返らない
            DateTime nextSleepTime = CalcNextSleepTime(startTime) ?? startTime;

            // 終了時刻が次の睡眠時間を跨ぐか否か
            if (nextSleepTime > endTime)
            {
                // 跨がない場合は終了時刻から開始時刻を引いた時間のみの計算とする
                subtractionTime += endTime - startTime;
            }
            else
            {
                // 跨ぐ場合は次の睡眠時間までの時間とそれ以降の日数と活動時間を加算して計算する

                // 次の睡眠時間までの経過時間を計算する
                subtractionTime += nextSleepTime - startTime;

                // それ以降の日数と活動時間を計算する

                // 次の睡眠時間からの経過時間を取得する
                TimeSpan subtractionNextDayTime = endTime - nextSleepTime;

                // 経過日数に合わせて睡眠時間分を減算する
                // ループ回数は経過日数＋１（初日分）
                int subtractionNextDays = (int)Math.Ceiling(subtractionNextDayTime.TotalDays);
                for (int loop = 0; loop < subtractionNextDays; loop++)
                {
                    subtractionNextDayTime -= OneDaySleepTimeSpan();
                }

                // 計算の結果、0 未満になっていた場合は 0 分で計算する
                if (subtractionNextDayTime < TimeSpan.Zero) return TimeSpan.Zero;

                // 計算結果を加算する
                subtractionTime += subtractionNextDayTime;
            }

            // 計算の結果、0 未満になっていた場合は 0 分で返却する
            if (subtractionTime < TimeSpan.Zero) return TimeSpan.Zero;

            return subtractionTime;
        }
        #endregion

        #region "睡眠時間の計算"
        /// <summary>
        /// 1日当たりの睡眠時間(TimeSpan)
        /// </summary>
        private TimeSpan OneDaySleepTimeSpan()
        {
            TimeSpan sleepTimeSpan = TimeSpan.Zero;

            // 睡眠時間が0時を跨ぐか否か（睡眠終了時刻が開始時刻の後か）
            if (p_EndSleepTime > p_StartSleepTime)
            {
                // 0時を跨がない場合
                // 全体の睡眠時間を引き算で算出する
                sleepTimeSpan += p_EndSleepTime - p_StartSleepTime;
            }
            else
            {
                // 0時を跨ぐ場合
                // 0時前の睡眠時間と0時後の睡眠時間をそれぞれ算出する
                sleepTimeSpan += new TimeSpan(1, 0, 0, 0) - p_StartSleepTime;

                sleepTimeSpan += p_EndSleepTime - new TimeSpan(0, 0, 0);
            }

            return sleepTimeSpan;
        }

        /// <summary>
        /// 指定時刻を基に睡眠中であれば睡眠開始からの睡眠時間を返す
        /// 睡眠中でなければ 0 経過を返す
        /// </summary>
        /// <param name="a_CheckTime"></param>
        /// <returns></returns>
        private TimeSpan CalcFromSleepTime(DateTime a_CheckTime)
        {
            // 指定時刻が睡眠時刻に含まれるか否か
            if (!CheckSleepTime(a_CheckTime))
            {
                // 含まれないなら 0 経過の時刻を返す
                return TimeSpan.Zero;
            }

            // 睡眠開始時刻を取得する
            // 事前に睡眠時刻チェックをしているので実際には null は返らない
            DateTime startSleepTime = CalcStartSleepTime(a_CheckTime) ?? a_CheckTime;

            // 睡眠開始からの時間を計算する
            return a_CheckTime - startSleepTime;
        }

        /// <summary>
        /// 指定時刻を基に睡眠中であれば睡眠終了までの睡眠時間を返す
        /// 睡眠中でなければ 0 経過を返す
        /// </summary>
        /// <param name="a_CheckTime"></param>
        /// <returns></returns>
        private TimeSpan CalcUntilSleepTime(DateTime a_CheckTime)
        {
            // 指定時刻が睡眠時刻に含まれるか否か
            if (!CheckSleepTime(a_CheckTime))
            {
                // 含まれないなら 0 経過の時刻を返す
                return TimeSpan.Zero;
            }

            // 睡眠終了時刻を取得する
            // 事前に睡眠時刻チェックをしているので実際には null は返らない
            DateTime endSleepTime = CalcEndSleepTime(a_CheckTime) ?? a_CheckTime;

            // 睡眠終了までの時間を計算する
            return endSleepTime - a_CheckTime;
        }
        #endregion

        #region "睡眠開始/終了時刻計算"
        /// <summary>
        /// 指定時刻を基に睡眠開始時刻を返す
        /// 指定時刻が睡眠中でなければnullを返す
        /// </summary>
        /// <param name="a_CheckTime"></param>
        /// <returns></returns>
        private DateTime? CalcStartSleepTime(DateTime a_CheckTime)
        {
            // 開始時刻が睡眠時刻に含まれるか否か
            if (!CheckSleepTime(a_CheckTime))
            {
                // 睡眠中でなければnullを返す
                return null;
            }

            // 同じ日付の睡眠開始時刻を取得する
            DateTime startSleepTime = new DateTime(
                a_CheckTime.Year,
                a_CheckTime.Month,
                a_CheckTime.Day,
                p_StartSleepTime.Hours,
                p_StartSleepTime.Minutes,
                p_StartSleepTime.Seconds
                );

            // 睡眠開始時刻が指定時刻を超えているか否か
            while (startSleepTime > a_CheckTime)
            {
                // 睡眠開始時刻が指定時刻を超えている場合
                // 前日の睡眠開始時刻を再計算する
                startSleepTime = startSleepTime - new TimeSpan(1, 0, 0, 0);
            }

            return startSleepTime;
        }

        /// <summary>
        /// 指定時刻を基に睡眠終了時刻を返す
        /// 指定時刻が睡眠中でなければnullを返す
        /// </summary>
        /// <param name="a_CheckTime"></param>
        /// <returns></returns>
        private DateTime? CalcEndSleepTime(DateTime a_CheckTime)
        {
            // 終了時刻が睡眠時刻に含まれるか否か
            if (!CheckSleepTime(a_CheckTime))
            {
                // 睡眠中でなければnullを返す
                return null;
            }

            // 同じ日付の睡眠終了時刻を取得する
            DateTime endSleepTime = new DateTime(
                a_CheckTime.Year,
                a_CheckTime.Month,
                a_CheckTime.Day,
                p_EndSleepTime.Hours,
                p_EndSleepTime.Minutes,
                p_EndSleepTime.Seconds
                );

            // 睡眠終了時刻が指定時刻を超えていないか
            while (a_CheckTime > endSleepTime)
            {
                // 睡眠終了時刻が指定時刻を超えていない場合
                // 次の日の睡眠終了時刻を再計算する
                endSleepTime = endSleepTime + new TimeSpan(1, 0, 0, 0);
            }

            return endSleepTime;
        }

        /// <summary>
        /// 指定時刻を基に次の睡眠開始時刻を返す
        /// 指定時刻が睡眠中ならばnullを返す
        /// </summary>
        /// <param name="a_CheckTime"></param>
        /// <returns></returns>
        private DateTime? CalcNextSleepTime(DateTime a_CheckTime)
        {
            // 開始時刻が睡眠時刻に含まれるか否か
            if (CheckSleepTime(a_CheckTime))
            {
                // 睡眠中ならnullを返す
                return null;
            }

            // 同じ日付の睡眠開始時刻を取得する
            DateTime nextSleepTime = new DateTime(
                a_CheckTime.Year,
                a_CheckTime.Month,
                a_CheckTime.Day,
                p_StartSleepTime.Hours,
                p_StartSleepTime.Minutes,
                p_StartSleepTime.Seconds
                );

            // 次の睡眠開始時刻が指定時刻を超えていないか
            while (a_CheckTime > nextSleepTime)
            {
                // 次の睡眠開始時刻が指定時刻を超えていない場合
                // 次の日の睡眠開始時刻を再計算する
                nextSleepTime = nextSleepTime + new TimeSpan(1, 0, 0, 0);
            }

            return nextSleepTime;
        }
        #endregion

        #region "共通関数"
        /// <summary>
        /// 指定の時刻が睡眠時刻に含まれるか否か
        /// </summary>
        /// <param name="a_DataTime"></param>
        private bool CheckSleepTime(DateTime a_DataTime)
        {
            // 日時から時刻情報だけを取得する
            TimeSpan timeOfDay = a_DataTime.TimeOfDay;

            // 睡眠時間が0時を跨ぐか否か（睡眠終了時刻が開始時刻の後か）
            if (p_EndSleepTime > p_StartSleepTime)
            {
                // 0時を跨がない場合
                // 睡眠時間の範囲内か（開始時刻と終了時刻そのものは睡眠時間に含まない）
                if ((p_StartSleepTime < timeOfDay) && (timeOfDay < p_EndSleepTime))
                {
                    return true;
                }
            }
            else
            {
                // 0時を跨ぐ場合
                // 睡眠時間の範囲内か（開始時刻と終了時刻そのものは睡眠時間に含まない）
                if (((new TimeSpan(0, 0, 0) <= timeOfDay) && (timeOfDay < p_EndSleepTime)) ||
                    ((p_StartSleepTime < timeOfDay) && (timeOfDay <= new TimeSpan(1, 0, 0, 0))))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}