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
    public partial class Menu_ispol : Form
    {
        SqlConnection sqlConn;
        public Menu_ispol(SqlConnection sqlConnd)
        {
            InitializeComponent();

            sqlConn = sqlConnd;
        }

        private void Menu_ispol_Load(object sender, EventArgs e)
        {

        }
    }
}
