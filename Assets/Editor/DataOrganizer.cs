using UnityEngine;
using UnityEditor;
using System.IO;


public class DataOrganizer : EditorWindow
{

    private bool autoChecking = false;

    private const string ASSET_FOLDER = "Assets";
    private const int POSITION_MAIN_ASSETS_FOLDER_IN_STRING = 1;

    private int numberOfException = 0;

    private string dataFileName = "Data.json";

    #region Type of Assets
    private const string SCRIPT_TYPE = "script";                            //work
    private const string SPRITE_TYPE = "sprite";                            //Work
    private const string PREFAB_TYPE = "prefab";                            //work
    private const string SCENE_TYPE = "scene";                              //work
    private const string AUDIO_CLIP_TYPE = "audioclip";                     //work
    private const string AUDIO_MIXER_TYPE = "audiomixer";                   //work
    private const string PHYSICS_MATERIAL_2D_TYPE = "physicsmaterial2d";    //work
    private const string PHYSIC_MATERIAL_TYPE = "physicmaterial";           //work
    private const string MATERIAL_TYPE = "material";                        //work
    private const string ANIMATION_TYPE = "animation";                      //work
    private const string ANIMATOR_CONTROLLER_TYPE = "animatorcontroller";   //work
    private const string RENDER_TEXTURE_TYPE = "rendertexture";             //work
    
    #endregion

    //All folder's name that should be use
    #region All Folder Name
    private const string SCRIPTS_FOLDER = "Scripts";
    private const string SPRITES_FOLDER = "Sprites";
    private const string PREFABS_FOLDER = "Prefabs";
    private const string SCENES_FOLDER = "Scenes";
    private const string SOUNDS_FOLDER = "Sounds";
    private const string MATERIALS_FOLDER = "Materials";
    private const string ANIMATIONS_FOLDER = "Animations";
    private const string TEXTURES_FOLDER = "Textures";
    private const string PHYSICS_MATERIAL_FOLDER = "Physics Material";

    private const string STREAMiNG_ASSETS_FOLDER = "StreamingAssets";

    //Folder that shouldn't be check for organization 
    private const string EXCEPTION_FOLDER = "Exceptions";
    private const string EDITOR_FOLDER = "Editor";
    private const string PLUGIN_FOLDER = "Plugins";
    private string[] exceptionFolderBonus;
    #endregion


    [MenuItem("Custom Tools/Data Organizer")]
    public static void ShowWindow()
    {
        GetWindow<DataOrganizer>("Data Organizer");
    }

