using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCreate.PostgreSql.Models
{
    internal class PostgreColumnInfo
    {
        public String Comment { get; set; }
        public String Type  { get; set; } 
        public string Name { get; set; }
        public Boolean NotNull { get; set; }
        public Boolean Primarykey { get; set; }
       // public object Default { get; set; }
    }
}
