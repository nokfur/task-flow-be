using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.Hubs
{
    public class BoardHub: Hub
    {
        public async Task JoinBoard(string boardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
        }

        public async Task NotifyBoardUpdated(string boardId)
        {
            await Clients.GroupExcept(boardId, Context.ConnectionId)
                 .SendAsync("BoardUpdated", boardId);
        }
    }
}
