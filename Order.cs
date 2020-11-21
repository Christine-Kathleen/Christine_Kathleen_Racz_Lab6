namespace AutoLotModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int OrderId { get; set; }

        public int? CustId { get; set; }

        public int? CarId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Inventory Inventory { get; set; }
    }

    public class OrderSpecial
    {
        public int OrderId { get; set; }
        public int? CustId { get; set; }

        public int? CarId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }


    }
}
