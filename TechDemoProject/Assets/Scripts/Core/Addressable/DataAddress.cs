using System;
using System.Collections.Generic;
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
            ,Action<SkillData> skillCallback = null)
        {
            try
            {
                IList<SkillData> skillDatas = await Addressables
                    .LoadAssetsAsync<SkillData>(obj.labelToInclude[0].labelString, skillCallback)
                    .ToUniTask(null, PlayerLoopTiming.Update, cancellationToken);

                progress?.Report(1);

                obj.SkillRetrieved(skillDatas);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}