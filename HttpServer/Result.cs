using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpServer
{
    public class Result
    {
        #region Model
        private string _state;
        private string _message;
        /// <summary>
        /// 状态
        /// </summary>
        public string state
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 信息
        /// </summary>
        public string message
        {
            set { _message = value; }
            get { return _message; }
        }
        #endregion Model
    }
}
