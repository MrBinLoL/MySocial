using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.ViewModel;
using WebApplication1.Data;
using WebApplication1.Areas.Identity.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public NewsController(AuthDbContext context,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Room>> Get(int id)
        //{
        //    var message = await _context.Messages.FindAsync(id);
        //    if (message == null)
        //        return NotFound();

        //    var messageViewModel = _mapper.Map<Message, MessageViewModel>(message);
        //    return Ok(messageViewModel);
        //}

        [HttpGet("Room/{roomName}")]
        public IActionResult GetMessages()
        {
            var messages = _context.News
                .Include(m => m.Like)
                .Include(m => m.Text)
                .Include(m => m.UserName)
                .OrderByDescending(m => m.dateTime)
                .Take(20)
                .AsEnumerable()
                .Reverse()
                .ToList();

            var NewsViewModel = new NewsModel(messages);

            return Ok(NewsViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<News>> Create(NewsModel newsModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return BadRequest();

            var msg = new News()
            {
                Text = Regex.Replace(newsModel.Text, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                UserName = user.UserName,
                UserId = user.Id,
                Like = 0,
                dateTime = DateTime.Now
            };

            _context.News.Add(msg);
            await _context.SaveChangesAsync();

            // Broadcast the message
            var createdMessage = new NewsModel(msg);
            await _hubContext.Clients.All.SendAsync("newMessage", createdMessage);

            return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdMessage);
        }

        public async Task<ActionResult> Get(string UserName)
        {
            var message = await _context.News.FindAsync(UserName);
            if (message == null)
                return NotFound();

            var messageViewModel = new NewsModel(message);
            return Ok(messageViewModel);
        }
    }
}
