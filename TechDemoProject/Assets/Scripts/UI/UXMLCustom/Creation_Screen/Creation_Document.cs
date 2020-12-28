using JetBrains.Annotations;
using MasterData;
using Tech.Data;
using Tech.DB;
using Tech.UI.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Unit = Tech.DB.Unit;

namespace Tech.UI.Panel
{
    public class Creation_Document : Base_Document
    {
        private readonly Button[] _skills = new Button[3];

        private Button _assassinButton;

        private Button _createButton;

        //Default
        private Unit _currentUnit;
        private MemoryDatabase _db;
        private readonly TechStaticDBBuilder _dbBuilder = new TechStaticDBBuilder();

        private string _headScene = string.Empty;

        private RotationDirection _modelRotateDirection;
        private Button _necromancerButton;
        private Button _oracleButton;

        private Button _rotationLeftButton;
        private Button _rotationRightButton;
        private string _tailScene = string.Empty;

        protected override void Init(params string[] scenes)
        {
            if (scenes == null) return;

            _headScene = scenes[0];
            _tailScene = scenes[1];
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

            _rotationLeftButton = this.Q<Button>("RotationArrowL_Button");
            _rotationRightButton = this.Q<Button>("RotationArrowR_Button");
        }

        protected override void Start()
        {
            _rotationRightButton
                .RegisterCallback(RotateModel<ClickEvent>(RotationDirection.Right));
            _rotationLeftButton
                .RegisterCallback(RotateModel<ClickEvent>(RotationDirection.Left));
            _rotationRightButton
                .RegisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));
            _rotationLeftButton
                .RegisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));

            _assassinButton.RegisterCallback(OnPressCharacter<ClickEvent>(_assassinButton.viewDataKey));
            _necromancerButton.RegisterCallback(OnPressCharacter<ClickEvent>(_necromancerButton.viewDataKey));
            _oracleButton.RegisterCallback(OnPressCharacter<ClickEvent>(_oracleButton.viewDataKey));
            
            
            _createButton.RegisterCallback(SaveUnitToUser<ClickEvent>());

            for (byte i = 0; i < _skills.Length; i++) _skills[i].RegisterCallback(ClickSkill<ClickEvent>(i));
        }

        protected override void OnDestroy()
        {
            _rotationRightButton
                .UnregisterCallback(RotateModel<ClickEvent>(RotationDirection.Right));
            _rotationLeftButton
                .UnregisterCallback(RotateModel<ClickEvent>(RotationDirection.Left));
            _rotationRightButton
                .UnregisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));
            _rotationLeftButton
                .UnregisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));


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
                
                _dbBuilder.Build(builder =>
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
                }, FileDestination.UserPath);
            };
        }
        
        [NotNull]
        private EventCallback<T> ClickSkill<T>(int index)
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                if (_currentUnit == null) return;

                MessageBroker
                    .Default
                    .Publish((_currentUnit, _currentUnit.Skills[index]));
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
                        .FadeInOrOut(FadeOutStyle, FadeInStyle, FadeInDuration);

                ChangeSkills<T>(unitName);
            };
        }

        private void ChangeSkills<T>(string unitName) where T : PointerEventBase<T>, new()
        {
            _db = TechDB.LoadDataBase(FileDestination.UnitPath);

            _currentUnit = _db.UnitTable.FindByName(unitName);

            MessageBroker
                .Default
                .Publish<(Unit, Skill)>((_currentUnit, null));

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

        [NotNull]
        private EventCallback<T> RotateModel<T>(RotationDirection direction)
            where T : PointerEventBase<T>, new()
        {
            return s => MessageBroker.Default.Publish(direction);
        }


        public new class UxmlFactory : UxmlFactory<Creation_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "start-scene", defaultValue = "Assets/Scenes/Creation.unity"};

            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Assets/Scenes/Game.unity"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nextSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((Creation_Document) ve).Init(sceneName, nextSceneName);
            }
        }
    }
}