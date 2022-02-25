using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;

namespace DAL.DAO
{
    public class EmployeeDAO : EmployeeContext
    {
        public static void AddEmployee(EMPLOYEE employee)
        {
            try
            {
                db.EMPLOYEEs.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static List<EMPLOYEE> GetUsers(int v)
        {
            return db.EMPLOYEEs.Where(x => x.UserNo == v).ToList();
        }

        public static List<EmployeeDetailDTO> GetEmployees()
        {
            List<EmployeeDetailDTO> employeeList = new List<EmployeeDetailDTO>();
            var list = (from e in db.EMPLOYEEs
                        join d in db.DEPARTMENTs on e.DepartmentID equals d.ID
                        join p in db.POSITIONs on e.PositionID equals p.ID
                        select new
                        {
                            userNo = e.UserNo,
                            surname = e.Surname,
                            name = e.Name,
                            employeeID = e.ID,
                            password = e.Password,
                            departmentName = d.DepartmentName,
                            positionName = p.PositionName,
                            departmentID = e.DepartmentID,
                            positionID = e.PositionID,
                            isAdmin = e.isAdmin,
                            salary = e.Salary,
                            imagePath = e.ImagePath,
                            birthDay = e.BirthDay,
                            address = e.Address
                        }).OrderBy(x => x.userNo).ToList();
            foreach (var item in list)
            {
                EmployeeDetailDTO dto = new EmployeeDetailDTO();
                dto.UserNo = item.userNo;
                dto.Surname = item.surname;
                dto.Name = item.name;
                dto.EmployeeID = item.employeeID;
                dto.Password = item.password;
                dto.DepartmentID = item.departmentID;
                dto.DepartmentName = item.departmentName;
                dto.PositionID = item.positionID;
                dto.PositionName = item.positionName;
                dto.isAdmin = item.isAdmin;
                dto.Salary = item.salary;
                dto.BirthDay = item.birthDay;
                dto.Address = item.address;
                employeeList.Add(dto);
            }
            return employeeList;
        }

        public static List<EMPLOYEE> GetEmployees(int v, string text)
        {
            try
            {
                List<EMPLOYEE> list = db.EMPLOYEEs.Where(x => x.UserNo == v && x.Password == text).ToList();
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void UpdateEmployee(int employeeId, int amount)
        {
            try
            {
                EMPLOYEE employee = db.EMPLOYEEs.First(x => x.ID == employeeId);
                employee.Salary = amount;
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
