using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Simple_Payroll_Software_Project
{
    class Program
    {
        static void Main(string[] args)
        {     
            List<Staff> staffs = new List<Staff>();
            FileReader fileReader = new FileReader();
            int month = 0, year = 0;

            while (year==0)
            {
                Console.WriteLine("Please enter the year: ");
                try
                {
                    year = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "Please try again!");                 
                }                                 
            }

            while (month == 0)
            {
                Console.WriteLine("Please enter the month: ");
                try
                {
                    month = Convert.ToInt32(Console.ReadLine());
                    if (month<1||month>12)
                    {
                        Console.WriteLine("Month must be from 1 to 12. Please try again!");
                        month = 0;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "Please try again!");
                }
            }

            staffs = fileReader.ReadFile();

            for (int i = 0; i < staffs.Count; i++)
            {
                try
                {
                    Console.WriteLine("Enter Hours worked for {0}", staffs[i].NameOfStaff);
                    staffs[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    staffs[i].CalculatePay();
                    Console.WriteLine(staffs[i]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    i--;
                }
            }

            PaySlip ps = new PaySlip(month, year);
            ps.GeneratePaySlip(staffs);
            ps.GenerateSummary(staffs);

            Console.Read();
        }

    }
    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay { get; protected set; }
        public float BasicPay { get; private set; }
        public string NameOfStaff { get; private set; }

        public int HoursWorked
        {
            get
            {
                return hWorked;
            }
            set
            {
                if (value>0)
                {
                    hWorked = value;
                }
                else
                {
                    hWorked = 0;
                }
            }
        }
        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay.....");
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff +
                "\nHourly rate: " + hourlyRate +
                "\nHours worked: "+HoursWorked +
                "\nBasic pay: " + BasicPay +
                "\nTotal pay: " + TotalPay;
        }
    } //父類別

    class Mangaer : Staff
    {
        private const float managerHourlyRate = 50;

        public int Allowance { get; private set; }

        public Mangaer(string name) : base(name, managerHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();
            
            if (HoursWorked>160)
            {
                Allowance = 1000;
                TotalPay = BasicPay + Allowance;
            }
        }

        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff +
                "\nMmanager hourly Rate: " + managerHourlyRate +
                "\nHours worked: " + HoursWorked +
                "\nBasic pay: " + BasicPay +
                "\nAllowance: "+Allowance +
                "\nTotal pay: " + TotalPay;
        }
    } //第一子類別

    class Admin : Staff
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30f;

        public float Overtine { get; private set; }

        public Admin(string name) : base(name, adminHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();
            if (HoursWorked>160)
            {
                Overtine = overtimeRate * (HoursWorked - 160);
                TotalPay = Overtine + BasicPay;
            }
        }

        public override string ToString()
        {
            return "\nName of Staff: " + NameOfStaff +
                "\nAdmin hourly rate: " + adminHourlyRate +
                "\nHours worked: " + HoursWorked +
                "\nBasic pay: " + BasicPay +
                "\nOvertine: " + Overtine +
                "\nTotal pay: " + TotalPay;
        }
    }  //第二子類別

    class FileReader
    {
        public List<Staff> ReadFile()
        {
            List<Staff> staffs = new List<Staff>();
            string path = "/Users/Onion/Desktop/staff.txt";
            string[] result = new string[2];
            string[] separator = { "," };

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while(!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        if (result[1]=="Manager")
                        {
                            staffs.Add(new Mangaer(result[0]));
                        }
                        else if (result[1]=="Admin")
                        {
                            staffs.Add(new Admin(result[0]));
                        }
                    }
                    sr.Close();
                }
                
            }
            else
            {
                Console.WriteLine("Error: File does not exists");
            }
            return staffs;
        }
    } //讀取員工資料檔案

    class PaySlip
    {
        private int month;
        private int year;

        enum MonthsOfYear
        {
            JAN=1,FEB=2,MAR=3,APR=4,
            MAY=5,JUN=6,JUL=7,AUG=8,
            SEP=9,OCT=10,NOV=11,DEC=12
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
                    sw.WriteLine("PAYSLIP FOR {0} {1}",(MonthsOfYear)month, year);
                    sw.WriteLine("====================");
                    sw.WriteLine("Name of Staff: {0}", s.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0}", s.HoursWorked);
                    sw.WriteLine("");
                    sw.WriteLine("Basic Pay: {0:C}", s.BasicPay);
                    if (s.GetType()==typeof(Mangaer))
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

    }  //製作出個人薪資單的表格

}
