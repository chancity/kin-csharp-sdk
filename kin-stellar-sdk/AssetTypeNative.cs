using Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class AssetTypeNative : Asset
    {
        public override int GetHashCode()
        {
            return 0;
        }

        public override string GetType()
        {
            return "native";
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override xdr.Asset ToXdr()
        {
            xdr.Asset thisXdr = new xdr.Asset();
            thisXdr.Discriminant = AssetType.Create(AssetType.AssetTypeEnum.ASSET_TYPE_NATIVE);
            return thisXdr;
        }
    }
}