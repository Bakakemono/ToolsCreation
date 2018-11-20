using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TypeDisplayPopup : PopupWindowContent
{
    public bool script = false;
    public bool sprite = false;
    public bool prefab = false;
    public bool scene = false;
    public bool audioClip = false;
    public bool audioMixer = false;
    public bool physicsMaterial2D = false;
    public bool physicMaterial = false;
    public bool material = false;
    public bool animation = false;
    public bool animatorController = false;
    public bool rendererTexture = false;

    private Vector2 size = new Vector2(190, 250);

    public override Vector2 GetWindowSize()
    {
        return size;
    }

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label("Type to move automatically", EditorStyles.boldLabel);

        script = EditorGUILayout.Toggle("Script", script);

        sprite = EditorGUILayout.Toggle("Sprite", sprite);

        prefab = EditorGUILayout.Toggle("Prefab", prefab);

        scene = EditorGUILayout.Toggle("Scene", scene);

        audioClip = EditorGUILayout.Toggle("Audio Clip", audioClip);

        audioMixer = EditorGUILayout.Toggle("Audio Mixer", audioMixer);

        physicsMaterial2D = EditorGUILayout.Toggle("Physics Material 2D", physicsMaterial2D);

        physicMaterial = EditorGUILayout.Toggle("Physic Material", physicMaterial);

        material = EditorGUILayout.Toggle("Material", material);

        animation = EditorGUILayout.Toggle("Animation", animation);

        animatorController = EditorGUILayout.Toggle("Animator Controller", animatorController);

        rendererTexture = EditorGUILayout.Toggle("Renderer Texture", rendererTexture);
    }
}
