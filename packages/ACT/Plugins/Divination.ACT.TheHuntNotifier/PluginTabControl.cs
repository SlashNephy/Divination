using System;
using System.Windows.Forms;
using Divination.ACT.TheHuntNotifier.TheHunt.Data;

namespace Divination.ACT.TheHuntNotifier
{
    public partial class PluginTabControl : UserControl
    {
        public PluginTabControl()
        {
            InitializeComponent();
        }

        private void PluginTabControl_Load(object sender, EventArgs e)
        {
            foreach (var name in Enum.GetNames(typeof(World)))
            {
                worldComboBox.Items.Add(name);
            }
        }
    }
}
