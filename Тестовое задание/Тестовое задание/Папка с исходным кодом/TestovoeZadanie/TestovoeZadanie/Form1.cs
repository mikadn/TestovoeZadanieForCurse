//ОСНОВНОЕ ЗАДАНИЕ (суть задания):
//Имеется два входных файла – файл_с_текстом и файл_словаря. Файл_с_текстом содержит текст в виде 
//последовательностей текстовых строк, разделенных стандартным разделителем строки. Файл_словаря 
//содержит произвольное количество строк, каждая из которых содержит ровно одно слово.
//Необходимо создать программу, создающую корректный HTML_файл, содержащий весь текст из файла_с_текстом 
//в котором помечены жирным и наклонным шрифтом все слова из файла_словаря. HTML_файл должен отображаться 
//в любом Internet-браузере (IE, Firefox, Chrome и др.) корректно, показывая весь текст, с выделением 
//указанных слов. 
//ОБЯЗАТЕЛЬНЫЕ ТРЕБОВАНИЯ:
//1. Максимальные размеры входных файлов: файл_с_текстом – 2 Мб, файл_словаря – 100 000 строк (2 Мб).
//2. При обработке файла_с_текстом нельзя целиком его загружать в память компьютера. Файл_словаря наоборот
//нужно целиком загрузить в память, используя эффективные по времени структуры данных.
//3. Сгенерированный HTML_файл должен быть загружен любым браузером (в том числе IE, FireFox и другими)
//за время не более 5-10 секунд.
//4. Если получившийся HTML_файл превыщает размер N строк, то его необходимо разбить на несколько файлов,
//каждый не более N строк. При разбиении необходимо учитывать структуру текста. Желательно не делить 
//одно предложение на 2 разных HTML_файла. (N задается через константу или через параметры
//строки в интервале от 10 строк до 100000 строк)
//5. В программе необходимо предусмотреть обработку нештатных ситуаций (Например, выбранный файл 
//заблокирован, или имеет неправильную структуру и т.д.)
//6. Программа должна обеспечивать корректную обработку исключительных ситуаций (exceptions).
//7. В решении необходимо использовать стандартные классы – например, если в программе требуется список 
//или словарь, тогда нужно использовать не собственные вручную написанные списки и словари, а имеющиеся в 
//стандартной библиотеке.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestovoeZadanie
{
    public partial class Form1 : Form
    {
        //Блок обьявления переменных для хранения///////////////////////////////////////
        List<string> list = new List<string>();//Для хранения словаря
        int kolstrok = 10;//Для хранения лимита строк html в файле
        int schet = 0;//Счетчик для определения нужности создать следующий html файл
        string stroka = "";//Строка для хранения кусков обработанного текста
        ////////////////////////////////////////////////////////////////////////////////
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            button3.Visible = false;
            numericUpDown1.Visible = false;
            label1.Text = "Выберите файл словаря";

        }
        //Блок загрузки, обработки и преобразования файла текста //////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "txt files (*.txt)|*.txt";
                openFileDialog1.ShowDialog();
                FileStream f2 = new FileStream(openFileDialog1.FileName, FileMode.Open);
                MessageBox.Show("Файл с текстом выбран. Выберите место сохранения html файла");
                saveFileDialog1.Filter = "html files (*.html)|*.html";
                //Проверка на максимальный размер файла и создание html файла 
                if (f2.Length <= 2097152)
                {                   
                    button1.Visible = false;
                    f2.Close();
                    System.IO.StreamReader reader = new System.IO.StreamReader(openFileDialog1.FileName, Encoding.Default);
                    string line2 = "";//Переменная для временного хранения загружаемой строки текста
                    bool prov = false;// Переменная для проверки изменяли ли слово или нет                    
                    while ((line2 = reader.ReadLine()) != null)
                    {
                        string[] mas = line2.Split(new char[] { ' ' });//Массив для хранения слов из считанной строки
                        //Действия для создания нескольких html файлов
                        if (schet==kolstrok )
                        {                            
                            saveFileDialog1.ShowDialog();                            
                            System.IO.StreamWriter s = new System.IO.StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                            s.WriteLine("<html> <head><title>Итог</title></head><body>");
                            s.WriteLine(stroka);
                            s.WriteLine("</body></html>");    
                            s.Close();
                            stroka = "";
                            schet = 0; 
                            MessageBox.Show("Количество строк достигло лимита. Выберите место для следующего файла");
                        }                                                   
                        //
                        schet += 1;//Подсчет строк
                        //Сверение со словарем и внесение тегов
                        foreach (string i in mas)
                        {
                            prov = false;
                            foreach (string j in list)
                            {
                                if (i.ToUpper().Contains(j.ToUpper()) == true) { stroka += ("<b><i>" + i + "</i></b> "); prov = true; break; }
                            }
                            if (prov != true) { stroka+=(i + " "); }
                        }
                        stroka+=("<br />");
                        //
                        
                    }
                    //Запись в html или остатков или одиночного файла
                    saveFileDialog1.ShowDialog();
                    System.IO.StreamWriter s2 = new System.IO.StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    s2.WriteLine("<html> <head><title>Итог</title></head><body>");
                    s2.WriteLine(stroka);
                    s2.WriteLine("</body></html>");
                    s2.Close();   
                    reader.Close();
                    label1.Text = "Создание файла(лов) завершено";
                    //
                    
                }
                else { MessageBox.Show("Error. Файл больше 2 Мбайт"); }
            }
            catch (FileNotFoundException) { MessageBox.Show("Файл не найден"); }
            catch (ArgumentException) { MessageBox.Show("Файл не создан"); this.Close(); }
        }
        ////////////////////////////////////////////////////////////////
        //Блок загрузки файла словаря целиком в память/////   
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog2.Filter = "txt files (*.txt)|*.txt";
                openFileDialog2.ShowDialog();
                FileStream f1 = new FileStream(openFileDialog2.FileName, FileMode.Open);                
                //Проверка на максимальный размер файла
                if (f1.Length <= 2097152)
                {
                    button2.Visible = false;
                    button3.Visible = true;
                    numericUpDown1.Visible = true;
                    f1.Close();
                    //Загрузка в память
                    System.IO.StreamReader reader = new System.IO.StreamReader(openFileDialog2.FileName, Encoding.Default);
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    } 
                    reader.Close();
                    //
                    MessageBox.Show("Файл словаря загружен");
                    label1.Text = "Выберите лимит строк html файле";
                }
                else { MessageBox.Show("Error. Файл больше 2 Мбайт"); }
            }
            catch (FileNotFoundException) { MessageBox.Show("Файл не найден"); }
        }
        /////////////////////////////////////////////
        //Блок выбора максимального количества строк//
        private void button3_Click(object sender, EventArgs e)
        {
            kolstrok = Convert.ToInt32(numericUpDown1.Value);
            button1.Visible = true;
            button3.Visible = false;
            numericUpDown1.Visible = false;
            label1.Text = "Загрузите файл с текстом";
        }
        //////////////////////////////////////////////
       

    }
}
