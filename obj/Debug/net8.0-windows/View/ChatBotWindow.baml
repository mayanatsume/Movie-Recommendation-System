   M S B A M L   `   `   `  ����  	TesteImbd+    %MovieBookRecommendation.ChatBotWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  ChatBot de Filmes��5   	   $	��400��6#   $	��600��60    � 	Resources 5         RoundedButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding3  -System.Windows.Media.Effects.DropShadowEffect   Color	   
BlurRadius
   ShadowDepth��%�  &	        �� 5   
   " ��6&   .�5      ��  � ��
 �����6+   �� 5	       � ��
 �   �6+   �� 5
       � ��$
 Bold"�6+   �� 5       � ��$ 5q�6(   �� 5       � ��$ 0q�60   �� 5       � ��$ 35��6'   �� 5       � ��$	 100��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 10��6!   #��z��63   #��z��5   !   #��z��5   !   # z��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       �@J�Effect 5        5      $
 Gray��6'   $	 10Q�64   $
 2Q�6D   5      5       5!      .��5"      � $��10q�6    � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $��* �6   .+�5)   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��0,0,0,10q�6K   .+�5*      �� -   5         ��Name$ 
BtnSinopse��5*      $��Sinopse��6)   $	��150��6;   $��40��6G   $��10q�6S   #��[  5+   .   �� 5,      -   5        $ BtnBuscarDescricao��5,      $#��Buscar Filme pela descrição��61   $	��200��6W   $��40��6c   $��10q�6o   #��[  5-   6   5.      � 51   
   $��1��6   .+�53      �� -   5         ��Name$ PanelSinopse��53      $y�	Collapsed=�6/   .+�54      �� $(��"Sinopse do último filme avaliado:��6   $
��Bold"�6E   $��0,0,0,10q�6W   �� 55      $
��Auto=�6   $	��200��6B   .��56      �� -   5         ��Name$ 
TxtSinopse��56       $
��Wrap=�64   57      58      �� 5;      -   5        $ 
PanelBusca��5;      $y�	Collapsed=�6-   .+�5<      �� $$��Digite a descrição do filme:��6   $
��Bold"�6@   $��0,0,0,10q�6R   �� 5=      -   5         ��Name$ TxtDescricao��5=      $��30��60   �� 5>      -   5        $��Buscar��5>      $	��100��6*   $��30��66   $��0,10,0,0q�6B   6T   �� 5?      $
��Auto=�6   $	��200��6B   $��0,10,0,0q�6O   .��5@      �� -   5        $ TxtResultados��5@       $
��Wrap=�67   5A      5B      5C      5D      5E      