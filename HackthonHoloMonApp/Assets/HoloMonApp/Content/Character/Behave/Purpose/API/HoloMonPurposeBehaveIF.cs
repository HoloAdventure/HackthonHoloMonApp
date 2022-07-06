using Cysharp.Threading.Tasks;
using HoloMonApp.Content.Character.Behave.ModeLogic;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    public interface HoloMonPurposeBehaveIF
    {
        /// <summary>
        /// 初期化
        /// </summary>
        void AwakeInit(HoloMonBehaveReference behaveReference, HoloMonActionModeLogicAPI actionModeLogicAPI);

        /// <summary>
        /// ホロモン目的行動種別
        /// </summary>
        HoloMonPurposeType GetHoloMonPurposeType();

        /// <summary>
        /// ロジックの実行
        /// </summary>
        UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse);
    }
}