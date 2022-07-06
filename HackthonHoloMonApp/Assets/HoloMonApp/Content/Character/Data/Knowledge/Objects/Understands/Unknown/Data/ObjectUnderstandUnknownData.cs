using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandUnknownData : ObjectUnderstandDataInterface
    {
        public ObjectUnderstandUnknownData()
        {
        }

        public int StatusHash()
        {
            return 0;
        }
    }
}