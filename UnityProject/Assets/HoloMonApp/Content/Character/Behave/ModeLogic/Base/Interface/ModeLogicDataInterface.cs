using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    public interface ModeLogicDataInterface
    {
        /// <summary>
        /// ホロモンのアクションモード種別
        /// </summary>
        /// <returns></returns>
        HoloMonActionMode GetActionMode();

        /// <summary>
        /// 種別固有設定データ
        /// </summary>
        /// <returns></returns>
        ModeLogicDataInterface GetModeLogicData();
    }
}