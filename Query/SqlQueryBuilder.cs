using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Core.Infrastructure.Query
{
    public class SqlQueryBuilder
    {

        public string GetInsertQuery<T>(T obj)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            string[] arrColumns;


            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Insert into {0} ({1}) values({2})";
            string tableName = entityType.Name;

            foreach (PropertyDescriptor prop in properties)

            {


                dynamic val;

                if (prop.PropertyType.Name == "DateTime")
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        var date = (DateTime)value;
                        sbValues.Append("'");
                        sbValues.Append(date);
                        sbValues.Append("'");
                        sbValues.Append(",");

                        sbColumns.Append(prop.Name);
                        sbColumns.Append(",");


                    }
                }
                else if (prop.PropertyType.Name == "Int32")
                {
                    val = prop.GetValue(obj);
                    if (val != null)
                    {
                        if (val > 0)
                        {
                            sbValues.Append("'");
                            sbValues.Append(val);
                            sbValues.Append("'");
                            sbValues.Append(",");

                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");

                        }


                    }

                }
                else
                {

                    val = prop.GetValue(obj);
                    if (val != null)
                    {

                        sbValues.Append("'");
                        sbValues.Append(val);
                        sbValues.Append("'");
                        sbValues.Append(",");

                        sbColumns.Append(prop.Name);
                        sbColumns.Append(",");


                    }




                }



            }
            sbColumns.Remove(sbColumns.Length - 1, 1);
            sbValues.Remove(sbValues.Length - 1, 1);

            var rvStr = string.Format(qryTmp, tableName, sbColumns, sbValues);
            return rvStr;
        }
        public string GetInsertQuery<T>(T obj, string columns)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            string[] arrColumns;
            if (string.IsNullOrEmpty(columns))
            {
                arrColumns = new string[] { };
            }
            else
            {
                arrColumns = columns.Split(',');
            }

            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Insert into {0} ({1}) values({2})";
            string tableName = entityType.Name;

            foreach (PropertyDescriptor prop in properties)

            {
                string columnName = "";
                if (arrColumns.Length > 0)
                {
                    columnName = arrColumns.FirstOrDefault(s => s.ToLower().Equals(prop.Name.ToLower()));
                }

                dynamic val;
                if (!string.IsNullOrEmpty(columnName))
                {
                    if (prop.PropertyType.Name == "DateTime")
                    {
                        var value = prop.GetValue(obj);
                        if (value != null)
                        {
                            var date = (DateTime)value;
                            sbValues.Append("'");
                            sbValues.Append(date);
                            sbValues.Append("'");
                            sbValues.Append(",");

                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");


                        }
                    }
                    else
                    {

                        val = prop.GetValue(obj);
                        if (val != null)
                        {
                            sbValues.Append("'");
                            sbValues.Append(val);
                            sbValues.Append("'");
                            sbValues.Append(",");

                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");


                        }


                    }

                }



            }
            sbColumns.Remove(sbColumns.Length - 1, 1);
            sbValues.Remove(sbValues.Length - 1, 1);

            var rvStr = string.Format(qryTmp, tableName, sbColumns, sbValues);
            return rvStr;

        }

        public string GetUpdateQuery<T>(T obj, string columns, string condition)
        {
            StringBuilder sbValues = new StringBuilder();

            string[] arrColumns;
            if (string.IsNullOrEmpty(columns))
            {
                arrColumns = new string[] { };
            }
            else
            {
                arrColumns = columns.Split(',');
            }

            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Update {0} {1} Where {2}";//0 Table,1 set value 2 condition
            string tableName = entityType.Name;
            sbValues.Append(" Set ");
            foreach (PropertyDescriptor prop in properties)

            {
                string columnName = "";
                if (arrColumns.Length > 0)
                {
                    columnName = arrColumns.FirstOrDefault(s => s.ToLower().Equals(prop.Name.ToLower()));
                }

                dynamic val;
                if (!string.IsNullOrEmpty(columnName))
                {
                    if (prop.PropertyType.Name == "DateTime")
                    {
                        var value = prop.GetValue(obj);
                        if (value != null)
                        {
                            var date = (DateTime)value;


                            sbValues.Append(prop.Name);
                            sbValues.Append("=");
                            sbValues.Append("'");
                            sbValues.Append(date);
                            sbValues.Append("'");
                            sbValues.Append(",");


                        }
                    }
                    else
                    {

                        val = prop.GetValue(obj);
                        if (val != null)
                        {

                            sbValues.Append(prop.Name);
                            sbValues.Append("=");
                            sbValues.Append("'");
                            sbValues.Append(val);
                            sbValues.Append("'");
                            sbValues.Append(",");


                        }


                    }

                }



            }

            sbValues.Remove(sbValues.Length - 1, 1);

            var rvStr = string.Format(qryTmp, tableName, sbValues, condition);
            return rvStr;

        }

    }
}
