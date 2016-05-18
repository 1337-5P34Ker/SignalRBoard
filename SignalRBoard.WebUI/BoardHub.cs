using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using SignalRBoard.Business;
using SignalRBoard.DataAccess;
using SignalRBoard.DataModel;

namespace SignalRBoard
{
    public class BoardHub : Hub
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger(); 

        [HubMethodName("Add")]
        public void Add(string title, string description)
        {
            _logger.Info("Add called");
            Board board = BoardHelper.AddCard(title, description);
            // Call the boardMessage method to update all clients.
            Clients.All.boardMessage(board.Lists);
        }

        [HubMethodName("Update")]
        public void Update(string title, string description, string cardId)
        {
            Card card = BoardHelper.UpdateCard(title, description, cardId);
            // Call the addCardMessage method to update clients.
            Clients.All.updateCardMessage(card);
        }

        [HubMethodName("Move")]
        public void Move(Guid cardId, Direction direction)
        {
            Board board = BoardHelper.MoveCard(cardId, direction);
            // Call the boardMessage method to update clients.
            Clients.All.boardMessage(board.Lists);
            Clients.Caller.activateCardMessage(cardId);
            Clients.Caller.directionsMessage(BoardHelper.GetDirections(cardId));
        }


        [HubMethodName("MoveTo")]
        public void MoveTo(Guid cardId, Guid listId, int position)
        {
            Board board = BoardHelper.MoveCardTo(cardId, listId, position);
            // Call the boardMessage method to update clients.
            Clients.All.boardMessage(board.Lists);
            Clients.Caller.activateCardMessage(cardId);
            Clients.Caller.directionsMessage(BoardHelper.GetDirections(cardId));
        }

        [HubMethodName("Delete")]
        public void Delete(string cardId)
        {
            BoardHelper.DeleteCard(cardId);
            // Call the deleteCardMessage method to update clients.
            Clients.All.deleteCardMessage(cardId);
        }

        [HubMethodName("Init")]
        public void Init(string message)
        {
            var board = BoardHelper.InitializeBoard();
            Clients.All.boardMessage(board.Lists);
        }

        [HubMethodName("GetCards")]
        public void GetCards(string message)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                var result = provider.GetCards(new CardParameters());
                // Call the broadcastMessage method to update clients.
                Clients.Caller.broadcastMessage(result);
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

        [HubMethodName("GetDirections")]
        public void GetDirections(Guid cardId)
        {
            // Call the directionsMessage method to update single client.
            Clients.Caller.directionsMessage(BoardHelper.GetDirections(cardId));
        }
    }
}