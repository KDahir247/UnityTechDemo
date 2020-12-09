using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Tech.Utility
{
    internal static class TechIO
    {
        [NotNull]
        public static async UniTask<string[]> ReadTextFileAsync(string fileName, int lines = 0)
        {
            var messageReceived = new List<string>(lines);
            using (var streamReader = File.OpenText($"{Application.dataPath}/Resources/Dialogue/{fileName}.txt"))
            {
                string readLine;

                while ((readLine = await streamReader.ReadLineAsync()) != null) messageReceived.Add(readLine);
            }

            return messageReceived.ToArray();
        }

        [NotNull]
        public static string[] ReadTextFile(string fileName, int lines = 0)
        {
            var messageReceived = new List<string>(lines);
            using (var streamReader = File.OpenText($"{Application.dataPath}/Resources/Dialogue/{fileName}.txt"))
            {
                string readLine;

                while ((readLine = streamReader.ReadLine()) != null) messageReceived.Add(readLine);

                return messageReceived.ToArray();
            }
        }
    }
}