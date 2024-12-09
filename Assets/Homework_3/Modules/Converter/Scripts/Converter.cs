using System;
using System.Threading;

namespace Modules.Converter
{
    /**
       Конвертер представляет собой преобразователь ресурсов, который берет ресурсы
       из зоны погрузки (справа) и через несколько секунд преобразовывает его в
       ресурсы другого типа (слева).
       
       Конвертер работает автоматически. Когда заканчивается цикл переработки
       ресурсов, то конвертер берет следующую партию и начинает цикл по новой, пока
       можно брать ресурсы из зоны загрузки или пока есть место для ресурсов выгрузки.
       
       Также конвертер можно выключать. Если конвертер во время работы был
       выключен, то он возвращает обратно ресурсы в зону загрузки. Если в это время
       были добавлены еще ресурсы, то при переполнении возвращаемые ресурсы
       «сгорают».
     
       Характеристики конвертера:
       - Зона погрузки: вместимость бревен
       - Зона выгрузки: вместимость досок
       - Кол-во ресурсов, которое берется с зоны погрузки
       - Кол-во ресурсов, которое поставляется в зону выгрузки
       - Время преобразования ресурсов
       - Состояние: вкл/выкл
     */
    
    public sealed class Converter : IConverter
    {
        private int _materialsAmount;
        private int _productsAmount;
        private int _materialStorageCapacity;
        private int _productStorageCapacity;
        private int _materialAmountForProduction;
        private int _productAmountFromProduction;
        private float _productionTime;
        private bool _isOn;

        public Converter(int materialStorageCapacity, int productStorageCapacity, int materialAmountForProduction, int productAmountFromProduction, float productionTime, bool isOn)
        {
            if (materialStorageCapacity <= 0 || 
                productStorageCapacity <= 0 || 
                materialAmountForProduction <= 0 || 
                productAmountFromProduction <= 0 || 
                productionTime <= 0)
                throw new ArgumentOutOfRangeException();
          
            _materialStorageCapacity = materialStorageCapacity;
            _productStorageCapacity = productStorageCapacity;
            _materialAmountForProduction = materialAmountForProduction;
            _productAmountFromProduction = productAmountFromProduction;
            _productionTime = productionTime;
            _isOn = isOn;
        }
        
        public Converter()
        {
      
        }


        public void Construct(int materialStorageCapacity, int productStorageCapacity, int materialAmountForProduction, int productAmountFromProduction, float productionTime, bool isOn)
        {
            _materialStorageCapacity = materialStorageCapacity;
            _productStorageCapacity = productStorageCapacity;
            _materialAmountForProduction = materialAmountForProduction;
            _productAmountFromProduction = productAmountFromProduction;
            _productionTime = productionTime;
            _isOn = isOn;
        }

        public bool AddMaterials(int quantity, out int change)
        {
            if (quantity < 1)
                throw new ArgumentOutOfRangeException();
            
            int possibleQuantity = _materialsAmount + quantity;
            _materialsAmount = Math.Min(possibleQuantity, _materialStorageCapacity);
            change = possibleQuantity > _materialStorageCapacity ? possibleQuantity - _materialStorageCapacity : 0;
            return true;
        }
        
        public bool TakeMaterials(int quantity)
        {
            if (quantity < 1 || quantity > _materialStorageCapacity)
                throw new ArgumentOutOfRangeException();

            if (_materialsAmount < quantity)
                return false;
            
            _materialsAmount -= quantity;
            return true;
        }

        public int GetMaterialsCount()
        {
            return _materialsAmount;
        }

        public bool AddProducts(int quantity)
        {
            if (quantity < 1)
                throw new ArgumentOutOfRangeException();
            
            int currentQuantity = _productsAmount;
            int possibleQuantity = currentQuantity + quantity;
            _productsAmount = Math.Min(possibleQuantity, _productStorageCapacity);
            return true;
        }
        
        public bool TakeProducts(int quantity)
        {
            if (quantity < 1 || quantity > _productStorageCapacity)
                throw new ArgumentOutOfRangeException();

            if (_productsAmount < quantity)
                return false;
            
            _productsAmount -= quantity;
            return true;
        }

        public int GetProductCount()
        {
            return _productsAmount;
        }

        public bool Produce()
        {
            if (!_isOn)
                return false;
            
            _materialsAmount -= _materialAmountForProduction;
            Thread.Sleep((int)(_productionTime*1000)); 
            _productAmountFromProduction += _productAmountFromProduction;
            return true;
        }
        
        public int MaterialAmountForProduction() => _materialAmountForProduction;
        
        public int ProductAmountFromProduction() => _productAmountFromProduction;
        
        public float ProductionTime() => _productionTime;
        
        public bool IsOn() => _isOn;
        
        public void SwitchState(bool isOn)
        {
            _isOn = isOn;
        }
    }
}
