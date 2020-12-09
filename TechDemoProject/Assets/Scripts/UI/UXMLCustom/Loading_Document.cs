using System;
using Tech.UI.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    //TODO look into this when reworking progressor
    public class Loading_Document : Base_Document
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private bool _isFaded;
        private Label _loadingDescription;
        private VisualElement _progress;
        private VisualElement _trackerProgress;

        public Loading_Document() : base(1000, 2000)
        {
        }

        internal PanelSettings PanelSettings { get; set; }

        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _loadingDescription = this.Q<Label>("Loading_Text");
            _trackerProgress = this.Q<VisualElement>("Tracker_Panel");
            _progress = this.Q<VisualElement>("Progress_Panel");
        }

        protected override void Start()
        {
        }

        protected override void OnDestroy()
        {
            PanelSettings.sortingOrder = 0;
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
                _progress.FadeInOrOutLoader(new Vector2(450, 50), 1000, () =>
                    _loadingDescription.FadeInOrOut(FadeOutStyle, FadeInStyle, FadeInDuration));
            }).AddTo(_disposable);
        }

        private void FadeOutLoader()
        {
            _isFaded = true;
            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
            {
                _progress.FadeInOrOutLoader(new Vector2(0, 50), FadeOutDuration, () =>
                    _loadingDescription.FadeInOrOut(FadeInStyle, FadeOutStyle, FadeInDuration, () =>
                        PanelSettings.sortingOrder = 100));
            }).AddTo(_disposable);
        }

        public void ChangeText(string loadingInfo)
        {
            if (_loadingDescription != null) _loadingDescription.text = loadingInfo;
        }

        public new class UxmlFactory : UxmlFactory<Loading_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ((Loading_Document) ve).Init();
            }
        }
    }
}