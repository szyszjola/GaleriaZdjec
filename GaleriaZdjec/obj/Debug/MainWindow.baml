   M S B A M L   `   `   `  ����G  CGaleriaZdjec, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:GaleriaZdjecGaleriaZdjec  5   	       GaleriaZdjec.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:GaleriaZdjec     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � WindowStyle$ ThreeDBorderWindow=�5	   	    � Title$ 
MainWindow��5   	   $	��800��6   $
��1100��6)   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $��3��6   ��5      �� 5       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5      �  � Color$ 	#FF647671��6#    � Offset$ 0Q�65   � 5      $ 	#FFB6F7E5��6#   $ 1Q�65   5      5      �� 5   
   $��3��6   $��3��6!   ��5       �� 5!      $ 0.5,1&�6&   $ 0.5,0&�67   .��5"      � $ 	#FF647671��6#   $ 0Q�65   � 5#      $ 	#FFB6F7E5��6#   $ 1Q�65   5$      5&      o� 5'   
   -   5        	 o�Name$	 	MenuGorne��5'      
������ $��3��6;   $��Center=�6O   $��Center=�6j   .@�5(      m� $
��Plik��6   $��20,10q�6%   
�������.@�5)      m� -   5        
 m�Name$
 	MenuDodaj��5)      ���	#FF6CB4C9$*��$Dodaj bieżący folder do ulubionych��6E   6r   m� 5*      -   5        $
 MenuUsun��5*      ���	#FF6CB4C9$��Usuń��6D   6R   m� 5+      -   5        $
 MenuZmienNazwe��5+      ���	#FF6CB4C9$��Zmień nazwę��6J   6_   �� 5,      m� 5-      -   5        $
 
MenuOdwiez��5-      ���	#FF6CB4C9$��	Odśwież��6F   6W   �� 5.      m� 5/      -   5        $
 
MenuKoniec��5/      ���	#FF6CB4C9$��Koniec��6F   6V   50      m� 51      -   5        $
 
MenuNapraw��51      $��Napraw��6+   $��20,10q�6<   
�������6_   m� 52      -	   5        $
 
MenuDrukuj��52      $��Drukuj��6+   $��20,10q�6<   
�������6_   m� 53      -
   5        $
 
MenuEdycja��53      $��Edycja��6+   $��20,10q�6<   
�������6_   54      R� 55   
   -   5         R�Name$ TreeViewFoldery��55      ���	#FFBCD9F1$��1��6D   $��0,0,1,0q�6Q   .@�56      Q� -   5         Q�Name$ UlubioneFolderyItems��56      $��Ulubione��69    Q� 
IsExpanded . 6K   Q� 57      -   5        $ FolderyItems��57      $��Foldery��61   58      �� 59   
   $��1��6   $��1��6$   $��2��64   
������  �� ResizeBehavior$ PreviousAndNext=�6W   �� 5:   
   -   5         ��Name$ 
ObrazkiBox��5:      $��1��6U   $��2��6b   $��Disabled=�6r   ��5;      �� 5<      .R�5=      � 5>      5?      5A      5B      5C      