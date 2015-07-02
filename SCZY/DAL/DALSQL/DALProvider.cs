using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SCZY.Entity;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALProvider : FramWork.BaseDALSQL<ProviderInfo>, IProvider
    {
        #region 对象实例及构造函数

        public static DALProvider Instance
        {
            get
            {
                return new DALProvider();
            }
        }
        public DALProvider()
            : base("Provider", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 重在删除函数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Delete(object key)
        {

            Database db = CreateDatabase();
            DbCommand command = null;

            DbTransaction transaction = null;

            if (!tableName.Contains("["))
            {
                tableName = "[" + tableName + "]";
            }

            try
            {
                ProviderInfo info = this.FindByID(key);

                if (info != null)
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        DALChanPin_GongYingShang chanPin_GongYingShangDal = new DALChanPin_GongYingShang();

                        string condition = string.Format("Provider_ID = {0}",key);
                        ChanPin_GongYingShangInfo chanPin_GongYingShangInfo = chanPin_GongYingShangDal.FindSingle(condition);

                        if (chanPin_GongYingShangInfo != null)
                        {
                            string commandTextChanPin = string.Format("Delete From {1} Where Provider_ID ={0} ", key,"ChanPin_GongYingShang");
                            command = db.GetSqlStringCommand(commandTextChanPin);
                            db.ExecuteNonQuery(command, transaction);
                        }

                        string commandText = string.Format("Delete From {1} Where ID ={0} ", key, tableName);
                        command = db.GetSqlStringCommand(commandText);
                        db.ExecuteNonQuery(command, transaction);

                        transaction.Commit();
                    }
                }
            }
            catch 
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return true;
        }
    }
}
