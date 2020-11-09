using System;
using System.Collections.Generic;
using System.Globalization;
using Tech.Initialization;
using UnityEngine;

namespace Tech.Report
{
    public class SceneProgress : IProgress<float>
    {
        public readonly string Description;
        public SceneProgress(string description)
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