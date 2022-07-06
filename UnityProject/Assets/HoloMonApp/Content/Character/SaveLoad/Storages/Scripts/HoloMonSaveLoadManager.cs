using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    public class HoloMonSaveLoadManager : MonoBehaviour
    {
        [SerializeField, Tooltip("現在のセーブデータ(参照用)")]
        private HoloMonSaveData p_HoloMonSaveData;

        [SerializeField, Tooltip("暗号化ON/OFF")]
        private bool p_IsCyptography = true;

        /// <summary>
        /// XMLシリアライザ
        /// </summary>
        private HoloMonDataXmlSerializer p_HoloMonDataXmlSerializer
            = new HoloMonDataXmlSerializer();

        /// <summary>
        /// 暗号化XMLシリアライザ
        /// </summary>
        private HoloMonDataXmlSerializerCryptography p_HoloMonDataXmlSerializerCryptography
            = new HoloMonDataXmlSerializerCryptography();


        private void Start()
        {
            p_HoloMonSaveData = new HoloMonSaveData();
            p_HoloMonSaveData.SavedDataTime = new SaveDateTime(System.DateTime.Now);
        }

        /// <summary>
        /// データ保存(引数指定)
        /// </summary>
        public bool SaveData(HoloMonSaveData a_Data)
        {
            try
            {
                // 最新のデータを保持する
                p_HoloMonSaveData = a_Data;

                // データをファイルに保存する
                ExecuteWriteFile(a_Data, p_IsCyptography);
            }
            catch(Exception ex)
            {
                Debug.Log("SaveData() Exception Failed : " + ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// データ読込(引数指定)
        /// </summary>
        public bool LoadData(out HoloMonSaveData o_Data)
        {
            try
            {
                // ファイルからデータを読み込む
                o_Data = ExecuteReadFile(p_IsCyptography);
                if (o_Data == null) return false;

                // 最新のデータを保持する
                p_HoloMonSaveData = o_Data;
            }
            catch(Exception ex)
            {
                Debug.Log("LoadData() Exception Failed : " + ex.ToString());
                o_Data = null;
                return false;
            }

            return true;
        }

        #region "ファイル処理"
        /// <summary>
        /// 書き込み処理を実行する
        /// </summary>
        private void ExecuteWriteFile(HoloMonSaveData a_SaveData, bool a_IsCyptography)
        {
            // 暗号化するか否か
            if (a_IsCyptography)
            {
                p_HoloMonDataXmlSerializerCryptography.WriteData(a_SaveData);
            }
            else
            {
                p_HoloMonDataXmlSerializer.WriteData(a_SaveData);
            }
        }

        /// <summary>
        /// 読み込み処理を実行する
        /// </summary>
        public HoloMonSaveData ExecuteReadFile(bool a_IsCyptography)
        {
            // 読込データ用変数
            HoloMonSaveData loadData = null;

            // 複合化するか否か
            if (a_IsCyptography)
            {
                loadData = p_HoloMonDataXmlSerializerCryptography.ReadData();
            }
            else
            {
                loadData = p_HoloMonDataXmlSerializer.ReadData();
            }

            return loadData;
        }
        #endregion
    }
}