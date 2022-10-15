using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2_BD.Models
{
    public partial class Order
    {
        
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int ClientId { get; set; }
        public int WorkerId { get; set; }
        public Client Client { get; set; }
        public Worker Worker { get; set; }
    }
}
