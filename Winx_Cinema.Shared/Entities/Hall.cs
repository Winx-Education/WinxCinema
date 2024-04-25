using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winx_Cinema.Shared.Entities
{
    public class Hall
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public string Resolution { get; set; }
    }
}
