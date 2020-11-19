using System.Runtime.InteropServices;

namespace Tech.Network
{
    public static class InternConnection
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnectedToInternet()
        {
            return InternetGetConnectedState(out _, 0);
        }
    }
}