using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Tech.BuildStep
{
    public class BuildIos
    {
        [PostProcessBuild(1)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            var projectPath = PBXProject.GetPBXProjectPath(path);
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            var TargetGUID = project.GetUnityFrameworkTargetGuid();

            project.AddFrameworkToProject(TargetGUID, "Libz.Tbd", false);

            project.SetBuildProperty(TargetGUID, "ENABLE_BITCODE", "NO");

            File.WriteAllText(projectPath, project.WriteToString());
        }

    }
}
