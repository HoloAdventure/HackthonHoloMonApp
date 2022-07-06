using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    [Serializable]
    public class ModeLogicReference
    {
        // 共通参照
        private HoloMonBehaveReference p_BehaveReference;


        // 参照用短縮変数
        public HoloMonControlAPI Control => p_BehaveReference.Control;

        public HoloMonViewAPI View => p_BehaveReference.View;


        public ModeLogicReference(HoloMonBehaveReference behaveReference)
        {
            p_BehaveReference = behaveReference;
        }
    }
}