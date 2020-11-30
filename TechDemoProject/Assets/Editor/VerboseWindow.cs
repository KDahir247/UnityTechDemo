using UniRx;
using UnityEditor;
using UnityEngine;

namespace Tech.Editor
{
    //TODO fix
    public class VerboseWindow : EditorWindow
    {
        private static bool EnableUnitaskBootstrapVerbose;

        private static readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();

        [MenuItem("Tech/Verbosity")]
        private static void Init()
        {
            EditorWindow window = CreateInstance<VerboseWindow>();
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(450, 450);
            window.Show();
        }


        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width / 4.0f, 0, Screen.width / 2.0f, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();


            EnableUnitaskBootstrapVerbose = GUILayout
                .Toggle(EnableUnitaskBootstrapVerbose, "BootstrapVerbose");

            Debug.Log(EnableUnitaskBootstrapVerbose);

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }
    }
}