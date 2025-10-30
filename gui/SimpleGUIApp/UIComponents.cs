using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Text.Json.Serialization;

namespace SimpleGUIApp
{
    public class UIComponents
    {
        private Form form;

        public TextBox screenTextBox;
        public ComboBox groupComboBox;
        private Label groupLabel; // Ny label for overskriften
        public TrackBar myTrackBar;
        private Label sliderLabel;
        private GroupBox buttonGroup;
        private GroupBox sliderGroup;
        private Button getRequestButton;

        public UIComponents(Form form)
        {
            this.form = form;
        }

        public void SetupUI()
        {
            SetupTextBox();
            SetupComboBox();
            SetupButtonGroup();
            SetupSliderGroup();
            SetupApiButtons();

            form.Resize += (sender, e) => AdjustForResize();
        }

        private void SetupTextBox()
        {
            screenTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = "Screen Output",
                Font = new System.Drawing.Font("Consolas", 12),
                Location = new System.Drawing.Point(50, 300),
                Size = new System.Drawing.Size(form.ClientSize.Width - 100, 100),
                ScrollBars = ScrollBars.Vertical
            };
            form.Controls.Add(screenTextBox);
        }

        public void UpdateScreen(string message)
        {
            screenTextBox.AppendText(Environment.NewLine + message);
        }

        private void SetupComboBox()
        {
            // Label for "Grupper"
            groupLabel = new Label
            {
                Text = "Grupper",
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(form.ClientSize.Width - 220, 10),
                AutoSize = true
            };
            form.Controls.Add(groupLabel);

            // ComboBox for grupper
            groupComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new System.Drawing.Size(200, 30),
                Location = new System.Drawing.Point(form.ClientSize.Width - 220, 40) // Plassering under label
            };

            LoadGroupsFromJson();
            form.Controls.Add(groupComboBox);
        }

        public void LoadGroupsFromJson(string filePath = "groups.json")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Filen ble ikke funnet: {filePath}");
                    return;
                }

                string jsonData = File.ReadAllText(filePath);
                List<Group> groups = JsonSerializer.Deserialize<List<Group>>(jsonData);

                if (groups != null)
                {
                    groupComboBox.Items.Clear();
                    foreach (var group in groups)
                    {
                        if (group.Name != null)
                        {
                            groupComboBox.Items.Add(group);
                        }
                    }

                    groupComboBox.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Feil ved lasting av grupper fra JSON: {ex.Message}");
            }
        }

        private void SetupButtonGroup()
        {
            buttonGroup = new GroupBox
            {
                Text = "Actions",
                Size = new System.Drawing.Size(350, 100),
                Location = new System.Drawing.Point(50, 50)
            };

            Button myButton = new Button
            {
                Text = "På",
                Size = new System.Drawing.Size(100, 50),
                Location = new System.Drawing.Point(10, 30),
                BackColor = System.Drawing.Color.LightGreen
            };
            myButton.Click += (sender, e) =>
            {
                SendBoolValue(true);
                UpdateScreen("Light on");
            };
            buttonGroup.Controls.Add(myButton);

            Button secondButton = new Button
            {
                Text = "Av",
                Size = new System.Drawing.Size(100, 50),
                Location = new System.Drawing.Point(150, 30),
                BackColor = System.Drawing.Color.IndianRed
            };
            secondButton.Click += (sender, e) =>
            {
                SendBoolValue(false);
                UpdateScreen("Light off");
            };
            buttonGroup.Controls.Add(secondButton);

            form.Controls.Add(buttonGroup);
        }

        private async void SendBoolValue(bool value)
        {
            if (groupComboBox.SelectedItem is Group selectedGroup)
            {
                var content = new { Id = selectedGroup.Id, State = value };
                string jsonContent = JsonSerializer.Serialize(content);

                string response = await new ApiHandler(form).SendPostRequest("http://localhost:5048/api/v1/Group/updateDevices", jsonContent);
                UpdateScreen(response);
            }
            else
            {
                MessageBox.Show("Please select a valid group.");
            }
        }

        private void SetupSliderGroup()
        {
            sliderGroup = new GroupBox
            {
                Text = "Slider Control",
                Size = new System.Drawing.Size(350, 120),
                Location = new System.Drawing.Point(50, 160)
            };

            sliderLabel = new Label
            {
                Text = "Slider value: 1",
                Font = new System.Drawing.Font("Arial", 10),
                Location = new System.Drawing.Point(10, 20),
                AutoSize = true
            };
            sliderGroup.Controls.Add(sliderLabel);

            myTrackBar = new TrackBar
            {
                Minimum = 1,
                Maximum = 12,
                Value = 1,
                TickFrequency = 1,
                Size = new System.Drawing.Size(300, 45),
                Location = new System.Drawing.Point(10, 50)
            };
            myTrackBar.Scroll += (sender, e) =>
            {
                sliderLabel.Text = $"Slider value: {myTrackBar.Value}";
            };
            sliderGroup.Controls.Add(myTrackBar);

            form.Controls.Add(sliderGroup);
        }

        private void SetupApiButtons()
        {
            getRequestButton = new Button
            {
                Text = "Oppdater Gruppe(GET)",
                Size = new System.Drawing.Size(100, 50),
                Location = new System.Drawing.Point(50, 450),
                BackColor = System.Drawing.Color.LightBlue
            };
            getRequestButton.Click += async (sender, e) =>
            {
                try
                {
                    string response = await new ApiHandler(form).SendGetRequest("http://localhost:5048/api/v1/Group/getAll");
                    UpdateScreen("GET Response: " + response);
                    File.WriteAllText("./groups.json", response);
                    LoadGroupsFromJson("./groups.json");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"GET Request Error: {ex.Message}");
                }
            };

            form.Controls.Add(getRequestButton);
        }

        private void AdjustForResize()
        {
            int formWidth = form.ClientSize.Width;
            int formHeight = form.ClientSize.Height;

            groupLabel.Location = new System.Drawing.Point(formWidth - groupComboBox.Width - 20, 10);
            groupComboBox.Location = new System.Drawing.Point(formWidth - groupComboBox.Width - 20, 40);

            screenTextBox.Width = formWidth - 100;
        }
    }

    public class Group
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("devices")]
        public List<Device> Devices { get; set; }
    }

    public class Device
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("status")]
        public bool State { get; set; }

        [JsonPropertyName("paired")]
        public bool Paired { get; set; }
    }
}
