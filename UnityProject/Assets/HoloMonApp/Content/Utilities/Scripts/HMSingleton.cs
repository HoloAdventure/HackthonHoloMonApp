using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.UtilitiesSpace
{
    // where 句で型の制約を指定する
    // 型パラメーター T が HMSingleton<T> インターフェイスを実装する
    public class HMSingleton<T> : MonoBehaviour where T : HMSingleton<T>
    {
        private static T instance;

        /// <summary>
        /// Returns the Singleton instance of the classes type.
        /// If no instance is found, then we search for an instance
        /// in the scene.
        /// If more than one instance is found, we throw an error and
        /// no instance is returned.
        /// クラスタイプのシングルトンインスタンスを返します。
        /// インスタンスが見つからない場合は、シーン内のインスタンスを検索します。
        /// 複数のインスタンスが見つかった場合、エラーがスローされてインスタンスは返されません。
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null && searchForInstance)
                {
                    searchForInstance = false;
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length == 1)
                    {
                        instance = objects[0];
                    }
                    else if (objects.Length > 1)
                    {
                        Debug.LogErrorFormat("Expected exactly 1 {0} but found {1}.", typeof(T).ToString(), objects.Length);
                    }
                }
                return instance;
            }
        }

        private static bool searchForInstance = true;

        public static void AssertIsInitialized()
        {
            Debug.Assert(IsInitialized, string.Format("The {0} singleton has not been initialized.", typeof(T).Name));
        }

        /// <summary>
        /// Returns whether the instance has been initialized or not.
        /// インスタンスが初期化されているかどうかを返します。
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return instance != null;
            }
        }

        /// <summary>
        /// Base Awake method that sets the Singleton's unique instance.
        /// Called by Unity when initializing a MonoBehaviour.
        /// Scripts that extend Singleton should be sure to call base.Awake() to ensure the
        /// static Instance reference is properly created.
        /// シングルトンの一意のインスタンスを設定するBaseAwakeメソッド。
        /// MonoBehaviourを初期化するときにUnityによって呼び出されます。
        /// シングルトンを拡張するスクリプトは、静的インスタンス参照が適切に作成されるように、必ずbase.Awake（）を呼び出す必要があります。
        /// </summary>
        protected virtual void Awake()
        {
            if (IsInitialized && instance != this)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(this);
                }
                else
                {
                    Destroy(this);
                }

                Debug.LogErrorFormat("Trying to instantiate a second instance of singleton class {0}. Additional Instance was destroyed", GetType().Name);
            }
            else if (!IsInitialized)
            {
                instance = (T)this;
            }
        }

        /// <summary>
        /// Base OnDestroy method that destroys the Singleton's unique instance.
        /// Called by Unity when destroying a MonoBehaviour. Scripts that extend
        /// Singleton should be sure to call base.OnDestroy() to ensure the
        /// underlying static Instance reference is properly cleaned up.
        /// シングルトンの一意のインスタンスを破棄するBaseOnDestroyメソッド。
        /// MonoBehaviourを破棄するときにUnityによって呼び出されます。拡張するスクリプト
        /// シングルトンは必ずbase.OnDestroy（）を呼び出して、基になる静的インスタンス参照が適切にクリーンアップされていることを確認する必要があります。
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                searchForInstance = true;
            }
        }
    }
}