using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ProductInfo :BaseEntity
    {
        #region Field Member

        private int m_ID = 0; //产品编号
        private string m_Brand; //产品品牌
        private string m_Texture; //
        private float m_GramWeight;//产品克重
        private float m_Width;//产品宽幅
        private float m_Length;//产品长度
        private float m_Reservation;//产品剩余量
        private string m_Remark; //备注

        #endregion

        #region Property Member

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }
        /// <summary>
        /// 产品品牌
        /// </summary>
        public virtual string Brand
        {
            get { return this.m_Brand; }
            set { this.m_Brand = value; }
        }
        /// <summary>
        /// ......
        /// </summary>
        public virtual string Texture
        {
            get { return this.m_Texture; }
            set { this.m_Texture = value; }
        }
        /// <summary>
        /// 产品克重
        /// </summary>
        public virtual float GramWeight
        {
            get { return this.m_GramWeight; }
            set { this.m_GramWeight = value; }
        }

        /// <summary>
        /// 产品宽幅
        /// </summary>
        public virtual float Width
        {
            get { return this.m_Width; }
            set { this.m_Width = value; }
        }

        /// <summary>
        /// 产品长度
        /// </summary>
        public virtual float Length
        {
            get { return this.m_Length; }
            set { this.m_Length = value; }
        }
        /// <summary>
        /// 产品剩余量
        /// </summary>
        public virtual float Reservation
        {
            get { return this.m_Reservation; }
            set { this.m_Reservation = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }

        //public Product(int m_ID, string m_Brand, string m_Texture, float m_GramWeight, float m_Width, float m_Length,
        //    float m_Reservation, string m_Remark)
        //{
        //    ID = m_ID;
        //    Brand = m_Brand;
        //    Texture = m_Texture;
        //    GramWeight = m_GramWeight;
        //    Width = m_Width;
        //    Length =  m_Length;
        //    Reservation = m_Reservation;
        //    Remark = m_Remark;
        //}

        #endregion

    }
}
