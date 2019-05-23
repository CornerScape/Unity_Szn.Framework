using System;
using System.Collections.Generic;
using UnityEngine;

namespace SznFramework.SQLite3Helper
{
    [Serializable]
    public class SQLite3SingleData
    {
        public string Name;
        public string LocalName;
        public string Md5;

        public override string ToString()
        {
            return "Name = " + Name + "; LocalName = " + LocalName + "; Md5 = " + Md5;
        }
    }

    [CreateAssetMenu(menuName = "Create Sqlite Data")]
    public class SQLite3Data : ScriptableObject
    {
        public List<SQLite3SingleData> AllData;
    }
}
