using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(WordBubbleController))]

    public class WordBubbleControllerEditor : Editor
    {
        // GUIの表示関数をオーバーライドする
        public override void OnInspectorGUI()
        {
            // 元のインスペクター部分を表示
            base.OnInspectorGUI();

            // targetを変換して対象スクリプトの参照を取得する
            WordBubbleController targetScript = target as WordBubbleController;

            // public関数を実行するボタンの作成
            if (GUILayout.Button("NonBreakStartの実行"))
            {
                targetScript.NonBreakStart();
            }

            // public関数を実行するボタンの作成
            if (GUILayout.Button("BreakStartの実行"))
            {
                targetScript.BreakStart();
            }
        }
    }
}
#endif