using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;
using System.Collections;
using System.Net.Sockets;

namespace HttpServer
{
    public class HttpClient
    {
        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB
        private const int BUF_SIZE = 4096;
        private Stream inputStream;
        public StreamWriter OutputStream;
        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();
        internal TcpClient _Socket;

        /// <summary>
        /// 这个是服务器收到有效链接初始化
        /// </summary>
        internal HttpClient(TcpClient client)
        {
            this._Socket = client;
            inputStream = new BufferedStream(_Socket.GetStream());
            OutputStream = new StreamWriter(new BufferedStream(_Socket.GetStream()), UTF8Encoding.Default);
            ParseRequest();
        }

        internal void process()
        {
            try
            {
                if (http_method.Equals("GET"))
                {
                    //Program.Pool.ActiveHttp(this, GetRequestExec());
                    GetRequestExec();
                }
                else if (http_method.Equals("POST"))
                {
                    Program.Pool.ActiveHttp(this, PostRequestExec());
                    //PostRequestExec();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                //WriteFailure();
            }
        }

        public void Close()
        {
            OutputStream.Flush();
            inputStream.Dispose();
            inputStream = null;
            OutputStream.Dispose();
            OutputStream = null; // bs = null;            
            this._Socket.Close();
        }

        #region 读取流的一行 private string ReadLine()
        /// <summary>
        /// 读取流的一行
        /// </summary>
        /// <returns></returns>
        private string ReadLine()
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = this.inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        #endregion

        #region 转化出 Request private void ParseRequest()
        /// <summary>
        /// 转化出 Request
        /// </summary>
        private void ParseRequest()
        {
            String request = ReadLine();
            if (request != null)
            {
                string[] tokens = request.Split(' ');
                if (tokens.Length != 3)
                {
                    throw new Exception("invalid http request line");
                }
                http_method = tokens[0].ToUpper();
                http_url = tokens[1];
                http_protocol_versionstring = tokens[2];
            }
            String line;
            while ((line = ReadLine()) != null)
            {
                if (line.Equals(""))
                {
                    break;
                }
                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;//过滤键值对的空格
                }
                string value = line.Substring(pos, line.Length - pos);
                httpHeaders[name] = value;
            }
        }
        #endregion

        #region 读取Get数据 private Dictionary<string, string> GetRequestExec()
        /// <summary>
        /// 读取Get数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetRequestExec()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int index = http_url.IndexOf("?", 0);
            if (index >= 0)
            {
                string data = http_url.Substring(index + 1);
                datas = getData(data);
            }
            WriteSuccess("");
            return datas;
        }
        #endregion

