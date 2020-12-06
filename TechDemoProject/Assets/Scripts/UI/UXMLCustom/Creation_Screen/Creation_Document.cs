using JetBrains.Annotations;
using MasterData;
using Tech.Data;
using Tech.DB;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Unit = Tech.DB.Unit;

namespace Tech.UI.Panel
{
    public class Creation_Document : Base_Document
    {
        //Default
        private Unit _currentUnit;
        
        private readonly Button[] _skills = new Button[3];
        
        private RotationDirection _modelRotateDirection;
        private Button _createButton;
        
        private Button _assassinButton;
        private Button _necromancerButton;
        private Button _oracleButton;

        private Button _rotationLeftButton;
        private Button _rotationRightButton;
        
        private string _headScene = string.Empty;
        private string _tailScene = string.Empty;

        protected override void Init(params string[] scenes)
        {
            if (scenes == null) return;

            _headScene = scenes[0];
            _tailScene = scenes[1];
        }

        protected override void UIQuery()
        {
            
            _skills[0] = this.Q<Button>("Skill1_Button");
            _skills[1] = this.Q<Button>("Skill2_Button");
            _skills[2] = this.Q<Button>("Skill3_Button");

            _assassinButton = this.Q<Button>("Assassin_Button");
            _necromancerButton = this.Q<Button>("Necromancer_Button");
            _oracleButton = this.Q<Button>("Oracle_Button");

            _rotationLeftButton = this.Q<Button>("RotationArrowL_Button");
            _rotationRightButton = this.Q<Button>("RotationArrowR_Button");

            _createButton = this.Q<Button>("Create_Button");
        }

        protected override void Start()
        {
            _rotationRightButton.RegisterCallback(RotateModel<ClickEvent>(RotationDirection.Right));
            _rotationLeftButton.RegisterCallback(RotateModel<ClickEvent>(RotationDirection.Left));
            _rotationRightButton.RegisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));
            _rotationLeftButton.RegisterCallback(RotateModel<PointerLeaveEvent>(RotationDirection.None));

            // this.Q<Button>("Create_Button").RegisterCallback<ClickEvent>();
            _assassinButton.RegisterCallback(ChangeSkillTexture<ClickEvent>(_assassinButton.viewDataKey));
            _necromancerButton.RegisterCallback(ChangeSkillTexture<ClickEvent>(_necromancerButton.viewDataKey));
            _oracleButton.RegisterCallback(ChangeSkillTexture<ClickEvent>(_oracleButton.viewDataKey));



            //TODO got to fix 
            for (int i = 0; i < _skills.Length; i++)
            {
                //pass as 
                var index = i;
                _skills[i].RegisterCallback<ClickEvent>(e =>
                {
                    if(_currentUnit == null) return;
                    
                    MessageBroker
                        .Default
                        .Publish((_currentUnit, _currentUnit.Skills[index]));
                });
            }
        }

        protected override void OnDestroy()
        {
        }

        [NotNull]
        private EventCallback<T> ChangeSkillTexture<T>(string unitName)
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                MemoryDatabase db = TechDB.LoadDataBase(FileDestination.UnitPath);

                _currentUnit = db.UnitTable.FindByName(unitName);
                
                MessageBroker
                    .Default
                    .Publish<(Unit, Skill)>((_currentUnit, null));
                
                for (var i = 0; i < _currentUnit.Skills.Length; i++)
                {
                    var tex = new Texture2D(256, 256, TextureFormat.DXT1, false);
                    tex.LoadRawTextureData(_currentUnit.Skills[i].ImageBytes);
                    tex.Apply();
                
                    var styleBackgroundImage = _skills[i].style.backgroundImage;
                    styleBackgroundImage.value = Background.FromTexture2D(tex);
                    _skills[i].style.backgroundImage = styleBackgroundImage;
                }
            };
        }

        [NotNull]
        private EventCallback<T> RotateModel<T>(RotationDirection direction)
            where T : PointerEventBase<T>, new()
        {
            return s => MessageBroker.Default.Publish(direction);
        }


        public new class UxmlFactory : UxmlFactory<Creation_Document, UxmlTraits>
        {
            public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
            {
                return base.Create(bag, cc);
            }

            public override bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc)
            {
                return base.AcceptsAttributeBag(bag, cc);
            }
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "start-scene", defaultValue = "Assets/Scenes/Creation.unity"};

            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Assets/Scenes/Game.unity"};

            //set the _headScene 
            //and set the _tailScene
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