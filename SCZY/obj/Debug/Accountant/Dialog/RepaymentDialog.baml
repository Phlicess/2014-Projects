   M S B A M L   `   `   `  ����  SCZY,    &SCZY.Accountant.Dialog.RepaymentDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  还款管理��5   	   $	��386��6   $	��566��6#    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterOwner=�5   	      DialogWinTemplate#���   6-    � WindowStyle$
 None=�5   	    � ShowInTaskbar .   � AllowsTransparency . 
������  � 	Resources 5       �� 
TargetType  TextBlockStyle �� Property	 �� Value  TextBoxStyle  headerStyle
 �� 
TargetType �� Padding �� Triggers P� Property P� Value �� 
TargetName  ListViewItemStyle �� Triggers��%�  &	       &	 �     &	 �    &	 �    �� 5	   
   " ��6   .�5
      ��  � ��$	 Center=�64   �� 5       � ��$	 Center=�62   �� 5       � ��$	 80��6&   �� 5       � ��$	 14(�6)   �� 5       � ��	 �#5556+   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 24��6'   �� 5       � ��$		 140��6&   �� 5       � ��$	 15(�6)   5      �� 5   
   " ��6$   .�5      ��  � ��	 �#5556+   �� 5       � ��$	 10q�60   �� 5       � ��	 �#aaa6,   �� 5       � ��	 �#aaa6+   �� 5       � ��	 5      �� 5      "
 ��6&   .R�5       � $��g��6   .+�5!      �� $��bd��6%   # z��6/   ._�5"   "   �� $��Center=�64   5#      n� 5$      $��1��6$   ���#d5d6db$��Right=�6D   $��0,0,-1,0q�6`   5%       5&      P� 5'       �@J�IsMouseOver . . �5(   "   �� $ g��6)    � ��	 �#d5d6db6N   5)      5+      5-      5/      �� 52   
   " ��6   .�53      ��  � ��$	 Center=�6;   �� 54       � ��$	 0q�60    55      P� 56       �@��
IsSelected . . �57      ��  � ��	 �#d5d6db63   �� 58       � ��$	 0q�68   59      P� 5:       �@J�IsMouseOver . . �5;      ��  � ��	 �#d5d6db63   �� 5<       � ��$	 0q�68   5=      5?      5@      .��5A      � -   5         �Name$ MainGrid��5A      $��1��6   
�������$��5 5 10 0q�6=    � RowDefinitions 5B   
   �� 5C      $
��8.5* �6   �� 5D      $
��1.5* �6   .+�5F   
   �� ���#c7c7c7$��1q�6'   $��5 5 5 -5q�6;   ._�5G      �� $��3 -3 3 0q�6   $��货款信息��6)   $
��Bold"�67   ���#555.��5H      ��  �� Orientation$ Vertical=�6   $��0 5 0 5q�64   .+�5I      �� $ 
Horizontal=�6!   $��35��6:   .+�5J      �� -   5        $��TextPaid��5J   $   #��[ 64   $��25 0 0 0q�6\   还款金额:6u   �� 5K      -   5        $
��Paid��5K   "   #��[ 6.   �� 5L      -   5        $��
TextUrgent��5L   $   #��[ 66   $��50 0 0 0q�6^   剩余欠款:6w   �� 5M      -   5        $��Leaving��5M   "   #��[ 61   {�.  6W   5N      �� 5O      $ 
Horizontal=�6!   $��35��6:   .+�5P      �� -   5        $��	TextTotal��5P   $   #��[ 65   $��25 0 0 0q�6]   $��   总金额:��6o   {�.  6�   �� 5Q      -   5        $��Total��5Q   "   #��[ 6/    �� 	MaxLength$ 18��6U   {�.  6x   �� 5R      -   5        $��TextConsumerName��5R   $   #��[ 6<   $��50 0 0 0q�6d   $��客户名称:��6v   {�.  6�   �� 5S      -	   5        $��ConsumerName��5S   "   #��[ 66   $ 20��6\   {�.  6   5T      �� 5U      $ 
Horizontal=�6%   $��35��6>   .+�5V      �� -
   5        $��	TextPhone��5V   $   #��[ 65   $��25 0 0 0q�6]   手机号码:6v   �� 5X      -   5        $��Phone��5X   "   #��[ 6/   $ 18��6U   {�.  6x   �� 5Y      -   5        $��TextTel��5Y   $   #��[ 63   $��50 0 0 0q�6\   公司电话:6u   �� 5[      -   5        $	��Tel��5[   "   #��[ 6-   $ 18��6S   {�.  6v   5\      �� 5]      $ 
Horizontal=�6!   $��35��6:   .+�5^      �� -   5        $��TextAddress��5^   $   #��[ 67   $��25 0 0 0q�6_   所在地址:6x   �� 5`      -   5        $��Address��5`   "   #��[ 61   $ 18��6W   {�.  6z   �� 5a      -   5        $��TextCompany��5a   $   #��[ 67   $��50 0 0 0q�6_   公司名称:6x   �� 5c      -   5        $��Company��5c   "   #��[ 61   $ 18��6W   {�.  6z   5d      �� 5e      .+�5f      �� -   5        $��Remark��5f   "   $	��150��60   $��5 5 5 5q�6=    �� TextWrapping$
 Wrap=�6N   6d   5g      5�      5�      5�      �� 5�   
   -   5        $��BtnStackPanel��5�      $��1��6*   $��Right=�67   $��Center=�6S   $ 
Horizontal=�6n   $��20 0 0 0q�6�   .+�5�      �� -   5        $��
SaveButton��5�      $��60��6'   $��25��62   $��0 0 20 0q�6>   .��6i   保存6m   �� 5�      -   5        $��CancelButton��5�      $��60��6)   $��25��64   $��0 0 20 0q�6@   .��6k   取消6o   5�      5�      5�      