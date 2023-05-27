using System;

namespace kursworkModel
{
    internal class OperatingMachine
    {
        Variables _variables;
        public bool[] ArrX = new bool[9];
        readonly Action[] _doOperationsY;
        Form1 form1;

        public OperatingMachine(Variables variables, Form1 form1)
        {
            _variables = variables;
            this.form1 = form1;
            _doOperationsY = new Action[]
            {
             () => { _variables.C = 0;}, //y1
             () => { _variables.Count = 0;}, // y2
             () => { _variables.D = (ushort)(_variables.B >> 15);}, // y3
             () => { _variables.B = (ushort)(_variables.B & 0x7FFF);}, // y4
             () => { _variables.AM &= 0xC0007FFF;}, // y5
             () => { _variables.AM = (uint)((_variables.AM >> 14 << 14) + (_variables.A & 0x7FFF));}, // y6
             () => {
                 if(_variables.Count == 0)
                     _variables.Count = 7;
                 else
                     _variables.Count--;}, // y7
             () => { _variables.C += _variables.AM << 2 >> 2;}, // y8
             () => { _variables.C += _variables.AM << 3 >> 2;}, // y9
             () => { _variables.C += (~_variables.AM + 0x1 << 2 >> 2) + 0xC0000000 ;}, // y10
             () => { _variables.AM = (_variables.AM >> 29 << 29) + _variables.AM << 2 & 0x3FFFFFFF;}, // y11
             () => { _variables.B >>= 2;}, // y12
             () => { _variables.B = (ushort)(_variables.B >> 2 + 1);}, // y13
             () => { _variables.C = (_variables.C + 0x8000) << 1;}, // y14
             () => { _variables.C |= 0x80000000;}, // y15
             };
        }

        public void ExecuteTact(bool[] operationsY)
        {
            for (int i = 0; i < _doOperationsY.Length; i++)
            {
                if (operationsY[i])
                {
                    _doOperationsY[i](); // Выполняем операцию
                }
            }
            CalculateXArray();
        }

        public void CalculateXArray() // Высчитываем значение логических условий
        {
            ArrX[0] = form1.checkStart.Checked; // x0
            ArrX[1] = (_variables.A & 0x7fff) == 0; // x1
            ArrX[2] = (_variables.B & 0x7fff) == 0; // x2
            ArrX[3] = (_variables.B & 0x3) == 0; // x3
            ArrX[4] = (_variables.B & 0x3) == 1; // x4
            ArrX[5] = (_variables.B & 0x3) == 2; // x5
            ArrX[6] = _variables.Count == 0; // x6
            ArrX[7] = ((_variables.C & 0x4000) >> 14) == 1; // x7
            ArrX[8] = ((_variables.A >> 15) ^ _variables.D) == 1; // x8 - сложение по модулю два
        }
    }
}
