using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class TurnUnitTests
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
        private EndPointPair<byte, byte>[] m_SubitutionSet = new EndPointPair<byte, byte>[256];
        private Turn m_Turn = new Turn();
        private const string ROTOR_NULL_ERR = "Rotor";
        private const string NEXTTURN_INUSE = "Can not change Turn while in use. You must Reset first.";
        private const string ROTOR_INUSE    = "Can not change Rotor while in use. You must Reset first.";

        private EndPointPair<byte, byte>[] PopulateSubitutionSet()
        {
            return m_SubitutionSet = rand.GenerateRotor();
        }

        #region Initialization of turn Tests

        [Test]
        [Category("Turn")]
        public void NextTurn_Default()
        {
            Assert.AreEqual(null, m_Turn.NextTurn);
        }

        [Test]
        [Category("Turn")]
        public void Rotor_Defaul()
        {
            Assert.AreEqual(null, m_Turn.Rotor);
        }

        [Test]
        [Category("Turn")]
        public void FullyCycled_Default()
        {
            Assert.AreEqual(false, m_Turn.FullyCycled);
        }

        [Test]
        [Category("Turn")]
        public void StarterRotor_Default()
        {
            Assert.AreEqual(false, m_Turn.StarterRotor);
        }

        #endregion

        #region Basic Operations Tests

        [Test]
        [Category("Turn")]
        public void Clear()
        {
            Turn t = new Turn();
            t.Rotor = new FakeRotor(PopulateSubitutionSet());
            t.NextTurn = new FakeTurn();

            Assert.AreNotEqual(null, t.Rotor);
            Assert.AreNotEqual(null, t.NextTurn);
            t.Clear();
            Assert.AreEqual(null, t.Rotor);
            Assert.AreEqual(null, t.NextTurn);
        }

        #endregion

        #region Operational Tests

        [Test]
        [Category("Turn")]
        public void FullyCycled_False()
        {
            Turn t = new Turn();
            t.Rotor = new FakeRotor(PopulateSubitutionSet());
            Assert.AreEqual(false, t.FullyCycled);
        }

        [Test]
        [Category("Turn")]
        public void FullyCycled_True()
        {
            FakeRotor rotor = new FakeRotor(PopulateSubitutionSet());
            rotor.Cycled = true;
            Turn t = new Turn();
            t.Rotor = rotor;
            Assert.AreEqual(true, t.FullyCycled);
        }

        [Test]
        [Category("Turn")]
        public void TurnRotor()
        {
            Turn t = new Turn();
            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            fr.StartPosition = 55;
            fr.Position = 123;
            fr.NextRotorNull = false;
            fr.FakeCycled = true;
            t.Rotor = fr;

            t.TurnRotor();

            Assert.AreEqual(55, t.Rotor.StartPosition);
            Assert.AreEqual(124, t.Rotor.Position);
        }

        public void StarterRotor()
        {
            Turn t = new Turn();
            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            fr.StartPosition = 55;
            fr.Position = 123;
            fr.NextRotorNull = false;
            t.Rotor = fr;
            t.StarterRotor = true;

            t.TurnRotor();

            Assert.AreEqual(55, t.Rotor.StartPosition);
            Assert.AreEqual(124, t.Rotor.Position);
        }

        [Test]
        [Category("Turn")]
        public void NextTurn_Call()
        {
            Turn t = new Turn();
     
            FakeTurn ft = new FakeTurn();
            ft.NextTurnNull = false;
            ft.FakeTurnRotor = true;
            t.NextTurn = ft;

            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            fr.FakeCycled = true;
            fr.FakeProcessByte = true;
            t.Rotor = fr;

            t.NextTurn.TurnRotor();

            Assert.AreEqual(true, ft.TurnRotorCalled);
        }

        #endregion

        #region Exception Tests

        [Test]
        [Category("Turn")]
        public void Change_Rotor()
        {
            Turn t = new Turn();
            FakeTurn ft = new FakeTurn();
            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            fr.NextRotorNull = false;
            t.Rotor = fr;
            t.NextTurn = ft;

            t.TurnRotor();

            var ex = Assert.Catch<Exception>(() => t.Rotor = new FakeRotor(PopulateSubitutionSet()));
            StringAssert.Contains(ROTOR_INUSE, ex.Message);
        }

        [Test]
        [Category("Turn")]
        public void Change_NextTurn()
        {
            Turn t = new Turn();
            FakeTurn ft = new FakeTurn();
            FakeRotor fr = new FakeRotor(PopulateSubitutionSet());
            t.Rotor = fr;
            t.NextTurn = ft;
            
            t.TurnRotor();

            var ex = Assert.Catch<Exception>(() => t.NextTurn = new FakeTurn());
            StringAssert.Contains(NEXTTURN_INUSE, ex.Message);
            
        }

        [Test]
        [Category("Turn")]
        public void Cycled_Null_Rotor()
        {
            Turn t = new Turn();

            Assert.AreEqual(false, t.FullyCycled);
        }
        #endregion
    }
}
