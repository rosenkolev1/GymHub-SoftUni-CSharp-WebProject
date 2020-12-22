using GymHub.Web.Models.InputModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GymHub.Web.Hubs
{
    public class ContactsChatHub : Hub
    {
        public async Task Send(MessageInputModel inputModel)
        {

            return;
        }
    }
}
