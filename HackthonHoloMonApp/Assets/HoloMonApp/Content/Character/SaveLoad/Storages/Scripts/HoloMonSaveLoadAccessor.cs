using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Control.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.Control.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    public class HoloMonSaveLoadAccessor : MonoBehaviour
    {
        [SerializeField, Tooltip("データ保存コンポーネントの参照")]
        private HoloMonSaveLoadManager p_HoloMonSaveLoadManager;

        /// <summary>
        /// 身体状態の管理データ参照
        /// </summary>
        [SerializeField, Tooltip("身体状態の管理データ参照")]
        private HoloMonBodyStatus p_ConditionBody;

        /// <summary>
        /// 身体状態の管理データ参照
        /// </summary>
        [SerializeField, Tooltip("生活状態の管理データ参照")]
        private HoloMonLifeStatus p_ConditionLife;


        /// <summary>
        /// データ保存
        /// </summary>
        /// <returns></returns>
        public bool SaveData(HoloMonSaveLoadReference a_Reference)
        {
            // 参照先のデータから保存データを生成する
            HoloMonSaveData saveData = new HoloMonSaveData();

            // 現在の保存時刻
            CurrentSaveDateTime(out SaveDateTime saveDateTime);
            saveData.SavedDataTime = saveDateTime;

            // 現在のアプリバージョン
            CurrentSavePackageVersion(out SavePackageVersion savePackageVersion);
            saveData.SavedPackageVersion = savePackageVersion;

            // 現在のアプリ名
            CurrentAppName(out string appName);
            saveData.AppName = appName;

            // 現在のボディコンディション
            CurrentBodyStatus(a_Reference.View.ConditionsBodyAPI, out HoloMonBodyStatus holoMonBodyStatus);
            saveData.BodyStatus = holoMonBodyStatus;

            // 現在のライフコンディション
            CurrentLifeStatus(a_Reference.View.ConditionsLifeAPI, out HoloMonLifeStatus holoMonLifeStatus);
            saveData.LifeStatus = holoMonLifeStatus;


            // データをファイルに保存する
            return p_HoloMonSaveLoadManager.SaveData(saveData);
        }

        /// <summary>
        /// データ読込
        /// </summary>
        /// <returns></returns>
        public bool LoadData(HoloMonSaveLoadReference a_Reference)
        {
            // ファイルからデータを読み込む
            HoloMonSaveData loadData = new HoloMonSaveData();
            bool result = p_HoloMonSaveLoadManager.LoadData(out loadData);
            if (loadData == null) return false;

            // 保存時刻のチェック
            ApplySaveDateTime(loadData.SavedDataTime);

            // 保存バージョン番号のチェック
            ApplySavePackageVersion(loadData.SavedPackageVersion);

            // 保存アプリ名のチェック
            ApplyAppName(loadData.AppName);

            // ボディコンディションの反映
            ApplyBodyStatus(a_Reference.Control.ConditionsBodyAPI, loadData.BodyStatus);

            // ライフコンディションの反映
            ApplyLifeStatus(a_Reference.Control.ConditionsLifeAPI, loadData.LifeStatus);

            return true;
        }

        #region "書き出し用private処理"
        private void CurrentSaveDateTime(out SaveDateTime o_SaveDateTime)
        {
            o_SaveDateTime = new SaveDateTime(DateTime.Now);
            return;
        }
        private void CurrentSavePackageVersion(out SavePackageVersion o_SavePackageVersion)
        {
            ushort majorVer = 0; // メジャーバージョン
            ushort minorVer = 0; // マイナーバージョン
            ushort buildVer = 0; // ビルドバージョン
            ushort revisionVer = 0; // リビジョンバージョン
#if WINDOWS_UWP
            Windows.ApplicationModel.PackageVersion version =
                Windows.ApplicationModel.Package.Current.Id.Version;
            majorVer = version.Major; // メジャーバージョン
            minorVer = version.Minor; // マイナーバージョン
            buildVer = version.Build; // ビルドバージョン
            revisionVer = version.Revision; // リビジョンバージョン
#endif
            o_SavePackageVersion = new SavePackageVersion{ Major = majorVer, Minor = minorVer, Build = buildVer, Revision = revisionVer };
            return;
        }
        private void CurrentAppName(out string o_AppName)
        {
            string appName = "TestApp";
#if WINDOWS_UWP
            appName = Windows.ApplicationModel.Package.Current.Id.Name;
#endif
            o_AppName = appName;
            return;
        }
        private void CurrentBodyStatus(HoloMonViewConditionsBodyAPI a_ViewBody, out HoloMonBodyStatus o_HoloMonBodyStatus)
        {
            o_HoloMonBodyStatus = a_ViewBody.BodyStatus;
            return;
        }
        private void CurrentLifeStatus(HoloMonViewConditionsLifeAPI a_ViewLife, out HoloMonLifeStatus o_HoloMonLifeStatus)
        {
            o_HoloMonLifeStatus = a_ViewLife.LifeStatus;
            return;
        }
        #endregion

        #region "読込用private処理"
        private void ApplySaveDateTime(SaveDateTime a_SaveDateTime)
        {
            // 保存時刻のチェック作業は特に実施しない
            return;
        }
        private void ApplySavePackageVersion(SavePackageVersion a_SavePackageVersion)
        {
            // 保存パッケージ番号のチェック作業は特に実施しない
            return;
        }
        private void ApplyAppName(string a_AppName)
        {
            // 保存パッケージ名のチェック作業は特に実施しない
            return;
        }
        private void ApplyBodyStatus(HoloMonControlConditionsBodyAPI a_ControlBody, HoloMonBodyStatus a_HoloMonBodyStatus)
        {
            a_ControlBody.ApplyBodyStatus(a_HoloMonBodyStatus);
            return;
        }
        private void ApplyLifeStatus(HoloMonControlConditionsLifeAPI a_ControlLife, HoloMonLifeStatus a_HoloMonLifeStatus)
        {
            a_ControlLife.ApplyLifeStatus(a_HoloMonLifeStatus);
            return;
        }
        #endregion
    }
}