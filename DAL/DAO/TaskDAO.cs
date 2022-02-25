using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;

namespace DAL.DAO
{
    public class TaskDAO : EmployeeContext
    {
        public static List<TASKSTATE> GetTaskStates()
        {
            return db.TASKSTATEs.ToList();
        }

        public static void AddTask(TASK task)
        {
            try
            {
                db.TASKs.InsertOnSubmit(task);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static List<TaskDetailDTO> GetTasks()
        {
            List<TaskDetailDTO> tasklist = new List<TaskDetailDTO>();
            var list = (from t in db.TASKs
                join s in db.TASKSTATEs on t.TaskState equals s.ID
                join e in db.EMPLOYEEs on t.EmployeeID equals e.ID
                join d in db.DEPARTMENTs on e.DepartmentID equals d.ID
                join p in db.POSITIONs on e.PositionID equals p.ID
                select new
                {
                    taskID = t.ID,
                    title = t.TaskTitle,
                    content = t.TaskContent,
                    startDate = t.TaskStartDate,
                    deliveryDate = t.TaskDeliveryDate,
                    taskStateName = s.StateName,
                    taskStateID = t.TaskState,
                    userNo = e.UserNo,
                    surname = e.Surname,
                    name = e.Name,
                    employeeID = t.EmployeeID,
                    positionName = p.PositionName,
                    departmentName = d.DepartmentName,
                    positionID = e.PositionID,
                    departmentID = e.DepartmentID,
                }).OrderBy(x => x.startDate).ToList();
            foreach (var item in list)
            {
                TaskDetailDTO dto = new TaskDetailDTO();
                dto.TaskID = item.taskID;
                dto.Title = item.title;
                dto.Content = item.content;
                dto.TaskStartDate = item.startDate;
                dto.TaskDeliveryDate = item.deliveryDate;
                dto.TaskStateName = item.taskStateName;
                dto.TaskStateID = item.taskStateID;
                dto.UserNo = item.userNo;
                dto.Surname = item.surname;
                dto.Name = item.name;
                dto.DepartmentID = item.departmentID;
                dto.DepartmentName = item.departmentName;
                dto.PositionID = item.positionID;
                dto.PositionName = item.positionName;
                dto.EmployeeID = item.employeeID;
                tasklist.Add(dto);
            }

            return tasklist;
        }

        public static void UpdateTask(TASK task)
        {
            try
            {
                TASK ts = db.TASKs.First(x => x.ID == task.ID);
                ts.TaskTitle = task.TaskTitle;
                ts.TaskContent = task.TaskContent;
                ts.TaskState = task.TaskState;
                ts.EmployeeID = task.EmployeeID;
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
