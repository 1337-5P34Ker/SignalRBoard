using SignalRBoard.DataAccess;
using SignalRBoard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBoard.Business
{
    public static class BoardHelper
    {
        public static Board InitializeBoard()
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

        public static Card AddCard(string title, string description, string listId)
        {
            Card card;
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                card = provider.UpdateCard(new CardParameters
                {
                    Title = title,
                    Description = description,
                    ListId = new Guid(listId)
                });
            }
            return card;
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
                            foreach (var card in sourceList.Cards)
                            {
                                card.Position = sourceList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
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
                            sourceList.Cards.Insert(sourceList.Cards.Count >= index + 1 ? index + 1 : sourceList.Cards.Count, affectedCard);
                            foreach (var card in sourceList.Cards)
                            {
                                card.Position = sourceList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
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
                            foreach (var card in sourceList.Cards)
                            {
                                card.Position = sourceList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
                            foreach (var card in destinationList.Cards)
                            {
                                card.Position = destinationList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
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
                            foreach (var card in sourceList.Cards)
                            {
                                card.Position = sourceList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
                            foreach (var card in destinationList.Cards)
                            {
                                card.Position = destinationList.Cards.IndexOf(card); // reset positions
                                UpdateCard(card);
                            }
                        }
                    }
                    break;
            }
            return board;
        }
    }
}
