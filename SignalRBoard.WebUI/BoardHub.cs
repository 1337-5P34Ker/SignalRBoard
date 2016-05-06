using System;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRBoard.DataAccess;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRBoard.DataModel;

namespace SignalRBoard
{
    public class BoardHub : Hub
    {
        [HubMethodName("Add")]
        public void Add(string title, string description, string listId)
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
            // Call the addCardMessage method to update clients.
            Clients.All.addCardMessage(card);
        }

        [HubMethodName("Update")]
        public void Update(string title, string description, string cardId)
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
            // Call the addCardMessage method to update clients.
            Clients.All.updateCardMessage(card);
        }



        [HubMethodName("Move")]
        public void Move(string cardId, Direction direction)
        {
            Board board = new Board();

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
                    }
                }
            }

            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:

                    

                    break;
            }

            // Call the addCardMessage method to update clients.
            Clients.All.boardMessage(board.Lists);
        }

        [HubMethodName("Delete")]
        public void Delete(string cardId)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                provider.DeleteCard(new CardParameters
                {
                    Id = new Guid(cardId)
                });
            }
            // Call the deleteCardMessage method to update clients.
            Clients.All.deleteCardMessage(cardId);
        }

        [HubMethodName("Init")]
        public void Init(string message)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                var lists = provider.GetLists(new ListParameters());
                // Call the broadcastMessage method to update clients.

                foreach (var list in lists)
                {
                    list.Cards = provider.GetCards(new CardParameters { ListId = list.Id });
                }

                Clients.All.boardMessage(lists);
            }
        }

        [HubMethodName("GetCards")]
        public void GetCards(string message)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                var result = provider.GetCards(new CardParameters());
                // Call the broadcastMessage method to update clients.
                Clients.All.broadcastMessage(result);
            }
        }

        [HubMethodName("GetLists")]
        public void GetLists(string message)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                var result = provider.GetLists(new ListParameters());
                // Call the broadcastMessage method to update clients.
                Clients.All.broadcastMessage(result);
            }
        }
    }
}