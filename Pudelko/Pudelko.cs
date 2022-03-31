using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round(2 * (A*B + A*C + B*C), 6);

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            var multiplier = unit switch
            {
                UnitOfMeasure.meter => 1,
                UnitOfMeasure.centimeter => 0.01,
                UnitOfMeasure.milimeter => 0.001,
                _ => throw new ArgumentException()
            };

            a = a==null ? 0.1 : a * multiplier;
            b = b==null ? 0.1 : b * multiplier;
            c = c==null ? 0.1 : c * multiplier;

            if (0.001 > a || a >= 10) throw new ArgumentOutOfRangeException("Argument a is out of range. Should be more than 0m and less than 10m.");
            if (0.001 > b || b >= 10) throw new ArgumentOutOfRangeException("Argument b is out of range. Should be more than 0m and less than 10m.");
            if (0.001 > c || c >= 10) throw new ArgumentOutOfRangeException("Argument c is out of range. Should be more than 0m and less than 10m.");

            A = (double)a;
            B = (double)b;
            C = (double)c;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as Pudelko);
        }

        public bool Equals(Pudelko other)
        {
            double[] x = { A, B, C };
            double[] y = { other.A, other.B, other.C };
            Array.Sort(x); 
            Array.Sort(y);

            return Enumerable.SequenceEqual(x, y);
        }

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double[] x = { p1.A, p1.B, p1.C };
            double[] y = { p2.A, p2.B, p2.C };
            Array.Sort(x); Array.Reverse(x);
            Array.Sort(y); Array.Reverse(y);

            var a = Math.Max(x[0], y[0]);
            var b = Math.Max(x[1], y[1]);
            var c = x[2] + y[2];

            return new Pudelko(a, b, c);
        }

        public override int GetHashCode() => HashCode.Combine(A, B, C);

        public static bool operator ==(Pudelko leftBox, Pudelko rightBox) => leftBox.Equals(rightBox);

        public static bool operator !=(Pudelko leftBox, Pudelko rightBox) => !leftBox.Equals(rightBox);

        public static explicit operator double[](Pudelko p) => new[] { p.A, p.B, p.C };

        public static implicit operator Pudelko(ValueTuple<int, int, int> t) => new Pudelko(t.Item1, t.Item2, t.Item3, UnitOfMeasure.milimeter);

        public double this[int i] => i switch 
        { 
            0 => A,
            1 => B,
            2 => C,
            _ => throw new IndexOutOfRangeException()
        };


        public override string ToString() => ToString("m", CultureInfo.CurrentCulture);

        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "m";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            return format switch
            {
                "m"  => $"{A.ToString("0.000", provider)} m × {B.ToString("0.000", provider)} m × {C.ToString("0.000", provider)} m",
                "cm" => $"{(A*100).ToString("0.0", provider)} cm × {(B*100).ToString("0.0", provider)} cm × {(C*100).ToString("0.0", provider)} cm",
                "mm" => $"{(A*1000).ToString("0", provider)} mm × {(B*1000).ToString("0", provider)} mm × {(C*1000).ToString("0", provider)} mm",
                _    => throw new FormatException($"The {format} format is not supported.")
            };
        }


        public static Pudelko Parse(string text)
        {
            string[] stringValues = text.Split('×', StringSplitOptions.TrimEntries);
            if (stringValues.Length != 3) throw new ArgumentException($"Wrong parse input given: {text}");

            double[] values = new double[3];
            for (int i = 0; i < stringValues.Length; i++)
            {
                string[] value_unit = stringValues[i].Split(' ', StringSplitOptions.TrimEntries);
                string unit = value_unit[1];
                double.TryParse(value_unit[0], out values[i]);

                values[i] = unit switch
                {
                    "m" => values[i],
                    "cm" => values[i] * 0.01,
                    "mm" => values[i] * 0.001,
                    _ => throw new FormatException($"Wrong unit: {unit}")
                };
            }

            return new Pudelko(values[0], values[1], values[2]);
        }


        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
