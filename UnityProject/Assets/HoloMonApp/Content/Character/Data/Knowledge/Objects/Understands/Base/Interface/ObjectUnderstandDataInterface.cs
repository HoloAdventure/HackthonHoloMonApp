using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    public interface ObjectUnderstandDataInterface
    {
        /// <summary>
        /// データの状態比較用ハッシュ値
        /// </summary>
        int StatusHash();
    }
}