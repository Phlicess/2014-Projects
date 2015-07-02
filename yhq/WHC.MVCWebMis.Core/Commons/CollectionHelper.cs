using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WHC.Framework.Commons
{
    public class CollectionHelper<T> where T : class
    {
        public static List<T> Fill(object pID, int level, List<T> list, string pidName, string idName, string name)
        {           
            List<T> returnList = new List<T>();
            foreach (T obj in list)
            {
                string typePID = ReflectionUtil.GetProperty(obj, pidName).ToString();
                string typeID = ReflectionUtil.GetProperty(obj, idName).ToString();
                string typeName = ReflectionUtil.GetProperty(obj, name) as string;

                if (pID.ToString() == typePID)
                {
                    string newName = new string('　', level * 2) + typeName;
                    ReflectionUtil.SetProperty(obj, name, newName);
                    returnList.Add(obj);

                    returnList.AddRange(Fill(typeID, level + 1, list, pidName, idName, name));
                }
            }
            return returnList;
        }
    }

    public sealed class ReflectionUtil
    {
        private ReflectionUtil()
        { }

        public static BindingFlags bf = BindingFlags.DeclaredOnly | BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void SetProperty(object obj, string name, object value)
        {
            PropertyInfo fi = obj.GetType().GetProperty(name, bf);
            fi.SetValue(obj, value, null);
        }

        public static object GetProperty(object obj, string name)
        {
            PropertyInfo fi = obj.GetType().GetProperty(name, bf);
            return fi.GetValue(obj, null);
        }
    }
}
