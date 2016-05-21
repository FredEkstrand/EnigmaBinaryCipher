/*
This interface was taken from "The Bouncy Castle C# Cryptographic API" https://www.bouncycastle.org/

*/
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


namespace Ekstrand.Encryption.Ciphers
{
    /// <summary>The interface stream ciphers conform to.</summary>
    public interface IStreamCipher
    {
        /// <summary>The name of the algorithm this cipher implements.</summary>
        string AlgorithmName { get; }

        /// <summary>Initialize the cipher.</summary>
        /// <param name="forEncryption">If true the cipher is initialized for encryption,
        /// if false for decryption.</param>
        /// <param name="parameters">The key and other data required by the cipher.</param>
        /// <exception cref="ArgumentException">
        /// If the parameters argument is inappropriate.
        /// </exception>
        void Init(bool forEncryption, ICipherParameters parameters);

        /// <summary>encrypt/decrypt a single byte returning the result.</summary>
        /// <param name="input">the byte to be processed.</param>
        /// <returns>the result of processing the input byte.</returns>
        byte ReturnByte(byte input);

        /// <summary>
        /// Process a block of bytes from <c>input</c> putting the result into <c>output</c>.
        /// </summary>
        /// <param name="input">The input byte array.</param>
        /// <param name="inOff">
        /// The offset into <c>input</c> where the data to be processed starts.
        /// </param>
        /// <param name="length">The number of bytes to be processed.</param>
        /// <param name="output">The output buffer the processed bytes go into.</param>
        /// <param name="outOff">
        /// The offset into <c>output</c> the processed data starts at.
        /// </param>
        /// <exception cref="DataLengthException">If the output buffer is too small.</exception>
        void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

        /// <summary>
        /// Reset the cipher to the same state as it was after the last init (if there was one).
        /// </summary>
        void Reset();
    }
}
