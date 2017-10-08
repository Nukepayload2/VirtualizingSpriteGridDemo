Imports VirtualizingSpriteGridDemo.ViewModels

Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class SpriteEntityGridPage
        Inherits Page
            property ViewModel as SpriteEntityGridViewModel = New SpriteEntityGridViewModel
        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
