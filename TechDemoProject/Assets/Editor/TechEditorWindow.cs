using Tech.Utility;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Tech.Editor
{
    //TODO fix
    public class TechEditorWindow : EditorWindow
    {
        //Verbosity 
        private static bool _enableUnitaskBootstrapVerbose;
        private static bool _enableStateVerbose;

        private static readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();


        //File IO
        private readonly string[] _dataPath =
        {
            "skill-data",
            "character-data",
            "equip-data",
            "item-data",
            "mat-data"
        };

        [MenuItem("Tech/Verbosity")]
        private static void Init()
        {
            EditorWindow window = CreateInstance<TechEditorWindow>();
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(450, 450);
            window.Show();
        }


        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width / 4.0f, 0, Screen.width / 2.0f, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();


            GUILayout.Label("Verbosity Setting");

            _enableUnitaskBootstrapVerbose = GUILayout
                .Toggle(_enableUnitaskBootstrapVerbose, "Enable BootstrapVerbose");

            _enableStateVerbose = GUILayout
                .Toggle(_enableStateVerbose, "Enable StateVerbose");

            GUILayout.Space(30);

            _dataPath[0] = GUILayout.TextArea(_dataPath[0]);
            _dataPath[1] = GUILayout.TextArea(_dataPath[1]);
            _dataPath[2] = GUILayout.TextArea(_dataPath[2]);
            _dataPath[3] = GUILayout.TextArea(_dataPath[3]);
            _dataPath[4] = GUILayout.TextArea(_dataPath[4]);


            if (GUILayout.Button("Initialize"))
            {
                Debug.Log("Changing Global Settings");


                foreach (var path in _dataPath)
                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.Log("Failed to change Global Settings due to an empty string");
                        return;
                    }

                GlobalSetting.SkillDataPath = _dataPath[0];
                GlobalSetting.CharacterDataPath = _dataPath[1];
                GlobalSetting.EquipmentDataPath = _dataPath[2];
                GlobalSetting.ItemDataPath = _dataPath[3];
                GlobalSetting.MaterialDataPath = _dataPath[4];


                GlobalSetting.EnableVerbosityUnitaskBootstrap = _enableUnitaskBootstrapVerbose;
                GlobalSetting.EnableVerbosityState = _enableStateVerbose;
            }

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }
    }
}