using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCreate.Models
{
    public enum ColumnType
    {
        Guid = 0,
        String = 1,
        Int = 2,
        Long =3,
        Time =4,
        Bool = 5,
        Other = 99
    }
    public  class ColumnInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ColumnType ColumnType { get; set; }
       
        /// <summary>
        /// 是否主键
        /// </summary>
        public Boolean IsKey { get; set; }
        /// <summary>
        /// 是否可空
        /// </summary>
        public Boolean IsRequired { get; set; }  
        public string Remark { get; set; } 
        public string Name { get; set; }

    }
}
