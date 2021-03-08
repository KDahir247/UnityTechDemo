using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Linq
{
    internal static class UIElementExtension
    {
        public static void RecursiveFadeOutIn([NotNull] this VisualElement target,
            StyleValues fadeInStyle,
            StyleValues fadeOutStyle,
            Func<float, float> easing,
            int fadeInDuration,
            int fadeOutDuration)
        {
            target
                .experimental
                .animation
                .Start(fadeInStyle, fadeOutStyle, fadeOutDuration)
                .Ease(easing)
                .OnCompleted(() => target
                    .experimental
                    .animation
                    .Start(fadeOutStyle, fadeInStyle, fadeInDuration)
                    .Ease(Easing.Linear)
                    .OnCompleted(() =>
                        target.RecursiveFadeOutIn(fadeInStyle, fadeOutStyle,easing, fadeInDuration, fadeOutDuration)));
        }

        public static void FadeToNewScreen([NotNull] this VisualElement fadeOutTarget,
            [NotNull] VisualElement fadeInTarget,
            StyleValues fadeOutStyle,
            StyleValues fadeInStyle,
            Func<float,float> easing,
            int fadeOutDuration,
            int fadeInDuration,
            [CanBeNull] Action onComplete = null)
        {
            fadeOutTarget
                .experimental
                .animation
                .Start(fadeInStyle, fadeOutStyle, fadeOutDuration)
                .Ease(easing)
                .OnCompleted(() =>
                {
                    fadeOutTarget.style.display = DisplayStyle.None;
                    fadeInTarget.style.display = DisplayStyle.Flex;

                    fadeInTarget
                        .experimental
                        .animation
                        .Start(fadeOutStyle, fadeInStyle, fadeInDuration)
                        .Ease(easing)
                        .OnCompleted(() => onComplete?.Invoke());
                });
        }


        [NotNull]
        public static VisualElement FadeInOrOut([NotNull] this VisualElement target,
            in StyleValues fadeFromStyle,
            in StyleValues fadeToStyle,
            Func<float,float> easing,
            int fadeDuration,
            [CanBeNull] Action onComplete = null)
        {
            target
                .experimental
                .animation
                .Start(fadeFromStyle, fadeToStyle, fadeDuration)
                .Ease(easing)
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

        public static void SwitchDisplay([NotNull] this VisualElement outDisplay,
            [NotNull] VisualElement inDisplay)
        {
            outDisplay.style.display = DisplayStyle.None;
            inDisplay.style.display = DisplayStyle.Flex;
        }


        public static async UniTask PlayTextSequence([NotNull] this Label text,
            int delayBeforeStarting,
            int delayBeforeEnding,
            Func<float,float> easing,
            string type,
            int intervalMs)
        {
            await UniTask.Delay(delayBeforeStarting, DelayType.Realtime);

            if (!text.text.Equals(type))
            {
                for (byte i = 0; i < type.Length; i++)
                {
                    text.text += type[i].ToString();
                    if (type[i] != '\\')
                        await UniTask.Delay(intervalMs, DelayType.Realtime);
                }

                await UniTask.Delay(delayBeforeEnding, DelayType.Realtime);


                text.FadeInOrOut(new StyleValues {opacity = 1}, new StyleValues {opacity = 0},easing, 1000);

                await UniTask.WaitUntil(() => text.style.opacity.value <= 0.0f);
            }
        }

        public static async UniTask PlayCollectionTextSequence([NotNull] this Label text,
            [NotNull] string[] typeCollection,
            int delayBeforeEnding,
            int delayBeforeStarting,
            Func<float,float> easing,
            int intervalMs,
            [CanBeNull] Action onComplete = null)
        {
            foreach (var s in typeCollection)
            {
                if (text.style.opacity.value <= 0)
                {
                    var styleOpacity = text.style.opacity;
                    styleOpacity.value = 1;
                    text.style.opacity = styleOpacity;

                    text.text = string.Empty;
                }


                await PlayTextSequence(text, delayBeforeStarting, delayBeforeEnding,easing, s, intervalMs);
            }


            onComplete?.Invoke();
        }
    }
}