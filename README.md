# UnityTechDemo
Self teaching on some of unity's packages and some other plugin.

Unity Packages using:
Addressables,
Burst,
Cinemachine,
Collection,
Entities,
Jobs,
Kinematica,
Mathametics, 
unity physics, havok physics, and
Shadergraph,
Universal RP,
Device Simulator
<br />

Packages using
UniTask,
UniRx,
Dotween,
ZLogger, and
ZString
<br />
<br />
rebuild the kinematica asset before playing whisch is located in. Assets->KinematicaAsset by opening the Kinematica asset builder and pressing build button on the top left.
<br />
<br />
Scene and Assets are loaded via by addressable thus requiring them to be release by the addressable. Releasing the asset will also remove it from the scene and free up space.
Asset not loaded by addressable should be destroyed and not released by the addressable.
<br />
LogManager.cs will store all Editor log in a Log file under Asset->Log by date to file time. LogHelper will remove any Editorlog file in Asset->Log that are 10 days old.
<br />
<br />
Console will be written in the Editor Log if it uses type Microsoft.Extensions.Logging.ILogger as Logging
<br />
ex.        private static readonly Microsoft.Extensions.Logging.ILogger Logger = LogManager.GetLogger("SceneLogger");
ex.        private readonly Microsoft.Extensions.Logging.ILogger Logger = LogManager.GetLogger("SceneLogger"); (if static class)
<br /> or  private static readonly Microsoft.Extensions.Logging.ILogger Loggers = LogManager.GetLogger<SceneLogger>(); (if not static class)

<br />
 Logger.ZLogError("Print Error in Console and store it in Editor Log");
 <br />
                 Logger.ZLogInformation("Print info in Console and store it in Editor Log");
                 
<br/>                 
Editor Log can take two format type Exception Format and Prefix Format
<br/>

Menu Layout
<br/>
<img src="https://github.com/KDahir247/UnityTechDemo/blob/main/TechDemoProject/Assets/Images/MenuGifReal.gif" width="500" height="300">


