using UnityEngine;
using UnityEditor;

public class ColorizerWindow : EditorWindow
{
    private Color color;

    [MenuItem("Example/Colorizer")]
    public static void ShowWindow()
    {
        GetWindow<ColorizerWindow>("Colorizer");
    }

    void OnGUI()
    {
        GUILayout.Label("Colorize all this STUFF", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("COLORIZE!"))
        {
            Colorize();
        }
    }

    private void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();

            if (sprite != null)
            {
                sprite.color = color;
            }
        }
    }
}
