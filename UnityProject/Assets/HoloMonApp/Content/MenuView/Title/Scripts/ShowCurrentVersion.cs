using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class ShowCurrentVersion : MonoBehaviour
    {
        [SerializeField, Tooltip("表示テキスト")]
        private TextMeshPro p_ShowText;

        private void Start()
        {
            // アプリのバージョン番号を表示する
            SetMessageCurrentVersion();
        }

        /// <summary>
        /// バージョンをテキストに表示する
        /// </summary>
        public void SetMessageCurrentVersion()
        {
            string showMessage = "";
#if WINDOWS_UWP
        Windows.ApplicationModel.PackageVersion version =
            Windows.ApplicationModel.Package.Current.Id.Version;
        ushort majorVer = version.Major; // メジャーバージョン
        ushort minorVer = version.Minor; // マイナーバージョン
        ushort buildVer = version.Build; // ビルドバージョン
        ushort revisionVer = version.Revision; // リビジョンバージョン

        showMessage = string.Format("{0}.{1}.{2}.{3}", majorVer, minorVer, buildVer, revisionVer);
#else
            showMessage = "Nothing";
#endif
            p_ShowText.text = "HolographicMonster   Version " + showMessage;
        }
    }
}