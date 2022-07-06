using Cysharp.Threading.Tasks;
using HoloMonApp.Content.Character.Interrupt;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    public interface HoloMonActionModeLogicIF
    {
        /// <summary>
        /// 初期化
        /// </summary>
        void AwakeInit(HoloMonBehaveReference reference);

        /// <summary>
        /// 現在の実行待機中フラグ
        /// </summary>
        /// <returns></returns>
        bool CurrentRunAwaitFlg();

        /// <summary>
        /// ホロモンアクションモード種別
        /// </summary>
        HoloMonActionMode GetHoloMonActionMode();

        /// <summary>
        /// モード実行(async/await制御)
        /// </summary>
        UniTask<ModeLogicResult> RunModeAsync(ModeLogicSetting a_ModeLogicSetting);

        /// <summary>
        /// モードキャンセル(async/await制御)
        /// </summary>
        void CancelMode();

        /// <summary>
        /// 割込み通知
        /// </summary>
        bool TransmissionInterrupt(InterruptInformation a_InterruptInfo);
    }
}