   M S B A M L   `   `   `  ����?  ;SCZY, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SCZY.CommonSCZY.Common  5      5clr-namespace:SCZY.OtherWindowsSCZY.OtherWindows  5          SCZY.Admin.UserManagePage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,commonclr-namespace:SCZY.Common     5      8otherWindowsclr-namespace:SCZY.OtherWindows     5        N� Title$  用户管理��5	       � 	Resources 5       �� MergedDictionaries �� Source   SCZY.Common.DateConverter   DataConverter�� 5   
    5      �� 5      $  ../Style/MainWinStyle.xaml?�6%   �� 5      $! ../Style/MainPageStyle.xaml?�6%   %   &	          5      6#   5      .��5      ]� .+�5   
   �� $
��Auto��6   $��30��6"   $��	0 -30 0 0q�6.   $	��Top=�6A    �� Orientation$ 
Horizontal=�6V   $��Right=�6o   .+�5      �� -   5        $��
SeacherBox��5      $��Center=�6C   $	��140��6e   $��20��6q   �� 5      -   5        $��SeacherBtn_Mo��5      $��30��6)   $��30��65   $��../Images/Seacher_Mo.png��6@   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property �@J�Opacity �� Value$ 1Q�64   �� 5       � ��$ Center=�6>   �� 5       � ��$ Right=�6@    �� Triggers 5      P� 5       	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �5!   "   ��  �@�Cursor$
 Hand��6;   �� 5"   "    �@J�Opacity$ 0Q�6<   5#      5%      5'      �� 5(      -   5        $��
SeacherBtn��5(       � ToolTip$ 查找��6&   $��30��6f   $��30��6r   $��	-30 0 0 0q�6}   $��../Images/Seacher.png��6�   ��5)      �� 5*      " ��6   .�5+      ��  �@J�Opacity$ 0Q�64   �� 5,       � ��$ Right=�6@   �� 5-       � ��$ Center=�6>    5.      P� 5/      	 �@J�IsMouseOver
 . . �50   "   ��  �@�Cursor$
 Hand��6;   �� 51   "    �@J�Opacity$ 1Q�6<   52      54      56      �� 58      -   5        $��ResetBtn_Mo��58      $��30��6'   $��30��63   $��../Images/Reset.png��6>   ��59      �� 5:      " ��6   .�5;      ��  �@J�Opacity$ 1Q�64   �� 5<       � ��$ Center=�6>   �� 5=       � ��$ Right=�6@    5>      P� 5?      	 �@J�IsMouseOver
 . . �5@   "   ��  �@�Cursor$
 Hand��6;   �� 5A   "    �@J�Opacity$ 0Q�6<   5B      5D      5F      �� 5G      -   5        $��ResetBtn��5G      $ 重置��6$   $��30��6b   $��30��6n   $��
-30 0 15 0q�6y   $��../Images/Reset_Mo.png��6�   ��5H      �� 5I      " ��6   .�5J      ��  �@J�Opacity$ 0Q�64   �� 5K       � ��$ Right=�6@   �� 5L       � ��$ Center=�6>    5M      P� 5N      	 �@J�IsMouseOver
 . . �5O   "   ��  �@�Cursor$
 Hand��6;   �� 5P   "    �@J�Opacity$ 1Q�6<   5Q      5S      5U      5V      &   System.Windows.Controls.DataGrid  5X   
   -   5          Name$ PendingUser��5X      $��0q�6(   $	��Top=�6<   
�������   SelectionMode$ Single=�5Y         AutoGenerateColumns .     RowHeaderWidth$ 0Q�6F      GridLinesVisibility$
 None=�5Z      ���#868686$��13(�6C      CanUserAddRows .     CanUserReorderColumns .    FrozenColumnCount$ 2��5[   G   ��5Z   Q   �� .@�5\      =  7System.Windows.Controls.Primitives.DataGridColumnHeader  .��5]      �� "  6   .�5^      ��  � ��$ Center=�6<   5_      5`         	CellStyle 5b      �� 5c      *  $System.Windows.Controls.DataGridCell"  6   .�5d      ��  � ��$ 0 0 0 1q�68   �� 5e       � �� �#fff64    5f      P� 5g      	 �@ 
IsSelected
 . . �5h      ��  � ��$ 0 0 0 1q�6@   �� 5i       � �� �#72aee06;   5j      5l         RowStyle 5n      �� 5o      )  #System.Windows.Controls.DataGridRow"  6   .�5p      ��  � �� �#d5d6db64   �� 5q       � ��$ 0 0 0 1q�68   �� 5r       � ��$ Center=�6<    5s      P� 5t      	 �@ 
IsSelected
 . . �5u      ��  � ��$ 0 0 0 1q�6@   �� 5v       � �� �#72aee06<   5w      P� 5x      	 �@J�IsMouseOver
 . . �5y      ��  � �� �#72aee06<   5z      5|       � ContextMenu 5~      �� 5      -   5        $��DataGridContextMenu��5      
������� �� 	StaysOpen . .@�5�      m� -   5        $��AddItem��5�   -   $��添加用户��6   6<   m� 5�      -	   5        $��EdiItem��5�   -   $��修改用户��6   6<   m� 5�      -
   5        $��DelItem��5�   -   $��删除用户��6   6<   5�         Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ 用户编号��6%      
Visibility$ Hidden=�63      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6G   �� *ID+6^     5�      $ 姓名��6%      Width5	  /System.Windows.Controls.DataGridLengthConverter$ *	 61    .  6;   �� *Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�     5�      $ 	用户名��6%   $ *	 62    .  6<   �� *
UserName+$ TwoWay=�$ PropertyChanged=�6�     5�      $ 	手机号��6%   $ *	 62    .  6<   �� *Phone+$ TwoWay=�$ PropertyChanged=�6�     5�      $ 角色��6%   $ *	 61    .  6;   �� *Role+$ TwoWay=�$ PropertyChanged=�6�     5�      $ 备注��6%   $ 2*	 61    .  6<   �� *Remark+$ TwoWay=�$ PropertyChanged=�6�   5�      %
   SCZY.OtherWindows.DataGridPager
  5�   
   -   5          
 Name$  	gridPager��5�   %   $��Bottom=�68   $��20 0 0 0q�6P   $��Bottom=�6b   5�      5�      