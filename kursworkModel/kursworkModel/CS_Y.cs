
namespace kursworkModel
{
    internal class CS_Y
    {
        public bool[] OperationsY { private set; get; } = new bool[16]; //Булевый массив для хранения состояний операций

        public void SetCurOperationsY(bool[] statesA, bool[] conditionsX) // Метод задающий, какие операции должны будут выполниться на текущем такте
        {
            for (int i = 0; i < OperationsY.Length; i++) // Очищаем массив
            {
                OperationsY[i] = false;
            }
            if (statesA[0] && conditionsX[0] && conditionsX[1] // a0x0x1
                || statesA[0] && conditionsX[0] && !conditionsX[1] && conditionsX[2]) // a0x0¬x1x2
            {
                OperationsY[0] = true;
            }
            else if (statesA[0] && conditionsX[0] && !conditionsX[1] && !conditionsX[2]) // a0x0¬x1¬x2
            {
                OperationsY[0] = true; // y1
                OperationsY[1] = true; // y2
                OperationsY[2] = true; // y3
                OperationsY[3] = true; // y4
                OperationsY[4] = true; // y5
                OperationsY[5] = true; // y6
            }
            else if (statesA[1] && conditionsX[3] // a1x3
                || statesA[2] && conditionsX[3] && !conditionsX[6]) // a2x3¬x6
            {
                OperationsY[6] = true; // y7
                OperationsY[10] = true; // y11
                OperationsY[11] = true; // y12
            }
            else if (statesA[1] && !conditionsX[3] && conditionsX[4] // a1¬x3x4
                || statesA[2] && !conditionsX[3] && conditionsX[4] && !conditionsX[6]) // a2¬x3x4¬x6
            {
                OperationsY[6] = true; // y7
                OperationsY[7] = true; // y8
                OperationsY[10] = true; // y11
                OperationsY[11] = true; // y12
            }
            else if (statesA[1] && !conditionsX[3] && !conditionsX[4] && conditionsX[5] // a1¬x3¬x4x5
                || statesA[2] && !conditionsX[3] && !conditionsX[4] && conditionsX[5] && !conditionsX[6]) // a2¬x3¬x4x5¬x6
            {
                OperationsY[6] = true; // y7
                OperationsY[8] = true; // y9
                OperationsY[10] = true; // y11
                OperationsY[11] = true; // y12
            }
            else if (statesA[1] && !conditionsX[3] && !conditionsX[4] && !conditionsX[5] // a1¬x3¬x4¬x5
                || statesA[2] && !conditionsX[3] && !conditionsX[4] && !conditionsX[5] && !conditionsX[6]) // a1¬x3¬x4¬x5¬x6
            {
                OperationsY[6] = true; // y7
                OperationsY[9] = true; // y10
                OperationsY[10] = true; // y11
                OperationsY[12] = true; // y13
            }
            else if (statesA[3] && conditionsX[6] && conditionsX[7]) // a2x6x7
            {
                OperationsY[13] = true; // y14
            }
            else if (statesA[0] && conditionsX[6] && !conditionsX[7] && conditionsX[8] // a2x6¬x7x8
                || statesA[0] && conditionsX[8]) // a3x8
            {
                OperationsY[14] = true; //y15
            }
        }
    }
}
