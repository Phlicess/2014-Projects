using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.FramWork;
using SCZY.IDAL;

namespace SCZY.BLL
{
    public class User:BaseBLL<UserInfo>
    {
        private IUser userDal;

        public User()
            : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.userDal = (IUser)base.baseDal;
        }

        /// <summary>
        /// 插入函数
        /// </summary>
        /// <param name="obj">要插入的对象</param>
        /// <returns></returns>
        public override bool Insert(UserInfo obj)
        {
            UserInfo info = (UserInfo)obj;

            if (obj.Role == null)
            {
                return false;
            }

            //info.Password = EncryptHelper.ComputeHash(info.Password, info.Name.ToLower());
            return base.Insert(obj);
        }

        public override bool Update(UserInfo obj,object id)
        {
            UserInfo info = (UserInfo)obj;
            //info.Password = EncryptHelper.ComputeHash(info.Password, info.Name.ToLower());
            return base.Update(obj,id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Delete(object id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// 根据名称查找单个对象 
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns></returns>
        public  UserInfo FindByName(string userName)
        {
            UserInfo info = null;
            if (!string.IsNullOrEmpty(userName))
            {
                string condition = string.Format("Name = '{0}'", userName);

                info = this.userDal.FindSingle(condition);
               
            }

            return info;
        }

        /// <summary>
        /// 根据名称查找所有符合条件的对象
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> FindByNames(string userName)
        {
            List<UserInfo> list = new List<UserInfo>();

            if (!string.IsNullOrEmpty(userName))
            {
                string condition = string.Format("Name = '{0}'", userName);

                list = userDal.Find(condition);
            }

            return list;
        } 

    }
}
