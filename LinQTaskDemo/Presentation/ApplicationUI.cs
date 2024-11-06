using LinQTaskDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LinQTaskDemo.Presentation
{
    internal class ApplicationUI
    {
        static List<Employee> employees = new List<Employee>()
            {
                new Employee{Id = 1, Name = "Shyam Kumavat", Age = 26, Department = "IT", Salary = 65000, JoinDate = new DateTime(2024, 9, 1)},
                new Employee{Id = 2, Name = "Mohan Chacherkar", Age = 24, Department = "HR", Salary = 55000, JoinDate = new DateTime(2019, 6, 15)},
                new Employee{Id = 3, Name = "Manish Wath", Age = 24, Department = "Finance", Salary = 35000, JoinDate = new DateTime(2020, 2, 20)},
                new Employee{Id = 4, Name = "Pavan Singh", Age = 32, Department = "IT", Salary = 72000, JoinDate = new DateTime(2022, 1, 10)},
                new Employee{Id = 5, Name = "Pooja Nimbalkar", Age = 35, Department = "Sales", Salary = 75000, JoinDate = new DateTime(2018, 11, 5)},
                new Employee{Id = 6, Name = "Shubham Paliwal", Age = 29, Department = "Finance", Salary = 62000, JoinDate = new DateTime(2020, 3, 30)},
                new Employee{Id = 7, Name = "Huma Qureshi", Age = 26, Department = "HR", Salary = 46000, JoinDate = new DateTime(2019, 9, 23)},
                new Employee{Id = 8, Name = "Shivraj Ghangale", Age = 22, Department = "IT", Salary = 45000, JoinDate = new DateTime(2024, 10, 12)},
                new Employee{Id = 9, Name = "Sahil Khanna", Age = 31, Department = "Sales", Salary = 50000, JoinDate = new DateTime(2020, 7, 10)},
                new Employee{Id = 10, Name = "Meena Kumari", Age = 23, Department = "HR", Salary = 47000, JoinDate = new DateTime(2021, 8, 18)},
                new Employee{Id = 11, Name = "Khushabu Kshatriya", Age = 22, Department = "HR", Salary = 47000, JoinDate = new DateTime(2022, 5, 18)}
            };
        public static void DisplayUI()
        {
            ShowEmployeeWithDepartmentNames();

            // Filtering....

            ShowEmployeeWithSalaryAboveSixtyT();

            // Sorting By Date 

            DisplayEmployeeSortByJoinDate();

            // Sorting 

            DisplaySortByDescendingSalary();

            // Average Salary By Department 

            DisplayAverageSalaryByGroup();

            // Departments And Emplioyee Join

            JoinDepartmentsAndEmployees();

            // Aggregation on Employees...

            AggregationOnEmployees();

            // Highest Salary Employee

            var highestSalaryEmp = employees.OrderByDescending(e => e.Salary).FirstOrDefault();

            Console.WriteLine("Employee with Highest Salary: " + 
                highestSalaryEmp.Name + " Salary: " + highestSalaryEmp.Salary + "\n");

            // Minimum Salary in HR Department

            Console.WriteLine("Minimum Salary in HR Department: " + employees.Where
                (e => e.Department == "HR").Min(e => e.Salary)+"\n");

            TakeSkipTop_3();

            // Any employee under 25 years of age

            bool isExist = employees.Any(e => e.Age <= 25);
            Console.WriteLine("Any Employee Exist under 25 years of age: " + (isExist?"Yes":"No")+"\n");
            Console.WriteLine("-----------------------------------------------------------\n");

            AllEmployeesHavingSalaryAboveThirtyT();

            // Distinct Departments

            PrintUniqueDepartments();
        }

        static void ShowEmployeeWithDepartmentNames()
        {
            Console.WriteLine("Select Employee With Department:\n");

            // Its Just a Decoration 
            Console.WriteLine("Employee Name             | Department Name\n" +
                "------------------------------------------------");
            employees.Select(e => new { e.Name, e.Department }).ToList().ForEach(e =>
            {
                Console.WriteLine("{0,-25} | {1,-15}",
                    e.Name.PadRight(25), e.Department.PadRight(15));
            });

            Console.WriteLine("\n\n============================================================\n");
        }

        static void ShowEmployeeWithSalaryAboveSixtyT()
        {
            Console.WriteLine("IT Employees with Salary > 60000:\n");

            employees.Where(e => e.Department == "IT" && e.Salary > 60000)
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name : {0,-15} | Salary : {1,7:F2}",
                                                     e.Name.PadRight(15), e.Salary));

            Console.WriteLine("\n\n============================================================\n");
        }


        static void DisplayEmployeeSortByJoinDate()
        {
            Console.WriteLine("Employees Ordered by Join Date:\n");

            employees.OrderBy(e => e.JoinDate)
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name: {0,-20} | Join Date: {1,10}",
                                                     e.Name.PadRight(20), e.JoinDate.ToShortDateString()));

            Console.WriteLine("\n\n============================================================\n");
        }


        static void DisplaySortByDescendingSalary()
        {
            Console.WriteLine("Employees by Descending Salary within Department:\n");

            employees.OrderByDescending(e => e.Salary)
                     .GroupBy(e => e.Department)
                     .ToList()
                     .ForEach(group =>
                     {
                         Console.WriteLine("Department: {0,-20}\n", group.Key.PadRight(20));

                         group.ToList().ForEach(emp => Console.WriteLine("    Name: {0,-22} | Salary: {1,10:F2}",
                                                                          emp.Name.PadRight(22), emp.Salary));
                         Console.WriteLine("\n----------------------------------------------------------");
                     });

            Console.WriteLine("\n\n============================================================\n");
        }


        static void DisplayAverageSalaryByGroup()
        {

            Console.WriteLine("Average Salary by Department:\n");

            employees.GroupBy(e => e.Department).ToList().ForEach(group =>
            {
                Console.WriteLine("Department Name: {0,-15} | Average Salary: {1,5:F2}",
                    group.Key.PadRight(15),
                    group.Average(emp => emp.Salary));
            });

            Console.WriteLine("\n\n============================================================\n");
        }

        static void JoinDepartmentsAndEmployees()
        {
            // Created Department List ...
            List<Department> departments = new List<Department>
            {
                new Department{DepartmentId = 1, DepartmentName = "IT"},
                new Department{DepartmentId = 2, DepartmentName = "HR"},
                new Department{DepartmentId = 3, DepartmentName = "Finance"},
                new Department{DepartmentId = 4, DepartmentName = "Sales"}
            };

            Console.WriteLine("Employees With Department Name:\n");

            employees.Join(departments, emp => emp.Department, dept =>
            dept.DepartmentName, (emp, dept) => new { emp.Name, dept.DepartmentName })
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name: {0,-20} | Department: {1,-15}",
                                                     e.Name.PadRight(20), e.DepartmentName.PadRight(15)));

            Console.WriteLine("\n\n============================================================\n");
        }

        static void AggregationOnEmployees()
        {
            Console.WriteLine("Aggregation on Employees : \n");

            // Employee Count 

            Console.WriteLine("\nTotal Number of Employees: " + employees.Count);

            // Total Salary Expenditure

            Console.WriteLine("\nTotal Salary Expenditure: " + employees.Sum(e => e.Salary));

            // Average Age of Employees

            Console.WriteLine("\nAverage Age of Employees: " + employees.Average(e => e.Age));

            Console.WriteLine("\n\n============================================================\n");
        }

        static void TakeSkipTop_3()
        {
            // Partitioning - Top 3 Highest Paid Employees

            Console.WriteLine("\n\n============================================================\n");
            Console.WriteLine("Top 3 Highest Paid Employees:\n");
            employees.OrderByDescending(e => e.Salary)
                     .Take(3)
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name: {0,-25} | Salary: {1,10:F2}",
                                                     e.Name.PadRight(25), e.Salary));
            Console.WriteLine("\n\n============================================================\n");

            // Partitioning - Next 3 Highest Paid Employees after Top 3

            Console.WriteLine("Next 3 Highest Paid Employees after Top 3:\n");
            employees.OrderByDescending(e => e.Salary)
                     .Skip(3)
                     .Take(3)
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name: {0,-25} | Salary: {1,10:F2}",
                                                     e.Name.PadRight(25), e.Salary));
            Console.WriteLine("\n\n============================================================\n");
        }


        static void AllEmployeesHavingSalaryAboveThirtyT()
        {
            Console.WriteLine("All Employees have Salary above 30000: \n");

            employees.Where(e => e.Salary > 30000)
                     .ToList()
                     .ForEach(e => Console.WriteLine("Name: {0,-25} | Salary: {1,10:F2}",
                                                     e.Name.PadRight(25), e.Salary));

            Console.WriteLine("\n\n============================================================\n");
        }


        static void PrintUniqueDepartments()
        {
            Console.WriteLine("Unique Departments:\n");

            employees.Select(e => e.Department)
                     .Distinct()
                     .ToList()
                     .ForEach(dept => Console.WriteLine("Department: {0,-15}", dept.PadRight(15)));

            Console.WriteLine("\n\n============================================================\n");
        }

    }
}
