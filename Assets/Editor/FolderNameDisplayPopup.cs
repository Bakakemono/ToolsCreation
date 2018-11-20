using UnityEngine;
using UnityEditor;

[System.Serializable]
public class FolderNameDisplayPopup : PopupWindowContent {

    #region All Folders Name
    public string scriptsFolder = "Scripts";
    public string spritesFolder = "Sprites";
    public string prefabsFolder = "Prefabs";
    public string scenesFolder = "Scenes";
    public string soundsFolder = "Sounds";
    public string materialsFolder = "Materials";
    public string animationsFolder = "Animations";
    public string texturesFolder = "Textures";
    public string physicsMaterialFolder = "Physics Material";
    #endregion

    private Vector2 size = new Vector2(300, 200);

    public override Vector2 GetWindowSize()
    {
        return size;
    }

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("Type to move automatically", EditorStyles.boldLabel);

        scriptsFolder = EditorGUILayout.TextField("Scripts folder", scriptsFolder);

        spritesFolder = EditorGUILayout.TextField("Sprites folder", spritesFolder);

        prefabsFolder = EditorGUILayout.TextField("Prefabs folder", prefabsFolder);

        scenesFolder = EditorGUILayout.TextField("Scenes folder", scenesFolder);

        soundsFolder = EditorGUILayout.TextField("Sounds folder", soundsFolder);

        materialsFolder = EditorGUILayout.TextField("Materials folder", materialsFolder);

        animationsFolder = EditorGUILayout.TextField("Animations folder", animationsFolder);

        texturesFolder = EditorGUILayout.TextField("Textures folder", texturesFolder);

        physicsMaterialFolder = EditorGUILayout.TextField("Physics Material folder", physicsMaterialFolder);
    }

}
