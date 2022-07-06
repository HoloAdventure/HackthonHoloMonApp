using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.WorldItem.Model.Common
{
    public interface WorldItemCommonInterface
    {
        /// <summary>
        /// アイテムをリセットする
        /// </summary>
        void ResetObject();

        /// <summary>
        /// アイテムを表示する
        /// </summary>

        void ShowItem();

        /// <summary>
        /// アイテムを隠す
        /// </summary>

        void HideItem();

        /// <summary>
        /// 物理演算を有効化する
        /// </summary>
        void EnablePhysics();

        /// <summary>
        /// 物理演算を無効化する
        /// </summary>
        void DisablePhysics();

        /// <summary>
        /// 座標を設定する
        /// </summary>
        void SetPosition(Vector3 a_Position);

        /// <summary>
        /// 回転を設定する
        /// </summary>
        void SetRotation(Quaternion a_Rotation);

        /// <summary>
        /// スケールを設定する
        /// </summary>
        void SetScale(Vector3 a_Scale);

        /// <summary>
        /// スケール比率を指定して即座に変形する
        /// </summary>
        void SetScaleRatio(float a_Ratio);

        /// <summary>
        /// 慣性速度を設定する
        /// </summary>
        void AddForce(Vector3 a_Force);

        /// <summary>
        /// スケール比率を指定して変形する
        /// </summary>
        void MorphScaleRatio(float a_Ratio);

        /// <summary>
        /// アイテムを消失させる
        /// </summary>
        void DisappearItem(float a_LearpTime);
    }
}