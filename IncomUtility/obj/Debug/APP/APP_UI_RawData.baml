   M S B A M L   `   `   `  ����G  CIncomUtility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:IncomUtility.APPIncomUtility.APP  5   	   %    IncomUtility.APP.APP_UI_RawData   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:IncomUtility.APP     5   	     � Title$  Monitor Raw Data��5   	   $	��600��6"   $	��600��6/   .��5	      � $��40��6   $	��170��6    � RowDefinitions 5
   
   �� 5      $	��44* �6   �� 5      $	��51* �6   .+�5   
   �� -   5         ��Name$ tBtn_GetGasReading��5      $��Get Gas Reading��6-   $
��Left=�6G   $��	10,10,0,0q�6b   $	��Top=�6u   $	��170��6�   $	��170��6�   $��30��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadRawADC��5      $��Read Raw ADC��6*   $
��Left=�6A   $��	10,60,0,0q�6\   $	��Top=�6o   $	��170��6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadTemperature��5      $��Read Temperature��6/   $
��Left=�6J   $��
10,110,0,0q�6e   $	��Top=�6y   $	��170��6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadAnalogueOutput��5      $��Read Analogue Output��62   $��
10,210,0,0q�6Q   $	��Top=�6e   $	��170��6}   $��30��6�   $	��170��6�   $
��Left=�6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_DebugGasReading��5      $��Debug Gas Reading��6/   $
��Left=�6K   $��
10,160,0,0q�6f   $	��Top=�6z   $	��170��6�    J� RenderTransformOrigin$ -1.234,0.475&�6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadVoltageOutput��5      $��Read Voltage Output��61   $
��Left=�6O   $��
210,10,0,0q�6j   $	��Top=�6~   $	��170��6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadVoltageLevel��5      $��Read Voltage Levels��60   $
��Left=�6N   $��
210,60,0,0q�6i   $	��Top=�6}   $	��170��6�   $ -0.032,0.553&�6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -   5        $ tBtn_ReadCellVoltage��5      $��Read Cell Voltage/Current��6/   $
��Left=�6S   $��210,110,0,0q�6n   $	��Top=�6�   $	��170��6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -	   5        $ tBtn_ReadRawCellSignal��5      $��Read Raw Cell Signal��61   $
��Left=�6P   $��210,160,0,0q�6k   $	��Top=�6�   $	��170��6�   $��30��6�   $	��170��6�   $��40��6�   6�   �� 5   
   -
   5         ��Name$ 	tTxt_Logs��5      $��1��6%   $��10q�62   6@   5      5      