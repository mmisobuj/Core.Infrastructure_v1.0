using System;
using System.Collections.Generic;


namespace Core.Infrastructure.Grid
{
    public static class GridQueryBuilder<T>
    {
        #region for Grid

        public static string Query(GridOptions options, string query, string orderBy, string gridCondition)
        {
            string condition = "";


            if (options != null)
            {
                condition = FilterCondition(options.filter).Trim();
            }

            if (!string.IsNullOrEmpty(condition))
            {
                condition = " WHERE " + condition;
            }


            if (!string.IsNullOrEmpty(gridCondition))
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition = " WHERE " + gridCondition;
                }
                else
                {
                    condition += " AND " + gridCondition;
                }
            }

            string orderby = "";
            if (options != null)
            {
                if (options.sort != null)
                {
                    foreach (var gridSort in options.sort)
                    {
                        if (orderby == "")
                        {
                            orderby += "ORDER by " + gridSort.field + " " + gridSort.dir;
                        }
                        else
                        {
                            orderby += " , " + gridSort.field + " " + gridSort.dir;
                        }
                    }
                }
            }

            if (orderby == "")
            {
                if (!string.IsNullOrEmpty(orderBy))
                {
                    orderby = " ORDER BY " + orderBy;
                }
                else
                {
                    throw new Exception("Must be set Orderby column Name");
                }
            }
            int pageupperBound = 0;
            int skip = 0;
            if (options != null)
            {
                skip = options.skip;
                pageupperBound = skip + options.take;
            }
            var sql =
                string.Format(
                    @"SELECT * FROM (SELECT ROW_NUMBER() OVER({4}) AS ROWINDEX, T.* FROM ({0}) T {2}) tbl WHERE ROWINDEX >{1} AND ROWINDEX <={3}",
                    query, skip, condition, pageupperBound, orderby);

            return sql;
        }

        public static string FilterCondition(KendoGridFilter.GridFilters filter)
        {
            string whereClause = "";

            if (filter != null && filter.Filters != null && filter.Filters.Count > 0)
            {
                var parameters = new List<object>();
                var filters = filter.Filters;

                for (var i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                    {
                        if (filters[i].Value == null)
                        {
                            i = i + 1;
                            if (filters.Count == i)
                            {
                                break;
                            }
                        }

                        //whereClause += string.Format(" {0}",
                        //    UtilityCommon.BuildWhereClause<T>(i, filter.Logic,
                        //        filters[i],
                        //        parameters));

                        whereClause += string.Format(" {1}",
                               UtilityCommon.ToLinqOperator(filter.Logic),
                               UtilityCommon.BuildWhereClause<T>(i, filter.Logic,
                                   filters[i],
                                   parameters));
                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                UtilityCommon.ToLinqOperator(filter.Logic),
                                UtilityCommon.BuildWhereClause<T>(i, filter.Logic,
                                    filters[i],
                                    parameters));
                        }
                    }
                }
            }
            return whereClause;
        }



        #endregion

        #region Auto Complete

        public static string FilterCondition(KendoGridFilter.AutoCompFilters filter)
        {
            string whereClause = "";

            if (filter != null && filter.filters != null && filter.filters.Count > 0)
            {
                var parameters = new List<object>();
                var filters = filter.filters;

                for (var i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                    {
                        if (filters[i].value == null)
                        {
                            i = i + 1;
                            if (filters.Count == i)
                            {
                                break;
                            }
                        }

                        whereClause += string.Format(" {0}",
                            UtilityCommon.BuildWhereClause<T>(i, filter.logic, filters[i],
                                parameters));
                    }
                    else
                    {
                        if (filters[i].value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                UtilityCommon.ToLinqOperator(filter.logic),
                                UtilityCommon.BuildWhereClause<T>(i, filter.logic,
                                    filters[i],
                                    parameters));
                        }
                    }
                }
            }
            return whereClause;
        }

        public static string Query(AutoCompOptions options, string query, string orderBy, string gridCondition)
        {
            string condition = "";

            condition = FilterCondition(options.filter);

            if (!string.IsNullOrEmpty(condition))
            {
                condition = " WHERE " + condition;
            }


            if (!string.IsNullOrEmpty(gridCondition))
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition = " WHERE " + gridCondition;
                }
                else
                {
                    condition += " AND " + gridCondition;
                }
            }

            string orderby = "";

            if (orderby == "")
            {
                if (!string.IsNullOrEmpty(orderBy))
                {
                    orderby = " ORDER BY " + orderBy;
                }
                else
                {
                    throw new Exception("Must be set Orderby column Name");
                }
            }

            var pageupperBound = options.skip + options.take;
            var sql =
                string.Format(
                    @"SELECT * FROM (SELECT ROW_NUMBER() OVER({4}) AS ROWINDEX, T.* FROM ({0}) T {2}) tbl WHERE ROWINDEX >{1} AND ROWINDEX <={3}",
                    query, options.skip, condition, pageupperBound, orderby);

            return sql;
        }

        #endregion
    }
}
