using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if XRPLATFORM_MRTK3
using HoloMonApp.Content.XRPlatform.MRTK3;
#elif XRPLATFORM_MRTKV2
using HoloMonApp.Content.XRPlatform.MRTKv2;
#endif

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingHandler
    {
#if XRPLATFORM_MRTK3
        HandTrackingHandlerMRTK3 p_HandTrackingHandler;
#elif XRPLATFORM_MRTKV2
        HandTrackingHandlerMRTKv2 p_HandTrackingHandler;
#endif

        public HandTrackingHandler(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            p_HandTrackingHandler = new HandTrackingHandlerMRTK3(a_Component);
            p_HandTrackingHandler.OnSourceDetectedAction += OnSourceDetected;
            p_HandTrackingHandler.OnSourceLostAction += OnSourceLost;
            p_HandTrackingHandler.OnHandJointsUpdatedAction += OnHandJointsUpdated;
#elif XRPLATFORM_MRTKV2
            p_HandTrackingHandler = new HandTrackingHandlerMRTKv2();
            p_HandTrackingHandler.OnSourceDetectedAction += OnSourceDetected;
            p_HandTrackingHandler.OnSourceLostAction += OnSourceLost;
            p_HandTrackingHandler.OnHandJointsUpdatedAction += OnHandJointsUpdated;
#endif
        }

        // 手の検出時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceDetectedAction;

        // 手のロスト時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceLostAction;

        // 手の更新時に呼び出すアクション
        public Action<HandTrackingInputEventData> OnHandJointsUpdatedAction;

        /// <summary>
        /// 手の検出時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(HandTrackingSourceStateEventData eventData)
        {
            // アクション呼び出し
            OnSourceDetectedAction?.Invoke(eventData);
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(HandTrackingSourceStateEventData eventData)
        {
            // アクション呼び出し
            OnSourceLostAction?.Invoke(eventData);
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(HandTrackingInputEventData eventData)
        {
            // アクション呼び出し
            OnHandJointsUpdatedAction?.Invoke(eventData);
        }
    }
}
