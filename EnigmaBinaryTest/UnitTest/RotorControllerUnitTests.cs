using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
//using NUnit.Common;
using Ekstrand.Encryption.Ciphers;

namespace EnigmaBinaryTest
{ 
    [TestFixture]
    public class RotorControllerUnitTests
    {
        private static RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
        private EndPointPair<byte, byte>[] m_SubitutionSet = new EndPointPair<byte, byte>[256];
        private RotorController m_rotorController = new RotorController();

        private const string ROTOR_TURN_COUNT = "Rotor and Turn Collection count do not match.";
        private const string REFLECTOR_NULL = "Reflector";
        private const string ENTRYROTOR_NULL = "EntryRotor";
        protected const string INITIALIZED_ERROR = "RotorController not initialized";

        private EndPointPair<byte, byte>[] PopulateRotor()
        {
            return m_SubitutionSet = rand.GenerateRotor();
        }

        private EndPointPair<byte, byte>[] PopulatePlugboard()
        {
            return m_SubitutionSet = rand.GeneratePlugboard();
        }

        private EndPointPair<byte, byte>[] PopulateEntryRotor()
        {
            return m_SubitutionSet = rand.GenerateEntryRotor();
        }

        #region Initialization of rotor controller

        [Test]
        [Category("RotorController")]
        public void EntryRotor()
        {
            Assert.AreEqual(null, m_rotorController.EntryRotor);
        }

        [Test]
        [Category("RotorController")]
        public void Rotors()
        {
            Assert.AreEqual(0, m_rotorController.Rotors.Count);
        }

        [Test]
        [Category("RotorController")]
        public void Reflector()
        {
            Assert.AreEqual(null, m_rotorController.Reflector);
        }

        [Test]
        [Category("RotorController")]
        public void Turns()
        {
            Assert.AreEqual(0, m_rotorController.Turns.Count);
        }

        #endregion

        #region Basic Operations Tests

        [Test]
        [Category("RotorController")]
        public void Clear()
        {
            RotorController rc = new RotorController();
            rc.Rotors.Add(new FakeRotor(PopulateRotor()));
            rc.Turns.Add(new FakeTurn());
            rc.Reflector = new FakeReflector(PopulateRotor());
            rc.EntryRotor = new FakeEntryRotor(PopulateEntryRotor());

            rc.Clear();

            Assert.AreEqual(0, rc.Rotors.Count, "Rotors not cleared");
            Assert.AreEqual(0, rc.Turns.Count, "Turns not cleared");
            Assert.AreEqual(null, rc.Reflector, "Reflector not null");
            Assert.AreEqual(null, rc.EntryRotor, "EntryRotor not null");
        }

        [Test]
        [Category("RotorController")]
        public void Reset()
        {
            RotorController rc = new RotorController();
            for (int i = 0; i < 3; i++)
            {
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.StartPosition = 5 * i;
                fr.Position = 44 + i;
                rc.Rotors.Add(fr);
            }

            Assert.AreEqual(3, rc.Rotors.Count);
            rc.Reset();
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(5 * i, rc.Rotors[i].Position, "Reset Rotors Position error");
            }
        }

        #endregion

        #region Operational Tests	

        [Test]
        [Category("RotorController")]
        public void ProcessByte()
        {
            RotorController rc = new RotorController();
            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;          
                rc.Rotors.Add(fr);         
                rc.Turns.Add(ft);
            }

            rc.Reflector = new FakeReflector(PopulateRotor());
            FakeEntryRotor fer = new FakeEntryRotor(PopulateEntryRotor());
            fer.ByteProccessedSimple = true;
            rc.EntryRotor = fer;
            rc.Initialize();


            Assert.AreEqual((byte)3, rc.ProcessByte((byte)2));
 
        }

        #endregion

        #region Exceptions throwing Tests
        [Test]
        [Category("RotorController")]
        public void Initialize_Missing_EntryRotor()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;
                rc.Rotors.Add(fr);
                rc.Turns.Add(ft);
            }

            Reflector refl = new Reflector(PopulateRotor());
            rc.Reflector = refl;

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ENTRYROTOR_NULL, ex.Message);
        }

        [Test]
        [Category("RotorController")]
        public void Initilizer_Missing_Reflector()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;
                rc.Rotors.Add(fr);
                rc.Turns.Add(ft);
            }

            EntryRotor er = new EntryRotor(PopulateRotor());
            rc.EntryRotor = er;

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(REFLECTOR_NULL, ex.Message);
        }

        [Test]
        [Category("RotorController")]
        public void Rotor_Turn_Count_Mismatch_Error()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;
                rc.Rotors.Add(fr);
                rc.Turns.Add(ft);
            }

            rc.Rotors.RemoveAt(1);

            EntryRotor er = new EntryRotor(PopulateRotor());
            rc.EntryRotor = er;
            Reflector refl = new Reflector(PopulateRotor());
            rc.Reflector = refl;

            var ex = Assert.Catch<Exception>(() => rc.Initialize());
            StringAssert.Contains(ROTOR_TURN_COUNT, ex.Message);
        }

        [Test]
        [Category("RotorController")]
        public void ProcessByte_Init_Without_Init_Call()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;
                rc.Rotors.Add(fr);
                rc.Turns.Add(ft);
            }

            rc.Rotors.RemoveAt(1);

            EntryRotor er = new EntryRotor(PopulateRotor());
            rc.EntryRotor = er;
            Reflector refl = new Reflector(PopulateRotor());
            rc.Reflector = refl;

            var ex = Assert.Catch<Exception>(() => rc.ProcessByte((byte)42));
            StringAssert.Contains(INITIALIZED_ERROR, ex.Message);
        }

        [Test]
        [Category("RotorController")]
        public void Reset_ProcessByte_Without_Init_Call()
        {
            RotorController rc = new RotorController();

            for (int i = 0; i < 3; i++)
            {
                FakeTurn ft = new FakeTurn();
                ft.NoRotor = true;
                FakeRotor fr = new FakeRotor(PopulateRotor());
                fr.Cycled = false;
                ft.Rotor = fr;
                rc.Rotors.Add(fr);
                rc.Turns.Add(ft);
            }

            rc.Rotors[1].SubstitutionSet = PopulateRotor();

            EntryRotor er = new EntryRotor(PopulateRotor());
            rc.EntryRotor = er;
            Reflector refl = new Reflector(PopulateRotor());
            rc.Reflector = refl;

            var ex = Assert.Catch<Exception>(() => rc.ProcessByte((byte)42));
            StringAssert.Contains(INITIALIZED_ERROR, ex.Message);
        }

        #endregion


    }
}
