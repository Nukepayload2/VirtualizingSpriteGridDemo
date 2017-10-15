''' <summary>
''' 限定高度但是不限定宽度的稀疏方块网格。
''' </summary>
Public Class HorizontalTileStorage

    Sub New(height As Integer)
        If height < 1 Then
            Throw New ArgumentOutOfRangeException(NameOf(height))
        End If
        ReDim _rows(height - 1)
        For i = 0 To _rows.Length - 1
            _rows(i) = New List(Of TileList)
        Next
    End Sub

    Private _rows As List(Of TileList)()

    ''' <summary>
    ''' 获取一个范围内非空气的方块的稀疏表示（从 0 开始）。
    ''' </summary>
    ''' <param name="fromX">从 X 坐标</param>
    ''' <param name="fromY">从 Y 坐标</param>
    ''' <param name="toX">到 X 坐标</param>
    ''' <param name="toY">到 Y 坐标</param>
    ''' <value>所有非空气方块</value>
    Public ReadOnly Iterator Property NonAirItems(fromX As Integer, fromY As Integer, toX As Integer, toY As Integer) As IEnumerable(Of (Tile As Tile, Row As Integer, Column As Integer))
        Get
            Debug.Assert(fromX >= 0, "起始位置 x 必须大于等于 0.")
            Debug.Assert(fromY >= 0, "起始位置 y 必须大于等于 0.")
            Debug.Assert(fromX <= toX, "起始位置 x 必须大于等于结束位置 x.")
            Debug.Assert(fromY <= toY, "起始位置 y 必须大于等于结束位置 y.")
            Debug.Assert(toY < _rows.Length, "结束位置 x 必须小于总行数.")
            For r = fromY To toY
                Dim curRow = _rows(r)
                For i = 0 To curRow.Count - 1
                    Dim curTileList = curRow(i)
                    Dim startIndex = curTileList.StartIndex
                    Dim endIndex = startIndex + curTileList.RepeatCount - 1
                    If startIndex <= fromX Then
                        If endIndex >= fromX Then
                            ' ----[----  ]
                            '  ---[------]
                            '   --[------]--
                            '     [------]--
                            Dim lBound = fromX, uBound = Math.Min(toX, endIndex)
                            For c = lBound To uBound
                                Yield (curTileList.TileData, r, c)
                            Next
                        End If
                    ElseIf startIndex <= toX Then
                        ' [  ----]---- 
                        ' [  ----] 
                        ' [  --  ]
                        Dim lBound = startIndex, uBound = Math.Min(toX, endIndex)
                        For c = lBound To uBound
                            Yield (curTileList.TileData, r, c)
                        Next
                    End If
                Next
            Next
        End Get
    End Property

    ''' <summary>
    ''' 将指定行的一串方块合并。
    ''' </summary>
    ''' <param name="row"></param>
    ''' <param name="tiles"></param>
    Public Sub MergeTileList(row As Integer, tiles As TileList)
        Debug.Assert(row >= 0, "起始行号必须大于等于 0.")
        Debug.Assert(row < _rows.Length, "起始行号必须小于行数 (高度).")
        MergeTileList(_rows(row), tiles)
    End Sub

    ''' <summary>
    ''' 将指定行的一串方块合并。
    ''' </summary>
    ''' <param name="curRow"></param>
    ''' <param name="tiles"></param>
    Public Sub MergeTileList(curRow As List(Of TileList), tiles As TileList)
        EraseTiles(curRow, tiles.StartIndex, tiles.RepeatCount)
        If Not tiles.TileData.IsAir Then
            ' 判断左侧是否有一样的方块。如果是，合并。否则添加新的 TileList。
            ' TBD: 删除优化: 如果正好有一个被删除的方块，可以重用原来的信息。
            For i = 0 To curRow.Count - 1
                Dim tl = curRow(i)
                If tl.StartIndex + tl.RepeatCount = tiles.StartIndex Then
                    If tiles.TileData.Kind = tl.TileData.Kind Then
                        tl.RepeatCount += tiles.RepeatCount
                    Else
                        curRow.Insert(i + 1, tiles)
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' 将指定行的一个区域中的所有方块消除。
    ''' </summary>
    Public Sub EraseTiles(curRow As List(Of TileList), start As Integer, length As Integer)
        For i = curRow.Count - 1 To 0 Step -1
            ' 尾部相交: 缩短 RepeatCount
            ' 头部相交: 更改 StartIndex, 缩短 RepeatCount
            ' 包含: 删除
            Dim tl = curRow(i)
            If tl.StartIndex >= start Then
                If tl.StartIndex + tl.RepeatCount <= start + length Then
                    ' 包含
                    curRow.RemoveAt(i)
                Else
                    ' 头部相交
                    tl.RepeatCount -= start - tl.StartIndex
                    tl.StartIndex = start
                End If
            Else
                If tl.StartIndex + tl.RepeatCount - 1 >= start Then
                    ' 尾部相交
                    tl.RepeatCount -= tl.StartIndex + tl.RepeatCount - start
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 获取或设定指定位置的单个方块。此属性不宜在循环中使用。
    ''' </summary>
    ''' <param name="fromX">从 X 坐标</param>
    ''' <param name="fromY">从 Y 坐标</param>
    ''' <param name="toX">到 X 坐标</param>
    ''' <param name="toY">到 Y 坐标</param>
    ''' <value>所有方块</value>
    Public Property Item(X As Integer, Y As Integer) As Tile
        Get
            Return NonAirItems(X, Y, X, Y).SingleOrDefault.Tile
        End Get
        Set(value As Tile)
            MergeTileList(Y, New TileList With {.StartIndex = X, .RepeatCount = 1, .TileData = value, .Collier = Tile.CollierFactory(value.Kind)()})
        End Set
    End Property

    Public ReadOnly Property Height As Integer
        Get
            Return _rows.Length
        End Get
    End Property
End Class
