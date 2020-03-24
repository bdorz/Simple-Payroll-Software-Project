using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simple_Payroll_Software_Project
{
    class FileReader  //讀取員工資料檔案
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
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        if (result[1] == "Manager")
                        {
                            staffs.Add(new Mangaer(result[0]));
                        }
                        else if (result[1] == "Admin")
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
    }
}
