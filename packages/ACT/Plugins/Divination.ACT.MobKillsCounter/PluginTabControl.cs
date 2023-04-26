using System;
using System.Windows.Forms;

#nullable enable
namespace Divination.ACT.MobKillsCounter
{
    public partial class PluginTabControl : UserControl
    {
        private bool updateFromOverlayMove;

        public PluginTabControl()
        {
            InitializeComponent();

            updateFromOverlayMove = false;
        }

        public void CounterView_Move(object sender, EventArgs? e)
        {
            updateFromOverlayMove = true;
            udViewX.Value = Plugin.Worker.CounterView.Left;
            udViewY.Value = Plugin.Worker.CounterView.Top;
            updateFromOverlayMove = false;
        }

        private void checkViewVisible_CheckedChanged(object sender, EventArgs? e)
        {
            if (checkViewVisible.Checked)
            {
                Plugin.Worker.CounterView.Show();
            }
            else
            {
                Plugin.Worker.CounterView.Hide();
            }
        }

        private void checkViewMouseEnable_CheckedChanged(object sender, EventArgs? e)
        {
            Plugin.Worker.CounterView.MoveByDrag = checkViewMouseEnable.Checked;
        }

        public bool IsAllKillsEnable()
        {
            return checkAllKillsEnable.Checked;
        }

        private void udViewX_ValueChanged(object sender, EventArgs? e)
        {
            if (!updateFromOverlayMove)
            {
                Plugin.Worker.CounterView.Left = (int) udViewX.Value;
            }
        }

        private void udViewY_ValueChanged(object sender, EventArgs? e)
        {
            if (!updateFromOverlayMove)
            {
                Plugin.Worker.CounterView.Top = (int) udViewY.Value;
            }
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs? e)
        {
            Plugin.Worker.CounterView.Opacity = (double) trackBarOpacity.Value / 100;
            labelCurrOpacity.Text = $"{trackBarOpacity.Value}%";
        }

        private void PluginTabControl_Load(object sender, EventArgs e)
        {
            checkAllKillsEnable.Checked = true;

            udViewX.Value = 100;
            udViewX_ValueChanged(this, null);

            udViewY.Value = 100;
            udViewY_ValueChanged(this, null);

            trackBarOpacity.Value = 70;
            trackBarOpacity_ValueChanged(this, null);

            checkViewMouseEnable.Checked = true;
            checkViewMouseEnable_CheckedChanged(this, null);

            checkViewVisible.Checked = true;
            checkViewVisible_CheckedChanged(this, null);
        }
    }
}
