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


Packages using
UniTask,
UniRx,asdas
Dotween,
ZLogger, and
ZString

rebuild the kinematica asset before playing whisch is located in. Assets->KinematicaAsset by opening the Kinematica asset builder and pressing build button on the top left.
Scene and Assets are loaded via by addressable thus requiring them to be release by the addressable. Releasing the asset will also remove it from the scene and free up space.
Asset not loaded by addressable should be destroyed and not released by the addressable.

LogManager.cs will store all Editor log in a Log file under Asset->Log by date to file time. LogHelper will remove any Editorlog file in Asset->Log that are 10 days old.
