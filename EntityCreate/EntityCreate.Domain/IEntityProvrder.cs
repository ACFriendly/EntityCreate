using EntityCreate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCreate.Domain
{ 
    public interface IEntityProvrder
    {
        Boolean CheckConn();
        List<string> GetTabses();

        List<ColumnInfo> GetColumnInfos(string tableName);
    }
}
