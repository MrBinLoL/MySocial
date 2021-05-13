using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        public HomeController(AuthDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult News() 
        {
            IEnumerable<News> news = _context.News;
            ViewBag.News = news;
            return View();
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

        //public async Task<IActionResult> Create(News news) 
        //{
        //    if (ModelState.IsValid)
        //    {
        //        news.UserName = User.Identity.Name;
        //        var sender = await _userManager.GetUserAsync(User);
        //        news.UserId = sender.Id;
        //        await _context.News.AddAsync(news);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    return Error();
        //}

        public IActionResult Messages()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
