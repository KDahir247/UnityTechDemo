using System;
using UnityEngine;

namespace Tech.Report
{
//string represent the asset loading and float is the percentage
    public class DataProgress : IProgress<float>
    {
        public void Report(float value)
        {
            Debug.Log(value);
        }
    }
}