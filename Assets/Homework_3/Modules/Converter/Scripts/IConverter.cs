namespace Modules.Converter
{
    public interface IConverter
    {
        void Construct(int materialStorageCapacity, int productStorageCapacity, int materialAmountForProduction, int productAmountFromProduction, float productionTime, bool isOn);
        bool AddMaterials(int quantity, out int change);
        bool TakeMaterials(int quantity);
        int GetMaterialsCount();
        bool AddProducts(int quantity);
        bool TakeProducts(int quantity);
        int GetProductCount();
        bool Produce();
        int MaterialAmountForProduction();
        int ProductAmountFromProduction();
        float ProductionTime();
        bool IsOn();
        void SwitchState(bool isOn);
    }
}