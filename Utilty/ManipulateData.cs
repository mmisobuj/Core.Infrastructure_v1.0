using Core.Infrastructure.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utilty
{

    public class ManipulateData : IManipulateData
    {
        public readonly ILogger _logger;
        private readonly IDbConnection _conn;
        public ManipulateData(ILogger logger, IConnectionFactory conn)
        {
            _logger = logger;
            _conn = conn.GetConnection;
        }
        public string CreateNextCode(string headerString, int codeTotalLength, string fieldName, string tableName)
        {
            string branchCode = "";
            int codeHeaderLength = headerString.Length;
            int numberOfZeroToAdd;
            try
            {
                string commandText = "select max(" + fieldName + ") from " + tableName;

                object obj = _conn.ExecuteScalar(commandText);
                if (obj == null)
                    obj = "";
                branchCode = obj.ToString();

                if (branchCode == "")
                {
                    branchCode = "1";
                    numberOfZeroToAdd = codeTotalLength - (1 + codeHeaderLength);
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        branchCode = "0" + branchCode;
                    }
                    branchCode = headerString + branchCode;
                }
                else
                {
                    branchCode = branchCode.Substring(codeHeaderLength);
                    int iBranchCode = int.Parse(branchCode);
                    iBranchCode++;
                    branchCode = iBranchCode.ToString();
                    int branchCodeLength = branchCode.Length;
                    numberOfZeroToAdd = codeTotalLength - (branchCodeLength + codeHeaderLength);
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        branchCode = "0" + branchCode;
                    }
                    branchCode = headerString + branchCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return branchCode;
        }
        public string createNextCode_Counter(string headerString, int codeTotalLength, string fieldName, string tableName)
        {
            string branchCode = "";
            int codeHeaderLength = headerString.Length;
            int numberOfZeroToAdd;
            try
            {
                string commandText = "select max(" + fieldName + ") from " + tableName;

                commandText = "SELECT max(" + fieldName + ") FROM " + tableName + " where substring(" + fieldName + ",2,1) = 'counterCode'";
                // UnitOfWork objCom = new UnitOfWork();

                object obj = _conn.ExecuteScalar(commandText);
                branchCode = obj.ToString();

                if (branchCode == "")
                {
                    branchCode = "1";
                    numberOfZeroToAdd = codeTotalLength - (1 + codeHeaderLength);
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        branchCode = "0" + branchCode;
                    }
                    branchCode = headerString + branchCode;
                }
                else
                {
                    branchCode = branchCode.Substring(codeHeaderLength);
                    int iBranchCode = int.Parse(branchCode);
                    iBranchCode++;
                    branchCode = iBranchCode.ToString();
                    int branchCodeLength = branchCode.Length;
                    numberOfZeroToAdd = codeTotalLength - (branchCodeLength + codeHeaderLength);
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        branchCode = "0" + branchCode;
                    }
                    branchCode = headerString + branchCode;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return branchCode;
        }

        //Get Next ID
        public int GetMaxValue(string fieldName, string tableName)
        {
            int maxValue = 0;
            try
            {
                string commandText = string.Format("select Max(substring({0},len({0})-4,5)) from {1}", fieldName, tableName);
                // Console.WriteLine("commandText = " + commandText);


                object obj = _conn.ExecuteScalar(commandText);

                if (obj != null)
                {
                    maxValue = Convert.ToInt32(obj);
                }


            }
            catch (Exception)
            {
                maxValue = -1;
            }
            return maxValue;
        }


        public string CreateNextCode(string prfix, string fieldName, string tableName)
        {
            //Sample Code: PO2021/04/00001
            string maxCode = "";
            int codeHeaderLength = prfix.Length;
            int codeTotalLength = 5;
            int numberOfZeroToAdd;
            try
            {

                maxCode = GetMaxValue(fieldName, tableName).ToString();

                if (maxCode == "0")
                {
                    maxCode = "1";
                    numberOfZeroToAdd = codeTotalLength - 1;
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        maxCode = "0" + maxCode;
                    }
                    // maxCode = prfix + maxCode;
                }
                else
                {
                    //maxCode = maxCode.Substring(codeHeaderLength);
                    int iBranchCode = int.Parse(maxCode);
                    iBranchCode++;
                    maxCode = iBranchCode.ToString();
                    int branchCodeLength = maxCode.Length;
                    numberOfZeroToAdd = codeTotalLength - branchCodeLength;
                    for (int i = 0; i < numberOfZeroToAdd; i++)
                    {
                        maxCode = "0" + maxCode;
                    }
                }
                maxCode = string.Format("{0}{1}/{2}/{3}", prfix, DateTime.Today.ToString("yyyy"), DateTime.Today.ToString("MM"), maxCode);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return maxCode;
        }


    }

}