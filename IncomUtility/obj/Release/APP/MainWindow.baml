   M S B A M L   `   `   `  ����   IncomUtility    IncomUtility.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     X TXceed.Wpf.Toolkit, Version=3.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  -     � Icon$&   /IncomUtility;component/logo.ico�� � Title$ Incom Utility v.0.1.0��$	��800��$
��1350��.���  � ColumnDefinitions �� $	��93* ��� $	��92* ��� $	��92* ��� $
��262* ��� $
��316* ��� $
��120* ��� $
��359* ��� $	��10* � � RowDefinitions �� $	��29* ��� $	��81* ��� $
��405* ��� $
��256* �.+�o� $��4��
�������$��10,5,26.2,1.8q�$��Normal$�$��Normal"�$��13(�.@�m� $��_View��.@�m� -    m�Name$ tMenu_ViewBLEInfo��$��View BLE Info��m� -   $ tMenu_ViewDeviceInfo��$��View Device Info��m� -   $ tMenu_ViewLogs��$��	View Logs��m� $��_Status��.@�m� -   $ tMenu_MonitorDeviceStatus��$��Monitor Device Status��m� -   $ tMenu_MonitorRawData��$��Monitor Raw Data��m� $��_Setting��.@�m� -   $ tMenu_EditSensorData��$��Edit Sensor Data��m� -   $" tMenu_EditInstrumentSettings��$��Edit Instrument Settings��m� -	   $ tMenu_SecuritySetup��$��Securtiy Control��m� -
   $ tMenu_Communication��$��Communication��m� $��
_Calibrate��.@�m� -   $ tMenu_GasCalibration��$��Gas Calibration��m� -   $ tMenu_CalAnalougeOutput��$��Calibrate Analogue Output��m� -   $ tMenu_CalVoltageOutput��$��Calibrate Voltage Output��m� -   $ tMenuCalCellDrive��$��Calibrate Cell Drive��m� $��_Test��.@�m� -   $ tMenu_HardwareTest��$��Hardware Test��m� $��_Debug��.@�m� -   $ tMenu_CommLog��$��	Comm. Log��m� -   $ tMenu_Debug��$��Debug��&   System.Windows.Controls.DataGrid  -     Name$ tGrid_GasData��$��5��$��2��$��10.2,8.4,4.4,10.4q�$��2��   AutoGenerateColumns .     
IsReadOnly .    ColumnHeaderStyle �� 	 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"	  .��� 
 �� Property
 � �� �� Value$ Center=��� 
 � ��$
 Bold"�   Columns 0  *System.Windows.Controls.DataGridTextColumn  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *data0+   �� *data1+   �� *data2+   �� *data3+   �� *data4+   �� *data5+   �� *data6+   �� *data7+   �� *data8+   �� *data9+   �� *data10+   �� *data11+   �� *data12+   �� *data13+   �� *data14+   �� *data15+�� $��Gas Monitoring��$��5,4.2,4.8,6.6q�$��1��$��3��.���  �� $��* ��� $��* ��� $��* �.+��� -    ��Name$ tBtn_MainStart��$��START���� -   $ tBtn_MainStop��$
��STOP��$��1��{�.  �� -   $ tBtn_MainClear��$��CLEAR��$��2���� $��2��$��2��$��0,2.4,2.8,4.4q�$��2��.+��� $��Log Interval (s)��$��
10,10,10,5q�%  Xceed.Wpf.Toolkit.DecimalUpDown  -     Name$ tUpdown_LogInterval��$��	14,0,70,0q��  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 1X�   DefaultValue$ 1X�%   DisplayDefaultValueOnEmptyText . �� $��Firmware Version��$��
10,10,10,5q��� -    ��Name$ tTxt_FirmVersion��$��	14,0,70,0q��� $��Sensor Data Version��$��
10,10,10,5q��� -   $ tTxt_SensorDataVersion��$��	14,0,70,0q��� $��Number of readings��$��
10,10,10,5q�  -   $ tUpdown_NumReadings��$��	14,0,70,0q�$ 1X�$	 600X� .    Maximum$ 10000X��� -    ��Name$ tChb_CaputreDebugPacket��$��Capture Debug Packets��$��10,10,10,10q��� -   $ tChb_GetGasDataOnce��$��Get Gas Data Once��$��10,10,10,10q��� -   $ tBtn_ResetAlarmFaults��$��Reset Alarams & Faults��$��
10,10,10,0q�$��45���� $��Chart��$��2��$��7��$��10,404.4,3.4,10.4q�$��2��.��� $��5,5,0,0q� �� $��1145* ��� $
��162* �.+�7  1System.Windows.Forms.Integration.WindowsFormsHost  -   $��chart_1��$
��1130��$	��204��$��10,0,0,0q�$	��Top=�$
��Left=�   
Foreground Z� �� -   $��chart_1_legend_1��$��1��$��10.2,10,0,0q�$��Legend1��$
��Left=�$��92��$��20��$	��Top=�$��14(��� -   $��chart_1_legend_2��$��1��$��10.2,30,0,0q�$��Legend2��$
��Left=�$��92��$��20��$	��Top=�$��14(��� -    $��chart_1_legend_3��$��1��$��10.2,50,0,0q�$��Legend3��$
��Left=�$��92��$��20��$	��Top=�$��14(��� -!   $��chart_1_legend_4��$��1��$��10.2,70,0,0q�$��Legend4��$
��Left=�$��92��$��20��$	��Top=�$��14(��� -"   $��chart_1_legend_5��$��1��$��10.2,90,0,0q�$��Legend5��$
��Left=�$��92��$��20��$	��Top=�$��14(��� $��
Connection��$��10.8,10,10.2,9.6q�$��2��$��4��.���  �� $	��47* ��� $	��23* �.+��� -#   $ tBtn_Connect��$��Connect��$��1��$��8,10,10,10.2q�]� $��Center=�$��Center=�.+��� $��14(�$��	Connect :���� -$    ��Name$ tTxt_ConnectSatus��$��Not Connected��$��Center=�$��14(�
���  ���� $!��Check Gas Monitoring Status��$��6��$��3��$��4.8,0,2.4,404.4q�.���  �� $	��13* ��� $	��16* � �� $��* ��� $��* ��� $��* �.+��� $��Fault��$��Center=�$��Center=��� $��Alarm��$��Center=�$��Center=�$��1���� $��Wanrning��$��Center=�$��Center=�$��2���� -%   $ tTxt_FaultStatus��$��Normal��
��� � �$��Center=�$��1��$��Center=�$��17(��� -&   $ tTxt_AlarmStatus��$��Normal��
��� � �$��Center=�$��1��$��1��$��Center=�$��17(��� -'   $ tTxt_WarningStatus��$��Normal��
��� � �$��Center=�$��1��$��2��$��Center=�$��17(