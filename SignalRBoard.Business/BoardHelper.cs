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
            List affectedList1, affectedList2;
            Card affectedCard1, affectedCard2;
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

                    affectedList1 = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    if (affectedList1 != null)
                    {
                        affectedCard1 = affectedList1.Cards.First(c => c.Id == cardId);

                        if (affectedCard1.Position > 0)
                        {
                            affectedCard2 = affectedList1.Cards.FirstOrDefault(c => c.Position == affectedCard1.Position - 1);
                            if (affectedCard2 != null)
                            {
                                affectedCard2.Position = affectedCard1.Position;
                                affectedCard1.Position = affectedCard1.Position - 1;
                                UpdateCard(affectedCard1);
                                UpdateCard(affectedCard2);
                            }

                        }
                    }
                        break;

                case Direction.Down:

                    affectedList1 = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    if (affectedList1 != null)
                    {
                        affectedCard1 = affectedList1.Cards.First(c => c.Id == cardId);

                        if (affectedCard1.Position < affectedList1.Cards.Count)
                        {
                            affectedCard2 = affectedList1.Cards.FirstOrDefault(c => c.Position == affectedCard1.Position + 1);
                            if (affectedCard2 != null)
                            {
                                affectedCard2.Position = affectedCard1.Position;
                                affectedCard1.Position = affectedCard1.Position + 1;
                                UpdateCard(affectedCard1);
                                UpdateCard(affectedCard2);
                            }

                        }
                    }
                    break;
                    case Direction.Right:
                    affectedList1 = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    
                    if (affectedList1 != null)
                    {
                        affectedCard1 = affectedList1.Cards.First(c => c.Id == cardId);
                        affectedList2 = board.Lists.FirstOrDefault(l => l.Position == affectedList1.Position + 1);
                        if (affectedList2 != null)
                        {
                            affectedCard1.ListId = affectedList2.Id;
                            UpdateCard(affectedCard1);
                        }

                    }

                    break;

                case Direction.Left:
                    affectedList1 = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));

                    if (affectedList1 != null)
                    {
                        affectedCard1 = affectedList1.Cards.First(c => c.Id == cardId);
                        affectedList2 = board.Lists.FirstOrDefault(l => l.Position == affectedList1.Position - 1);
                        if (affectedList2 != null)
                        {
                            affectedCard1.ListId = affectedList2.Id;
                            UpdateCard(affectedCard1);
                        }
                    }
                    break;
            }

            foreach (var list in board.Lists)
            {
                list.Cards = list.Cards.OrderBy(c => c.Position).ToList();                
            }

            return board;
        }
    }
}
