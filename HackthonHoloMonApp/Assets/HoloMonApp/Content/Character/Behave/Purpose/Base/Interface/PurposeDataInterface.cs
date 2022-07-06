using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    public interface PurposeDataInterface
    {
        /// <summary>
        /// ホロモンの目的種別
        /// </summary>
        HoloMonPurposeType GetPurposeType();

        /// <summary>
        /// ホロモンの目的個別データ
        /// </summary>
        PurposeDataInterface GetPurposeData();
    }
}