﻿using EntityCreate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCreate.Domain.EntityCreate
{
    public class AbpEntityCreate : IEntityCreate
    {
      
        public string CreateEntity(string tableName, List<ColumnInfo> columnInfos, EntityConfig entityConfig)
        {
         
            var keyColumn = GetKey(columnInfos);
            var usingStr = @"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;";
            var stringbuilder = new StringBuilder();
            stringbuilder.Append(usingStr);
            stringbuilder.Append("\n\n");
            stringbuilder.AppendLine($"namespace {entityConfig.Namespace}");
            stringbuilder.AppendLine("{");
            stringbuilder.AppendLine($"\tpublic class {tableName}:Entity<{GetColumnTypeStr(keyColumn)}>");
            stringbuilder.AppendLine("\t{");
            foreach (var item in columnInfos)
            {
                GetColumnStr(item, stringbuilder);
            }
            stringbuilder.AppendLine("\t}\r\n}");
            return stringbuilder.ToString();
        }
        private string GetColumnTypeStr(ColumnInfo columnInfo)
        {
            if (columnInfo == null)
            {
                return string.Empty;
            }
            bool isStuct = false;
            var str = "";
            switch (columnInfo.ColumnType)
            {
                case ColumnType.Bool:
                    {
                        str = "bool";
                        isStuct = true;
                    }
                    break;
                case ColumnType.Guid:
                    {
                        str = "Guid";
                        isStuct = true;
                    }
                    break;
                case ColumnType.Int:
                    {
                        str = "int";
                        isStuct = true;
                    }
                    break;
                case ColumnType.Long:
                    {
                        str = "long";
                        isStuct = true;
                    }
                    break;
                case ColumnType.String:
                    {
                        str = "String";
                        isStuct = false;
                    }
                    break;
                case ColumnType.DateTime:
                    {
                        str = "DateTime";
                        isStuct = true;
                    }
                    break;
                default:
                    {
                        str = "Other";
                        break;
                    }

            }

            if (!columnInfo.IsRequired && isStuct)
            {
                return $"{str}?";
            }
            else
            {
                return str;

            }
        }

        private ColumnInfo GetKey(List<ColumnInfo> columnInfos)
        {
            return columnInfos.FirstOrDefault(n => n.IsPrimarykey == true);
        }
        private void GetColumnStr(ColumnInfo columnInfo, StringBuilder builder)
        {
            if (columnInfo.IsPrimarykey &&
                (columnInfo.ColumnType != ColumnType.Long && columnInfo.ColumnType != ColumnType.Int))
            {
                return;
            }
            var typeStr = GetColumnTypeStr(columnInfo);
            builder.AppendLine("\r\n\t\t/// <summary>");
            builder.AppendLine($"\t\t/// {columnInfo.Remark}");
            builder.AppendLine("\t\t/// <summary>");
            if (columnInfo.IsPrimarykey && (columnInfo.ColumnType == ColumnType.Long || columnInfo.ColumnType == ColumnType.Int))
            {
                builder.AppendLine("\t\t[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                builder.AppendLine($"\t\tpublic override {typeStr} {columnInfo.Name} {{get;protected set;}}");
            }
            else
            {
                builder.AppendLine($"\t\tpublic {typeStr} {columnInfo.Name} {{get; set;}}");
            }

        }

      
    }
}
