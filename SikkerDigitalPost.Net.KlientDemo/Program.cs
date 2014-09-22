using System;
using System.IO;
using SikkerDigitalPost.Net.Domene;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Forsendelse();
            f.MpcId = "22";
            
            Console.WriteLine(f.MpcId);
            Console.ReadKey();

        }
    }
}
