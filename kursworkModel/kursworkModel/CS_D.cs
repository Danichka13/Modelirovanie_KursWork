
namespace kursworkModel
{
    internal class CS_D
    {
        public bool[] NextStateCode { get; private set; } = new bool[2]; //Булевый массив для хранения кода состояния

        public void setNext(bool[] statesA, bool[] conditionsX) // Метод, формирующий код следующего состояния
        {
            for (int i = 0; i < NextStateCode.Length; i++) //Очищаем массив, тем самым задаём код состояния = 00
            {
                NextStateCode[i] = false; 
            }
            if (statesA[0] && conditionsX[0] && !conditionsX[1] && !conditionsX[2]) // a0x0¬x1¬x2
                NextStateCode[0] = true;
            else if (statesA[1] && conditionsX[3] // a1x3
                || statesA[1] && !conditionsX[3] && conditionsX[4] // a1¬x3x4
                || statesA[1] && !conditionsX[3] && !conditionsX[4] && conditionsX[5] // a1¬x3¬x4x5
                || statesA[1] && !conditionsX[3] && !conditionsX[4] && !conditionsX[5] // a1¬x3¬x4¬x5
                || statesA[2] && conditionsX[3] && !conditionsX[6] // a2x3¬x6
                || statesA[2] && !conditionsX[3] && conditionsX[4] && !conditionsX[6] // a2¬x3x4¬x6
                || statesA[2] && !conditionsX[3] && !conditionsX[4] && conditionsX[5] && !conditionsX[6] // a2¬x3¬x4x5¬x6
                || statesA[2] && !conditionsX[3] && !conditionsX[4] && !conditionsX[5] && !conditionsX[6]) // a2¬x3¬x4¬x5¬x6
                NextStateCode[1] = true;
            else if (statesA[2] && conditionsX[6] && conditionsX[7]) // a2x6x7
            {
                NextStateCode[0] = true;
                NextStateCode[1] = true;
            }

        }
    }
}