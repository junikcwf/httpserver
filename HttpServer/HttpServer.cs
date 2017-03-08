using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Net.Http;

namespace HttpServer
{
    internal class HttpServer
    {
        private IPEndPoint _IP;
        private TcpListener _Listeners;
        private volatile bool IsInit = false;
        HashSet<string> Names;

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public HttpServer(string ip, int port, HashSet<string> names)
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
            this._IP = localEP;
            Names = names;
            if (Names == null)
            {
                Names = new HashSet<string>();
            }
            try
            {
                foreach (var item in names)
                {
                    Console.WriteLine(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff：") + "Start Listen Http Socket -> {0}:{1}{2} ", ip, port, item));
                }
                this._Listeners = new TcpListener(IPAddress.Parse(ip), port);
                this._Listeners.Start(5000);
                IsInit = true;
                this.AcceptAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.Dispose();
            }
        }

        private void AcceptAsync()
        {
            try
            {
                this._Listeners.BeginAcceptTcpClient(new AsyncCallback(AcceptAsync_Async), null);
            }
            catch (Exception) { }
        }

        private void AcceptAsync_Async(IAsyncResult iar)
        {
            this.AcceptAsync();
            try
            {
                TcpClient client = this._Listeners.EndAcceptTcpClient(iar);
                //client.Client.IOControl(IOControlCode.KeepAliveValues, KeepAlive(1, 30000, 10000), null);//设置Keep-Alive参数
                var socket = new HttpClient(client);
                //if (socket.http_url.StartsWith("/data/upload3"))
                //{
                //    Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff：") + " RemoteEndPoint：" + client.Client.RemoteEndPoint.ToString());
                //    socket.process();
                //}
                Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff：") + "Create Http Socket Remote Socket LocalEndPoint：" + client.Client.LocalEndPoint + " RemoteEndPoint：" + client.Client.RemoteEndPoint.ToString());
                foreach (var item in Names)
                {
                    if (socket.http_url.StartsWith(item))
                    {
                        try
                        {
                            socket.process();
                            return;
                        }
                        catch (Exception ex){
                            Console.WriteLine("process error:" + ex.Message);
                            break; }
                    }
                }
                //////socket.WriteFailure();
                //socket.Close();
            }
            catch (Exception e) {
                Console.WriteLine("AcceptAsync_Async error:" + e.Message);
            }
        }

        private byte[] KeepAlive(int onOff, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];
            BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (IsInit)
            {
                IsInit = false;
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        /// <param name="flag1"></param>
        protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool flag1)
        {
            if (flag1)
            {
                if (_Listeners != null)
                {
                    try
                    {
                        Console.WriteLine(string.Format("Stop Http Listener -> {0}:{1} ", this.IP.Address.ToString(), this.IP.Port));
                        _Listeners.Stop();
                        _Listeners = null;
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 获取绑定终结点
        /// </summary>
        public IPEndPoint IP { get { return this._IP; } }
    }
}
