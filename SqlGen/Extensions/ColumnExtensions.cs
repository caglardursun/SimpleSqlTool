using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen
{
    public static class ColumnExtensions
    {
        public static bool IsRowVersion(this Column c) => string.Equals("timestamp", c.DataType, StringComparison.OrdinalIgnoreCase) || string.Equals("rowversion", c.DataType, StringComparison.OrdinalIgnoreCase);
        public static bool IsNullable(this Column c) => string.Equals("YES", c.IsNullable, StringComparison.OrdinalIgnoreCase);
        public static bool IsAuditColumn(this Column c) => c.ColumnName.StartsWith("AUDIT_", StringComparison.OrdinalIgnoreCase);
        public static bool IsSequenceNumber(this Column c) => c.ColumnName.Equals("SEQUENCE_NUMBER", StringComparison.OrdinalIgnoreCase);

        public static string TypeDeclaration(this Column c)
        {
            const string max = "max";
            switch (c.DataType.ToLower())
            {
                case "binary":
                case "varbinary":
                case "char":
                case "nchar":
                case "varchar":
               
                case "nvarchar":
                    var len = c.CharacterMaximumLength == -1 ? max : c.CharacterMaximumLength.ToString();
                    return $"{c.DataType}({len})";
                case "numeric":
                case "decimal":
                    return $"{c.DataType}({c.NumericPrecision},{c.NumericScale})";
                default:
                    return c.DataType;
            }
        }

        public static string TableValue(this Column col, string alias)
        {
            switch (col.ColumnName.ToUpper())
            {
                case "AUDIT_START_DATE":
                case "AUDIT_DATE_TIME":
                    return $"ISNULL({alias}.[{col}], GETUTCDATE())";
                case "AUDIT_UPDATE_USER":
                case "AUDIT_USER":
                    return $"ISNULL({alias}.[{col}], dbo.ALL_UserContextGet())";
                case "AUDIT_APPLICATION_NAME":
                case "AUDIT_APPLICATION":
                    return $"ISNULL({alias}.[{col}], APP_NAME())";
                case "AUDIT_MACHINE_NAME":
                case "AUDIT_MACHINE":
                    return $"ISNULL({alias}.[{col}], HOST_NAME())";
                case "SEQUENCE_NUMBER":
                    return $"ISNULL({alias}.[{col}], 0) + 1";
                default:
                    return $"{alias}.[{col}]";
            }
        }

        public static string ParameterValue(this Column col)
        {
            switch (col.ColumnName.ToUpper())
            {
                case "AUDIT_START_DATE":
                case "AUDIT_DATE_TIME":
                    return $"ISNULL(@{col}, GETUTCDATE())";
                case "AUDIT_UPDATE_USER":
                case "AUDIT_USER":
                    return $"ISNULL(@{col}, dbo.ALL_UserContextGet())";
                case "AUDIT_APPLICATION_NAME":
                case "AUDIT_APPLICATION":
                    return $"ISNULL(@{col}, APP_NAME())";
                case "AUDIT_MACHINE_NAME":
                case "AUDIT_MACHINE":
                    return $"ISNULL(@{col}, HOST_NAME())";
                case "SEQUENCE_NUMBER":
                    return $"ISNULL(@{col}, 0) + 1";
                default:
                    return $"@{col}";
            }
        }

        public static string ClrTypeName(this Column c)
        {
            switch (c.DataType.ToLower())
            {
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                    return "string";
                case "numeric":
                case "decimal":
                    return c.IsNullable() ? "decimal?" : "decimal";
                case "int":
                    return c.IsNullable() ? "int?" : "int";
                case "bigint":
                    return c.IsNullable() ? "long?" : "long";
                case "smallint":
                    return c.IsNullable() ? "short?" : "short";
                case "tinyint":
                    return c.IsNullable() ? "byte?" : "byte";
                case "bit":
                    return c.IsNullable() ? "bool?" : "bool";
                case "datetime":
                case "datetime2":
                    return c.IsNullable() ? "DateTime?" : "DateTime";
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return c.DataType;
            }
        }


        public static dynamic ClrTypeDefault(this Column c)
        {
            switch (c.DataType.ToLower())
            {
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                    return "\"\"";
                case "numeric":
                case "decimal":                    
                case "int":                    
                case "bigint":                    
                case "smallint":                    
                case "tinyint":
                    return 0;
                case "bit":
                    return false;
                case "datetime":
                case "datetime2":
                    return DateTime.Now.ToString("yyyy-MM-dd-THH:mm:ss.000Z", CultureInfo.InvariantCulture);
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "uniqueidentifier":
                    return Guid.NewGuid().ToString();
                default:
                    return c.DataType;
            }
        }
    }
}
