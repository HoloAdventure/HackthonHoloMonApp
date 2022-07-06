using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
// クラス名を別名でまとめて管理する
using CUSTOMTYPE = HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel.HoloMonEmotionPanelAPI;

using UnityEditor;
using System.Reflection;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(CUSTOMTYPE))]
    public class HoloMonEmotionPanelAPIEditor : Editor
    {
        // GUIの表示関数をオーバーライドする
        public override void OnInspectorGUI()
        {
            // 元のインスペクター部分を表示
            base.OnInspectorGUI();

            // targetを変換して対象スクリプトの参照を取得する
            CUSTOMTYPE targetScript = target as CUSTOMTYPE;

            // Editor実行中のみ有効化なUIを設定する
            if (EditorApplication.isPlaying)
            {
                // Publicかつ指定したクラス階層のメソッドのみの一覧を取得する
                BindingFlags flag = BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.DeclaredOnly;
                MethodInfo[] methods = targetScript.GetType().GetMethods(flag);

                foreach(MethodInfo method in methods)
                {
                    // メソッドの引数一覧を取得する
                    ParameterInfo[] methodParam = method.GetParameters();

                    // 引数がない関数のみ実行ボタンを作成する
                    if(methodParam.Length == 0)
                    {
                        // public関数を実行するボタンの作成
                        if (GUILayout.Button(method.Name + "の実行"))
                        {
                            method.Invoke(targetScript, null);
                        }
                    }
                }
            }
        }
    }
}
#endif
