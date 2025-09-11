using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormMain : Form
    {
        private char[,] buttons =
        {
            { '%', '\u0152', 'C', '\u232B' },
            { '\u215F', '\u00B2', '\u221A', '\u00F7' },
            { '7', '8', '9', 'x' },
            { '4', '5', '6', '-' },
            { '1', '2', '3', '+' },
            { '\u00B1', '0', ',', '=' }
        };

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MakeButtons(buttons.GetLength(0), buttons.GetLength(1));
        }

        private void MakeButtons(int nRow, int nCol)
        {
            int btnWidth = 80, btnHeight = 60;
            int posY = 106;
            for (int i = 0; i < nRow; i++)
            {
                int posX = 0;
                for (int j = 0; j < nCol; j++)
                {
                    Button button = new Button();
                    button.Width = btnWidth;
                    button.Height = btnHeight;
                    button.Top = posY;
                    button.Left = posX;
                    button.Font = new Font("Segoe UI", 16);
                    button.Text = buttons[i,j].ToString();
                    Controls.Add(button);
                    posX += btnWidth;
                }
                posY += btnHeight;
            }
        }
    }
}
