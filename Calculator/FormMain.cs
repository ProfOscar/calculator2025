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
        Label resultLabel;

        //private char[,] buttons =
        //{
        //    { '%', '\u0152', 'C', '\u232B' },
        //    { '\u215F', '\u00B2', '\u221A', '\u00F7' },
        //    { '7', '8', '9', 'x' },
        //    { '4', '5', '6', '-' },
        //    { '1', '2', '3', '+' },
        //    { '\u00B1', '0', ',', '=' }
        //};

        static private Color OPERATION_BKG = Color.LightGray;
        static private Color NUMBER_BKG = Color.WhiteSmoke;
        static private Color EQUAL_BKG = Color.LightSeaGreen;

        public struct BtnStruct
        {
            public char Content;
            public Color Color;
            public BtnStruct(char content, Color color)
            {
                this.Content = content;
                this.Color = color;
            }
            public override string ToString()
            {
                return Content.ToString();
            }
        }

        private BtnStruct[,] buttons =
        {
            { new BtnStruct('%', OPERATION_BKG), new BtnStruct('\u0152', OPERATION_BKG), new BtnStruct('C', OPERATION_BKG), new BtnStruct('\u232B', OPERATION_BKG) },
            { new BtnStruct('\u215F', OPERATION_BKG), new BtnStruct('\u00B2', OPERATION_BKG), new BtnStruct('\u221A', OPERATION_BKG), new BtnStruct('\u00F7', OPERATION_BKG) },
            { new BtnStruct('7', NUMBER_BKG), new BtnStruct('8', NUMBER_BKG), new BtnStruct('9', NUMBER_BKG), new BtnStruct('x', OPERATION_BKG) },
            { new BtnStruct('4', NUMBER_BKG), new BtnStruct('5', NUMBER_BKG), new BtnStruct('6', NUMBER_BKG), new BtnStruct('-', OPERATION_BKG) },
            { new BtnStruct('1', NUMBER_BKG), new BtnStruct('2', NUMBER_BKG), new BtnStruct('3', NUMBER_BKG), new BtnStruct('+', OPERATION_BKG) },
            { new BtnStruct('\u00B1', NUMBER_BKG), new BtnStruct('0', NUMBER_BKG), new BtnStruct(',', NUMBER_BKG), new BtnStruct('=', EQUAL_BKG) }
        };

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MakeResultLabel();
            MakeButtons(buttons.GetLength(0), buttons.GetLength(1));
        }

        private void MakeResultLabel()
        {
            resultLabel = new Label();
            resultLabel.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            resultLabel.TextAlign = ContentAlignment.MiddleRight;
            resultLabel.AutoSize = false;
            resultLabel.Location = new Point(0, 0);
            resultLabel.Size = new Size(this.Width, 100);
            this.Controls.Add(resultLabel);
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
                    // button.Text = buttons[i,j].ToString();
                    button.Text = buttons[i,j].Content.ToString();
                    button.BackColor = buttons[i,j].Color;
                    button.Click += Button_Click;
                    Controls.Add(button);
                    posX += btnWidth;
                }
                posY += btnHeight;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            resultLabel.Text += btn.Text;
        }
    }
}
