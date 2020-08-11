using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPOS.Core
{
    [Flags]
    public enum ProductAddRules
    {
        DisableExceptionOnNegative = 0,
        DisableExceptionOnZero = 2,
    }
}
