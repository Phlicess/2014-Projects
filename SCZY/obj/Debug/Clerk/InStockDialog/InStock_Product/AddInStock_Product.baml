   M S B A M L   `   `   `  ����  SCZYA    ;SCZY.Clerk.InStockDialog.InStock_Product.AddInStock_Product   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��AddInStock_ProductWindow��5        � Title$  添加产品��6	   $	��306��5   	   $	��556��6    � 
ResizeMode$ NoResize=�6"    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-   .��5	      � -   5         �Name$ MainGrid��5	      $��1��6   
�������$��5 5 10 0q�6=    � RowDefinitions 5
   
   �� 5      $
��8.5* �6   �� 5      $
��1.5* �6   .+�5   
   �� ���#c7c7c7$��1q�6'   $��5 5 5 -5q�6;   ._�5      ��  �� Orientation$ Vertical=�6   $��0 5 0 5q�60    J� RenderTransformOrigin$ 0.499,0.508&�6A   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��ProductDataGrid��5      	   RowHeaderWidth$	 0Q�63   $��0q�6F   
   SelectionMode$
 Single=�5         GridLinesVisibility$
 None=�62   ���#868686$��13(�5   0      AutoGenerateColumns .     CanUserAddRows .     	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    .�5      ��  �� Property � �� �� Value$ 0 0 0 1q�6@   �� 5       � �� �#fff6<    �� Triggers 5      P� 5   "    P� Property �@ 
IsSelected P� Value . . �5   &   ��  � ��$ 0 0 0 1q�6H   �� 5   &    � �� �#72aee06C   5   #   5         RowStyle 5!      �� 5"      )  #System.Windows.Controls.DataGridRow"  6    .�5#      ��  � �� �#d5d6db6<   �� 5$       � ��$ 0 0 0 1q�6@    5%      P� 5&   "    �@ 
IsSelected . . �5'   &   ��  � ��$ 0 0 0 1q�6H   �� 5(   &    � �� �#72aee06D   5)   #   P� 5*   "    �@J�IsMouseOver . . �5+   &   ��  � �� �#72aee06D   5,   #   5.         Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$ 入库单号��6-      
IsReadOnly .    
Visibility$ Hidden=�6M   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6a   �� *ID+  52      $ 材质��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 69    .  6V   �� *	Texture+  53      $ 品牌��6-   $ 2* 69    .  6V   �� *Brand+  54      $ 克重��6-   $ 2* 69    .  6V   �� *
GramWeight+  55      $ 宽幅��6-   $
 2.5* 69    .  6X   �� *Width+  56      $ 长度��6-   $
 2.5* 69    .  6X   �� *Length+  57      $ 等级��6-   $ * 69    .  6U   �� *Level+  58      $ 剩余吨数��6-   $
 2.1* 6;    .  6Z   �� *Reservation+5:      5;      5<      �� 5=   
   -   5        $��BtnStackPanel��5=      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   .+�5>      �� $��订购数量：��6   $
��Bold"�6%   $��13(�67   ���#555$��Center=�6W   �� 5?      -   5        $��CountBox��5?      $	��120��6&   $��20��62   �� 5@      -   5        $��UnitTextBlock��5@      $��40��6-   $��20��68   $	��吨��6D   $
��Bold"�6M   $��13(�6_   ���#555$��Center=�6   �� 5A      -   5        $��WanningBlock��5A      
���Gc��$	��150��6@   6N   �� 5B      -   5        $��
SaveButton��5B      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6k   保存6o   �� 5C      -	   5        $��CancelButton��5C      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5D      5E      5F      