using UnityEngine;

namespace Experimental.Network
{
    public class TestConnect : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Client.instance.ConnectToServer();
        }
    }
}