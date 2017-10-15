Public Structure Tile
    ''' <summary>
    ''' 这个方块的类型的编号 (主和次都是 0 表示空气, 其余的需要自定义)。
    ''' </summary>
    Public Kind As TileKind

    Public Shared CollierFactory As New Dictionary(Of TileKind, Func(Of IRectangleCollier))

    Public Shared ReadOnly Air As New Tile

    Public ReadOnly Property IsAir As Boolean
        Get
            Return Kind.Primary = 0 AndAlso Kind.Secondary = 0
        End Get
    End Property
End Structure
