using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeCreate.Models;
using Dapper;
using Npgsql;

namespace CodeCreate.PostgreSql
{
    public class PostgreSqlCreate
    {
        private readonly string _connStr;

        public PostgreSqlCreate(string connStr)
        {
            _connStr = connStr;
        }

        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connStr);
        }

        public List<string> GetTabses()
        {
            var sql = @"SELECT tablename FROM pg_tables
                        WHERE tablename NOT LIKE 'pg%'
                        AND tablename NOT LIKE 'sql_%'
                        ORDER BY tablename; ";
            var conn = GetDbConnection();
            var list = conn.Query<string>(sql).ToList();
            return list;
        }
        public List<ColumnInfo> GetColumnInfos(string tableName)
        {
            var coulumnInfoSql = $"SELECT col_description(a.attrelid,a.attnum) as \"Comment\",format_type(a.atttypid,a.atttypmod) as \"ColumnInType\",a.attname as \"Name\", a.attnotnull as \"NotNull\" FROM pg_class as c,pg_attribute as a where c.relname = '{tableName}' and a.attrelid = c.oid and a.attnum > 0";
            var conn = GetDbConnection();
            var list = conn.Query<PostgreColumnInfo>(coulumnInfoSql).ToList();
            var result = list.Where(n=>!n.Name.Contains("..")).Select(n => new ColumnInfo
            {
                ColumnType = GetType(n.ColumnInType),
                IsKey = false,
                IsRequired = n.NotNull,
                Remark = n.Comment == "null" ? null:n.Comment,
                Name = n.Name
            }).ToList();
            var sql = $@"select pg_attribute.attname as colname from 
                        pg_constraint inner join pg_class
                        on pg_constraint.conrelid = pg_class.oid
                        inner join pg_attribute on pg_attribute.attrelid = pg_class.oid
                        and pg_attribute.attnum = pg_constraint.conkey[1]
                        inner join pg_type on pg_type.oid = pg_attribute.atttypid
                        where pg_class.relname = '{tableName}'
                        and pg_constraint.contype = 'p'";
            var t = conn.QueryFirstOrDefault<string>(sql);
            if (!string.IsNullOrEmpty(t))
            {
                var c =  result.FirstOrDefault(n => n.Name == t);
                if(c!=null)c.IsKey = true;
            }


            return result;
        }
        private ColumnType GetType(string str )
        {
            //character varying(255)
            if (str.StartsWith("character"))
            {
                return ColumnType.String;
            }
            // timestamp without time zone
            if (str.StartsWith("time"))
            {
                return ColumnType.Time;
            }
            switch (str)
            {
                case "bigint": return ColumnType.Long;
                case "uuid": return ColumnType.Guid;
                case "integer": return ColumnType.Int;
                case "boolean": return ColumnType.Bool;
                default:return ColumnType.Other;
            }

        }

    }
}
