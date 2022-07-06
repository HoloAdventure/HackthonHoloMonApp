using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    // クラス名を別名でまとめて管理する
    using CUSTOMTYPE = HoloMonSaveData;

    public class HoloMonDataXmlSerializerCryptography
    {
        // Encryption key used to encrypt the stream.
        // The same value must be used to encrypt and decrypt the stream.
        // ストリームの暗号化に使用される暗号化キー。
        // ストリームの暗号化と復号化には同じ値を使用する必要があります。
        byte[] key = { 0x48, 0x4F, 0x4C, 0x4F, 0x4D, 0x4F, 0x4E, 0x41, 0x44, 0x56, 0x45, 0x4E, 0x54, 0x55, 0x52, 0x45 };

        /// <summary>
        /// データファイル名(暗号化XML)のベース
        /// </summary>
        private string p_DataFileNameCryptography = "HoloMonData.bin";

        /// <summary>
        /// データ読み込み
        /// </summary>
        public CUSTOMTYPE ReadData()
        {
            return XMLReadCryptography();
        }

        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <param name="a_Data"></param>
        public void WriteData(CUSTOMTYPE a_Data)
        {
            XMLWriteCryptography(a_Data);
        }

        #region "暗号化ファイル処理"
        /// <summary>
        /// データファイル読み込み(暗号化XML)
        /// （データが短いと読み込みに失敗するので注意）
        /// </summary>
        private CUSTOMTYPE XMLReadCryptography()
        {
            // 返却用データ
            CUSTOMTYPE data = null;

            // オブジェクトの型を指定して Serializer オブジェクトを作成する
            System.Xml.Serialization.XmlSerializer serializer;
            serializer = new System.Xml.Serialization.XmlSerializer(typeof(CUSTOMTYPE));

            // ファイルの存在確認
            if (System.IO.File.Exists(XMLFilePathCryptography()) == true)
            {
                // ファイルストリームを作成する。
                using (System.IO.FileStream myStream =
                    new System.IO.FileStream(XMLFilePathCryptography(), System.IO.FileMode.Open))
                {
                    // デフォルトのAes実装クラスの新しいインスタンスを作成します。
                    using (System.Security.Cryptography.Aes aes =
                        System.Security.Cryptography.Aes.Create())
                    {
                        // ファイルの先頭からIV値を読み取ります。
                        byte[] iv = new byte[aes.IV.Length];
                        myStream.Read(iv, 0, iv.Length);

                        // CryptoStreamを作成し、ファイルストリームを渡し、キーとIVを使用してAesクラスで復号化します。
                        using (System.Security.Cryptography.CryptoStream cryptStream =
                            new System.Security.Cryptography.CryptoStream(
                            myStream,
                            aes.CreateDecryptor(key, iv),
                            System.Security.Cryptography.CryptoStreamMode.Read))
                        {
                            // 読み込みファイルを開く
                            using (System.IO.StreamReader streamreader =
                                new System.IO.StreamReader(cryptStream))
                            {
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
                        }
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// データファイル書き込み(暗号化XML)
        /// （データが短いと読み込みに失敗するので注意）
        /// </summary>
        private void XMLWriteCryptography(CUSTOMTYPE a_Data)
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
                if (System.IO.File.Exists(XMLFilePathCryptography()) == true)
                {
                    System.IO.File.Delete(XMLFilePathCryptography());
                }
#endif
                // ファイルストリームを作成する。
                using (System.IO.FileStream myStream =
                    new System.IO.FileStream(XMLFilePathCryptography(), System.IO.FileMode.OpenOrCreate))
                {
                    // デフォルトのAes実装クラスの新しいインスタンスを作成します。
                    using (System.Security.Cryptography.Aes aes =
                        System.Security.Cryptography.Aes.Create())
                    {
                        // 暗号化キーを構成します。
                        aes.Key = key;

                        // ファイルの先頭にIVを格納します。
                        // この情報は復号化に使用されます。
                        byte[] iv = aes.IV;
                        myStream.Write(iv, 0, iv.Length);

                        // CryptoStreamを作成し、FileStreamを渡して、Aesクラスで暗号化します。
                        using (System.Security.Cryptography.CryptoStream cryptStream =
                            new System.Security.Cryptography.CryptoStream(
                            myStream,
                            aes.CreateEncryptor(),
                            System.Security.Cryptography.CryptoStreamMode.Write))
                        {
                            // 書き込みファイルを開く
                            using (System.IO.StreamWriter streamwriter =
                                new System.IO.StreamWriter(cryptStream))
                            {
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
                    }
                }
            }
        }
        #endregion

        #region "ファイルパス生成処理"
        /// <summary>
        /// データファイルファイルパス(暗号化XML)
        /// 実行環境によって参照ディレクトリを変更する
        /// </summary>
        private string XMLFilePathCryptography()
        {
            string filepath = System.IO.Path.Combine(SettingFileDirectoryPath(), p_DataFileNameCryptography);
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