using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormMain : Form
    {
        static private Color OPERATOR_BKG = Color.LightGray;
        static private Color NUMBER_BKG = Color.WhiteSmoke;
        static private Color EQUAL_BKG = Color.LightSeaGreen;

        const float LBLRESULT_DEFAULT_FONT_SIZE = 36;
        const int LBLRESULT_DEFAULT_V_PADDING = 12;
        const int LBLRESULT_DEFAULT_H_PADDING = 16;

        Label lblInfo;
        Label lblResult;

        public enum SymbolType
        {
            Number,
            Operator,
            SpecialOperator,
            EqualSign,
            DecimalPoint,
            PlusMinusSign,
            Backspace,
            ClearAll,
            ClearEntry,
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
            { new BtnStruct('%'), new BtnStruct('\u0152', SymbolType.ClearEntry), new BtnStruct('C', SymbolType.ClearAll), new BtnStruct('\u232B', SymbolType.Backspace) },
            { new BtnStruct('\u215F', SymbolType.SpecialOperator), new BtnStruct('\u00B2', SymbolType.SpecialOperator), new BtnStruct('\u221A', SymbolType.SpecialOperator), new BtnStruct('\u00F7', SymbolType.Operator) },
            { new BtnStruct('7', SymbolType.Number), new BtnStruct('8', SymbolType.Number), new BtnStruct('9', SymbolType.Number), new BtnStruct('x', SymbolType.Operator) },
            { new BtnStruct('4', SymbolType.Number), new BtnStruct('5', SymbolType.Number), new BtnStruct('6', SymbolType.Number), new BtnStruct('-', SymbolType.Operator) },
            { new BtnStruct('1', SymbolType.Number), new BtnStruct('2', SymbolType.Number), new BtnStruct('3', SymbolType.Number), new BtnStruct('+', SymbolType.Operator) },
            { new BtnStruct('\u00B1', SymbolType.PlusMinusSign), new BtnStruct('0', SymbolType.Number), new BtnStruct(',', SymbolType.DecimalPoint), new BtnStruct('=', SymbolType.EqualSign) }
        };

        char lastOperator = ' ';
        decimal operand1, operand2, result;
        BtnStruct previousBtnStruct;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            MakeInfoLabel();
            MakeResultLabel();
            MakeButtons(buttons.GetLength(0), buttons.GetLength(1));
        }

        private void MakeInfoLabel()
        {
            lblInfo = new Label();
            lblInfo.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblInfo.TextAlign = ContentAlignment.MiddleRight;
            lblInfo.AutoSize = false;
            lblInfo.Location = new Point(0, 0);
            lblInfo.Size = new Size(this.Width - 16, 40);
            lblInfo.Padding = new Padding(LBLRESULT_DEFAULT_H_PADDING, LBLRESULT_DEFAULT_V_PADDING,
                    LBLRESULT_DEFAULT_H_PADDING, 0);
            lblInfo.Text = "";
            this.Controls.Add(lblInfo);
        }

        private void MakeResultLabel()
        {
            lblResult = new Label();
            lblResult.Font = new Font("Segoe UI", LBLRESULT_DEFAULT_FONT_SIZE, FontStyle.Regular);
            lblResult.TextAlign = ContentAlignment.MiddleRight;
            lblResult.AutoSize = false;
            lblResult.Location = new Point(0, 40);
            lblResult.Size = new Size(this.Width, 66);
            lblResult.Padding = new Padding(LBLRESULT_DEFAULT_H_PADDING, 0,
                    LBLRESULT_DEFAULT_H_PADDING, LBLRESULT_DEFAULT_V_PADDING);
            lblResult.Text = "0";
            lblResult.TextChanged += LblResult_TextChanged;
            this.Controls.Add(lblResult);
        }

        private void LblResult_TextChanged(object sender, EventArgs e)
        {
            // Formattazione adeguata dei numeri con separatore decimale e separatore delle migliaia
            if (lblResult.Text.Length > 0)
            {
                if (!decimal.TryParse(lblResult.Text, out decimal result))
                    lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
                decimal num = decimal.Parse(lblResult.Text);
                NumberFormatInfo nfi = new CultureInfo("it-IT", false).NumberFormat;
                int decimalSeparatorPosition = lblResult.Text.IndexOf(',');
                if (decimalSeparatorPosition == -1)
                    nfi.NumberDecimalDigits = 0;
                else
                    nfi.NumberDecimalDigits = lblResult.Text.Length - decimalSeparatorPosition - 1;
                string stOut = num.ToString("N", nfi);
                if (decimalSeparatorPosition == lblResult.Text.Length - 1)
                    stOut += ",";
                lblResult.Text = stOut;
            }

            int textWidth = TextRenderer.MeasureText(lblResult.Text, lblResult.Font).Width
                + 2 * LBLRESULT_DEFAULT_H_PADDING;
            if (textWidth > 0)
            {
                float newFontSize = lblResult.Font.Size * (((float)lblResult.Size.Width - LBLRESULT_DEFAULT_H_PADDING) / textWidth);
                if (newFontSize > LBLRESULT_DEFAULT_FONT_SIZE) newFontSize = LBLRESULT_DEFAULT_FONT_SIZE;
                lblResult.Font = new Font("Segoe UI", newFontSize, FontStyle.Regular);
            }
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
                    button.Text = buttons[i, j].Content.ToString();
                    switch (buttons[i, j].Type)
                    {
                        case SymbolType.Number:
                            button.BackColor = NUMBER_BKG;
                            break;
                        case SymbolType.Operator:
                        case SymbolType.SpecialOperator:
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
                    button.Tag = buttons[i, j];
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
                    if(previousBtnStruct.Type== SymbolType.EqualSign)
                    {
                        ClearAll();
						lblResult.Text = btn.Text;
					}
                    else
                    {
						if (lblResult.Text == "0" || previousBtnStruct.Type == SymbolType.Operator)
							lblResult.Text = "";
						lblResult.Text += btn.Text;
					}
                    break;
                case SymbolType.Operator:
                    ManageOperator(btnStruct);
                    break;
                case SymbolType.SpecialOperator:
                    ManageSpecialOperator(btnStruct);
                    break;
                case SymbolType.EqualSign:
                    ManageOperator(btnStruct);
                    if (lastOperator == '=') 
                        result = operand1;
                    break;
                case SymbolType.DecimalPoint:
					if (previousBtnStruct.Type == SymbolType.EqualSign)
                        ClearAll();
					else if (!lblResult.Text.Contains(",")) lblResult.Text += ",";
                    break;
                case SymbolType.PlusMinusSign:
                    if (lblResult.Text != "0")
                        if (!lblResult.Text.Contains("-"))
                            lblResult.Text = "-" + lblResult.Text;
                        else
                            lblResult.Text = lblResult.Text.Substring(1);
                    decimal current = decimal.Parse(lblResult.Text);
					if (previousBtnStruct.Type == SymbolType.EqualSign)
					{
						result = current;
						operand1 = result;
					}
					else if (previousBtnStruct.Type == SymbolType.Operator)
					{
						operand2 = current;
					}
					break;
                case SymbolType.Backspace:
                    if (previousBtnStruct.Type != SymbolType.Operator
                        && previousBtnStruct.Type != SymbolType.EqualSign)
                    {
                        lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
                        if (lblResult.Text == "" || lblResult.Text == "-" || lblResult.Text == "-0")
                            lblResult.Text = "0";
                    }
                    break;
                case SymbolType.ClearEntry:
                case SymbolType.ClearAll:
                    ClearAll();
                    break;
            }
            if (btnStruct.Type != SymbolType.Backspace && btnStruct.Type != SymbolType.PlusMinusSign)
                previousBtnStruct = btnStruct;
        }

		private void ManageSpecialOperator(BtnStruct btnStruct)
		{
            operand2 = decimal.Parse(lblResult.Text);
			switch (btnStruct.Content) {
                case '\u215F':
                    if (operand2 == 0)
                    {
                        ClearAll();
                    }
                    else
                    {
						operand2 = 1 / operand2;
					}
                    break;
                case '\u00B2':
                    break;
                case '\u221A':
                    break;
			}
            lblResult.Text = operand2.ToString();
		}

		private async void ManageOperator(BtnStruct btnStruct)
        {
            if (previousBtnStruct.Type == SymbolType.Operator)
            {
                lastOperator = btnStruct.Content;
            }
            else
            {
                if (lastOperator == ' ')
                {
                    operand1 = decimal.Parse(lblResult.Text);
                    lastOperator = btnStruct.Content;
                }
                else
                {
                    if (previousBtnStruct.Type != SymbolType.EqualSign)
                        operand2 = decimal.Parse(lblResult.Text);
                    else
                        operand1 = result;
                    if (!(previousBtnStruct.Type == SymbolType.EqualSign
                        && btnStruct.Type == SymbolType.Operator))
                        switch (lastOperator)
                        {
                            case '+':
                                result = operand1 + operand2;
                                break;
                            case '-':
                                result = operand1 - operand2;
                                break;
                            case 'x':
                                result = operand1 * operand2;
                                break;
                            case '\u00F7':
                                if (operand2 == 0)
                                {
                                    string temp = lblInfo.Text;
                                    lblInfo.Text = "Impossibile dividere per zero";
                                    await Task.Delay(2000);
                                    lblInfo.Text = temp;
                                    lblInfo.Text = operand1 + " " + lastOperator;
                                    return;
                                }
                                result = operand1 / operand2;
                                break;
                        }
                    if (btnStruct.Type != SymbolType.EqualSign)
                    {
                        operand1 = result;
                        lastOperator = btnStruct.Content;
                    }
                    lblResult.Text = result.ToString();
                }
            }
            if (btnStruct.Type != SymbolType.EqualSign)
                lblInfo.Text = operand1 + " " + lastOperator;
            else
                lblInfo.Text = lastOperator == '=' ? $"{operand1} =" : $"{operand1} {lastOperator} {operand2} =";
        }

        private void ClearAll()
        {
			operand1 = operand2 = result = 0;
			lastOperator = ' ';
			previousBtnStruct = default;
			lblInfo.Text = "";
			lblResult.Text = "0";
		}
    }
}
