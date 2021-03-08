using Cysharp.Threading.Tasks;
using Tech.DB;
using Tech.Utility;

using JetBrains.Annotations;
using Tech.UI.Linq;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    //TODO really need refactoring.....
    public class Dialogue_Document : BaseDocument
    {
        private string _headSceneName = string.Empty;
        private string _tailSceneName = string.Empty;

        private readonly DatabaseStream _dbStream = new DatabaseStream();
        private DynamicDbBuilder _dbBuilder;

        private Button _createCharacterBtn;
        private Label _dialogueText;
        private TextField _heroNameTextField;

        protected override void Init(params string[] scenes)
        {
            _dbBuilder = new DynamicDbBuilder(_dbStream);

            if(scenes == null || scenes.Length <= 1) return;

            _headSceneName = scenes[0];
            _tailSceneName = scenes[1];
        }

        protected override void UIQuery()
        {
            _dialogueText = this.Q<Label>("Dialogue_Text");

            _heroNameTextField = this.Q<TextField>("Hero_TextField");
            _createCharacterBtn = this.Q<Button>("CompleteCreation_Button");
        }

        protected override void RegisterCallback()
        {
            _dialogueText
                .PlayCollectionTextSequence(TechIO.ReadTextFile("IntroDialogue"),
                    1000,
                    1000,
                    Easing.Linear,
                    50,
                    ShowNameTextField)
                .Forget();

            _createCharacterBtn.RegisterCallback(SaveDataAndLoadNextScene<ClickEvent>());
        }

        protected override void UnregisterCallback()
        {
            _createCharacterBtn?.UnregisterCallback(SaveDataAndLoadNextScene<ClickEvent>());
        }


        [NotNull]
        private EventCallback<T> SaveDataAndLoadNextScene<T>()
        {
            return evt =>
            {
                if (_heroNameTextField.value.Length <= 3 || _heroNameTextField.value.Length > 16) return;

                _dbBuilder.DynamicallyMutateDatabase(FileDestination.UserPath, builder =>
                {
                    builder.Diff(new[]
                    {
                        new User
                        {
                            Level = 1,
                            Username =  _heroNameTextField.text,
                        }
                    });

                    return builder;
                });

                _dbBuilder.BuildToDatabaseAsync()
                    .ContinueWith((() =>
                {
                    UnregisterCallback();

                    _createCharacterBtn.style.display = DisplayStyle.None;
                    OnLoadedNextScene(_tailSceneName);
                }));
            };
        }


        private void ShowNameTextField()
        {
            _dialogueText.SwitchDisplay(_heroNameTextField);
            _createCharacterBtn.style.display = DisplayStyle.Flex;
        }


        public new class UxmlFactory : UxmlFactory<Dialogue_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "current-scene", defaultValue = "Creation"};
            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Game"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nextSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((Dialogue_Document) ve).Init(sceneName, nextSceneName);
            }
        }
    }
}