   M S B A M L   `   `   `  ���� D  @TesteImbd, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MovieBookRecommendationMovieBookRecommendation  O,clr-namespace:MovieBookRecommendation.ModelsMovieBookRecommendation.Models  W0clr-namespace:MovieBookRecommendation.Converters"MovieBookRecommendation.Converters  e aMicrosoft.Web.WebView2.Wpf, Version=1.0.1343.22, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf (    "MovieBookRecommendation.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     7local%clr-namespace:MovieBookRecommendation     ?models,clr-namespace:MovieBookRecommendation.Models     G
converters0clr-namespace:MovieBookRecommendation.Converters     \wv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf       � Title$  Recomendações��$	��700��$	��900��
���   � � 	Resources ;   5MovieBookRecommendation.Converters.StarColorConverter   StarColorConverter �� 
TargetType �� Property �� Value �� Padding �� CornerRadius  RoundedButton �� 
TargetType3  -System.Windows.Media.Effects.DropShadowEffect   Color	   
BlurRadius
   ShadowDepth��%�  &	        '��      &	 {       �� " ��.���  � �� �� .R��� 
�������$ 5q�$ 5��._��� 
���   �$
��Bold"�$��18(����� �� " ��.���  � ��
 �������  � ��
 �   ���  � ��$
 Bold"���  � ��$ 5q���  � ��$ 0q���  � ��$ 35����  � ��$	 100����  � �� �� " ��.R��� $ 10��#��z��#��z��#��z��# z��._��� $��Center=�$��Center=���  �@J�Effect   $
 Gray��$	 10Q�$
 2Q�.���� $
��Auto=�$��Disabled=�.��� 
���   � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��
�������$ 10q�._��  � ColumnDefinitions �� $��* ��� $
��Auto �.+���  �� Orientation$ 
Horizontal=�$��Center=�.+��� -    ��Name$ SearchTextBox��$	��300��$��25��$��5q��� -   $��Buscar��$��80��$��25��$��5q�$
��Bold"��� $ 
Horizontal=�$��1��$��Center=�.+��� -   $��Faceswap��$��5q�#��[ �� -   $��ChatBot��$��5q�#��[ �� -    ��Name$ LoginButton��$��Login��$��5q�#��[ �� -   $ SendPreferencesButton��$��Enviar Preferências��$��5q�#��[ $	��150��$y�	Collapsed=��� -    ��Name$ StatusTextBlock��$��1��$��10q�
�������$
��Bold"�$��18(��� $��2��$��10q�
���   �.@��� $��Filmes��.���� 
���   �.+��� $��Trailer��
�������$
��Bold"�$��18(�$��10q�)  #Microsoft.Web.WebView2.Wpf.WebView2  -     Name$ TrailerWebView2��$	��300��$	��500��$��10q�$��Center=��� $��Filmes��
�������$
��Bold"�$��18(�$��10q��� -	    ��Name$ MoviesItemsControl��
���   ����� .R�A�  A� Columns$ 8������  �� DataType*   $MovieBookRecommendation.Models.Movie"  .R��� 
�������$��1q�$��5q�$ 5q�$	��180��
�������._��� .+��� $	��200��$��Uniform=����� *Poster+�� $
��Bold"�$��0,5,0,0q�
�������$��16(����� *Title+�� $��Italic$�$��0,0,0,5q�
�������$��14(����� *Year+�� $��0,0,0,5q�
����ح�$��14(����� *Genre+�� $ 
Horizontal=�.+��� -
   $	��★��$��1��$��2q����� *Rating+ �� 	Converter# [   �� ConverterParameter$ 1��
 � Tag �� �� -   $	��★��$��2��$��2q����� *Rating+# [  $ 2�� �� �� -   $	��★��$��3��$��2q����� *Rating+# [  $ 3�� �� �� -   $	��★��$��4��$��2q����� *Rating+# [  $ 4�� �� �� -   $	��★��$��5��$��2q����� *Rating+# [  $ 5�� �� �� $��Recomendados��
�������$
��Bold"�$��18(�$��10q��� -   $! RecommendationsItemsControl��
���   ����� .R�A� $ 8������ .R��� 
�������$��1q�$��5q�$ 5q�$	��180��
�������._��� .+��� $	��200��$��Uniform=����� *Poster+�� $
��Bold"�$��0,5,0,0q�
�������$��16(����� *Title+�� $��Italic$�$��0,0,0,5q�
�������$��14(����� *Year+�� $��0,0,0,5q�
����ح�$��14(����� *Genre+�� $��Livros��.���� 
���   �.+��� $��Livros��
�������$
��Bold"�$��18(�$��10q��� -   $ BooksItemsControl��
���   ����� .R�A� $ 8������ )   #MovieBookRecommendation.Models.Book"  .R��� 
�������$��1q�$��5q�$ 5q�$	��180��
�������._��� .+��� $	��200��$��Uniform=����� *
ImageUrl+�� $
��Bold"�$��0,5,0,0q�
�������$��16(����� *Title+�� $��Italic$�$��0,0,0,5q�
�������$��14(����� *	Authors+�� $ 
Horizontal=�.+��� -   $	��★��$��1��$��2q����� *Rating+# [  $ 1�� �� �� -   $	��★��$��2��$��2q����� *Rating+# [  $ 2�� �� �� -   $	��★��$��3��$��2q����� *Rating+# [  $ 3�� �� �� -   $	��★��$��4��$��2q����� *Rating+# [  $ 4�� �� �� -   $	��★��$��5��$��2q����� *Rating+# [  $ 5�� �� �� $��Recomendados��
�������$
��Bold"�$��10q�$��18(��� -   $&  BooksRecommendationsItemsControl��
���   ����� .R�A� $ 8������ "  .R��� 
�������$��1q�$��5q�$ 5q�$	��180��
�������._��� .+��� $	��200��$��Uniform=����� *
ImageUrl+�� $
��Bold"�$��0,5,0,0q�
�������$��16(����� *Title+�� $��Italic$�$��0,0,0,5q�
�������$��14(����� *	Authors+