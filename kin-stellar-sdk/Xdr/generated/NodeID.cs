// Automatically generated by xdrgen
// DO NOT EDIT or your changes may be overwritten

namespace Kin.Stellar.Sdk.xdr
{
    // === xdr source ============================================================

    //  typedef PublicKey NodeID;

    //  ===========================================================================
    public class NodeID
    {
        public PublicKey InnerValue { get; set; } = default(PublicKey);

        public NodeID() { }

        public NodeID(PublicKey value)
        {
            InnerValue = value;
        }

        public static void Encode(XdrDataOutputStream stream, NodeID encodedNodeID)
        {
            PublicKey.Encode(stream, encodedNodeID.InnerValue);
        }

        public static NodeID Decode(XdrDataInputStream stream)
        {
            NodeID decodedNodeID = new NodeID();
            decodedNodeID.InnerValue = PublicKey.Decode(stream);
            return decodedNodeID;
        }
    }
}