using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(EmotionUIController))]
    public class EmotionUIControllerEditor : Editor
    {
        // GUIの表示関数をオーバーライドする
        public override void OnInspectorGUI()
        {
            // 元のインスペクター部分を表示
            base.OnInspectorGUI();

            // targetを変換して対象スクリプトの参照を取得する
            EmotionUIController targetScript = target as EmotionUIController;

            // public関数を実行するボタンの作成
            if (GUILayout.Button("ExclamationStartの実行"))
            {
                targetScript.ExclamationStart();
            }

            // public関数を実行するボタンの作成
            if (GUILayout.Button("QuestionStartの実行"))
            {
                targetScript.QuestionStart();
            }

            // public関数を実行するボタンの作成
            if (GUILayout.Button("IgnoredStartの実行"))
            {
                targetScript.IgnoredStart();
            }
        }
    }
}
#endif