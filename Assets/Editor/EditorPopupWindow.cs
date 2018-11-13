using UnityEngine;
using UnityEditor;

public class EditorPopupWindow : EditorWindow {

    [MenuItem("Example/Popup Example")]
    public static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<EditorPopupWindow>();
        window.Show();
    }

    private Rect buttonRect;

    void OnGUI()
    {
        GUILayout.Label("Editor window with Popup example", EditorStyles.boldLabel);
        if (GUILayout.Button("Popup Options", GUILayout.Width(200)))
        {
            PopupWindow.Show(buttonRect, new PopupExample());
        }
        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
    }
}
