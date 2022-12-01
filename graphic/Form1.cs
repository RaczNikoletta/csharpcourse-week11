using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Data;

namespace graphic
{
    
    public partial class Form1 : Form
    {
        SQLiteCommand cmd;


    public Form1()
        {
            string cs = @"URI=file:C:\Users\raczn\source\repos\graphic\graphic\bin\Debug\net6.0-windows\robot.db";
            var con = new SQLiteConnection(cs);
            cmd = new SQLiteCommand(con);
            con.Open();
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
   
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.randomuser.me");
                    var response = await client.GetAsync("https://api.randomuser.me");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<Rootobject>(stringResult);
                    label1.Text = String.Join(" ", "Name:", rawData.results[0].name.title, rawData.results[0].name.first, rawData.results[0].name.last);
                    label2.Text = String.Join(" ", "E-mail:", rawData.results[0].email);
                    if (rawData.results[0].gender == "female")
                    {
                        femaleRadio.Checked = true;
                        maleRadio.Checked = false;
                    }
                    else {
                        maleRadio.Checked = true;
                        femaleRadio.Checked = false;


                    }
                  
                    pictureBox1.Load(rawData.results[0].picture.large.ToString());
                    pictureBox2.Load("https://robohash.org/" + rawData.results[0].name.first);
   
                    cmd.CommandText = "INSERT INTO user(name, gender, email) VALUES(@name,@gender,@email)";
                    cmd.Parameters.Add(new SQLiteParameter("@name", rawData.results[0].name.first));
                    cmd.Parameters.Add(new SQLiteParameter("@gender", rawData.results[0].gender));
                    cmd.Parameters.Add(new SQLiteParameter("@email", rawData.results[0].email));
                    cmd.ExecuteNonQuery();



                }

                catch (Exception ex)
                {
                    label1.Text = ex.Message;
                    label2.Text = "";
                }
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void userNames_Click(object sender, EventArgs e)
        {

        }
    }
}