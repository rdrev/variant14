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
    public partial class Form1 : Form
    {
        SqlConnection sqlConn;

        public Form1()
        {
            InitializeComponent();
        }

        public static class Glab
        {
            public static string login;//хранение логина пользователя 
        }//метод глобальных переменых

        private void vxod_Click(object sender, EventArgs e)
        {
            sql_zapros();
        }

        private async void sql_zapros()
        {
            string ConnStr = @"Data Source=62.63.74.62,1433;Initial Catalog=Variant14;User ID=work;Password=1954";//строка подключения 

            sqlConn = new SqlConnection(ConnStr); //Создание подключения 

            await sqlConn.OpenAsync();//открытие подключение

            SqlDataReader sqlRead = null;//переменая для вывода запроса

            int proverka = 0;//счетчек на количество ответов 
            bool proverkaMen = false;//переменая для записи статуса 

            SqlCommand comed = new SqlCommand("SELECT * FROM [Avtoris] WHERE [login] = @log AND [password] = @pass", sqlConn);//запрос на ноличиее пользователя 
            comed.Parameters.AddWithValue("@log", LoginBox.Text);//иннициация переменых 
            comed.Parameters.AddWithValue("@pass", PasswordBox.Text);

            try
            {
                sqlRead = await comed.ExecuteReaderAsync();//запрос в базу

                while (await sqlRead.ReadAsync())//проверка на ответ
                {
                    if ((bool)sqlRead["dostup"] == true)//проверка на статус пользователя
                        proverkaMen = true;
                    proverka++;

                    Glab.login = LoginBox.Text;//сохраняем логин 
                }

                if (proverka != 0 && proverkaMen == true)//открытие окна менеджера 
                {
                    Menu_men settingsForm = new Menu_men();

                    settingsForm.Show();

                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                        sqlConn.Close();

                    this.Hide();

                }
                else if (proverka != 0 && proverkaMen == false)//открытие окна пользователя
                {
                    Menu_ispol settingsForm = new Menu_ispol(sqlConn);

                    settingsForm.Show();

                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                        sqlConn.Close();

                    this.Hide();

                }
                else//вывод собшение об отчуствие пользователя
                {
                    MessageBox.Show(
                          "Неверное имя пользователя или ты пытаешся его взломать",
                          "Упс",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Question);

                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                        sqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();
            }
            finally
            {
                if (sqlRead != null)
                    sqlRead.Close();
            }
        }//запрос на вход

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Clos_Avtoris();
        }

        public void Clos_Avtoris()//закрытие подключение
        {
            if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

    }
}
