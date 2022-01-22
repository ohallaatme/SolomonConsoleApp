using System;
using System.Collections.Generic;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        public long GetLongAccountValue(object finDataObject, string accountName)
        {
            string acctBalanceRaw = Convert.ToString(GetPropertyValue(finDataObject, accountName));

            if (acctBalanceRaw == "None")
            {
                // TODO: Consider if there needs to be error handling as this will
                // screw up some of the metrics, probably better to handle in 'parent'
                // KPI parser
                return 0;
            }

            long acctBalance = Convert.ToInt64(acctBalanceRaw);

            return acctBalance;

        }

        public decimal GetDecimalKpiValue(object finDataObject, string kpiName)
        {
            string kpiBalanceRaw = Convert.ToString(GetPropertyValue(finDataObject, kpiName));

            if (kpiBalanceRaw == "None")
            {
                return 0;
            }

            decimal kpiBalance = Convert.ToDecimal(kpiBalanceRaw);

            return kpiBalance;

        }

        public object GetPropertyValue(object finDataObject, string propName)
        {
            return finDataObject.GetType().GetProperty(propName).GetValue(finDataObject, null);
        }

    }
}
