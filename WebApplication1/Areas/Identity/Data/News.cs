using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.Identity.Data
{
    public class News
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime dateTime { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public int Like { get; set; }

        public News()
        {
            dateTime = DateTime.Now;
            Like = 0;
        }
    }
}
