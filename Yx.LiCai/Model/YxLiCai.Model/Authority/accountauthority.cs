using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Authority
{
    public class accountauthority
    {
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> AuthorityId { get; set; }

        public virtual account account { get; set; }
        public virtual authority authority { get; set; }
    }
}
