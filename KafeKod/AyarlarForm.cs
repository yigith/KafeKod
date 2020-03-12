﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeKod
{
    public partial class AyarlarForm : Form
    {
        public AyarlarForm()
        {
            InitializeComponent();
            nudMasaAdet.Value = Properties.Settings.Default.MasaAdet;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MasaAdet = (int)nudMasaAdet.Value;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }
    }
}
