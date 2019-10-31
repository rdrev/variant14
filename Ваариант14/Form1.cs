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
            public static string login;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void vxod_Click(object sender, EventArgs e)
        {
            sql_zapros();
        }

        private async void sql_zapros()
        {
            string ConnStr = @"Data Source=БАБУШКА-ПК\CENTAUR;Initial Catalog=Variant14;User ID=work;Password=1954";

            sqlConn = new SqlConnection(ConnStr); 

            await sqlConn.OpenAsync();

            SqlDataReader sqlRead = null;

            int proverka = 0;
            bool proverkaMen = false;

            SqlCommand comed = new SqlCommand("SELECT * FROM [Avtoris] WHERE [login] = @log AND [password] = @pass", sqlConn);
            comed.Parameters.AddWithValue("@log", LoginBox.Text);
            comed.Parameters.AddWithValue("@pass", PasswordBox.Text);

            try
            {
                sqlRead = await comed.ExecuteReaderAsync();

                while (await sqlRead.ReadAsync())
                {
                    if ((bool)sqlRead["dostup"] == true)
                        proverkaMen = true;
                    proverka++;

                    Glab.login = LoginBox.Text;
                }

                if (proverka != 0 && proverkaMen == true)
                {
                    // Create a new instance of the Form2 class
                    Menu_men settingsForm = new Menu_men();

                    settingsForm.Show();

                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                        sqlConn.Close();

                    this.Hide();

                }
                else if (proverka != 0 && proverkaMen == false)
                {
                    // Create a new instance of the Form2 class
                    Menu_ispol settingsForm = new Menu_ispol();

                    settingsForm.Show();

                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                        sqlConn.Close();

                    this.Hide();

                }
                else
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
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Clos_Avtoris();
        }

        public void Clos_Avtoris()
        {
            if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

    }
}
