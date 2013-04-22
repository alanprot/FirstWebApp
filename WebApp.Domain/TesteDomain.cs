using MongoDBFramework.Databind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Domain
{
    [DataContract]
    public class TesteDomain : DataBind<TesteDomain>
    {
        [DataMember(Name = "nome")]
        public string Nome { get; set; }

        [DataMember (Name = "idade")]
        public int Idade { get; set; }
    }
}
