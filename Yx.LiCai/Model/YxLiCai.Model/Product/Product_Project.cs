using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Product
{
    public class Product_Project
    {
        public int ProductID { get; set; }
        public int ProjectID { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
