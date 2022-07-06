using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLookAtController : MonoBehaviour
{
    private enum TrackingAxis
    {
        All,
        XAxis,
        YAxis,
        ZAxis,
    }

    [SerializeField, Tooltip("追跡の有効無効")]
    private bool p_isLookAtTracking;

    [SerializeField, Tooltip("軸の反転フラグ")]
    private bool p_isReverse;

    [SerializeField, Tooltip("LookAt対象のハンドルトランスフォーム")]
    private Transform p_LookAtHandleTransform;

    [SerializeField, Tooltip("ローカル回転軸の指定")]
    private TrackingAxis p_LocalTrackingAxis;

    [SerializeField, Tooltip("ハンドルリセット位置のトランスフォーム")]
    private Transform p_HandleResetTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p_isLookAtTracking)
        {
            Tracking();
        }
    }

    /// <summary>
    /// 追跡フラグの切り替え
    /// </summary>
    /// <param name="onoff"></param>
    public void ChangeLookAtTracking(bool onoff)
    {
        p_isLookAtTracking = onoff;
    }

    /// <summary>
    /// ハンドルの位置を動きに合わせてリセットする
    /// </summary>
    public void ResetHandlePosition()
    {
        p_LookAtHandleTransform.transform.position = p_HandleResetTransform.position;
        p_LookAtHandleTransform.transform.rotation = p_HandleResetTransform.rotation;
    }

    /// <summary>
    /// 角度の手動変更
    /// </summary>
    /// <param name="a_LocalEulerAngle"></param>
    public void ChangeAngle(Vector3 a_LocalEulerAngle)
    {
        this.transform.localEulerAngles = a_LocalEulerAngle;

        // ハンドル位置をリセット
        ResetHandlePosition();
    }

    /// <summary>
    /// 軸設定に合わせてLookAt方向を追跡する
    /// </summary>
    private void Tracking()
    {
        if (p_LookAtHandleTransform != null)
        {
            this.transform.LookAt(p_LookAtHandleTransform);
            float reverseFact = 1.0f;
            if (p_isReverse) reverseFact = -1.0f;
            switch (p_LocalTrackingAxis)
            {
                case TrackingAxis.All:
                    this.transform.localEulerAngles = new Vector3(
                        reverseFact * this.transform.localEulerAngles.x,
                        reverseFact * this.transform.localEulerAngles.y,
                        reverseFact * this.transform.localEulerAngles.z
                        );
                    break;
                case TrackingAxis.XAxis:
                    this.transform.localEulerAngles = new Vector3(
                        reverseFact * this.transform.localEulerAngles.x,
                        0.0f,
                        0.0f
                        );
                    break;
                case TrackingAxis.YAxis:
                    this.transform.localEulerAngles = new Vector3(
                        0.0f,
                        reverseFact * this.transform.localEulerAngles.y,
                        0.0f
                        );
                    break;
                case TrackingAxis.ZAxis:
                    this.transform.localEulerAngles = new Vector3(
                        0.0f,
                        0.0f,
                        reverseFact * this.transform.localEulerAngles.z
                        );
                    break;
                default:
                    break;
            }
        }
    }
}
