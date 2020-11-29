using System;
using Tech.Initialization;

namespace Tech.Report
{
//string represent the asset loading and float is the percentage
    public class DataProgress : IProgress<float>
    {
        public readonly string Description;

        public DataProgress(string description)
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