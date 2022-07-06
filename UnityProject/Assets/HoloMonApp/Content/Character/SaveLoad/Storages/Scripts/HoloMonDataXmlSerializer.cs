using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    // クラス名を別名でまとめて管理する
    using CUSTOMTYPE = HoloMonSaveData;

    public class HoloMonDataXmlSerializer
    {
        /// <summary>
        /// データファイル名(XML)のベース
        /// </summary>
        [SerializeField, Tooltip("データファイル名")]
        private string p_DataFileName = "HoloMonData.xml";

        /// <summary>
        /// データ読み込み
        /// </summary>
        public CUSTOMTYPE ReadData()
        {
            return XMLRead();
        }

        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <param name="a_Data"></param>
        public void WriteData(CUSTOMTYPE a_Data)
        {
            XMLWrite(a_Data);
        }

        #region "シリアライズ処理"

        /// <summary>
        /// データファイル読み込み(XML)
        /// </summary>
        private CUSTOMTYPE XMLRead()
        {
            // 返却用データ
            CUSTOMTYPE data = null;

            // オブジェクトの型を指定して Serializer オブジェクトを作成する
            System.Xml.Serialization.XmlSerializer serializer;
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(CUSTOMTYPE));

            // ファイルの存在確認
            if (System.IO.File.Exists(XMLFilePath()) == true)
            {
                // 読み込みファイルを開く
                System.IO.StreamReader streamreader;
#if WINDOWS_UWP
                // 読み込みファイルを開く(UWPアプリではStreamReader(filepath)メソッドは使用不可)
                streamreader = new System.IO.StreamReader((System.IO.Stream)System.IO.File.OpenRead(XMLFilePath()));
#else
                // 読み込みファイルを開く
                streamreader = new System.IO.StreamReader(XMLFilePath(), new System.Text.UTF8Encoding(false));
#endif
                // XMLファイルから逆シリアル化する
                data = (CUSTOMTYPE)serializer.Deserialize(streamreader);

#if WINDOWS_UWP
                // ファイルを閉じる(UWPアプリではClose()メソッドは使用不可)
                streamreader.Dispose();
#else
                // ファイルを閉じる
                streamreader.Close();
#endif
            }

            return data;
        }

        /// <summary>
        /// データファイル書き込み(XML)
        /// </summary>
        private void XMLWrite(CUSTOMTYPE a_Data)
        {
            // オブジェクトの型を指定して Serializer オブジェクトを作成する
            System.Xml.Serialization.XmlSerializer serializer;
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(CUSTOMTYPE));

            // ディレクトリの存在確認
            if (System.IO.Directory.Exists(SettingFileDirectoryPath()) == true)
            {
                // UWPの場合は書き込み前に既存のファイルを削除する
#if WINDOWS_UWP
                // ファイルの存在確認
                if (System.IO.File.Exists(XMLFilePath()) == true)
                {
                    System.IO.File.Delete(XMLFilePath());
                }
#endif

                // 書き込みファイルを開く
                System.IO.StreamWriter streamwriter;
#if WINDOWS_UWP
                // 書き込みファイルを開く(UWPアプリではStreamWriter(filepath)メソッドは使用不可)
                streamwriter = new System.IO.StreamWriter((System.IO.Stream)System.IO.File.OpenWrite(XMLFilePath()));
#else
                // 書き込みファイルを開く
                streamwriter = new System.IO.StreamWriter(XMLFilePath(), false, new System.Text.UTF8Encoding(false));
#endif

                // シリアル化してXMLファイルに保存する
                serializer.Serialize(streamwriter, a_Data);

#if WINDOWS_UWP
                // ファイルを閉じる(UWPアプリではClose()メソッドは使用不可)
                streamwriter.Dispose();
#else
                // ファイルを閉じる
                streamwriter.Close();
#endif
            }
        }
        #endregion

        #region "ファイルパス生成処理"
        /// <summary>
        /// データファイルファイルパス(XML)
        /// 実行環境によって参照ディレクトリを変更する
        /// </summary>
        private string XMLFilePath()
        {
            string filepath = System.IO.Path.Combine(SettingFileDirectoryPath(), p_DataFileName);
            return filepath;
        }

        /// <summary>
        /// データファイルディレクトリパス
        /// 実行環境によって参照ディレクトリを変更する
        /// </summary>
        private string SettingFileDirectoryPath()
        {
            string directorypath = "";
#if WINDOWS_UWP
            // HoloLens上での動作の場合、LocalAppData/AppName/LocalStateフォルダを参照する
            directorypath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#elif UNITY_IOS
            // iOS上での動作の場合、/var/mobile/Applications/アプリ番号/Documentsフォルダを参照する
            directorypath = UnityEngine.Application.persistentDataPath;
#else
            // Unity上での動作の場合、Assets/StreamingAssetsフォルダを参照する
            directorypath = UnityEngine.Application.streamingAssetsPath;
#endif
            return directorypath;
        }
#endregion
    }
}