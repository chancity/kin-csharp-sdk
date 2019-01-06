using System;
using System.Runtime.InteropServices;
using System.Text;
using Kin.Backup.LibSodium.Interop;

namespace Kin.Backup.LibSodium
{
    /// <summary>Various utility methods.</summary>
    internal static class Utilities
    {
        /// <summary>Takes a byte array and returns a hex-encoded string.</summary>
        /// <param name="data">Data to be encoded.</param>
        /// <returns>Hex-encoded string, lodercase.</returns>
        /// <exception cref="OverflowException"></exception>
        public static string BinaryToHex(this byte[] data)
        {
            byte[] hex = new byte[data.Length * 2 + 1];
            IntPtr ret = SodiumLibrary.sodium_bin2hex(hex, hex.Length, data, data.Length);

            if (ret == IntPtr.Zero)
            {
                throw new OverflowException("Internal error, encoding failed.");
            }

            return Marshal.PtrToStringAnsi(ret);
        }
        /// <summary>Converts a hex-encoded string to a byte array.</summary>
        /// <param name="hex">Hex-encoded data.</param>
        /// <returns>A byte array of the decoded string.</returns>
        /// <exception cref="Exception"></exception>
        public static byte[] HexToBinary(this string hex)
        {
            const string IGNORED_CHARS = ":- ";

            byte[] arr = new byte[hex.Length >> 1];
            IntPtr bin = Marshal.AllocHGlobal(arr.Length);
            int binLength;

            //we call sodium_hex2bin with some chars to be ignored
            int ret = SodiumLibrary.sodium_hex2bin(bin, arr.Length, hex, hex.Length, IGNORED_CHARS, out binLength,
                null);

            Marshal.Copy(bin, arr, 0, binLength);
            Marshal.FreeHGlobal(bin);

            if (ret != 0)
            {
                throw new Exception("Internal error, decoding failed.");
            }

            //remove the trailing nulls from the array, if there were some format characters in the hex string before
            if (arr.Length != binLength)
            {
                byte[] tmp = new byte[binLength];
                Array.Copy(arr, 0, tmp, 0, binLength);
                return tmp;
            }

            return arr;
        }
    }
}