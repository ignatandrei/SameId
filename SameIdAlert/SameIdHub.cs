using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SameIdAlert
{
    public class SameIdHub : Hub
    {
        public void SendToPrevious(string name, string message, string groupId)
        {
            //Clients.Client(connId).
            // Call the addNewMessageToPage method to update clients.
            Clients.Group(groupId).addNewMessageToPage(name, message);
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public async  Task JoinGroupPageAndSendMessage(string name, string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            Clients.OthersInGroup(groupName).addUser( name);
            

        }
        public override Task OnDisconnected()
        {
            LeaveGroupPageAndSendMessage(this.Context.User.Identity.Name);
            return base.OnDisconnected();

        }
        
        public async Task LeaveGroupPageAndSendMessage(string name)
        {
            
            //Clients.OthersInGroup(groupName).removeUser(name);
            await Clients.All.removeUser(name);

        }
    }
}