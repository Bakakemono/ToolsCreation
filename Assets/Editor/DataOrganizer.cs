using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityScript.Steps;


public class DataOrganizer : EditorWindow
{

    private bool isActive = true;

    private const string ASSET_FOLDER = "Assets";
    private const int POSITION_MAIN_ASSETS_FOLDER_IN_STRING = 1;

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

    #endregion

    //All folder's name that should be use
    #region All Folder Name
    private const string SCRIPTS_FOLDER = "Scripts";
    private const string SPRITES_FOLDER = "Sprites";
    private const string PREFABS_FOLDER = "Prefabs";
    private const string SCENES_FOLDER = "Scenes";
    private const string SOUNDS_FOLDER = "Sounds";
    private const string MATERIALS_FOLDER = "Materials";

    //Folder that shouldn't be check for organization 
    private const string EXCEPTION_FOLDER = "Exceptions";
    private const string EDITOR_FOLDER = "Editor";
    #endregion


    [MenuItem("Custom Tools/Data Organizer")]
    public static void ShowWindow()
    {
        GetWindow<DataOrganizer>("Data Organizer");
    }

    public void OnGUI()
    {
        GUILayout.Label("Organizer", EditorStyles.boldLabel);

        isActive = EditorGUILayout.Toggle("Active auto checker", isActive);

        if (isActive)
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

        OrganizeAssets(ANIMATION_TYPE, SPRITES_FOLDER);

        OrganizeAssets(ANIMATOR_CONTROLLER_TYPE, SPRITES_FOLDER);

        OrganizeAssets(PHYSICS_MATERIAL_2D_TYPE, MATERIALS_FOLDER);

        OrganizeAssets(PHYSIC_MATERIAL_TYPE, MATERIALS_FOLDER);

        OrganizeAssets(MATERIAL_TYPE, MATERIALS_FOLDER);
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
                if (!GetMoveToExceptionFolderChoice(assetPath, assetGoodLocationFolder, cutedAssetPath))
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
            if (cutedAssetPath[i] == EXCEPTION_FOLDER || cutedAssetPath[i] == EDITOR_FOLDER)
            {
                isInExceptionFolder = true;
                break;
            }
        }

        return isInExceptionFolder;
    }

    //This function is to display a popup window that will ask the user if he want to move the assets at the good place
    //or if he want move it in a exception folder
    private bool GetMoveToExceptionFolderChoice(string assetPath, string assetGoodLocationFolder, string[] cutedAssetPath)
    {
        return EditorUtility.DisplayDialog( "Your assets <" + cutedAssetPath[cutedAssetPath.Length - 1] + "> is at the wrong place", 
                                            "Current path : " + assetPath + "\n" +
                                            "Do you want to move it in the good folder ? \n" +
                                            "If you want to move it : \n" +
                                            "New path : " + ASSET_FOLDER + "/"+ assetGoodLocationFolder + "/" + cutedAssetPath[cutedAssetPath.Length - 1] + "\n" +
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
}
