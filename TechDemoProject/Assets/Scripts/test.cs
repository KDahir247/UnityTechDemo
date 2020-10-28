using Microsoft.Extensions.Logging;
using Tech.Core;
using UnityEngine;
using ZLogger;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private static ILogger<test> testLog = LogManager.GetLogger<test>();
    void Start()
    {
        //testing log
        testLog.ZLogInformation("Initial LogManager Test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
