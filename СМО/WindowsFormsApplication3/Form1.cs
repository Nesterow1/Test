using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        static long Factorial(int n)  //факториал
        {
            if (n == 0)
                return 1;
            else
                return n * Factorial(n - 1);
        }
        double Ny;   //интенсивность потока обслуживания
        double La;   //интенсивность потока заявок
        double p0;   //вероятность того, что в системе находится 0 заявок 
        double p1;   //вероятность того, что в системе находится 1 заявка
        double p2;   //2 заявки
        double p3;   //3 заявки
        double pn;   //n заявок она же вероятность отказа
        double q;    //относительная пропускная способность q
        double A;    //абсолютную пропускную способность А
        double potk; //вероятность отказа
        int n;       //количество сверверов
        double Fact = 0;     //храним факториал
        double RoPow = 0;    //храним степень ро
        double Ro;   //интенсивность нагрузки канала
        double kzan; //среднее число занятых каналов
        double Loch; //Средняя длина очереди
        double Toch; //среднее время ожидания в очереди
        int m; //длина очереди
        double poch; //Вероятность образования очереди
        double nz; //Среднее число занятых каналов
        double npr; //Среднее число простаивающих каналов
        double kz; //Коэффициент занятости каналов
        double kpr; //Коэффициент простоя каналов
        double Tsmo; //Среднее время пребывания заявки в системе
        double Lsmo; //Среднее число заявок в системе
        double nsv; //среднее число свободных каналов
        public void OtkazOdin() //Одноканальная СМО с отказами в обслуживании
        {
            
            p0 = Ny / (Ny + La); //вероятность того, что в системе находится 0 заявок 
            p1 = La / (Ny + La); //вероятность того, что в системе находится 1 заявка
            q = p0; //относительная пропускная способность q
            A = La * q; //абсолютную пропускную способность А
            potk = 1 - q;//вероятность отказа
        }
        public void OtkazMnogo() //Многоканальная СМО с отказами в обслуживании
        {

            p0 = 0;
            Fact = 0;
            RoPow = 0;
            Ro = La / Ny; //интенсивность нагрузки канала
            for (int i = 0; i < n; i++)
            {
                Fact = Factorial(i);  //факториал
                RoPow = Math.Pow(Ro, i); //степень Ro
                p0 += RoPow / Fact;     //вероятность того, что в системе находится 0 заявок 
            }
            p1 = Ro * p0;  //одна заявка
            p2 = (Math.Pow(Ro, 2) / Factorial(2)) * p0;  //две заявки
            p3 = (Math.Pow(Ro, 3) / Factorial(3)) * p0;  //три заявки
            pn = (Math.Pow(Ro, n) / Factorial(n)) * p0;  //n заявок она же вероятность отказа
            q = 1 - pn;
            A = La * q;
            kzan = A / Ny; //среднее число занятых каналов
        }
        public void OgranOdin() //Одноканальная СМО с ограниченной длиной очереди
        {
            Ro = La / Ny; //интенсивность нагрузки канала
            if (Ro == 1)
            {
                p0 = 1 / (m + 2);
            }
            else
            {
                p0 = (1 - Ro) / (1 - Ro); //вероятность того, что в системе находится 0 заявок 
            }
            p1 = Ro * p0;   //Одна заявка
            p2 = p1 * Ro;   //две заявки
            p3 = p2 * Ro;   //три заявки
            pn = Math.Pow(Ro, m + 1) * p0;  //вероятность того, что в системе находится m+1 заявок она же вероятность отказа
            q = 1 - pn; //относительная пропускная способность q
            Loch = (Ro * Ro) * ((1 - Math.Pow(Ro, m) * (m - m * Ro + 1)) / (Math.Pow(1 - Ro, 2)) * p0);  //Средняя длина очереди
            Toch = Loch / La;  //среднее время ожидания в очереди.
        }
        public void OgranMnog() //Многоканальная СМО с ограниченной длиной очереди
        {
            p0 = 0;
            Fact = 0;     //храним факториал
            RoPow = 0;    //храним степень ро
            Ro = La / Ny; //интенсивность нагрузки канала
            for (int i = 0; i < n; i++)
            {
                Fact = Factorial(i);  //факториал
                RoPow = Math.Pow(Ro, i); //степень Ro
                p0 += RoPow / Fact;
            }//помним, что n - const
            pn = RoPow / Fact; //храним эту дробь
            for (int i = 1; i <= m; i++)
            {
                RoPow = Math.Pow(Ro, n + i); //степень Ro
                Fact = Factorial(n) * Math.Pow(n, i);
                p0 += RoPow / Fact;
            }
            p0 = 1 / p0; //вероятность того, что в системе находится 0 заявок 
            pn = pn * p0;  //вероятность что все каналы заняты
            p1 = p0 * (Math.Pow(Ro, 1) / Factorial(1)); //1 канал занят
            p2 = p0 * (Math.Pow(Ro, 2) / Factorial(2)); //2 канал занято
            p3 = p0 * (Math.Pow(Ro, 3) / Factorial(3)); //2 канал занято
            potk = p0 * (Math.Pow(Ro, n + m) / (Factorial(n) * Math.Pow(n, m))); //Вероятность отказа
            q = 1 - potk; //Вероятность отказа
            A = La * q;
            nz = Ro * q; //Среднее число занятых каналов
            npr = n - nz; //Среднее число простаивающих каналов
            kz = nz / n; //Коэффициент занятости каналов
            kpr = 1 - kz; //Коэффициент простоя каналов
            Loch = (Math.Pow(Ro, n + 1) / (Factorial(n) * n)) * ((1 - (Math.Pow((Ro / n), 2)) * (m + 1 - (m / n) * Ro)) / (Math.Pow((1 - (Ro / n)), 2)));
            Loch = Loch * p0; //Среднее число заявок, находящихся в очереди
            Toch = Loch / La; //Среднее время ожидания в очереди
            Tsmo = Toch + (q / Ny); //Среднее время пребывания заявки в системе


        }
        public void NeogranOdin() //Одноканальная СМО с неограниченной очередью
        {
            
            Ro = La / Ny; //интенсивность нагрузки канала
            p0 = 1 - Ro; //вероятность того, что в системе находится 0 заявок 
            p1 = Math.Pow(Ro, 1) * (1 - Ro);  //вероятность нахождения одной заявки
            p2 = p1 * Ro; //2 
            p3 = p2 * Ro; //3
            Loch = (Ro * Ro) / (1 - Ro); //Среднее число заявок в очереди
            Lsmo = Loch + Ro; //Среднее число заявок в системе
            Toch = Loch / La; //Среднее время ожидания обслуживания в очереди
            Tsmo = Lsmo / La; //Среднее время пребывания заявки в системе
            q = 1 - pn; //относительная пропускная способность q
        }
        public void NeogranMnog() //Многоканальная СМО с неограниченной очередью
        {
            p0 = 0;
            Ro = La / Ny; //интенсивность нагрузки канала
            for (int i = 0; i < n; i++)
            {
                Fact = Factorial(i);  //факториал
                RoPow = Math.Pow(Ro, i); //степень Ro
                p0 += RoPow / Fact;
            }
            p0 += (Math.Pow(Ro, n + 1) / Factorial(n));
            p0 = 1 / p0;  ////вероятность того, что в системе находится 0 заявок 
            p1 = Ro / Factorial(1) * p0; //1 заявка
            p2 = Ro / Factorial(2) * p0; //2 заявки
            p3 = Ro / Factorial(3) * p0; //3 заявки
            potk = 0; //вероятность отказа
            A = La;
            pn = p0 * (Math.Pow(Ro, n) / Factorial(n));  //все каналы заняты
            Loch = p0 * (Math.Pow(Ro, n + 1) / n * Factorial(n) * (1 - (Ro / n) * (1 - (Ro / n))));  //среднее число заявок в очереди
            poch = p0 * (Math.Pow(Ro, n + 1) / Factorial(n) * (n - Ro)); // вероятность оказаться в очереди
            nsv = n - Ro; //среднее число свободных каналов
            kz = Ro / n; // коэффициент занятости каналов
            Lsmo = Loch + Ro; //среднее число заявок в очереди
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (radioButton2.Checked == true)
            {
                La = Convert.ToInt32(textBox3.Text);
                Ny = Convert.ToInt32(textBox2.Text);
                OtkazOdin();
                richTextBox1.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox1.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox1.Text += Convert.ToString(q) +  " относительная пропускная способность q\n";
                richTextBox1.Text += Convert.ToString(A) + " абсолютную пропускную способность А\n";
                richTextBox1.Text += Convert.ToString(potk) + " вероятность отказа\n";
            }
            if (radioButton1.Checked == true)
            {
                La = Convert.ToInt32(textBox3.Text);
                Ny = Convert.ToInt32(textBox2.Text);
                n = Convert.ToInt32(textBox1.Text);
                OtkazMnogo();
                richTextBox1.Text += Convert.ToString(Ro) + " интенсивность нагрузки канала\n";
                richTextBox1.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox1.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox1.Text += Convert.ToString(p2) + " в системе находится 2 заявки\n";
                richTextBox1.Text += Convert.ToString(p3) + " в системе находится 3 заявки\n";
                richTextBox1.Text += Convert.ToString(q) + " относительная пропускная способность q\n";
                richTextBox1.Text += Convert.ToString(A) + " абсолютную пропускную способность А\n";
                richTextBox1.Text += Convert.ToString(pn) + " вероятность отказа\n";
                richTextBox1.Text += Convert.ToString(kzan) + " среднее число занятых каналов\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            if (radioButton3.Checked == true)
            {
                La = Convert.ToInt32(textBox4.Text);
                Ny = Convert.ToInt32(textBox5.Text);
                m = Convert.ToInt32(textBox7.Text);
                OgranOdin();
                richTextBox2.Text += Convert.ToString(Ro) + " интенсивность нагрузки канала\n";
                richTextBox2.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox2.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox2.Text += Convert.ToString(p2) + " в системе находится 2 заявки\n";
                richTextBox2.Text += Convert.ToString(p3) + " в системе находится 3 заявки\n";
                richTextBox2.Text += Convert.ToString(q) + " относительная пропускная способность q\n";
                richTextBox2.Text += Convert.ToString(Loch) + " Средняя длина очереди\n";
                richTextBox2.Text += Convert.ToString(Toch) + " среднее время ожидания в очереди\n";
                richTextBox2.Text += Convert.ToString(pn) + " вероятность отказа\n";
                
            }
            if (radioButton4.Checked == true)
            {
                La = Convert.ToInt32(textBox4.Text);
                Ny = Convert.ToInt32(textBox5.Text);
                m = Convert.ToInt32(textBox7.Text);
                n = Convert.ToInt32(textBox6.Text);
                OgranMnog();
                richTextBox2.Text += Convert.ToString(Ro) + " интенсивность нагрузки канала\n";
                richTextBox2.Text += Convert.ToString(pn) + " вероятность что все каналы заняты\n";
                richTextBox2.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox2.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox2.Text += Convert.ToString(p2) + " в системе находится 2 заявки\n";
                richTextBox2.Text += Convert.ToString(p3) + " в системе находится 3 заявки\n";
                richTextBox2.Text += Convert.ToString(poch) + " вероятность образования очереди\n";
                richTextBox2.Text += Convert.ToString(q) + " вероятность отказа\n";
                richTextBox2.Text += Convert.ToString(A) + " абсолютную пропускную способность А\n";
                richTextBox2.Text += Convert.ToString(nz) + " Среднее число занятых каналов\n";
                richTextBox2.Text += Convert.ToString(npr) + " Среднее число простаивающих каналов\n";
                richTextBox2.Text += Convert.ToString(kz) + " Коэффициент занятости каналов\n";
                richTextBox2.Text += Convert.ToString(kpr) + " Коэффициент простоя каналов\n";
                richTextBox2.Text += Convert.ToString(Loch) + " Средняя длина очереди\n";
                richTextBox2.Text += Convert.ToString(Toch) + " среднее время ожидания в очереди\n";
                richTextBox2.Text += Convert.ToString(Tsmo) + " Среднее время пребывания заявки в системе\n";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            if (radioButton5.Checked == true)
            {
                La = Convert.ToInt32(textBox8.Text);
                Ny = Convert.ToInt32(textBox9.Text);
                
                NeogranOdin();
                richTextBox3.Text += Convert.ToString(Ro) + " интенсивность нагрузки канала\n";
                richTextBox3.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox3.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox3.Text += Convert.ToString(p2) + " в системе находится 2 заявки\n";
                richTextBox3.Text += Convert.ToString(p3) + " в системе находится 3 заявки\n";
                richTextBox3.Text += Convert.ToString(kz) + " коэффициент занятости каналов\n";
                richTextBox3.Text += Convert.ToString(Loch) + " Средняя длина очереди\n";
                richTextBox3.Text += Convert.ToString(Toch) + " вероятность оказаться в очереди\n";
                richTextBox3.Text += Convert.ToString(q) + " относительная пропускная способность q\n";

                
            }
            if (radioButton6.Checked == true)
            {
                La = Convert.ToInt32(textBox8.Text);
                Ny = Convert.ToInt32(textBox9.Text);
                
                n = Convert.ToInt32(textBox10.Text);
                NeogranMnog();
                richTextBox3.Text += Convert.ToString(Ro) + " интенсивность нагрузки канала\n";
                richTextBox3.Text += Convert.ToString(p0) + " в системе находится 0 заявок\n";
                richTextBox3.Text += Convert.ToString(p1) + " в системе находится 1 заявка\n";
                richTextBox3.Text += Convert.ToString(p2) + " в системе находится 2 заявки\n";
                richTextBox3.Text += Convert.ToString(p3) + " в системе находится 3 заявки\n";
                richTextBox3.Text += Convert.ToString(kz) + " коэффициент занятости каналов\n";
                richTextBox3.Text += Convert.ToString(Loch) + " Средняя длина очереди\n";
                richTextBox3.Text += Convert.ToString(poch) + " вероятность оказаться в очереди\n";
                richTextBox3.Text += Convert.ToString(pn) + " все каналы заняты\n";
                richTextBox3.Text += Convert.ToString(Lsmo) + " среднее число заявок в очереди\n";
            }
        }

        private void button7_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
