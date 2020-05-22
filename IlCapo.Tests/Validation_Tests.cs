using System;
using IlCapo.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace IlCapo.Tests
{
    [TestClass]
    public class Validation_Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = Substitute.For<IlCapoContext>();
            var worker = new Models.Worker() { Name = "Nombre", Mail = "", WorkerId = 0 };
            var worker1 = new Models.Worker() { Name = "Nombre1", Mail = "", WorkerId = 0 };
            db.Workers.Add(worker);
            db.Workers.Add(worker1);
            var elValidador = new Validation.Validation(db, "Nombre1");
            var elResultadoEsperado = new Result() { IsValid = true, Message = ""};
            var elResultadoObtenido = elValidador.ValidateUser();
            Assert.AreEqual(elResultadoEsperado, elResultadoObtenido);
        }
    }
}
