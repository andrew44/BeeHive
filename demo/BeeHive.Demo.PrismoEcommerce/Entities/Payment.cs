﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeHive.Demo.PrismoEcommerce.Entities
{
    public class Payment
    {
        public Payment()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }
    }
}
