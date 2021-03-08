using Pixelplacement;
using Tech.Core;
using Tech.UI.Panel;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using ZLogger;

//TODO got to fix or remove
namespace Tech.Initialization
{
//Handles show the progress bar and the loading percentage of asset that are loading.
//Need to be persistent, since some asset are loaded dynamically and not when the game start running 
    [RequireComponent(typeof(UIDocument))]
    public class LoadManager : Singleton<LoadManager>
    {
        private Loading_Document _coreUxmlDocument;
        private UIDocument _uiDocument;

        [FormerlySerializedAs("_progressQueue")]
        public ReactiveProperty<(string, float)> progressQueue = new ReactiveProperty<(string, float)>();

        private void Awake()
        {
            _uiDocument = gameObject.GetComponent<UIDocument>();
            _coreUxmlDocument = _uiDocument.rootVisualElement?.Q<Loading_Document>();

            if (_coreUxmlDocument != null)
                _coreUxmlDocument.PanelSettings = _uiDocument.panelSettings;
            else
                LogManager.Logger.ZLogError(
                    "Failed to Get Loading_Document Visual Element script and Element from the UI Document");
        }

        private void Start()
        {
            progressQueue.ObserveEveryValueChanged(cond => cond.Value.Item2).Select(_ => progressQueue.Value).Subscribe(
                val => UpdateProgress(val.Item1, val.Item2)).AddTo(this);
        }

        private void UpdateProgress(string loadingInfo, float percentage)
        {
            _coreUxmlDocument.ChangeText(loadingInfo);
            _coreUxmlDocument.ChangeSlider(percentage);
        }
    }
}