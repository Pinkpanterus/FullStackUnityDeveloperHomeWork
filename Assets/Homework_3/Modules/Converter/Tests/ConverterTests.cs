using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Converter
{
    public sealed class ConverterTests
    {
        [Test]
        public void InstantiateConverter()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Assert:
            Assert.IsNotNull(converter);
        }

        [TestCaseSource(nameof(AddSpecifiedWrongArgumentCases))]
        public void WhenInstantiateConverterWithNegativeOrZeroArgumentThenException(int materialStorageCapacity, int productStorageCapacity,
            int materialAmountForProduction, int productAmountFromProduction, float productionTime, bool isOn)
        {
            //Assert:
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var _ = new Converter(materialStorageCapacity, productStorageCapacity, materialAmountForProduction, productAmountFromProduction, productionTime, isOn);
            });
        }

        private static IEnumerable<TestCaseData> AddSpecifiedWrongArgumentCases()
        {
            yield return new TestCaseData(0, 100, 2, 1, 3, false).SetName("0 material storage capacity");

            yield return new TestCaseData(-1, 100, 2, 1, 3, false).SetName("-1 material storage capacity");

            yield return new TestCaseData(100, 0, 2, 1, 3, false).SetName("0 product storage capacity");

            yield return new TestCaseData(100, -1, 2, 1, 3, false).SetName("-1 product storage capacity");

            yield return new TestCaseData(100, 100, 0, 1, 3, false).SetName("0 material amount for production");

            yield return new TestCaseData(100, 100, -1, 1, 3, false).SetName("-1 material amount for production");

            yield return new TestCaseData(100, 100, 2, 0, 3, false).SetName("0 product amount from production");

            yield return new TestCaseData(100, 100, 2, -1, 3, false).SetName("-1 product amount from production");

            yield return new TestCaseData(100, 100, 2, 1, 0, false).SetName("0 time for production");

            yield return new TestCaseData(100, 100, 2, 1, -1, false).SetName("-1 time for production");
        }

        [Test]
        public void AddMaterial()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Act:
            bool success = converter.AddMaterials(5, out int change);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(5, converter.GetMaterialsCount());
            Assert.AreEqual(0, change);
        }

        [Test]
        public void TakeMaterial()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Act:
            bool addSuccess = converter.AddMaterials(10, out int change);
            bool takeSuccess = converter.TakeMaterials(5);

            //Assert:
            Assert.IsTrue(addSuccess);
            Assert.IsTrue(takeSuccess);
            Assert.AreEqual(5, converter.GetMaterialsCount());
            Assert.AreEqual(0, change);
        }

        [Test]
        public void AddProduct()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Act:
            bool success = converter.AddProducts(5);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(5, converter.GetProductsCount());
        }

        [Test]
        public void TakeProduct()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Act:
            bool addSuccess = converter.AddProducts(10);
            bool takeSuccess = converter.TakeProducts(5);

            //Assert:
            Assert.IsTrue(addSuccess);
            Assert.IsTrue(takeSuccess);
            Assert.AreEqual(5, converter.GetProductsCount());
        }

        [Test]
        public void WhenAddMaterialWithAmountMoreThanLimitThenTrueAndReturnChange()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Act:
            bool success = converter.AddMaterials(110, out int change);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(100, converter.GetMaterialsCount());
            Assert.AreEqual(10, change);
        }

        /*
         * В ТЗ не написано, поэтому предполагаю, что если на склад готовой продукции
         * кладем больше продукции чем он вмещает, то то что не влазит - "сгорает".
         * На случай, если во время производства кто-то забьет склад готовой продукции.
         */
        [Test]
        public void WhenAddProductWithAmountMoreThanLimitThenTrueAndBurnChange()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);
            //Act:
            bool success = converter.AddProducts(101);

            //Assert:
            Assert.IsTrue(success);
            Assert.AreEqual(100, converter.GetProductsCount());
        }

        [Test]
        public void WhenAddProductWithNegativeAmountThenException()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Assert:
            Assert.Catch<ArgumentOutOfRangeException>(() => converter.AddProducts(-10));
        }

        [Test]
        public void WhenAddMaterialWithNegativeAmountThenException()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);

            //Assert:
            Assert.Catch<ArgumentOutOfRangeException>(() => converter.AddMaterials(-10, out int change));
        }

        [Test]
        public void WhenStartProductionThenTakeMaterials()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(converter.ProductionTime() / 2);

            //Assert:
            Assert.AreEqual(0, change);
            Assert.AreEqual(startMaterialAmount - converter.MaterialAmountForProduction(), converter.GetMaterialsCount());
        }

        [Test]
        public void WhenFinishProductionThenAddProducts()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(Time.deltaTime);
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(converter.ProductAmountFromProduction(), converter.GetProductsCount());
        }

        [Test]
        public void WhenIsNotOnThenNoStartProduction()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, false);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(Time.deltaTime);
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(0, converter.GetProductsCount());
        }

        [Test]
        public void WhenStopProductionThenNoAddProducts()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(Time.deltaTime);
            converter.SwitchState(false);
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(0, converter.GetProductsCount());
        }

        [Test]
        public void WhenStopProductionThenReturnMaterials()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(Time.deltaTime);
            converter.SwitchState(false);
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(startMaterialAmount, converter.GetMaterialsCount());
        }

        [Test]
        public void WhenSwitchStateThenStateChange()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;

            //Act:
            bool success = converter.AddMaterials(startMaterialAmount, out int change);
            converter.Update(Time.deltaTime);
            converter.SwitchState(false);
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(false, converter.IsOn());
        }

        [Test]
        public void WhenProductionEndsThenStartProduction()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 10;
            bool success = converter.AddMaterials(startMaterialAmount, out int change);

            //Act:
            converter.Update(Time.deltaTime);
            converter.Update(converter.ProductionTime());
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(converter.ProductAmountFromProduction() * 2, converter.GetProductsCount());
        }
        
        [Test]
        public void WhenNotEnoughMaterialsThenStopProduction()
        {
            //Arrange:
            var converter = new Converter(100, 100, 2, 1, 3, true);
            int startMaterialAmount = 3;
            bool success = converter.AddMaterials(startMaterialAmount, out int change);

            //Act:
            converter.Update(Time.deltaTime);
            converter.Update(converter.ProductionTime());
            converter.Update(converter.ProductionTime());

            //Assert:
            Assert.AreEqual(converter.ProductAmountFromProduction(), converter.GetProductsCount());
        }
    }
}