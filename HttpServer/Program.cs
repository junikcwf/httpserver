using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Web;
using System.Globalization;

namespace HttpServer
{
    class Program
    {
        //static AsyncTcpServer server;

        //static void Main(string[] args)
        //{

        //    server = new AsyncTcpServer(9999);
        //    server.Encoding = Encoding.UTF8;
        //    server.ClientConnected +=
        //      new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
        //    server.ClientDisconnected +=
        //      new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
        //    server.PlaintextReceived +=
        //      new EventHandler<TcpDatagramReceivedEventArgs<string>>(server_PlaintextReceived);
        //    server.Start();

        //    Console.WriteLine("TCP server has been started.");
        //    Console.WriteLine("Type something to send to client...");
        //    while (true)
        //    {
        //        string text = Console.ReadLine();
        //        server.SendAll(text);
        //    }
        //}
 
     //static void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
     //{
     //    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, 
     //    "TCP client {0} has connected.", 
     //    e.TcpClient.Client.RemoteEndPoint.ToString()));
     //}
 
     //static void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
     //{
     //    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, 
     //    "TCP client {0} has disconnected.", 
     //    e.TcpClient.Client.RemoteEndPoint.ToString()));
     //}

     //static void server_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
     //{
     //    if (e.Datagram != "Received")
     //    {
     //        Console.Write(string.Format("Client : {0} --> ",
     //          e.TcpClient.Client.RemoteEndPoint.ToString()));
     //        Console.WriteLine(string.Format("{0}", e.Datagram));
     //        server.Send(e.TcpClient, "Server has received you text : " + e.Datagram);
     //    }
     //}

