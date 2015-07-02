using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WHC.Framework.Commons;

namespace WHC.MVCWebMis.Controllers
{
    public static class ExtensionMethed
    {
        public static SearchCondition AddNumberCondition(this SearchCondition searchCondition, string fieldName, string fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                string[] itemArray = fieldValue.Split('~');
                if (itemArray != null)
                {
                    decimal value = 0M;
                    bool result = false;
                    if (itemArray.Length == 1)
                    {
                        result = decimal.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.Equal);
                        }
                    }
                    else
                    {
                        if (itemArray.Length > 0)
                        {
                            result = decimal.TryParse(itemArray[0].Trim(), out value);
                            if (result)
                            {
                                searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                            }
                        }
                        if (itemArray.Length > 1)
                        {
                            result = decimal.TryParse(itemArray[1].Trim(), out value);
                            if (result)
                            {
                                searchCondition.AddCondition(fieldName, value, SqlOperator.LessThanOrEqual);
                            }
                        }
                    }

                    /* old code
                    if (itemArray.Length > 0)
                    {
                        result = decimal.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                        }
                    }
                    if (itemArray.Length > 1)
                    {
                        result = decimal.TryParse(itemArray[1].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.LessThanOrEqual);
                        }
                    }
                     */
                }
            }
            return searchCondition;
        }

        public static SearchCondition AddDateCondition(this SearchCondition searchCondition, string fieldName, string fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                string[] itemArray = fieldValue.Split('~');
                if (itemArray != null)
                {
                    DateTime value;
                    bool result = false;


                    //wangyuting
                    if (itemArray.Length == 1)
                    {
                        result = DateTime.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.Equal);
                        }
                    }
                    else
                    {
                        if (itemArray.Length > 0)
                        {
                            result = DateTime.TryParse(itemArray[0].Trim(), out value);
                            if (result)
                            {
                                searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                            }
                        }
                        if (itemArray.Length > 1)
                        {
                            result = DateTime.TryParse(itemArray[1].Trim(), out value);
                            if (result)
                            {
                                searchCondition.AddCondition(fieldName, value.AddDays(1), SqlOperator.LessThan);
                            }
                        }
                    }

                    /* old code
                    if (itemArray.Length > 0)
                    {
                        result = DateTime.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                        }
                    }
                    if (itemArray.Length > 1)
                    {
                        result = DateTime.TryParse(itemArray[1].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value.AddDays(1), SqlOperator.LessThan);
                        }
                    }
                    */
                }
            }
            return searchCondition;
        }

    }
}