using System;
using System.Collections.Generic;

namespace SignalRBoard.DataModel
{
    public class List
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int MaxItems { get; set; }

        public List<Card> Cards { get; set; }
        
        public int Position { get; set; }
    }
}