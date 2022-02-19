using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SolomonApp.Models.Results;

namespace SolomonApp.ExcelExport
{
    public class XmlWriter
    {
        public void WriteFinStatementScorecard(List<IncStatementScorecard> incStatementScorecards,
            List<BalSheetScorecard> balSheetScorecards)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument   
                .Create(@"/Users/katherineohalloran/Documents/Solomon/TestOutputs/fin-scorecard.xlsx",
                SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "FinStmtScorecard"
                };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<string> columns = new List<string>
                {
                    "Company",
                    "Statement",
                    "Year",
                    "KPI",
                    "Value"
                };

                foreach (string colName in columns)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(colName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                for (int i=0; i < incStatementScorecards.Count; i++)
                {
                    IncStatementScorecard incStatementScorecard = incStatementScorecards[i];
                    BalSheetScorecard balSheetScorecard = balSheetScorecards[i];

                    string incTicker = incStatementScorecard.CoTicker;
                    string balTicker = balSheetScorecard.CoTicker;

                    // Income Statement Metrics
                    Dictionary<string, Dictionary<string, decimal>> incStmtMetrics = new Dictionary<string, Dictionary<string, decimal>>();

                    incStmtMetrics.Add("Gross Profit", incStatementScorecard.GrossProfitResults);
                    incStmtMetrics.Add("Sga as % of Op Inc", incStatementScorecard.SgaResults);
                    incStmtMetrics.Add("Interest Exp as % of Op Inc", incStatementScorecard.IntExpResults);
                    incStmtMetrics.Add("Tax Rate %", incStatementScorecard.TaxExpResults);

                    // Balance Sheet Metrics
                    Dictionary<string, Dictionary<string, decimal>> balShtMetrics = new Dictionary<string, Dictionary<string, decimal>>();

                    balShtMetrics.Add("Net Receivables as % of Revenue", balSheetScorecard.NetRecResults);
                    balShtMetrics.Add("Cash to Debt Results", balSheetScorecard.CashToDebtResults);
                    balShtMetrics.Add("Inventory as a % of Net Earnings", balSheetScorecard.InvToNetEarningsResults);


                    foreach (var metric in incStmtMetrics)
                    {
                        string kpiName = metric.Key;
                        foreach (var vals in metric.Value)
                        {
                            Row newRow = new Row();

                            // Company
                            Cell coCell = new Cell();
                            coCell.DataType = CellValues.String;
                            coCell.CellValue = new CellValue(incTicker);
                            newRow.AppendChild(coCell);

                            // Statement
                            Cell stmtCell = new Cell();
                            stmtCell.DataType = CellValues.String;
                            stmtCell.CellValue = new CellValue("Income Statement");
                            newRow.AppendChild(stmtCell);

                            // Fiscal yr reported
                            Cell yrCell = new Cell();
                            yrCell.DataType = CellValues.String;
                            yrCell.CellValue = new CellValue(vals.Key);
                            newRow.AppendChild(yrCell);

                            // KPI cell
                            Cell kpiCell = new Cell();
                            kpiCell.DataType = CellValues.String;
                            kpiCell.CellValue = new CellValue(kpiName);
                            newRow.AppendChild(kpiCell);

                            Cell valCell = new Cell();
                            valCell.DataType = CellValues.Number;
                            valCell.CellValue = new CellValue(vals.Value);
                            newRow.AppendChild(valCell);

                            sheetData.Append(newRow);
                        }
                    }

                    foreach (var metric in balShtMetrics)
                    {
                        string kpiName = metric.Key;
                        foreach (var vals in metric.Value)
                        {
                            Row newRow = new Row();

                            // Company
                            Cell coCell = new Cell();
                            coCell.DataType = CellValues.String;
                            coCell.CellValue = new CellValue(balTicker);
                            newRow.AppendChild(coCell);

                            // Statement
                            Cell stmtCell = new Cell();
                            stmtCell.DataType = CellValues.String;
                            stmtCell.CellValue = new CellValue("Balance Sheet");
                            newRow.AppendChild(stmtCell);

                            // Fiscal yr reported
                            Cell yrCell = new Cell();
                            yrCell.DataType = CellValues.String;
                            yrCell.CellValue = new CellValue(vals.Key);
                            newRow.AppendChild(yrCell);

                            // KPI cell
                            Cell kpiCell = new Cell();
                            kpiCell.DataType = CellValues.String;
                            kpiCell.CellValue = new CellValue(kpiName);
                            newRow.AppendChild(kpiCell);

                            Cell valCell = new Cell();
                            valCell.DataType = CellValues.Number;
                            valCell.CellValue = new CellValue(vals.Value);
                            newRow.AppendChild(valCell);

                            sheetData.Append(newRow);
                        }
                    }
                }

                workbookPart.Workbook.Save();
            }
        }
    }
}
