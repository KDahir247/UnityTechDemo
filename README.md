# UnityTechDemo

Self teaching on some of unity's packages and some other plugin. (Ongoing) 
<br/>
<br/>
Project will now follow the Agile Mthodology
<br/>
![alt text](https://lh4.googleusercontent.com/nW_yFs4Hu2IfSEEokyas2_gFESgctL2nWzQEJ2Vc8dr39HsbmppAmAqHlHK2F_mtsEsn4HQEVP8EH9A0mqFDQjU2KiVPldOAIH0u343jbBwm78MLPJw1tCk3DMyy606Y8bqUDq4)
<br/>
<br/>
Current Kinematica Package Caveats:
<br/>
Can't Disable and Re-Enable GameObject with Kinematica or it will throw an assert exception on the SnapShot script and registry script
<br/>
Can't change binary reference on Kinematica. It will not update Kinematica.
<br/>
Not fully integrated with DOTS.
<br/>
Animation must be generic and can't be anything else.
<br/>
<br/>
Current Progress on Kinematica in Project. https://streamable.com/h86sl9
<br/>
<br/>

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
Device Simulator,
Cinemachine,
Hybrid Renderer,
UI Builder,
UI Toolkit

<br />

Packages using
UniTask,
UniRx,
Dotween,
ZLogger, 
ITween,
ITweenExtension,
Surge,
Castle Core,
Moq,
MagicOnion,
GRpc,
Ulid,
System libraries
Microsoft libraries,
MasterMemory,
MessagePack,and
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
<br/>
Youtube video on previous development (need to update url)
[![Watch the video](https://github.com/KDahir247/UnityTechDemo/blob/main/TechDemoProject/Assets/Images/2020.11.03-21.59.png)](https://www.youtube.com/watch?v=N7ExiAEQPE4&feature=youtu.be)


<br/>
<br/>
Scene Structure 
<br/>
Core Scene:
<br/>
MainMenu (Through Scene Addressable Loading)
<br/>
Creation (Through Scene Addressable Loading)
<br/>
Game (Game scene will be the only Scene that will have ECS and DOTS Logic to drive it) within Game Scene there will be SubScene (Through Scene Addressable Loading and SubScene with ECS, DOTS)
<br/>

<br/>
TODO:
<br/>
Cleaning, refactoring, and optimizing code.
<br/> 
Write Testing for features thus far and write test for future code structure
