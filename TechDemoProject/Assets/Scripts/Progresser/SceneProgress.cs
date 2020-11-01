using System;
using System.Globalization;
using UnityEngine;

namespace Tech.Report
{
    public class SceneProgress : IProgress<float>
    {
        public void Report(float value)
        {
            //DO some tween for loadbar
            Debug.Log(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}