Imports VirtualizingSpriteGridDemo.ViewModels

Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class HorizontalSpriteEntityGridPage
        Inherits Page
            property ViewModel as HorizontalSpriteEntityGridViewModel = New HorizontalSpriteEntityGridViewModel
        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
