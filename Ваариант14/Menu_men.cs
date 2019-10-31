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

        public Menu_men()
        {
            InitializeComponent();
        }

        private async void Menu_men_Load(object sender, EventArgs e)
        {
            string ConnStr = @"Data Source=БАБУШКА-ПК\CENTAUR;Initial Catalog=Variant14;User ID=work;Password=1954";


            sqlConn = new SqlConnection(ConnStr);

            await sqlConn.OpenAsync();
            comboBox1.SelectedItem = "по наз ЖК";
            this.comboBox1.SelectedIndexChanged +=
            new System.EventHandler(ComboBox1_SelectedIndexChanged);
            obnov();
        }
        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            obnov();
        }

        private async void obnov()
        {
            listBox1.Items.Clear();

            SqlDataReader sqlRead = null;

            SqlCommand comend = new SqlCommand();
            string comondProto = "SELECT DISTINCT [Name], [Street], [House].[Number], [Status] " +
                "FROM [House], [ResidentialComplex], [Apartment]";

       if (string.IsNullOrEmpty(streetBox.Text) && string.IsNullOrWhiteSpace(streetBox.Text) &&
               string.IsNullOrEmpty(ZKBox.Text) && string.IsNullOrWhiteSpace(ZKBox.Text))
            {
                if (comboBox1.Text == "по наз ЖК")
                    comend = new SqlCommand(comondProto + " ORDER BY [Name]", sqlConn);
                else if (comboBox1.Text == "по улице")
                    comend = new SqlCommand(comondProto + " ORDER BY [Street]", sqlConn);
                else if (comboBox1.Text == "по номеру дома")
                    comend = new SqlCommand(comondProto + " ORDER BY [Number]", sqlConn);
            
            }

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
            }

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

            }

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

            }
            try
            {
                sqlRead = await comend.ExecuteReaderAsync();
                while (await sqlRead.ReadAsync())
                {
                    int Length1 = Convert.ToString(sqlRead["Name"]).Length,
                        Length2 = Convert.ToString(sqlRead["Street"]).Length,
                        Length3 = Convert.ToString(sqlRead["Number"]).Length,
                        Length4 = Convert.ToString(sqlRead["Status"]).Length;

                    int numberOfSpaces1 = 100 - Length1 - Length2,
                        numberOfSpaces2 = 20 - Length2 - Length3 + 70,
                        numberOfSpaces3 = 17 - Length3 - Length4 + 40;

                    string spaces1 = string.Empty,
                           spaces2 = string.Empty,
                           spaces3 = string.Empty;


                    for (int i = 0; i < numberOfSpaces1; i++)
                        spaces1 = spaces1 + " ";

                    for (int i = 0; i < numberOfSpaces2; i++)
                        spaces2 = spaces2 + " ";

                    for (int i = 0; i < numberOfSpaces3; i++)
                        spaces3 = spaces3 + " ";


                    listBox1.Items.Add(
                         Convert.ToString(sqlRead["Name"]) + spaces1 +
                         Convert.ToString(sqlRead["Street"]) + spaces2 +
                         Convert.ToString(sqlRead["Number"]) + spaces3 +
                         Convert.ToString(sqlRead["Status"])
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlRead != null)
                    sqlRead.Close();

            }
        }

        private void ОбновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obnov();
        }
    }
}
