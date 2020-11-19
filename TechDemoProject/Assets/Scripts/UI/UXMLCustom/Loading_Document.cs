using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public class Loading_Document : VisualElement
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isFaded;
        private Label _loadingDescription;
        private VisualElement _progress;
        private VisualElement _trackerProgress;

        public Loading_Document()
        {
            Application.quitting += () =>
            {
                PanelSettings.sortingOrder = 0;

                if (!_disposable.IsDisposed)
                    _disposable.Dispose();
            };

            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public PanelSettings PanelSettings { get; set; }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            _loadingDescription = this.Q<Label>("Loading_Text");
            _trackerProgress = this.Q<VisualElement>("Tracker_Panel");
            _progress = this.Q<VisualElement>("Progress_Panel");
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }


        public void ChangeSlider(float loadingProgress)
        {
            if (_trackerProgress == null || _progress == null) return;

            _trackerProgress.style.width = 450.0f * loadingProgress;

            if (Math.Abs(loadingProgress - 1) <= 0)
            {
                _loadingDescription.text = "Loaded status: Complete";

                FadeOutLoader();
            }
            else if (_isFaded)
            {
                FadeInLoader();
            }
        }

        private void FadeInLoader()
        {
            PanelSettings.sortingOrder = 0;

            _isFaded = false;
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                _progress.experimental.animation
                    .Size(new Vector2(450, 50), 1000).OnCompleted(() =>
                    {
                        _loadingDescription.experimental.animation
                            .Start(new StyleValues {opacity = 0}, new StyleValues {opacity = 1}, 1000);
                    });
            }).AddTo(_disposable);
        }

        private void FadeOutLoader()
        {
            _isFaded = true;
            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
            {
                _progress.experimental.animation
                    .Size(new Vector2(0, 50), 2000).OnCompleted(() =>
                    {
                        _loadingDescription.experimental.animation
                            .Start(new StyleValues {opacity = 1}, new StyleValues {opacity = 0}, 1000);
                    }).onAnimationCompleted += () => { PanelSettings.sortingOrder = 100; };
            }).AddTo(_disposable);
        }

        public void ChangeText(string loadingInfo)
        {
            if (_loadingDescription != null) _loadingDescription.text = loadingInfo;
        }

        public new class UxmlFactory : UxmlFactory<Loading_Document, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}