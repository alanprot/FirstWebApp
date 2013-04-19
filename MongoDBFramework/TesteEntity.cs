using MongoDBFramework.Databind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBFramework
{
    public class TesteEntity : DataBind<TesteEntity>
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}
