   M S B A M L   `   `   `  ����G  CIncomUtility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:IncomUtility.APPIncomUtility.APP  5   	   (    "IncomUtility.APP.APP_UI_SensorData   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:IncomUtility.APP     5   	     � Title$  Program Sensor Data��5   	   $	��450��6%   $	��600��62   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��19* �6   �� 5      $	��41* �6   .+�5   
   �� -   5         ��Name$ tBtn_WriteSensorData��5      $!��Write Sensor Data to Device��6/   $
��Left=�6U   $��	10,24,0,0q�6p   $	��Top=�6�   $	��130��6�   $��40��6�   $��30��6�   $	��170��6�   6�   �� 5   
   -   5        $ tBtn_ReadSensorData��5      $"��Read Sensor Data from Device��6.   $
��Left=�6U   $��
190,24,0,0q�6p   $	��Top=�6�   $	��170��6�   $��40��6�   6�   � 5   
   -   5         �Name$ tPbar_DataDownBar��5      $
��Left=�61   $��30��6L   
���   �$��	10,75,0,0q�6m   $	��Top=�6�   $	��300��6�   $��30��6�   $	��350��6�   �� 5   
   -   5         ��Name$ 	tTxt_Logs��5      $��1��6%   $��10q�62   5      5      