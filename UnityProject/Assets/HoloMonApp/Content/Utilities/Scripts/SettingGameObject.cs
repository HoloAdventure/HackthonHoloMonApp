using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class SettingGameObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetTransform(Transform a_transform)
        {
            this.transform.position = a_transform.position;
            this.transform.rotation = a_transform.rotation;
        }
    }
}