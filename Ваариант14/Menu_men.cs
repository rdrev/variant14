using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ваариант14
{
    public partial class Menu_men : Form
    {
        SqlConnection sqlConn;

        public static class Glab
            //метод глобальных переменых
        {
            public static List<int> idList = new List<int>();
            public static List<string> str = new List<string>();
            public static int id = 0;//хранение логина пользователя 
        }

        public Menu_men()
        {
            InitializeComponent();
        }
        private async void Menu_men_Load(object sender, EventArgs e)
            //настройка первычных параметров
        {
            string ConnStr = @"Data Source=62.63.74.62,1433;
            Initial Catalog=Variant14; 
            User ID=work;
            Password=1954";//строка подключения

            sqlConn = new SqlConnection(ConnStr);

            await sqlConn.OpenAsync();

            comboBox1.SelectedItem = "по наз ЖК";//значение по умолчанию

            this.comboBox1.SelectedIndexChanged +=
            new System.EventHandler(ComboBox1_SelectedIndexChanged);
            this.comboBox2.SelectedIndexChanged +=
            new System.EventHandler(ComboBox2_SelectedIndexChanged);
            this.comboBox3.SelectedIndexChanged +=
            new System.EventHandler(ComboBox3_SelectedIndexChanged);
            //задоеи метода на отслежку изменения комбобоксов

            listView1.GridLines = true;//Выводим элементы управление 

            listView1.FullRowSelect = true;//Для выделение строки

            listView1.View = View.Details;//отображения текста

            listView1.Columns.Add("жилой комплекс");
            listView1.Columns.Add("улицы");
            listView1.Columns.Add("номера дома");
            listView1.Columns.Add("статус строительства ЖК");
            listView1.Columns.Add("количества проданных квартир");
            listView1.Columns.Add("количества продающихся квартир");
            //добоаляем название столбы


            listView2.GridLines = true;//Выводим элементы управление 

            listView2.FullRowSelect = true;//Для выделение строки

            listView2.View = View.Details;//отображения текста

            
            listView2.Columns.Add("номер квартиры");
            listView2.Columns.Add("площади квартиры");
            listView2.Columns.Add("количества комнат");
            listView2.Columns.Add("стоимость строительства");
            listView2.Columns.Add("добавленная стоимость");
            listView2.Columns.Add("стоимость квартиры ");
            //добоаляем название столбы

            obnov();//запускаем загрузку даных 
        }
       
    
        private async void obnov()
            //запускаем загрузку даных 
        {
            await HouseObnov();//метод для загрузки таблици с домами
            await ApartmentObnov();//метод для загрузки таблици с квартирами
        }
        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
            //кнопка для обновление даных
        {
            obnov();
        }


        private async Task HouseObnov()
            //метод для загрузки таблици с домами
        {
            listView1.Items.Clear();//очишаем таблицу 


            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение  запроса

            List<SqlCommand> comendProdono = new List<SqlCommand>(),
                comendNoProdono = new List<SqlCommand>();// масивы для перемеена для хранение  запроса

            List<string> House = new List<string>(),
                Name = new List<string>(),
                Street = new List<string>(),
                Number = new List<string>(),
                Status = new List<string>();
            //масив для зранение столбцов вывода

            List<object> Prodono = new List<object>(),
                 NoProdono = new List<object>();
            //масив для зранение столбцов вывода

            string comondProto = "SELECT DISTINCT [House].[ID], [Name], [Street], [House].[Number], [Status]" +
                "FROM [House], [ResidentialComplex]";
            //прототип функцие конечный пезультат зависит от парамеиров в форме

            if (string.IsNullOrEmpty(streetBox.Text) && string.IsNullOrWhiteSpace(streetBox.Text) &&
                    string.IsNullOrEmpty(ZKBox.Text) && string.IsNullOrWhiteSpace(ZKBox.Text))
            {
                if (comboBox1.Text == "по наз ЖК")
                    comend = new SqlCommand(comondProto + " ORDER BY [Name]", sqlConn);
                else if (comboBox1.Text == "по улице")
                    comend = new SqlCommand(comondProto + " ORDER BY [Street]", sqlConn);
                else if (comboBox1.Text == "по номеру дома")
                    comend = new SqlCommand(comondProto + " ORDER BY [Number]", sqlConn);
                //параметры сортеровки
            }
            //если не указаны поля улици и ЖК

            else if (string.IsNullOrEmpty(streetBox.Text) && string.IsNullOrWhiteSpace(streetBox.Text) &&
                              !string.IsNullOrEmpty(ZKBox.Text) && !string.IsNullOrWhiteSpace(ZKBox.Text))
            {

                comondProto += " WHERE [Name] = @name";

                if (comboBox1.Text == "по наз ЖК")
                    comondProto += " ORDER BY [Name]";

                else if (comboBox1.Text == "по улице")
                    comondProto += " ORDER BY [Street]";

                else if (comboBox1.Text == "по номеру дома")
                    comondProto += " ORDER BY [Number]";

                comend = new SqlCommand(comondProto, sqlConn);

                comend.Parameters.AddWithValue("name", ZKBox.Text);

                //параметры сортеровки
            }
            //если указан только ЖК

            else if (!string.IsNullOrEmpty(streetBox.Text) && !string.IsNullOrWhiteSpace(streetBox.Text) &&
                            string.IsNullOrEmpty(ZKBox.Text) && string.IsNullOrWhiteSpace(ZKBox.Text))
            {

                comondProto += " WHERE [Street] = @street";

                if (comboBox1.Text == "по наз ЖК")
                    comondProto += " ORDER BY [Name]";

                else if (comboBox1.Text == "по улице")
                    comondProto += " ORDER BY [Street]";

                else if (comboBox1.Text == "по номеру дома")
                    comondProto += " ORDER BY [Number]";

                comend = new SqlCommand(comondProto, sqlConn);

                comend.Parameters.AddWithValue("@street", streetBox.Text);

                //параметры сортеровки
            }
            //если указан только улица

            else if (!string.IsNullOrEmpty(streetBox.Text) && !string.IsNullOrWhiteSpace(streetBox.Text) &&
                                !string.IsNullOrEmpty(ZKBox.Text) && !string.IsNullOrWhiteSpace(ZKBox.Text))
            {

                comondProto += " WHERE [Street] = @street AND [Name] = @name";

                if (comboBox1.Text == "по наз ЖК")
                    comondProto += " ORDER BY [Name]";

                else if (comboBox1.Text == "по улице")
                    comondProto += " ORDER BY [Street]";

                else if (comboBox1.Text == "по номеру дома")
                    comondProto += " ORDER BY [Number]";

                comend = new SqlCommand(comondProto, sqlConn);

                comend.Parameters.AddWithValue("@street", streetBox.Text);
                comend.Parameters.AddWithValue("@name", ZKBox.Text);

                //параметры сортеровки
            }
            //если указан Улица и ЖК

            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            try
            {
                sqlRead = await comend.ExecuteReaderAsync();//создаем запрос


                while (await sqlRead.ReadAsync())
                {
                    House.Add(Convert.ToString(sqlRead["Name"]));
                    Street.Add(Convert.ToString(sqlRead["Street"]));
                    Number.Add(Convert.ToString(sqlRead["Street"]));
                    Status.Add(Convert.ToString(sqlRead["Street"]));
                    //водим результат в масивы

                    comendProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 1", sqlConn));
                    comendNoProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 0", sqlConn));
                    //генирируем запрос на количество проданых квартир
                }

                if (sqlRead != null)
                    sqlRead.Close();//проверка на откратасть

                foreach (SqlCommand comm in comendProdono)
                {
                    Prodono.Add(await comm.ExecuteScalarAsync());//находим количество проданых домов
                }
                foreach (SqlCommand comm in comendNoProdono)
                {
                    NoProdono.Add(await comm.ExecuteScalarAsync());//находим количество не проданых домов
                }
                for (int i = 0; i < House.Count; i++)
                    listView1.Items.Add
                       (new ListViewItem(new string[]
                           {
                            House[i],
                            Street[i],
                            Number[i],
                            Status[i],
                            Convert.ToString(Prodono[i]),
                            Convert.ToString(NoProdono[i])
                         })//водим результат в таблицу 
                       );

            }
            catch (Exception ex)//обработка исключений
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlRead != null)
                    sqlRead.Close();//проверка на откратасть 

            }
        }
        private async void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
            //обновление таблицы при изменением комбобокса
        {
            await HouseObnov();
        }

        private async Task ApartmentObnov()
            //метод для загрузки в комбобокс
        {
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox5.Visible = false;
            comboBox6.Visible = false;
            //скрываем комбобокса

            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            //скрываем лейбал

            comboBox2.Items.Clear();//очишаем

            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение  запроса

            comend = new SqlCommand("SELECT DISTINCT [Name]" +
                " FROM [ResidentialComplex]", sqlConn);
            //команда зпроса

            sqlRead = await comend.ExecuteReaderAsync();//запрос в базу

            while (await sqlRead.ReadAsync())
                comboBox2.Items.Add(Convert.ToString(sqlRead["Name"]));//выводим название ЖК в комбобокс
            

            sqlRead.Close();//закрываем вывод
        }

        private void ComboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
            //метод для загрузки в комбобокс зависимый от комбобокса2
        {
            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение вывода  запроса

            comboBox3.Items.Clear();//очишаем таблицу

            comboBox3.Visible = true;
            label6.Visible = true;
            //показываем поле

            comend = new SqlCommand("SELECT DISTINCT [House].[ID], [Street], [Number]" +
                " FROM [House], [ResidentialComplex]" +
                "WHERE [Name] = @name", sqlConn);
            //команда запроса
            comend.Parameters.AddWithValue("@name", comboBox2.Text);//водим параметор

            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead = comend.ExecuteReader();//запрос в базу

            
            while (sqlRead.Read())
            {
                comboBox3.Items.Add("ул." + Convert.ToString(sqlRead["Street"])
                    + " д." + Convert.ToString(sqlRead["Number"]));//запись в кобобобокс

                Glab.idList.Add(Convert.ToInt32(sqlRead["ID"]));//запись id

                Glab.str.Add("ул." + Convert.ToString(sqlRead["Street"])
                    + " д." + Convert.ToString(sqlRead["Number"]));//запись справачника
            }

            sqlRead.Close();//закрываем запрс
        }
        private void ComboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
        //метод для загрузки в комбобокс зависимый от комбобокса3
        {
            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение вывода  запроса

            comboBox4.Items.Clear();//очишаем таблицу


            foreach (int i in Glab.idList)
            {
                if (Glab.str[Glab.id] == comboBox3.Text) 
                    break;

                Glab.id++;//вычеслем id выброного дома
            }

            comend = new SqlCommand("SELECT DISTINCT [Floor]" +
                " FROM  [Apartment]" +
                "WHERE [HouseID] = @House"
                , sqlConn);//команда запроса
            comend.Parameters.AddWithValue("@House", Glab.id);//водим параметор


            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead = comend.ExecuteReader();//запрос в базу


            while (sqlRead.Read())
                comboBox4.Items.Add(Convert.ToString(sqlRead["Floor"]));
            // запись в кобобобокс


            comboBox4.Visible = true;
            label5.Visible = true;
            //показываем поле

            comboBox6.Items.Clear();//очишаем таблицу

            comend = new SqlCommand("SELECT DISTINCT [Section]" +
                " FROM  [Apartment]" +
                "WHERE [HouseID] = @House"
                , sqlConn);//водим параметор
            comend.Parameters.AddWithValue("@House", Glab.id);//водим параметор

            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead =comend.ExecuteReader();//запрос в базу


            while (sqlRead.Read())
                comboBox6.Items.Add(Convert.ToString(sqlRead["Section"]));
            // запись в кобобобокс

            comboBox5.Visible = true;
            comboBox6.Visible = true;
            //показываем поле

            sqlRead.Close();//закрываем запрс
        }
        private void ApartmentButton_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();//очишаем таблицу

            SqlDataReader sqlRead = null;

            SqlCommand comend = new SqlCommand(
                "SELECT [Number], [Area], [CountOfRooms], [BuildingCost], [ApartmentValueAdded]" +
                 "FROM [Apartment]" +
                 "WHERE [HouseID] = @house AND [Section] = @section AND [Floor] = @floor AND [IsSold] = @isSold",
                  sqlConn);
            //команда зпроса


            comend.Parameters.AddWithValue("house", Glab.id);
            comend.Parameters.AddWithValue("section", comboBox6.Text);
            comend.Parameters.AddWithValue("floor", comboBox4.Text);
            
            if (comboBox5.Text == "Да")
                comend.Parameters.AddWithValue("isSold", true);
            else
                comend.Parameters.AddWithValue("isSold", false);
            //водим параметор

            sqlRead = comend.ExecuteReader();//запрос в базу


            while (sqlRead.Read())
            {
                listView2.Items.Add
                      (new ListViewItem(new string[]
                          {
                              Convert.ToString(sqlRead["Number"]),
                              Convert.ToString(sqlRead["Area"]),
                              Convert.ToString(sqlRead["CountOfRooms"]),
                              Convert.ToString(sqlRead["BuildingCost"]),
                              Convert.ToString(sqlRead["ApartmentValueAdded"]),
                               Convert.ToString(Convert.ToInt32(sqlRead["BuildingCost"]) + Convert.ToInt32(sqlRead["ApartmentValueAdded"]))
                        })//водим результат в таблицу 
                      );
            }
            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 
        }
    }
}
