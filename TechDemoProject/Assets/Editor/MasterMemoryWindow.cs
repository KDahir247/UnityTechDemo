using System;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class MasterMemoryWindow : EditorWindow
{
    private static string _nameSpaceGenerated = "MasterData";
    private static bool _returnNull = true;

    private static bool _immutable = true;

    // Add menu item
    [MenuItem("MasterMemory/Generator")]
    static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<MasterMemoryWindow>();
        window.minSize = new Vector2(400, 400);
        window.maxSize = new Vector2(450, 450);
        window.Show();
    }

    void OnGUI()
    {
        {
            GUILayout.BeginArea(new Rect(Screen.width / 4.0f, 0, Screen.width / 2.0f, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            GUILayout.Label("MasterMemory Initialization", EditorStyles.boldLabel);
            GUILayout.Space(20);

            GUILayout.Label("MasterMemory Namespace");
            _nameSpaceGenerated = GUILayout.TextArea(_nameSpaceGenerated);

            _returnNull = GUILayout.Toggle(_returnNull, "Return null");

            _immutable = GUILayout.Toggle(_immutable, "Immutable");

            GUILayout.Space(20);
            if (GUILayout.Button("MasterMemory Generator"))
            {
                if (string.IsNullOrEmpty(_nameSpaceGenerated))
                {
                    throw new Exception("Can't Generate the MasterMemory code with an empty namespace");
                }

                ExecuteMasterMemoryCodeGenerator(_returnNull, _immutable);
            }

            GUILayout.Space(5);
            if (GUILayout.Button("MessagePack Generator"))
            {
                ExecuteMessagePackCodeGenerator();
            }
            // GUILayout.Space(5);
            // if (GUILayout.Button("Build"))
            // {
            //     
            // }

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

        }
    }


    private static void ExecuteMasterMemoryCodeGenerator(bool returnNull, bool immutable)
    {
        UnityEngine.Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

        var exProcess = new Process();

        var rootPath = Application.dataPath + "/..";
        var filePath = rootPath + "/GeneratorTools/MasterMemory.Generator";
        var exeFileName = "";
#if UNITY_EDITOR_WIN
        exeFileName = "/win-x64/MasterMemory.Generator.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/osx-x64/MasterMemory.Generator";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/linux-x64/MasterMemory.Generator";
#else
        return;
#endif

        var psi = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = filePath + exeFileName,
            Arguments =
                $@"-i ""{Application.dataPath}/Scripts/Database/Tables"" -o ""{Application.dataPath}/Scripts/Generated"" -n {_nameSpaceGenerated} {(returnNull ? "-t" : "")} -c",
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (object sender, System.EventArgs e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log($"{data}");
            UnityEngine.Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }


    private static void ExecuteMessagePackCodeGenerator()
    {
        UnityEngine.Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : start");

        var exProcess = new Process();

        var rootPath = Application.dataPath + "/..";
        var filePath = rootPath + "/GeneratorTools/MessagePackUniversalCodeGenerator";
        var exeFileName = "";
#if UNITY_EDITOR_WIN
        exeFileName = "/win-x64/mpc.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/osx-x64/mpc";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/linux-x64/mpc";
#else
        return;
#endif

        var psi = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = filePath + exeFileName,
            Arguments =
                $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/Scripts/Generated/MessagePack.Generated.cs""",
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (object sender, System.EventArgs e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log($"{data}");
            UnityEngine.Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }
}
    
