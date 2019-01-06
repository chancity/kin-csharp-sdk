using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace Kin.Backup.LibSodium.Interop
{
  internal static class SodiumLibrary
  {
    private const string Name = SodiumRuntimeConfig.LibraryName;

    static SodiumLibrary()
    {
      RuntimeShim.PinDllImportLibrary(Name);
      sodium_init();
    }

    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sodium_init();

    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void randombytes_buf(byte[] buffer, int size);

    //randombytes_uniform
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int randombytes_uniform(int upperBound);

    //sodium_increment
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sodium_increment(byte[] buffer, long length);

    //sodium_compare
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sodium_compare(byte[] a, byte[] b, long length);

    //sodium_version_string
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sodium_version_string();


    //crypto_pwhash_str
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_str(byte[] buffer, byte[] password, long passwordLen, long opsLimit, int memLimit);

    //crypto_pwhash_str_verify
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash_str_verify(byte[] buffer, byte[] password, long passLength);

    //crypto_pwhash
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_pwhash(byte[] buffer, long bufferLen, byte[] password, long passwordLen, byte[] salt, long opsLimit, int memLimit, int alg);

    //crypto_secretbox_easy
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_secretbox_easy(byte[] buffer, byte[] message, long messageLength, byte[] nonce, byte[] key);

    //crypto_secretbox_open_easy
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int crypto_secretbox_open_easy(byte[] buffer, byte[] cipherText, long cipherTextLength, byte[] nonce, byte[] key);

    //sodium_bin2hex
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sodium_bin2hex(byte[] hex, int hexMaxlen, byte[] bin, int binLen);

    //sodium_hex2bin
    [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
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
