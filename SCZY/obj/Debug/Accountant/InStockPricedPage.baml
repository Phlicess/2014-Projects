   M S B A M L   `   `   `  ����?  ;SCZY, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SCZY.CommonSCZY.Common  5      5clr-namespace:SCZY.OtherWindowsSCZY.OtherWindows  5      '    !SCZY.Accountant.InStockPricedPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,commonclr-namespace:SCZY.Common     5      8otherWindowsclr-namespace:SCZY.OtherWindows     5      $
��Auto��5      $
��Auto��6%     N� Title$  InStockPricedPage��5	       � 	Resources 5       �� MergedDictionaries �� Source   SCZY.Common.DateConverter   DataConverter�� 5   
    5      �� 5      $  ../Style/MainWinStyle.xaml?�6%   6K   �� 5      $! ../Style/MainPageStyle.xaml?�6%   6L   %   &	          5      6#   5      .��5       ]� .+�5"   
   �� $
��Auto��6   $��30��6"   $��	0 -30 0 0q�6.   $	��Top=�6A    �� Orientation$ 
Horizontal=�6V   $��Right=�6o   .+�5#      �� -   5        $��
SeacherBox��5#      $��Center=�6C   $	��140��6e   $��20��6q   �� 5$      -   5        $��SeacherBtn_Mo��5$      $��30��6)   $��30��65   $��../Images/Seacher_Mo.png��6@   ��5%      �� 5&       �� 
TargetType" ��6   .�5'      ��  �� Property �@J�Opacity �� Value$ 1Q�64   �� 5(       � ��$ Center=�6>   �� 5)       � ��$ Right=�6@    �� Triggers 5*      P� 5+      	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �5,   "   ��  �@�Cursor$
 Hand��6;   �� 5-   "    �@J�Opacity$ 0Q�6<   5.      50      52      �� 53      -   5        $��
SeacherBtn��53       � ToolTip$ 查找��6&   $��30��6f   $��30��6r   $��	-30 0 0 0q�6}   $��../Images/Seacher.png��6�   ��54      �� 55      " ��6   .�56      ��  �@J�Opacity$ 0Q�64   �� 57       � ��$ Right=�6@   �� 58       � ��$ Center=�6>    59      P� 5:      	 �@J�IsMouseOver
 . . �5;   "   ��  �@�Cursor$
 Hand��6;   �� 5<   "    �@J�Opacity$ 1Q�6<   5=      5?      5A      �� 5C      -   5        $��ResetBtn_Mo��5C      $��30��6'   $��30��63   $��../Images/Reset.png��6>   ��5D      �� 5E      " ��6   .�5F      ��  �@J�Opacity$ 1Q�64   �� 5G       � ��$ Center=�6>   �� 5H       � ��$ Right=�6@    5I      P� 5J      	 �@J�IsMouseOver
 . . �5K   "   ��  �@�Cursor$
 Hand��6;   �� 5L   "    �@J�Opacity$ 0Q�6<   5M      5O      5Q      �� 5R      -   5        $��ResetBtn��5R      $ 重置��6$   $��30��6b   $��30��6n   $��
-30 0 15 0q�6y   $��../Images/Reset_Mo.png��6�   ��5S      �� 5T      " ��6   .�5U      ��  �@J�Opacity$ 0Q�64   �� 5V       � ��$ Right=�6@   �� 5W       � ��$ Center=�6>    5X      P� 5Y      	 �@J�IsMouseOver
 . . �5Z   "   ��  �@�Cursor$
 Hand��6;   �� 5[   "    �@J�Opacity$ 1Q�6<   5\      5^      5`      5a      &   System.Windows.Controls.DataGrid  5z   
   -   5          Name$ PendingInStocks��5z   <   $��0q�6   $	��Top=�6'   
�������   SelectionMode$ Single=�5{         AutoGenerateColumns .     RowHeaderWidth$ 0Q�6F      GridLinesVisibility$
 None=�5|      ���#868686$��13(�6C      CanUserAddRows .     CanUserReorderColumns .    FrozenColumnCount$ 2��5}   G   ��5|   Q   �� .@�5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader  .��5�      �� "  6   .�5�      ��  � ��$ Center=�6<   5�      5�         	CellStyle 5�      �� 5�      *  $System.Windows.Controls.DataGridCell"  6   .�5�      ��  � ��$ 0 0 0 1q�68   �� 5�       � �� �#fff64    5�      P� 5�      	 �@ 
IsSelected
 . . �5�      ��  � ��$ 0 0 0 1q�6@   �� 5�       � �� �#72aee06;   5�      5�         RowStyle 5�      �� 5�      )  #System.Windows.Controls.DataGridRow"  6   .�5�      ��  � �� �#d5d6db64   �� 5�       � ��$ 0 0 0 1q�68   �� 5�       � ��$ Center=�6<    5�      P� 5�      	 �@ 
IsSelected
 . . �5�      ��  � ��$ 0 0 0 1q�6@   �� 5�       � �� �#72aee06<   5�      P� 5�      	 �@J�IsMouseOver
 . . �5�      ��  � �� �#72aee06<   5�      5�       � ContextMenu 5�      �� 5�      -   5        $��DataGridContextMenu��5�       �� 	StaysOpen . .@�5�      m� -   5        $��修改价格��5�      6-   5�         Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ 供应商姓名��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 64      
IsReadOnly . 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 6>   �� *ProviderName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�     5�      $ 是否划价��6%   $ * 63    .  6=   �� *InStockState+$ TwoWay=�$ PropertyChanged=�6�     5�      $ 进货日期��6%   $ * 63    .  6=   �� *InStockDate+$ TwoWay=�$ PropertyChanged=� �� StringFormat$ {0:yyyy-MM-dd}��6�   5�      %
   SCZY.OtherWindows.DataGridPager
  5�   
   -	   5          
 Name$  	gridPager��5�   %   $��Bottom=�68   $��20 0 0 0q�6P   $��Bottom=�6b   5�      5�      