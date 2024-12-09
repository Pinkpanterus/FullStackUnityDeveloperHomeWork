using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Modules.Converter
{
    public class ConverterMonobehaviour : MonoBehaviour, IConverter
    {
        private IConverter _converter;
        private TimerScript _timer;

        public ConverterMonobehaviour()
        {
            _converter = new Converter();
        }

        public ConverterMonobehaviour(int materialStorageCapacity, int productStorageCapacity, int materialAmountForProduction, int productAmountFromProduction,
            float productionTime, bool isOn, TimerScript timer)
        {
            _converter = new Converter(materialStorageCapacity, productStorageCapacity, materialAmountForProduction, productAmountFromProduction, productionTime, isOn);
            _timer = timer;
            _timer.SetDuration(_converter.ProductionTime());
        }

        public void Construct(int materialStorageCapacity, int productStorageCapacity, int materialAmountForProduction, int productAmountFromProduction,
            float productionTime,
            bool isOn)
        {
            _converter.Construct(materialStorageCapacity, productStorageCapacity, materialAmountForProduction, productAmountFromProduction, productionTime, isOn);
            _timer = gameObject.AddComponent<TimerScript>();
            _timer.SetDuration(_converter.ProductionTime());
        }

        public bool AddMaterials(int quantity, out int change)
        {
            return _converter.AddMaterials(quantity, out change);
        }

        public bool TakeMaterials(int quantity)
        {
            return _converter.TakeMaterials(quantity);
        }

        public int GetMaterialsCount()
        {
            return _converter.GetMaterialsCount();
        }

        public bool AddProducts(int quantity)
        {
            return _converter.AddProducts(quantity);
        }

        public bool TakeProducts(int quantity)
        {
            return _converter.TakeProducts(quantity);
        }

        public int GetProductCount()
        {
            return _converter.GetProductCount();
        }

        public bool Produce()
        {
            if (!_converter.IsOn())
                return false;
            
            int materialAmountForProduction = MaterialAmountForProduction();
            if (!_converter.TakeMaterials(materialAmountForProduction))
                return false;

            
            Coroutine coroutine = StartCoroutine(ProduceCoroutine());

            return true;
        }

        private IEnumerator ProduceCoroutine(Action<bool> callback = null)
        {
            _timer.Play();
            yield return new WaitUntil(() => !_timer.IsPlaying);

            int productAmountFromProduction = ProductAmountFromProduction();
            _converter.AddProducts(productAmountFromProduction);
        }

        // private void OnProductionCanceled()
        // {
        //     _timer.OnFinished -= OnProductionEnded;
        //     _timer.OnCanceled -= OnProductionCanceled;
        // }

        // private void OnProductionEnded()
        // {
        //     int productAmountFromProduction = ProductAmountFromProduction();
        //     _converter.AddProducts(productAmountFromProduction);
        //     _timer.OnFinished -= OnProductionEnded;
        //     _timer.OnCanceled -= OnProductionCanceled;
        // }

        public int MaterialAmountForProduction()
        {
            return _converter.MaterialAmountForProduction();
        }

        public int ProductAmountFromProduction()
        {
            return _converter.ProductAmountFromProduction();
        }

        public float ProductionTime()
        {
            return _converter.ProductionTime();
        }

        public bool IsOn()
        {
            return _converter.IsOn();
        }

        public void SwitchState(bool isOn)
        {
            _converter.SwitchState(isOn);
        }
    }
}