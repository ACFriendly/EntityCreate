using System;
using System.Collections.Generic;
using System.Text;

namespace EntityCreate.Domain.Models
{
    public enum ColumnType
    {
        Guid = 0,
        String = 1,
        Int = 2,
        Long =3,
        DateTime =4,
        Bool = 5,
        Decimal = 6,
        Double = 7,
        Float = 8,
        Other = 99
    }
}
