   M S B A M L   `   `   `  ����  SCZYG    ASCZY.Admin.ProductDialog.EditProduct_Provider.AddProduct_Provider   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������  � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-    � Title$! 添加供应商报价信息��5   	    � ShowInTaskbar .  $	��556��61   $	��306��6=    � 	Resources 5       �� 
TargetType  TextBlockStyle �� Property	 �� Value  TextBoxStyle��%�  &	       &	 �     �� 5	   
   " ��6   .�5
      ��  � ��$	 Center=�64   �� 5       � ��$	 Center=�62   �� 5       � ��$	 80��6&   �� 5       � ��$	 13(�6)   �� 5       � ��	 �#5556+   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 20��6'   �� 5       � ��$		 140��6&   5      5      .��5      � -   5        
 �Name$
 MainGrid��5      $��1��6   
�������$��5 5 10 0q�6=    � RowDefinitions 5   
   �� 5      $
��8.5* �6   �� 5      $
��1.5* �6   .+�5   
   �� ���#c7c7c7$��1q�6'   $��5 5 5 -5q�6;   ._�5      ��  �� Orientation$ Vertical=�6   $��0 5 0 5q�60    J� RenderTransformOrigin$ 0.499,0.508&�6A   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��ProviderDataGrid��5         RowHeaderWidth$ 0Q�64   $��0q�6G      SelectionMode$ Single=�5         GridLinesVisibility$
 None=�62   ���#868686$��13(�5   0      AutoGenerateColumns .     CanUserAddRows .     	CellStyle 5"      �� 5#      *  $System.Windows.Controls.DataGridCell"  6    .�5$      ��  � ��$	 0 0 0 1q�6@   �� 5%       � ��	 �#fff6<    �� Triggers 5&      P� 5'   "    P� Property �@ 
IsSelected P� Value . . �5(   &   ��  � ��$	 0 0 0 1q�6H   �� 5)   &    � ��	 �#72aee06C   5*   #   5,         RowStyle 5.      �� 5/      )  #System.Windows.Controls.DataGridRow"  6    .�50      ��  � ��	 �#d5d6db6<   �� 51       � ��$	 0 0 0 1q�6@    52      P� 53   "    �@ 
IsSelected . . �54   &   ��  � ��$	 0 0 0 1q�6H   �� 55   &    � ��	 �#72aee06D   56   #   P� 57   "    �@J�IsMouseOver . . �58   &   ��  � ��	 �#72aee06D   59   #   5;         Columns 5=      0  *System.Windows.Controls.DataGridTextColumn  5>      ,  &System.Windows.Controls.DataGridColumn   Header$ 供应商姓名��6-      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6N   �� *Name+  5?      $ 公司名称��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6;    .  6X   �� *	Company+  5@      $ 公司地址��6-   $ 2* 6;    .  6X   �� *	Address+  5A      $ 公司电话��6-   $ 2* 6;    .  6X   �� *Tel+  5B      $ 手机��6-   $
 2.5* 69    .  6X   �� *Phone+5D      5E      5F      �� 5G   
   -   5        $��BtnStackPanel��5G      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   .+�5H      �� $��产品报价：��6   $
��Bold"�6%   $��13(�67   ���#555$��Center=�6W   �� 5I      -   5        $��PriceBox��5I      $	��120��6&   $��20��62   �� 5K      -   5        $��WanningBlock��5K      $
��Bold"�6-   $��13(�6?   ���#555$��Center=�6_   $	��190��6z   	元/吨6�   �� 5L      -   5        $��
SaveButton��5L      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6k   保存6o   �� 5M      -   5        $��CancelButton��5M      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5N      5O      5P      