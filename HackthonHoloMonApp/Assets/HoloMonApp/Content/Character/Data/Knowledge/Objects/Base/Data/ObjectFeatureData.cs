using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectFeatureData
    {
        [SerializeField, Tooltip("ゲームオブジェクトの参照")]
        public GameObject GameObject;

        [SerializeField, Tooltip("オブジェクトの理解種別情報")]
        public ObjectUnderstandInformation UnderstandInformation;
    }
}