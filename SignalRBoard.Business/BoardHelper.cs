using SignalRBoard.DataAccess;
using SignalRBoard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SignalRBoard.Business
{
    public static class BoardHelper
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static Board InitializeBoard()
        {
            return GetBoard();
        }

        public static Board AddCard(string title, string description)
        {
            Board board = GetBoard();
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                var sourceList = board.Lists.FirstOrDefault();
                if (sourceList != null && sourceList.Cards.Count < sourceList.MaxItems)
                {
                    provider.UpdateCard(new CardParameters
                    {
                        Title = title,
                        Description = description,
                        ListId = sourceList.Id
                    });
                }
            }
            return GetBoard();
        }

        public static Card UpdateCard(string title, string description, string cardId)
        {
            Card card;
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                card = provider.UpdateCard(new CardParameters
                {
                    Title = title,
                    Description = description,
                    Id = new Guid(cardId)
                });
            }
            return card;
        }

        public static void DeleteCard(string cardId)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                provider.DeleteCard(new CardParameters
                {
                    Id = new Guid(cardId)
                });
            }
        }

        private static void UpdateCard(Card card)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                provider.UpdateCard(new CardParameters
                {
                    Id = card.Id,
                    Position = card.Position,
                    Description = card.Description,
                    Title = card.Title,
                    ListId = card.ListId
                });
            }
        }

        public static Board MoveCard(Guid cardId, Direction direction)
        {
            Board board = new Board();
            List sourceList, destinationList;
            Card affectedCard;
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                board.Lists = provider.GetLists(new ListParameters()).OrderBy(l => l.Position).ToList();
                if (board.Lists.Any())
                {
                    foreach (var list in board.Lists)
                    {
                        list.Cards = provider.GetCards(new CardParameters
                        {
                            ListId = list.Id
                        }).OrderBy(c => c.Position).ToList();
                        foreach (var card in list.Cards)
                        {
                            card.Position = list.Cards.IndexOf(card); // reset positions
                        }
                    }
                }
            }
            switch (direction)
            {
                case Direction.Up:

                    sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    if (sourceList != null)
                    {
                        affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                        var index = sourceList.Cards.IndexOf(affectedCard);
                        if (index > 0)
                        {
                            sourceList.Cards.Remove(affectedCard);
                            sourceList.Cards.Insert(index - 1, affectedCard);
                            ResetPositions(sourceList);
                        }
                    }
                    break;

                case Direction.Down:

                    sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    if (sourceList != null)
                    {
                        affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                        var index = sourceList.Cards.IndexOf(affectedCard);
                        if (index < sourceList.Cards.Count - 1)
                        {
                            sourceList.Cards.Remove(affectedCard);
                            sourceList.Cards.Insert(index + 1, affectedCard);
                            ResetPositions(sourceList);
                        }

                    }
                    break;
                case Direction.Right:
                    sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));

                    if (sourceList != null)
                    {
                        affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                        var index = sourceList.Cards.IndexOf(affectedCard);
                        destinationList = board.Lists.FirstOrDefault(l => l.Position == sourceList.Position + 1);
                        if (destinationList != null)
                        {
                            sourceList.Cards.Remove(affectedCard);
                            affectedCard.ListId = destinationList.Id;
                            destinationList.Cards.Insert(destinationList.Cards.Count >= index ? index : destinationList.Cards.Count, affectedCard);
                            ResetPositions(sourceList);
                            ResetPositions(destinationList);
                        }

                    }

                    break;

                case Direction.Left:
                    sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));

                    if (sourceList != null)
                    {
                        affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                        var index = sourceList.Cards.IndexOf(affectedCard);
                        destinationList = board.Lists.FirstOrDefault(l => l.Position == sourceList.Position - 1);
                        if (destinationList != null)
                        {
                            sourceList.Cards.Remove(affectedCard);
                            affectedCard.ListId = destinationList.Id;
                            destinationList.Cards.Insert(destinationList.Cards.Count >= index ? index : destinationList.Cards.Count, affectedCard);

                            ResetPositions(sourceList);
                            ResetPositions(destinationList);
                        }
                    }
                    break;
            }
            return board;
        }

        public static Board MoveCardTo(Guid cardId, Guid listId, int position)
        {
            Board board = GetBoard();
            var sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
            var destinationList = board.Lists.FirstOrDefault(l => l.Id == listId);
            if (sourceList != null && destinationList != null)
            {
                var affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                if (destinationList.Cards.Count < destinationList.MaxItems || destinationList == sourceList)
                {
                    sourceList.Cards.Remove(affectedCard);
                    affectedCard.ListId = destinationList.Id;
                    destinationList.Cards.Insert(position - 1, affectedCard);
                    ResetPositions(sourceList);
                    ResetPositions(destinationList);
                }
            }
            return board;
        }

        public static List<string> GetDirections(Guid cardId)
        {
            var board = GetBoard();
            var directions = new List<string>();

            var sourceList = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));


            if (sourceList != null)
            {
                var affectedCard = sourceList.Cards.First(c => c.Id == cardId);
                var index = sourceList.Cards.IndexOf(affectedCard);
                if (index < sourceList.Cards.Count - 1)
                {
                    directions.Add(Direction.Down.ToString());
                }

                if (index > 0)
                {
                    directions.Add(Direction.Up.ToString());
                }

                var destinationList = board.Lists.FirstOrDefault(l => l.Position == sourceList.Position + 1);  //right
                if (destinationList != null && destinationList.Cards.Count < destinationList.MaxItems)
                {
                    directions.Add(Direction.Right.ToString());
                }

                destinationList = board.Lists.FirstOrDefault(l => l.Position == sourceList.Position - 1); // left
                if (destinationList != null && destinationList.Cards.Count < destinationList.MaxItems)
                {
                    directions.Add(Direction.Left.ToString());
                }
            }
            return directions;
        }

        private static void ResetPositions(List destinationList)
        {
            foreach (var card in destinationList.Cards)
            {
                card.Position = destinationList.Cards.IndexOf(card); // reset positions
                UpdateCard(card);
            }
        }

        private static Board GetBoard()
        {
            Board board = new Board();
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                board.Lists = provider.GetLists(new ListParameters());
                // Call the broadcastMessage method to update clients.

                foreach (var list in board.Lists)
                {
                    list.Cards = provider.GetCards(new CardParameters { ListId = list.Id });
                }
            }
            return board;
        }
    }
}
