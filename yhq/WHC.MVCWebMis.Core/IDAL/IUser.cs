using System.Collections;
using WHC.MVCWebMis.Entity;
using System.Collections.Generic;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    public interface IUser : IBaseDAL<UserInfo>
	{
        List<SimpleUserInfo> GetSimpleUsers();
        List<SimpleUserInfo> GetSimpleUsers(string userIDs);
        List<SimpleUserInfo> GetSimpleUsersByOU(int ouID);
        List<SimpleUserInfo> GetSimpleUsersByRole(int roleID);
        List<UserInfo> GetUsersByOU(int ouID);
        List<UserInfo> GetUsersByRole(int roleID);

        /// <summary>
        /// 根据个人图片枚举类型获取图片数据
        /// </summary>
        /// <param name="imagetype">图片枚举类型</param>
        /// <returns></returns>
        byte[] GetPersonImageBytes(UserImageType imagetype, int userId);

        /// <summary>
        /// 更新个人相关图片数据
        /// </summary>
        /// <param name="imagetype">图片类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns></returns>
        bool UpdatePersonImageBytes(UserImageType imagetype, int userId, byte[] imageBytes);
	}
}