using System;
using Tech.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

//TODO check if it works with ECS as a component
//TODO make a base class to handle all the VisualElement
//TODO Refactor when done
public class MainMenuDocumentQ : MonoBehaviour
{
    
    [SerializeField] 
    [Tooltip("The Fade out duration of a panel. \n 1000 ms is equivalent to 1 sec")]
    private int fadeOutMs = 1000;

    [SerializeField] [Tooltip("The Fade in duration of a panel. \n 1000 ms is equivalent to 1 sec")]
    private int fadeInMs = 1000;
    
    private VisualElement _coreElement;
    
    private VisualElement _titleScreen;
    private VisualElement _optionScreen;
    private VisualElement _supportScreen;
    private VisualElement _newScreen;
    
    private Label _versionLabel;
    private Label _idLabel;

    private bool hasPressed;
    
    //TODO use a Utf8ValueStringBuilder to increase performance
    // Start is called before the first frame update
    void Awake()
    {
        _coreElement = GetComponent<UIDocument>().rootVisualElement;

        _titleScreen = _coreElement.Q<VisualElement>("MainMenu_Panel");
        _optionScreen = _coreElement.Q<VisualElement>("Option_Panel");
        _supportScreen = _coreElement.Q<VisualElement>("Support_Panel");
        _newScreen = _coreElement.Q<VisualElement>("News_Panel");
        
        
        _titleScreen?
            .Q<Button>("Option_Button")
            .RegisterCallback<ClickEvent>(EnableOptionScreen);

        _titleScreen?
            .Q<Button>("Support_Button")
            .RegisterCallback<ClickEvent>(EnableSupportScreen);
        
        _titleScreen?
            .Q<Button>("Mail_Button")
            .RegisterCallback<ClickEvent>(EnableNewScreen);
        
        //TODO not yet implemented
         // _optionScreen?
         //     .Q<Button>("back_Button")
         //     .RegisterCallback<ClickEvent>(EnableTitleScreen);
         
        // _supportScreen?
        //     .Q<Button>("back_Button")
        //     .RegisterCallback<ClickEvent>(EnableTitleScreen);
        
        _newScreen?
            .Q<Button>("back_Button")
            .RegisterCallback<ClickEvent>(evt => {EnableTitleScreen(evt, _newScreen);});
        
        //Get From event pool rather then creating an event
        //RegisterCallBack(ClickEvent.GetPooled());

        _versionLabel = _titleScreen?.Q<Label>("Version_Text");
        _idLabel = _titleScreen?.Q<Label>("ID_Text");
        if (_versionLabel != null) 
            _versionLabel.text = "Ver." + GlobalSetting<MainMenuDocumentQ>.ReactiveVersion.Value;

        //TODO Temp ID Text will be the player ID in network
        if (_idLabel != null)
            _idLabel.text = "ID. 14444";
    }


    private void OnDestroy() => UnRegisterCallBack(ClickEvent.GetPooled());
    private void OnApplicationQuit() => UnRegisterCallBack(ClickEvent.GetPooled());

    
    //TODO create an actual callback for FadeOut and FadeIn rather then calling an empty callback
    //TODO FadeIn has a snappy look
    
    /// <summary>
    /// Retrieve Option Button Callback
    /// </summary>
    /// <param name="evt">the parameter of the event type received</param>
    /// <typeparam name="T">The event type for the callback</typeparam>
    private void EnableOptionScreen<T>(T evt) where T : PointerEventBase<T>, new()
    {
        FadeOut(_titleScreen, fadeOutMs, () =>
        {
            _titleScreen.style.display = DisplayStyle.None;
            _optionScreen.style.display = DisplayStyle.Flex;
            
            FadeIn(_optionScreen, fadeInMs, () => { });
        });
    }

    /// <summary>
    /// Retrieve Support Button Callback
    /// </summary>
    /// <param name="evt">the parameter of the event type received</param>
    /// <typeparam name="T">The event type for the callback</typeparam>
    private void EnableSupportScreen<T>(T evt) where T : PointerEventBase<T>, new()
    {
        FadeOut(_titleScreen, fadeOutMs, () =>
        {
            _titleScreen.style.display = DisplayStyle.None;
            _supportScreen.style.display = DisplayStyle.Flex;
            
            FadeIn(_supportScreen, fadeInMs, () => { });
        } );
    }


    /// <summary>
    /// Retrieve News Button Callback
    /// </summary>
    /// <param name="evt">the parameter of the event type received</param>
    /// <typeparam name="T">The event type for the callback</typeparam>
    private void EnableNewScreen<T>(T evt) where T : PointerEventBase<T>, new()
    {
        FadeOut(_titleScreen, fadeOutMs, () =>
        {
            _titleScreen.style.display = DisplayStyle.None;

            _newScreen.style.display = DisplayStyle.Flex;
            FadeIn(_newScreen,fadeInMs, () => {});

        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="visualElement"></param>
    /// <typeparam name="T"></typeparam>
    private void EnableTitleScreen<T>(T evt, VisualElement visualElement) where T : PointerEventBase<T>, new()
    {
        FadeOut(visualElement, fadeOutMs, () =>
        {
            visualElement.style.display = DisplayStyle.None;

            _titleScreen.style.display = DisplayStyle.Flex;
            FadeIn(_titleScreen, fadeInMs, () => {});

        });
    }
    
    /// <summary>
    /// Retrieve Core Panel Callback
    /// </summary>
    /// <param name="evt">the parameter of the event type received</param>
    /// <typeparam name="T">The event type for the callback</typeparam>
    private void CoreCall<T>(T evt) where T : PointerEventBase<T>, new()
    {
        Debug.Log("loading next scene");
    }

    private void FadeOut(VisualElement elementToFadeOut, int durationMs, Action callback)
    {
        elementToFadeOut
            .experimental
            .animation
            .Start(new StyleValues {opacity = 1}, new StyleValues {opacity = 0}, durationMs)
            .Ease(Easing.InQuad).OnCompleted(callback);
    }
    
    
    private void FadeIn(VisualElement elementToFadeIn, int durationMs,Action callback)
    {
        elementToFadeIn
            .experimental
            .animation
            .Start(new StyleValues {opacity = 0}, new StyleValues {opacity = 1}, durationMs)
            .Ease(Easing.Linear).OnCompleted(callback);
    }

    /// <summary>
    /// De-initialize and unregister all the callbacks
    /// </summary>
    /// <param name="callback">the parameter of the event type received</param>
    /// <typeparam name="T">The event type for the callback</typeparam>
    private void UnRegisterCallBack<T>(PointerEventBase<T> callback)
        where T : PointerEventBase<T>, new()
    {
        _titleScreen.Q<Button>("Option_Button").UnregisterCallback<T>(EnableOptionScreen);
        _titleScreen.Q<Button>("Support_Button").UnregisterCallback<T>(EnableSupportScreen);
        _titleScreen.Q<Button>("Mail_Button").UnregisterCallback<T>(EnableNewScreen);
        _titleScreen.UnregisterCallback<T>(CoreCall);
    }
}
