﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpServer
{
    public interface ISocketPool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        void ActiveHttp(HttpClient client, Dictionary<string, string> parms);
    }
}
