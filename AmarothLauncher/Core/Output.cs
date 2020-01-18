using System;
using System.Windows.Forms;

namespace AmarothLauncher.Core
{
    /// <summary>
    /// OutputWriter是一个单例，负责将输出写入主窗口中的调试窗口。
    /// </summary>
    class OutputWriter
    {
        static int index = 1;
        public static RichTextBox outputBox;
        private static OutputWriter instance;
        static Config c = Config.Instance;

        private OutputWriter() { }

        public static OutputWriter Instance
        {
            get
            {
                if (instance == null)
                    instance = new OutputWriter();
                return instance;
            }
        }

        /// <summary>
        /// 清空输出窗口并重置索引。
        /// </summary>
        public void Reset()
        {
            outputBox.Text = c.SubElText("Messages", "HelloMessage") + "\n\n";
            if (c.isDefaultConfigUsed)
                outputBox.Text += c.SubElText("Messages", "XmlNotOpened");
            index = 1;
        }

        #region Standart text outputs...
        /// <summary>
        /// 使用消息的ID将消息写入输出窗口。
        /// </summary>
        public void Output(string text)
        {
            if (outputBox != null)
                outputBox.Text += index + ": " + text + "\n";
            index++;
        }

        /// <summary>
        /// 将消息写入输出窗口，后跟换行符和给定异常的消息。
        /// </summary>
        public void Output(string text, Exception e)
        {
            Output(text + "\n" + e.Message);
            index++;
        }

        /// <summary>
        /// 使用消息的ID将消息写入输出窗口，并可以选择在其前面添加缩进。
        /// </summary>
        public void Output(string text, bool indent)
        {
            if (outputBox != null)
            {
                if (indent && index != 1)
                    outputBox.Text += "\n";
                Output(text);
            }
        }

        /// <summary>
        /// 根据bool result的结果，写入TRUE或FALSE。用于调试purpouses。
        /// </summary>
        public void Output(bool test)
        {
            if (test)
                Output("TRUE");
            else
                Output("FALSE");
        }
        #endregion

        #region MessageBox outputs, should be used for critical messages.
        /// <summary>
        /// 显示包含错误的消息框，并将错误发送到输出窗口。用于严重错误。
        /// </summary>
        public void Messagebox(string text)
        {
            MessageBox.Show(text);
            Output(text);
        }

        /// <summary>
        /// 显示一个消息框，其中包含和error以及由换行符分隔的给定异常的消息。用于严重错误。
        /// </summary>
        public void Messagebox(string text, Exception e)
        {
            MessageBox.Show(text + "\n" + e.Message);
            Output(text, e);
        }
        #endregion
    }
}