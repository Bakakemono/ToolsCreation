using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class ExampleWindow : EditorWindow
{
    private string MyString = "Hello My Moto";

    [MenuItem("Example/Example")]
    public static void ShowWindow()
    {
        /*EditorWindow.*/GetWindow<ExampleWindow>("Example");
    }

    public void OnGUI()
    {
        // Window Code

        // This method can display label
        GUILayout.Label("This is a Label", EditorStyles.boldLabel);

        // This method can display a text field
        MyString = EditorGUILayout.TextField("Name", MyString);

        // This one is to Setup a button
        if (GUILayout.Button("Can U press me?"))
        {
            Debug.Log("Thank you");
        }

        if (GUILayout.Button("ATTENTION!!"))
        {
            if (EditorUtility.DisplayDialog("Wait a minute...", "Are you dumb?", "Yes, I am", "Of Course"))
            {
                Debug.Log("You admit it and it's great");
            }
            else
            {
                Debug.Log("Don't try to escape from your destiny");
            }
        }
    }

}
