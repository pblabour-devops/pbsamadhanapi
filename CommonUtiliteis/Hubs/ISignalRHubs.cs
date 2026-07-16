using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis.Hubs
{
    public interface ISignalRHubs
    {
        Task DisplayMessage(string message);
        Task SendMessageToClient(string message);
        Task SendQrScanLoginResponseToConnectionClient(string message);
    }
}
