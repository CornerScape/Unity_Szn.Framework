#if UNIT_TEST
using SznFramework.SQLite3Helper;
using UnityEngine;

public class Sqlite3Test : MonoBehaviour
{

    void OnGUI()
    {
        GUI.skin.button.fontSize = 32;
        if (GUILayout.Button("Load Sqlite3 Data"))
        {
            SQLite3Data data = Resources.Load<SQLite3Data>("Sqlite3Data");
            foreach (SQLite3SingleData singleData in data.AllData)
            {
                Debug.LogError(singleData);
            }
        }
    }
}
#endif