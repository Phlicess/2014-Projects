using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SCZY.Common
{
    static class GetParentControl
    {
        /// <summary>
        /// 获取指定Name的父控件
        /// </summary>
        /// <typeparam name="T">要查找的父控件的类型</typeparam>
        /// <param name="obj">当前控件</param>
        /// <param name="name">要查找的父控件的Name</param>
        /// <returns></returns>
        static public T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name || string.IsNullOrEmpty(name)))
                {
                   return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        /// <summary>
        /// 获取指定Name的子控件
        /// </summary>
        /// <typeparam name="T">要查找的子控件的类型</typeparam>
        /// <param name="obj">当前控件</param>
        /// <param name="name">要查找的子控件的Name</param>
        /// <returns></returns>
        static public T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
    }
}
