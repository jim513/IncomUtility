using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IncomUtility
{
    class LogToFile
    {
        private string log_file_name;
        private int log_file_name_numbering = 1;
        private int log_data_max = 10000;
        private int log_data_count = 0;

        private FileStream log_fs;
        private StreamWriter FileWriter;
        private System.Windows.Forms.FolderBrowserDialog LogBrowserDialog;
        private string open_file_path;
        private string data_header;

        public LogToFile(string file_name_prefix, string file_extension)
        {
            log_file_name = "\\" + file_name_prefix + "_{0:yyyy-MM-dd-HHmmss}_{1:000}." + file_extension;
        }

        private void New()
        {
            open_file_path = LogBrowserDialog.SelectedPath + string.Format(log_file_name, DateTime.Now, log_file_name_numbering++);

            log_fs = File.Open(open_file_path, FileMode.Append, FileAccess.Write, FileShare.None);

            FileWriter = new StreamWriter(log_fs);
            FileWriter.WriteLine(data_header);
            FileWriter.Close();
        }

        public void Reset()
        {
            open_file_path = null;

            log_data_count = 0;

            log_file_name_numbering = 1;
        }

        public string Checked()
        {
            LogBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            LogBrowserDialog.ShowDialog();

            if (LogBrowserDialog.SelectedPath == "")
            {
                return null;
            }

            open_file_path = LogBrowserDialog.SelectedPath + string.Format(log_file_name, DateTime.Now, log_file_name_numbering);

            return open_file_path;
        }

        public void Unchecked()
        {
            Close();

            Reset();
        }

        public void SetDataHeader(string header)
        {
            data_header = header;
        }

        public void GetCountMax(TextBox input_max_textbox)
        {
            try
            {
                log_data_max = int.Parse(input_max_textbox.Text);
            }
            catch
            {
                log_data_max = 10000;

                input_max_textbox.Dispatcher.BeginInvoke(new Action(() =>
                {
                    input_max_textbox.Text = "10000";
                }
                ));
            }

            if (log_data_max < 1)
            {
                log_data_max = 10000;

                input_max_textbox.Dispatcher.BeginInvoke(new Action(() =>
                {
                    input_max_textbox.Text = "10000";
                }
                ));
            }
        }

        public long Position
        {
            set
            {
                if (log_fs != null)
                {
                    log_fs.Position = value;
                }
            }

            get
            {
                if (log_fs == null)
                {
                    return 0;
                }
                else
                {
                    return log_fs.Position;
                }
            }
        }

        public void Write(byte[] data, int offset, int count)
        {
            if (open_file_path != null)
            {
                if (log_data_count == 0)
                {
                    log_fs = File.Open(open_file_path, FileMode.Append, FileAccess.Write, FileShare.None);
                }

                if (log_fs != null)
                {
                    log_fs.Write(data, offset, count);

                    log_data_count++;
                }
            }
        }

        public string Write(string log)
        {
            if (open_file_path != null)
            {
                if (log_data_count >= log_data_max)
                {
                    log_data_count = 0;
                }

                if (log_data_count == 0)
                {
                    New();
                }

                try
                {
                    log_fs = File.Open(open_file_path, FileMode.Append, FileAccess.Write, FileShare.None);
                    FileWriter = new StreamWriter(log_fs);
                    FileWriter.WriteLine(log);
                    FileWriter.Close();

                    log_data_count++;
                }
                catch
                {

                }
            }

            return open_file_path;
        }

        public void Close()
        {
            if (log_fs != null)
            {
                log_fs.Close();
                log_fs = null;
            }
        }
    }
}
