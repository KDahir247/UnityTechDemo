using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using Tech.Core;
using Tech.DB;
using Tech.UI.Linq;
using Tech.Utility;
using UniRx;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Dialogue_Document : Base_Document
    {
        //private readonly TechDynamicDBBuilder _dbBuilder = new TechDynamicDBBuilder();
        
        private Button _createCharacterBtn;
        private Label _dialogueText;
        private TextField _heroNameTextField;

        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _dialogueText = this.Q<Label>("Dialogue_Text");

            _heroNameTextField = this.Q<TextField>("Hero_TextField");

            _createCharacterBtn = this.Q<Button>("CompleteCreation_Button");
        }

        protected override void Start()
        {
            this
                .ObserveEveryValueChanged(val =>
                    parent.style.opacity.value >= 1.0f && parent.style.display == DisplayStyle.Flex)
                .Take(1)
                .Subscribe(_ =>
                    _dialogueText
                        .PlayCollectionTextSequence(TechIO.ReadTextFile("IntroDialogue"),
                            1000,
                            1000,
                            50,
                            ShowNameTextField)
                        .Forget())
                .AddTo(Disposable);

           // _createCharacterBtn.RegisterCallback(SaveDataAndLoadNextScene<ClickEvent>());
        }

        protected override void OnDestroy()
        {
           // _createCharacterBtn?.UnregisterCallback(SaveDataAndLoadNextScene<ClickEvent>());
        }


        /*[NotNull]
        private EventCallback<T> SaveDataAndLoadNextScene<T>()
        {
            return evt =>
            {
                if (_heroNameTextField.value.Length <= 3 || _heroNameTextField.value.Length > 16) return;

                if (_dbBuilder.TryLoadDatabase(FileDestination.UserPath,out ImmutableBuilder immutableBuilder))
                {

                    User user = TechDB
                        .LoadDataBase(FileDestination.UserPath).
                        UserTable
                        .FindByLevel(0); //Retrieve the UserData by finding the primary key (Level)

                    immutableBuilder.Diff(new[]
                    {
                        new User{ 
                            Level = 0, 
                            Username = _heroNameTextField.text,
                            PossessedUnit = user.PossessedUnit
                        }, 
                    });
                    
                    _dbBuilder.Build(immutableBuilder);
                }   
                _createCharacterBtn.style.display = DisplayStyle.None;
                
                StateSingleton.Instance.Next();
            };
        }*/


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
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((Dialogue_Document) ve).Init();
            }
        }
    }
}