using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    class EndPointPairUnitTest
    {
        [Test]
        [Category("EndPointPair")]
        public void CompareToTestLessThan()
        {
            EndPointPair<byte, byte> A = new EndPointPair<byte, byte>((byte)1, (byte)0);
            EndPointPair<byte, byte> B = new EndPointPair<byte, byte>((byte)0, (byte)1);

            Assert.AreEqual(-1, B.SideA.CompareTo(A.SideA));
        }

        [Test]
        [Category("EndPointPair")]
        public void CompareToTestGreaterThan()
        {
            EndPointPair<byte, byte> A = new EndPointPair<byte, byte>((byte)1, (byte)0);
            EndPointPair<byte, byte> B = new EndPointPair<byte, byte>((byte)0, (byte)1);

            Assert.AreEqual(1, A.SideA.CompareTo(B.SideA));
        }

        [Test]
        [Category("EndPointPair")]
        public void CompareToTestEqual()
        {
            EndPointPair<byte, byte> A = new EndPointPair<byte, byte>((byte)1, (byte)0);
            EndPointPair<byte, byte> B = new EndPointPair<byte, byte>((byte)1, (byte)1);

            Assert.AreEqual(0, A.SideA.CompareTo(B.SideA));
        }
    }
}
