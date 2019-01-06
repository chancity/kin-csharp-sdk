using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;


namespace Kin.Backup.LibSodium
{
  internal static class SodiumLibrary
  {
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sodium_init();

    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void randombytes_buf(byte[] buffer, int size);

    //randombytes_uniform
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int randombytes_uniform(int upperBound);

    //sodium_version_string
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sodium_version_string();


    //crypto_pwhash_str
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_str(byte[] buffer, byte[] password, long passwordLen, long opsLimit, int memLimit);

    //crypto_pwhash_str_verify
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_str_verify(byte[] buffer, byte[] password, long passLength);

    //crypto_pwhash
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash(byte[] buffer, long bufferLen, byte[] password, long passwordLen, byte[] salt, long opsLimit, int memLimit, int alg);

    //crypto_secretbox_easy
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_secretbox_easy(byte[] buffer, byte[] message, long messageLength, byte[] nonce, byte[] key);

    //crypto_secretbox_open_easy
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_secretbox_open_easy(byte[] buffer, byte[] cipherText, long cipherTextLength, byte[] nonce, byte[] key);

    //sodium_bin2hex
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sodium_bin2hex(byte[] hex, int hexMaxlen, byte[] bin, int binLen);

    //sodium_hex2bin
    [DllImport("libsodium", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sodium_hex2bin(IntPtr bin, int binMaxlen, string hex, int hexLen, string ignore, out int binLen, string hexEnd);

    //crypto_generichash_state
    [StructLayout(LayoutKind.Sequential, Size = 384)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    internal struct _HashState
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
      public ulong[] h;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      public ulong[] t;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      public ulong[] f;

      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
      public byte[] buf;

      public uint buflen;

      public byte last_node;
    }
  }
}
