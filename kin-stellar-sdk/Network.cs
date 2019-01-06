﻿using System;
using System.Text;

namespace Kin.Stellar.Sdk
{
    public class Network
    {
        private static readonly string PUBLIC = "Public Global Stellar Network ; September 2015";
        private static readonly string TESTNET = "Test SDF Network ; September 2015";

        public string NetworkPassphrase { get; }

        public byte[] NetworkId => Util.Hash(Encoding.UTF8.GetBytes(NetworkPassphrase));

        public static Network Current { get; private set; }

        public Network(string networkPassphrase)
        {
            NetworkPassphrase = networkPassphrase ??
                                throw new ArgumentNullException(nameof(networkPassphrase),
                                    "networkPassphrase cannot be null");
        }

        public static Network Public()
        {
            return new Network(PUBLIC);
        }

        public static Network Test()
        {
            return new Network(TESTNET);
        }

        public static void Use(Network network)
        {
            Current = network;
        }

        public static bool IsPublicNetwork(Network network)
        {
            return network.NetworkPassphrase == PUBLIC;
        }

        public static void UsePublicNetwork()
        {
            Use(Public());
        }

        public static void UseTestNetwork()
        {
            Use(Test());
        }
    }
}