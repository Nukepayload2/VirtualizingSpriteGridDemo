﻿Imports System.Collections.ObjectModel

Imports VirtualizingSpriteGridDemo.Helpers
Imports VirtualizingSpriteGridDemo.Models
Imports VirtualizingSpriteGridDemo.Services

Namespace ViewModels
    Public Class PerformanceTestsViewModel
        Inherits Observable
        Public Sub New()
        End Sub

        Public ReadOnly Property Source() As ObservableCollection(Of DataPoint)
            Get
                ' TODO WTS: Replace this with your actual data
                Return SampleDataService.GetChartSampleData()
            End Get
        End Property
    End Class
End Namespace
