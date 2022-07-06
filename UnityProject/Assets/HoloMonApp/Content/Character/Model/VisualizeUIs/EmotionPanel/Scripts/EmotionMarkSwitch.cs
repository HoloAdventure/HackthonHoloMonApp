using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    public class EmotionMarkSwitch : MonoBehaviour
    {
        [SerializeField, Tooltip("マークオブジェクトリスト")]
        private List<GameObject> p_MarkList;

        private enum MarkNumber
        {
            Exclamation = 0,
            Question = 1,
            Stop = 2,
        }

        // Start is called before the first frame update
        void Start()
        {
            // 初期状態では全てのオブジェクトを無効化する
            foreach(GameObject mark in p_MarkList)
            {
                mark.SetActive(false);
            }
        }

        public bool ExclamationActive()
        {
            int activeNumber = (int)MarkNumber.Exclamation;

            // 指定のマークが登録されていない場合、処理しない
            if (p_MarkList.Count <= activeNumber) return false;

            // 全てのオブジェクトを無効化する
            foreach (GameObject mark in p_MarkList)
            {
                mark.SetActive(false);
            }

            // 指定のマークを有効化する
            p_MarkList[activeNumber].SetActive(true);

            return true;
        }

        public bool QuestionActive()
        {
            int activeNumber = (int)MarkNumber.Question;

            // 指定のマークが登録されていない場合、処理しない
            if (p_MarkList.Count <= activeNumber) return false;

            // 全てのオブジェクトを無効化する
            foreach (GameObject mark in p_MarkList)
            {
                mark.SetActive(false);
            }

            // 指定のマークを有効化する
            p_MarkList[activeNumber].SetActive(true);

            return true;
        }

        public bool StopActive()
        {
            int activeNumber = (int)MarkNumber.Stop;

            // 指定のマークが登録されていない場合、処理しない
            if (p_MarkList.Count <= activeNumber) return false;

            // 全てのオブジェクトを無効化する
            foreach (GameObject mark in p_MarkList)
            {
                mark.SetActive(false);
            }

            // 指定のマークを有効化する
            p_MarkList[activeNumber].SetActive(true);

            return true;
        }
    }
}