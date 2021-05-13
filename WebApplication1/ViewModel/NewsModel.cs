using System;
using System.Collections.Generic;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.ViewModel
{
    public class NewsModel
    {
        private List<News> messages;
        private News msg;

        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime dateTime { get; set; }
        public string UserName { get; set; }

        public NewsModel(List<News> messages)
        {
            this.messages = messages;
        }
        public NewsModel(){ }

        public NewsModel(News msg)
        {
            this.msg = msg;
        }
    }
}
