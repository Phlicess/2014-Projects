using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WHC.Framework.Commons
{
    /// <summary>
    /// ���������¼�ֵ��ֵ���࣬����Comobox�ȿؼ������ֵ����
    /// </summary>
    [Serializable]
    [DataContract]
    public class CListItem
    {
        /// <summary>
        /// ����������CListItem����
        /// </summary>
        /// <param name="text">��ʾ������</param>
        /// <param name="value">ʵ�ʵ�ֵ����</param>
        public CListItem(string text, string value)
        {
            this.text = text;
            this.value = value;
        }

        /// <summary>
        /// ����������CListItem����
        /// </summary>
        /// <param name="text">��ʾ������</param>
        public CListItem(string text)
        {
            this.text = text;
            this.value = text;
        }

        private string text;
        private string value;

        /// <summary>
        /// ��ʾ����
        /// </summary>
        [DataMember]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// ʵ��ֵ����
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// ������ʾ������
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Text.ToString();
        }

    }
}
