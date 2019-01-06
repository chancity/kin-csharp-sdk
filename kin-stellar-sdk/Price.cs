using System;
using System.Collections.Generic;
using sdkxdr = Kin.Stellar.Sdk.xdr;

namespace Kin.Stellar.Sdk
{
    public class Price
    {
        public int Numerator { get; }

        public int Denominator { get; }
        //@SerializedName("d")

        //@SerializedName("n")

        /// <summary>
        ///     Create a new price. Price in Stellar is represented as a fraction.
        /// </summary>
        /// <param name="n">Numerator</param>
        /// <param name="d">Denominator</param>
        public Price(int n, int d)
        {
            Numerator = n;
            Denominator = d;
        }

        /// <summary>
        ///     Approximates<code> price</code> to a fraction.
        /// </summary>
        /// <param name="price">Example 1.25</param>
        public static Price FromString(string price)
        {
            if (string.IsNullOrEmpty(price))
            {
                throw new ArgumentNullException(nameof(price), "price cannot be null");
            }

            decimal maxInt = new decimal(int.MaxValue);
            decimal number = Convert.ToDecimal(price);
            decimal a;
            decimal f;
            List<decimal[]> fractions = new List<decimal[]>();
            fractions.Add(new[] {new decimal(0), new decimal(1)});
            fractions.Add(new[] {new decimal(1), new decimal(0)});
            int i = 2;

            while (true)
            {
                if (number.CompareTo(maxInt) > 0)
                {
                    break;
                }

                a = decimal.Floor(number);
                f = decimal.Subtract(number, a);
                decimal h = decimal.Add(decimal.Multiply(a, fractions[i - 1][0]), fractions[i - 2][0]);
                decimal k = decimal.Add(decimal.Multiply(a, fractions[i - 1][1]), fractions[i - 2][1]);

                if (h.CompareTo(maxInt) > 0 || k.CompareTo(maxInt) > 0)
                {
                    break;
                }

                fractions.Add(new[] {h, k});

                if (f.CompareTo(0m) == 0)
                {
                    break;
                }

                number = decimal.Divide(1m, f);
                i = i + 1;
            }

            decimal n = fractions[fractions.Count - 1][0];
            decimal d = fractions[fractions.Count - 1][1];
            return new Price(Convert.ToInt32(n), Convert.ToInt32(d));
        }

        /// <summary>
        ///     Generates Price XDR object.
        /// </summary>
        public sdkxdr.Price ToXdr()
        {
            sdkxdr.Price xdr = new sdkxdr.Price();
            sdkxdr.Int32 n = new sdkxdr.Int32();
            sdkxdr.Int32 d = new sdkxdr.Int32();
            n.InnerValue = Numerator;
            d.InnerValue = Denominator;
            xdr.N = n;
            xdr.D = d;
            return xdr;
        }

        public new bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Price))
            {
                return false;
            }

            Price price = (Price) obj;

            return Numerator == price.Numerator &&
                   Denominator == price.Denominator;
        }
    }
}