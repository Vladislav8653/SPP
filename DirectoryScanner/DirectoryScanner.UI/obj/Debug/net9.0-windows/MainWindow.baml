   M S B A M L   `   `   `  ����N  JDirectoryScanner.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DirectoryScanner.UIDirectoryScanner.UI  5   	   M+clr-namespace:DirectoryScanner.UI.ViewModelDirectoryScanner.UI.ViewModel  5   	   $    DirectoryScanner.UI.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:DirectoryScanner.UI     5   	   A	viewModel+clr-namespace:DirectoryScanner.UI.ViewModel     5   	     � Title$  Directory Scanner��5	   	   $	��450��6#   $	��800��60    � Icon$ ../Assets/Folder.png��6<   
����ح� � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%�  '��       �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ 1q�60   �� 5       � ��$ 10q�6(   �� 5       � ��$	 150��6)   �� 5       � ��$ 20��6*   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 5��5   !   ._�5      �� $��Center=�6/   $��Center=�5   /   5      5      5       5!      5"      .��5$      �  � RowDefinitions 5%   
   �� 5&      $
��Auto �6   �� 5'      $��* �6   .+�5)   
   �� $��0��6   	 �� Orientation$	 
Horizontal=�6"   $��5q�6;   .+�5*      �� $��Select Directory��6   $��5q�6`   ��60   �� 
 �� Path$
 SelectDirectoryCommand�6`   �� 5+      $��Cancel scanning��6   $��5q�6V   ��6/   �� $
 CancelCommand�6V   5,      R� 5-   
   $��1��6   
���������6    �� $
 RootNode.Children� 5.       �� DataType:   4DirectoryScanner.UI.ViewModel.DirectoryNodeViewModel �� ItemsSource5   /DirectoryScanner.UI.ViewModel.FileNodeViewModel��%  (��       5/   +   *, +)(�� �    57      *, +)�� 5/      "  6+    6`   �� *
Children+.R�50      �� $	 
Horizontal=�6!   $��0 0 0 1q�6:   .+�51      �� $��./Assets/Folder.png��6    $��16��6=   $��16��6H   �� 52      $	��5,0q�6:   ��6$   �� *Name+6:   �� 53      $	��5,0q�6>   ��6$   �� *
SizeText+6>   �� 54      $	��5,0q�6D   ��6$   �� *PercentageText+6D   55      56      �� 57      "  6   .R�58      �� $	 
Horizontal=�6!   $��0 0 0 1q�6:   .+�59      �� $��./Assets/File.png��6    $��16��6;   $��16��6F   �� 5:      $	��5,0q�6:   ��6$   �� *Name+6:   �� 5;      $	��5,0q�6>   ��6$   �� *
SizeText+6>   �� 5<      $	��5,0q�6D   ��6$   �� *PercentageText+6D   5=      5>      5?      5@      5A      5B      