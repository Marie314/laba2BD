using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laba2_BD.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Description { get; set; }


    }
}
