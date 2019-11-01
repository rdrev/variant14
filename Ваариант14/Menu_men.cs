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
            string ConnStr = @"Data Source=62.63.74.62,1433;Initial Catalog=Variant14;User ID=work;Password=1954";

            sqlConn = new SqlConnection(ConnStr);

            await sqlConn.OpenAsync();

            comboBox1.SelectedItem = "по наз ЖК";
            this.comboBox1.SelectedIndexChanged +=
            new System.EventHandler(ComboBox1_SelectedIndexChanged);

            listView1.GridLines = true;

            listView1.FullRowSelect = true;

            listView1.View = View.Details;

            listView1.Columns.Add("жилой комплекс");
            listView1.Columns.Add("улицы");
            listView1.Columns.Add("номера дома");
            listView1.Columns.Add("статус строительства ЖК");
            listView1.Columns.Add("количества проданных квартир");
            listView1.Columns.Add("количества продающихся квартир");

            obnov();
        }
        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            obnov();
        }

        private async void obnov()
        {
            listView1.Items.Clear();


            SqlDataReader sqlRead = null;

            SqlCommand comend = new SqlCommand();
            List < SqlCommand> comendProdono = new List<SqlCommand>(),
                comendNoProdono = new List<SqlCommand>();
            List<string> House = new List<string>(),
                Name = new List<string>(),
                Street = new List<string>(),
                Number = new List<string>(),
                Status = new List<string>();

            List<object> Prodono = new List<object>(),
                 NoProdono = new List<object>();

            string comondProto = "SELECT DISTINCT [House].[ID], [Name], [Street], [House].[Number], [Status]" +                           
                "FROM [House], [ResidentialComplex]";

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

            if (sqlRead != null)
                sqlRead.Close();
            try
            {
                sqlRead = await comend.ExecuteReaderAsync();


                while (await sqlRead.ReadAsync())
                {
                    //comendProdono = new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = @id AND [IsSold] = 1", sqlConn);
                    //comendProdono.Parameters.AddWithValue("id", Convert.ToString(sqlRead["ID"]));

                    //comendNoProdono = new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = @id AND [IsSold] = 0", sqlConn);
                    //comendNoProdono.Parameters.AddWithValue("id", Convert.ToString(sqlRead["ID"]));

                    //listView1.Items.Add
                    //(   new ListViewItem(new string[]
                    //    {   Convert.ToString(sqlRead["Name"]),
                    //        Convert.ToString(sqlRead["Street"]),
                    //        Convert.ToString(sqlRead["Number"]),
                    //        Convert.ToString(sqlRead["Status"]),
                    //        Convert.ToString(await comendProdono.ExecuteScalarAsync()),
                    //         Convert.ToString(await comendProdono.ExecuteScalarAsync())
                    //  })
                    //);
                    House.Add(Convert.ToString(sqlRead["Name"]));
                    Street.Add(Convert.ToString(sqlRead["Street"]));
                    Number.Add(Convert.ToString(sqlRead["Street"]));
                    Status.Add(Convert.ToString(sqlRead["Street"]));

                    comendProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 1", sqlConn));
                    comendNoProdono.Add(new SqlCommand("SELECT COUNT(DISTINCT [IsSold]) FROM [Apartment] WHERE [HouseID] = " + Convert.ToString(sqlRead["ID"]) + " AND [IsSold] = 0", sqlConn));
                }

                if (sqlRead != null)
                    sqlRead.Close();
                foreach (SqlCommand comm in comendProdono)
                {
                    Prodono.Add(await comm.ExecuteScalarAsync());
                }
                foreach (SqlCommand comm in comendNoProdono)
                {
                    NoProdono.Add(await comm.ExecuteScalarAsync());
                }
               for(int i = 0; i < House.Count; i++)
                 listView1.Items.Add
                    (new ListViewItem(new string[]
                        {
                            House[i],
                            Street[i],
                            Number[i],
                            Status[i],
                            Convert.ToString(Prodono[i]),
                            Convert.ToString(NoProdono[i])
                      })
                    );
                
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
