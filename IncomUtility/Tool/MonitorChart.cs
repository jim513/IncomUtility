using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;

namespace IncomUtility
{
    class MonitorChart
    {
        private const int m_series_max = 10;

        private int m_chart_data_max = 1800;
        private MainWindow m_window;
        private Chart m_chart;

        private Color[] m_temp_color = new Color[m_series_max];

        private void HideSeries(int index)
        {
            m_temp_color[index] = m_chart.Series[index].Color;

            m_chart.Series[index].Color = Color.FromArgb(0, 0, 0, 0);

            m_chart.Series[index].Enabled = false;
        }

        private void ShowSeries(int index)
        {
            m_chart.Series[index].Color = m_temp_color[index];

            m_chart.Series[index].Enabled = true;
        }

        private void ResetLegend()
        {
            for (int index = 0; index < m_chart.Series.Count; index++)
            {
                Series series = m_chart.Series[index];

                series.SetCustomProperty("LEGEND_TOGGLE", "OFF");
            }
        }

        private object GetObjectName(string str)
        {
            return m_window.FindName(str);
        }

        public MonitorChart(MainWindow main_window, WindowsFormsHost form)
        {
            m_window = main_window;

            m_chart = new Chart();
            ChartArea chartArea = new ChartArea();
            ((System.ComponentModel.ISupportInitialize)m_chart).BeginInit();

            m_chart.Location = new Point(0, 0);
            m_chart.Name = form.Name;

            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.Name = "ChartArea";
            m_chart.ChartAreas.Add(chartArea);
            m_chart.Legends.Add("Legend");

            form.Child = m_chart;

            m_chart.Legends[0].CellColumns.Add(new LegendCellColumn()
            {
                Name = "textbox",
                ColumnType = LegendCellColumnType.Text,
                Text = "#CUSTOMPROPERTY(LEGEND_BOX)",
            });

            m_chart.Legends[0].CellColumns.Add(new LegendCellColumn()
            {
                Name = "legendtoggle",
                ColumnType = LegendCellColumnType.Text,
                Text = "#CUSTOMPROPERTY(LEGEND_TOGGLE)",
            });
        }

        public MonitorChart(MainWindow main_window, WindowsFormsHost form, double y_max)
        {
            m_window = main_window;

            m_chart = new Chart();
            ChartArea chartArea = new ChartArea();
            ((System.ComponentModel.ISupportInitialize)m_chart).BeginInit();

            m_chart.Location = new Point(0, 0);
            m_chart.Name = form.Name;

            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.Maximum = y_max;
            chartArea.Name = "ChartArea";
            m_chart.ChartAreas.Add(chartArea);
            m_chart.Legends.Add("Legend");

            form.Child = m_chart;

            m_chart.Legends[0].CellColumns.Add(new LegendCellColumn()
            {
                Name = "textbox",
                ColumnType = LegendCellColumnType.Text,
                Text = "#CUSTOMPROPERTY(LEGEND_BOX)",
            });

            m_chart.Legends[0].CellColumns.Add(new LegendCellColumn()
            {
                Name = "legendtoggle",
                ColumnType = LegendCellColumnType.Text,
                Text = "#CUSTOMPROPERTY(LEGEND_TOGGLE)",
            });
        }

        public int WindowSize { get => m_chart_data_max; set => m_chart_data_max = value; }

        public Series AddSeries(string series_name, TextBlock legend_block, Color color)
        {
            if ((series_name == null) || (m_chart.Series.Count >= m_series_max))
            {
                return null;
            }

            Series series = m_chart.Series.Add(series_name);

            series.ChartType = SeriesChartType.Line;
            series.Color = color;
            series.IsVisibleInLegend = false;
            series.XValueType = ChartValueType.String;
            series.SetCustomProperty("LEGEND_BOX", legend_block.Name);
            series.SetCustomProperty("LEGEND_TOGGLE", "OFF");

            return series;
        }

        public void AddData(int index, object x, object y)
        {
            Series series = m_chart.Series[index];

            if (series.Points.Count >= m_chart_data_max)
            {
                series.Points.RemoveAt(0);
            }

            series.Points.AddXY(x, y);

            m_chart.Update();
        }

        public void RemoveData(int index, int data_index)
        {
            m_chart.Series[index].Points.RemoveAt(data_index);

            m_chart.Update();
        }

        public System.Windows.Media.Brush GetLegendBrush(int index)
        {
            Series series = m_chart.Series[index];
            TextBlock t_block = (TextBlock)GetObjectName(series.GetCustomProperty("LEGEND_BOX"));

            return t_block.Foreground;
        }

        public Color GetLegendColor(int index)
        {
            return m_chart.Series[index].Color;
        }

        public void Reset()
        {
            ResetLegend();

            HideLegend();

            m_chart.Series.Clear();
        }

        public void Refresh(int index)
        {
            DataPoint point;
            int count = m_chart.Series[index].Points.Count;

            if (count > 1)
            {
                count -= 1;
            }

            point = m_chart.Series[index].Points[count];

            AddData(index, point.XValue, point.YValues[0]);

            RemoveData(index, count);
        }

        public void ShowLegend()
        {
            for (int index = 0; index < m_chart.Series.Count; index++)
            {
                Series series = m_chart.Series[index];
                TextBlock t_block = (TextBlock)GetObjectName(series.GetCustomProperty("LEGEND_BOX"));

                Color color;

                if (series.Enabled == true)
                {
                    color = series.Color;

                    series.SetCustomProperty("LEGEND_TOGGLE", "ON");
                }
                else
                {
                    color = Color.DarkGray;

                    series.SetCustomProperty("LEGEND_TOGGLE", "OFF");
                }

                t_block.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
                t_block.Text = series.Name;
            }
        }

        public void HideLegend()
        {
            for (int index = 0; index < m_chart.Series.Count; index++)
            {
                Series series = m_chart.Series[index];
                TextBlock t_block = (TextBlock)GetObjectName(series.GetCustomProperty("LEGEND_BOX"));

                t_block.Text = "";
            }
        }

        public void ToggleLegend(int index)
        {
            Series series = m_chart.Series[index];
            TextBlock t_block = (TextBlock)GetObjectName(series.GetCustomProperty("LEGEND_BOX"));
            string on_off = series.GetCustomProperty("LEGEND_TOGGLE");
            Color legend_color;

            if (on_off == "OFF")
            {
                ShowSeries(index);

                legend_color = series.Color;

                series.SetCustomProperty("LEGEND_TOGGLE", "ON");
            }
            else
            {
                HideSeries(index);

                legend_color = Color.DarkGray;

                series.SetCustomProperty("LEGEND_TOGGLE", "OFF");
            }

            t_block.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(legend_color.R, legend_color.G, legend_color.B));

            Refresh(index);
        }
    }
}
