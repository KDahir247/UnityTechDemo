using Tech.UI.Panel;
using Tech.Utility;
using UnityEngine;
using UnityEngine.UIElements;

public class CreationDocumentQ : MonoBehaviour
{
    private static Creation_Document _coreVisualElement;

    private void Awake()
    {
        _coreVisualElement = gameObject.GetComponent<UIDocument>().rootVisualElement?.Q<Creation_Document>();

        _coreVisualElement.Q<Button>("Assassin_Button")
            .RegisterCallback<ClickEvent>(evt => QueryStoredCharacter("Assassin"));

        _coreVisualElement.Q<Button>("Necromancer_Button")
            .RegisterCallback<ClickEvent>(evt => QueryStoredCharacter("Necromancer"));

        _coreVisualElement.Q<Button>("Oracle_Button")
            .RegisterCallback<ClickEvent>(evt => { QueryStoredCharacter("Oracle"); });
    }

    //static void to prevent Closure allocation on UI Query Callbacks
    private static void QueryStoredCharacter(in string key)
    {
        for (var i = 0; i < GlobalSetting.StoredCharacter[key].skills.Length; i++)
            _coreVisualElement
                .SetSkillTexture(GlobalSetting.StoredCharacter[key].skills[i].Image, i);
    }
}