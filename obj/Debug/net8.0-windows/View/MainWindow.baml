   M S B A M L   `   `   `  ����D  @TesteImbd, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MovieBookRecommendationMovieBookRecommendation  5   	   O,clr-namespace:MovieBookRecommendation.ModelsMovieBookRecommendation.Models  5   	   W0clr-namespace:MovieBookRecommendation.Converters"MovieBookRecommendation.Converters  5   	   e aMicrosoft.Web.WebView2.Wpf, Version=1.0.1343.22, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf 5   	   (    "MovieBookRecommendation.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   7local%clr-namespace:MovieBookRecommendation     5   	   ?models,clr-namespace:MovieBookRecommendation.Models     5   	   G
converters0clr-namespace:MovieBookRecommendation.Converters     5   	   \wv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf     5   	     � Title$  Recomendações��5   	   $	��700��6   $	��900��6,   
���   � � 	Resources 5      ;   5MovieBookRecommendation.Converters.StarColorConverter   StarColorConverter �� 
TargetType �� Property �� Value �� Padding �� CornerRadius  RoundedButton �� 
TargetType3  -System.Windows.Media.Effects.DropShadowEffect   Color	   
BlurRadius
   ShadowDepth��%�  &	        '��      &	       5   
   6(   �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      .R�5      �� 
�������$ 5q�64   $ 5��6@   ._�5      �� 
���   �$
��Bold"�6L   $��18(�6^   ��6(   �� 6^   5      5      5      5      �� 5   
   " ��6&   .�5      ��  � ��
 �����6+   �� 5       � ��
 �   �6+   �� 5        � ��$
 Bold"�6+   �� 5!       � ��$ 5q�6(   �� 5"       � ��$ 0q�60   �� 5#       � ��$ 35��6'   �� 5$       � ��$	 100��6&   �� 5%       � �� 5&      �� 5'      " ��6&   .R�5(      �� $ 10��6!   #��z��63   #��z��5)   !   #��z��5*   !   # z��5+   !   ._�5,      �� $��Center=�6/   $��Center=�6L   5-      5.      50      �� 51       �@J�Effect 52        53      $
 Gray��6'   $	 10Q�64   $
 2Q�6D   55      56      57      .��5:      �� $
��Auto=�6   $��Disabled=�66   .��5;   
   � 
���   � � RowDefinitions 5<      �� 5=      $
��Auto �6    �� 5>      $
��Auto �6    �� 5?      $��* �6    .+�5C      �� $��0��6   
�������$ 10q�64   ._�5D      �  � ColumnDefinitions 5E      �� 5F      $��* �6+   �� 5G      $
��Auto �6+   .+�5K      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   .+�5L      �� -   5         ��Name$ SearchTextBox��5L   "   $	��300��69   $��25��6E   $��5q�6Q   �� 5M      -   5        $��Buscar��5M   !   $��80��62   $��25��6=   $��5q�6I   $
��Bold"�6o   5N      �� 5Q      $ 
Horizontal=�6!   $��1��6:   $��Center=�6J   .+�5S      �� -   5        $��Faceswap��5S   !   $��5q�64   #��[ 6?   6f   �� 5V      -   5        $��ChatBot��5V   !   $��5q�63   #��[ 6>   6e   �� 5Y      -   5         ��Name$ LoginButton��5Y   !   $��Login��5Z   !   $��5q�5[   !   #��[ 5\   !   5]   !   �� 5`      -   5        $ SendPreferencesButton��5`   !   $��Enviar Preferências��6@   $��5q�6^   #��[ 5a   !   $	��150��6H   $y�	Collapsed=�5b   E   5c      5d      5e      �� 5h      -   5         ��Name$ StatusTextBlock��5h      $��1��61   $��10q�6>   
�������$
��Bold"�6]   $��18(�6o   �� 5k      $��2��6   $��10q�6&   
���   �.@�5n      �� $��Filmes��6   .��5o      �� 
���   �.+�5q      �� $��Trailer��6$   
�������$
��Bold"�6F   $��18(�6X   $��10q�6f   )  #Microsoft.Web.WebView2.Wpf.WebView2  5r      -   5          Name$ TrailerWebView2��5r   '   $	��300��6@   $	��500��6M   $��10q�6Y   $��Center=�6e   �� 5u      $��Filmes��6$   
�������$
��Bold"�6E   $��18(�6W   $��10q�6e   �� 5v      -	   5         ��Name$ MoviesItemsControl��5v   '   
���   ���5w      �� 5x   "   .R�5y   &   A�  A� Columns$ 8��62   5z   #   ��5|      �� 5}   "    �� DataType*   $MovieBookRecommendation.Models.Movie"  6/   .R�5~   &   �� 
�������$��1q�6@   $��5q�6T   $ 5q�6_   $	��180��6k   
�������._�5   *   �� .+�5�   .   �� $	��200��6N   $��Uniform=�6[   ��64   �� *Poster+6[   �� 5�   .   $
��Bold"�6O   $��0,5,0,0q�6a   
�������$��16(�6�   ��68   �� *Title+6�   �� 5�   .   $��Italic$�6N   $��0,0,0,5q�6a   
�������$��14(�6�   ��68   �� *Year+6�   �� 5�   .   $��0,0,0,5q�6O   
����ح�$��14(�6w   ��68   �� *Genre+6w   �� 5�   .   $ 
Horizontal=�69   .+�5�   2   �� -
   5        $	��★��5�   9   $��1��5�   b   $��2q�6w   ��5�   E   �� *Rating+ �� 	Converter# [   �� ConverterParameter$ 1��
 � Tag 5�   R   �� 6w   �� 5�   2   -   5        $	��★��5�   9   $��2��5�   b   $��2q�6w   ��5�   E   �� *Rating+# [  $ 2�� 5�   R   �� 6w   �� 5�   2   -   5        $	��★��5�   9   $��3��5�   b   $��2q�6w   ��5�   E   �� *Rating+# [  $ 3�� 5�   R   �� 6w   �� 5�   2   -   5        $	��★��5�   9   $��4��5�   b   $��2q�6w   ��5�   E   �� *Rating+# [  $ 4�� 5�   R   �� 6w   �� 5�   2   -   5        $	��★��5�   9   $��5��5�   b   $��2q�6w   ��5�   E   �� *Rating+# [  $ 5�� 5�   R   �� 6w   5�   /   5�   +   5�   '   5�   #   5�      �� 5�      $��Recomendados��6$   
�������$
��Bold"�6K   $��18(�6]   $��10q�6k   �� 5�      -   5        $! RecommendationsItemsControl��5�   '   
���   ���5�      �� 5�   "   .R�5�   &   A� $ 8��62   5�   #   ��5�      �� 5�   "   .R�5�   &   �� 
�������$��1q�6@   $��5q�6T   $ 5q�6_   $	��180��6k   
�������._�5�   *   �� .+�5�   .   �� $	��200��6N   $��Uniform=�6[   ��64   �� *Poster+6[   �� 5�   .   $
��Bold"�6O   $��0,5,0,0q�6a   
�������$��16(�6�   ��68   �� *Title+6�   �� 5�   .   $��Italic$�6N   $��0,0,0,5q�6a   
�������$��14(�6�   ��68   �� *Year+6�   �� 5�   .   $��0,0,0,5q�6O   
����ح�$��14(�6w   ��68   �� *Genre+6w   5�   +   5�   '   5�   #   5�      5�      5�      �� 5�      $��Livros��6   .��5�      �� 
���   �.+�5�      �� $��Livros��6$   
�������$
��Bold"�6E   $��18(�6W   $��10q�6e   �� 5�      -   5        $ BooksItemsControl��5�   '   
���   ���5�      �� 5�   "   .R�5�   &   A� $ 8��62   5�   #   ��5�      �� 5�   "   )   #MovieBookRecommendation.Models.Book"  6/   .R�5�   &   �� 
�������$��1q�6@   $��5q�6T   $ 5q�6_   $	��180��6k   
�������._�5�   *   �� .+�5�   .   �� $	��200��6P   $��Uniform=�6]   ��64   �� *
ImageUrl+6]   �� 5�   .   $
��Bold"�6O   $��0,5,0,0q�6a   
�������$��16(�6�   ��68   �� *Title+6�   �� 5�   .   $��Italic$�6Q   $��0,0,0,5q�6d   
�������$��14(�6�   ��68   �� *	Authors+6�   �� 5�   .   $ 
Horizontal=�69   .+�5�   2   �� -   5        $	��★��5�   9   $��1��5�   f   $��2q�6{   ��5�   E   �� *Rating+# [  $ 1�� 5�   V   �� 6{   �� 5�   2   -   5        $	��★��5�   9   $��2��5�   f   $��2q�6{   ��5�   E   �� *Rating+# [  $ 2�� 5�   V   �� 6{   �� 5�   2   -   5        $	��★��5�   9   $��3��5�   f   $��2q�6{   ��5�   E   �� *Rating+# [  $ 3�� 5�   V   �� 6{   �� 5�   2   -   5        $	��★��5�   9   $��4��5�   f   $��2q�6{   ��5�   E   �� *Rating+# [  $ 4�� 5�   V   �� 6{   �� 5�   2   -   5        $	��★��5�   9   $��5��5�   f   $��2q�6{   ��5�   E   �� *Rating+# [  $ 5�� 5�   V   �� 6{   5�   /   5�   +   5�   '   5�   #   5�      �� 5�      $��Recomendados��6$   
�������$
��Bold"�6K   $��10q�6]   $��18(�6i   �� 5�      -   5        $&  BooksRecommendationsItemsControl��5�   '   
���   ���5�      �� 5�   "   .R�5�   &   A� $ 8��62   5�   #   ��5�      �� 5�   "   "  6/   .R�5�   &   �� 
�������$��1q�6@   $��5q�6T   $ 5q�6_   $	��180��6k   
�������._�5�   *   �� .+�5�   .   �� $	��200��6P   $��Uniform=�6]   ��64   �� *
ImageUrl+6]   �� 5�   .   $
��Bold"�6O   $��0,5,0,0q�6a   
�������$��16(�6�   ��68   �� *Title+6�   �� 5�   .   $��Italic$�6Q   $��0,0,0,5q�6d   
�������$��14(�6�   ��68   �� *	Authors+6�   5�   +   5�   '   5�   #   5�      5�      5�      5�      5�      5�      5�      