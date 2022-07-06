using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform
{
    public class HoloMonXRPlatform : MonoBehaviour
    {
        /// <summary>
        /// 単一インスタンス
        /// </summary>
        private static HoloMonXRPlatform ps_instance;
        private static bool ps_searchForInstance = true;

        /// <summary>
        /// 参照用インスタンス
        /// </summary>
        public static HoloMonXRPlatform Instance
        {
            get
            {
                if (ps_instance == null && ps_searchForInstance)
                {
                    ps_searchForInstance = false;
                    var ps_instances = FindObjectsOfType<HoloMonXRPlatform>();
                    if (ps_instances.Length == 1)
                    {
                        ps_instance = ps_instances[0];
                    }
                    else if (ps_instances.Length > 1)
                    {
                        Debug.LogErrorFormat("Expected exactly 1 {0} but found {1}.", ps_instance.GetType().ToString(), ps_instances.Length);
                    }
                }
                return ps_instance;
            }
        }


        public EnvironmentMapAPI EnvironmentMap { get; private set; }

        public HandTrackingAPI HandTracking { get; private set; }

        public MainCameraAPI MainCamera { get; private set; }

        private void Awake()
        {
            EnvironmentMap = new EnvironmentMapAPI(this);
            HandTracking = new HandTrackingAPI(this);
            MainCamera = new MainCameraAPI(this);
        }
    }
}