using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HttpServer
{
    class MessagePool : ISocketPool
    {
        public void ActiveHttp(HttpClient client, Dictionary<string, string> parms)
        {
            Thread.Sleep(new Random().Next(0, 3000));
//            foreach (var item in parms)
//            {
//                Console.WriteLine(DateTime.Now.ToString() + "item.Key：" + item.Key + "； item.Value：" + item.Value);
//            }
//            string strHtml = @"
//<html><head></head>
//<body>
//<div>&nbsp;</div>
//<div>&nbsp;</div>
//<div>&nbsp;</div>
//<div>&nbsp;</div>
//<div>&nbsp;</div>
//{0}
//</body>
//</html>
//";
            //client.OutputStream.WriteLine(string.Format(strHtml, DateTime.Now.ToString() + "xxxxxxxxxxx"));
            client.OutputStream.Write("ok");
            client.Close();
        }
    }
}
