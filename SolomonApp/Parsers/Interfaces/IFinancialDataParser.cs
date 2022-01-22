using System;
namespace SolomonApp.Parsers.Interfaces
{
	public interface IFinancialDataParser
	{
		public long GetLongAccountValue(object finDataObject, string accountName);

		public decimal GetDecimalKpiValue(object finDataObject, string kpiName);

		public object GetPropertyValue(object finDataObject, string propName);
	}
}
