using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SceneProgress : IProgress<float>
{
    public void Report(float value)
    {
        Debug.Log(value.ToString(CultureInfo.InvariantCulture));
    }
}
