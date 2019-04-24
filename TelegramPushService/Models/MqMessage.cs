using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramPushService.Models
{
    public class MqMessage
    {
        public string Content { get; set; }
        public List<int> Receivers { get; set; }
    }
}
