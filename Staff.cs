using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple_Payroll_Software_Project
{
    class Staff  //父類別
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
                if (value > 0)
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
                "\nHours worked: " + HoursWorked +
                "\nBasic pay: " + BasicPay +
                "\nTotal pay: " + TotalPay;
        }
    }

    class Mangaer : Staff //第一子類別
    {
        private const float managerHourlyRate = 50;

        public int Allowance { get; private set; }

        public Mangaer(string name) : base(name, managerHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();

            if (HoursWorked > 160)
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
                "\nAllowance: " + Allowance +
                "\nTotal pay: " + TotalPay;
        }
    }

    class Admin : Staff  //第二子類別
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30f;

        public float Overtine { get; private set; }

        public Admin(string name) : base(name, adminHourlyRate) { }

        public override void CalculatePay()
        {
            base.CalculatePay();
            if (HoursWorked > 160)
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
    }
}