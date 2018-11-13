using UnityEngine;
using UnityEditor;


public class DataOrganizer : EditorWindow
{

    private const string ASSET_FOLDER = "Assets";


    //Once tests finish, change this by an array
    #region Type of Assets
    private const string SCRIPT_TYPE = "script";       //work
    private const string SPRITE_TYPE = "...";          //unknown
    private const string PREFAB_TYPE = "prefab";       //maybe...
    private const string SCENE_TYPE = "scene";         //maybe...
    private const string SOUND_TYPE = "...";           //unknown
    private const string PHYSIC_MATERIAL_TYPE = "...";  //unknown
    private const string MATERIAL_TYPE = "...";        //unknown
    private const string ANIMATION_TYPE = "...";       //unknown
    private const string ANIMATOR_TYPE = "...";        //unknown
    
    #endregion


    [MenuItem("Custom Tools/Data Organizer")]
    public static void ShowWindow()
    {
        GetWindow<DataOrganizer>("Data Organizer");
    }

    public void OnGUI()
    {
        GUILayout.Label("Organizer", EditorStyles.boldLabel);





        GUILayout.Label("Test Zone", EditorStyles.boldLabel);

        if (GUILayout.Button("Check"))
        {
            AssetsOrganize();
        }


    }

    private void AssetsOrganize()
    {
        AssetLocationChecker(SCRIPT_TYPE);

    }

    private void AssetLocationChecker(string assetType)
    {
        string[] assetPaths = AssetDatabase.FindAssets("t:" + assetType, new[] { ASSET_FOLDER });

        if (assetPaths != null)
            foreach (string assetPath in assetPaths)
            {
                switch (assetType)
                {
                    case SCRIPT_TYPE:
                        
                        break;

                }
            }
    }

    private void CheckFolderExist(string folderName)
    {
        string[] subFolders = AssetDatabase.GetSubFolders(ASSET_FOLDER);

        bool alreadyExiste = false;

        foreach (string folder in subFolders)
        {
            string[] cutedFolderPath = folder.Split("/".ToCharArray());

            if (cutedFolderPath[cutedFolderPath.Length - 1] == folderName)
                alreadyExiste = true;
        }

        if(!alreadyExiste)
            AssetDatabase.CreateFolder(ASSET_FOLDER, folderName);
    }

}
