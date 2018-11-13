using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SameIDAlert
{
    public class SameIdHub : Hub
    {
        public async Task SendToPrevious(string name, string message, string groupName)
        {
            //Clients.Client(connId).
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.SendAsync("AddNewMessageToPage",name, message, groupName);
        }
        public void Send(string name, string message, string groupName)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.SendAsync("AddNewMessageToPage",name, message, groupName);
        }

        public async Task JoinGroupPageAndSendMessage(string name, string groupName)
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //await Clients.OthersInGroup(groupName).SendAsync("AddUser",name);
            await Clients.All.SendAsync("AddUser", name, groupName);

        }
        
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await LeaveGroupPageAndSendMessage(this.Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);

        }

        public async Task LeaveGroupPageAndSendMessage(string name)
        {

            //Clients.OthersInGroup(groupName).removeUser(name);
            await Clients.All.SendAsync("RemoveUser",name);

        }
    }

}
