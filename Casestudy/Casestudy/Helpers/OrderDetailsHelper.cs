using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public int OrderLineItemId { get; set; }
        public int UserId { get; set; }
        public int QtyOrder { get; set; }
        public int QtyBackOrder { get; set; }
        public int QtySold { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal ProductPrice { get; set; }

        public string DateCreated { get; set; }
    }
}
