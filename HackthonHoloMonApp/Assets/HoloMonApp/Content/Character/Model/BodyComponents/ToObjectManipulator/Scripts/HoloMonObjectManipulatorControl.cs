using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToObjectManipulator
{
    public class HoloMonObjectManipulatorControl : MonoBehaviour
    {
        /// <summary>
        /// 遠距離(Far)操作フラグ変更に合わせた切り替え
        /// </summary>
        /// <param name="a_RenderingFlg"></param>
        public void ChangeFarManipulationFlg(bool a_FarManipulationFlg)
        {
            Debug.Log(this.GetType() + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + ","
                + "a_FarManipulationFlg:" + a_FarManipulationFlg.ToString());

            if (a_FarManipulationFlg)
            {
                // 遠距離操作を有効化
                EnablePathLineDraw();
            }
            else
            {
                // 遠距離操作を無効化
                DisablePathLineDraw();
            }
        }

        /// <summary>
        /// 遠距離(Far)操作の有効化
        /// </summary>
        private void EnablePathLineDraw()
        {
            // TODO: 未実装
        }

        /// <summary>
        /// 遠距離(Far)操作の無効化
        /// </summary>
        private void DisablePathLineDraw()
        {
            // TODO: 未実装
        }
    }
}