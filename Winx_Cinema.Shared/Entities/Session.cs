using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winx_Cinema.Shared.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmResolutionId { get; set; }
        public DateTime StartTime { get; set; }

        public Hall Hall { get; set; }
        public FilmResolution FilmResolution { get; set; }
    }
}
