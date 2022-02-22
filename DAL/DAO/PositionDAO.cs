using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class PositionDAO:EmployeeContext
    {
        public static void AddPosition(POSITION position)
        {
            try
            {
                db.POSITIONs.InsertOnSubmit(position);
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
