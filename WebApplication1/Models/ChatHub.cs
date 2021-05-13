using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ChatHub : Hub
    {
        public readonly static List<UserModel> _Connections = new List<UserModel>();
        private readonly AuthDbContext _context;

        public ChatHub(AuthDbContext context)
        {
            _context = context;
        }

        public async Task Send(string message)
        {
            await Clients.All.SendAsync("newMessage", message);
        }
        public async Task Join(string message)
        {
            await Clients.All.SendAsync("Join", IdentityName);
        }

        //public async Task SendPrivate(string receiverName, string message)
        //{
        //    if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
        //    {
        //        // Who is the sender;
        //        var sender = _Connections.Where(u => u.UserName == receiverName).First();

        //        if (!string.IsNullOrEmpty(message.Trim()))
        //        {
        //            // Build the message
        //            var messageModel = new NewsModel()
        //            {
        //                Text = message,
        //                UserId = sender.Id,
        //                UserName = sender.UserName,
        //                dateTime = DateTime.Now
        //            };

        //            // Send the message
        //            await Clients.Client(userId).SendAsync("newMessage", messageModel);
        //            await Clients.Caller.SendAsync("newMessage", messageModel);
        //        }
        //    }
        //}

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
    }
}