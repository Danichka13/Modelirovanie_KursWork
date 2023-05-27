using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace kursworkModel
{
    public partial class Form1 : Form
    {

        Variables variables = new Variables();
        MicroProgram microProgram;
        AXMemory axMemory = new AXMemory();
        CS_Y cs_Y = new CS_Y();
        CS_D cs_D = new CS_D();
        OperatingMachine operatingMachine;
        ControlMachine controlMachine;
        bool MP_YAOA = false; // false - Выполняем микропрограмму, true - выполняем взаимодействие УАиОА
        Label[] arrIshodA, arrResD, arrIshodB, arrResA, arrResB, arrResAM, arrResC, arrCurCount, 
            arrStateMemory, arrDS,arrCS_D, arrCS_Y, arrStateConditions;

        string strValueA = "";
        string strValueB = "";
        bool isOver = false;
        bool tact1 = true;

        public Form1()
        {
            InitializeComponent();

            microProgram = new MicroProgram(variables, this);

            arrIshodA = new Label[] { ishodA0, ishodA1, ishodA2, ishodA3, ishodA4, ishodA5, ishodA6, ishodA7,
                ishodA8, ishodA9, ishodA10, ishodA11, ishodA12, ishodA13, ishodA14, ishodA15 };

            arrIshodB = new Label[] { ishodB0, ishodB1, ishodB2, ishodB3, ishodB4, ishodB5, ishodB6, ishodB7,
                ishodB8, ishodB9, ishodB10, ishodB11, ishodB12, ishodB13, ishodB14, ishodB15 };

            arrResA = new Label[] { rezA0, rezA1, rezA2, rezA3, rezA4, rezA5, rezA6, rezA7,
                rezA8, rezA9, rezA10, rezA11, rezA12, rezA13, rezA14, rezA15 };

            arrResB = new Label[] { rezB0, rezB1, rezB2, rezB3, rezB4, rezB5, rezB6, rezB7,
                rezB8, rezB9, rezB10, rezB11, rezB12, rezB13, rezB14, rezB15 };
            
            arrResD = new Label[] { rezD0, rezD1, rezD2, rezD3, rezD4, rezD5, rezD6, rezD7,
                rezD8, rezD9, rezD10, rezD11, rezD12, rezD13, rezD14, rezD15 };

            arrResAM = new Label[] { rezAM0, rezAM1, rezAM2, rezAM3, rezAM4, rezAM5, rezAM6, rezAM7,
                rezAM8, rezAM9, rezAM10, rezAM11, rezAM12, rezAM13, rezAM14, rezAM15,
                rezAM16, rezAM17, rezAM18, rezAM19, rezAM20, rezAM21, rezAM22, rezAM23,
                rezAM24, rezAM25, rezAM26, rezAM27, rezAM28, rezAM29, rezAM30, rezAM31 };

            arrResC = new Label[] { C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15,
                C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, C26, C27, C28, C29, C30, C31 };

            arrCurCount = new Label[] { CH0, CH1, CH2 };

            arrStateMemory = new Label[] { PS0, PS1 };

            arrDS = new Label[] { DS0, DS1, DS2, DS3 };

            arrCS_D = new Label[] { KD0, KD1 };

            arrCS_Y = new Label[] { KY1, KY2, KY3, KY4, KY5, KY6, KY7, KY8, KY9, KY10, KY11, KY12, KY12, KY13, KY14, KY15 };

            arrStateConditions = new Label[] { PL0, PL1, PL2, PL3, PL4, PL5, PL6, PL7, PL8 };
        }

        private void labelStartValue_Click(object sender, EventArgs e) // Задаём значение А и В
        {
            Label lbl = sender as Label;
            if (lbl.Text == " 0 ") // Если в ячейке 0,
                lbl.Text = " 1 "; // то заменяем на 1
            else
                lbl.Text = " 0 "; // иначе заменяем на 0
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value == 0)
            {
                labelMicro.ForeColor = Color.Blue;
                labelYAOA.ForeColor = Color.Black;
                MP_YAOA = false;
            }
            else
            {
                labelYAOA.ForeColor = Color.Blue;
                labelMicro.ForeColor = Color.Black;
                MP_YAOA = true;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            radioA0.Checked = true;
            buttonTact.Enabled = true;
            buttonAuto.Enabled = true;

            for (int i = arrIshodA.Length - 1; i >= 0; i--) // считываем значения А и В
            {
                strValueA += arrIshodA[i].Text.Substring(1, 1);
                strValueB += arrIshodB[i].Text.Substring(1, 1);
            }
            // переводим значения А и В в двоичное представление
            variables.A = Convert.ToUInt16(strValueA, 2);
            variables.B = Convert.ToUInt16(strValueB, 2);
            strValueA = "";
            strValueB = "";
            if (MP_YAOA) // Если ОАиУА - выполняем первый такт УА, для перехода в состояние А0
            {
                operatingMachine = new OperatingMachine(variables, this);
                operatingMachine.CalculateXArray();
                axMemory.ConditionsX = operatingMachine.ArrX;
                controlMachine = new ControlMachine(axMemory, cs_D, cs_Y, operatingMachine);
                controlMachine.ExecuteTact();
                UpdateStructScheme();
            }
        }

        private void buttonTact_Click(object sender, EventArgs e)
        {
            if (!MP_YAOA) // Если на уровне микропрограммы
            {
                microProgram.ExecuteTact(); // выполняем такт
                UpdateVariables(); // обновляем переменные
                UpdateSings(); // обновляем флажки
                if (microProgram.ArrA[0]) // если в состоянии А0 - умножение окончено
                {
                    MessageBox.Show("Умножение выполнено.");
                    buttonTact.Enabled = false;
                    buttonAuto.Enabled = false;
                }
            }
            else // иначе (если на уровне УА и ОА)
            {
                controlMachine.ExecuteTact();
                UpdateVariables();
                UpdateSings();
                UpdateStructScheme(); // обновляем структурную схему
                if (axMemory.ArrStateA[0] && isOver) // если в состоянии А0 и булевая переменная равна true
                {
                    MessageBox.Show("Умножение выполнено.");
                    buttonTact.Enabled = false;
                    buttonAuto.Enabled = false;
                }
            }
        }

        private void buttonAuto_Click(object sender, EventArgs e) // Автоматический режим
        {
            if (!MP_YAOA) // Если на уровне микропрограммы
            {
                do
                {
                    microProgram.ExecuteTact();
                    UpdateVariables();
                    UpdateSings();
                } while (!microProgram.ArrA[0]);
                MessageBox.Show("Умножение выполнено.");
            }
            else  // иначе (на уровне УА и ОА)
            {
                do
                {
                    controlMachine.ExecuteTact();
                    UpdateVariables();
                    UpdateSings();
                    UpdateStructScheme();
                } while (!isOver);
                MessageBox.Show("Умножение выполнено.");
            }
            buttonAuto.Enabled = false;
        }

        private void UpdateVariables()
        {
            // Символьные массивы для всех переменных, элемент массива - значение разряда числа
            char[] arrCharB = variables.ToCharArray(variables.B);
            char[] arrCharA = variables.ToCharArray(variables.A);
            char[] arrCharAM = variables.ToCharArray(variables.AM);
            char[] arrCharC = variables.ToCharArray(variables.C);
            char[] arrCharD = variables.ToCharArray(variables.D);
            char[] arrCharCount = variables.ToCharArray(variables.Count);

            // Посимвольная перезапись числа в двоичном коде, соответственно пустные разряды заполняются нулями
            for (int i = 0; i < arrResA.Length; i++)
            {
                if (i <= arrCharA.Length - 1)
                    arrResA[i].Text = " " + arrCharA[i].ToString() + " ";
                else
                    arrResA[i].Text = " 0 ";
                if (i <= arrCharB.Length - 1)
                    arrResB[i].Text = " " + arrCharB[i].ToString() + " ";
                else
                    arrResB[i].Text = " 0 ";
                if (i <= arrCharD.Length - 1)
                    arrResD[i].Text = " " + arrCharD[i].ToString() + " ";
                else
                    arrResD[i].Text = " 0 ";
            }
            for (int i = 0; i < arrResAM.Length; i++)
            {
                if (i <= arrCharAM.Length - 1)
                    arrResAM[i].Text = " " + arrCharAM[i].ToString() + " ";
                else
                    arrResAM[i].Text = " 0 ";
                if (i <= arrCharC.Length - 1)
                    arrResC[i].Text = " " + arrCharC[i].ToString() + " ";
                else
                    arrResC[i].Text = " 0 ";
            }
            for (int i = 0; i < arrCurCount.Length; i++)
            {
                if (i <= arrCharCount.Length - 1)
                    arrCurCount[i].Text = " " + arrCharCount[i].ToString() + " ";
                else
                    arrCurCount[i].Text = " 0 ";
            }
        }

        private void UpdateStructScheme() // обновление значений на структурной схеме
        {
            for (int i = 0; i < arrCS_Y.Length-1; i++)
            {
                if (i < arrCS_D.Length)
                {
                    if (axMemory.CurStateCode[i] == true)
                        arrStateMemory[i].Text = " 1 ";
                    else
                        arrStateMemory[i].Text = " 0 ";
                    if (cs_D.NextStateCode[i] == true)
                        arrCS_D[i].Text = " 1 ";
                    else
                        arrCS_D[i].Text = " 0 ";
                }
                if (i < arrDS.Length)
                {
                    if (axMemory.ArrStateA[i] == true)
                        arrDS[i].Text = " 1 ";
                    else
                        arrDS[i].Text = " 0 ";
                }
                if (i < arrStateConditions.Length)
                {
                    if (axMemory.ConditionsX[i] == true)
                        arrStateConditions[i].Text = " 1 ";
                    else
                        arrStateConditions[i].Text = " 0 ";
                }
                if (cs_Y.OperationsY[i] == true)
                    arrCS_Y[i].Text = " 1 ";
                else
                    arrCS_Y[i].Text = " 0 ";
            }
        }

        private void UpdateSings() // обновление меток
        {
            bool[] statesA = new bool[4];
            if (!MP_YAOA)
            {
                statesA = microProgram.ArrA;
            }
            else
            {
                statesA = axMemory.ArrStateA;
            }
            FalseSings(); // все метки становятся равными false
            if (statesA[0])
            {
                if ((microProgram.CisNull || cs_Y.OperationsY[0]) && tact1)
                {
                    checkBox1.Checked = true;
                    radioA0end.Checked = true;
                }
                else if ((microProgram.NegC || cs_Y.OperationsY[14]) && !tact1)
                {
                    checkBox8.Checked = true;
                    radioA0end.Checked = true;
                }
                else
                {
                    radioA0end.Checked = true;
                }
                isOver = true;
            }
            else if (statesA[1])
            {
                checkBox2.Checked = true;
                radioA1.Checked = true;
                tact1 = false;
                checkStart.Checked = false;
            }
            else if (statesA[2])
            {
                if(microProgram.WayOfCircle[1] || cs_Y.OperationsY[7])
                    checkBox4.Checked = true;
                else if (microProgram.WayOfCircle[2] || cs_Y.OperationsY[8])
                    checkBox5.Checked = true;
                else if(microProgram.WayOfCircle[3] || cs_Y.OperationsY[9])
                    checkBox6.Checked = true;
                else
                    checkBox3.Checked = true;
                radioA2.Checked = true;
            }
            else if (statesA[3])
            {
                checkBox7.Checked = true;
                radioA3.Checked = true;
            }
        }


        private void FalseSings() // флажки состояний и блоков операций становятся false
        {
            radioA0.Checked = false;
            radioA1.Checked = false;
            radioA2.Checked = false;
            radioA3.Checked = false;
            radioA0end.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
        }



        private void buttonClear_Click(object sender, EventArgs e) // нажатие кнопки "очистить"
        {                                               // приводит в исходной состояние программу
            FalseSings();
            variables.A = 0;
            variables.B = 0;
            variables.C = 0;
            variables.D = 0;
            variables.AM = 0;
            variables.Count = 0;
            UpdateVariables();
            for (int i = 0; i < arrCS_Y.Length; i++)
            {
                if (i < arrCS_D.Length)
                {
                    arrStateMemory[i].Text = " 0 ";
                    arrCS_D[i].Text = " 0 ";
                }
                if (i < arrDS.Length)
                {
                    arrDS[i].Text = " 0 ";
                }
                if (i < arrStateConditions.Length)
                {
                    arrStateConditions[i].Text = " 0 ";
                }
                arrCS_Y[i].Text = " 0 ";
            }
        }
    }
}
