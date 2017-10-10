Imports Nukepayload2.N2Engine.Prototypes.UI

''' <summary>
''' 限定高度但是不限定宽度的方块存储
''' </summary>
Public Class HorizontalTileStorage

    Sub New(height As Integer)
        If height < 1 Then
            Throw New ArgumentOutOfRangeException(NameOf(height))
        End If
        ReDim _rows(height - 1)
    End Sub

    Private _rows As TileRow()

    ''' <summary>
    ''' 选择一个范围内非空气的方块（从 0 开始）。
    ''' </summary>
    ''' <param name="fromX"></param>
    ''' <param name="fromY"></param>
    ''' <param name="toX"></param>
    ''' <param name="toY"></param>
    ''' <returns></returns>
    Default Public ReadOnly Iterator Property NonAirItems(fromX As Integer, fromY As Integer, toX As Integer, toY As Integer) As IEnumerable(Of (tile As Tile, posX As Integer, posY As Integer))
        Get
            Debug.Assert(fromX >= 0, "起始位置 x 必须大于等于 0.")
            Debug.Assert(fromY >= 0, "起始位置 y 必须大于等于 0.")
            Debug.Assert(fromX <= toX, "起始位置 x 必须大于等于结束位置 x.")
            Debug.Assert(fromY <= toY, "起始位置 y 必须大于等于结束位置 y.")
            Debug.Assert(toY < _rows.Length, "结束位置 x 必须小于总行数.")
            For r = fromY To toY
                Dim curRow = _rows(r).TileLists
                If curRow IsNot Nothing Then
                    For i = 0 To curRow.Length - 1
                        Dim curTileList = curRow(i)
                        Dim startIndex = curTileList.StartIndex
                        Dim endIndex = startIndex + curTileList.RepeatCount
                        If startIndex <= fromX Then
                            If endIndex >= fromX Then
                                ' ----[----  ]
                                '  ---[------]
                                '   --[------]--
                                '     [------]--
                                ' 左边界 fromX, 右边界 Min(toX, endIndex)
                                Dim lBound = fromX, uBound = Math.Min(toX, endIndex)
                                For j = lBound To uBound
                                    Yield (curTileList.TileData, j, i)
                                Next
                            End If
                        ElseIf startIndex <= toX Then
                            ' [  ----]---- 
                            ' [  ----] 
                            ' [  --  ]
                            ' 左边界 startIndex, 右边界 Min(toX, endIndex)
                            Dim lBound = startIndex, uBound = Math.Min(toX, endIndex)
                            For j = lBound To uBound
                                Yield (curTileList.TileData, j, i)
                            Next
                        End If
                    Next
                End If
            Next
        End Get
    End Property

    Public ReadOnly Property Height As Integer
        Get
            Return _rows.Length
        End Get
    End Property
End Class

Friend Structure TileRow
    Public TileLists As TileList()
End Structure

Friend Structure TileList
    Public StartIndex As Integer
    Public TileData As Tile
    Public RepeatCount As Integer
    Public Collier As IRectangleCollier
End Structure

Public Structure Tile
    Public Kind As ITileKind
    Public Data As ITileData

    Public Shared ReadOnly Air As New Tile

    Public ReadOnly Property IsAir As Boolean
        Get
            Return Kind Is Nothing
        End Get
    End Property
End Structure
