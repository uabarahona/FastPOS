using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPOS.Core
{
    [Flags]
    public enum ProductDecreaseRules
    {
        DisableExceptionOnPositive = 0,
        DisableExceptionOnZero = 2,
        DisableDeleteOnZero = 4,
    }
}
