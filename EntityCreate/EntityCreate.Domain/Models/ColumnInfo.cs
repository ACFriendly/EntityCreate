using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCreate.Domain.Models
{
    public class ColumnInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ColumnType ColumnType { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public Boolean IsPrimarykey { get; set; }
        public Boolean IsIdentity { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public Boolean IsRequired { get; set; }
        public string Remark { get; set; }
        public string Name { get; set; }

    }
}
