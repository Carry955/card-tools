using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 热水卡充值系统
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string in_money;
            String in_id;
            int money;
            String hex_money;
            String out_id = "";
            String hex_money_rev;

            //获取输入金额(文本)
            in_money = textBox1.Text;
            //若未输入金额
            if (in_money.Length == 0)
            {
                MessageBox.Show("金额不能为空！");
                return;
            }
            //获取输入十六进制串
            in_id = textBox.Text;
            //若输入位数不匹配
            if (in_id.Length < 32)
            {
                MessageBox.Show("输入长度小于32位！");
                return;
            }
            //当将字符串转换为数字失败
            try
            {
                money = Convert.ToInt32(in_money, 10) * 100;
            }
            catch
            {
                MessageBox.Show("金额必须为整数！");
                return;
            }
            //若金额大于655
            if (money > 65500)
            {
                MessageBox.Show("金额不能大于655！");
                return;
            }
            //将金额转换为十六进制
            hex_money = Convert.ToString(money, 16);
            while (hex_money.Length < 4)
            {
                hex_money = '0' + hex_money;
            }
            //十六进制金额(逆)
            hex_money_rev = hex_money.Substring(2, 2) + hex_money.Substring(0, 2);
            
            if (radioButton.IsChecked == true)
            {   //第一种加密类型
                //计算校验值
                string hex_check = Convert.ToString(Convert.ToInt32("FFFF", 16) - Convert.ToInt32(hex_money_rev, 16), 16);
                //不够补零
                while (hex_check.Length < 4)
                {
                    hex_check = "0" + hex_check;
                }
                //子串
                string sub_id_1 = in_id.Substring(4, 4);
                string sub_id_2 = in_id.Substring(20, 12);
                //结果=十六进制金额(逆)+源子串1+十六进制校验+"FFFF"+十六进制金额(逆)+源子串2
                out_id = hex_money_rev + sub_id_1 + hex_check + "FFFF" + hex_money_rev + sub_id_2;

            }else if(radioButton1.IsChecked == true)
            {
                //第二种加密类型
                //子串
                string sub_id = in_id.Substring(4, 26);
                //结果子串
                string sub_out_id = hex_money_rev + sub_id;
                //计算校验值
                int check = 0;
                for (int i = 0; i < 30; i += 2)
                {
                    String temp = sub_out_id.Substring(i, 2);
                    check = check + Convert.ToInt32(temp, 16);
                }
                //校验值(十六进制)
                string hex_check = Convert.ToString(check, 16);
                //不够补零
                while (hex_check.Length < 2)
                {
                    hex_check = '0' + hex_check;
                }
                //结果=十六进制金额(逆)+源子串+十六进制校验
                out_id = sub_out_id + hex_check.Substring(hex_check.Length - 2);
            }
            //结果十六进制串输出
            textBox2.Text = out_id.ToUpper();
        }
    }
}
