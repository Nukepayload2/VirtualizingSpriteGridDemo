Public Structure TileKind
    Implements IEquatable(Of TileKind)

    Public ReadOnly Primary As Integer
    Public ReadOnly Secondary As Integer

    Public Sub New(primary As Integer, secondary As Integer)
        Me.Primary = primary
        Me.Secondary = secondary
    End Sub

    Public Overloads Function Equals(other As TileKind) As Boolean Implements IEquatable(Of TileKind).Equals
        Return other.Primary = Primary AndAlso other.Secondary = Secondary
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If
        If TypeOf obj Is TileKind Then
            Dim other = DirectCast(obj, TileKind)
            Return Equals(other)
        End If
        Return False
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Primary Xor Secondary
    End Function

    Public Shared Operator =(left As TileKind, right As TileKind) As Boolean
        Return left.Primary = right.Primary AndAlso left.Secondary = right.Secondary
    End Operator

    Public Shared Operator <>(left As TileKind, right As TileKind) As Boolean
        Return left.Primary <> right.Primary OrElse left.Secondary <> right.Secondary
    End Operator
End Structure
