using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    public interface HoloMonHeadLogicInterface
    {
        /// <summary>
        /// 現在のモードを取得する
        /// </summary>
        /// <returns></returns>
        HoloMonActionHeadStatus GetHoloMonHeadStatus();

        /// <summary>
        /// EveryValueChanged オブザーバ参照変数を取得する
        /// </summary>
        /// <returns></returns>
        IObservable<HoloMonActionHeadStatus> GetHoloMonHeadStatusEveryValueChanged();

        /// <summary>
        /// モードを有効化する
        /// </summary>
        bool EnableHead();

        /// <summary>
        /// モードを無効化する
        /// </summary>
        bool DisableHead(HoloMonActionHeadStatus a_DisableStatus);

        /// <summary>
        /// モード設定を反映する
        /// </summary>
        bool ApplySetting(HeadLogicSetting a_HeadLogicSetting);
    }
}