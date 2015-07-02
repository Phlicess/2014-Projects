using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.FramWork;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALUser : FramWork.BaseDALSQL<UserInfo>, IUser
    {
        #region 对象实例及构造函数

        public static DALUser Instance
        {
            get
            {
                return new DALUser();
            }
        }
        public DALUser()
            : base("User", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">name</param>
        /// <returns></returns>
        public override bool Delete(object key)
        {
            return false;
        }
    }
}
