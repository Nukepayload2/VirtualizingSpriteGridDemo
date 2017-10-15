''' <summary>
''' �޶��߶ȵ��ǲ��޶���ȵ�ϡ�跽������
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
    ''' ��ȡһ����Χ�ڷǿ����ķ����ϡ���ʾ���� 0 ��ʼ����
    ''' </summary>
    ''' <param name="fromX">�� X ����</param>
    ''' <param name="fromY">�� Y ����</param>
    ''' <param name="toX">�� X ����</param>
    ''' <param name="toY">�� Y ����</param>
    ''' <value>���зǿ�������</value>
    Public ReadOnly Iterator Property NonAirItems(fromX As Integer, fromY As Integer, toX As Integer, toY As Integer) As IEnumerable(Of (Tile As Tile, Row As Integer, Column As Integer))
        Get
            Debug.Assert(fromX >= 0, "��ʼλ�� x ������ڵ��� 0.")
            Debug.Assert(fromY >= 0, "��ʼλ�� y ������ڵ��� 0.")
            Debug.Assert(fromX <= toX, "��ʼλ�� x ������ڵ��ڽ���λ�� x.")
            Debug.Assert(fromY <= toY, "��ʼλ�� y ������ڵ��ڽ���λ�� y.")
            Debug.Assert(toY < _rows.Length, "����λ�� x ����С��������.")
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
    ''' ��ָ���е�һ������ϲ���
    ''' </summary>
    ''' <param name="row"></param>
    ''' <param name="tiles"></param>
    Public Sub MergeTileList(row As Integer, tiles As TileList)
        Debug.Assert(row >= 0, "��ʼ�кű�����ڵ��� 0.")
        Debug.Assert(row < _rows.Length, "��ʼ�кű���С������ (�߶�).")
        MergeTileList(_rows(row), tiles)
    End Sub

    ''' <summary>
    ''' ��ָ���е�һ������ϲ���
    ''' </summary>
    ''' <param name="curRow"></param>
    ''' <param name="tiles"></param>
    Public Sub MergeTileList(curRow As List(Of TileList), tiles As TileList)
        EraseTiles(curRow, tiles.StartIndex, tiles.RepeatCount)
        If Not tiles.TileData.IsAir Then
            ' �ж�����Ƿ���һ���ķ��顣����ǣ��ϲ�����������µ� TileList��
            ' TBD: ɾ���Ż�: ���������һ����ɾ���ķ��飬��������ԭ������Ϣ��
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
    ''' ��ָ���е�һ�������е����з���������
    ''' </summary>
    Public Sub EraseTiles(curRow As List(Of TileList), start As Integer, length As Integer)
        For i = curRow.Count - 1 To 0 Step -1
            ' β���ཻ: ���� RepeatCount
            ' ͷ���ཻ: ���� StartIndex, ���� RepeatCount
            ' ����: ɾ��
            Dim tl = curRow(i)
            If tl.StartIndex >= start Then
                If tl.StartIndex + tl.RepeatCount <= start + length Then
                    ' ����
                    curRow.RemoveAt(i)
                Else
                    ' ͷ���ཻ
                    tl.RepeatCount -= start - tl.StartIndex
                    tl.StartIndex = start
                End If
            Else
                If tl.StartIndex + tl.RepeatCount - 1 >= start Then
                    ' β���ཻ
                    tl.RepeatCount -= tl.StartIndex + tl.RepeatCount - start
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' ��ȡ���趨ָ��λ�õĵ������顣�����Բ�����ѭ����ʹ�á�
    ''' </summary>
    ''' <param name="fromX">�� X ����</param>
    ''' <param name="fromY">�� Y ����</param>
    ''' <param name="toX">�� X ����</param>
    ''' <param name="toY">�� Y ����</param>
    ''' <value>���з���</value>
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
