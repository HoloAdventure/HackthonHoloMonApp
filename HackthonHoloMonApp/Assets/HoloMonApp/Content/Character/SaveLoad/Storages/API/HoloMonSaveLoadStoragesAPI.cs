using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    public class HoloMonSaveLoadStoragesAPI : MonoBehaviour, HoloMonSaveLoadIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonSaveLoadReference p_Reference;

        public void AwakeInit(HoloMonSaveLoadReference reference)
        {
            p_Reference = reference;
        }

        [SerializeField, Tooltip("ホロモンのデータセーブコントローラの参照")]
        private HoloMonSaveLoadAccessor p_Accessor;

        /// <summary>
        /// データ保存
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            return p_Accessor.SaveData(p_Reference);
        }

        /// <summary>
        /// データ読込
        /// </summary>
        /// <returns></returns>
        public bool LoadData()
        {
            return p_Accessor.LoadData(p_Reference);
        }
    }
}