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
            public static List<int> idResidentialComplexList = new List<int>();
            public static List<int> idHouseList = new List<int>();

            public static List<string> strResidentialComplex = new List<string>();
            public static List<string> strHouse = new List<string>();

            public static int idHouse = 0;
            public static int idResidentialComplex = 0;

            public static List<string> id = new List<string>();

            public static List<int>  Otsenka_IdApartment_List= new List<int>();
            public static List<int> Otsenka_IdHouse_List = new List<int>();
            public static List<int> Otsenka_IdResidentialComplex_List = new List<int>();

            public static List<int> Otsenka_CountOfRooms_List = new List<int>();
            public static List<int> Otsenka_Area_List = new List<int>();
            public static List<string> Otsenka_IsSold_List = new List<string>();

            public static List<int> Otsenka_ApartmentBuilding_List = new List<int>();
            public static List<int> Otsenka_ApartmentValueAdded_List = new List<int>();

            public static List<int> Otsenka_HouseBuilding_List = new List<int>();
            public static List<int> Otsenka_HouseValueAdded_List = new List<int>();

            public static List<int> Otsenka_ResidentialComplexBuilding_List = new List<int>();
            public static List<int> Otsenka_ComplexValueAdded_List = new List<int>();

            public static int scet = 0;
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


            // listView2.Columns.Add("жилой комплекс");
            listView2.Columns.Add("номер дома");
            listView2.Columns.Add("номер квартиры");
            listView2.Columns.Add("подезд");
            listView2.Columns.Add("этаж");
            listView2.Columns.Add("площади квартиры");
            listView2.Columns.Add("количества комнат");
            listView2.Columns.Add("статус");
            listView2.Columns.Add("стоимость строительства кв");
            listView2.Columns.Add("добавленная стоимость кв");
            //добоаляем название столбы

            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            //Скрываем вывод

            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;

            textBox1.Text = Convert.ToString(0);
            textBox2.Text = Convert.ToString(0);


            obnov();//запускаем загрузку даных 
        }

        ////////////////////////////////////////////////////////////////////////////////////////

        private void obnov()
        //запускаем загрузку даных 
        {
            HouseObnov();//метод для загрузки таблици с домами
            ApartmentFilitr();//метод для настройки фильтра
            ApartmentObnov();//метод для загрузки таблици с квартирами
        }
        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        //кнопка для обновление даных
        {
            obnov();
        }

        ////////////////////////////////////////////////////////////////////////////////////////


        private void HouseObnov()
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

            string comondProto = "SELECT DISTINCT [House].[ID], [Name], [Street], [Number], [Status]" +
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
                sqlRead = comend.ExecuteReader();//создаем запрос


                while (sqlRead.Read())
                {
                    House.Add(Convert.ToString(sqlRead["Name"]));
                    Street.Add(Convert.ToString(sqlRead["Street"]));
                    Number.Add(Convert.ToString(sqlRead["Number"]));
                    Status.Add(Convert.ToString(sqlRead["Status"]));
                    //водим результат в масивы

                    comendProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 1", sqlConn));
                    comendNoProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 0", sqlConn));
                    //генирируем запрос на количество проданых квартир
                }

                if (sqlRead != null)
                    sqlRead.Close();//проверка на откратасть

                foreach (SqlCommand comm in comendProdono)
                {
                    Prodono.Add(comm.ExecuteScalar());//находим количество проданых домов
                }
                foreach (SqlCommand comm in comendNoProdono)
                {
                    NoProdono.Add(comm.ExecuteScalar());//находим количество не проданых домов
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
        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        //обновление таблицы при изменением комбобокса
        {
            HouseObnov();
        }



        ////////////////////////////////////////////////////////////////////////////////////////


        private void ApartmentFilitr()//
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

            Glab.idResidentialComplexList.Clear();
            Glab.strResidentialComplex.Clear();
            //очищаем спарвачники


            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение  запроса

            comend = new SqlCommand("SELECT DISTINCT [ID], [Name]" +
                " FROM [ResidentialComplex]", sqlConn);
            //команда зпроса

            sqlRead = comend.ExecuteReader();//запрос в базу

            comboBox2.Items.Add("");//вставлем пустую строку как выбор нечиго

            while (sqlRead.Read())
            {
                comboBox2.Items.Add(Convert.ToString(sqlRead["Name"]));//выводим название ЖК в комбобокс

                Glab.strResidentialComplex.Add(Convert.ToString(sqlRead["Name"]));//Записаваем список ЖК для справки

                Glab.idResidentialComplexList.Add(Convert.ToInt32(sqlRead["ID"]));//Записаваем список ID ЖК для справки
            }

            sqlRead.Close();//закрываем вывод
        }

        private void ComboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        //метод для загрузки в комбобокс зависимый от комбобокса2
        {
            SqlDataReader sqlRead = null;//перемеена для хранение вывода  запроса

            SqlCommand comend = new SqlCommand();//перемеена для хранение вывода  запроса

            comboBox3.Items.Clear();//очишаем таблицу


            Glab.idHouseList.Clear();
            Glab.strHouse.Clear();
            //jxb

            comboBox3.Visible = true;
            label6.Visible = true;
            //показываем поле

            int id = 0;//переменая для счета

            foreach (int i in Glab.idResidentialComplexList)
            {
                if ("" == comboBox2.Text)//провареем не выбрано ли пусто
                    break;
                else if (Glab.strResidentialComplex[id] == comboBox2.Text)//находим выбраный ЖК
                    break;

                id++;//вычеслем id выброного ЖК
            }

            id++;

            Glab.idResidentialComplex = id;//Хроним id выброного ЖК для последоешива вывлда информачи 

            comend = new SqlCommand("SELECT DISTINCT  [ID], [Street], [Number]" +
                " FROM [House]" +
                "WHERE [ResidentialComplexID] = @name", sqlConn);
            //команда запроса
            comend.Parameters.AddWithValue("@name", id);//водим параметор

            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead = comend.ExecuteReader();//запрос в базу

            comboBox3.Items.Add("");//вставлем пустую строку как выбор нечиго

            while (sqlRead.Read())
            {
                comboBox3.Items.Add("ул." + Convert.ToString(sqlRead["Street"])
                    + " д." + Convert.ToString(sqlRead["Number"]));//запись в кобобобокс

                Glab.idHouseList.Add(Convert.ToInt32(sqlRead["ID"]));//запись id

                Glab.strHouse.Add("ул." + Convert.ToString(sqlRead["Street"])
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
            comboBox6.Items.Clear();//очишаем таблицу

            int id = 0;//переменая для счета

            foreach (int i in Glab.idHouseList)
            {
                if ("" == comboBox3.Text)//провареем не выбрано ли пусто
                    break;
                else if (Glab.strHouse[id] == comboBox3.Text)//находим выбраный дома
                    break;

                id++;//вычеслем id выброного дома
            }

            Glab.idHouse = Convert.ToInt32(Glab.idHouseList[id]);//записаваем id выброного дома из спарвочника для последуещего вывода 


            comend = new SqlCommand("SELECT DISTINCT [Floor]" +
                " FROM  [Apartment]" +
                "WHERE [HouseID] = @House"
                , sqlConn);//команда запроса
            comend.Parameters.AddWithValue("@House", Glab.idHouse);//водим параметор


            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead = comend.ExecuteReader();//запрос в базу

            comboBox4.Items.Add("");//вставлем пустую строку как выбор нечиго

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
            comend.Parameters.AddWithValue("@House", Glab.idHouse);//водим параметор

            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 

            sqlRead = comend.ExecuteReader();//запрос в базу

            comboBox6.Items.Add("");
            //вставлем пустую строку как выбор нечиго
            while (sqlRead.Read())
                comboBox6.Items.Add(Convert.ToString(sqlRead["Section"]));
            // запись в кобобобокс

            comboBox5.Visible = true;
            comboBox6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            //показываем поле

            sqlRead.Close();//закрываем запрс
        }




        ////////////////////////////////////////////////////////////////////////////////////////



        private void ApartmentObnov()
        {
            SqlDataReader sqlRead = null;

            listView2.Items.Clear();//очишаем таблицу

            string commendProto = "SELECT  DISTINCT " +
                                    "[ID]," +
                                    "[HouseID] ," +
                                 " [Apartment].[Number] AS [kvart], " +
                                  "[Section] ," +
                                  "[Floor]  ," +
                                 "[Area]," +
                                 " [CountOfRooms]," +
                                 " [IsSold]," +

                                 " [Apartment].[BuildingCost] AS [ApartmentBuilding]," +
                                 " [ApartmentValueAdded]" +


                                 "FROM [Apartment]";
            //прототип запроса

            SqlCommand comend = new SqlCommand();
            //команда зпроса

            if (comboBox2.SelectedIndex > 0)
            {

                if (comboBox3.SelectedIndex > 0)//если выбрали дом 
                {
                    if (comboBox4.SelectedIndex > 0 &&
                        comboBox5.SelectedIndex > 0 &&
                        comboBox6.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                                    commendProto +
                                    "WHERE [Apartment].[HouseID] = @house" +
                                    " AND [Section] = @section" +
                                    " AND [Floor] = @floor" +
                                    " AND [IsSold] = @isSold",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("section", comboBox6.Text);
                        comend.Parameters.AddWithValue("floor", comboBox4.Text);

                        if (comboBox5.Text == "Да")
                            comend.Parameters.AddWithValue("isSold", true);
                        else
                            comend.Parameters.AddWithValue("isSold", false);
                        //водим параметор
                    }
                    //если выбрали все поля 

                    else if (comboBox5.SelectedIndex > 0 &&
                       comboBox6.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                                   commendProto +
                                    "WHERE [Apartment].[HouseID] = @house" +
                                    " AND [Section] = @section " +
                                    "AND [IsSold] = @isSold",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("section", comboBox6.Text);

                        if (comboBox5.Text == "Да")
                            comend.Parameters.AddWithValue("isSold", true);
                        else
                            comend.Parameters.AddWithValue("isSold", false);
                        //водим параметор
                    }
                    //если выбрали все поля кроме этожа

                    else if (comboBox4.SelectedIndex > 0 &&
                       comboBox5.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                                  commendProto +
                                    "WHERE [Apartment].[HouseID] = @house AND [Floor] = @floor AND [IsSold] = @isSold",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("floor", comboBox4.Text);

                        if (comboBox5.Text == "Да")
                            comend.Parameters.AddWithValue("isSold", true);
                        else
                            comend.Parameters.AddWithValue("isSold", false);
                        //водим параметор
                    }
                    //если выбрали все поля кроме подъезда 

                    else if (comboBox4.SelectedIndex > 0
                        && comboBox6.SelectedIndex > 0)//если выбрали все остольные поля статуса
                    {
                        comend = new SqlCommand(
                            commendProto +
                                     "WHERE [Apartment].[HouseID] = @house AND [Section] = @section AND [Floor] = @floor",
                                        sqlConn);
                        //команда зпроса

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("section", comboBox6.Text);
                        comend.Parameters.AddWithValue("floor", comboBox4.Text);
                        //водим параметор

                    }
                    //если выбрали все поля кроме статуса 


                    else if (comboBox4.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                                    commendProto +
                                    "WHERE [Apartment].[HouseID] = @house AND [Floor] = @floor",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("floor", comboBox4.Text);
                        //водим параметор
                    }
                    //если выбрали только этаж

                    else if (comboBox6.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                             commendProto +
                                    "WHERE [Apartment].[HouseID] = @house AND [Section] = @section",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        comend.Parameters.AddWithValue("section", comboBox6.Text);
                        //водим параметор
                    }
                    //если выбрали только подъезд

                    else if (comboBox5.SelectedIndex > 0)
                    {
                        comend = new SqlCommand(
                              commendProto +
                                   "WHERE [Apartment].[HouseID] = @house AND [IsSold] = @isSold",
                                     sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);

                        if (comboBox5.Text == "Да")
                            comend.Parameters.AddWithValue("isSold", true);
                        else if (comboBox5.Text == "Нет")
                            comend.Parameters.AddWithValue("isSold", false);
                        //водим параметор
                    }
                    //если выбрали только статус

                    else
                    {
                        comend = new SqlCommand(
                                   commendProto +
                                        "WHERE [Apartment].[HouseID] = @house ",
                                         sqlConn);

                        comend.Parameters.AddWithValue("house", Glab.idHouse);
                        //водим параметор
                        //если выбрали только этаж
                    }
                    //есл не эказали остальные праметры
                }

                else
                {
                    commendProto += "WHERE [HouseID] = @ID";

                    foreach (int i in Glab.idHouseList)
                    {
                        commendProto += " OR [HouseID] = '" + i + "'";
                    }

                    comend = new SqlCommand(commendProto, sqlConn);

                    comend.Parameters.AddWithValue("ID", 0);
                }
            }

            else
                comend = new SqlCommand(
                                commendProto,
                                 sqlConn);

            sqlRead = comend.ExecuteReader();//запрос в базу

            while (sqlRead.Read())
            {
                listView2.Items.Add
                      (new ListViewItem(new string[]
                          {
                              Convert.ToString(sqlRead["HouseID"]),//
                              Convert.ToString(sqlRead["kvart"]),
                              Convert.ToString(sqlRead["Section"]),
                              Convert.ToString(sqlRead["Floor"]),
                              Convert.ToString(sqlRead["Area"]),//
                              Convert.ToString(sqlRead["CountOfRooms"]),//
                              Convert.ToString(sqlRead["IsSold"]),//
                              Convert.ToString(sqlRead["ApartmentBuilding"]),//
                              Convert.ToString(sqlRead["ApartmentValueAdded"])//
                        })//водим результат в таблицу 
                      );

                Glab.Otsenka_IdHouse_List.Add(Convert.ToInt32(sqlRead["HouseID"]));
                Glab.Otsenka_IdApartment_List.Add(Convert.ToInt32(sqlRead["ID"]));

                Glab.Otsenka_Area_List.Add(Convert.ToInt32(sqlRead["Area"]));
                Glab.Otsenka_CountOfRooms_List.Add(Convert.ToInt32(sqlRead["CountOfRooms"]));

                Glab.Otsenka_ApartmentBuilding_List.Add(Convert.ToInt32(sqlRead["ApartmentBuilding"]));
                Glab.Otsenka_ApartmentValueAdded_List.Add(Convert.ToInt32(sqlRead["ApartmentValueAdded"]));

                Glab.Otsenka_IsSold_List.Add(Convert.ToString(sqlRead["IsSold"]));
                

                Glab.scet++;
            }
            if (sqlRead != null)
                sqlRead.Close();//проверка на откратасть 
        }
        //метод для вывода информаци о квартирах
        private void ApartmentButton_Click(object sender, EventArgs e)
        {
            ApartmentObnov();
        }
        //кнопка для обновление данных


        ////////////////////////////////////////////////////////////////////////////////////////

        private void Otsenka()
        {
            SqlDataReader sqlRead = null;

            string commendProto = "SELECT  DISTINCT " +
                                    "[ID]," +
                                    "[ResidentialComplexID] ," +
                                 " [BuildingCost]," +
                                 " [HouseValueAdded]" +

                                 "FROM [House]";
            //прототип запроса

            SqlCommand comend = new SqlCommand();
            //команда зпроса

            int id = 0,
                idHouse = 0,
                idResidentialComplex = 0,

                sumStoim = 0,
                sumStoimTrue = 0,
                sumStoimFalse = 0,
                sumZatrat = 0,
                pribyl = 0;

            {
               
                commendProto += "WHERE [ID] = @id";

                foreach (int i in Glab.Otsenka_IdHouse_List)
                {
                    commendProto += " OR [ID] = '" + i + "'";
                }

                comend = new SqlCommand(commendProto, sqlConn);

                comend.Parameters.AddWithValue("id", 0);


                if (sqlRead != null)
                    sqlRead.Close();

                sqlRead = comend.ExecuteReader();//запрос в базу

                while (sqlRead.Read())
                {
                    Glab.Otsenka_HouseBuilding_List.Add(Convert.ToInt32(sqlRead["BuildingCost"]));
                    Glab.Otsenka_HouseValueAdded_List.Add(Convert.ToInt32(sqlRead["HouseValueAdded"]));

                    Glab.Otsenka_IdResidentialComplex_List.Add(Convert.ToInt32(sqlRead["ResidentialComplexID"]));
                }

            }


            /////////////////////////////////////////////////////////////////////////////////

             if (sqlRead != null)
                sqlRead.Close();

            id = 0;
                {

                    commendProto = "SELECT  DISTINCT " +
                                            "[ID]," +
                                         " [BuildingCost]," +
                                         " [ComplexValueAdded] " +

                                         "FROM [ResidentialComplex]";
                    //прототип запроса

                    commendProto += "WHERE [ID] = @id";

                    foreach (int i in Glab.Otsenka_IdResidentialComplex_List)
                    {
                        commendProto += " OR [ID] = '" + i + "'";
                    }

                    comend = new SqlCommand(commendProto, sqlConn);

                    comend.Parameters.AddWithValue("id", 0);

                    sqlRead = comend.ExecuteReader();//запрос в базу

                    while (sqlRead.Read())
                    {
                        Glab.Otsenka_ResidentialComplexBuilding_List.Add(Convert.ToInt32(sqlRead["BuildingCost"]));
                        Glab.Otsenka_ComplexValueAdded_List.Add(Convert.ToInt32(sqlRead["ComplexValueAdded"]));
                    }

                }
            id = 0;

            int ResidentialComplex = Glab.Otsenka_IdResidentialComplex_List[0],
                House = Glab.Otsenka_IdHouse_List[0];

                for (int i = 0; i < Glab.Otsenka_IsSold_List.Count; i++)
                {
                    if (id != Glab.Otsenka_ApartmentBuilding_List.Count)
                    {
                        if (Glab.Otsenka_IdResidentialComplex_List[idHouse] != ResidentialComplex)
                        {
                            ResidentialComplex = Glab.Otsenka_IdResidentialComplex_List[idHouse];

                            sumZatrat += Glab.Otsenka_ResidentialComplexBuilding_List[idResidentialComplex];

                            idResidentialComplex++;
                        }
                        else if (Glab.Otsenka_IdHouse_List[id] != House)
                        {
                            sumZatrat += Glab.Otsenka_HouseBuilding_List[idHouse];

                            idHouse++;

                            House = Glab.Otsenka_IdHouse_List[id];
                        }
                          sumZatrat += Glab.Otsenka_ApartmentBuilding_List[id];

                            if (Glab.Otsenka_IsSold_List[id] == "True")
                            {
                                sumStoimTrue += Glab.Otsenka_Area_List[id] * Convert.ToInt32(textBox1.Text) +
                                               Glab.Otsenka_CountOfRooms_List[id] * Convert.ToInt32(textBox2.Text) +
                                               Glab.Otsenka_ApartmentValueAdded_List[id] +
                                               Glab.Otsenka_HouseValueAdded_List[idHouse] +
                                               Glab.Otsenka_ComplexValueAdded_List[idResidentialComplex];
                            }

                            else if (Glab.Otsenka_IsSold_List[id] == "False")
                            {
                                sumStoimFalse += Glab.Otsenka_Area_List[id] * Convert.ToInt32(textBox1.Text) +
                                               Glab.Otsenka_CountOfRooms_List[id] * Convert.ToInt32(textBox2.Text) +
                                               Glab.Otsenka_ApartmentValueAdded_List[id] +
                                               Glab.Otsenka_HouseValueAdded_List[idHouse] +
                                               Glab.Otsenka_ComplexValueAdded_List[idResidentialComplex];
                            }
                        
                        id++;
                    }
                }
                id++;
            

            sumStoim = sumStoimFalse + sumStoimTrue;



            pribyl = sumStoim - sumZatrat;

            {
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;


                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                //вывод

                textBox3.Text = Convert.ToString(sumStoim);
                textBox4.Text = Convert.ToString(sumStoimTrue);
                textBox5.Text = Convert.ToString(sumStoimFalse);
                textBox6.Text = Convert.ToString(sumZatrat);
                textBox7.Text = Convert.ToString(pribyl);
            }

            sqlRead.Close();
        }

        private void OtsenkaButton_Click(object sender, EventArgs e)
        {
            Otsenka();
        }

        private void ИзменитьКоэфицентToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                SqlCommand commend = new SqlCommand("SELECT  DISTINCT " +
                                         "[ID]" +

                                      "FROM [ResidentialComplex]" +
                                      "WHERE [ID] = @id", sqlConn);
                //прототип запроса




                commend.Parameters.AddWithValue("id", Glab.Otsenka_IdHouse_List[Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text)]);

                SqlDataReader sqlRead;

                sqlRead = commend.ExecuteReader();//запрос в базу

                int ZK = 0;
                while (sqlRead.Read())
                {
                    ZK = Convert.ToInt32(sqlRead["ID"]);
                }

                Update up = new Update(Glab.Otsenka_IdApartment_List[Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text) - 1],
                                        Glab.Otsenka_IdHouse_List[Convert.ToInt32(listView2.SelectedItems[0].SubItems[0].Text) - 1],
                                        ZK);
                up.Show();

                sqlRead.Close();
            }
        }
    }
}
