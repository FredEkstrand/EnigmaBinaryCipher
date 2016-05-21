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
    public class RandomGeneratorTests
    {
        [Test]
        [Category("Random Generated Substitution Set")]
        public void RotorRNG()
        {
            RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>();
            items.AddRange(rand.GenerateRotor());

            Assert.AreEqual(false, CheckForDuplicatEntries(items), ErrorMessage);
        }

        [Test]
        [Category("Random Generated Substitution Set")]
        public void EntryRotorRNG()
        {
            RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>();
            items.AddRange(rand.GenerateEntryRotor());

            Assert.AreEqual(false, CheckForDuplicatEntries(items), ErrorMessage);
        }

        [Test]
        [Category("Random Generated Substitution Set")]
        public void ReflectorRNG()
        {
            RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>();
            items.AddRange(rand.GenerateReflector());

            Assert.AreEqual(false, CheckForDuplicatEntries(items), ErrorMessage);
        }

        [Test]
        [Category("Random Generated Substitution Set")]
        public void PlugboardRNG()
        {
            RandomEndPointsGenerator rand = new RandomEndPointsGenerator();
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>();
            items.AddRange(rand.GeneratePlugboard());

            Assert.AreEqual(false, CheckForDuplicatEntries(items), ErrorMessage);
        }

        #region Common Methods used for testing

        public static bool CheckForDuplicatEntries(List<EndPointPair<byte, byte>> items)
        {
            bool success = false;
            ErrorMessage = string.Empty;    // clear out old error messages if any.

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (j != i)
                    {
                        if (items[i].SideA == items[j].SideA)
                        {
                            ErrorMessage += "Match found on SideA for: " + items[i].SideA + " at index: " + j + "\n";
                            success = true;
                        }

                        if (items[i].SideB == items[j].SideB)
                        {
                            ErrorMessage += "Match found on SideB for: " + items[i].SideB + " at index: " + j + "\n";
                            success = true;
                        }
                    }
                }
            }

            Assert.AreEqual(false, success, ErrorMessage);
            return success;
        }

        public static string ErrorMessage
        {
            get; set;
        }

        #endregion
    }
}
