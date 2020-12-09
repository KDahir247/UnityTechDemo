using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public abstract class Base_Document : VisualElement
    {
        //belong to the class
        protected static readonly CompositeDisposable Disposable = new CompositeDisposable();
        protected readonly int FadeInDuration;

        protected readonly StyleValues FadeInStyle;
        protected readonly int FadeOutDuration;

        protected readonly StyleValues FadeOutStyle;

        protected Base_Document() : this(1000, 1000, new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f})
        {
        }

        protected Base_Document(int fadeInDuration = 1000, int fadeOutDuration = 1000) : this(fadeInDuration,
            fadeOutDuration, new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f})
        {
        }

        protected Base_Document(int fadeInDuration, int fadeOutDuration, StyleValues fadeInStyle,
            StyleValues fadeOutStyle)
        {
            FadeInDuration = fadeInDuration;
            FadeOutDuration = fadeOutDuration;

            FadeInStyle = fadeInStyle;
            FadeOutStyle = fadeOutStyle;

            Application.quitting += () =>
            {
                if (!Disposable.IsDisposed && Disposable.Count > 0)
                    Disposable?.Dispose();

                OnDestroy();
            };

            RegisterCallback<GeometryChangedEvent>(OnUIGeometryChange);
        }


        private void OnUIGeometryChange(GeometryChangedEvent evt)
        {
            UIQuery();
            Start();
        }

        protected abstract void Init([CanBeNull] params string[] scenes);
        protected abstract void UIQuery();
        protected abstract void Start();
        protected abstract void OnDestroy();
    }
}