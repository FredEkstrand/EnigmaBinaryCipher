/*
	Software Developer: Fred Ekstrand 
    Copyright (C) 2016 by: Fred Ekstrand


    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
	to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE SOFTWARE DEVLOPER BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
	IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

    Except as contained in this notice, the name of the software developer shall not be used in advertising or otherwise to promote the sale, 
	use or other dealings in this "Software" without prior written authorization from the software developer.
 */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("EnigmaBinaryTest")]
namespace Ekstrand.Encryption.Ciphers
{
    /// <summary>
    /// Randomly generated set of EntryRotor, Rotor, Reflector, and Plugboard end points pairs.
    /// </summary> 
    [Serializable] 
    internal class RandomEndPointsGenerator
	{
        private Random rand = new Random();

        /// <summary>
        /// Transposition pair strength level.
        /// </summary>
        public enum TranspositionLevel
        {
            /// <summary>
            /// Lowest transposition combinations
            /// </summary>
            LOW = 240,      // 1.238591E+259 combinations.            
            /// <summary>
            /// Medium transposition combinations. 
            /// </summary>
            MEDIUM = 248,   // 2.286387E+259 combinations.             
            /// <summary>
            /// Highest transposition combinations
            /// </summary>
            HIGH = 256      // 2.303121E+259 combinations. 
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomEndPointsGenerator"/> class.
        /// </summary>
        public RandomEndPointsGenerator()
		{
			
		}

        /// <summary>
        /// Generates EndPointPairs for plugboard.
        /// </summary>
        /// <returns></returns>
        public EndPointPair<byte,byte>[] GeneratePlugboard(TranspositionLevel level = TranspositionLevel.HIGH)
		{         
            return GeneratePlugboardSet((int)level);
		}


        /// <summary>
        /// Generates EndPointPairs for rotor.
        /// </summary>
        /// <returns></returns>
        public EndPointPair<byte,byte>[] GenerateRotor()
		{
            return GenerateRotorSet(256);

        }

        /// <summary>
        /// Generates EndPointPairs for reflector.
        /// </summary>
        /// <returns></returns>
        public EndPointPair<byte, byte>[] GenerateReflector()
        {
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];
            // 90 degree offset
            int innerPos = (int)(90 / 1.40625);

            for (int i = 0; i < (256-innerPos); i++)
            {
                if (items[i] == null)
                {
                    items[i] = new EndPointPair<byte, byte>((byte)i, (byte)((innerPos + i) % 360));
                    items[i + innerPos] = new EndPointPair<byte, byte>((byte)((innerPos + i) % 360), (byte)i);
                }
            }
            return items;
            
		}

        /// <summary>
        /// Generates EndPointsPairs for Entry Rotor.
        /// </summary>
        /// <returns></returns>
        public EndPointPair<byte,byte>[] GenerateEntryRotor()
		{
            EndPointPair<byte, byte>[] items = new EndPointPair<byte, byte>[256];

            for (int i = 0; i < 256; i++)
            {
                items[i] = new EndPointPair<byte, byte>((byte)i, (byte)i);
            }
            return items;
		}

