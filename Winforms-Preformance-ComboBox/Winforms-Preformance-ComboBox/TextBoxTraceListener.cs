using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Trevor.Fellman
{
    public class TextBoxTraceListener : TraceListener
    {
        public TextBox _textBox;

        public TextBoxTraceListener(TextBox textBox)
        {
            this._textBox = textBox;
        }

        public override void Write(string message)
        {
            UpdateTextBox(message, false);
        }

        public override void WriteLine(string message)
        {
            UpdateTextBox(message, true);
        }

        void UpdateTextBox(string message, bool newLine = false)
        {
            if (null == this._textBox)
                return;

            if (_textBox.InvokeRequired)
            {
                _textBox.BeginInvoke(new Action(() => UpdateTextBox(message, newLine)));
            }
            else
            {
                _textBox.Text += message + (newLine ? Environment.NewLine : " ");
            }

        }
    }
}