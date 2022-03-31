using System;

namespace PudelkoLibrary
{
    public static class PudelkoExtension
    {
        public static Pudelko Kompresuj(this Pudelko p)
        {
            var len = Math.Pow(p.Objetosc, 1 / 3);
            return new Pudelko(len, len, len);
        }
    }
}