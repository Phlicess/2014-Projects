using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    internal class UserInfo
    {
        public static Decimal id = 0; //定义C#静态变量  
        public static string name = ""; //定义C#静态变量  
        public static string userName = ""; //定义C#静态变量  
        public static string password = ""; //定义C#静态变量  
        public static string role = ""; //定义C#静态变量  

        public UserInfo()
        {
            id = 0;
            name = "";
            userName = ""; //赋值构造  
            password = "";
            role = "";
        }

        public Decimal ID
        {
            get { return id; } //C#静态变量定义  

            set { id = value; }
        }
        public string Name
        {
            get { return name; } //C#静态变量定义  

            set { name = value; }
        }
        public string UserName
        {
            get { return userName; } //C#静态变量定义  

            set { userName = value; }
        }
        public string Password
        {
            get { return password; } //C#静态变量定义  

            set { password = value; }
        }
        public string Role
        {
            get { return role; } //C#静态变量定义  

            set { role = value; }
        }

    }
}
