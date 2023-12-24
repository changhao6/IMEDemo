using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME
{
    internal class MyTextBox : TextBox
    {

        const int GCS_RESULTREADSTR = 0x0200;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_IME_ENDCOMPOSITION)
            {
                MessageBox.Show(GetCompositionString(GCS_RESULTREADSTR));
            }
            base.WndProc(ref m);
        }

        string GetCompositionString(uint dwIndex)
        {

            IntPtr hWnd = this.Handle;

            IntPtr hIMC = ImmGetContext(hWnd);

            int dwSize = ImmGetCompositionString(hIMC, dwIndex, null, 0);

            if (dwSize <= 0)
            {
                return string.Empty;
            }

            string result = new string('\0', dwSize);
            ImmGetCompositionString(hIMC, dwIndex, result, dwSize);

            return result.ToString().TrimEnd('\0');
        }

        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        static extern int ImmGetCompositionString(IntPtr hIMC, uint dwIndex, string lpBuf, int dwBufLen);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
    }
}
