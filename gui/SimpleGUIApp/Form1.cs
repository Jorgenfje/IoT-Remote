using System;
using System.Windows.Forms;

namespace SimpleGUIApp
{
    public partial class Form1 : Form
    {
        private UIComponents uiComponents;
        private ApiHandler apiHandler;

        public Form1()
        {
            // Initialize the form properties
            this.Text = "ESP Simulator";
            this.Size = new System.Drawing.Size(600, 600);
            this.MinimumSize = new System.Drawing.Size(500, 500);

            // Instantiate helper classes
            uiComponents = new UIComponents(this);
            apiHandler = new ApiHandler(this);

            // Setup UI
            uiComponents.SetupUI();
        }
    }
}
