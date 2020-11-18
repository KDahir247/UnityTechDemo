using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tech.Data.Scriptable;
using UnityEngine.AddressableAssets;

namespace Tech.Core
{
    public static class DataAddress
    {
        public static async UniTask<bool> LoadCharacterData(CharacterData obj, IProgress<float> progress = null,
            CancellationToken cancellationToken = default
            , Action<SkillData> skillCallback = null)
        {
            try
            {
                var skillDatas = await Addressables
                    .LoadAssetsAsync(obj.labelToInclude[0].labelString, skillCallback)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);

                progress?.Report(1);

                obj.SkillRetrieved(skillDatas);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}