        #region 读取提交的数据 private void handlePOSTRequest()
        /// <summary>
        /// 读取提交的数据
        /// </summary>
        private Dictionary<string, string> PostRequestExec()
        {
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                //内容的长度
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE) { throw new Exception(String.Format("POST Content-Length({0}) 对于这个简单的服务器太大", content_len)); }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)  
                {
                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0) { break; }
                        else { throw new Exception("client disconnected during post"); }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
           
            StreamReader inputData = new StreamReader(ms);
            string data = inputData.ReadToEnd().Trim();
            string apMac = "", probeMac = "", httpType = "";
            Dictionary<string, string> rets = new Dictionary<string, string>();
            Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
            string[] splitData = data.Split('&');
            if (splitData != null && splitData.Length > 0)
            {
                foreach (string param in splitData)
                {
                    string[] kvPair = param.Split('=');
                    string key = kvPair[0];
                    if (kvPair.Length > 1)
                    {
                        if (key.Equals("data"))
                        {
                            //data内容是ascii码类型，需要进行转换
                            string value = HttpUtility.UrlDecode(kvPair[1]);
                            macRssiDic = GetStaMacRssiDic(value, httpType);
                            Console.WriteLine("macRssiDicCount = " + macRssiDic.Count);
                            //WriteSuccess("");
                        }
                        else
                        {
                            string value = HttpUtility.UrlDecode(kvPair[1]);
                            rets[key] = value;
                            if (key.Equals("sta"))
                            {
                                probeMac = value;
                            }
                            else if (key.Equals("r"))
                            {
                                apMac = value;
                            }
                            else if (key.Equals("type"))
                            {
                                httpType = value;
                            }
                        }
                    }
                }
            }

            WriteSuccess(httpType);
            return rets;
            //return getData(data);
        }
        #endregion

        public static String getMacAddrWithFormat(String mac, String in_split)
        {
            String outMac = "";
            for (int i = 0; i < mac.Length; )
            {
                outMac += mac.Substring(i++, 1);
                if (0 == i % 2 && i < mac.Length)
                {
                    outMac += in_split;
                }
            }
            return outMac;
        }

        public static Dictionary<string, List<int>> GetStaMacRssiDic(string data, string httpType)
        {
            Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
            try
            {
                //string temp = "a41731f4b9bbHEFDG3480b35abbceI9cb2b2c9a5a5MQ1c7b2355018eHGe458b8dd9139Yf4ec3801283b7674;20720d391d4eA@ABBc8f7330a5d4dFD9cb70d66bbd8E86J668a3c42159cfIIaccf85159430MLJJ68a3c4be22f0358";
                byte[] dataBytes = System.Text.Encoding.ASCII.GetBytes(data);
                if (dataBytes != null && dataBytes.Length > 0)
                {
                    byte splitAscii = new byte();
                    string splitReg = "";
                    if (dataBytes.Length > 13)
                    {
                        splitReg = ((char)dataBytes[0]).ToString();
                        splitAscii = dataBytes[0];
                    }

                    List<string> macList = new List<string>();
                    List<int> splitIndexList = new List<int>();
                    if (splitAscii.ToString() == "1" || splitReg == " ")
                    {
                        for (int i = 0; i < dataBytes.Length; i++)
                        {
                            if (splitAscii == dataBytes[i])
                            {
                                splitIndexList.Add(i);
                            }
                        }
                        int[] indexArr = splitIndexList.ToArray();
                        StringBuilder sb = new StringBuilder();
                        //indexArr.Length - 1 
                        for (int i = 0; i < indexArr.Length - 1; i++)
                        {
                            int start = indexArr[i];
                            int end = indexArr[i + 1];
                            if (start < dataBytes.Length && end < dataBytes.Length)
                            {
                                for (; start < end; start++)
                                {
                                    if (!(dataBytes[start] == splitAscii))
                                    {
                                        sb.Append(((char)dataBytes[start]).ToString());
                                    }
                                }
                            }
                            if (sb.ToString().Trim().Length > 12)
                            {
                                macList.Add(sb.ToString().Trim());
                            }
                            sb = new StringBuilder();
                        }
                        foreach (string macrssi in macList)
                        {
                            if (httpType.Equals("unixtime"))
                            {
                                //获取服务器时间
                            }
                            else
                            {
                                string mac = macrssi.Substring(0, 12).ToUpper();
                                mac = getMacAddrWithFormat(mac, ":");
                                List<int> rssiList = new List<int>();
                                if (httpType.Equals("probea") || httpType.Equals("probeb"))
                                {
                                    for (int j = 12; j < macrssi.Length; j++)
                                    {
                                        int rssi = macrssi[j];
                                        if (rssi > 9 && rssi < 99 && !rssiList.Contains(rssi))
                                        {
                                            rssiList.Add(rssi);
                                        }
                                    }
                                }
                                else if (httpType.Equals("flash"))
                                {
                                    //截取mac地址后面的两个字符即可，其他忽略
                                    string rssiHex = macrssi.Substring(12, 2);
                                    int rssi = Convert.ToInt32(rssiHex, 16);
                                    if (rssi > 9 && rssi < 99 && !rssiList.Contains(rssi))
                                    {
                                        rssiList.Add(rssi);
                                    }
                                }
                                else if (httpType.Equals("ap"))
                                {
                                    //周围AP信息
                                    ScanApInfo apinfo = new ScanApInfo();
                                    apinfo.ApMac = mac;
                                    int rssi = macrssi[12];
                                    if (rssi > 9 && rssi < 99 && !rssiList.Contains(rssi))
                                    {
                                        apinfo.Rssi = rssi;
                                    }

                                    int channel = macrssi[13];
                                    channel = channel - 50;
                                    apinfo.Channel = channel;
                                    apinfo.Ssid = macrssi.Substring(14);

                                }
                                else if (httpType.Equals("configa"))
                                {
                                    //WiFi A模块的配置信息
                                }
                                else if (httpType.Equals("check"))
                                {
                                    //WiFi B模块的心跳
                                }
                                else if (httpType.Equals("register"))
                                {
                                    //手机与mac地址的关联信息
                                }
                                else if (httpType.Equals("tag"))
                                {
                                    //商品标签信息
                                }
                                if (!macRssiDic.ContainsKey(mac) && rssiList.Count > 0)
                                {
                                    macRssiDic.Add(mac, rssiList);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return macRssiDic;
        }
        

        #region 输出状态
        /// <summary>
        /// 输出200状态
        /// </summary>
        public void WriteSuccess(string type)
        {
            //OutputStream.WriteLine("HTTP/1.1 200 OK");
            //OutputStream.Write("Content-Type: text/html");
            //OutputStream.Write("Connection: close");
            //if (type.Equals("unixtime"))
            //{
            //    OutputStream.Write("ok-timestart-" + ConvertDateTimeInt(DateTime.Now) + "-timeend");
            //}
            //else
            //{
            //    OutputStream.Write("ok");
            //}
            OutputStream.Write("ok");
            OutputStream.Flush();
            inputStream.Dispose();
            inputStream = null;
            OutputStream.Dispose();
            OutputStream = null; // bs = null; 
        }

        
 

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }
        /// <summary>
        /// 输出状态404
        /// </summary>
        public void WriteFailure()
        {
            //OutputStream.Write("HTTP/1.0 404 File not found");
            //OutputStream.Write("Content-Type: text/html");
            //OutputStream.Write("Connection: close");
            OutputStream.WriteLine("fail");
            OutputStream.Flush();
        }
        #endregion

        /// <summary>
        /// 分析http提交数据分割
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static Dictionary<string, string> getData(string rawData)
        {
            var rets = new Dictionary<string, string>();
            string[] rawParams = rawData.Split('&');
            foreach (string param in rawParams)
            {
                string[] kvPair = param.Split('=');
                string key = kvPair[0];
                string value = HttpUtility.UrlDecode(kvPair[1]);
                rets[key] = value;
            }
            return rets;
        }
    }

    public class ScanApInfo{
        private string apMac;
        private int rssi;
        private int channel;
        private string ssid;

        public int Rssi{
            get{ return rssi;}
            set { rssi = value; }
        }
         public int Channel{
            get{ return channel;}
            set { channel = value; }
        }

         public string Ssid{
            get{ return ssid;}
            set { ssid = value; }
        }
         public string ApMac
         {
             get { return apMac; }
             set { apMac = value; }
         }
    }
}
