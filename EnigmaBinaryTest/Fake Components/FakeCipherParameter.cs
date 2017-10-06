using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{
    [ExcludeFromCoverageAttribute]
    public class FakeCipherParameter : ICipherParameters
    {
        public FakeCipherParameter()
        {

        }
    }
}