        /// <summary>
        /// Generate a set of random EndPointPair
        /// </summary>
        /// <param name="size">int The number of end points to be generated</param>
        /// <returns>Returns an array of EndPointPairs</returns>
        private EndPointPair<byte, byte>[] GenerateRotorSet(int size)
        {
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>(256);
            EndPointPair<byte, byte> set1;
            EndPointPair<byte, byte> set2;

            byte[] sideA = new byte[1];
            byte[] sideB = new byte[1];
            bool success = false;
            bool foundA = false;
            bool foundB = false;

            for (int i = 0; i < size / 2; i++)
            {
                set1 = new EndPointPair<byte, byte>();
                set2 = new EndPointPair<byte, byte>();

                while (!success)
                {
                    if (!foundA)
                    {
                        sideA[0] = (byte)rand.Next(0, 256);
                    }

                    if (!foundB)
                    {
                        sideB[0] = (byte)rand.Next(0, 256);
                    }

                    if (sideA[0] == sideB[0])
                    {
                        continue;
                    }

                    if (items.Count == 0 && sideA[0] != sideB[0])
                    {
                        set1.SideA = sideA[0];
                        set1.SideB = sideB[0];
                        set2.SideA = sideB[0];
                        set2.SideB = sideA[0];
                        foundB = foundA = true;
                    }

                    for (int k = 0; k < items.Count; k++)
                    {
                        if (items[k].SideA == sideA[0])
                        {
                            foundA = false;
                            break;
                        }

                        if (items[k].SideB == sideB[0])
                        {
                            foundB = false;
                            break;
                        }

                        set1.SideA = sideA[0];
                        set1.SideB = sideB[0];
                        set2.SideA = sideB[0];
                        set2.SideB = sideA[0];
                        foundA = foundB = true;

                    }

                    if (foundA == true && foundB == true)
                    {
                        items.Add(set1);
                        items.Add(set2);
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                success = false;
                foundA = foundB = false;
            }

            items.Sort(delegate (EndPointPair<byte, byte> p1, EndPointPair<byte, byte> p2)
                        {
                            return p1.SideA.CompareTo(p2.SideA);
                        }
                       );

            return items.ToArray();
        }
    


        /// <summary>
        /// Generate a set of random EndPointPair
        /// </summary>
        /// <param name="size">int The number of end points to be generated</param>
        /// <returns>Returns an array of EndPointPairs</returns>
        private EndPointPair<byte, byte>[] GeneratePlugboardSet(int size)
        {
            List<EndPointPair<byte, byte>> items = new List<EndPointPair<byte, byte>>(256);
            EndPointPair<byte, byte> set1;
            EndPointPair<byte, byte> set2;

            byte[] sideA = new byte[1];
            byte[] sideB = new byte[1];
            bool success = false;
            bool foundA = false;
            bool foundB = false;

            for (int i = 0; i < 256 / 2; i++)
            {
                set1 = new EndPointPair<byte, byte>();
                set2 = new EndPointPair<byte, byte>();

                while (!success)
                {
                    if (!foundA)
                    {
                        sideA[0] = (byte)rand.Next(0, 256);
                    }

                    if (!foundB)
                    {
                        sideB[0] = (byte)rand.Next(0, 256);
                    }

                    if (sideA[0] == sideB[0])
                    {
                        continue;
                    }

                    if (items.Count == 0 && sideA[0] != sideB[0])
                    {
                        set1.SideA = sideA[0];
                        set1.SideB = sideB[0];
                        set2.SideA = sideB[0];
                        set2.SideB = sideA[0];
                        foundB = foundA = true;
                    }

                    for (int k = 0; k < items.Count; k++)
                    {
                        if (items[k].SideA == sideA[0])
                        {
                            foundA = false;
                            break;
                        }

                        if (items[k].SideB == sideB[0])
                        {
                            foundB = false;
                            break;
                        }

                        set1.SideA = sideA[0];
                        set1.SideB = sideB[0];
                        set2.SideA = sideB[0];
                        set2.SideB = sideA[0];
                        foundA = foundB = true;

                    }

                    if (foundA == true && foundB == true)
                    {
                        items.Add(set1);
                        items.Add(set2);
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                success = false;
                foundA = foundB = false;
            }

            if ((256 - size) != 0)
            {
                RemovePairs(items, (256 - size));
            }
            
            items.Sort(delegate (EndPointPair<byte, byte> p1, EndPointPair<byte, byte> p2)
                        {
                            return p1.SideA.CompareTo(p2.SideA);
                        }
                       );

            return items.ToArray();
        }

        private void RemovePairs(List<EndPointPair<byte,byte>> items, int removeNumber)
        {
            EndPointPair<byte, byte> epp;
            int pos;
            for(int i = 0; i < removeNumber/2; i++)
            {
                pos = rand.Next(0, items.Count);
                epp = items[pos];
                items.RemoveAt(pos);

                for(int k = 0; k < items.Count; k++)
                {
                    if(items[k].SideB == epp.SideA)
                    {
                        items.RemoveAt(k);
                        break;
                    }
                }
            }
        }
    }
}
