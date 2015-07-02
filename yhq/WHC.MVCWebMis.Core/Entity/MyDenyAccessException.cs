using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 自定义拒绝访问异常
    /// </summary>
    public class MyDenyAccessException : UnauthorizedAccessException
    {
        public MyDenyAccessException(string message) : base(message) 
        {
        }

    }
}
