#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CoreSystemへのアクセスのため
using Microsoft.MixedReality.Toolkit;
// 空間認識情報の取得のため
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    public class EnvironmentMapChangeMRTKv2
    {
        public EnvironmentMapChangeMRTKv2()
        {
        }

        public void OnToggleMapMesh()
        {
            // 現在の表示状態に合わせて切り替え
            switch (CheckDisplayOption())
            {
                case SpatialAwarenessMeshDisplayOptions.Visible:
                    // 表示状態ならオクルージョンに変更する
                    SwitchOcclusion();
                    break;
                case SpatialAwarenessMeshDisplayOptions.Occlusion:
                    // オクルージョン状態なら表示に変更する
                    SwitchVisible();
                    break;
            }
        }

        #region "private"
        /// <summary>
        /// SpatialMapの表示状態を取得する
        /// </summary>
        /// <returns></returns>
        private SpatialAwarenessMeshDisplayOptions CheckDisplayOption()
        {
            var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
            if (observer == null) return SpatialAwarenessMeshDisplayOptions.None;
            return observer.DisplayOption;
        }

        /// <summary>
        /// SpatialMapを表示(Visible)に変更する
        /// </summary>
        private void SwitchVisible()
        {
            // 利用可能な最初のメッシュオブザーバーを取得する
            var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
#if UNITY_EDITOR
            // エディター起動時は「Spatial Object Mesh Observer」のメッシュオブザーバを取得します
            var meshObserverName = "Spatial Object Mesh Observer";
            observer = ((IMixedRealityDataProviderAccess)CoreServices.SpatialAwarenessSystem).GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>(meshObserverName);
#endif

            // 表示(Visible)に設定する
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Visible;
        }

        /// <summary>
        /// SpatialMapをオクルージョン(Occlusion)に変更する
        /// </summary>
        private void SwitchOcclusion()
        {
            // 利用可能な最初のメッシュオブザーバーを取得する
            var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
#if UNITY_EDITOR
            // エディター起動時は「Spatial Object Mesh Observer」のメッシュオブザーバを取得します
            var meshObserverName = "Spatial Object Mesh Observer";
            observer = ((IMixedRealityDataProviderAccess)CoreServices.SpatialAwarenessSystem).GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>(meshObserverName);
#endif

            // オクルージョン(Occlusion)に設定する
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Occlusion;
        }

        /// <summary>
        /// SpatialMapを非表示(None)に変更する
        /// </summary>
        private void SwitchNone()
        {
            // 利用可能な最初のメッシュオブザーバーを取得する
            var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
#if UNITY_EDITOR
            // エディター起動時は「Spatial Object Mesh Observer」のメッシュオブザーバを取得します
            var meshObserverName = "Spatial Object Mesh Observer";
            observer = ((IMixedRealityDataProviderAccess)CoreServices.SpatialAwarenessSystem).GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>(meshObserverName);
#endif

            // 非表示(None)に設定する
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.None;
        }
        #endregion
    }
}
#endif