using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expences_Mangeign_System
{
    public partial class Form2 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Navigate to Form1
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private async void button2_Click(object sender, EventArgs e) // Register button
        {
            // Collect data from text boxes
            string firstName = textBox1.Text.Trim(); // Assuming textBox1 is for "First Name"
            string lastName = textBox2.Text.Trim();  // Assuming textBox2 is for "Last Name"
            string email = textBox3.Text.Trim();     // Assuming textBox3 is for "Email"
            string password = textBox4.Text.Trim();  // Assuming textBox4 is for "Password"
            string confirmPassword = textBox5.Text.Trim(); // Assuming textBox5 is for "Confirm Password"

            // Validate input
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prepare data for Firebase
            var user = new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password // Store securely in a real application
            };

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(user);

            try
            {
                // Send POST request to Firebase
                string firebaseUrl = "https://expense-tracking-system-6dd2e-default-rtdb.firebaseio.com/users.json"; // Replace with your Firebase URL
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to register. Error: {errorDetails}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e) // Reset button
        {
            // Clear all text boxes
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
    }
}
