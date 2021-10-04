using System;
using System.Transactions;

namespace TransactionManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var vrm = new VolatileRM("RM1");
            var vrm2 = new VolatileRM("RM2");
            Console.WriteLine($"Member Value 1:{vrm.MemberValue}");
            Console.WriteLine($"Member Value 2:{vrm2.MemberValue}");

            using (var tsc = new TransactionScope())
            {
                vrm.MemberValue = 3;
                vrm2.MemberValue = 5;
                tsc.Complete();
            }

            Console.WriteLine($"Member Value 1:{vrm.MemberValue}");
            Console.WriteLine($"Member Value 2:{vrm2.MemberValue}");
        }
    }
}
