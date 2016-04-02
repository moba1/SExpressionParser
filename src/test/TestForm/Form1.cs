using System;
using System.IO;
using System.Windows.Forms;

using SExpressionParser;
using TestForm.Properties;

namespace TestForm
{
    /// <summary>
    /// SExpressionParserのテストのためのメインフォーム
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Load"ボタンが押されたら呼び出されるコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            // ファイルダイアログを開いてファイルを選択させる
            if (ofd.ShowDialog() == DialogResult.OK)
                sourceBox.Text = File.ReadAllText(ofd.FileName);
        }

        /// <summary>
        /// "Go"ボタンが押されたら呼び出されるコールバックメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click(object sender, EventArgs e)
        {
            var parser = new Parser();
            
            try
            {
                // S式をパースした結果をダンプして表示
                MessageBox.Show(Dump.Dumps(parser.Parse(sourceBox.Text)), Resources.Form1_goButton_Click_Result, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                // エラーが起きた場合はエラー内容を通知
                MessageBox.Show(Resources.Form1_goButton_Click_パースエラーです, Resources.Form1_goButton_Click_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
