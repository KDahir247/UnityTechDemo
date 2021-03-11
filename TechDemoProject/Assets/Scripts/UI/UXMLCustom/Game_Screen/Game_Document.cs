using UnityEngine.UIElements;
using Unity.Scenes;
//Accidentally made the Notification a Label Might make it the quest button.
//Information_Button (Button)
//Store_Button (Button)
//Equipment_Button (Button)
//Summon_Button (Button)
//Adventure_Button (Button)

namespace Tech.UI.Panel
{
    public class Game_Document : BaseDocument
    {
        private Button _informationButton;
        private Button _storeButton;
        private Button _equipmentButton;
        private Button _summonButton;
        private Button _adventureButton;
        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _informationButton = this.Q<Button>("Information_Button");
            _storeButton = this.Q<Button>("Store_Button");
            _equipmentButton = this.Q<Button>("Equipment_Button");
            _summonButton = this.Q<Button>("Summon_Button");
            _adventureButton = this.Q<Button>("Adventure_Button");
        }

        protected override void RegisterCallback()
        {
            //  
        }

        protected override void UnregisterCallback()
        {
            //
        }

        public new class UxmlFactory : UxmlFactory<Game_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ((Game_Document) ve).Init();
            }
        }
    }
}