        public static MessagePool Pool = new MessagePool();
        static void Main(string[] args)
        {
            #region 解析报文例子
            //string temp = "a0e453eeb242?5ccf7fd4775bxxxxc0188527e787x20c9d0dae9b7x0c826857bb82Fxxxx84a1342a5bb3GHIHJd0fccc9e21bc554683c91570c40e5xxxxK0090a2cc2839x5ccf7fd4775b	xxxc4072ff34125xf4ec3801283b87987381c4a0213e4xx68a3c4be1977RxxxRd4612e3834ddWf4ec3801283b87A0c8268580dc2x3c46d84f0737xxxxx5ccf7fd4775b!c0188527e787Of4ec3801283bxxxxx5ccf7fd4775b";
            //byte[] dataBytes = System.Text.Encoding.ASCII.GetBytes(temp);
            //if (dataBytes != null && dataBytes.Length > 0)
            //{
            //    byte splitAscii = new byte();
            //    string splitReg = "";
            //    if (dataBytes.Length > 13)
            //    {
            //        splitReg = ((char)dataBytes[0]).ToString();
            //        splitAscii = dataBytes[0];
            //    }
            //    Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
            //    List<string> macList = new List<string>();
            //    List<int> splitIndexList = new List<int>();
            //    if (splitAscii.ToString() == "1" || splitReg == " ")
            //    {
            //        for (int i = 0; i < dataBytes.Length; i++)
            //        {
            //            if (splitAscii == dataBytes[i])
            //            {
            //                splitIndexList.Add(i + 1);
            //            }
            //        }
            //        int[] indexArr = splitIndexList.ToArray();
            //        StringBuilder sb = new StringBuilder();
            //        for (int i = 0; i < indexArr.Length; )
            //        {
            //            int start = indexArr[i];
            //            int end = indexArr[i + 1];
            //            for (; start < end; start++)
            //            {
            //                sb.Append(((char)dataBytes[start]).ToString());
            //            }
            //            macList.Add(sb.ToString().Trim());
            //            //Console.WriteLine("macdata:" + sb.ToString());
            //            sb = new StringBuilder();
            //            i = i + 1;
            //        }
            //        foreach (string macrssi in macList)
            //        {
            //            string mac = macrssi.Substring(0, 12).ToUpper();
            //            mac = getMacAddrWithFormat(mac, ":");
            //            List<int> rssiList = new List<int>();
            //            for (int j = 12; j < macrssi.Length; j++)
            //            {
            //                int rssi = macrssi[j];
            //                if (rssi > 9 && rssi < 99 && !rssiList.Contains(rssi))
            //                {
            //                    rssiList.Add(rssi);
            //                }
            //            }
            //            if (!macRssiDic.ContainsKey(mac))
            //            {
            //                macRssiDic.Add(mac, rssiList);
            //            }
            //        }

            //    }
            //}
            #endregion

            //string result = "{\"state\":\"0000\",\"message\":\"ok\"}";
            //Result resultJson = JsonHelper.JsonDeserialize<Result>(result);



            //HttpServer httpss = new HttpServer("172.17.1.123", 7000, new HashSet<string>() { "/data/upload3" });
            //Console.ReadLine();

            //while (true)
            //{
            //TestHttp test = new TestHttp();
            //string temp = Console.ReadLine();
            //}

            #region HttpListener 作为http服务器，OK
            //try
            //{
            //    HttpListener listerner = new HttpListener();
            //    {
            //        for (; true; )
            //        {
            //            try
            //            {
            //                Console.Write("请输入服务器IP地址:");
            //                string ip = Console.ReadLine();

            //                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
            //                listerner.Prefixes.Add("http://" + ip + ":7000/data/upload3/");

            //                // listerner.Prefixes.Add("http://localhost/web/");
            //                listerner.Start();
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine("未能成功连接服务器.....");
            //                listerner = new HttpListener();
            //                continue;
            //            }
            //            break;
            //        }
            //        Console.WriteLine("服务器启动成功.......");

            //        int maxThreadNum, portThreadNum;

            //        //线程池
            //        int minThreadNum;
            //        ThreadPool.GetMaxThreads(out maxThreadNum, out portThreadNum);
            //        ThreadPool.GetMinThreads(out minThreadNum, out portThreadNum);
            //        Console.WriteLine("最大线程数：{0}", maxThreadNum);
            //        Console.WriteLine("最小空闲线程数：{0}", minThreadNum);


            //        //ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc1), x);

            //        Console.WriteLine("\n\n等待客户连接中。。。。");
            //        while (true)
            //        {
            //            //等待请求连接
            //            //没有请求则GetContext处于阻塞状态
            //            HttpListenerContext ctx = listerner.GetContext();

            //            ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
            //        }
            //        //listerner.Stop();
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    Console.Write("Press any key to continue . . . ");
            //    Console.ReadKey();
            //}
            #endregion

        }
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return macRssiDic;
        }
        public static Dictionary<string, List<int>> GetMacRssiDic(string data)
        {
            Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
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
                            splitIndexList.Add(i + 1);
                        }
                    }
                    int[] indexArr = splitIndexList.ToArray();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < indexArr.Length; )
                    {
                        int start = indexArr[i];
                        int end = indexArr[i + 1];
                        for (; start < end; start++)
                        {
                            sb.Append(((char)dataBytes[start]).ToString());
                        }
                        macList.Add(sb.ToString().Trim());
                        //Console.WriteLine("macdata:" + sb.ToString());
                        sb = new StringBuilder();
                        i = i + 2;
                    }
                    foreach (string macrssi in macList)
                    {
                        string mac = macrssi.Substring(0, 12).ToUpper();
                        mac = getMacAddrWithFormat(mac, ":");
                        List<int> rssiList = new List<int>();
                        for (int j = 12; j < macrssi.Length; j++)
                        {
                            int rssi = macrssi[j];
                            if (rssi > 9 && rssi < 99 && !rssiList.Contains(rssi))
                            {
                                rssiList.Add(rssi);
                            }
                        }
                        if (!macRssiDic.ContainsKey(mac))
                        {
                            macRssiDic.Add(mac, rssiList);
                        }
                    }

                }
            }

            return macRssiDic;
        }
        
        static void TaskProc(object o)
        {
            HttpListenerContext ctx = (HttpListenerContext)o;

            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码

            Stream reqStream = ctx.Request.InputStream;

            System.IO.Stream body = ctx.Request.InputStream;
            System.Text.Encoding encoding = ctx.Request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            if (ctx.Request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", ctx.Request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", ctx.Request.ContentLength64);

            Console.WriteLine("Start of client data:");
            string receiptData = reader.ReadToEnd();
            //sta=5ccf7fd4775b&shop=1&token=123232jd&r=70658221bf82&id=069b63be&type=probea&data=5ccf7f26f060BH74de2bdb8d24EC009569a7d3a9\68a3c4be22f0xxxxxf4ec3801283bxf4ec380cb4b9>=D5ccf7fd4775bxx74de2bdb8d24xAS;;68a3c4be22f0CE34xbcf6854936f2]5ccf7fd4775b74de2bdb8d249><>@20ab3768b12dxxxxx68a3c4be22f0xxxe02cb27692dbx7423448eb362xdc85de323707x5ccf7fd4775bx74de2bdb8d24xxxxxe02cb27692dbx18dc56d008f8xxxxf4ec3801283bx5ccf7fd4775bxxxdc85de323707x5ccf7fd4775bxxxe4029bec3bc3x5ccf7fd4775bxxxxx1c872cb6409dxxxx5ccf7fd4775bx8c8401db0274;c8f230439c58xx683e349acb07^xxxxc4f0811b3029xxxxx9cb70d5bb146>>5ccf7fd46b7fB5ccf7fd4775bxxxxx5ccf7fd46b7fIKd4a1484d07e8xxxxx5ccf7fd4eb0f455ccf7fd46b7fK5ccf7fd4775b74de2bdb8d244344314f65ac318a6777550023cdae2305xxxxxd037423cd9e1CB5ccf7fd46b7fHF9cb70d66b161.5ccf7fd4775b0c826857771dB6c5c143f13f1x74de2bdb8d24535674de2bdb8d249:;<d4612e3834ddQc09f05b234bc`5ccf7fd4775bxxxxx5ccf7f26f060=74de2bdb8d24G009569a7d3a9xxbcf6854936f2^^b8ee65d0bf7exd07e35e0aedeRSxxx009569a7d3a9x683e349acb07xx[e4d3325c3bc6S
            Console.WriteLine(receiptData);
            Console.WriteLine("End of client data:");
            body.Close();
            reader.Close();

            string apMac = "", probeMac = "", httpType = "", strData = "";
            Dictionary<string, string> rets = new Dictionary<string, string>();
            Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
            string[] splitData = receiptData.Split('&');
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

            //进行处理

            //使用Writer输出http响应代码
            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
            {
                writer.Write("ok");
                writer.Close();
                ctx.Response.Close();
            }
        }
    }

    public class TestHttp
    {
        THttpListener _HttpListener;
        public TestHttp()
        {
            //string[] strUrl = new string[] { "http://+:7000/data/upload3/" };
            string[] strUrl = new string[] { "http://*:7000/data/upload3/" };
            _HttpListener = new THttpListener(strUrl);
            _HttpListener.ResponseEvent += TaskProc;
            _HttpListener.Start();
        }

        void TaskProc(System.Net.HttpListenerContext ctx)
        {
          ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
          if (ctx.Request.RawUrl.Contains("data/upload3"))
          {
              Stream reqStream = ctx.Request.InputStream;

              System.IO.Stream body = ctx.Request.InputStream;
              System.Text.Encoding encoding = ctx.Request.ContentEncoding;
              System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
              if (ctx.Request.ContentType != null)
              {
                  //Console.WriteLine("Client data content type {0}", ctx.Request.ContentType);
              }
              //Console.WriteLine("Client data content length {0}", ctx.Request.ContentLength64);

              //Console.WriteLine("Start of client data:");
              string receiptData = reader.ReadToEnd();
              //sta=5ccf7fd4775b&shop=1&token=123232jd&r=70658221bf82&id=069b63be&type=probea&data=5ccf7f26f060BH74de2bdb8d24EC009569a7d3a9\68a3c4be22f0xxxxxf4ec3801283bxf4ec380cb4b9>=D5ccf7fd4775bxx74de2bdb8d24xAS;;68a3c4be22f0CE34xbcf6854936f2]5ccf7fd4775b74de2bdb8d249><>@20ab3768b12dxxxxx68a3c4be22f0xxxe02cb27692dbx7423448eb362xdc85de323707x5ccf7fd4775bx74de2bdb8d24xxxxxe02cb27692dbx18dc56d008f8xxxxf4ec3801283bx5ccf7fd4775bxxxdc85de323707x5ccf7fd4775bxxxe4029bec3bc3x5ccf7fd4775bxxxxx1c872cb6409dxxxx5ccf7fd4775bx8c8401db0274;c8f230439c58xx683e349acb07^xxxxc4f0811b3029xxxxx9cb70d5bb146>>5ccf7fd46b7fB5ccf7fd4775bxxxxx5ccf7fd46b7fIKd4a1484d07e8xxxxx5ccf7fd4eb0f455ccf7fd46b7fK5ccf7fd4775b74de2bdb8d244344314f65ac318a6777550023cdae2305xxxxxd037423cd9e1CB5ccf7fd46b7fHF9cb70d66b161.5ccf7fd4775b0c826857771dB6c5c143f13f1x74de2bdb8d24535674de2bdb8d249:;<d4612e3834ddQc09f05b234bc`5ccf7fd4775bxxxxx5ccf7f26f060=74de2bdb8d24G009569a7d3a9xxbcf6854936f2^^b8ee65d0bf7exd07e35e0aedeRSxxx009569a7d3a9x683e349acb07xx[e4d3325c3bc6S
              //Console.WriteLine(receiptData);
              //Console.WriteLine("End of client data:");
              Console.WriteLine("IP:" + ctx.Request.RemoteEndPoint.ToString() + ";;;Time:" + DateTime.Now);
              body.Close();
              reader.Close();

              //byte[] buffer = new byte[(int)reqStream.Length];
              //reqStream.Read(buffer, 0, (int)reqStream.Length);
              //string req = System.Text.Encoding.Default.GetString(buffer);
              //Console.WriteLine("" + req);
              string apMac = "", probeMac = "", httpType = "", strData = "";
              Dictionary<string, string> rets = new Dictionary<string, string>();
              Dictionary<string, List<int>> macRssiDic = new Dictionary<string, List<int>>();
              string[] splitData = receiptData.Split('&');
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

              //string delimiter = "\\1";
              //if (strData != "")
              //{
              //    Console.WriteLine("data =" + strData.ToString());

              //    int startindex = strData.IndexOf(delimiter);
              //    if (startindex != -1)
              //    {
              //        int endindex = strData.LastIndexOf(delimiter);
              //        if (endindex - startindex - 1 > 12)
              //        {
              //            string datatrimed = strData.Substring(startindex + 1, endindex);
              //            string[] deli = new string[1];
              //            deli[0] = delimiter;
              //            string[] splitMacData = datatrimed.Split(deli, StringSplitOptions.RemoveEmptyEntries);
              //            for (int i = 0; i < splitMacData.Length; i++)
              //            {
              //                if (splitMacData[i].Length > 12)
              //                {
              //                    string mac = splitMacData[i].Substring(0, 12);
              //                    byte[] dataspliebytes = System.Text.Encoding.Default.GetBytes(splitMacData[i]);
              //                    for (int j = 12; j < dataspliebytes.Length; j++)
              //                    {
              //                        int rssi = dataspliebytes[j];
              //                        if (rssi > 9 && rssi < 99)
              //                        {
              //                        }
              //                    }
              //                }
              //            }
              //        }
              //    }
              //}

              //进行处理

              //使用Writer输出http响应代码
              using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
              {
                  writer.Write("ok");
                  writer.Close();
                  ctx.Response.Close();
              }
              reqStream.Close();
          }
        }

        void _HttpListener_ResponseEvent(System.Net.HttpListenerContext ctx)
        {
            //直接获取数据
            Dictionary<string, string> rets = _HttpListener.getData(ctx);
            //获取get数据
            Dictionary<string, string> retGets = _HttpListener.getData(ctx, THttpListener.DataType.Get);

            //获取post数据
            Dictionary<string, string> retPosts = _HttpListener.getData(ctx, THttpListener.DataType.Post);
            ResponseWrite(ctx.Request.AcceptTypes[0], "Ret", ctx.Response);
        }

        static void ResponseWrite(string type, string msg, System.Net.HttpListenerResponse response)
        {
            //使用Writer输出http响应代码
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(response.OutputStream, new UTF8Encoding()))
            {
                response.ContentType = type + ";charset=utf-8";
                writer.WriteLine(msg);
                writer.Close();
                response.Close();
            }
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return macRssiDic;
        }

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

    }
}
