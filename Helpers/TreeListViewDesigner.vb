Namespace Design

    ''' <summary>
    ''' TreeListViewDesigner Class.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class TreeListViewDesigner
        Inherits ContainerListViewDesigner

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListViewDesigner.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._preInit()
        End Sub

#End Region

#Region " Field Declarations "
        Dim _Verbs As New DesignerVerbCollection
#End Region

#Region " Properties "

        ''' <summary>
        ''' Shadowed.  Gets the TreeListView the designer is designing.
        ''' </summary>
        ''' <value>A TreeListView" /> control.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Control() As TreeListView
            Get
                Return DirectCast(MyBase.Control, TreeListView)
            End Get
        End Property

        ''' <summary>
        ''' Overriden.  Gets the design-time verbs supported by the component that is associated with the ContainerListView.
        ''' </summary>
        ''' <value>A DesignerVerbCollection of DesignerVerb objects, or a null reference (Nothing in Visual Basic) if no designer verbs are available.</value>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property Verbs() As DesignerVerbCollection
            Get
                Return Me._Verbs
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overriden.  Creates a method signature in the source code file for the default event on the component and navigates the user's cursor to that location.
        ''' </summary>
        ''' <remarks>In Windows Forms and Web Forms designers, this method is invoked when a user double-clicks a component.</remarks>
        Public Overrides Sub DoDefaultAction()
            Dim Pnt As Point = Me.Control.PointToClient(Windows.Forms.Control.MousePosition)
            Dim Nde As TreeListNode = DirectCast(Me.Control.GetItemAt(Pnt), TreeListNode)

            If (Nde IsNot Nothing) Then Nde.Toggle()
        End Sub

        ''' <summary>
        ''' Overriden.  Indicates whether a mouse click at the specified point should be handled by the control.
        ''' </summary>
        ''' <param name="point">A Point" /> indicating the position at which the mouse was clicked, in screen coordinates. </param>
        ''' <returns><c>TRUE</c> if a click at the specified point is to be handled by the control; otherwise, <c>FALSE</c>.</returns>
        ''' <remarks>
        ''' The GetHitTest method determines whether a click at the specified point should be passed to the control, while the control is in design mode. 
        ''' You can override and implement this method to enable your control to receive clicks in the design-time environment.
        ''' </remarks>
        Protected Overrides Function GetHitTest(ByVal point As Point) As Boolean
            Dim Valid As Boolean = MyBase.GetHitTest(point)

            'THE BASE GETHITTEST CHECKS IF A COLUMN OR SCROLLBAR WAS CLICKED.  IF NOT, THEN TEST FOR A NODE
            If (Not Valid) Then
                Dim Nde As TreeListNode = Nothing
                Dim Pnt As Point = Me.Control.PointToClient(point)
                Dim MsArg As New MouseEventArgs(MouseButtons.Left, 1, Pnt.X, Pnt.Y, 0)

                If (Me.Control.PlusMinusClicked(MsArg, Nde)) Then Valid = True
            End If

            Return Valid
        End Function

#End Region

#Region " Procedures "

        Private Sub _checkAll(ByVal sender As Object, ByVal e As EventArgs)
            For Each Nde As TreeListNode In Me.Control.Nodes
                Nde.CheckNodes(True, False)
            Next
        End Sub

        Private Sub _collapseAll(ByVal sender As Object, ByVal e As EventArgs)
            Me.Control.CollapseAll()
        End Sub

        Private Sub _expandAll(ByVal sender As Object, ByVal e As EventArgs)
            Me.Control.ExpandAll()
        End Sub

        Private Sub _preInit()
            With Me._Verbs
                .Add(New DesignerVerb("Collapse All", AddressOf Me._collapseAll))
                .Add(New DesignerVerb("Expand All", AddressOf Me._expandAll))
                .Add(New DesignerVerb("Check All", AddressOf Me._checkAll))
                .Add(New DesignerVerb("UnCheck All", AddressOf Me._unCheckAll))
            End With
        End Sub

        Private Sub _unCheckAll(ByVal sender As Object, ByVal e As EventArgs)
            For Each Nde As TreeListNode In Me.Control.Nodes
                Nde.CheckNodes(False, False)
            Next
        End Sub

#End Region

    End Class

End Namespace