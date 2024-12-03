using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expences_Mangeign_System
{
    public partial class Form3 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form3()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) // Save button
        {
            try
            {
                // Get the monthly income from the TextBox
                string monthlyIncome = textBox1.Text;

                // Validate the input
                if (string.IsNullOrEmpty(monthlyIncome) || !decimal.TryParse(monthlyIncome, out _))
                {
                    MessageBox.Show("Please enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create the JSON data
                var data = new
                {
                    MonthlyIncome = monthlyIncome
                };
                string jsonData = System.Text.Json.JsonSerializer.Serialize(data);

                // Firebase URL (Replace with your actual Firebase database URL)
                string firebaseUrl = "https://expense-tracking-system-6dd2e-default-rtdb.firebaseio.com/users.json";

                // Send POST request to Firebase
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Monthly income saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Navigate to Form4
                    Form4 form4 = new Form4();
                    form4.Show();
                    this.Hide();
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to save data. Error: {errorDetails}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
