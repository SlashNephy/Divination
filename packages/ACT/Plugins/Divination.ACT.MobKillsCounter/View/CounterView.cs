using System;
using System.Windows.Forms;
using Divination.ACT.MobKillsCounter.Util;

namespace Divination.ACT.MobKillsCounter.View
{
    public partial class CounterView : Form
    {
        private bool moveByDrag;

        public CounterView()
        {
            InitializeComponent();

            MoveByDrag = true;

            Move += CounterView_Move;
            MouseDown += CounterView_MouseDown;

            Reset();
        }

        public bool MoveByDrag
        {
            get => moveByDrag;
            set
            {
                moveByDrag = value;
                Win32ApiUtils.SetWS_EX_TRANSPARENT(Handle, !moveByDrag);
            }
        }

        private void CounterView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MoveByDrag)
            {
                Win32ApiUtils.DragMove(Handle);
            }
        }

        private static void CounterView_Move(object sender, EventArgs e)
        {
            Plugin.TabControl.CounterView_Move(sender, e);
        }

        private void Reset()
        {
            dataGridView1.Rows.Clear();
            UpdateLayout();
        }

        public void CountUp(string mob)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    continue;
                }

                if (row.Cells[0].Value.ToString() != mob)
                {
                    continue;
                }

                var count = Convert.ToInt32(row.Cells[1].Value);
                row.Cells[1].Value = ++count;

                return;
            }

            dataGridView1.Rows.Add(mob, 1);
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            // グリッドの行数変化に合わせてフォームサイズも変える
            Height = dataGridView1.ColumnHeadersHeight +
                     dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.None);
        }
    }
}
