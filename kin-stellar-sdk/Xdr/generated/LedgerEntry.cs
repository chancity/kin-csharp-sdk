// Automatically generated by xdrgen
// DO NOT EDIT or your changes may be overwritten

namespace Kin.Stellar.Sdk.xdr
{
    // === xdr source ============================================================

    //  struct LedgerEntry
    //  {
    //      uint32 lastModifiedLedgerSeq; // ledger the LedgerEntry was last changed
    //  
    //      union switch (LedgerEntryType type)
    //      {
    //      case ACCOUNT:
    //          AccountEntry account;
    //      case TRUSTLINE:
    //          TrustLineEntry trustLine;
    //      case OFFER:
    //          OfferEntry offer;
    //      case DATA:
    //          DataEntry data;
    //      }
    //      data;
    //  
    //      // reserved for future use
    //      union switch (int v)
    //      {
    //      case 0:
    //          void;
    //      }
    //      ext;
    //  };

    //  ===========================================================================
    public class LedgerEntry
    {
        public Uint32 LastModifiedLedgerSeq { get; set; }
        public LedgerEntryData Data { get; set; }
        public LedgerEntryExt Ext { get; set; }

        public static void Encode(XdrDataOutputStream stream, LedgerEntry encodedLedgerEntry)
        {
            Uint32.Encode(stream, encodedLedgerEntry.LastModifiedLedgerSeq);
            LedgerEntryData.Encode(stream, encodedLedgerEntry.Data);
            LedgerEntryExt.Encode(stream, encodedLedgerEntry.Ext);
        }

        public static LedgerEntry Decode(XdrDataInputStream stream)
        {
            LedgerEntry decodedLedgerEntry = new LedgerEntry();
            decodedLedgerEntry.LastModifiedLedgerSeq = Uint32.Decode(stream);
            decodedLedgerEntry.Data = LedgerEntryData.Decode(stream);
            decodedLedgerEntry.Ext = LedgerEntryExt.Decode(stream);
            return decodedLedgerEntry;
        }

        public class LedgerEntryData
        {
            public LedgerEntryType Discriminant { get; set; } = new LedgerEntryType();

            public AccountEntry Account { get; set; }
            public TrustLineEntry TrustLine { get; set; }
            public OfferEntry Offer { get; set; }
            public DataEntry Data { get; set; }

            public static void Encode(XdrDataOutputStream stream, LedgerEntryData encodedLedgerEntryData)
            {
                stream.WriteInt((int) encodedLedgerEntryData.Discriminant.InnerValue);

                switch (encodedLedgerEntryData.Discriminant.InnerValue)
                {
                    case LedgerEntryType.LedgerEntryTypeEnum.ACCOUNT:
                        AccountEntry.Encode(stream, encodedLedgerEntryData.Account);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.TRUSTLINE:
                        TrustLineEntry.Encode(stream, encodedLedgerEntryData.TrustLine);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.OFFER:
                        OfferEntry.Encode(stream, encodedLedgerEntryData.Offer);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.DATA:
                        DataEntry.Encode(stream, encodedLedgerEntryData.Data);
                        break;
                }
            }

            public static LedgerEntryData Decode(XdrDataInputStream stream)
            {
                LedgerEntryData decodedLedgerEntryData = new LedgerEntryData();
                LedgerEntryType discriminant = LedgerEntryType.Decode(stream);
                decodedLedgerEntryData.Discriminant = discriminant;

                switch (decodedLedgerEntryData.Discriminant.InnerValue)
                {
                    case LedgerEntryType.LedgerEntryTypeEnum.ACCOUNT:
                        decodedLedgerEntryData.Account = AccountEntry.Decode(stream);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.TRUSTLINE:
                        decodedLedgerEntryData.TrustLine = TrustLineEntry.Decode(stream);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.OFFER:
                        decodedLedgerEntryData.Offer = OfferEntry.Decode(stream);
                        break;
                    case LedgerEntryType.LedgerEntryTypeEnum.DATA:
                        decodedLedgerEntryData.Data = DataEntry.Decode(stream);
                        break;
                }

                return decodedLedgerEntryData;
            }
        }

        public class LedgerEntryExt
        {
            public int Discriminant { get; set; }

            public static void Encode(XdrDataOutputStream stream, LedgerEntryExt encodedLedgerEntryExt)
            {
                stream.WriteInt(encodedLedgerEntryExt.Discriminant);

                switch (encodedLedgerEntryExt.Discriminant)
                {
                    case 0:
                        break;
                }
            }

            public static LedgerEntryExt Decode(XdrDataInputStream stream)
            {
                LedgerEntryExt decodedLedgerEntryExt = new LedgerEntryExt();
                int discriminant = stream.ReadInt();
                decodedLedgerEntryExt.Discriminant = discriminant;

                switch (decodedLedgerEntryExt.Discriminant)
                {
                    case 0:
                        break;
                }

                return decodedLedgerEntryExt;
            }
        }
    }
}