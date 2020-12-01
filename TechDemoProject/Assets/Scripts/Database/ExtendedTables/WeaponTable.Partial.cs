using System.Linq;
using System.Runtime.CompilerServices;

namespace MasterData.Tables
{
    public sealed partial class WeaponTable
    {
        private int test;
        partial void OnAfterConstruct()
        {
            Unsafe.AsRef(test) = All.Select(x => x.Index).Min();
        }
    }
}