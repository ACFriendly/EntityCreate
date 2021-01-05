using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCreate.PostgreSql
{
    public class PostgreColumnInfo
    {
        public String Comment { get; set; }
        public String ColumnInType { get; set; }
        public string Name { get; set; }
        public Boolean NotNull { get; set; }
    }
}
