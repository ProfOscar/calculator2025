using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
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

        static private Color OPERATOR_BKG = Color.LightGray;
        static private Color NUMBER_BKG = Color.WhiteSmoke;
        static private Color EQUAL_BKG = Color.LightSeaGreen;

        public enum SymbolType
        {
            Number,
            Operator,
            EqualSign,
            DecimalPoint,
            PlusMinusSign,
            Backspace,
            Undefined
        }

        public struct BtnStruct
        {
            public char Content;
            public SymbolType Type;
            public BtnStruct(char content, SymbolType type = SymbolType.Undefined)
            {
                this.Content = content;
                this.Type = type;
            }
            public override string ToString()
            {
                return Content.ToString();
            }
        }

        private BtnStruct[,] buttons =
        {
            { new BtnStruct('%'), new BtnStruct('\u0152'), new BtnStruct('C'), new BtnStruct('\u232B', SymbolType.Backspace) },
            { new BtnStruct('\u215F'), new BtnStruct('\u00B2'), new BtnStruct('\u221A'), new BtnStruct('\u00F7', SymbolType.Operator) },
            { new BtnStruct('7', SymbolType.Number), new BtnStruct('8', SymbolType.Number), new BtnStruct('9', SymbolType.Number), new BtnStruct('x', SymbolType.Operator) },
            { new BtnStruct('4', SymbolType.Number), new BtnStruct('5', SymbolType.Number), new BtnStruct('6', SymbolType.Number), new BtnStruct('-', SymbolType.Operator) },
            { new BtnStruct('1', SymbolType.Number), new BtnStruct('2', SymbolType.Number), new BtnStruct('3', SymbolType.Number), new BtnStruct('+', SymbolType.Operator) },
            { new BtnStruct('\u00B1', SymbolType.PlusMinusSign), new BtnStruct('0', SymbolType.Number), new BtnStruct(',', SymbolType.DecimalPoint), new BtnStruct('=', SymbolType.EqualSign) }
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
            resultLabel.Padding = new Padding(18);
            resultLabel.Text = "0";
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
                    switch (buttons[i, j].Type)
                    {
                        case SymbolType.Number:
                            button.BackColor = NUMBER_BKG;
                            break;
                        case SymbolType.Operator:
                            button.BackColor = OPERATOR_BKG;
                            break;
                        case SymbolType.EqualSign:
                            button.BackColor = EQUAL_BKG;
                            break;
                        case SymbolType.DecimalPoint:
                            button.BackColor = NUMBER_BKG;
                            break;
                        case SymbolType.PlusMinusSign:
                            button.BackColor = NUMBER_BKG;
                            break;
                    }
                    button.Tag = buttons[i,j];
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
            BtnStruct btnStruct = (BtnStruct)btn.Tag;
            switch (btnStruct.Type)
            {
                case SymbolType.Number:
                    if (resultLabel.Text == "0") resultLabel.Text = "";
                    resultLabel.Text += btn.Text;
                    break;
                case SymbolType.Operator:
                    break;
                case SymbolType.EqualSign:
                    break;
                case SymbolType.DecimalPoint:
                    if (!resultLabel.Text.Contains(",")) resultLabel.Text += ",";
                    break;
                case SymbolType.PlusMinusSign:
                    if (!resultLabel.Text.Contains("-"))
                        resultLabel.Text = "-" + resultLabel.Text;
                    break;
                case SymbolType.Backspace:
                    break;
                case SymbolType.Undefined:
                    break;
                default:
                    break;
            }
        }
    }
}
