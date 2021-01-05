using EntityCreate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCreate.Domain
{
    public interface IEntityCreate 
    {
        string CreateEntity(string tableName, List<ColumnInfo> columnInfos, EntityConfig entityConfig);
    }
}
