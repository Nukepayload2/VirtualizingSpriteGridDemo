Imports VirtualizingSpriteGridDemo.ViewModels

Imports Windows.UI.Xaml.Controls

Namespace Views
    Public NotInheritable Partial Class PerformanceTestsPage
        Inherits Page
            property ViewModel as PerformanceTestsViewModel = New PerformanceTestsViewModel
        ' TODO WTS: Change the chart as appropriate to your app.
        ' For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
