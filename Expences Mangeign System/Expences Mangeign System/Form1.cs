using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expences_Mangeign_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Navigate to Form3
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Convert data to JSON format
            string jsonData = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";

            try
            {
                string response = await FirebaseHelper.PostDataAsync("users", jsonData);
                MessageBox.Show("Login successful! Firebase response: " + response);

                // Navigate to Form3
                Form3 form3 = new Form3();
                form3.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