    public void OnGUI()
    {
        GUILayout.Label("Organizer", EditorStyles.boldLabel);

        if (exceptionFolderBonus != null)
        {
            numberOfException = exceptionFolderBonus.Length;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            SaveData();
        }
        if (GUILayout.Button("Load"))
        {
            LoadData();
            numberOfException = exceptionFolderBonus.Length;
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Reset"))
        {
            numberOfException = 0;
            Debug.Log(Application.dataPath);
        }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Exception"))
            {
                numberOfException++;
            }

            if (GUILayout.Button("Remove Exception"))
            {
                numberOfException--;
                if (numberOfException < 0)
                    numberOfException = 0;
            }

            EditorGUILayout.EndHorizontal();

        //Display field to enter name if one or more exception needed
        if(numberOfException > 0)
        {
            string[] transitionExceptionFolderName = new string[numberOfException];

            for (int i = 0; i < exceptionFolderBonus.Length && i < numberOfException; i++)
            {
                transitionExceptionFolderName[i] = exceptionFolderBonus[i];
            }

            for (int i = 0; i < transitionExceptionFolderName.Length; i++)
            {
                transitionExceptionFolderName[i] = EditorGUILayout.TextField("Exception Folder " + i + " ", transitionExceptionFolderName[i]);
            }

            exceptionFolderBonus = transitionExceptionFolderName;
        }
        else
        {
            exceptionFolderBonus = new string[0];
        }


        autoChecking = EditorGUILayout.Toggle("Active auto checker", autoChecking);

        if (autoChecking)
            AssetsOrganize();

        if (GUILayout.Button("Manual Check"))
        {
            AssetsOrganize();
        }
    }


    //This method is the main method of the tool, it act like an update
    private void AssetsOrganize()
    {
        OrganizeAssets(SCRIPT_TYPE, SCRIPTS_FOLDER);

        OrganizeAssets(SPRITE_TYPE, SPRITES_FOLDER);

        OrganizeAssets(PREFAB_TYPE, PREFABS_FOLDER);

        OrganizeAssets(SCENE_TYPE, SCENES_FOLDER);

        OrganizeAssets(AUDIO_CLIP_TYPE, SOUNDS_FOLDER);

        OrganizeAssets(AUDIO_MIXER_TYPE, SOUNDS_FOLDER);

        OrganizeAssets(ANIMATION_TYPE, ANIMATIONS_FOLDER);

        OrganizeAssets(ANIMATOR_CONTROLLER_TYPE, ANIMATIONS_FOLDER);

        OrganizeAssets(PHYSICS_MATERIAL_2D_TYPE, PHYSICS_MATERIAL_FOLDER);

        OrganizeAssets(PHYSIC_MATERIAL_TYPE, PHYSICS_MATERIAL_FOLDER);

        OrganizeAssets(MATERIAL_TYPE, MATERIALS_FOLDER);

        OrganizeAssets(RENDER_TEXTURE_TYPE, TEXTURES_FOLDER);
    }


    //This method search all asset and than execute the other method/function
    //to know if an asset must be move or not 
    private void OrganizeAssets(string assetType, string assetGoodLocationFolder)
    {
        string[] assetsGUID = AssetDatabase.FindAssets("t:" + assetType, new[] { ASSET_FOLDER });
        
        if(assetsGUID == null)
            return;


        foreach (string assetGUID in assetsGUID)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
            string[] cutedAssetPath = assetPath.Split("/".ToCharArray());

            CheckFolderExist(assetGoodLocationFolder);

            if (CheckAssetsIsInWrongFolder(cutedAssetPath, assetGoodLocationFolder) && !CheckAsstetsIsInExceptionFolder(cutedAssetPath))
            {
                if (!GetMoveToExceptionFolderChoice(assetPath, assetGoodLocationFolder, cutedAssetPath, assetType))
                {
                    MoveAssetToExceptionFolder(assetPath, cutedAssetPath);
                }
                else
                {
                    MoveAssetToGoodLocation(assetPath, cutedAssetPath, assetGoodLocationFolder);
                }
            }
        }

    }

    //This method check if the folder where we want the asset to go exist or if we must create it
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

    //This function return true if the assets is directly in the main folder or if it is in the wrong folder
    private bool CheckAssetsIsInWrongFolder(string[] cutedAssetPath, string assetGoodLocationFolder)
    {
        bool isInMainFolder = cutedAssetPath.Length - 1 == POSITION_MAIN_ASSETS_FOLDER_IN_STRING;
        bool isInWrongFolder = cutedAssetPath[POSITION_MAIN_ASSETS_FOLDER_IN_STRING] != assetGoodLocationFolder;

        return (isInMainFolder || isInWrongFolder);
    }

    //This function return true if the assets is in an exception folder so it must not be moved
    private bool CheckAsstetsIsInExceptionFolder(string[] cutedAssetPath)
    {
        bool isInExceptionFolder = false;

        for (int i = 1; i < cutedAssetPath.Length - 1; i++)
        {
            if (cutedAssetPath[i] == EXCEPTION_FOLDER || cutedAssetPath[i] == EDITOR_FOLDER || cutedAssetPath[i] == PLUGIN_FOLDER)
            {
                isInExceptionFolder = true;
                break;
            }

            if (exceptionFolderBonus != null)
            {
                foreach (string exceptionFolderName in exceptionFolderBonus)
                {
                    if (cutedAssetPath[i] == exceptionFolderName)
                    {
                        isInExceptionFolder = true;
                        break;
                    }
                }

                if (isInExceptionFolder)
                {
                    break;
                }
            }
        }

        return isInExceptionFolder;
    }

