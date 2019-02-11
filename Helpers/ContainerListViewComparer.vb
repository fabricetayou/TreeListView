Imports WinControls.ListView
Imports Microsoft.VisualBasic

Namespace Helpers

    ''' <summary>
    ''' ContainerListViewComparer Class.  Helper class used to sort any ListView by a specific column.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ContainerListViewComparer
        Implements IComparer

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewComparer.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._Col = 0
            Me._Order = SortOrder.Ascending
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewComparer.
        ''' </summary>
        ''' <param name="aCol">The Index of the column clicked in the ListView.</param>
        ''' <param name="aSortOrder">The SortOrder.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aCol As Integer, ByVal aSortOrder As SortOrder)
            If (aSortOrder = SortOrder.None) Then Throw New InvalidEnumArgumentException("aSortOrder", aSortOrder, GetType(SortOrder))

            Me._Col = Math.Abs(aCol)
            Me._Order = aSortOrder
        End Sub

#End Region

#Region " Field Declarations "
        Private _Col As Integer = 0
        Private _Order As SortOrder = SortOrder.Ascending
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets index of the ClickedColumn.
        ''' </summary>
        ''' <value>An Integer representing the Index of the Column clicked in the ListView.</value>
        ''' <remarks></remarks>
        Public Property ColumnIndex() As Integer
            Get
                Return Me._Col
            End Get
            Set(ByVal Value As Integer)
                If (Value >= 0) Then Me._Col = Value
            End Set
        End Property

        ''' <summary>
        '''Gets or Sets the SortOrder.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property Order() As SortOrder
            Get
                Return Me._Order
            End Get
            Set(ByVal Value As SortOrder)
                If (Value <> SortOrder.None AndAlso Me._Order <> Value) Then Me._Order = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other
        ''' </summary>
        ''' <param name="x">First object to compare.</param>
        ''' <param name="y">Second object to compare.</param>
        ''' <returns>A Integer value indicating whether one is less than, equal to or greater than the other.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim LVx As String = String.Empty
            Dim LVy As String = String.Empty
            Dim ReturnVal As Integer = -1
            Dim TmpClvo As ContainerListViewObject = Nothing
            Dim DateX As Date = Date.MinValue
            Dim DateY As Date = Date.MinValue
            Dim DecimalX As Decimal = Decimal.MinValue
            Dim DecimalY As Decimal = Decimal.MinValue

            'IF IT'S COLUMN ZERO THEN I KNOW IT'S SOME TYPE OF CONTAINERLISTVIEWOBJECT, OTHERWISE IT'S A SUBITEM
            If (Me._Col = 0) Then
                'GET THE FIRST VALUE AS STRING
                TmpClvo = DirectCast(x, ContainerListViewObject)
                If (TmpClvo IsNot Nothing) Then LVx = TmpClvo.Text.Trim()

                'GET THE 2ND VALUE AS STRING
                TmpClvo = DirectCast(y, ContainerListViewObject)
                If (TmpClvo IsNot Nothing) Then LVy = TmpClvo.Text.Trim()
            Else
                'GET THE FIRST VALUE AS STRING
                TmpClvo = DirectCast(x, ContainerListViewObject)
                If (TmpClvo IsNot Nothing) Then LVx = TmpClvo.SubItems(Me._Col - 1).Text.Trim()

                'GET THE 2ND VALUE AS STRING
                TmpClvo = DirectCast(y, ContainerListViewObject)
                If (TmpClvo IsNot Nothing) Then LVy = TmpClvo.SubItems(Me._Col - 1).Text.Trim()
            End If

            'COMPARE
            If (Date.TryParse(LVx, DateX) AndAlso Date.TryParse(LVy, DateY)) Then
                ReturnVal = Date.Compare(DateX, DateY)
            ElseIf (Decimal.TryParse(LVx, DecimalX) And Decimal.TryParse(LVy, DecimalY)) Then
                ReturnVal = Decimal.Compare(DecimalX, DecimalY)
            Else
                ReturnVal = String.Compare(LVx, LVy, StringComparison.OrdinalIgnoreCase)
            End If

            'REVERSE THE SIGN IF WE ARE DESCENDING
            If (Me._Order = SortOrder.Descending) Then ReturnVal *= -1

            Return ReturnVal
        End Function

#End Region

#Region " Procedures "

#End Region

    End Class

    ''' <summary>
    ''' PropInfoViewComparer Class.  Helper class used to sort arrays or collections of PropertyInfo objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class PropInfoViewComparer
        Implements IComparer

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewComparer.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._Order = SortOrder.Ascending
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewComparer.
        ''' </summary>
        ''' <param name="aSortOrder">The SortOrder.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aSortOrder As SortOrder)
            If (aSortOrder = SortOrder.None) Then Throw New InvalidEnumArgumentException("aSortOrder", aSortOrder, GetType(SortOrder))

            Me._Order = aSortOrder
        End Sub

#End Region

#Region " Field Declarations "
        Private _Order As SortOrder = SortOrder.Ascending
#End Region

#Region " Properties "

        ''' <summary>
        '''Gets or Sets the SortOrder.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property Order() As SortOrder
            Get
                Return Me._Order
            End Get
            Set(ByVal Value As SortOrder)
                If (Value <> SortOrder.None AndAlso Me._Order <> Value) Then Me._Order = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other
        ''' </summary>
        ''' <param name="aObj1">First object to compare.</param>
        ''' <param name="aObj2">Second object to compare.</param>
        ''' <returns>A Integer value indicating whether one is less than, equal to or greater than the other.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Compare(ByVal aObj1 As Object, ByVal aObj2 As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim PI01 As Reflection.PropertyInfo
            Dim PI02 As Reflection.PropertyInfo
            Dim ReturnVal As Integer

            'CONVERT
            PI01 = DirectCast(aObj1, Reflection.PropertyInfo)
            PI02 = DirectCast(aObj2, Reflection.PropertyInfo)

            ReturnVal = String.Compare(PI01.Name, PI02.Name, StringComparison.OrdinalIgnoreCase)

            'REVERSE THE SIGN IF WE ARE DESCENDING
            If (Me._Order = SortOrder.Descending) Then ReturnVal *= -1

            Return ReturnVal
        End Function

#End Region

#Region " Procedures "

#End Region

    End Class

End Namespace
