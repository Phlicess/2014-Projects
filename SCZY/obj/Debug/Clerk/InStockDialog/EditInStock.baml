   M S B A M L   `   `   `  ����  SCZY*    $SCZY.Clerk.InStockDialog.EditInStock   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStyle$
  None=�5   	    � AllowsTransparency . 
������  � 
ResizeMode$ NoResize=�6O    � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-    � Title$ 修改库存信息��5   	    � ShowInTaskbar .  $	��200��6.   $	��300��6;    � 	Resources 5       �� 
TargetType  TextBlockStyle �� Property	 �� Value  TextBoxStyle��%�  &	       &	 �     �� 5   
   " ��6   .�5	      ��  � ��$	 Center=�64   �� 5
       � ��$	 Center=�62   �� 5       � ��$	 80��6&   �� 5       � ��$	 14(�6)   �� 5       � ��	 �#5556+   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 24��6'   �� 5       � ��$		 140��6&   �� 5       � ��$	 15(�6)   5      5      .��5      � -   5        
 �Name$
 MainGrid��5      $��1��6   
�������$��5 5 10 0q�6=    � RowDefinitions 5   
   �� 5      $��7* �6   �� 5      $��3* �6   .+�5   
   �� ���#c7c7c7$��1q�6'   $��5 5 5 -5q�6;   ._�5      ��  �� Orientation$ Vertical=�6   $��0 5 0 5q�60   .+�5      �� $ 
Horizontal=�6   $��35��66   .+�5      �� -   5        $��TextProvider��5       #��[ 64   $��25 0 0 0q�6\   供应商名:6u   �� 5      -   5        $��ProviderName��5      $	��140��63   $��24��6?   $��13(�6K   5       �� 5!      $ 
Horizontal=�6   $��35��66   .+�5"      �� -   5        $��TextInStockDate��5"       #��[ 67   $��25 0 0 0q�6_   入库日期:6x   (  "System.Windows.Controls.DatePicker  5#      -   5        $��InStockDate��5#   !   $	��140��64   $��24��6@   $��13(�6L   6\   5$      5%      5'      �� 5(   
   -   5        $��BtnStackPanel��5(      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   .+�5)      �� -   5        $��
SaveButton��5)      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6k   保存6o   �� 5*      -   5        $��CancelButton��5*      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5+      5,      5-      