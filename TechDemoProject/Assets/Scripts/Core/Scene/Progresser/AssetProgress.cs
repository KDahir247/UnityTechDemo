using System;
using Tech.Initialization;

namespace Tech.Report
{
    public class AssetProgress : IProgress<float>
    {
        public readonly string Description;

        public AssetProgress(string description)
        {
            Description = description;
        }

        public void Report(float value)
        {
            LoadManager
                .Instance
                .progressQueue
                .Value = (Description, value);
        }
    }
}