using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 用户登录日志信息
    /// </summary>
	public class LoginLog : BaseBLL<LoginLogInfo>
    {
        public LoginLog() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 记录用户操作日志
        /// </summary>
        /// <param name="info">用户信息</param>
        /// <param name="systemType">系统类型ID</param>
        /// <param name="ip">IP地址</param>
        /// <param name="macAddr">Mac地址</param>
        /// <param name="note">备注说明</param>
        public void AddLoginLog(UserInfo info, string systemType, string ip, string macAddr, string note)
        {
            if (info == null) return;

            #region 记录用户登录操作
            try
            {
                LoginLogInfo logInfo = new LoginLogInfo();
                logInfo.IPAddress = ip;
                logInfo.MacAddress = macAddr;
                logInfo.LastUpdated = DateTime.Now;
                logInfo.Note = note;
                logInfo.SystemType_ID = systemType;

                logInfo.User_ID = info.ID.ToString();
                logInfo.FullName = info.FullName;
                logInfo.LoginName = info.Name;

                BLLFactory<LoginLog>.Instance.Insert(logInfo);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
            }
            #endregion
        }

        /// <summary>
        /// 根据最后更新日前的数据获取数据
        /// </summary>
        /// <param name="shopGuid">商店GUID</param>
        /// <param name="LastUpdated">最后更新日前</param>
        /// <returns></returns>
        public List<LoginLogInfo> GetList(DateTime LastUpdated)
        {
            SearchCondition search = new SearchCondition();
            search.AddCondition("LastUpdated", LastUpdated, SqlOperator.MoreThanOrEqual);
            string condition = search.BuildConditionSql().Replace("Where", "");
            return Find(condition);
        }

        /// <summary>
        /// 如果目标不存在则插入，否则判断更新时间，如果目标较旧则更新
        /// </summary>
        /// <param name="infoList"></param>
        public void InsertOrUpdate(List<LoginLogInfo> infoList)
        {
            if (infoList != null && infoList.Count > 0)
            {
                foreach (LoginLogInfo info in infoList)
                {
                    LoginLogInfo tempInfo = baseDal.FindByID(info.ID);
                    if (tempInfo != null)
                    {
                        if (tempInfo.LastUpdated < info.LastUpdated)
                        {
                            baseDal.Update(info, info.ID.ToString());
                        }
                    }
                    else
                    {
                        baseDal.Insert(info);
                    }
                }
            }
        }

        /// <summary>
        /// 删除一个月前的数据
        /// </summary>
        public void DeleteMonthLog()
        {
            SearchCondition search = new SearchCondition();
            search.AddCondition("LastUpdated", DateTime.Now.AddDays(-30), SqlOperator.LessThanOrEqual);
            string condition = search.BuildConditionSql().Replace("Where", "");
            baseDal.DeleteByCondition(condition);
        }

        /// <summary>
        /// 获取上一次（非刚刚登录）的登录日志
        /// </summary>
        /// <param name="userId">登录用户ID</param>
        /// <returns></returns>
        public LoginLogInfo GetLastLoginInfo(string userId)
        {
            ILoginLog loginDal = baseDal as ILoginLog;
            return loginDal.GetLastLoginInfo(userId);
        }
    }
}
