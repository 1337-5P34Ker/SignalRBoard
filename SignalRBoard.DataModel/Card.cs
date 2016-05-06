using System;

namespace SignalRBoard.DataModel
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public Guid ListId { get; set; }
        public int Position { get; set; }
    }
}