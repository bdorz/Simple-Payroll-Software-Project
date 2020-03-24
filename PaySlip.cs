using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simple_Payroll_Software_Project
{
    class PaySlip  //製作出個人薪資單的表格
    {
        private int month;
        private int year;

        enum MonthsOfYear
        {
            JAN = 1, FEB = 2, MAR = 3, APR = 4,
            MAY = 5, JUN = 6, JUL = 7, AUG = 8,
            SEP = 9, OCT = 10, NOV = 11, DEC = 12
        }

        public PaySlip(int payMonth, int payYear)
        {
            month = payMonth;
            year = payYear;
        }

        public void GeneratePaySlip(List<Staff> staffs)
        {
            string path;

            foreach (Staff s in staffs)
            {
                path = s.NameOfStaff + ".txt"; //Jerry.txt

                using (StreamWriter sw = new StreamWriter(path))
                {
                    /*
                        PAYSLIP FOR DEC 2010
                        ====================
                        Name of Staff: Jerry
                        Hours Worked: 1231

                        Basic Pay: $61,550
                        Allowance: $1,000

                        ====================
                        Total Pay: $62,550
                        ====================          
                    */
                    sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear)month, year);
                    sw.WriteLine("====================");
                    sw.WriteLine("Name of Staff: {0}", s.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0}", s.HoursWorked);
                    sw.WriteLine("");
                    sw.WriteLine("Basic Pay: {0:C}", s.BasicPay);
                    if (s.GetType() == typeof(Mangaer))
                    {
                        sw.WriteLine("Allowance: {0:C}", ((Mangaer)s).Allowance);
                    }
                    else if (s.GetType() == typeof(Admin))
                    {
                        sw.WriteLine("Overtime: {0:C}", ((Admin)s).Overtine);
                    }
                    sw.WriteLine("");
                    sw.WriteLine("====================");
                    sw.WriteLine("Total Pay: {0:C}", s.TotalPay);
                    sw.WriteLine("====================");

                    sw.Close();

                }
            }
        }
        public void GenerateSummary(List<Staff> staffs)
        {
            var result =
                from s in staffs
                where s.HoursWorked < 10
                orderby s.NameOfStaff ascending
                select new { s.NameOfStaff, s.HoursWorked };

            /*var newResult = staffs.Where(x => x.HoursWorked < 10).OrderBy(x => x.NameOfStaff).Select(x => new { x.NameOfStaff, x.HoursWorked };)*/

            string path = "summary.txt";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Staff with less than 10 working hours");
                sw.WriteLine("");

                foreach (var r in result)
                {
                    sw.WriteLine("Name of Staff: {0}, Hours Worked: {1}", r.NameOfStaff, r.HoursWorked);
                }
                sw.Close();
            }

        }
        public override string ToString()
        {
            return "month = " + month + "year = " + year;
        }

    }
}
