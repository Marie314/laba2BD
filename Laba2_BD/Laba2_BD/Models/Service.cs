﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2_BD.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ServiceKindId { get; set; }
        public int OrderId { get; set; }
        public ServiceKind ServiceKind { get; set; }
        public Order Order { get; set; }
    }
}
