using EntityCreate.Domain;
using EntityCreate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Npgsql;
using System.Data;
using System.Linq;
using EntityCreate.PostgreSql.Models;

namespace EntityCreate.PostgreSql
{
    public class PostgreSqlProvrder : IEntityProvrder
    {
        private readonly string _connStr;

        public PostgreSqlProvrder(string connStr)
        {
            _connStr = connStr;
        }
        public bool CheckConn()
        {
            try
            {
                var sql = "select 1";
                using (var conn = GetDbConnection())
                {
                    var t = conn.QueryFirst<int>(sql);
                }
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
           
            

        }

        public List<ColumnInfo> GetColumnInfos(string tableName)
        {
            var sql = $@"
                        SELECT DISTINCT
                            a.attnum as num,
                            a.attname as name,
                            format_type(a.atttypid, a.atttypmod) as type,
                            a.attnotnull as notnull, 
                            com.description as comment,
                            coalesce(i.indisprimary, false) as primarykey,
		                        def.adbin as default
                                --,
                         --def.adsrc as default
                        FROM pg_attribute a
                        JOIN pg_class pgc ON pgc.oid = a.attrelid
                        LEFT JOIN pg_index i ON
                            (pgc.oid = i.indrelid AND i.indkey[0] = a.attnum)
                        LEFT JOIN pg_description com on
                            (pgc.oid = com.objoid AND a.attnum = com.objsubid)
                        LEFT JOIN pg_attrdef def ON
                            (a.attrelid = def.adrelid AND a.attnum = def.adnum)
                        WHERE a.attnum > 0 AND pgc.oid = a.attrelid
                        AND pg_table_is_visible(pgc.oid)
                        AND NOT a.attisdropped
                        AND pgc.relname = '{tableName}'-- Your table name here
                        ORDER BY a.attnum; ";
            using (var conn = GetDbConnection())
            {
                var list = conn.Query<PostgreColumnInfo>(sql).ToList();
                var list2 = conn.Query(sql).ToList();
                var result = list.Where(n => !n.Name.Contains("..")).Select(n=>new ColumnInfo
                {
                    IsPrimarykey = n.Primarykey,
                    Name = n.Name,
                    ColumnType = GetColumnType(n.Type),
                    IsRequired = n.NotNull,
                    Remark = n.Comment,
                   // IsIdentity = (!string.IsNullOrEmpty(n.Default?.ToString()) && (n.Default?.ToString().Contains("location -1")??false)),

                }).ToList();
                return result;
            }


                
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
        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connStr);
        }
        private ColumnType GetColumnType(string str) 
        {
            //character varying(255)
            if (str.StartsWith("character"))
            {
                return ColumnType.String;
            }
            // timestamp without time zone
            if (str.StartsWith("time"))
            {
                return ColumnType.DateTime;
            }
            if (str.StartsWith("numeric"))
            {
                return ColumnType.Decimal;
            }
            switch (str)
            {
                case "bigint": return ColumnType.Long;
                case "uuid": return ColumnType.Guid;
                case "integer": return ColumnType.Int;
                case "boolean": return ColumnType.Bool;
                case "text":return ColumnType.String;
                case "real":return ColumnType.Float;
             

                default: return ColumnType.Other;
            }

        }
    }
}
