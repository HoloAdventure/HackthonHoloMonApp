using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(WordBubbleAnimation))]
    public class WordBubbleAnimationEditor : Editor
    {
        // GUIの表示関数をオーバーライドする
        public override void OnInspectorGUI()
        {
            // 元のインスペクター部分を表示
            base.OnInspectorGUI();

            // targetを変換して対象スクリプトの参照を取得する
            WordBubbleAnimation targetScript = target as WordBubbleAnimation;

            // public関数を実行するボタンの作成
            if (GUILayout.Button("NeutralAnimationの実行"))
            {
                targetScript.ActiveAnimation();
            }

            // public関数を実行するボタンの作成
            if (GUILayout.Button("NonBreakAnimationの実行"))
            {
                targetScript.NonBreakAnimation();
            }

            // public関数を実行するボタンの作成
            if (GUILayout.Button("BreakAnimationの実行"))
            {
                targetScript.BreakAnimation();
            }
        }
    }
}
#endif