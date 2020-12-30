using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public sealed class MasterMemoryWindow : EditorWindow
{
    private static string nameSpaceGenerated = "MasterData";
    private static bool returnNull = true;

    private static bool immutable;

    private void OnGUI()
    {
        {
            GUILayout.BeginArea(new Rect(Screen.width / 4.0f, 0, Screen.width / 2.0f, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            GUILayout.Label("MasterMemory Initialization", EditorStyles.boldLabel);
            GUILayout.Space(20);

            GUILayout.Label("MasterMemory Namespace");
            nameSpaceGenerated = GUILayout.TextArea(nameSpaceGenerated);

            returnNull = GUILayout.Toggle(returnNull, "Return null");

            immutable = GUILayout.Toggle(immutable, "Immutable");

            GUILayout.Space(20);
            if (GUILayout.Button("MasterMemory Generator"))
            {
                if (string.IsNullOrEmpty(nameSpaceGenerated))
                    throw new Exception("Can't Generate the MasterMemory code with an empty namespace");

                ExecuteMasterMemoryCodeGenerator(returnNull, immutable);
            }

            GUILayout.Space(5);
            if (GUILayout.Button("MessagePack Generator")) ExecuteMessagePackCodeGenerator();
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


    [MenuItem("MasterMemory/Generator")]
    private static void ShowWindow()
    {
        var window = GetWindow<MasterMemoryWindow>();
        window.titleContent = new GUIContent("MasterWindow");
        window.minSize = new Vector2(400, 400);
        window.maxSize = new Vector2(450, 450);
        window.Show();
    }


    private static void ExecuteMasterMemoryCodeGenerator(bool shouldReturnNull, bool isImmutable)
    {
        Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

        var unused = new Process();

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

        var psi = new ProcessStartInfo
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = filePath + exeFileName,
            Arguments =
                $@"-i ""{Application.dataPath}/Project/Scripts/Database/Tables"" -o ""{Application.dataPath}/Project/Scripts/Generated"" -n {nameSpaceGenerated} {(shouldReturnNull ? "-t" : "")} {(isImmutable ? "-c" : "")}"
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (sender, e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            Debug.Log($"{data}");
            Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }


    private static void ExecuteMessagePackCodeGenerator()
    {
        Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : start");

        var unused = new Process();

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

        var psi = new ProcessStartInfo
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = filePath + exeFileName,
            Arguments =
                $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/Project/Scripts/Generated/MessagePack.Generated.cs"""
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (sender, e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            Debug.Log($"{data}");
            Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }
}