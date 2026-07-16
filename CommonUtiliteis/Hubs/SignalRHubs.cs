using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis.Hubs
{
    public class SignalRHubs: Hub<ISignalRHubs>
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public void SendMessageToClient(string message)
        {
            Clients.Client(GetConnectionId()).SendMessageToClient(message);
        }
        public void FirstEndpointOfSignalRService()
        {
            Clients.Client(GetConnectionId()).DisplayMessage("Hello from the SignalrDemoHub!");
        }
    }
}
