using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Access;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    [RequireComponent(typeof(LineRenderer))]
    public class NavMeshPathLineRenderer : MonoBehaviour
    {
        [SerializeField]
        private HoloMonAccessAPI p_AccessAPI;

        /// <summary>
        /// ターゲット追従ロジック
        /// </summary>
        [SerializeField, Tooltip("ターゲット追従ロジック")]
        private HoloMonModeLogicTrackingTargetNavMesh p_LogicTrackingTarget;

        /// <summary>
        /// 描画ライン
        /// </summary>
        LineRenderer p_PathLineRenderer;

        /// <summary>
        /// 描画実行フラグ
        /// </summary>
        bool isDraw;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 各コンポーネントの参照を取得する
            p_PathLineRenderer = GetComponent<LineRenderer>();

            // デフォルトは無効状態とする
            DisablePathLineDraw();
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // 描画実行フラグが有効か否かチェックする
            if (isDraw)
            {
                // 経路のパスを取得する
                Vector3[] positions = p_AccessAPI.View.BodyComponentsToNavMeshAgentAPI.Path.corners;

                // パスを取得できたか確認する
                if (positions.Length > 1)
                {
                    // 経路のパスをラインレンダラーで描画する
                    p_PathLineRenderer.positionCount = positions.Length;
                    for (int i = 0; i < positions.Length; i++)
                    {
                        // ラインをパスの位置から50cm上げた位置に配置する
                        Vector3 linePos = new Vector3(positions[i].x, positions[i].y + 0.5f, positions[i].z);
                        p_PathLineRenderer.SetPosition(i, linePos);
                    }
                }
            }
        }

        /// <summary>
        /// パスラインの描画を切り替える
        /// </summary>
        /// <param name="onoff"></param>
        public void ChangePathLineDraw(bool onoff)
        {
            if(onoff)
            {
                // 有効化中にパスを描画する
                EnablePathLineDraw();
            }
            else
            {
                // 無効化中はパスを描画しない
                DisablePathLineDraw();
            }
        }

        /// <summary>
        /// 描画実行状態への切り替え
        /// </summary>
        private void EnablePathLineDraw()
        {
            p_PathLineRenderer.enabled = true;
            isDraw = true;
        }

        /// <summary>
        /// 描画停止状態への切り替え
        /// </summary>
        private void DisablePathLineDraw()
        {
            p_PathLineRenderer.enabled = false;
            isDraw = false;
        }
    }
}