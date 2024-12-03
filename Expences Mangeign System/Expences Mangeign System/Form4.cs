using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;
using System.Drawing;

namespace Expences_Mangeign_System
{
    public partial class Form4 : Form
    {
        // Firebase URL (replace with your Firebase database URL)
        private const string FirebaseUrl = "https://expense-tracking-system-6dd2e-default-rtdb.firebaseio.com/expenses.json";

        // Dictionary to store category (ComboBox) and amount (TextBox) pairs
        private Dictionary<ComboBox, TextBox> expenseControls = new Dictionary<ComboBox, TextBox>();

        private int nextControlY; // Tracks the Y-coordinate for adding controls dynamically

        public Form4()
        {
            InitializeComponent();
            nextControlY = comboBox1.Location.Y + 40; // Initialize position for dynamic controls
        }


        // Event: Add button clicked
        private void button2_Click(object sender, EventArgs e) // Add button
        {
            // Calculate the position for the new ComboBox and TextBox
            int yOffset = 40 * (expenseControls.Count + 1); // +1 accounts for the new row
            int baseY = comboBox1.Location.Y;

            // Create a new ComboBox
            ComboBox newCategory = new ComboBox
            {
                Width = comboBox1.Width,
                Location = new Point(comboBox1.Location.X, baseY + yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            newCategory.Items.AddRange(comboBox1.Items.Cast<object>().ToArray());
            this.Controls.Add(newCategory);

            // Create a new TextBox for the amount
            TextBox newAmount = new TextBox
            {
                Width = textBox1.Width,
                Location = new Point(textBox1.Location.X, baseY + yOffset)
            };
            this.Controls.Add(newAmount);

            // Add the new controls to the dictionary
            expenseControls.Add(newCategory, newAmount);

            // Refresh the form to ensure the new controls are displayed immediately
            this.Refresh();
        }





        // Event: Save button clicked
        private async void button3_Click(object sender, EventArgs e) // Save button
        {
            try
            {
                var expenses = new List<object>();

                foreach (var pair in expenseControls)
                {
                    ComboBox categoryBox = pair.Key;
                    TextBox amountBox = pair.Value;

                    // Check if category is selected
                    if (categoryBox.SelectedIndex < 0)
                    {
                        MessageBox.Show($"Please select a category for one of the rows.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Validate the TextBox amount
                    if (!decimal.TryParse(amountBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show($"Please enter a valid numeric amount for category '{categoryBox.SelectedItem}'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string category = categoryBox.SelectedItem.ToString();
                    expenses.Add(new { Category = category, Amount = amount });
                }

                // Firebase Save Logic
                using (HttpClient client = new HttpClient())
                {
                    var jsonData = JsonSerializer.Serialize(expenses);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(FirebaseUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Expenses saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save expenses. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        // Event: View button clicked
        private void button1_Click(object sender, EventArgs e) // View button
        {
            // Navigate to Form5 to show expense reports
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