    //This function is to display a popup window that will ask the user if he want to move the assets at the good place
    //or if he want move it in a exception folder
    private bool GetMoveToExceptionFolderChoice(string assetPath, string assetGoodLocationFolder, string[] cutedAssetPath, string assetType)
    {
        string[] cutedAssetName = cutedAssetPath[cutedAssetPath.Length - 1].Split(".".ToCharArray());

        string assetName = cutedAssetName[cutedAssetName.Length - 1];

        return EditorUtility.DisplayDialog( "WARNING : Asset wrong location",
                                            "Your " + assetType + " <" + assetName + "> is at the wrong place \n" +
                                            "\n" +
                                            "Do you want to move it in the good folder ? \n" +
                                            "Current path : " + assetPath + "\n" +
                                            "\n" +
                                            "If you want to move it : \n" +
                                            "New path : " + ASSET_FOLDER + "/"+ assetGoodLocationFolder + "/" + cutedAssetPath[cutedAssetPath.Length - 1] + "\n" +
                                            "\n" +
                                            "or let it here and setup an exception folder ?\n" +
                                            "Exception path : " + GetExceptionFolderPath(cutedAssetPath) + "/" + cutedAssetPath[cutedAssetPath.Length - 1] + "\n",
                                            "Move it",
                                            "Exception");
    }

    //This method move the asset to the folder that it receive the name
    private void MoveAssetToGoodLocation(string assetPath, string[] cutedAssetPath, string assetGoodLocationFolder)
    {
        AssetDatabase.MoveAsset(assetPath, ASSET_FOLDER + "/" + assetGoodLocationFolder + "/" + cutedAssetPath[cutedAssetPath.Length - 1]);
    }

    //This method check if an exception folder exist at the current place, create one if not,
    //and then move the asset in it
    private void MoveAssetToExceptionFolder(string assetPath, string[] cutedAssetPath)
    {
        string exceptionFolderPath = GetExceptionFolderPath(cutedAssetPath);
        string[] cutedExceptionPath = exceptionFolderPath.Split("/".ToCharArray());
        string exceptionFolderLocation = "";

        for (int i = 0; i < cutedExceptionPath.Length - 1; i++)
        {
            exceptionFolderLocation += cutedExceptionPath[i] ;

            if (i < cutedExceptionPath.Length - 2)
            {
                exceptionFolderLocation += "/";
            }
        }

        string[] subFolders = AssetDatabase.GetSubFolders(exceptionFolderLocation);

        bool alreadyExiste = false;

        foreach (string folder in subFolders)
        {
            string[] cutedFolderPath = folder.Split("/".ToCharArray());

            if (cutedFolderPath[cutedFolderPath.Length - 1] == EXCEPTION_FOLDER)
                alreadyExiste = true;
        }

        if (!alreadyExiste)
        {
            AssetDatabase.CreateFolder(exceptionFolderLocation, EXCEPTION_FOLDER);
        }

        AssetDatabase.MoveAsset(assetPath, exceptionFolderPath + "/" + cutedAssetPath[cutedAssetPath.Length - 1]);
    }

    //This function return a string of the the location where the exception folder should be
    private string GetExceptionFolderPath(string[] cutedAssetPath)
    {
        string exceptionFolderPath = "";
        for (int i = 0; i < cutedAssetPath.Length - 1; i++)
        {
            exceptionFolderPath += cutedAssetPath[i] + "/";
        }

        exceptionFolderPath += EXCEPTION_FOLDER;
        return exceptionFolderPath;
    }

    private void SaveData()
    {
        CheckFolderExist(EDITOR_FOLDER);
        string filePath = Path.Combine(UnityProjectEditorFolderPath(), dataFileName);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        DataOrganizer_Data data = new DataOrganizer_Data();
        data.exceptionFoldersName = exceptionFolderBonus;

        string DataAsJson = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, DataAsJson);
    }

    private void LoadData()
    {
        CheckFolderExist(EDITOR_FOLDER);
        
        string filePath = Path.Combine(UnityProjectEditorFolderPath(), dataFileName);

        Debug.Log(Application.streamingAssetsPath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        string dataAsJson = File.ReadAllText(filePath);
        DataOrganizer_Data loadedData = JsonUtility.FromJson<DataOrganizer_Data>(dataAsJson);

        exceptionFolderBonus = loadedData.exceptionFoldersName;
    }

    private string UnityProjectEditorFolderPath()
    {
        return Application.dataPath + "/" + EDITOR_FOLDER;
    }
}
