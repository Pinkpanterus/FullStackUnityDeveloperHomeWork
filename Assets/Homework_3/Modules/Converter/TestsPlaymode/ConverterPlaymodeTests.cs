using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Modules.Converter.TestsPlaymode
{
    public class ConverterPlaymodeTests
    {
        [UnityTest]
        public IEnumerator ConverterGetRightAmountOfMaterialsForProduction()
        {
            //Arrange:
            float productionTime = 1f;
            ConverterMonobehaviour converter = new GameObject().AddComponent<ConverterMonobehaviour>();
            converter.Construct(100, 100, 2, 1, productionTime, true);

            //Act:
            converter.AddMaterials(3, out int woodAmount);

            //Asume:
            Assert.AreEqual(0, woodAmount);
            Assert.AreEqual(3, converter.GetMaterialsCount());

            //Act:
            bool success = converter.Produce();
            yield return new WaitForSeconds(productionTime);

            //Asume:
            Assert.AreEqual(1, converter.GetMaterialsCount());
            Assert.IsTrue(success);
        }

        [UnityTest]
        public IEnumerator ConverterProduceRightAmountOfProducts()
        {
            //Arrange:
            float productionTime = 1f;
            ConverterMonobehaviour converter = new GameObject().AddComponent<ConverterMonobehaviour>();
            converter.Construct(100, 100, 2, 1, productionTime, true);

            //Act:
            converter.AddMaterials(3, out int woodAmount);

            //Asume:
            Assert.AreEqual(0, converter.GetProductCount());

            //Act:
            bool success = converter.Produce();
            yield return new WaitForSeconds(productionTime);

            //Asume:
            Assert.AreEqual(1, converter.GetProductCount());
            Assert.IsTrue(success);
        }
        
        [UnityTest]
        public IEnumerator ConverterProduceFalseIfIsNotOn()
        {
            //Arrange:
            float productionTime = 1f;
            ConverterMonobehaviour converter = new GameObject().AddComponent<ConverterMonobehaviour>();
            converter.Construct(100, 100, 2, 1, productionTime, false);

            //Act:
            converter.AddMaterials(3, out int woodAmount);

            //Asume:
            Assert.AreEqual(0, converter.GetProductCount());

            //Act:
            bool success = converter.Produce();
            yield return new WaitForSeconds(productionTime);

            //Asume:
            Assert.AreEqual(0, converter.GetProductCount());
            Assert.IsFalse(success);
        }
    }
}