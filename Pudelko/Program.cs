using System;
using System.Collections.Generic;

namespace PudelkoLibrary
{
    class Program
    {
        private static void Main()
        {
            List<Pudelko> pudelka = new List<Pudelko>
            {
                new Pudelko(),
                new Pudelko(2.192, 1.442, 3.123),
                new Pudelko(b: 3140, a: 5125 , unit: UnitOfMeasure.milimeter),
                new Pudelko(10, 20 ,30, UnitOfMeasure.centimeter),
                new Pudelko(100, 200, 300, UnitOfMeasure.milimeter)
            };

            foreach (var p in pudelka)
            {
                Console.WriteLine(p.ToString());
            }
            Console.WriteLine();

            pudelka.Sort(delegate (Pudelko p1, Pudelko p2)
            {
                if (p1.Objetosc > p2.Objetosc) return 1;
                if (p1.Objetosc < p2.Objetosc) return -1;

                if (p1.Pole > p2.Pole) return 1;
                if (p1.Pole < p2.Pole) return -1;

                double p1Sum = p1.A + p1.B + p1.C;
                double p2Sum = p2.A + p2.B + p2.C;

                if (p1Sum > p2Sum) return 1;
                if (p1Sum < p2Sum) return 1;

                return 0;
            });

            foreach (var p in pudelka)
            {
                Console.WriteLine(p.ToString());
            }
        }
    }
}
