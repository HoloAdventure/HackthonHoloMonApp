using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.SaveLoad.Storages;

namespace HoloMonApp.Content.Character.SaveLoad
{
    [RequireComponent(typeof(HoloMonSaveLoadReference))]
    public class HoloMonSaveLoadAPI : MonoBehaviour
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonSaveLoadReference p_Reference;

        [SerializeField]
        private HoloMonSaveLoadStoragesAPI p_Storages;

        // 参照用短縮変数

        public HoloMonSaveLoadStoragesAPI StoragesAPI => p_Storages;

        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonSaveLoadIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonSaveLoadIF>()
            {
                p_Storages,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonSaveLoadIF> p_ListIFInstance;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            p_Reference = this.GetComponent<HoloMonSaveLoadReference>();
            foreach (HoloMonSaveLoadIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }
    }
}