using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expences_Mangeign_System
{
    public partial class Form5 : Form
    {
        private readonly string firebaseUrl = "https://expense-tracking-system-6dd2e-default-rtdb.firebaseio.com/"; // Replace with your Firebase URL

        public Form5()
        {
            InitializeComponent();
        }

        private async void Form5_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Form5 is loading...");
            await LoadExpensesAsync(); // Load expenses asynchronously on form load
        }

        private async Task LoadExpensesAsync()
        {
            try
            {
                string jsonResponse = await GetExpensesFromFirebaseAsync();

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    MessageBox.Show("No data found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse the JSON response
                JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse);
                JsonElement root = jsonDoc.RootElement;

                // Clear existing items in the ListBox
                listBox1.Items.Clear();

                foreach (JsonProperty group in root.EnumerateObject())
                {
                    JsonElement expensesArray = group.Value;

                    if (expensesArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement expense in expensesArray.EnumerateArray())
                        {
                            try
                            {
                                // Extract individual properties
                                int amount = expense.GetProperty("Amount").GetInt32(); // Use GetDouble() for decimals
                                string category = expense.GetProperty("Category").GetString();

                                // Format and add the item to the ListBox
                                string listItem = $"Category: {category}, Amount: {amount}";
                                listBox1.Items.Add(listItem);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error parsing an expense item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                MessageBox.Show("Expenses loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> GetExpensesFromFirebaseAsync()
        {
            try
            {
                var firebase = new FirebaseClient(firebaseUrl);
                var expenses = await firebase
                    .Child("expenses") // Replace with your Firebase reference
                    .OnceAsync<object>();

                // Mocked response format for Firebase data (adjust to your actual structure)
               var expensesJson = "{ \"Group1\": [ { \"Amount\": 100, \"Category\": \"Food\" }, { \"Amount\": 200, \"Category\": \"Transport\" } ], \"Group2\": [ { \"Amount\": 50, \"Category\": \"Utilities\" }, { \"Amount\": 75, \"Category\": \"Entertainment\" } ] }";


                return expensesJson; // Return the JSON data fetched from Firebase
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data from Firebase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the logic for listBox1 item selection here.
        }

    }
}
