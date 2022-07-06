using System;
using System.Xml;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    [Serializable]
    public class SavePackageVersion
    {
        public ushort Major;
        public ushort Minor;
        public ushort Build;
        public ushort Revision;
    }
}