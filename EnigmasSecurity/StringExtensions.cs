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
using System.Text;


namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string that center-aligns the characters in this string by padding them with spaces on the left and right, for a specified total length. 
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="totalWidth">Integer The number of characters in the resulting string, equal to the number of original characters plus any additional 
        /// padding characters. </param>
        /// <returns>String A new string that is equivalent to this instance, but center-aligned and padded on the left and right with as many spaces as needed 
        /// to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance. 
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        public static string PadCenter(this string s, int totalWidth)
        {
            if (s.Length > totalWidth) { return s; }
            if (s.Length == totalWidth) { return s; }

            StringBuilder sb = new StringBuilder();

            int idx = 0;
            int value = totalWidth - s.Length;
            value = value / 2;

            for (int i = 0; i < totalWidth; i++)
            {
                sb.Append(" ");
                if (i >= value && idx < s.Length)
                {
                    sb[sb.Length - 1] = s[idx];
                    idx++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string that center-aligns the characters in this string by padding them with spaces on the left and right, for a specified total length using user defined character. 
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="totalWidth">Integer The number of characters in the resulting string, equal to the number of original characters plus any additional 
        /// padding characters. </param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>String A new string that is equivalent to this instance, but center-aligned and padded on the left and right with as many spaces as needed 
        /// to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance. 
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        public static string PadCenter(this string s, int totalWidth, char paddingChar)
        {
            if (s.Length > totalWidth) { return s; }
            if (s.Length == totalWidth) { return s; }

            StringBuilder sb = new StringBuilder();

            int idx = 0;
            int value = totalWidth - s.Length;
            value = value / 2;

            for (int i = 0; i < totalWidth; i++)
            {
                sb.Append(paddingChar);
                if (i >= value && idx < s.Length)
                {
                    sb[sb.Length - 1] = s[idx];
                    idx++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string that left-aligns the characters in this string by padding them with spaces on the right, for a specified total length. 
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="totalWidth">Integer The number of characters in the resulting string, equal to the number of original characters plus any additional 
        /// padding characters. </param>
        /// <returns>String A new string that is equivalent to this instance, but left-aligned and padded on the right with as many spaces as needed to create 
        /// a length of totalWidth. However, if totalWidth is less than the length of this instance, the method will crop the right end to totalWidth. 
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        public static string PadRightCrop(this string s, int totalWidth)
        {
            StringBuilder sb = new StringBuilder();
            int idx = 0;

            for (int i = 0; i < totalWidth; i++)
            {
                sb.Append(" ");
                if (idx < s.Length)
                {
                    sb[sb.Length - 1] = s[idx];
                    idx++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a new string that right-aligns the characters in this string by padding them with spaces on the right, for a specified total length.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="totalWidth">Integer The number of characters in the resulting string, equal to the number of original characters plus any additional 
        /// padding characters. </param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>String A new string that is equivalent to this instance, but left-aligned and padded on the right with as many spaces as needed to create 
        /// a length of totalWidth. However, if totalWidth is less than the length of this instance, the method will crop the right end to totalWidth. 
        /// If totalWidth is equal to the length of this instance, the method returns a new string that is identical to this instance.</returns>
        public static string PadRightCrop(this string s, int totalWidth, char paddingChar)
        {
            StringBuilder sb = new StringBuilder();
            int idx = 0;

            for (int i = 0; i < totalWidth; i++)
            {
                sb.Append(paddingChar);
                if (idx < s.Length)
                {
                    sb[sb.Length - 1] = s[idx];
                    idx++;
                }
            }

            return sb.ToString();
        }
    }
}
