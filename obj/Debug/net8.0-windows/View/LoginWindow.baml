   M S B A M L   `   `   `  ����  	TesteImbd)    #MovieBookRecommendation.LoginWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Login��5   	   $	��250��6   $	��400��6$    � 	Resources 5         RoundedLoginButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding3  -System.Windows.Media.Effects.DropShadowEffect   Color	   
BlurRadius
   ShadowDepth��%�  &	        �� 5   
   " ��6+   .�5      ��  � ��
 �����6+   �� 5	       � ��
 �   �6+   �� 5
       � ��$
 Bold"�6+   �� 5       � ��$ 10q�6(   �� 5       � ��$ 0q�60   �� 5       � ��$ 35��6'   �� 5       � ��$	 120��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 10��6!   #��z��63   #��z��5   !   #��z��5   !   # z��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       �@J�Effect 5        5      $
 Gray��6'   $	 10Q�64   $
 2Q�6D   5      5       5!      .��5#      � $��10q�6    � RowDefinitions 5$   
   �� 5%      $
��Auto �6   �� 5&      $
��Auto �6   �� 5'      $
��Auto �6   .+�5)   
   �� $��0��6    �� Orientation$ Vertical=�6"   $��0,0,0,10q�69   .+�5*      �� $��Email:��6   $
��Bold"�6&   �� 5+      -   5         ��Name$ EmailTextBox��5+      $	��300��6,   $��30��68   $
��Bold"�6D   5,      �� 5-   
   $��1��6   $ Vertical=�6"   $��0,0,0,10q�69   .+�5.      �� $��Senha:��6   $
��Bold"�6&   G� 5/      -   5         G�Name$ PasswordBox��5/      $	��300��6/   $��30��6;   $
��Bold"�6G   50      �� 51   
   -   5         ��Name$ LoginButton��51      $��2��6&   $��Login��63   #��[  6C   $
��Left=�6o   6�   52      53      