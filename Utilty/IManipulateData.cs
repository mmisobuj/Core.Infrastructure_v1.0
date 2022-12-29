using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utilty
{
    public interface IManipulateData
    {
        string CreateNextCode(string headerString, int codeTotalLength, string fieldName, string tableName);
        string createNextCode_Counter(string headerString, int codeTotalLength, string fieldName, string tableName);
        string CreateNextCode(string prefix, string fieldName, string tableName);
        int GetMaxValue(string fieldName, string tableName);
    }
}
