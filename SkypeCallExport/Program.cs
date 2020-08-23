using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeCallExport
{
    public class Container
    {
        public DateTime exportDate { get; set; }
        public List<Conversation> conversations { get; set; }

    }
    public class Conversation
    {
        public string id { get; set; }
        public Message[] MessageList { get; set; }
    }

    public class Message
    {
        public DateTimeOffset originalarrivaltime { get; set; }
        public string messagetype { get; set; }
        public string content { get; set; }
    }
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
