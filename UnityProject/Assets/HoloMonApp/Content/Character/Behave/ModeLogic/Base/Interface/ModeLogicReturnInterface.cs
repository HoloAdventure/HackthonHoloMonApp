using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    public interface ModeLogicReturnInterface
    {
        /// <summary>
        /// ホロモンのアクションモード種別
        /// </summary>
        /// <returns></returns>
        HoloMonActionMode GetActionMode();

        /// <summary>
        /// 種別固有返却データ
        /// </summary>
        /// <returns></returns>
        ModeLogicReturnInterface GetModeLogicReturn();
    }
}