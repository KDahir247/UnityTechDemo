using System;
using UnityEngine;

namespace Tech.Report
{
    public class AssetProgress : IProgress<float>
    {
        public void Report(float value)
        {
            Debug.Log(value);
        }
    }
}