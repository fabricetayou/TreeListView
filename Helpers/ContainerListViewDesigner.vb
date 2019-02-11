Namespace Design

    ''' <summary>
    ''' ContainerListViewDesigner Class.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class ContainerListViewDesigner
        Inherits System.Windows.Forms.Design.ControlDesigner

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewDesigner.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._preInit()
        End Sub

        ''' <summary>
        ''' Overriden.  Releases the unmanaged resources used by the ComponentDesigner and optionally releases the managed resources.
        ''' </summary>
        ''' <param name="disposing"><c>TRUE</c> to release both managed and unmanaged resources; <c>FALSE</c> to release only unmanaged resources.</param>
        ''' <remarks></remarks>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            'UNHOOK EVENTS
            RemoveHandler Me._CompChgSvc.ComponentRemoving, AddressOf Me._componentRemoving
            RemoveHandler Me._SelSvc.SelectionChanged, AddressOf Me._selectionChanged

            MyBase.Dispose(disposing)
        End Sub

#End Region

#Region " Field Declarations "
        Private _CompChgSvc As IComponentChangeService
        Private _DsgnHost As IDesignerHost
        Private _SelCol As ContainerColumnHeader
        Private _SelSvc As ISelectionService
#End Region

#Region " Properties "

        ''' <summary>
        ''' Overriden.  Gets the collection of components associated with the component managed by the designer.
        ''' </summary>
        ''' <value>The components that are associated with the component managed by the designer.</value>
        ''' <remarks>
        ''' This property indicates any components to copy or move along with the component managed by the designer during a copy, drag or move operation.  
        ''' If this collection contains references to other components in the current design mode document, those components will be copied along with the component managed by the designer during a copy operation.  
        ''' When the component managed by the designer is selected, this collection is filled with any nested controls. This collection can also include other components, such as the buttons of a toolbar.
        ''' </remarks>
        Public Overrides ReadOnly Property AssociatedComponents() As System.Collections.ICollection
            Get
                Return Me.Control.Columns
            End Get
        End Property

        '''' <summary>
        '''' Gets or Sets the shadowed ColumnHeaderContextMenuStrip property of the control being designed.
        '''' </summary>
        '''' <value></value>
        '''' <remarks></remarks>
        'Public Property ColumnHeaderContextMenu() As ContextMenuStrip
        '    Get
        '        Return DirectCast(Me.ShadowProperties("ColumnHeaderContextMenu"), ContextMenuStrip)
        '    End Get
        '    Set(ByVal Value As ContextMenuStrip)
        '        Me.ShadowProperties("ColumnHeaderContextMenu") = Value
        '    End Set
        'End Property

        ''' <summary>
        ''' Gets or Sets the shadowed ColumnSortColorEnabled property of the control being designed.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property ColumnSortColorEnabled() As Boolean
            Get
                Return CBool(Me.ShadowProperties.Item("ColumnSortColorEnabled"))
            End Get
            Set(ByVal Value As Boolean)
                Me.ShadowProperties.Item("ColumnSortColorEnabled") = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the shadowed ColumnTracking property of the control being designed.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public Property ColumnTracking() As Boolean
            Get
                Return CBool(Me.ShadowProperties.Item("ColumnTracking"))
            End Get
            Set(ByVal Value As Boolean)
                Me.ShadowProperties.Item("ColumnTracking") = Value
            End Set
        End Property

        ''' <summary>
        ''' Shadowed.  Gets the ContainerListView the designer is designing.
        ''' </summary>
        ''' <value>A ContainerListView" /> control.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Control() As ContainerListView
            Get
                Return DirectCast(MyBase.Control, ContainerListView)
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overriden.  Indicates whether a mouse click at the specified point should be handled by the control.
        ''' </summary>
        ''' <param name="point">A Point" /> indicating the position at which the mouse was clicked, in screen coordinates. </param>
        ''' <returns><c>TRUE</c> if a click at the specified point is to be handled by the control; otherwise, <c>FALSE</c>.</returns>
        ''' <remarks>
        ''' The GetHitTest method determines whether a click at the specified point should be passed to the control, while the control is in design mode. 
        ''' You can override and implement this method to enable your control to receive clicks in the design-time environment.
        ''' </remarks>
        Protected Overrides Function GetHitTest(ByVal point As System.Drawing.Point) As Boolean
            Dim HRect, VRect As Rectangle
            Dim Pnt As Point = Me.Control.PointToClient(point)
            Dim MsArg As New MouseEventArgs(MouseButtons.Left, 1, Pnt.X, Pnt.Y, 0)

            'CHECK FOR COLUMNS
            For Each Col As ContainerColumnHeader In Me.Control.Columns
                If (Col.Bounds.Contains(MsArg.X, MsArg.Y) OrElse _
                    Col.SizingBounds.Contains(MsArg.X, MsArg.Y)) Then
                    Return True
                End If
            Next

            'CHECK FOR HSCROLL
            With Me.Control.HScroll
                HRect = New Rectangle(.Location.X, .Location.Y, .Width, .Height)
                If (HRect.Contains(MsArg.X, MsArg.Y)) Then Return True
            End With

            'CHECK FOR VSCROLL
            With Me.Control.VScroll
                VRect = New Rectangle(.Location.X, .Location.Y, .Width, .Height)
                If (HRect.Contains(MsArg.X, MsArg.Y)) Then Return True
            End With

            Return False
        End Function

        ''' <summary>
        ''' Overriden.  Initializes the designer with the specified component.
        ''' </summary>
        ''' <param name="component">The IComponent to associate the designer with. This component must always be an instance of, or derive from, Control.</param>
        ''' <remarks>The IComponent to associate the designer with. This component must always be an instance of, or derive from, Control.</remarks>
        Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
            MyBase.Initialize(component)

            'INITIALIZE CLASS VARIABLES
            Me._DsgnHost = DirectCast(GetService(GetType(IDesignerHost)), IDesignerHost)
            Me._CompChgSvc = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
            Me._SelSvc = DirectCast(GetService(GetType(ISelectionService)), ISelectionService)

            'HOOK UP EVENTS
            AddHandler Me._SelSvc.SelectionChanged, AddressOf Me._selectionChanged
            AddHandler Me._CompChgSvc.ComponentRemoving, AddressOf Me._componentRemoving

            'SET CERTAIN PROPERTIES
            'Me.Control.ColumnHeaderContextMenuStrip = Nothing
            Me.Control.ColumnSortColorEnabled = False
            Me.Control.ColumnTracking = False
        End Sub

        ''' <summary>
        ''' Overriden.  Called when the control that the designer is managing has painted its surface so the designer can paint any additional adornments on top of the control.
        ''' </summary>
        ''' <param name="pe">A PaintEventArgs the designer can use to draw on the control.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnPaintAdornments(ByVal pe As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaintAdornments(pe)

            If (Me._SelSvc.PrimarySelection Is Me._SelCol) Then
                Dim BckClr As Color = Color.White
                Dim ActiveCol As ContainerColumnHeader

                'GET THE COLUMN
                If (Me._SelCol IsNot Nothing) Then ActiveCol = Me._SelCol Else ActiveCol = DirectCast(Me._SelSvc.PrimarySelection, ContainerColumnHeader)

                If (ActiveCol IsNot Nothing) Then
                    'IF WE ARE NOT USING VISUALSTYLES, THEN THE BACKCOLOR WILL BE CONTROL
                    If (ActiveCol.ListView IsNot Nothing AndAlso Not ActiveCol.ListView.VisualStyles) Then BckClr = SystemColors.Control

                    'DRAW THE FOCUSRECTANGLE
                    ControlPaint.DrawFocusRectangle(pe.Graphics, ActiveCol.Bounds, Me._SelCol.ForeColor, BckClr)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Overriden.  Adjusts the set of properties the component exposes through a TypeDescriptor.
        ''' </summary>
        ''' <param name="properties">An IDictionary containing the properties for the class of the component.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub PreFilterProperties(ByVal properties As System.Collections.IDictionary)
            MyBase.PreFilterProperties(properties)

            'REPLACE SELECTED PROPERTIES WITH OUR SHADOWED ONES
            'properties("ColumnHeaderContextMenuStrip") = TypeDescriptor.CreateProperty(GetType(ContainerListViewDesigner), _
            '                                        DirectCast(properties.Item("ColumnHeaderContextMenuStrip"), PropertyDescriptor), Nothing)

            properties("ColumnSortColorEnabled") = TypeDescriptor.CreateProperty(GetType(ContainerListViewDesigner), _
                                                   DirectCast(properties.Item("ColumnSortColorEnabled"), PropertyDescriptor), Nothing)

            properties("ColumnTracking") = TypeDescriptor.CreateProperty(GetType(ContainerListViewDesigner), _
                                           DirectCast(properties.Item("ColumnTracking"), PropertyDescriptor), Nothing)
        End Sub

#End Region

#Region " Procedures "

        Private Sub _componentRemoving(ByVal sender As Object, ByVal e As ComponentEventArgs)
            Dim CCH As ContainerColumnHeader

            If (TypeOf e.Component Is ContainerColumnHeader) Then
                CCH = DirectCast(e.Component, ContainerColumnHeader)

                If (Me.Control.Columns.Contains(CCH)) Then
                    Me._CompChgSvc.OnComponentChanging(Me.Control, Nothing)
                    CCH.Remove()
                    Me._CompChgSvc.OnComponentChanged(Me.Control, Nothing, Nothing, Nothing)
                End If
            ElseIf (e.Component Is Me.Control) Then
                For I As Integer = (Me.Control.Columns.Count - 1) To 0 Step -1
                    CCH = Me.Control.Columns(I)

                    Me._CompChgSvc.OnComponentChanging(Me.Control, Nothing)
                    CCH.Remove()
                    Me._DsgnHost.DestroyComponent(CCH)
                    Me._CompChgSvc.OnComponentChanged(Me.Control, Nothing, Nothing, Nothing)
                Next
            End If
        End Sub

        Private Sub _preInit()
            'PUT ANY INTIALIZATION CODE IN HERE.  IF YOU DO NOT NEED THIS SUB, THEN YOU CAN SAFELY DELETE IT AND THE CALL IN 
            'THE CONSTRUCTOR ALSO.
        End Sub

        Private Sub _selectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim NewSelCol As ContainerColumnHeader = Nothing

            'SEE IF THE PRIMARY SELECTION IS ONE OF THE COLUMNS
            For Each Col As ContainerColumnHeader In Me.Control.Columns
                If (Me._SelSvc.PrimarySelection Is Col) Then
                    NewSelCol = Col
                    Exit For
                End If
            Next

            'APPLY IF NECESSARY
            If (Not Me._SelCol Is NewSelCol) Then
                Me._SelCol = NewSelCol
                Me.Control.Invalidate()
            End If
        End Sub

#End Region

    End Class

End Namespace