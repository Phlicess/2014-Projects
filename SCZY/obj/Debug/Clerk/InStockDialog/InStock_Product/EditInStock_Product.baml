   M S B A M L   `   `   `  ����  SCZYB    <SCZY.Clerk.InStockDialog.InStock_Product.EditInStock_Product   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��EditInStock_ProduceWindow��5   	     � 
ResizeMode$  NoResize=�6*    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-    � Title$! 修改入库单产品信息��5   	    � ShowInTaskbar .  $	��556��61   $	��306��6=    � 	Resources 5       �� 
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
IsSelected P� Value . . �5(   &   ��  � ��$	 0 0 0 1q�6H   �� 5)   &    � ��	 �#72aee06C   5*   #   5,         RowStyle 5.      �� 5/      )  #System.Windows.Controls.DataGridRow"  6    .�50      ��  � ��	 �#d5d6db6<   �� 51       � ��$	 0 0 0 1q�6@    52      P� 53   "    �@ 
IsSelected . . �54   &   ��  � ��$	 0 0 0 1q�6H   �� 55   &    � ��	 �#72aee06D   56   #   P� 57   "    �@J�IsMouseOver . . �58   &   ��  � ��	 �#72aee06D   59   #   5;         Columns 5=      0  *System.Windows.Controls.DataGridTextColumn  5>      ,  &System.Windows.Controls.DataGridColumn   Header$ 	订单号��6-      
IsReadOnly .    
Visibility$ Hidden=�6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6`   �� *ID+  5?      $ 材质��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 69    .  6V   �� *	Texture+  5@      $ 品牌��6-   $ 2* 69    .  6V   �� *Brand+  5A      $ 克重��6-   $ 2* 69    .  6V   �� *
GramWeight+  5B      $ 宽幅��6-   $
 2.5* 69    .  6X   �� *Width+  5C      $ 长度��6-   $
 2.5* 69    .  6X   �� *Length+  5D      $ 等级��6-   $ * 69    .  6U   �� *Level+  5E      $ 剩余吨数��6-   $
 2.1* 6;    .  6Z   �� *Reservation+5G      5H      5I      �� 5J   
   -   5        $��BtnStackPanel��5J      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   .+�5K      �� $��订购数量：��6   $
��Bold"�6%   $��13(�67   ���#555$��Center=�6W   �� 5L      -   5        $��CountBox��5L      $	��120��6&   $��20��62   �� 5M      $	��吨��6   $��40��6!   $��20��6,   $
��Bold"�68   $��13(�6J   ���#555$��Center=�6j   �� 5O      -   5        $��WanningBlock��5O      
���Gc��$	��150��6@   6N   �� 5P      -   5        $��
SaveButton��5P      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6k   保存6o   �� 5Q      -   5        $��CancelButton��5Q      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5R      5S      5T      