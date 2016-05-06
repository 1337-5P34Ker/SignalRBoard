using System;

namespace SignalRBoard.DataAccess
{
    public class CardParameters
    {
        public Guid? ListId { get; set; }
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int? Position { get; set; }
    }
}