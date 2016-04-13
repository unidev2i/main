using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication2
{
    public partial class Suppresion_User : Form
    {
        public Suppresion_User()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = comboBox1.Text;

            string[] result = Regex.Split(user, " ");
            string log = result[0];
            string mdp = result[1];
            ("DELETE * from user WHERE Login='" + log + "' AND Password='" + mdp + "'").SimpleRequest();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Suppresion_User_Load(object sender, EventArgs e)
        {
            foreach (var a in Database.GetListRequest("user", new[] {"Login", "Password"}))
            {
                comboBox1.Items.Add(a);
            }
        }
    }
}
