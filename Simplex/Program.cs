using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            var tab = new SimplexObject(2, 2);
            tab[0] = new double[] { 1, -1, 1, 0, -1 };
            tab[1] = new double[] { 1,  1, 0, 1,  4 };
            tab.Max = new double[] { 3, 1, 0, 0,  0 };

            Console.WriteLine(tab);
            Console.ReadKey(true);

            while (tab.ShouldContinue())
            {
                tab.Iterate();
                Console.WriteLine(tab);
                Console.ReadKey(true);
            }

            Console.WriteLine("la réponse est : " + (-tab[tab.width - 1, tab.height - 1]));
            Console.ReadKey(true);
        }
        
        
        
    }

}
