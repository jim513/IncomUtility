   M S B A M L   `   `   `  ����G  CIncomUtility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:IncomUtility.APPIncomUtility.APP  5   	   *    $IncomUtility.APP.APP_UI_CalCellDrive   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:IncomUtility.APP     5   	   X TXceed.Wpf.Toolkit, Version=3.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  66     � Title$  Calibrate Cell Drive��5   	   $	��500��6&   $	��700��63   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��193* �6   �� 5      $
��204* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��Target Drive��6   $��10q�6*   .��5      �� -   5         ��Name$ tTxt_TargetDrive��5      $��Voltage : 3.3V��62    �� TextAlignment$ Center=�6H   $
��Bold"�6_   $��33(�6q   
����  �$��Center=�6�   5      �� 5   
   $��Cell Drive Feedback��6   $��10q�60   $��1��6<   .��5      �� -   5        $ tTxt_CellDriveFeedback��5      $��
3000 / 200��68   $ Center=�6J   $
��Bold"�6a   $��33(�6s   
����  �$��Center=�6�   5      �� 5   
   $��Adjust PWM duty��6   $��2��6,   $��10q�69   .��5      �  5      �� 5      $��* �6$   �� 5      $��* �6$    5       �� 5!      $
��153* �6'   �� 5"      $	��35* �6'   �� 5#      $	��80* �6'   �� 5$      $	��35* �6'   .+�5&      %  Xceed.Wpf.Toolkit.IntegerUpDown  -   5          Name$ tUpdown_AdjustPv��5&   %   $��2��6@   $��40,10,9.8,10q�6Q   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value$ 1X�6g   $��Center=�6r   ,  &Xceed.Wpf.Toolkit.Primitives.InputBase   TextAlignment$
 Left=�6�   $��36(�6�   �� 5'      -   5         ��Name$ tBtn_Increase��5'      $��2��60   $��Increase��6@   $��5q�6S   6^   �� 5(      -   5        $ tBtn_Decrease��5(      $��2��60   $��1��6A   $��Decrease��6N   $��5q�6a   6l   5)      5*      �� 5+   
   $��1��6   $��2��6"   $��10q�63   
�������$����1. Measure Cell Drive Voltage / Current with DVM                        
2. Adjust PWM to match target                        
3. Accpet when measured value matches the target
��5,      � 5/   
   $��1��6   $��2��6    50      �� 51      $��* �6#   �� 52      $��* �6#   �� 53      $��* �6#   .+�55      �� -   5        $ tBtn_StopCal��55      $��Stop Cal��6+   $��10,30q�6>   6M   �� 56      -   5        $ tBtn_StartCal��56      $��	Start Cal��6,   $��1��6@   $��10,30q�6P   6_   �� 57      -   5        $ tBtn_AcceptCal��57      $��
Accept Cal��6-   $��2��6B   $��10,30q�6R   6a   58      �� 59   
   -	   5        	 ��Name$	 	tTxt_Logs��59      $��3��6%   $��2��62   $��10q�6F   5:      5;      