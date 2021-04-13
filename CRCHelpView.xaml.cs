using GreenWhale.UpdateCore.Model;

using Lierda.Formatter;

using System;
using System.Windows;
using System.Windows.Controls;

namespace GreenWhale.UpdateCore
{
    /// <summary>
    /// CRCHelpView.xaml 的交互逻辑
    /// </summary>
    public partial class CRCHelpView : UserControl
    {
        public CRCHelpView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sou = source.Text;
            var plo = pole.Text;
            if (string.IsNullOrEmpty(sou))
            {
                MessageBox.Show("请输入待校验的数据");
                source.Focus();
                return;
            }
            if (string.IsNullOrEmpty(plo))
            {
                MessageBox.Show("请输入校验多项式");
                pole.Focus();
                return;
            }
            if (string.IsNullOrEmpty(ini.Text))
            {
                MessageBox.Show("请输入初始值");
                ini.Focus();
                return;
            }
            CRCHelper helper = new CRCHelper();
            var pole2=  Convert.ToUInt16(plo,16);
            helper.pPoly = pole2;
            var pole3 = Convert.ToUInt16(ini.Text, 16);
            helper.CRCInit = pole3;
            var res2 = sou.ToHex();
            var buffer=helper.GetCRC16(res2);
            var txt = buffer.ToHex();
            result.Text = txt;
        }
    }
}
