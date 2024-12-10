namespace Modules.Converter
{
    public interface IConverter
    {
        bool AddMaterials(int quantity, out int change);
        bool TakeMaterials(int quantity);
        int GetMaterialsCount();
        bool AddProducts(int quantity);
        bool TakeProducts(int quantity);
        int GetProductsCount();
        void Produce();
        int MaterialAmountForProduction();
        int ProductAmountFromProduction();
        float ProductionTime();
        bool IsOn();
        void SwitchState(bool isOn);
    }
}