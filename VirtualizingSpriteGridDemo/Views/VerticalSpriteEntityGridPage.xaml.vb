Imports VirtualizingSpriteGridDemo.ViewModels

Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class VerticalSpriteEntityGridPage
        Inherits Page
            property ViewModel as VerticalSpriteEntityGridViewModel = New VerticalSpriteEntityGridViewModel
        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
