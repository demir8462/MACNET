using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
namespace TempCounter
{
    static class Program
    {
        public static String value = "";
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            try
            {
                value = Registry.CurrentUser.GetValue("MACNET").ToString();
                if(value == "STARTED_ONCE")
                {

                }
                else
                {
                    Registry.CurrentUser.SetValue("MACNET","STARTED_ONCE");
                }
            }catch(Exception EE)
            {
                Registry.CurrentUser.SetValue("MACNET", "STARTED_ONCE");
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
