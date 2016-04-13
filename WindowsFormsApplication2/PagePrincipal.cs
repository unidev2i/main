﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    public partial class PagePrincipal : Form
    {
        AjoutEleve graphic = null;
        ChangerLogin graphic4 = null;
        ChangerMdp graphic5 = null;
        private BindingSource bindingSource1 = new BindingSource();
        private AssistantConnexion form1;
        Inscription graphic2 = null;
        Suppresion_User graphic3=null;
        DataGridDebug dataForm;
        string login;

        public PagePrincipal()
        {
            InitializeComponent();
            dataGridView1.AutoResizeColumns();
        }

        public PagePrincipal(AssistantConnexion form1,string login, bool statut)
        {
            InitializeComponent(); 
            this.form1 = form1;
            if (statut == false)
            {
                aToolStripMenuItem.Visible = false;
            }

            HelloBox(login);
        }

        private void GetData(string selectCommand)
        {

            String connectionString = "Server="+Database.Server+";Uid="+Database.Username+";Database="+Database.DatabaseName+";Password="+Database.Password+";";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(selectCommand, connectionString);
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
        private void Form3_Load_1(object sender, EventArgs e)
        {

            dataGridView1.DataSource = bindingSource1;
            dataGridView1.RowHeadersVisible = false;

            foreach (var a in Database.GetListRequest("eleve", new[] {"Prenom", "Nom"}))
                comboBox1.Items.Add(a);

            comboBox2.Items.Add("seconde");
            comboBox2.Items.Add("premiere");
            comboBox2.Items.Add("terminale");

            foreach (var a in Database.GetListRequest("classe", new[] {"Promotion"}))
                comboBox3.Items.Add(a);
        }

        private void Form3_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            form1.Close();
        }

        private void unEleveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deconnexionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sgInfo = new ProcessStartInfo("WindowsFormsApplication2.exe");
            Process.Start(sgInfo);
            this.Close();
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            foreach (var a in Database.GetListRequest("classe", new[] { "Niveau" }))
                comboBox2.Items.Add(a);
        }

        private void comboBox3_TextUpdate(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            foreach (var a in Database.GetListRequest("classe", new[] { "numClasse" }))
                comboBox3.Items.Add(a);
            comboBox3.Select(50, 50);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CHANGEMENT D'ELEVE, FAIRE LES MODIFICATIONS DES GRAPHIQUES ...
            string str = comboBox1.Text;
            string[] result = Regex.Split(str, " ");
            string prenom = result[0];
            string nom = result[1];    
            List<string> classe = new List<string>();

            foreach(var a in Database.GetListRequest("eleve", new []{"idClasse"}, "Nom='"+nom+"' and Prenom='"+prenom+"'"))
                comboBox3.Text = a;

            foreach (var classe2 in classe)
            {
                foreach (var a in Database.GetListRequest("classe", new[] { "Niveau" }, "numClasse='" + classe2 + "'"))
                {
                    comboBox2.Text = a;
                } 
            }
            comboBox1.Select(50, 50);

            var str2 = Regex.Split(comboBox1.Text, " ");
            var idEleve = "";

            foreach (var a in Database.GetListRequest("eleve", new[] { "idEleve" }, "Nom='" + str2[1] + "' and Prenom='" + str2[0] + "'"))
                idEleve = a ?? "1";


            GetData("SELECT Prenom, Nom, idTp AS TP, date AS Date, idCompetence AS Competence, Note, maxNote AS 'Note Maximum' FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE idEleve='" + idEleve + "'");
            dataGridView1.AutoResizeColumns();


            // Draw graphics


            var w = Database.GetWtfRequest(idEleve);
            var z = Database.GetWebRequest(idEleve);

            drawGraph(w);
            drawWeb(z);
            chart1.Visible = true;
            chart2.Visible = true;
            chart3.Visible = true;
            //chart1.ChartAreas[0].AxisX.Maximum = 100;

        }

        private void drawWeb(List<Tuple<string, float>> aTuples)
        {
            chart3.Series[0].Points.Clear();
            foreach (var a in aTuples)
            {
                var p = chart3.Series[0].Points.Add(a.Item2);
                p.Name = a.Item1;
                p.AxisLabel = a.Item1;
                //p.Label = a.Item1;
            }
        }

        private void drawGraph(List<Tuple<string, float, int>> tuples)
        {
            var array = tuples.Select(a => a.Item1).ToList();
            var parray = tuples.Select(a => a.Item2).ToList();
            var xarray = tuples.Select(a => a.Item3).ToList();

            //chart1.Palette = ChartColorPalette.Excel;
            chart1.Series.Clear();

            for (var a = 0; a != array.Count; a++)
            {
                chart1.Series.Add(new Series(array[a]));
            }

            var i = 0;
            foreach (var tSeries in from tSeries in chart1.Series let a = tSeries select tSeries)
            {
                tSeries.Points.AddY(parray[i]);
                i++;
            }

            chart2.Series[0].Points.Clear();
                                                           
            foreach (var a in tuples)
            {
                chart2.Series[0].Points.AddXY(a.Item1 + Environment.NewLine + a.Item3.ToString(), a.Item3);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string niveau = comboBox2.Text;
            List<String> list_classe = new List<string>();

            comboBox3.Items.Clear();
            foreach (var a in Database.GetListRequest("classe", new[] {"numClasse"}, "Niveau='" + niveau + "'"))
            {
                comboBox3.Items.Add(a);
                list_classe.Add(a);
            }     

            comboBox1.Items.Clear();  
            foreach (var a in list_classe.SelectMany(classe => Database.GetListRequest("eleve", new[] {"Prenom", "Nom"}, "idClasse='" + classe + "'")))
            {
                comboBox1.Items.Add(a);
            }
           
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //REFAIRE PAREIL QUE POUR LE NIVEAU
            string classe = comboBox3.Text;

            comboBox1.Items.Clear();
            foreach (var a in Database.GetListRequest("eleve", new[] { "Prenom", "Nom" }))
                comboBox1.Items.Add(a);
        }

        private void ajouterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graphic2 = new Inscription();
            graphic2.Show();
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic3 = new Suppresion_User();
            graphic3.Show();
        }

        private void dataGridDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataForm = new DataGridDebug();
            dataForm.Show();
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            new Form1(chart2).Show();
        }

        private void ajouterToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //BOUTON POUR AJOUTER UN ELEVE
            graphic = new AjoutEleve();
            graphic.Show();
        }

        private void ajouterUnPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changerDeMotDePasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic5 = new ChangerMdp(login);
            graphic5.Show();
        }

        private void changerDeLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic4 = new ChangerLogin(login, this);
            graphic4.Show();
        }

        public void UpdateLogin(string login)
        {
            this.login = login;
            label4.Text = "Professeur connecté: " + login;
        }

        public void Majlog(string newlog)
        {
            this.login = newlog;
            HelloBox(login);
        }

        public void HelloBox(string nom)
        {
            label4.Text = "Professeur connecté: " + nom;
            this.login = nom;
        }

        private void chart3_Click(object sender, EventArgs e)
        {
            new Form1(chart3).Show();
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test de push
            Database.BackupDatabase();
        }
    }
}