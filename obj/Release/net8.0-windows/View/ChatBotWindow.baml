   M S B A M L   `   `   `  ����   	TesteImbd+    %MovieBookRecommendation.ChatBotWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  ChatBot de Filmes��$	��400��$	��600�� � 	Resources    RoundedButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding3  -System.Windows.Media.Effects.DropShadowEffect   Color	   
BlurRadius
   ShadowDepth��%Y  &	        �� " ��.���  � ��
 �������  � ��
 �   ���  � ��$
 Bold"���  � ��$ 5q���  � ��$ 0q���  � ��$ 35����  � ��$	 100����  � �� �� " ��.R��� $ 10��#��z��#��z��#��z��# z��._��� $��Center=�$��Center=���  �@J�Effect   $
 Gray��$	 10Q�$
 2Q�.��� $��10q� � RowDefinitions �� $
��Auto ��� $��* �.+���  �� Orientation$ 
Horizontal=�$��Center=�$��0,0,0,10q�.+��� -    ��Name$ 
BtnSinopse��$��Sinopse��$	��150��$��40��$��10q�#��[  �� -   $ BtnBuscarDescricao��$#��Buscar Filme pela descrição��$	��200��$��40��$��10q�#��[  � $��1��.+��� -    ��Name$ PanelSinopse��$y�	Collapsed=�.+��� $(��"Sinopse do último filme avaliado:��$
��Bold"�$��0,0,0,10q��� $
��Auto=�$	��200��.���� -    ��Name$ 
TxtSinopse��$
��Wrap=��� -   $ 
PanelBusca��$y�	Collapsed=�.+��� $$��Digite a descrição do filme:��$
��Bold"�$��0,0,0,10q��� -    ��Name$ TxtDescricao��$��30���� -   $��Buscar��$	��100��$��30��$��0,10,0,0q��� $
��Auto=�$	��200��$��0,10,0,0q�.���� -   $ TxtResultados��$
��Wrap=