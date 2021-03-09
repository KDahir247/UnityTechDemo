using Cysharp.Threading.Tasks;
using Tech.DB;

using JetBrains.Annotations;
using Tech.UI.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public class Creation_Document : BaseDocument
    {
        private DB.Unit _currentUnit;

        private readonly DatabaseStream _dbStream = new DatabaseStream();
        private StaticDbBuilder _dbBuilder;

        private Button _assassinButton;
        private Button _necromancerButton;
        private Button _oracleButton;

        private readonly Button[] _skills = new Button[3];
        private Button _createButton;

        private IMessageBroker _messages = MessageBroker.Default;

        protected override void Init(params string[] scenes)
        {
            _dbBuilder = new StaticDbBuilder(_dbStream);
        }

        protected override void UIQuery()
        {
            _createButton = this.Q<Button>("Create_Button");

            _skills[0] = this.Q<Button>("Skill1_Button");
            _skills[1] = this.Q<Button>("Skill2_Button");
            _skills[2] = this.Q<Button>("Skill3_Button");

            _assassinButton = this.Q<Button>("Assassin_Button");
            _necromancerButton = this.Q<Button>("Necromancer_Button");
            _oracleButton = this.Q<Button>("Oracle_Button");
        }

        protected override void RegisterCallback()
        {
            _assassinButton.RegisterCallback(OnPressCharacter<ClickEvent>(_assassinButton.viewDataKey));
            _necromancerButton.RegisterCallback(OnPressCharacter<ClickEvent>(_necromancerButton.viewDataKey));
            _oracleButton.RegisterCallback(OnPressCharacter<ClickEvent>(_oracleButton.viewDataKey));


            _createButton.RegisterCallback(SaveUnitToUser<ClickEvent>());

            for (byte i = 0; i < _skills.Length; i++) _skills[i].RegisterCallback(ClickSkill<ClickEvent>(i));
        }

        protected override void UnregisterCallback()
        {
            _assassinButton.UnregisterCallback(OnPressCharacter<ClickEvent>(_assassinButton.viewDataKey));
            _necromancerButton.UnregisterCallback(OnPressCharacter<ClickEvent>(_necromancerButton.viewDataKey));
            _oracleButton.UnregisterCallback(OnPressCharacter<ClickEvent>(_oracleButton.viewDataKey));

            for (byte i = 0; i < _skills.Length; i++) _skills[i].UnregisterCallback(ClickSkill<ClickEvent>(i));

            _createButton.UnregisterCallback(SaveUnitToUser<ClickEvent>());
        }


        [NotNull]
        private EventCallback<T> SaveUnitToUser<T>()
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                if(_currentUnit == null) return;

                _dbBuilder.StaticallyMutateDatabase(FileDestination.UserPath,builder =>
                {
                    builder.Append(new[]
                    {
                        new User
                        {
                            PossessedUnit = new []
                            {
                                _currentUnit
                            }
                        },
                    });
                    return builder;
                });

                _dbBuilder.BuildToDatabaseAsync().Forget();
            };
        }

        [NotNull]
        private EventCallback<T> ClickSkill<T>(int index)
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                if (_currentUnit == null) return;

                _messages.Publish((_currentUnit, _currentUnit.Skills[index]));
            };
        }

        [NotNull]
        private EventCallback<T> OnPressCharacter<T>(string unitName)
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                if (_createButton.style.opacity.value <= 0)
                    _createButton
                        .FadeInOrOut(FadeOutStyle, FadeInStyle, Easing.Linear, FadeInDuration);

                ChangeSkills<T>(unitName);
            };
        }

        private void ChangeSkills<T>(string unitName) where T : PointerEventBase<T>, new()
        {

            _currentUnit = _dbStream.TryGetDatabase(FileDestination.UnitPath).UnitTable.FindByName(unitName);

            _messages.Publish<(DB.Unit, Skill)>((_currentUnit, null));

            SetSkillTexture<T>();
        }

        private void SetSkillTexture<T>() where T : PointerEventBase<T>, new()
        {
            for (byte i = 0; i < _currentUnit.Skills.Length; i++)
            {
                var tex = new Texture2D(256, 256, TextureFormat.DXT1, false);
                tex.LoadRawTextureData(_currentUnit.Skills[i].ImageBytes);
                tex.Apply();

                var styleBackgroundImage = _skills[i].style.backgroundImage;
                styleBackgroundImage.value = Background.FromTexture2D(tex);
                _skills[i].style.backgroundImage = styleBackgroundImage;
            }
        }

        public new class UxmlFactory : UxmlFactory<Creation_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ((Creation_Document) ve).Init();
                base.Init(ve, bag, cc);
            }
        }
    }
}