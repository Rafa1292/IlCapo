using System;
using IlCapo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IlCapo.Tests
{
    [TestClass]
    public class Bill_Tests
    {
        [TestMethod]
        public void GetEmptyBill_Tests()
        {
            var elControlador = new Controllers.BillsController();
            Bill elResultadoEsperado = new Bill();
            Bill elResultadoObtenido = elControlador.GetEmptyBill();
            Assert.AreEqual(elResultadoEsperado, elResultadoObtenido);
        }
    }
}
