using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace LibreHardwareMonitor.UI
{
    public partial class ArduinoDislplays : Form
    {
        public ArduinoDislplays()
        {
            this.Displays = new List<GroupBox>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GroupBox display = this._createDisplay();
            Controls.Add(display);
            this.Displays.Add(display);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private GroupBox _createDisplay()
        {
            GroupBox groupBox = new GroupBox();

            int countDisplays = 1;
            try
            {
                countDisplays = this.Displays.Count() + 1;
            }
            catch (ArgumentNullException exception) { }

            Label label = new Label();
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(18, 22);
            label.Name = "label1";
            label.Size = new System.Drawing.Size(64, 13);
            label.TabIndex = 2;
            label.Text = "Display type";

            ComboBox displayTypeComboBox = new ComboBox();
            displayTypeComboBox.FormattingEnabled = true;
            displayTypeComboBox.Items.AddRange(new object[] {"List", "Pilot"});
            displayTypeComboBox.Location = new System.Drawing.Point(96, 19);
            displayTypeComboBox.Name = "DisplayType";
            displayTypeComboBox.Size = new System.Drawing.Size(121, 21);
            displayTypeComboBox.TabIndex = 2;
            displayTypeComboBox.SelectedIndexChanged += new System.EventHandler(displayTypeSelectedIndexChange);

            groupBox.SuspendLayout();
            groupBox.Controls.Add(label);
            groupBox.Controls.Add(displayTypeComboBox);
            groupBox.Location = new System.Drawing.Point(12, 70 + (countDisplays * 100));
            groupBox.Name = "Diplay " + countDisplays;
            groupBox.Size = new System.Drawing.Size(418, 100);
            groupBox.TabIndex = 1;
            groupBox.TabStop = false;
            groupBox.Text = "Diplay " + countDisplays;

            return groupBox;
        }

        private void displayTypeSelectedIndexChange(object sender, System.EventArgs e)
        {
            ComboBox displayTypeComboBox = (ComboBox)sender;
            object displayBox = displayTypeComboBox.Parent;
            string selectedItem = (string)displayTypeComboBox.SelectedItem;

            if (selectedItem == "list")
            {
/*                ComboBox displayTypeComboBox = new ComboBox();
                displayTypeComboBox.FormattingEnabled = true;
                displayTypeComboBox.Items.AddRange(new object[] { "List", "Pilot" });
                displayTypeComboBox.Location = new System.Drawing.Point(96, 19);
                displayTypeComboBox.Name = "DisplayType";
                displayTypeComboBox.Size = new System.Drawing.Size(121, 21);
                displayTypeComboBox.TabIndex = 2;
                displayTypeComboBox.SelectedIndexChanged += new System.EventHandler(displayTypeSelectedIndexChange);*/
            }
            else
            {

            }

            int a = 1;
        }
    }
}
