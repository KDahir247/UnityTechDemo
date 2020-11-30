using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Linq
{
    public static class UIElementExtension
    {
        //Handle UI animation in linq style 


        public static void RecursiveFadeOutIn([NotNull] this VisualElement target,
            StyleValues fadeInStyle,
            StyleValues fadeOutStyle,
            int fadeInDuration,
            int fadeOutDuration)
        {
            target
                .experimental
                .animation
                .Start(fadeInStyle, fadeOutStyle, fadeOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(() => target
                    .experimental
                    .animation
                    .Start(fadeOutStyle, fadeInStyle, fadeInDuration)
                    .Ease(Easing.Linear)
                    .OnCompleted(() =>
                        target.RecursiveFadeOutIn(fadeInStyle, fadeOutStyle, fadeInDuration, fadeOutDuration)));
        }

        public static void FadeToNewScreen([NotNull] this VisualElement fadeOutTarget,
            [NotNull] VisualElement fadeInTarget,
            StyleValues fadeOutStyle,
            StyleValues fadeInStyle,
            int fadeOutDuration,
            int fadeInDuration,
            [CanBeNull] Action onComplete = null)
        {
            fadeOutTarget
                .experimental
                .animation
                .Start(fadeInStyle, fadeOutStyle, fadeOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(() =>
                {
                    fadeOutTarget.style.display = DisplayStyle.None;
                    fadeInTarget.style.display = DisplayStyle.Flex;

                    fadeInTarget
                        .experimental
                        .animation
                        .Start(fadeOutStyle, fadeInStyle, fadeInDuration)
                        .Ease(Easing.Linear)
                        .OnCompleted(() => onComplete?.Invoke());
                });
        }


        [NotNull]
        public static VisualElement FadeInOrOut([NotNull] this VisualElement target,
            in StyleValues fadeFromStyle,
            in StyleValues fadeToStyle,
            int fadeDuration,
            [CanBeNull] Action onComplete = null)
        {
            target
                .experimental
                .animation
                .Start(fadeFromStyle, fadeToStyle, fadeDuration)
                .Ease(Easing.Linear)
                .OnCompleted(() => onComplete?.Invoke());

            return target;
        }


        public static void FadeInOrOutLoader([NotNull] this VisualElement target,
            in Vector2 to,
            int durationMs,
            Action onComplete)
        {
            target
                .experimental
                .animation
                .Size(to, durationMs)
                .OnCompleted(onComplete);
        }
    }
}