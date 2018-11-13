using UnityEngine;
using UnityEditor;

public class AccessDataWindow : EditorWindow
{
    private const string ASSETS_MAIN_FOLDER = "Assets";
    private string folderNameNew = "New Folder";

    private string assetTypeName = "scripts";

    #region Assets Type

    private string scriptType = "script";
    
    

        #endregion

    [MenuItem("Example/Data Manager")]
    public static void ShowWindow()
    {
        GetWindow<AccessDataWindow>("Data Manager");
    }

    public void OnGUI()
    {
        GUILayout.Label("Create a new folder", EditorStyles.boldLabel);

        folderNameNew = EditorGUILayout.TextField("Name of the new folder", folderNameNew);
        

        if (GUILayout.Button("Create folder"))
        {
            CreateFolder();
        }

        GUILayout.Label("Move Assets", EditorStyles.boldLabel);

        assetTypeName = EditorGUILayout.TextField("Enter the type of assets that you want to move", assetTypeName);

        if(GUILayout.Button("Move This"))
        {
            MoveAssets();
        }
        //EditorGUILayout.EnumPopup()
    }

    private void CreateFolder()
    {
        bool isCreated = false;

        //It work but it doesn't give me a good name
        foreach (string folder in AssetDatabase.GetSubFolders("Assets"))
        {
            if ((ASSETS_MAIN_FOLDER + "/" + folderNameNew) == folder)
            {
                isCreated = true;
                break;
            }
        }

        if (!isCreated)
        {
            //string guid = AssetDatabase.CreateFolder("Assets", folderNameNew);
        }

        else
        {
            EditorUtility.DisplayDialog("WARNING", "This folder already exist", "OK");
        }
    }

    private void MoveAssets()
    {
        //AssetDatabase.MoveAsset(ASSETS_MAIN_FOLDER + "/Blubl", ASSETS_MAIN_FOLDER + "/Scripts/Blubl");
        string[] guids = AssetDatabase.FindAssets("t:" + assetTypeName, new[] { ASSETS_MAIN_FOLDER });

        foreach (string guid in guids)
        {
            if (!AssetDatabase.GUIDToAssetPath(guid).Contains("Editor"))
            {
                string asset = AssetDatabase.GUIDToAssetPath(guid);
                string[] originalAssetsPath = asset.Split("/".ToCharArray());
                
                
                string ErrorCheck = AssetDatabase.MoveAsset(asset, ASSETS_MAIN_FOLDER + "/" + "TestMove" + "/" + originalAssetsPath[originalAssetsPath.Length - 1]);
            }
        }
    }

    private void moveFileWarning (string typeOfMessage, string oldPathFolder, string newPathFolder, string assetName)
    {

    }
}
