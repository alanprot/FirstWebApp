using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<TesteEntity>();
            var PageNumber = TesteEntity.GetPageCount((a, b) => a.Idade > b.Idade);


            for (int i = 1; i < PageNumber + 1; i++)
            {
                var partialList = TesteEntity.GetPage((a, b) => a.Idade > b.Idade, i).ToList();
                list.AddRange(partialList);
            }
           // var teste = TesteEntity.GetById(a.Id);
        }
    }
}
