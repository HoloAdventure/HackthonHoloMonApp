using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class AuthorLinkBrowser : MonoBehaviour
    {
        public void LaunchAuthorLinkEdgeBrowser()
        {
            // microsoft edge: で起動すると、Microsoft Edge ブラウザでページが表示される
            var uri = @"microsoft-edge:https://twitter.com/holoAdventure";

            // URI を指定して起動する
            // bool 値の指定により URI が安全でない可能性の警告を表示します
            Launcher.LaunchUri(uri, true);
        }
    }
}