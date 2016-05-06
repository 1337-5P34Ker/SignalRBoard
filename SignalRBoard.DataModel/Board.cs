using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBoard.DataModel
{
    public class Board
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<List> Lists { get; set; }
    }
}
