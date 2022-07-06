using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Access;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.ItemSpace
{
    public class ItemShitController : MonoBehaviour
    {
        [SerializeField]
        private HoloMonAccessAPI p_HoloMonAccessAPI;

        /// <summary>
        /// モデルコントローラ
        /// </summary>
        [SerializeField, Tooltip("モデルコントローラ")]
        private ItemCommonController p_ModelController;

        /// <summary>
        /// 現在のスケール変化比率
        /// </summary>
        [SerializeField, Tooltip("現在のスケール変化比率")]
        private float p_ScaleRatio;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // モデルを初期化する
            p_ModelController.Initialize();

            // モデルを非表示にする
            p_ModelController.HideItem();

            // デフォルトの変化比率は 1 とする
            p_ScaleRatio = 1.0f;
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// オブジェクトリセット
        /// </summary>
        public void ResetObject()
        {
            // モデルをリセットする
            p_ModelController.ResetObject();

            // モデルを非表示にする
            p_ModelController.HideItem();
        }

        /// <summary>
        /// オブジェクトをスポーンする
        /// </summary>
        /// <param name="a_Position"></param>
        public void SpawnObject(Vector3 a_Position)
        {
            // 現在のホロモンサイズ変化比率を取得する
            float holomonRatio = p_HoloMonAccessAPI.View.ConditionsBodyAPI.DefaultBodyHeightRatio;

            // 現在のホロモンサイズ変化比率に合わせて大きさを調節する
            ApplySizeCondition(holomonRatio);

            // 現在のホロモンサイズに合わせて位置を指定する
            p_ModelController.SetPosition(a_Position);

            // モデルを表示する
            p_ModelController.ShowItem();
        }

        /// <summary>
        /// 他オブジェクトが衝突したときの処理
        /// </summary>
        /// <param name="a_GameObject"></param>
        public void HitObjectAction(GameObject a_GameObject)
        {
            // オブジェクトの種別理解情報を取得する
            ObjectFeatures registObjectFeatures = a_GameObject.GetComponent<ObjectFeatures>();

            // 種別理解情報が付与されているか否か
            if (registObjectFeatures == null) return;

            switch(registObjectFeatures.DataWrap.UnderstandInformation.ObjectUnderstandType)
            {
                case ObjectUnderstandType.ShowerWater:
                    // シャワー水流が当たった場合はうんちを消失させる
                    p_ModelController.DisappearItem(1.0f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 現在のホロモンの身長に合わせて大きさを反映する
        /// </summary>
        /// <param name="a_ScaleRatio"></param>
        private void ApplySizeCondition(float a_ScaleRatio)
        {
            // 現在のホロモンサイズに合わせた大きさを算出して設定する
            p_ModelController.SetScaleRatio(a_ScaleRatio);

            // 設定中のサイズ比率を記録する
            p_ScaleRatio = a_ScaleRatio;
        }
    }
}