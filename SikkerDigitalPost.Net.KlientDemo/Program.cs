using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
