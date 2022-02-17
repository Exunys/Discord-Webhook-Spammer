using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webhook_Spammer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Application.Run(new Form1());
            }
            catch
            {
                MessageBox.Show("Too much trolling, the program crashed...", "Webhook Troller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Run(new Form1());
            }
        }
    }
}
