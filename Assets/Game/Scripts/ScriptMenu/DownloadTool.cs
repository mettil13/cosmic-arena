using System;
using UnityEditor;
using UnityEngine;

public class DownloadTool : EditorWindow
{
    private string tableId;
    private string sheetId;

    [MenuItem("Window/Download Google Sheet")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DownloadTool));
    }

     void OnGUI()
    {
        tableId = EditorGUILayout.TextField("Id Tabella", tableId);
        sheetId = EditorGUILayout.TextField("Id Foglio", sheetId);

        if(GUILayout.Button("Scarica Foglio!"))
        {
            string url = "https://docs.google.com/spreadsheets/d/" + tableId + "/export?format=csv";

            if (!string.IsNullOrEmpty(sheetId))
                url += "#gid=" + sheetId;

            WWWForm form = new WWWForm();
            WWW download = new WWW(url, form);
            while (!download.isDone)
            {
                
            }
            GoogleSheetDownload.DownloadCSVCoroutine(download, true, sheetId);
            AssetDatabase.Refresh();

            switch (sheetId)
            {
                case "Personaggi":
                    ScriptableReader.ReadCharacterSO(sheetId);
                    break;
            }
        }
    }

}
