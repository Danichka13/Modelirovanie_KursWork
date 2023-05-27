using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursworkModel
{
    internal class AXMemory
    {
        private bool[] _arrStateA; // поле для хранения текущего состояния
        public bool[] ArrStateA { get { return _arrStateA; } private set { _arrStateA = ArrStateA; } } 
        private bool[] _curStateCode = new bool[2]; // поле для хранения кода текущего состояния
        public bool[] CurStateCode
        {
            set
            {
                value.CopyTo(_curStateCode, 0);
                Decoder();
            }
            get { return _curStateCode; }
        }
        public bool[] ConditionsX { get; set; } = new bool[7]; //Поле для хранения значений условий на текущем такте
        private void Decoder() // Дешифратор
        {
            _arrStateA = new bool[4];
            byte curStateIndex = 0; // Номер состояния
            if (CurStateCode[0]) // Если на первом D тригерре единица
                curStateIndex += 1;
            if (CurStateCode[1]) // Если на втором D тригерре единица
                curStateIndex += 2;
            _arrStateA[curStateIndex] = true; 
        }
    }
}
