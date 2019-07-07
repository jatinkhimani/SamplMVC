using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore
{
    public class CustomExceptionHandler
    {
        public static void ThrowHandledException(DbUpdateException dbUpdateEx)
        {
            SqlException sqlEx = dbUpdateEx.InnerException.InnerException as SqlException;
            if (sqlEx != null && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                throw new UniqueKeyException(dbUpdateEx.Message);
            }
            if (sqlEx != null && (sqlEx.Number == 547))
            {
                throw new ForeignKeyException(dbUpdateEx.Message);
            }
        }

        public static void ThrowHandledException(DbEntityValidationException dbEntityEx)
        {
            var validationErrs = dbEntityEx.EntityValidationErrors.SelectMany(f => f.ValidationErrors).Select(f => f.ErrorMessage);
            var fullMsg = string.Join(",", validationErrs);
            var exMsg = string.Concat(dbEntityEx.Message, "The validation Errors are : " + fullMsg);
            throw new DbEntityValidationException(exMsg, dbEntityEx.EntityValidationErrors);
        }
    }
}
