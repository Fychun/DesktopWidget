   M S B A M L   `   `   `  ����H  DDesktopWidget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:DesktopWidgetDesktopWidget  5   	       DesktopWidget.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:DesktopWidget     5   	   -   5          � Title$  桌面小部件��5   	   $	��400��6   $	��600��6$    � WindowStyle$
 None=�5	   	    � AllowsTransparency$
 True��6   
������ $	��200��5   	   $	��150��6    � 	Resources 5         WidgetContextMenu  ResizeThumbStyle �� 
TargetType �� Property �� Value �� 
TargetType��%�  &	        &	 �    �� 5   
   .@�5      m� -   5        $��添加背景图片��5      6'   m� 5      -   5        $��更换图片��5      6%   m� 5      -   5        $��设置切换间隔��5      6'   m� 5      -   5        $��设置透明度��5      6&   m� 5      -   5        $��重置窗口大小��5      6'   m� 5      -   5        $��删除当前图片��5      6'   m� 5      -   5        $��退出��5      6#   5      �� 5   
   " n�6)   .�5      ��  � �� 5      �� 5      " n�6&   .R�5      �� 
������ 6$   5      5      5       5!      .��5#      � .+�5%   
   �� -	   5         ��Name$ BackgroundImage��5%      $
��Fill=�6)   68   �� 5(   
   $��Right=�6   $	��Top=�61   $��10q�6I   .+�5)      �� -
   5        	 ��Name$	 CloseButton��5)      $��X��6*   $��20��66   $��20��6A   
������ $��0q�5*   .   
�������
 J� Opacity$	
 0.5Q�5+   B   5,      n� 5/   
   -   5         n�Name$ 
ThumbRight��5/      #��[ 6$   $��5��6N   $��Right=�50      $��Stretch=�6,    � Cursor$ SizeWE��6H   51      n� 52   
   -   5        $ ThumbBottom��52      #��[ 6%   $��5��6O   $��Stretch=�53      $��Bottom=�6.   $ SizeNS��6I   54      n� 55   
   -   5        $ ThumbBottomRight��55      #��[ 6*   $��10��6T   $��10��6_   $��Right=�56      $��Bottom=�6,   $ SizeNWSE��6G   57      58      59      