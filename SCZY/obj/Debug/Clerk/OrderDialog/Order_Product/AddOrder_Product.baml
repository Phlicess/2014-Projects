   M S B A M L   `   `   `  ����  SCZY-    'SCZY.Clerk.OrderDialog.AddOrder_Product   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��AddOrder_ProduceWindow��5   	     � 
ResizeMode$  NoResize=�6'    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-    � Title$ 添加订单产品信息��5   	    � ShowInTaskbar .  $	��556��60   $	��306��6<    � 	Resources 5       �� 
TargetType  TextBlockStyle �� Property	 �� Value  TextBoxStyle��%�  &	       &	 �     �� 5	   
   " ��6   .�5
      ��  � ��$	 Center=�64   �� 5       � ��$	 Center=�62   �� 5       � ��$	 80��6&   �� 5       � ��$	 13(�6)   �� 5       � ��	 �#5556+   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 20��6'   �� 5       � ��$		 140��6&   5      5      .��5      � -   5        
 �Name$
 MainGrid��5      $��1��6   
�������$��5 5 10 0q�6=    � RowDefinitions 5   
   �� 5      $
��8.5* �6   �� 5      $
��1.5* �6   .+�5   
   �� ���#c7c7c7$��1q�6'   $��5 5 5 -5q�6;   ._�5      ��  �� Orientation$ Vertical=�6   $��0 5 0 5q�60    J� RenderTransformOrigin$ 0.499,0.508&�6A   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��ProductDataGrid��5         RowHeaderWidth$ 0Q�63   $��0q�6F      SelectionMode$ Single=�5         GridLinesVisibility$
 None=�62   ���#868686$��13(�5   0      AutoGenerateColumns .     CanUserAddRows .     	CellStyle 5"      �� 5#      *  $System.Windows.Controls.DataGridCell"  6    .�5$      ��  � ��$	 0 0 0 1q�6@   �� 5%       � ��	 �#fff6<    �� Triggers 5&      P� 5'   "    P� Property �@ 
IsSelected P� Value . . �5(   &   ��  � ��$	 0 0 0 1q�6H   �� 5)   &    � ��	 �#72aee06C   5*   #   50         RowStyle 52      �� 53      )  #System.Windows.Controls.DataGridRow"  6    .�54      ��  � ��	 �#d5d6db6<   �� 55       � ��$	 0 0 0 1q�6@    56      P� 57   "    �@ 
IsSelected . . �58   &   ��  � ��$	 0 0 0 1q�6H   �� 59   &    � ��	 �#72aee06D   5:   #   P� 5;   "    �@J�IsMouseOver . . �5<   &   ��  � ��	 �#72aee06D   5=   #   5?         Columns 5A      0  *System.Windows.Controls.DataGridTextColumn  5B      ,  &System.Windows.Controls.DataGridColumn   Header$ 	订单号��6-      
IsReadOnly .    
Visibility$ Hidden=�6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6`   �� *ID+  5C      $ 材质��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 69    .  6V   �� *	Texture+  5D      $ 品牌��6-   $ 2* 69    .  6V   �� *Brand+  5E      $ 克重��6-   $ 2* 69    .  6V   �� *
GramWeight+  5F      $ 宽幅��6-   $
 2.5* 69    .  6X   �� *Width+  5G      $ 长度��6-   $
 2.5* 69    .  6X   �� *Length+  5H      $ 等级��6-   $ * 69    .  6U   �� *Level+  5I      $ 剩余吨数��6-   $
 2.1* 6;    .  6Z   �� *Reservation+5K      5a      5b      �� 5c   
   -   5        $��BtnStackPanel��5c      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   .+�5d      �� $��订购数量：��6   $
��Bold"�6%   $��13(�67   ���#555$��Center=�6W   �� 5e      -   5        $��CountBox��5e      $	��120��6&   $��20��62   �� 5f      -   5        $��UnitComboBox��5f      $��40��6+   $��20��66   �� 5g      -   5        $��WanningBlock��5g      
���Gc��$	��150��6@   6N   �� 5h      -   5        $��
SaveButton��5h      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6k   保存6o   �� 5i      -	   5        $��CancelButton��5i      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5j      5k      5l      