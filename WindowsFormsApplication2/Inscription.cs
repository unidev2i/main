using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApplication2
{
    public partial class Inscription : Form
    {
        private AssistantConnexion form1;
        public Inscription()
        {
            InitializeComponent();
        }

        public Inscription(AssistantConnexion form1)
        {
            InitializeComponent();
            this.form1 = form1;
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string mdp = textBox2.Text;
            int statut = 0;
            if (checkBox1.Checked)
                statut = 1;

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "mydb";

            MySqlConnection conn = new MySqlConnection(builder.ToString());
            try
            {
                conn.Open();
                MessageBox.Show("Connection Réussit à la BDD");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            String newuser_sql = "INSERT INTO user (Login,Password,Statut) VALUES (@login, @password,@statut)";
            MySqlCommand newuser = new MySqlCommand(newuser_sql, conn);

            /*String delete_sql = "DELETE FROM user WHERE Statut=0";
            MySqlCommand delete = new MySqlCommand(delete_sql, conn);
            delete.CommandText = delete_sql;
            delete.ExecuteNonQuery();*/

            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT Login FROM user ";
            MySqlDataReader reader = command.ExecuteReader();
            int flag = 1;
            while (reader.Read())
            {
                if (login == reader["Login"].ToString())
                    flag = 0;
            }
            reader.Close();
            if (flag == 1 && login != "" && mdp != "")
            {
                newuser.CommandText = newuser_sql;
                newuser.Parameters.AddWithValue("@login", login);
                newuser.Parameters.AddWithValue("@password", mdp);
                newuser.Parameters.AddWithValue("@statut", statut);
                newuser.ExecuteNonQuery();
                MessageBox.Show("Inscription Réussie");
            }
            else
            {
                if (login == "" || mdp == "")
                {
                    MessageBox.Show("Vous ne pouvez pas vous inscrire avec un champ vide");
                }
                else
                {
                    MessageBox.Show("Un Utilisateur ayant le même Login existe déjà");
                }
            }
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Inscription_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
