using IlCapo.Models;
using System.Linq;

namespace IlCapo.Validation
{
    public class Validation
    {

        private static IlCapoContext db;
        private static string elUsuario;

        public Validation(IlCapoContext elContextoDeDatos, string elUsuarioPorParametro)
        {
            db = elContextoDeDatos;
            elUsuario = elUsuarioPorParametro;
        }

        public Result ValidateUser()
        {
            Result elValidateWorker = ValidateWorker();
            if (!elValidateWorker.IsValid)
            {
                return elValidateWorker;
            }

            Result elValidateWorkerDay = ValidateWorkerDay();
            if (!elValidateWorkerDay.IsValid)
            {
                return elValidateWorkerDay;
            }

            return new Result() { IsValid = true};
        }

        private Result ValidateWorker()
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == elUsuario);

            if (worker == null)
            {
                return new Result()
                {
                    IsValid = false,
                    Message = "Debes iniciar sesion antes!!!"
                };
            }
            return new Result()
            {
                IsValid = true
            };
        }

        private Result ValidateWorkerDay()
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == elUsuario);
            WorkDay workDay = new WorkDay();

            if (!workDay.IsInWorkingDay(worker))
            {
                return new Result()
                {
                    IsValid = false,
                    Message = "Debes abrir una jornada antes!!!"
                };
            }
            return new Result()
            {
                IsValid = true
            };
        }

    }
}