
namespace kursworkModel
{
    internal class ControlMachine
    {
        AXMemory _axMemory;
        CS_D _CS_D;
        CS_Y _CS_Y;
        OperatingMachine _operatingMachine;

        public ControlMachine(AXMemory axMemory, CS_D cmb_D, CS_Y cmb_Y, OperatingMachine operatingMachine)
        {
            _axMemory = axMemory;
            _CS_D = cmb_D;
            _CS_Y = cmb_Y;
            _operatingMachine = operatingMachine;
        }
        public void ExecuteTact()
        {
            _axMemory.CurStateCode = _CS_D.NextStateCode; // Запись кода состояния, полученного из КС_D в ПС
            _operatingMachine.ArrX.CopyTo(_axMemory.ConditionsX, 0); //Запись ЛУ, вычисленных в ОА в ПЛУ
            _CS_Y.SetCurOperationsY(_axMemory.ArrStateA, _axMemory.ConditionsX); // Формирование вектора выходных сигналов Y
            _operatingMachine.ExecuteTact(_CS_Y.OperationsY); // Выполнение микрооперации 
            _CS_D.setNext(_axMemory.ArrStateA, _axMemory.ConditionsX); // Формирование кода следующего состояния 
        }
    }
}
