using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements
{
    /// <summary>
    /// SELECT statement.
    /// 
    /// Not implemented:
    ///     [SQL_SMALL_RESULT] [SQL_BIG_RESULT] [SQL_BUFFER_RESULT]
    ///     [SQL_CACHE | SQL_NO_CACHE] [SQL_CALC_FOUND_ROWS]
    ///     [into_option]
    ///     [PARTITION partition_list]
    /// </summary>
    [DbQuery(Template)]
    public class Select : MySqlTemplate
    {
        public const string Template =
            "SELECT {{@rows}} {{@priority}} {{@join_order}}\n" +
            "  {{@columns}}\n" +
            "FROM {{@table}} {{@joins}}\n" +
            "{{@where}}\n" +
            "{{@group_by}}\n" +
            "{{@having}}\n" +
            "{{@order_by}}\n" +
            "{{@limit}}";

        /// <summary>
        /// Rows selection option.
        /// </summary>
        [DbParameter("rows")]
        public SelectRows Rows { get; set; }

        /// <summary>
        /// Priority option.
        /// </summary>
        [DbParameter("priority")]
        public SelectPriority Priority { get; set; }

        /// <summary>
        /// Join order option.
        /// </summary>
        [DbParameter("join_order")]
        public SelectJoinOrder JoinOrder { get; set; }

        /// <summary>
        /// Columns.
        /// </summary>
        [DbParameter("columns")]
        public string Columns { get; set; }

        /// <summary>
        /// Table.
        /// </summary>
        [DbParameter("table")]
        public string Table { get; set; }

        /// <summary>
        /// Joins.
        /// </summary>
        [DbParameter("joins")]
        protected string Joins { get; set; }

        /// <summary>
        /// Where condition.
        /// </summary>
        public string Where { get => where; set => SetWhere(value); }

        private string where;

        /// <summary>
        /// Where clause.
        /// </summary>
        [DbParameter("where")]
        protected string WhereClause { get; set; }

        /// <summary>
        /// Group by columns
        /// </summary>
        public string GroupBy { get => groupBy; set => SetGroupBy(value); }

        private string groupBy;

        /// <summary>
        /// Group by clause.
        /// </summary>
        [DbParameter("group_by")]
        protected string GroupByClause { get; set; }

        /// <summary>
        /// Having condition.
        /// </summary>
        public string Having { get => having; set => SetHaving(value); }

        private string having;

        /// <summary>
        /// Having clause.
        /// </summary>
        [DbParameter("having")]
        protected string HavingClause { get; set; }

        /// <summary>
        /// Order by columns.
        /// </summary>
        public string OrderBy { get => orderBy; set => SetOrderBy(value); }

        private string orderBy;

        /// <summary>
        /// Order by clause.
        /// </summary>
        [DbParameter("order_by")]
        protected string OrderByClause { get; set; }

        /// <summary>
        /// Limit rows.
        /// </summary>
        public int? LimitRows { get => limitRows; set => SetLimitRows(value); }

        private int? limitRows;

        /// <summary>
        /// Rows offset.
        /// </summary>
        public int? LimitOffset { get => limitOffset; set => SetLimitOffset(value); }

        private int? limitOffset;

        /// <summary>
        /// Limit clause.
        /// </summary>
        [DbParameter("limit")]
        protected string LimitClause { get; set; }

        /// <summary>
        /// Single table all columns SELECT statement execution.
        /// </summary>
        public Select(string table, string whereCondition = null, string orderBy = null)
        {
            Columns = AllColumns;
            Table = table;
            Where = whereCondition;
            OrderBy = orderBy;
        }

        #region Clause setters

        private void SetWhere(string value)
        {
            where = value;
            WhereClause = !string.IsNullOrEmpty(where) ? $"WHERE {where}" : string.Empty;
        }

        private void SetGroupBy(string value)
        {
            groupBy = value;
            GroupByClause = !string.IsNullOrEmpty(groupBy) ? $"GROUP BY {groupBy}" : string.Empty;
        }

        private void SetHaving(string value)
        {
            having = value;
            HavingClause = !string.IsNullOrEmpty(having) ? $"HAVING {having}" : string.Empty;
        }

        private void SetOrderBy(string value)
        {
            orderBy = value;
            OrderByClause = !string.IsNullOrEmpty(orderBy) ? $"ORDER BY {orderBy}" : string.Empty;
        }

        private void SetLimitRows(int? value)
        {
            limitRows = value;
            UpdateLimitClause();
        }

        private void SetLimitOffset(int? value)
        {
            limitOffset = value;
            UpdateLimitClause();
        }

        private void UpdateLimitClause()
        {
            LimitClause = !limitOffset.HasValue && !limitRows.HasValue ? string.Empty :
                @"LIMIT " + (limitOffset.HasValue ? limitOffset.Value.ToString() + @", " : string.Empty) + (limitRows ?? int.MaxValue);
        }

        #endregion
    }
}
