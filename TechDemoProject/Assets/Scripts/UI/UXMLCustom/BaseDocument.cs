using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public abstract class BaseDocument : VisualElement
    {
        public static event Action<String> OnLoadNext;

        //belong to the class
        protected static readonly CompositeDisposable Disposable = new CompositeDisposable();

        protected readonly int FadeInDuration;
        protected readonly StyleValues FadeInStyle;

        protected readonly int FadeOutDuration;
        protected readonly StyleValues FadeOutStyle;

        protected BaseDocument()
            : this(1000, 1000, new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f})
        {
        }

        protected BaseDocument(int fadeInDuration = 1000, int fadeOutDuration = 1000)
            : this(fadeInDuration, fadeOutDuration, new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f})
        {
        }

        protected BaseDocument(int fadeInDuration, int fadeOutDuration, StyleValues fadeInStyle, StyleValues fadeOutStyle)
        {
            FadeInDuration = fadeInDuration;
            FadeOutDuration = fadeOutDuration;

            FadeInStyle = fadeInStyle;
            FadeOutStyle = fadeOutStyle;

            Application.quitting += () =>
            {
                if (!Disposable.IsDisposed && Disposable.Count > 0)
                    Disposable?.Dispose();

                UnregisterCallback();
                OnDispose();
            };

            RegisterCallback<GeometryChangedEvent>(OnUIGeometryChange);
        }


        private void OnUIGeometryChange(GeometryChangedEvent evt)
        {
            UIQuery();
            RegisterCallback();

            UnregisterCallback<GeometryChangedEvent>(OnUIGeometryChange); //only need to happen once.
        }

        protected abstract void Init([CanBeNull] params string[] scenes);
        protected abstract void UIQuery();
        protected abstract void RegisterCallback();
        protected abstract void UnregisterCallback();
        
        protected virtual void OnDispose(){}

        protected void OnLoadedNextScene(string scene)
        {
            OnLoadNext?.Invoke(scene);
        }
    }
}