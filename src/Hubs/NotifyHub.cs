using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace src.Hubs
{
    public class NotifyHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("sendToAll", message);
        }
    }
}
