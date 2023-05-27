using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursworkModel
{
    internal class MicroProgram
    {
        Variables _variables;
        Form1 _form1;
        public bool[] ArrA { get; set; } = new bool[4] { true, false, false, false }; // Массив состояний
        public bool[] WayOfCircle { get; set; } = new bool[4] { false, false, false, false }; // Массив, для отслеживания по какой ветку идёт цикл
        public bool CisNull { get; set; } = false; // true - выполняем y1 и переходим в состояние А0, если А или B равны 0
        public bool NegC { get; set; } = false; // true - знаковый разряд 1 и переходим в состояние А0
        public MicroProgram(Variables variables, Form1 form1)
        {
            _variables = variables;
            _form1 = form1;
        }

        public Variables ExecuteTact()
        {
            if (ArrA[0])
            {
                if (!_form1.checkStart.Checked) // Проверка стоит на "ПУСК"
                {
                }
                else if ((_variables.A & 0x7fff) == 0 || (_variables.B & 0x7fff) == 0) // Равны ли А или В нулю
                {
                    _variables.C = 0; // C равно 0, находимся также в состоянии А0
                    CisNull = true;
                }
                else
                {
                    _variables.C = 0; // Все разряды С становятся равны 0
                    _variables.Count = 0; // y2
                    _variables.D = (ushort)(_variables.B >> 15); // y3
                    _variables.B = (ushort)(_variables.B & 0x7FFF); // y4
                    _variables.AM &= 0xC0007FFF; // y5
                    _variables.AM = (uint)((_variables.AM >> 14 << 14) + (_variables.A & 0x7FFF)); // y6
                    ArrA[0] = false;
                    ArrA[1] = true; // Перешли в А1
                }
            }
            else if (ArrA[1])
            {
                OneCycleIteration(); // Выполняем одну итерацию цикла
                ArrA[1] = false;
                ArrA[2] = true; // Переходим в А2
            }
            else if (ArrA[2])
            {
                if (_variables.Count == 0)
                {
                    if (((_variables.C & 0x4000) >> 14) == 1) // Если C(14) равно 1
                    {
                        _variables.C = (_variables.C + 0x8000) << 1; // Прибавляем к С(15) единицу
                        ArrA[2] = false;
                        ArrA[3] = true; // Перешли в А3
                    }
                    else if (((_variables.A >> 15) ^ _variables.D) == 1) // иначе, если AM(30) - положительное,
                                                                               // а D - отрицательное, или наоборот
                    {
                        NegC = true;
                        _variables.C |= 0x80000000; // С(31) - знаковый разряд становится равным 1
                        ArrA[2] = false;
                        ArrA[0] = true; // Перешли в состояние А0, конец алгоритма
                    }
                    else
                    {
                        ArrA[2] = false;
                        ArrA[0] = true; // Перешли в сосотяние А0, конец алгоритма
                    }
                }
                else // Если счетсчмк не равен 0, делаем еще одну итерацию цикла
                {
                    OneCycleIteration();
                }
            }
            else if (ArrA[3])
            {
                if (((_variables.A >> 15) ^ _variables.D) == 1)
                {
                    NegC = true;
                    _variables.C |= 0x80000000;
                }
                ArrA[3] = false;
                ArrA[0] = true; // Перешли в сосотяние А0, конец алгоритма
            }
            return _variables;
        }

        private void OneCycleIteration()
        {
            if ((_variables.B & 0x3) == 0) // если 0 и 1 разряды B = 00 
            {
                if (_variables.Count == 0) // если счётчик равен 0, 
                    _variables.Count = 7;  // то ставим ему максимальное значение
                else
                    _variables.Count--; // иначе уменьшаем на 1

                _variables.AM = (_variables.AM >> 29 << 29) + _variables.AM << 2 & 0x3FFFFFFF; // y11
                _variables.B >>= 2; // y12

                WayOfCircle[0] = true; // прошли по первой ветке цикла
                WayOfCircle[1] = false; 
                WayOfCircle[2] = false; 
                WayOfCircle[3] = false; 
            }
            else if((_variables.B & 0x3) == 3) // иначе если 0 и 1 разряды B = 11
            {
                if (_variables.Count == 0)
                    _variables.Count = 7;
                else
                    _variables.Count--;

                _variables.C += (~_variables.AM + 0x1 << 2 >> 2) + 0xC0000000; // y10
                _variables.AM = (_variables.AM >> 29 << 29) + _variables.AM << 2 & 0x3FFFFFFF; // y11
                _variables.B = (ushort)((_variables.B >> 2) + 1); // y13

                WayOfCircle[0] = false;
                WayOfCircle[1] = false;
                WayOfCircle[2] = false;
                WayOfCircle[3] = true; // прошли по четвёртой ветке цикла
            }
            else if ((_variables.B & 0x3) == 2) // иначе если 0 и 1 разряды B = 10
            {
                if (_variables.Count == 0)
                    _variables.Count = 7;
                else
                    _variables.Count--;

                _variables.C += _variables.AM << 3 >> 2; // y9
                _variables.AM = (_variables.AM >> 29 << 29) + _variables.AM << 2 & 0x3FFFFFFF; // y11
                _variables.B >>= 2; // y12

                WayOfCircle[0] = false;
                WayOfCircle[1] = false;
                WayOfCircle[2] = true; // прошли по третьей ветке цикла
                WayOfCircle[3] = false;
            }
            else  // иначе (если 0 и 1 разряды B = 01)
            {
                if (_variables.Count == 0) 
                    _variables.Count = 7;
                else
                    _variables.Count--; 

                _variables.C += _variables.AM << 2 >> 2; // y8
                _variables.AM = (_variables.AM >> 29 << 29) + _variables.AM << 2 & 0x3FFFFFFF; // y11
                _variables.B >>= 2; // y12

                WayOfCircle[0] = false; 
                WayOfCircle[1] = true; // прошли по второй ветке цикла
                WayOfCircle[2] = false;
                WayOfCircle[3] = false;
            } 
            
        }
    }
}
