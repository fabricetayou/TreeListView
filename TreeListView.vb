Imports WinControls.ListView.ContainerListViewItem

''' <summary>
''' TreeListView Class.
''' </summary>
''' <remarks></remarks>
<DefaultProperty("Nodes"),
 DefaultEvent("AfterSelect"),
 Designer(GetType(TreeListViewDesigner)),
 ToolboxBitmap(GetType(TreeListView), "TreeListView.png")>
Public Class TreeListView
    Inherits ContainerListView

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New instance of TreeListView.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        Me._preInit()
    End Sub

#End Region

#Region " Field Declarations "
    Private _AllCollapsed As Boolean
    Private _AlwaysShowPM As Boolean
    Private _Brush1 As New SolidBrush(Color.White)
    Private _CachedImageList As ImageList
    Private _CurNode As TreeListNode
    Private _DefaultDragDrop As Boolean
    Private _DefaultImageIndex As Integer = 0
    Private _DefaultSelectedIndex As Integer = 1
    Private _DragArg As TreeListViewItemDragEventArgs
    Private _DragImgList As ImageList
    Private _DragScroll As Integer = 4
    Private _FireItemDrag As Boolean
    Private _FirstSelectedNode As TreeListNode
    Private _FolderImages As New ImageList
    Private _ImageRects As New Hashtable
    Private _Indent As Integer = 19
    Private _InDrag As Boolean
    Private _LastNodeHovered As TreeListNode
    Private _MinusBitMap, _PlusBitMap As Bitmap
    Private _MouseActivate As Boolean
    Private _NodeRowRects As New Hashtable
    Private _Nodes As TreeListNodeCollection
    Private _PathDivider As String = "\"
    Private _Pen1 As New Pen(SystemBrushes.ControlDark)
    Private _Pen2 As New Pen(New SolidBrush(Color.Black))
    Private _NodeGridLineStyle As NodeGridLineStyle = NodeGridLineStyle.AsListView
    Private _NodeGridLineColor As Color = Color.Silver
    Private _NodeGridLinePen As Pen
    Private _PlusMinusRects As New Hashtable
    Private _RendCount As Integer
    Private _RootLineColor As Color = SystemColors.ControlDark
    Private _ShowLines As Boolean = True
    Private _ShowRootLines As Boolean = True
    Private _ShowPlusMinus As Boolean = True
    Private _TempDropNode As TreeListNode
    Private _TotalRend As Integer
    Private _UseDefaultFolders As Boolean = True
    Private _VirtualParent As New TreeListNode(Me)
#End Region

#Region " Events "

    ''' <summary>
    ''' Occurs after the TreeListNode has collapsed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AfterCollapse As TreeListViewEventHandler

    ''' <summary>
    ''' Occurs after the TreeListNode has expanded.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AfterExpand As TreeListViewEventHandler

    ''' <summary>
    ''' Occurs before the TreeListNode is collapsed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeforeCollapse As TreeListViewCancelEventHandler

    ''' <summary>
    ''' Occurs before the TreeListNode is expanded.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeforeExpand As TreeListViewCancelEventHandler

    ''' <summary>
    ''' Occurs when the user begins dragging a node.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ItemDrag As TreeListViewItemDragEventHandler

    ''' <summary>
    ''' Occurs when the user has dropped data on a node.
    ''' </summary>
    ''' <remarks>
    ''' This event will NOT fire if property AllowDefaultDragDrop=<c>FALSE</c>.  This is only a convenience event for the developer so 
    ''' they don't have to handle the normal DragDrop event during drag operations.  Also, it will only fire if the user is 
    ''' dragging nodes from within the control itself.  If you are expecting any outside dragging onto this control, you will have 
    ''' to handle the normal DragDrop event.
    ''' </remarks>
    Public Event ItemDrop As TreeListViewItemDropEventHandler

#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or Sets a value that determines if the control will implement the DragDrop behavior within itself (not accepting 
    ''' data from other sources).  If setting this property to <c>TRUE</c> you must also set the 'AllowDrop' property to <c>TRUE</c> in 
    ''' order for it to work.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control will implement it's own, default DragDrop behavior; otherwise <c>FALSE</c> if the DragDrop behavior will be implemented by the developer.</value>
    ''' <remarks>
    ''' If <c>TRUE</c>, then the only events that need to be handled are the ItemDrag and ItemDrop events.  Otherwise typical Microsoft 
    ''' drag and drop events should be handled for the desired effect.
    ''' NOTE:  The ItemDrag event will always be fired regardless if this property is <c>TRUE</c> or <c>FALSE</c>.
    ''' </remarks>
    <Category("Behavior"),
     DefaultValue(False),
     RefreshProperties(RefreshProperties.All),
     Description("Determines if the control will implement the DragDrop behavior within itself (not accepting data from other " &
                 "sources).  If setting this property to TRUE you must also set the 'AllowDrop' property to TRUE in order for " &
                 "it to work.  See the help file for more information on this property and how do handle it accordingly.")>
    Public Property AllowDefaultDragDrop() As Boolean
        Get
            Return Me._DefaultDragDrop
        End Get
        Set(ByVal Value As Boolean)
            Me._DefaultDragDrop = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if Plus and Minus signs are always shown next to each node.
    ''' </summary>
    ''' <value><c>TRUE</c> if Plus and Minus signs are always shown next to each node; otherwise <c>FALSE</c>.  The default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"),
     DefaultValue(False),
     Description("Determines if Plus and Minus signs are always shown next to each node.")>
    Public Property AlwaysShowPlusMinus() As Boolean
        Get
            Return Me._AlwaysShowPM
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._AlwaysShowPM.Equals(Value)) Then
                Me._AlwaysShowPM = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the amount the Vertical scroll will increment whenever a node or nodes are being dragged out of the visible area.  
    ''' The lareger the number, the faster the scroll.
    ''' </summary>
    ''' <value>An Integer value.</value>
    ''' <remarks></remarks>
    <Category("Behavior"),
     DefaultValue(GetType(Integer), "4"),
     Description("The amount the Vertical scroll will increment/decrement whenever a node or nodes are being dragged out of the visible area.  " &
                 "The larger the number, the faster the scroll.")>
    Public Property DragScrollIncrement() As Integer
        Get
            Return Me._DragScroll
        End Get
        Set(ByVal Value As Integer)
            Me._DragScroll = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the currently Focused TreeListNode.
    ''' </summary>
    ''' <value>A TreeListNode with the current focus.</value>
    ''' <remarks></remarks>
    <Browsable(False)>
    Protected Friend ReadOnly Property FocusedNode() As TreeListNode
        Get
            Return Me._FirstSelectedNode
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the Font used to display text in the control.
    ''' </summary>
    ''' <value>A Font value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Font), "System.Drawing.SystemFonts.DefaultFont"),
     Description("The Font used to display text in the control.")>
    Public Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal Value As Font)
            If (Not MyBase.Font.Equals(Value)) Then
                MyBase.Font = Value

                Me.BeginUpdate()
                For Each Nde As TreeListNode In Me._Nodes
                    Me._baseFontChanged(Nde)
                Next
                Me.EndUpdate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the foreground color used to display text and graphics in the control.
    ''' </summary>
    ''' <value>A Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "WindowText"),
     Description("The foreground color used to display text and graphics in the control.")>
    Public Overrides Property ForeColor() As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(ByVal Value As Color)
            If (Not MyBase.ForeColor.Equals(Value)) Then
                MyBase.ForeColor = Value

                Me.BeginUpdate()
                For Each Nde As TreeListNode In Me._Nodes
                    Me._baseForeColorChanged(Nde)
                Next
                Me.EndUpdate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Shadowed.  Gets or Sets a value that determines which gridlines will be shown.
    ''' </summary>
    ''' <value>An enumerated type specifying which gridlines are shown on the control.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(GridLineSelections), "Column"),
     Description("Determines which gridlines will be shown.")>
    Public Shadows Property GridLines() As GridLineSelections
        Get
            Return MyBase.GridLines
        End Get
        Set(ByVal Value As GridLineSelections)
            MyBase.GridLines = Value
        End Set
    End Property
    ''' <summary>
    ''' Gets or Sets the Color used for Gridlines.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "Silver"),
     Description("Specifies the Color used for Gridlines.")>
    Public Property NodeGridLineColor() As Color
        Get
            Return Me._NodeGridLineColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._NodeGridLineColor.Equals(Value)) Then
                Me._NodeGridLineColor = Value
                _refreshNodeGriden()
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property
    ''' <summary>
    ''' Gets or Sets the style used for Tree Gridlines.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(NodeGridLineStyle), NameOf(NodeGridLineStyle.AsListView)),
     Description("Specifies the style used for Tree Gridlines.")>
    Public Property NodeGridLineStyle() As NodeGridLineStyle
        Get
            Return Me._NodeGridLineStyle
        End Get
        Set(ByVal Value As NodeGridLineStyle)
            If (Not Me._NodeGridLineStyle.Equals(Value)) Then
                Me._NodeGridLineStyle = Value
                _refreshNodeGriden()
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the control should use the default open/closed folder images for nodes.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control should use the default open/closed folder images; otherwise <c>FALSE</c> if not.</value>
    ''' <remarks>NOTE:  Any custom image list will be overwritten with the default, internal image list if this is set to <c>TRUE</c>.</remarks>
    <Category("Appearance"),
     DefaultValue(True),
     Description("Determines if the control should use the default open/closed folder images for nodes.")>
    Public Property DefaultFolderImages() As Boolean
        Get
            Return Me._UseDefaultFolders
        End Get
        Set(ByVal value As Boolean)
            If (Me._UseDefaultFolders <> value) Then
                Me._UseDefaultFolders = value
                Me._useFoldersImages()
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the default image index to use when creating nodes.
    ''' </summary>
    ''' <value>The default zero-based index of the image in the ImageList that is displayed for the TreeListNode when it is created. The default is 0.</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(0),
     Description("The default image index to use when creating nodes."),
     TypeConverter(GetType(ImageIndexConverter)),
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(UITypeEditor))>
    Public Property DefaultImageIndex() As Integer
        Get
            Return Me._DefaultImageIndex
        End Get
        Set(ByVal value As Integer)
            If (Me._DefaultImageIndex <> value) Then
                Me._DefaultImageIndex = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the default selected image index to use when creating nodes.
    ''' </summary>
    ''' <value>The zero-based default selected index of the image in the ImageList that is displayed for the TreeListNode when it is created. The default is 1.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(1),
     Description("The default selected image index to use when creating nodes."),
     TypeConverter(GetType(ImageIndexConverter)),
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(UITypeEditor))>
    Public Property DefaultSelectedImageIndex() As Integer
        Get
            Return Me._DefaultSelectedIndex
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._DefaultSelectedIndex.Equals(Value)) Then Me._DefaultSelectedIndex = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Indent value of child nodes (in pixels)
    ''' </summary>
    ''' <value>An Integer representing the Indent value of child nodes.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(19),
     Description("The Indent value of child nodes (in pixels)")>
    Public Property IndentSize() As Integer
        Get
            Return Me._Indent
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._Indent.Equals(Value)) Then
                Me._Indent = Math.Abs(Value)
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets the Items contained in the control.
    ''' </summary>
    ''' <value>A ContainerListViewItemCollection.</value>
    ''' <remarks>Items collection only used for a ContainerListView.  Use the Nodes collection instead.</remarks>
    <XmlIgnore(),
     Browsable(False),
     EditorBrowsable(EditorBrowsableState.Never),
     Obsolete("Items collection is only used for a ContainerListView.  For a TreeListView, use the Nodes collection instead.", True)>
    Public Overrides ReadOnly Property Items() As ContainerListViewItemCollection
        Get
            Return MyBase.Items
        End Get
    End Property

    ''' <summary>
    ''' Gets the child Nodes of the current Tree.
    ''' </summary>
    ''' <value>A TreeListNode collection.</value>
    ''' <remarks></remarks>
    <Category("Data"),
     Localizable(True),
     MergableProperty(False),
     Description("The collection of ChildNodes of the current Tree."),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property Nodes() As TreeListNodeCollection
        Get
            Return Me._Nodes
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets the string to use as the path separator when using the FullPath property on a TreeListNode.
    ''' </summary>
    ''' <value>A String value.  Default is '\' (without the quotes).</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(String), "\"),
     Description("The string to use as the path separator when using the FullPath property on a TreeListNode.")>
    Public Property PathSeparator() As String
        Get
            Return Me._PathDivider
        End Get
        Set(ByVal Value As String)
            If (Not Value.Equals(String.Empty)) Then
                Me._PathDivider = Value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines the Color used for drawing Root lines of TreeListNodes.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "ControlDark"),
     Description("The Color used for drawing Root lines of TreeListNodes.")>
    Public Property RootLineColor() As Color
        Get
            Return Me._RootLineColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._RootLineColor.Equals(Value)) Then
                Me._RootLineColor = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the RowHeight of the Items in the listview.
    ''' </summary>
    ''' <value>An Integer representing the row height.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(16),
     Description("Sets the RowHeight of the Items in the listview.")>
    Public Shadows Property RowHeight() As Integer
        Get
            Return MyBase.RowHeight
        End Get
        Set(ByVal Value As Integer)
            MyBase.RowHeight = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the indexes of the Selected items in the control.
    ''' </summary>
    ''' <value>A collection of selected Indices</value>
    ''' <remarks></remarks>
    <XmlIgnore(),
     Browsable(False),
     Obsolete("SelectedIndexes collection is used for a ContainerListView only.  TreeListViews do not support this property.", True),
     EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Shadows ReadOnly Property SelectedIndexes() As SelectedIndexCollection
        Get
            Return MyBase.SelectedIndexes
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if Lines are shown between sibling nodes and between parent and child nodes.
    ''' </summary>
    ''' <value><c>TRUE</c> if Lines are shown between sibling nodes and between parent and child nodess; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"),
     DefaultValue(True),
     Description("Determines if Lines are shown between sibling nodes and between parent and child nodes.")>
    Public Property ShowLines() As Boolean
        Get
            Return Me._ShowLines
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._ShowLines.Equals(Value)) Then
                Me._ShowLines = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether plus/minus signs are shown next to parent nodes.
    ''' </summary>
    ''' <value><c>TRUE</c> if plus/minus signs are shown; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"),
     DefaultValue(True),
     Description("Determines whether plus/minus signs are shown next to parent nodes.")>
    Public Property ShowPlusMinus() As Boolean
        Get
            Return Me._ShowPlusMinus
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._ShowPlusMinus.Equals(Value)) Then
                Me._ShowPlusMinus = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if Lines are shown between Root nodes.
    ''' </summary>
    ''' <value><c>TRUE</c> if Lines are show between Root nodes; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"),
     DefaultValue(True),
     Description("Determines if Lines are shown between Root nodes.")>
    Public Property ShowRootLines() As Boolean
        Get
            Return Me._ShowRootLines
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._ShowRootLines.Equals(Value)) Then
                Me._ShowRootLines = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Collapses every TreeListNode in the control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CollapseAll()
        For Each Node As TreeListNode In Me._Nodes
            Node.CollapseAll()
        Next

        Me._AllCollapsed = True
        Me.Invalidate()
    End Sub
    Public Sub SetColumnWidth(index As Integer)
        If index > Me.Columns.Count Then Return
        Dim Col As ContainerColumnHeader = Me.Columns(index)

        Dim Mwid, Twid As Integer
        For Each Node As TreeListNode In Me._Nodes
            If index = 0 Then
                Twid = Tools.GetStringWidth(Node.Text.ToUpper, Node.Font) + 4
            Else
                Twid = Tools.GetStringWidth(Node.SubItems(index).Text.ToUpper, Node.SubItems(index).Font) + 4
            End If
            If (Twid > Mwid) Then Mwid = Twid
        Next
        Col.Width = Mwid
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Expands every TreeListNode in the control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExpandAll()
        For Each Node As TreeListNode In Me._Nodes
            Node.ExpandAll()
        Next

        Me._AllCollapsed = False
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Overriden.  Returns the bounding Rectangle of the TreeListNode and it's SubItems.
    ''' </summary>
    ''' <param name="aObj">The TreeListNode whose rectangle to return.</param>
    ''' <returns>A Rectangle.</returns>
    ''' <remarks></remarks>
    Protected Friend Overrides Function GetCompleteItemBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        If (aObj Is Nothing) Then Throw New NullReferenceException("aObj cannot be NULL.")
        If (Not TypeOf aObj Is TreeListNode) Then Throw New InvalidCastException("aObj must be a TreeListNode")

        Return Me._getNodeCompleteBounds(DirectCast(aObj, TreeListNode))
    End Function

    ''' <summary>
    ''' Retrieves the TreeListNode at the specified position.
    ''' </summary>
    ''' <param name="aPoint">The Point the TreeListNode is at.</param>
    ''' <returns>A TreeListNode; otherwise <c>NOTHING</c> if there is no TreeListNode at the specified point.</returns>
    ''' <remarks></remarks>
    Public Overloads Overrides Function GetItemAt(ByVal aPoint As Point) As ContainerListViewObject
        Dim Rect As Rectangle
        Dim Keys As IEnumerator = Me._NodeRowRects.Keys.GetEnumerator

        While (Keys.MoveNext)
            Rect = DirectCast(Keys.Current, Rectangle)

            'OFFSET THE BOTTOM BY -1 AS THE BOTTOM OF ONE NODE RECT IS ACTUALLY THE TOP OF THE NEXT (THEY OVERLAP)
            If (aPoint.X >= Rect.X AndAlso aPoint.X <= Rect.Right) AndAlso (aPoint.Y >= Rect.Y AndAlso (aPoint.Y <= Rect.Bottom - 1)) Then
                Return DirectCast(Me._NodeRowRects(Rect), TreeListNode)
            End If
        End While

        Return Nothing
    End Function

    ''' <summary>
    ''' Retrieves a TreeListNode based upon the specified Path (path must be fully qualified).
    ''' </summary>
    ''' <param name="aNodePath">The Path of the Node to retrieve.</param>
    ''' <returns>A TreeListNode ojbect; otherwise <c>NOTHING</c> if the node cannot be located within the specified path.</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetItemAt(ByVal aNodePath As String) As TreeListNode
        Dim Node As TreeListNode = Nothing

        If (aNodePath <> String.Empty) Then
            'FIRST REPLACE THE NODEPATHSEPARATOR WITH OUR DEFAULT FOR EASY PARSING.
            Dim NewPath As String = aNodePath.Trim.Replace(Me._PathDivider, "\")
            Dim Path() As String = NewPath.Split("\"c)

            If (Path IsNot Nothing) Then
                Node = Me._getTheNode(Me._Nodes, Path(0))

                If (Node IsNot Nothing) Then
                    For I As Integer = 1 To Path.GetUpperBound(0)
                        Node = Me._getTheNode(Node.Nodes, Path(I))
                        If (Node Is Nothing) Then Exit For
                    Next
                End If
            End If
        End If

        Return Node
    End Function

    ''' <summary>
    ''' Overriden.  Returns the bounding Rectangle of the TreeListNode only.
    ''' </summary>
    ''' <param name="aObj">The TreeListNode whose rectangle to return.</param>
    ''' <returns>A Rectangle.</returns>
    ''' <remarks></remarks>
    Protected Friend Overrides Function GetItemBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        If (Not TypeOf aObj Is TreeListNode) Then Throw New InvalidCastException("aObj must be a TreeListNode")

        Return Me._getNodeBounds(DirectCast(aObj, TreeListNode))
    End Function

    ''' <summary>
    ''' Gets the Next node relative to the specified node.
    ''' </summary>
    ''' <param name="aNode">The Node whose next node to retrieve.</param>
    ''' <returns>A TreeListNode; otherwise <c>NOTHING</c> if no node can be found.</returns>
    ''' <remarks></remarks>
    Protected Friend Function GetNextNode(ByVal aNode As TreeListNode) As TreeListNode
        If (aNode IsNot Nothing) Then
            If (aNode.IsExpanded AndAlso aNode.Nodes.Count > 0) Then
                Return aNode.FirstChildNode
            ElseIf (aNode.NextSiblingNode IsNot Nothing) Then
                Return aNode.NextSiblingNode
            ElseIf (aNode.NextSiblingNode Is Nothing) Then
                'THIS IS WHERE GETNODESINPATH COMES IN HANDY
                Dim Nodes() As TreeListNode = aNode.GetNodesInPath

                'IF THE NEXTSIBLINGNODE IS NOTHING, WE MUST KEEP TRAVERSING BACK UP THE NODES UNTIL WE FIND A NEXTSIBLING NODE.
                'IF WE DON'T FIND ONE, THEN THAT MEANS WE ARE ON THE VERY LAST NODE IN THE TREE.
                If (Nodes IsNot Nothing) Then
                    Dim Nde As TreeListNode

                    'START AT THE END OF THE ARRAY, BECAUSE THE NODES ARE INDEXED STARTING FROM THE ROOTNODE
                    For I As Integer = Nodes.GetUpperBound(0) To 0 Step -1
                        Nde = Nodes(I)
                        If (Not Nde Is aNode AndAlso Nde.NextSiblingNode IsNot Nothing) Then Return Nde.NextSiblingNode
                    Next
                End If

                Return Nothing
            Else
                'I DOUBT IF WE WILL EVER GET TO THIS POINT, BUT IT IS HERE JUST IN CASE
                Return aNode.RootNode.NextSiblingNode
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the number of child tree nodes.
    ''' </summary>
    ''' <param name="aIncludeSubTrees"><c>TRUE</c> if the resulting count includes all nodes indirectly rooted at this node; otherwise <c>FALSE</c>.</param>
    ''' <returns>An integer representing the number of child nodes.</returns>
    ''' <remarks></remarks>
    Public Function GetNodeCount(ByVal aIncludeSubTrees As Boolean) As Integer
        If (aIncludeSubTrees) Then Return Me._VirtualParent.ChildrenCount

        Return Me.Nodes.Count
    End Function

    ''' <summary>
    ''' Gets the Prior node relative to the specified node.
    ''' </summary>
    ''' <param name="aNode">The Node whose prior node to retrieve.</param>
    ''' <returns>A TreeListNode; otherwise <c>NOTHING</c> if no previous node can be found.</returns>
    ''' <remarks></remarks>
    Protected Friend Function GetPreviousNode(ByVal aNode As TreeListNode) As TreeListNode
        If (aNode IsNot Nothing) Then
            If (aNode.PreviousSiblingNode Is Nothing AndAlso aNode.ParentNode IsNot Nothing) Then
                Return aNode.ParentNode
            ElseIf (aNode.PreviousSiblingNode IsNot Nothing) Then
                Dim TempNode As TreeListNode = aNode.PreviousSiblingNode

                If (TempNode.Nodes.Count > 0 AndAlso TempNode.IsExpanded) Then
                    Do
                        TempNode = TempNode.LastChildNode
                        If (Not TempNode.IsExpanded OrElse TempNode.Nodes.Count = 0) Then
                            Return TempNode
                        End If
                    Loop While (TempNode.Nodes.Count > 0 AndAlso TempNode.IsExpanded)
                Else
                    Return aNode.PreviousSiblingNode
                End If
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Overriden.  Occurs when the ScrollBars need to be setup and displayed by the control.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnAdjustScrollBars()
        Me._adjustScrollBars()
    End Sub

    ''' <summary>
    ''' Raises the AfterCollapse event.
    ''' </summary>
    ''' <param name="e">A TreeListViewEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterCollapse(ByVal e As TreeListViewEventArgs)
        RaiseEvent AfterCollapse(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the AfterExpand event.
    ''' </summary>
    ''' <param name="e">A TreeListViewEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterExpand(ByVal e As TreeListViewEventArgs)
        RaiseEvent AfterExpand(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the BeforeCollapse event.
    ''' </summary>
    ''' <param name="e">A TreeListViewCancelEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeCollapse(ByVal e As TreeListViewCancelEventArgs)
        RaiseEvent BeforeCollapse(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the BeforeExpand event.
    ''' </summary>
    ''' <param name="e">A TreeListViewCancelEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeExpand(ByVal e As TreeListViewCancelEventArgs)
        RaiseEvent BeforeExpand(Me, e)
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the DragEnter event.
    ''' </summary>
    ''' <param name="drgevent">A DragEventArgs that contains the event data. </param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnDragEnter(ByVal drgevent As DragEventArgs)
        If (Me._DefaultDragDrop AndAlso Me._InDrag) Then
            DragHelpers.ImageList_DragEnter(Me.Handle, drgevent.X - Me.Left, drgevent.Y - Me.Top)
        Else
            MyBase.OnDragEnter(drgevent)
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the DragDrop event.
    ''' </summary>
    ''' <param name="drgevent">A DragEventArgs that contains the event data. </param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnDragDrop(ByVal drgevent As DragEventArgs)
        'SET THE MULTISELECT MODE BACK TO SINGLE
        Me._MultiSelectMode = MultiSelectModes.Single

        'TAKE APPROPRIATE PATH
        If (Me._DefaultDragDrop AndAlso Me._InDrag) Then
            Me._customDragDrop(drgevent)
        Else
            MyBase.OnDragDrop(drgevent)
        End If

        'RESET THE DRAG VARIABLES
        Me._InDrag = False
        Me._DragArg = Nothing
        Me._TempDropNode = Nothing
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the DragEnter event.
    ''' </summary>
    ''' <param name="e">The EventArg containing the data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnDragLeave(ByVal e As EventArgs)
        If (Me._DefaultDragDrop AndAlso Me._InDrag) Then
            DragHelpers.ImageList_DragLeave(Me.Handle)
        Else
            MyBase.OnDragLeave(e)
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the DragOver event.
    ''' </summary>
    ''' <param name="drgevent">A DragEventArgs that contains the event data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnDragOver(ByVal drgevent As DragEventArgs)
        Dim DropNde As TreeListNode
        Dim Client As Point = Me.PointToClient(New Point(drgevent.X, drgevent.Y))

        'FIRE THE ITEMDRAG EVENT FIRST HERE IF THIS IS THE FIRST TIME WE HAVE HIT THIS ON A NEW DRAG
        'AND THE MOUSE POINTER HAS MOVED MORE THAN 4 PIXELS IN ANY DIRECTION
        If (Me._FireItemDrag) Then
            Dim X_Diff As Integer = Math.Abs(Me._DragArg.MousePosition.X - Client.X)
            Dim Y_Diff As Integer = Math.Abs(Me._DragArg.MousePosition.Y - Client.Y)

            If (X_Diff > 4 OrElse Y_Diff > 4) Then
                Me._FireItemDrag = False
                Me.OnItemDrag(Me._DragArg)

                'IF THE DRAG WAS CANCELLED THEN CALL ONQUERY.. TO CANCEL THE DRAG
                If (Me._DragArg.Cancel) Then
                    Me.OnQueryContinueDrag(New QueryContinueDragEventArgs(drgevent.KeyState, True, DragAction.Cancel))
                End If

                'GO NO FURTHER
                Exit Sub
            End If
        Else
            Me._FireItemDrag = False
        End If

        'SCROLL IF NEEDED
        DropNde = DirectCast(Me.GetItemAt(Client), TreeListNode)
        If (DropNde IsNot Nothing) Then
            Dim HScrollSize, Val As Integer

            'ACCOUNT FOR THE HSCROLL IF IT IS VISIBLE
            If (Me.HScroll.Visible) Then HScrollSize = Me.HScroll.Height

            'IF WE ARE NEAR THE BOTTOM, SCROLL DOWN
            If (DropNde.Bounds.Y > (Me.Height - 30 - HScrollSize)) Then
                Val = Me.VScroll.Value + Me._DragScroll

                If (Val > Me.VScroll.Maximum) Then Me.VScroll.Value = Me.VScroll.Maximum Else Me.VScroll.Value = Val
            ElseIf (DropNde.Bounds.Y < 30) Then
                Val = Me.VScroll.Value - Me._DragScroll

                If (Val < Me.VScroll.Minimum) Then Me.VScroll.Value = Me.VScroll.Minimum Else Me.VScroll.Value = Val
            End If
        End If

        'NOW DETERMINE WHICH PATH TO FOLLOW
        If (Me._DefaultDragDrop AndAlso Me._InDrag) Then
            Me._customDragOver(drgevent)
        Else
            MyBase.OnDragOver(drgevent)
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Draws the Rows of the control.
    ''' </summary>
    ''' <param name="aGr">A Graphics object to draw with.</param>
    ''' <param name="Rect">A Rectangle to draw in.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnDrawRows(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Not Me.InUpdateMode AndAlso Me._Nodes.Count > 0 AndAlso Me.Columns.Count > 0) Then
            Dim NodeDraw As TreeListNode
            Dim MaxRend, RenderedSoFar As Integer

            'DETERMINE THE TOTAL # OF NODES THAT CAN BE RENDERED IN THE CLIENT RECTANGLE
            If (Me.HScroll.Visible) Then
                MaxRend = CType(Math.Floor((Me.ClientRectangle.Height - Me.HeaderBuffer - Me.HScroll.Height) / Me.RowHeight), Integer)
            Else
                MaxRend = CType(Math.Floor((Me.ClientRectangle.Height - Me.HeaderBuffer) / Me.RowHeight), Integer)
            End If

            'GET THE FIRST NODE TO DRAW
            NodeDraw = Me._findFirstNode(Me.Nodes(0), Me.VScroll.Value)

            'CLEAR EXISTING DATA
            Me._TotalRend = 0
            Me._NodeRowRects.Clear()
            Me._PlusMinusRects.Clear()
            MyBase._CheckBoxRects.Clear()
            Me._ImageRects.Clear()

            'START DRAWING THE ROWS
            While (NodeDraw IsNot Nothing AndAlso MaxRend > RenderedSoFar)
                Me._renderNodeRows(NodeDraw, aGr, Rect, RenderedSoFar)
                RenderedSoFar += 1
                Me._TotalRend += 1
                NodeDraw = Me.GetNextNode(NodeDraw)
            End While

            'REFRESH THE OBJECT IN AN EDIT STATE
            If (MyBase.EditedObject IsNot Nothing) Then MyBase.EditedObject.RefreshEditing()

            'RENDER THE COLUMN GRIDLINES
            Me.OnDrawColumnGridLines(aGr)

            Me.OnAdjustScrollBars()
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the GiveFeedback event.
    ''' </summary>
    ''' <param name="gfbevent">A GiveFeedbackEventArgs that contains the event data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnGiveFeedback(ByVal gfbevent As GiveFeedbackEventArgs)
        If (Me._DefaultDragDrop AndAlso Me._InDrag) Then
            If (gfbevent.Effect = DragDropEffects.Move) Then
                gfbevent.UseDefaultCursors = False
                Me.Cursor = Cursors.Default
            Else
                gfbevent.UseDefaultCursors = True
            End If
        Else
            MyBase.OnGiveFeedback(gfbevent)
        End If
    End Sub

    ''' <summary>
    ''' Raises the ItemDrag event.
    ''' </summary>
    ''' <param name="e">The TreeListViewItemDragEventArg containing the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnItemDrag(ByVal e As TreeListViewItemDragEventArgs)
        RaiseEvent ItemDrag(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the ItemDrag event.
    ''' </summary>
    ''' <param name="e">The TreeListViewItemDropEventArgs containing the data.</param>
    ''' <remarks>
    ''' This method will NOT fire if property AllowDefaultDragDrop=<c>FALSE</c>.  This is only a convenience method for the developer so 
    ''' they don't have to handle the normal OnDragDrop event during drag operations.  Also, it will only fire if the user is 
    ''' dragging nodes from within the control itself.  If you are expecting any outside dragging onto this control, you will have 
    ''' to handle the normal OnDragDrop method.
    ''' </remarks>
    Protected Overridable Sub OnItemDrop(ByVal e As TreeListViewItemDropEventArgs)
        RaiseEvent ItemDrop(Me, e)
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the WinControls.TreeListview.ContainerListView.KeyDown event.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs containing the data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Left, Keys.Right
                MyBase.OnCheckShiftState(e)
                Me.OnLeftRightKeys(e)

            Case Keys.Space
                If (Me.CheckBoxes) Then Me.OnSpaceBarKey(e)

            Case Else
                MyBase.OnKeyDown(e)
        End Select
    End Sub

    ''' <summary>
    ''' Handles the Left or Right arrow KeyPress of the control which either expand or collapse the current node.
    ''' </summary>
    ''' <param name="e">A KeyEventArg containing the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnLeftRightKeys(ByVal e As KeyEventArgs)
        If (Me._Nodes.Count > 0 AndAlso Me._CurNode IsNot Nothing) Then
            Select Case e.KeyCode
                Case Keys.Left 'COLLAPSE CURRENT NODE
                    If (Me._CurNode.IsExpanded) Then
                        Me._CurNode.Collapse()
                        'Me._adjustScrollBars()
                    ElseIf (Me._CurNode.ParentNode IsNot Nothing) Then
                        Me._CurNode = Me._CurNode.ParentNode
                    End If

                Case Keys.Right 'EXPAND CURRENT NODE
                    If (Not Me._CurNode.IsExpanded) Then
                        Me._CurNode.Expand()
                    ElseIf (Me._CurNode.FirstChildNode IsNot Nothing) Then
                        Me._CurNode = Me._CurNode.FirstChildNode
                    End If
            End Select

            Me._showSelectedItems()
            e.Handled = True
            Me.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the MouseDown event.
    ''' </summary>
    ''' <param name="e">A MouseEventArgs that contains the event data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        'SET THIS VARIABLE BEFORE THE BASE CALL
        MyBase._ProcessRows = False
        MyBase.OnMouseDown(e)

        'DO THE BODY
        If (MyBase._ProcessRows) Then
            If (Me.RowsRectangle.Contains(e.X, e.Y)) Then
                If (e.Button = Windows.Forms.MouseButtons.Left OrElse (Me.RightMouseSelects AndAlso e.Button = Windows.Forms.MouseButtons.Right)) Then
                    Dim Node As TreeListNode = Nothing
                    Dim TestObj As Object = Nothing

                    'VARIABLE 'Node' IS BEING PASSED IN BYREF HERE
                    If (Me._nodeRowClicked(e, Node) OrElse Me._imageClicked(e, Node)) Then
                        Select Case MyBase._MultiSelectMode
                            Case MultiSelectModes.Single
                                Me.SelectedItems.Clear()

                                Me._CurNode = Node
                                Me._CurNode.Focused = True
                                Me._CurNode.Selected = True

                                If (e.Clicks = 2) Then Me._CurNode.Toggle()
                                Me._showSelectedItems()

                            Case MultiSelectModes.Range
                                Me.SelectedItems.Clear()
                                Me._CurNode = Node
                                Me._showSelectedItems()

                            Case MultiSelectModes.Selective
                                If (Node.Selected) Then
                                    Node.Focused = False
                                    Node.Selected = False
                                Else
                                    Node.Focused = True
                                    Node.Selected = True
                                    Me._CurNode = Node
                                End If
                        End Select

                        'ACTIVATE THE ITEM(S) IF PERMITTED
                        If (Not Me.MultiSelect OrElse (Me.MultiSelect AndAlso Me.AllowMultiSelectActivation)) Then
                            If (e.Clicks = 1 AndAlso Me.ActivationType = ItemActivation.OneClick) OrElse (e.Clicks = 2 AndAlso Me.ActivationType = ItemActivation.TwoClick) Then
                                Me.OnItemActivate(EventArgs.Empty)
                            End If
                        End If

                        'PREPARE FOR TREELISTNODE DRAG OPERATIONS IF NECESSARY
                        If (Node IsNot Nothing AndAlso Me.AllowDrop) Then
                            If (e.Button = Windows.Forms.MouseButtons.Left AndAlso e.Clicks = 1) Then Me._customBeginDrag(e)
                        End If

                    ElseIf (Me.PlusMinusClicked(e, Node)) Then
                        Node.Toggle()

                    ElseIf (Me.CheckBoxClicked(MyBase._CheckBoxRects, e, TestObj)) Then
                        If (e.Button = Windows.Forms.MouseButtons.Left) AndAlso
                           ((e.Clicks = 2 AndAlso (Me.CheckBoxSelection = ItemActivation.Standard OrElse Me.CheckBoxSelection = ItemActivation.TwoClick)) OrElse
                             e.Clicks = 1 AndAlso (Me.CheckBoxSelection = ItemActivation.OneClick)) Then
                            Node = DirectCast(TestObj, TreeListNode)

                            If (Node.CheckBoxEnabled) Then Node.Checked = Not Node.Checked
                        End If
                    End If
                End If
            Else
                Me.SelectedItems.Clear()
            End If

            Me.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the MouseLeave event.
    ''' </summary>
    ''' <param name="e">A System.EventArgs that contains the Data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)

        If (Me._LastNodeHovered IsNot Nothing) Then
            Me._LastNodeHovered.Hovered = False
            Me.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the MouseMove event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If (Me._Nodes.Count > 0) Then
            Dim Node As TreeListNode = Nothing

            If (MyBase.RowsRectangle.Contains(e.X, e.Y)) AndAlso ((Me._nodeRowClicked(e, Node)) OrElse
               (Me.CheckBoxClicked(MyBase._CheckBoxRects, e, DirectCast(Node, Object))) OrElse (Me._imageClicked(e, Node)) OrElse (Me.PlusMinusClicked(e, Node))) Then

                If (Not Me._LastNodeHovered Is Node) Then
                    If (Me._LastNodeHovered IsNot Nothing) Then Me._LastNodeHovered.Hovered = False
                    Me._LastNodeHovered = Node
                    If (Node IsNot Nothing) Then Node.Hovered = True
                End If
            Else
                If (Me._LastNodeHovered IsNot Nothing) Then Me._LastNodeHovered.Hovered = False
                Me._LastNodeHovered = Nothing
            End If

            Me.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Occurs when a TreeListNode in the collection has been added, removed, or changed.
    ''' </summary>
    ''' <param name="e">a TreeListViewEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnNodesChanged(ByVal e As TreeListViewEventArgs)
        Me._adjustScrollBars()
    End Sub

    ''' <summary>
    ''' Overriden.  Occurs when the PageUp, PageDown, Home, or End Keys are pressed.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnPageKeys(ByVal e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Home
                If (Me.VScroll.Visible) Then Me.VScroll.Value = 0
                If (Me.HScroll.Visible) Then Me.HScroll.Value = 0
                Me._moveToIndex(0)

            Case Keys.End
                If (Me.VScroll.Visible) Then Me.VScroll.Value = Me.VScroll.Maximum - Me.VScroll.LargeChange
                Me._moveToIndex(Me._Nodes.Count - 1)

            Case Keys.PageUp
                If (Me.VScroll.Visible) Then
                    If (Me.VScroll.LargeChange > Me.VScroll.Value) Then
                        Me.VScroll.Value = 0
                    Else
                        Me.VScroll.Value = Me.VScroll.Value - Me.VScroll.LargeChange
                    End If
                    Me._moveToIndex(CType(Math.Round(Me.VScroll.Value / Me.RowHeight), Integer))
                Else
                    Me._moveToIndex(0)
                End If

            Case Keys.PageDown
                If (Me.VScroll.Visible) Then
                    Dim Diff As Integer = 0

                    If (Me.HScroll.Visible) Then Diff = 2 Else Diff = 3

                    If ((Me.VScroll.Value + Me.VScroll.LargeChange) > (Me.VScroll.Maximum - Me.VScroll.LargeChange)) Then
                        Me.VScroll.Value = Me.VScroll.Maximum - Me.VScroll.LargeChange
                    Else
                        Me.VScroll.Value = Me.VScroll.Value + Me.VScroll.LargeChange - Me.VScroll.SmallChange
                    End If
                    Me._moveToIndex((CType(Math.Round(Me.VScroll.Value / Me.RowHeight), Integer) + CType(Math.Round((Me.VScroll.LargeChange / Me.RowHeight)), Integer) - Diff))
                Else
                    Me._moveToIndex(Me._Nodes.Count - 1)
                End If
        End Select
        e.Handled = True
    End Sub

    ''' <summary>
    ''' Overriden.  Occurs on the MouseDown when the control is checking to see if a ColumnHeader was clicked.
    ''' </summary>
    ''' <param name="aColIndex">The Index of the ColumnHeader that is currently being checked/processed.</param>
    ''' <param name="aTwid">A variable being used to check the width of the current subitem string.</param>
    ''' <param name="aMwid">A variable being used to keep track of the largest aTwid (the largest string processed).</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnProcessColumnMouseDownItems(ByVal aColIndex As Integer, ByRef aTwid As Integer, ByVal aMwid As Integer)
        Me._autoSetColWidth(Me._Nodes, aMwid, aTwid, aColIndex)
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the System.Windows.Forms.Control.QueryContinueDrag event.
    ''' </summary>
    ''' <param name="qcdevent">A System.Windows.Forms.QueryContinueDragEventArgs that contains the event data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnQueryContinueDrag(ByVal qcdevent As QueryContinueDragEventArgs)
        If (qcdevent.Action = DragAction.Cancel OrElse qcdevent.EscapePressed) Then
            'RESET THE DRAG VARIABLES
            Me._InDrag = False
            Me._DragArg = Nothing
            Me._TempDropNode = Nothing
            MyBase._MultiSelectMode = ContainerListView.MultiSelectModes.Single
        End If

        MyBase.OnQueryContinueDrag(qcdevent)
    End Sub

    ''' <summary>
    ''' Overriden.  Occurs when the control resizes.
    ''' </summary>
    ''' <param name="e">an EventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)

        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Overriden.  Sets focus to the selected TreeListNode.
    ''' </summary>
    ''' <param name="aClObj">The TreeListNode to set focus to.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnSetFocusedObject(ByVal aClObj As ContainerListViewObject)
        If (Me._FirstSelectedNode IsNot Nothing) Then Me._FirstSelectedNode.Focused = False
        Me._FirstSelectedNode = DirectCast(aClObj, TreeListNode)
    End Sub

    ''' <summary>
    ''' Overriden.  Occurs when the control needs to be sorted.
    ''' </summary>
    ''' <param name="aIndex">The zero-based index of the column to sort on.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnSort(ByVal aIndex As Integer)
        'SORT THE ROOT NODES FIRST
        Me._Nodes.Sort()

        'NOW GO THRU ALL THE CHILDREN
        'For Each Nde As TreeListNode In Me._Nodes
        '   Me._recursiveSort(Nde.Nodes)
        'Next
    End Sub

    ''' <summary>
    ''' Overriden.  Handles the SpaceBar key which Checks/UnChecks checkboxes on the selected TreeListNode(s).
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnSpaceBarKey(ByVal e As KeyEventArgs)
        If (e.Shift) Then
            MyBase.OnSpaceBarKey(e)
            Exit Sub
        Else
            Me.SelectedItems.Clear()
            Me._FirstSelectedNode.Selected = True
            If (Me._FirstSelectedNode.CheckBoxVisible AndAlso Me._FirstSelectedNode.CheckBoxEnabled) Then Me._FirstSelectedNode.Checked = (Not Me._FirstSelectedNode.Checked)
        End If

        e.Handled = True
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Occurs when the Up or Down Keys are pressed.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnUpDownKeys(ByVal e As KeyEventArgs)
        Dim TestNode As TreeListNode = Nothing

        Select Case e.KeyCode
            Case Keys.Up
                TestNode = Me.GetPreviousNode(Me._CurNode)

            Case Keys.Down
                TestNode = Me.GetNextNode(Me._CurNode)
        End Select

        If (TestNode IsNot Nothing AndAlso Not TestNode Is Me._CurNode) Then
            Me._CurNode = TestNode

            If (MyBase._MultiSelectMode = MultiSelectModes.Single) Then
                Me.SelectedItems.Clear()
                Me._CurNode.Selected = True
                Me._CurNode.Focused = True
            End If

            Me._showSelectedItems()
            Me.Invalidate()
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Determines if Plus or Minus was clicked.
    ''' </summary>
    ''' <param name="e">The MouseEventArg containing the data.</param>
    ''' <param name="aNode">A Node (can be <c>NOTHING</c>) to assist in evaluation.</param>
    ''' <returns><c>TRUE</c> if a Plus or Minus was clicked; otherwise <c>FALSE</c>.</returns>
    ''' <remarks>aNode will be set to the Node the plus or minus was clicked on.</remarks>
    Protected Friend Function PlusMinusClicked(ByVal e As MouseEventArgs, ByRef aNode As TreeListNode) As Boolean
        If (Me._PlusMinusRects IsNot Nothing) Then
            aNode = DirectCast(Tools.EvaluateObject(e, Me._PlusMinusRects.Keys.GetEnumerator, Me._PlusMinusRects.Values.GetEnumerator), TreeListNode)

            If (aNode IsNot Nothing) Then Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Overriden.  Selects all the Nodes in the control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub SelectAll()
        If (Me.MultiSelect) Then
            Me.BeginUpdate()
            For Each Node As TreeListNode In Me._Nodes
                Me._selectAll(Node)
            Next
            Me.EndUpdate()
        End If
    End Sub

    ''' <summary>
    ''' Calls the OnAfterCollapse method.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode calling the method.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Friend Sub TreeListViewAfterCollapse(ByVal aNode As TreeListNode)
        If (aNode IsNot Nothing) Then Me.OnAfterCollapse(New TreeListViewEventArgs(aNode, TreeViewAction.Collapse))
    End Sub

    ''' <summary>
    ''' Calls the OnAfterExpand method.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode calling the method.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Friend Sub TreeListViewAfterExpand(ByVal aNode As TreeListNode)
        If (aNode IsNot Nothing) Then Me.OnAfterExpand(New TreeListViewEventArgs(aNode, TreeViewAction.Expand))
    End Sub

    ''' <summary>
    ''' Calls the OnBeforeCollapse method.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode calling the method.</param>
    ''' <returns><c>TRUE</c> if the call to the 'OnAfter' equivalent method should be cancelled; othewise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Friend Function TreeListViewBeforeCollapse(ByVal aNode As TreeListNode) As Boolean
        If (aNode IsNot Nothing) Then
            Dim Arg As New TreeListViewCancelEventArgs(aNode, False, TreeViewAction.Collapse)
            Me.OnBeforeCollapse(Arg)

            Return Arg.Cancel
        End If

        Return True
    End Function

    ''' <summary>
    ''' Calls the OnBeforeExpand method.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode calling the method.</param>
    ''' <returns><c>TRUE</c> if the call to the 'OnAfter' equivalent method should be cancelled; othewise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Friend Function TreeListViewBeforeExpand(ByVal aNode As TreeListNode) As Boolean
        If (aNode IsNot Nothing) Then
            Dim Arg As New TreeListViewCancelEventArgs(aNode, False, TreeViewAction.Expand)
            Me.OnBeforeExpand(Arg)

            Return Arg.Cancel
        End If

        Return True
    End Function

#End Region

#Region " Procedures "

    Private Sub _adjustScrollBars()
        If (Me._Nodes IsNot Nothing AndAlso (Me._Nodes.Count > 0 OrElse (Me.Columns.Count > 0 AndAlso Not MyBase._ColScaleMode))) Then
            Dim ShowVert, ShowHoriz As Boolean
            Dim VertVar1, HorizVar1 As Integer
            Dim ColWdths As Integer = MyBase.GetSumOfVisibleColumnWidths
            Dim Rect As Rectangle = Me.ClientRectangle
            Dim RowsHeight As Integer = (Me.RowHeight * (Me._VirtualParent.VisibleChildren)) + (Me.RowHeight * 2)

            'SET SOME VARIABLES
            VertVar1 = (Rect.Height - Me.HScroll.Height - 2)
            HorizVar1 = (Rect.Width - Me.VScroll.Width - 2)

            'SET SOME UNAFFECTED PROPERTIES 0F THE VSCROLL
            With Me.VScroll
                .Left = (Rect.Left + Rect.Width - .Width - 2)
                .Top = Rect.Top + 2
                .SmallChange = Me.RowHeight
                .Maximum = RowsHeight
            End With

            'SET SOME UNAFFECTED PROPERTIES 0F THE HSCROLL
            With Me.HScroll
                .Left = Rect.Left + 2
                .Top = Rect.Top + Rect.Height - .Height - 2
                Try
                    If (Me.Columns.Count > 0) Then .SmallChange = ColWdths \ Me.Columns.Count Else .SmallChange = 0
                Catch ex As Exception
                    .SmallChange = 0
                End Try
                .Maximum = ColWdths
            End With

            'DO SOME CHECKS
            If (RowsHeight > VertVar1) Then ShowVert = True Else ShowVert = False
            If (ColWdths > HorizVar1) Then ShowHoriz = True Else ShowHoriz = False
            If (Not Me.Scrollable) Then
                ShowVert = False
                ShowHoriz = False
            End If

            'SET SCROLL SPECIFICS
            If (ShowVert AndAlso ShowHoriz) Then
                Me.VScroll.Height = VertVar1
                If (VertVar1 > 0) Then Me.VScroll.LargeChange = Me.EnsureVerticalScroll(VertVar1, True) Else Me.VScroll.LargeChange = 0

                Me.HScroll.Width = HorizVar1
                If (HorizVar1 > 0) Then Me.HScroll.LargeChange = HorizVar1 Else Me.HScroll.LargeChange = 0

                If (Me.Scrollable) Then
                    Me.VScroll.Show()
                    Me.HScroll.Show()
                End If
            ElseIf (ShowVert AndAlso Not ShowHoriz) Then
                Me.HScroll.Hide()
                Me.HScroll.Value = 0
                If (HorizVar1 > 0) Then Me.HScroll.LargeChange = HorizVar1 Else Me.HScroll.LargeChange = 0

                Me.VScroll.Height = VertVar1 + Me.HScroll.Height - 2
                If (VertVar1 > 0) Then Me.VScroll.LargeChange = Me.EnsureVerticalScroll(VertVar1, False) Else Me.VScroll.LargeChange = 0
                If (Me.Scrollable) Then Me.VScroll.Show()
            ElseIf (ShowHoriz AndAlso Not ShowVert) Then
                Me.VScroll.Hide()
                Me.VScroll.Value = 0
                If (VertVar1 > 0) Then Me.VScroll.LargeChange = Me.EnsureVerticalScroll(VertVar1, True) Else Me.VScroll.LargeChange = 0

                Me.HScroll.Width = HorizVar1 + Me.VScroll.Width
                If (HorizVar1 > 0) Then Me.HScroll.LargeChange = HorizVar1 + Me.VScroll.Width Else Me.HScroll.LargeChange = 0
                If (Me.Scrollable) Then Me.HScroll.Show()
            Else
                Me.VScroll.Hide()
                Me.VScroll.Value = 0
                If (VertVar1 > 0) Then Me.VScroll.LargeChange = Me.EnsureVerticalScroll(VertVar1, False) Else Me.VScroll.LargeChange = 0

                Me.HScroll.Hide()
                Me.HScroll.Value = 0
                If (HorizVar1 > 0) Then Me.HScroll.LargeChange = HorizVar1 Else Me.HScroll.LargeChange = 0
            End If
        End If
    End Sub

    Private Sub _autoSetColWidth(ByVal aNodeColl As TreeListNodeCollection, ByRef aMwid As Integer, ByRef aTwid As Integer, ByVal aColIndex As Integer)
        Dim Node As TreeListNode

        For J As Integer = 0 To (aNodeColl.Count - 1)
            Node = aNodeColl(J)

            If (aColIndex > 0 AndAlso aColIndex <= Node.SubItems.Count) Then
                Dim ThisFont As Font

                If (Node.UseItemStyleForSubItems) Then ThisFont = Node.Font Else ThisFont = Node.SubItems(aColIndex - 1).Font
                aTwid = Tools.GetStringWidth(Node.SubItems(aColIndex - 1).Text.ToUpper, ThisFont) + 4
            ElseIf (aColIndex = 0) Then
                aTwid = Tools.GetStringWidth(Node.Text.ToUpper, Node.Font) + (Node.Bounds.Left - Node.RootNode.Bounds.Left) + 4
            Else
                aTwid = Tools.GetStringWidth(Node.Text.ToUpper, Node.Font) + 4
            End If

            If (aTwid > aMwid) Then aMwid = aTwid
            If (Node.IsExpanded AndAlso Node.Nodes.Count > 0) Then Me._autoSetColWidth(Node.Nodes, aMwid, aTwid, aColIndex)
        Next
    End Sub

    Private Sub _baseFontChanged(ByVal aNode As TreeListNode)
        aNode.ChangeGlobalFont(Me.Font)

        'NOW DO THE SUBITEMS
        For Each SU As ContainerListViewSubItem In aNode.SubItems
            SU.ChangeGlobalFont(Me.Font)
        Next

        'NOW DO THE CHILD NODES
        For Each Nde As TreeListNode In aNode.Nodes
            Me._baseFontChanged(Nde)
        Next
    End Sub

    Private Sub _baseForeColorChanged(ByVal aNode As TreeListNode)
        aNode.ChangeGlobalForeColor(Me.ForeColor)

        'NOW DO THE SUBITEMS
        For Each SU As ContainerListViewSubItem In aNode.SubItems
            SU.ChangeGlobalForeColor(Me.ForeColor)
        Next

        'NOW DO THE CHILD NODES
        For Each Nde As TreeListNode In aNode.Nodes
            Me._baseForeColorChanged(Nde)
        Next
    End Sub

    Private Sub _customBeginDrag(ByVal e As MouseEventArgs)
        If (Me.SelectedItems.Count > 0) Then
            Dim Bm As Bitmap
            Dim X_OffSet As Integer = 18
            Dim Gr As Graphics
            Dim Ht, MaxNodes As Integer
            Dim DrawFooter As Boolean = False
            Dim MaxHeight As Integer = 256
            Dim NdeArr(Me.SelectedItems.Count - 1) As TreeListNode

            'COPY THE NODES
            Me.SelectedItems.CopyTo(NdeArr, 0)

            'GET THE HEIGHT AND WIDTH OF THE DRAWN IMAGE (IMAGES CAN'T BE LARGER THAN 256 IN HEIGHT)
            Ht = Me.RowHeight * (Me.SelectedItems.Count + 1)
            If (Ht > MaxHeight) Then
                Ht = MaxHeight
                DrawFooter = True
                MaxNodes = CType(Math.Floor((Ht - (Me.RowHeight * 2)) / Me.RowHeight), Integer)
            Else
                MaxNodes = CType(Math.Floor((Ht - Me.RowHeight) / Me.RowHeight), Integer)
            End If

            'CREATE THE GRAPHICS OBJECT
            Gr = Me.CreateGraphics

            'CREATE THE IMAGE AND GRAPHICS OBJECT
            Bm = New Bitmap(MaxHeight, Ht)
            Gr = Graphics.FromImage(Bm)

            'CREATE THE DRAGIMAGELIST IF NOT ALREADY
            If (Me._DragImgList Is Nothing) Then
                Me._DragImgList = New ImageList
                Me._DragImgList.ColorDepth = ColorDepth.Depth32Bit
            Else
                Me._DragImgList.Images.Clear()
            End If
            Me._DragImgList.ImageSize = New Size(MaxHeight, Ht)

            'DRAW EACH IMAGE TO DRAG
            For I As Integer = 0 To (MaxNodes - 1)
                Dim HasImage As Boolean = True
                Dim Img As Image = Nothing
                Dim Nde As TreeListNode = DirectCast(Me.SelectedItems(I), TreeListNode)

                'VERIFY THAT THE IMAGEINDEX EXISTS
                X_OffSet = 18
                Try
                    If (Nde.IsExpanded) Then
                        Img = Me.ImageList.Images(Nde.SelectedImageIndex)
                    Else
                        Img = Me.ImageList.Images(Nde.ImageIndex)
                    End If
                Catch ex As Exception
                    HasImage = False
                    X_OffSet = 0
                End Try

                'DRAW THE IMAGE
                If (HasImage) Then Gr.DrawImage(Img, 18, (I * Me.RowHeight) - 2)

                'DRAW THE TEXT INTO THE IMAGE
                Gr.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
                Gr.DrawString(Nde.Text, Me.Font, Brushes.Black, 18 + X_OffSet, (I * Me.RowHeight))
            Next

            'DRAW THE "MORE NODES" TEXT IF NECESSARY
            If (DrawFooter) Then
                Gr.DrawString(("+" & Me.SelectedItems.Count - MaxNodes).ToString & " more nodes...", Me.Font, Brushes.Blue,
                              18 + X_OffSet, Ht - (Me.RowHeight * 2))
            End If

            'ADD THE IMAGE
            Me._DragImgList.Images.Add(Bm)

            'BEGIN DRAG
            If (DragHelpers.ImageList_BeginDrag(Me._DragImgList.Handle, 0, 0, 0)) Then
                Me._InDrag = True
                Me._FireItemDrag = True
                Me._DragArg = New TreeListViewItemDragEventArgs(e.Button, Me.PointToClient(New Point(e.X, e.Y)), NdeArr, 0)
                Me.DoDragDrop(Bm, DragDropEffects.Move)
                DragHelpers.ImageList_EndDrag()
            End If
        End If
    End Sub

    Private Sub _customDragDrop(ByVal e As DragEventArgs)
        Dim Pt As Point = Me.PointToClient(New Point(e.X, e.Y))
        Dim TargetNde As TreeListNode = DirectCast(Me.GetItemAt(Pt), TreeListNode)

        DragHelpers.ImageList_DragLeave(Me.Handle)
        Me.OnItemDrop(New TreeListViewItemDropEventArgs(Me._DragArg.Button, Pt, Me._DragArg.Item, TargetNde, e.KeyState))
    End Sub

    Private Sub _customDragOver(ByVal e As DragEventArgs)
        Dim Pt As Point = Me.PointToClient(New Point(e.X, e.Y))
        Dim IsNodeBeingDragged As Boolean
        Dim DropNde As TreeListNode = DirectCast(Me.GetItemAt(Me.PointToClient(New Point(e.X, e.Y))), TreeListNode)

        'UNSELECT THE PREVIOUS TEMPORARY DROP NODE
        If (Me._TempDropNode IsNot Nothing AndAlso Not Me._TempDropNode Is DropNde) Then
            If (Array.IndexOf(Me._DragArg.Item, Me._TempDropNode) = -1) Then
                Me._TempDropNode.Focused = False
                Me._TempDropNode.Selected = False
            End If
            Me.Invalidate(Me._TempDropNode.CompleteBounds)
        End If

        'SET THE DATA
        Me._TempDropNode = DropNde

        'SET THE EFFECT BASED UPON OUR TEMPNODE
        DragHelpers.ImageList_DragMove(Pt.X + 7, Pt.Y + 22)
        If (Me._TempDropNode Is Nothing) Then
            e.Effect = DragDropEffects.None
            Exit Sub
        Else
            e.Effect = DragDropEffects.Move
            IsNodeBeingDragged = Array.IndexOf(Me._DragArg.Item, Me._TempDropNode) >= 0
        End If

        'FOCUS AND SELECT THE DROPNODE
        If (Not IsNodeBeingDragged) Then Me._TempDropNode.Selected = True
        Me._TempDropNode.Focused = True
        Me.Invalidate(Me._TempDropNode.CompleteBounds)

        'MAKE SURE THE DROPNODE IS NOT ONE OF THE NODES BEING DRAGGED AND ALSO
        'MAKE SURE THE DROPNDE IS NOT A CHILD SOMEWHERE OF ONE OF THE NODES BEING DRAGGED (WE CAN'T DROP A PARENT NODE INTO ONE OF IT'S CHILDREN OR ITSELF)
        If (IsNodeBeingDragged) Then
            e.Effect = DragDropEffects.None
        Else
            For Each TN As TreeListNode In Me._DragArg.Item
                If (TN.ParentNode Is Me._TempDropNode OrElse TN.IsNodeInBranch(Me._TempDropNode)) Then
                    e.Effect = DragDropEffects.None
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function _findFirstNode(ByVal aStartingNode As TreeListNode, ByVal aStartingPoint As Integer) As TreeListNode
        Dim KidsCount As Integer = aStartingNode.VisibleChildren

        If (Me.RowHeight * KidsCount >= aStartingPoint) Then
            Dim Nde As TreeListNode = aStartingNode

            'THE NODE IS SOMEWHERE IN THIS BRANCH
            For I As Integer = 1 To (aStartingPoint \ Me.RowHeight)
                Nde = Me.GetNextNode(Nde)
            Next

            Return Nde
        Else
            Return Me._findFirstNode(aStartingNode.NextSiblingNode, aStartingPoint - ((KidsCount + 1) * 16))
        End If
    End Function

    Private Function _getNodeBounds(ByVal aNode As TreeListNode) As Rectangle
        Dim NdeRect As Rectangle = Rectangle.Empty

        If (Me.Columns.Count > 0 AndAlso Not Me.Columns(0).Hidden) Then
            'SEE IF WE ALREADY HAVE THE RECTANGLE
            For Each RT As Rectangle In Me._NodeRowRects.Keys
                If (DirectCast(Me._NodeRowRects.Item(RT), TreeListNode) Is aNode) Then
                    NdeRect = New Rectangle(RT.X, RT.Y, Me.Columns(0).Width - RT.X, RT.Height)
                    Exit For
                End If
            Next

            'IF THE RECTANGLE IS EMPTY, THEN WE MUST CALCULATE THE BOUNDARY
            If (NdeRect.IsEmpty) Then
                Dim Rect As Rectangle
                Dim Index As Integer
                Dim Xpos, Ypos, Wdth As Integer
                Dim EdgeBuffer As Integer = 10
                Dim IconBuffer As Integer
                Dim CheckBuffer As Integer
                Dim LevelBuffer As Integer = Me._Indent * aNode.Level

                Rect = Me.ClientRectangle
                If (Me._ShowRootLines OrElse Me._ShowPlusMinus) Then EdgeBuffer += 10
                If (Me.CheckBoxes AndAlso aNode.CheckBoxVisible) Then CheckBuffer = 18
                If (aNode.Selected OrElse aNode.Focused) Then Index = aNode.SelectedImageIndex Else Index = aNode.ImageIndex
                If (Me.ImageList IsNot Nothing AndAlso Index >= 0 AndAlso Index < Me.ImageList.Images.Count) Then
                    IconBuffer = 18
                End If

                'NOW CALCULATE THE COORDINATES AND CREATE THE RECTANGLE
                Xpos = Rect.Left + LevelBuffer + CheckBuffer + IconBuffer + EdgeBuffer + 4 - Me.HScroll.Value
                Ypos = Rect.Top + Me.HeaderBuffer + (Me.RowHeight * Me._TotalRend) + 2
                Wdth = Me.Columns(0).Bounds.Width - Xpos '(LevelBuffer + CheckBuffer + IconBuffer + EdgeBuffer + 3)
                NdeRect = New Rectangle(Xpos, Ypos, Wdth, Me.RowHeight)
            End If
        End If

        Return NdeRect
    End Function

    Private Function _getNodeCompleteBounds(ByVal aNode As TreeListNode) As Rectangle
        Dim NdeRect As Rectangle = Rectangle.Empty

        'SEE IF WE ALREADY HAVE THE RECTANGLE
        For Each RT As Rectangle In Me._NodeRowRects.Keys
            If (DirectCast(Me._NodeRowRects.Item(RT), TreeListNode) Is aNode) Then
                NdeRect = RT
                Exit For
            End If
        Next

        'IF THE RECTANGLE IS EMPTY, THEN WE MUST CALCULATE THE BOUNDARY
        If (NdeRect.IsEmpty) Then
            Dim Sum As Integer = Me.GetSumOfVisibleColumnWidths

            If (Sum > 0) Then
                Dim Rect As Rectangle
                Dim Index As Integer
                Dim Xpos, Ypos, Wdth As Integer
                Dim EdgeBuffer As Integer = 10
                Dim IconBuffer As Integer
                Dim CheckBuffer As Integer
                Dim LevelBuffer As Integer = Me._Indent * aNode.Level

                Rect = Me.ClientRectangle
                If (Me._ShowRootLines OrElse Me._ShowPlusMinus) Then EdgeBuffer += 10
                If (Me.CheckBoxes AndAlso aNode.CheckBoxVisible) Then CheckBuffer = 18
                If (aNode.Selected OrElse aNode.Focused) Then Index = aNode.SelectedImageIndex Else Index = aNode.ImageIndex
                If (Me.ImageList IsNot Nothing AndAlso Index >= 0 AndAlso Index < Me.ImageList.Images.Count) Then
                    IconBuffer = 18
                End If

                'NOW CALCULATE THE COORDINATES AND CREATE THE RECTANGLE
                Xpos = Rect.Left + LevelBuffer + CheckBuffer + IconBuffer + EdgeBuffer + 4 - Me.HScroll.Value
                Ypos = Rect.Top + Me.HeaderBuffer + (Me.RowHeight * Me._TotalRend) + 2
                Wdth = Sum - Xpos '(LevelBuffer + CheckBuffer + IconBuffer + EdgeBuffer + 3)
                NdeRect = New Rectangle(Xpos, Ypos, Wdth, Me.RowHeight)
            End If
        End If

        Return NdeRect
    End Function

    Private Function _getTheNode(ByVal aNodes As TreeListNodeCollection, ByVal aText As String) As TreeListNode
        For Each Nde As TreeListNode In aNodes
            If (Nde.Text.Trim.ToLower = aText.Trim.ToLower) Then Return Nde
        Next

        Return Nothing
    End Function

    Private Function _imageClicked(ByVal e As MouseEventArgs, ByRef aNode As TreeListNode) As Boolean
        If (Me._ImageRects IsNot Nothing) Then
            aNode = DirectCast(Tools.EvaluateObject(e, Me._ImageRects.Keys.GetEnumerator, Me._ImageRects.Values.GetEnumerator), TreeListNode)
            If (aNode Is Nothing) Then Return False

            Return True
        End If

        Return False
    End Function

    Private Sub _makeSelectedVisible()
        If (Not Me._CurNode Is Nothing AndAlso Me.EnsureVisible) Then
            If (Me._CurNode.Selected) Then
                Dim Var1 As Integer
                Dim Rect As Rectangle = Me.ClientRectangle
                Dim Pos As Integer = Rect.Top + Me.HeaderBuffer + (Me.RowHeight * Me._CurNode.RowIndex()) + 2 - Me.VScroll.Value
                If (Me.HScroll.Visible) Then Var1 = Me.HScroll.Height Else Var1 = 0

                Try
                    If ((Pos + Me.RowHeight + Var1) > (Rect.Top + Rect.Height)) Then
                        Pos = Math.Abs((Rect.Top + Rect.Height) - (Pos + Me.RowHeight + Var1))
                        If (Pos > (Me.RowHeight / 2)) Then Pos = Me.RowHeight Else Pos = 0
                        Me.VScroll.Value += Pos
                    ElseIf (Pos < (Rect.Top + Me.HeaderBuffer + 2)) Then
                        Me.VScroll.Value -= Math.Abs(Rect.Top + Me.HeaderBuffer - Pos)
                    End If
                Catch ex As Exception
                    If (Me.VScroll.Value > Me.VScroll.Maximum) Then
                        Me.VScroll.Value = Me.VScroll.Maximum
                    ElseIf (Me.VScroll.Value < Me.VScroll.Minimum) Then
                        Me.VScroll.Value = Me.VScroll.Minimum
                    End If
                End Try
            End If
        End If
    End Sub
    Private Sub _moveToIndex(ByVal aIndex As Integer)
        If (aIndex < 0 OrElse aIndex >= Me._Nodes.Count) Then Exit Sub

        Me.SelectedItems.Clear()
        If (MyBase._MultiSelectMode = MultiSelectModes.Single) Then
            Dim Node As TreeListNode = Me.Nodes(aIndex)

            'LETS UNFOCUS FIRST
            Me._CurNode.Focused = False
            Me._FirstSelectedNode.Focused = False

            Me._CurNode = Node
            Me._FirstSelectedNode = Node
            Node.Selected = True
            Node.Focused = True
        End If

        Me._makeSelectedVisible()
        Me.Invalidate()
    End Sub

    Private Function _nodeRowClicked(ByVal e As MouseEventArgs, ByRef aNode As TreeListNode) As Boolean
        If (Not Me._NodeRowRects Is Nothing) Then
            aNode = DirectCast(Tools.EvaluateObject(e, Me._NodeRowRects.Keys.GetEnumerator, Me._NodeRowRects.Values.GetEnumerator), TreeListNode)
            If (aNode Is Nothing) Then Return False

            Return True
        End If

        Return False
    End Function

    Private Sub _preInit()
        'MISC 
        Me.RowHeight = 16
        Me.GridLines = ContainerListView.GridLineSelections.Column

        'CREATE THE COLLECTIONS
        Me._Nodes = Me._VirtualParent.Nodes

        'ADD THE DEFAULT IMAGES
        With Me._FolderImages.Images
            .Add(My.Resources.ClosedFolderYellow)
            .Add(My.Resources.OpenFolderYellow)
            .Add(My.Resources.ClosedFolderGray)
            .Add(My.Resources.OpenFolderGray)
            .Add(My.Resources.ClosedFolderBlue)
            .Add(My.Resources.OpenFolderBlue)
        End With
        Me.ImageList = Me._FolderImages

        'INITIALIZE THE IMAGES
        Me._MinusBitMap = My.Resources.Minus
        Me._PlusBitMap = My.Resources.Plus

        'INITIALIZE THE BRUSHES AND PEN
        _refreshNodeGriden()
    End Sub

    Private Sub _recursiveSort(ByVal aNdeColl As TreeListNodeCollection)
        'SORT THE PASSED IN COLLECTION
        aNdeColl.Sort()

        'NOW SORT ALL ITS CHILDREN
        For Each Nde As TreeListNode In aNdeColl
            Me._recursiveSort(Nde.Nodes)
        Next
    End Sub

    Private Sub _refreshNodeGriden()
        If Me._NodeGridLineStyle = NodeGridLineStyle.Custom Then Me._NodeGridLinePen = New Pen(Me._NodeGridLineColor, 1.0F)
        If Me._NodeGridLineStyle = NodeGridLineStyle.AsListView Then
            Me._NodeGridLinePen = New Pen(Color.FromArgb(Me.AlphaComponent, Me.GridLineColor.R, Me.GridLineColor.G, Me.GridLineColor.B), 1.0F)
        End If
        If Me._NodeGridLineStyle = NodeGridLineStyle.None Then Me._NodeGridLinePen = New Pen(Color.Empty, 1.0F)
    End Sub
    Private Sub _renderNodeRows(ByVal aNode As TreeListNode, ByVal aGr As Graphics, ByVal Rect As Rectangle, ByVal aTotalRend As Integer)
        Dim EdgeBuffer As Integer = 10
        Dim TempVar1 As Integer = Rect.Top + CType(Me.RowHeight * aTotalRend, Integer) + CType(EdgeBuffer / 4, Integer)

        'ONLY RENDER ROW IF VISIBLE TO USER
        If ((TempVar1 + Me.RowHeight) >= (Rect.Top + 2)) AndAlso (TempVar1 < (Rect.Top + Rect.Height)) Then
            Dim CheckBuffer, IconBuffer As Integer
            Dim ChildCount As Integer = 0
            Dim Count As Integer
            Dim HdrBuffer As Integer = Me.HeaderBuffer
            Dim Index, EdgeBuffer2 As Integer
            Dim LinePen As New Pen(Me.RootLineColor, 1.0F)
            Dim NodeIndex As Integer = aNode.Index
            Dim NodeLevel As Integer = aNode.Level
            Dim NodeRect As Rectangle = Me.GetCompleteItemBounds(aNode)
            Dim ItemRect As Rectangle = Me.GetItemBounds(aNode)
            Dim LevelBuffer As Integer = Me.IndentSize * NodeLevel
            Dim X, Y, X2, Y2 As Integer
            Dim SBrush As New SolidBrush(Me.DetermineBackGroundColor(aNode))
            Dim TheText As String = String.Empty
            Dim SelRowRect As Rectangle

            'MISC
            LinePen.DashStyle = DashStyle.Dot
            Me._RendCount += 1
            If (aNode.ParentNode Is Nothing) Then Count = Me._VirtualParent.Nodes.Count Else Count = aNode.ParentNode.Nodes.Count
            If (Not aNode.PreviousSiblingNode Is Nothing) Then ChildCount = aNode.PreviousSiblingNode.VisibleChildren
            If (Me.CheckBoxes AndAlso aNode.CheckBoxVisible) Then CheckBuffer = 18
            If (Me.ImageList IsNot Nothing) Then
                If (aNode.IsExpanded) Then Index = aNode.SelectedImageIndex Else Index = aNode.ImageIndex
                If (Index >= 0 AndAlso Index < Me.ImageList.Images.Count) Then IconBuffer = 18
            End If

            'ADD SPACE FOR ROOT LINES AND/OR PLUS-MINUS ICONS
            If (Me.ShowRootLines OrElse Me.ShowPlusMinus) Then EdgeBuffer += 10
            EdgeBuffer2 = EdgeBuffer \ 2

            'ADD THE RECTANGLE TO THE HASH TABLE (FOR EASY LOOKUPS LATER) AND THEN SET THE CLIP TO THAT RECTANGLE ONLY
            Me._NodeRowRects.Add(NodeRect, aNode)
            aGr.Clip = New Region(NodeRect)

            'SET THE SELECTED ROW RECTANGLE
            If (Not Me.FullRowSelect) Then
                SelRowRect = ItemRect
            ElseIf (NodeRect.Width < (Rect.Width - 4) OrElse Me.HScroll.Visible) Then
                SelRowRect = NodeRect
            Else
                SelRowRect = New Rectangle(NodeRect.X, NodeRect.Y, Rect.Width - 4, NodeRect.Height)
            End If

            'RENDER ITEM BACKGROUND
            aGr.FillRectangle(SBrush, ItemRect)

            'RENDER SELECTED ITEM
            If (Me.FullRowSelect AndAlso (aNode.Selected OrElse aNode.Hovered)) Then aGr.FillRectangle(SBrush, SelRowRect)

            'RENDER ROOTLINES IF VISIBLE
            aGr.Clip = New Region(New Rectangle(Rect.Left + 2 - Me.HScroll.Value, Rect.Top + HdrBuffer + 2, ItemRect.Right, Rect.Height - HeaderBuffer - 4))
            If ((Rect.Left + EdgeBuffer - Me.HScroll.Value) > Rect.Left) Then
                If (Me._ShowRootLines AndAlso NodeLevel = 0) Then
                    Dim HasMoreSiblings As Boolean = (Not aNode.NextSiblingNode Is Nothing)

                    X = Rect.Left + EdgeBuffer2 - Me.HScroll.Value
                    X2 = Rect.Left - Me.HScroll.Value

                    If (NodeIndex = 0) Then
                        Y = Rect.Top + EdgeBuffer2 + HdrBuffer
                        Y2 = Rect.Top + HdrBuffer

                        aGr.DrawLine(LinePen, X, Y, (X2 + EdgeBuffer), Y2 + EdgeBuffer2)
                        If (HasMoreSiblings) Then aGr.DrawLine(LinePen, X, Y, (X2 + EdgeBuffer2), Y2 + EdgeBuffer)
                    ElseIf (NodeIndex = (Count - 1)) Then
                        Y = Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend)

                        aGr.DrawLine(LinePen, X, Y + EdgeBuffer2, X2 + EdgeBuffer, Y + EdgeBuffer2)
                        aGr.DrawLine(LinePen, X, Y, X2 + EdgeBuffer2, Y + EdgeBuffer2)
                    Else
                        Y = Rect.Top + EdgeBuffer + HdrBuffer

                        aGr.DrawLine(LinePen, X, Y + (Me.RowHeight * aTotalRend) - EdgeBuffer2, X2 + EdgeBuffer, Y + (Me.RowHeight * aTotalRend) - EdgeBuffer2)
                        If (HasMoreSiblings) Then aGr.DrawLine(LinePen, X, Y + (Me.RowHeight * (aTotalRend - 1)), X2 + EdgeBuffer2, Y + (Me.RowHeight * aTotalRend))
                    End If

                    If (ChildCount > 0) Then
                        aGr.DrawLine(LinePen, X, Rect.Top + HdrBuffer + (Me.RowHeight * (aTotalRend - ChildCount)), X2 + EdgeBuffer2, Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend))
                    End If
                End If
            End If

            'RENDER CHILD LINES IF VISIBLE
            If ((Rect.Left + LevelBuffer + EdgeBuffer - Me.HScroll.Value) > Rect.Left) Then
                If (Me._ShowRootLines AndAlso (NodeLevel > 0)) Then
                    X = Rect.Left + LevelBuffer + EdgeBuffer2 - Me.HScroll.Value
                    X2 = Rect.Left + LevelBuffer - Me.HScroll.Value
                    Y = Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend) + 2

                    If (NodeIndex = (Count - 1)) Then
                        aGr.DrawLine(LinePen, X, Y - 2 + EdgeBuffer2, X2 + EdgeBuffer, Y - 2 + EdgeBuffer2)
                        aGr.DrawLine(LinePen, X, Y, X2 + EdgeBuffer2, Y + EdgeBuffer2)
                    Else
                        aGr.DrawLine(LinePen, X, Y - 2 + EdgeBuffer2, X2 + EdgeBuffer, Y - 2 + EdgeBuffer2)
                        aGr.DrawLine(LinePen, X, Y, X2 + EdgeBuffer2, Y + EdgeBuffer)
                    End If

                    If (ChildCount > 0) Then
                        aGr.DrawLine(LinePen, X, Rect.Top + HdrBuffer + (Me.RowHeight * (aTotalRend - ChildCount)), X2 + EdgeBuffer2, Y)
                    End If
                End If
            End If

            'RENDER PLUS/MINUS SIGNS IF VISIBLE
            If ((Rect.Left + LevelBuffer + EdgeBuffer2 + 5 - Me.HScroll.Value) > Rect.Left) Then
                If (Me._ShowPlusMinus AndAlso ((aNode.Nodes.Count > 0) OrElse Me._AlwaysShowPM)) Then
                    X = Rect.Left + LevelBuffer + EdgeBuffer2 - 4 - Me.HScroll.Value

                    If (NodeIndex = 0 AndAlso NodeLevel = 0) Then
                        Me._renderPlusMinus(aGr, X, Rect.Top + HdrBuffer + EdgeBuffer2 - 4, 8, 8, aNode)
                    Else
                        Me._renderPlusMinus(aGr, X, Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend) + EdgeBuffer2 - 4, 8, 8, aNode)
                    End If
                End If
            End If

            'DRAW THE CHECKBOX IF NECESSARY
            If (CheckBuffer <> 0) Then
                Dim CkBxRect As Rectangle

                'SET THE VARIABLES
                X = Rect.Left + LevelBuffer + EdgeBuffer + 2 - Me.HScroll.Value
                Y = CType(Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend) + (EdgeBuffer / 4) - 3, Integer)
                CkBxRect = New Rectangle(X, Y, 16, 16)

                'DRAW THE CHECKBOX AND ADD IT TO THE HASH
                Me.DrawObjectCheckBox(aGr, CkBxRect, aNode)
                MyBase._CheckBoxRects.Add(CkBxRect, aNode)
            End If

            'DRAW THE ICON IF AVAILABLE
            If (IconBuffer <> 0) Then
                X = Rect.Left + LevelBuffer + EdgeBuffer + CheckBuffer + 2 - Me.HScroll.Value
                Y = CType(Rect.Top + HdrBuffer + (Me.RowHeight * aTotalRend) + (EdgeBuffer / 4) - 3, Integer)

                aGr.DrawImage(Me.ImageList.Images(Index), X, Y, 16, 16)
                Me._ImageRects.Add(New Rectangle(X, Y, 16, 16), aNode)
            End If

            'RENDER TEXT IF VISIBLE
            TheText = Tools.TruncateString(aNode.Text, aNode.Font, Me.Columns(0).Width, (LevelBuffer + EdgeBuffer + IconBuffer + 4), aGr)
            aGr.DrawString(TheText, aNode.Font, New SolidBrush(aNode.ForeColor), NodeRect.X - 1, NodeRect.Y + 2)

            'RENDER SUBITEMS
            If (Me.Columns.Count > 0) Then
                For J As Integer = 0 To (aNode.SubItems.Count - 1)
                    If (J < (Me.Columns.Count - 1)) Then
                        Dim SubItem As ContainerListViewSubItem = aNode.SubItems(J)
                        Dim Brush As SolidBrush = New SolidBrush(Me.DetermineBackGroundColor(aNode, SubItem))
                        Dim SubItemAlign As HorizontalAlignment
                        Dim SIRect As Rectangle = SubItem.Bounds
                        Dim Offset As Integer = 0

                        'SET THE CLIP
                        aGr.Clip = New Region(SIRect)

                        'SET THE OFFSET
                        If (Me.GridLines = ContainerListView.GridLineSelections.Both OrElse Me.GridLines = ContainerListView.GridLineSelections.Row) Then
                            Offset = 1
                        End If

                        'FILL THE RECTANGLE
                        aGr.FillRectangle(Brush, SIRect.X, SIRect.Y, SIRect.Width, SIRect.Height - Offset)

                        If (aNode.SubItems(J).Control IsNot Nothing) Then
                            Dim Ctl As Control = aNode.SubItems(J).Control

                            With Ctl
                                .Location = New Point(SIRect.X + 1, SIRect.Y + 1)
                                .ClientSize = New Size(SIRect.Width - 3, SIRect.Height - 3)
                                .Parent = Me
                                .Visible = True
                                .BringToFront()
                                .Refresh()
                            End With
                        Else
                            Dim MDSW As Integer = 0
                            Dim SubItemFont As Font
                            Dim TempStr As String = String.Empty

                            'SET HANDY VARIABLES
                            If (Not aNode.UseItemStyleForSubItems) Then
                                SubItemFont = SubItem.Font
                                SubItemAlign = SubItem.TextAlign
                                Brush = New SolidBrush(SubItem.ForeColor)
                            Else
                                SubItemFont = aNode.Font
                                SubItemAlign = aNode.TextAlign
                                Brush = New SolidBrush(aNode.ForeColor)
                            End If
                            TempStr = Tools.TruncateString(aNode.SubItems(J).Text, SubItemFont, SIRect.Width, 9, aGr)
                            MDSW = CType(Helpers.Tools.MeasureDisplayString(aGr, TempStr, Me.Font).Width, Integer)

                            'DRAW THE TEXT
                            If (SubItemAlign = HorizontalAlignment.Left) Then
                                Dim TempInt As Integer = 0

                                If (SIRect.X <= 0) Then TempInt = 2 Else TempInt = 0
                                aGr.DrawString(TempStr, SubItemFont, Brush, SIRect.X + 1 + TempInt, SIRect.Y + 1)
                            ElseIf (SubItemAlign = HorizontalAlignment.Right) Then
                                Dim OS As Integer = 0

                                If (SubItemFont.Bold) Then OS = 8 Else OS = 4
                                aGr.DrawString(TempStr, SubItemFont, Brush, SIRect.Right - MDSW - OS, SIRect.Y + 1)
                            Else
                                aGr.DrawString(TempStr, SubItemFont, Brush, (SIRect.X + ((SIRect.Width - MDSW) \ 2)), SIRect.Y + 1)
                            End If
                        End If
                    End If
                Next
            End If

            'RENDER THE ROW GRIDLINE
            aGr.Clip = New Region(New Rectangle(2, NodeRect.Y, Me.GetSumOfVisibleColumnWidths - 1, NodeRect.Height))
            If (Me.GridLines = ContainerListView.GridLineSelections.Both OrElse Me.GridLines = ContainerListView.GridLineSelections.Row) Then
                aGr.DrawLine(Me.GridLinePen, ItemRect.Right, NodeRect.Bottom - 1, NodeRect.Right, NodeRect.Bottom - 1)
                aGr.DrawLine(Me._NodeGridLinePen, ItemRect.Left, NodeRect.Bottom - 1, ItemRect.Right, NodeRect.Bottom - 1)
            End If

            'RENDER FOCUS
            aGr.Clip = New Region(NodeRect)
            If (aNode.Focused AndAlso Me.SelectedItems.Count > 1 AndAlso Me.IsFocused) Then
                Dim TmpRct As Rectangle = Rectangle.Empty

                If (Me.FullRowSelect) Then TmpRct = NodeRect Else TmpRct = ItemRect
                ControlPaint.DrawFocusRectangle(aGr, TmpRct, aNode.ForeColor, aNode.BackColor)
            End If
        End If
    End Sub

    Private Sub _renderPlusMinus(ByVal aGr As Graphics, ByVal aX As Integer, ByVal aY As Integer, ByVal aW As Integer, ByVal aH As Integer, ByVal aNode As TreeListNode)
        If (aNode.IsExpanded) Then aGr.DrawImage(Me._MinusBitMap, aX, aY) Else aGr.DrawImage(Me._PlusBitMap, aX, aY)
        Me._PlusMinusRects.Add(New Rectangle(aX, aY, aW, aH), aNode)
    End Sub

    Private Sub _selectAll(ByVal aNode As TreeListNode)
        If (Not aNode.Selected) Then aNode.Selected = True
        If (aNode.IsExpanded) Then
            For Each Nde As TreeListNode In aNode.Nodes
                Me._selectAll(Nde)
            Next
        End If
    End Sub

    Private Sub _showSelectedItems()
        If (Me._CurNode IsNot Nothing) Then
            If (Me._FirstSelectedNode Is Me._CurNode) Then
                If (Me._CurNode.Selected) Then
                    For Each Nde As TreeListNode In Me.SelectedItems
                        If (Not Nde Is Me._CurNode) Then Nde.Selected = False
                    Next
                Else
                    Me._CurNode.Selected = True
                End If
            Else
                Dim TempNode As TreeListNode = Me._FirstSelectedNode

                Me.SelectedItems.Clear()
                Me.BeginUpdate()
                Me._FirstSelectedNode.Selected = True
                If (Me._FirstSelectedNode.RowIndex < Me._CurNode.RowIndex) Then
                    While (Not TempNode Is Me._CurNode)
                        TempNode = Me.GetNextNode(TempNode)
                        If (TempNode Is Nothing) Then Exit While
                        TempNode.Selected = True
                    End While
                Else
                    While (Not TempNode Is Me._CurNode)
                        TempNode = Me.GetPreviousNode(TempNode)
                        If (TempNode Is Nothing) Then Exit While
                        TempNode.Selected = True
                    End While
                End If
                Me.EndUpdate()
            End If

            Me._makeSelectedVisible()
        End If
    End Sub

    Private Sub _useFoldersImages()
        If (Me._UseDefaultFolders) Then
            If (Not Me.ImageList Is Me._FolderImages) Then
                'ADD THE DEFAULT IMAGES
                Me._FolderImages.Images.Clear()
                With Me._FolderImages.Images
                    .Add(My.Resources.ClosedFolderYellow)
                    .Add(My.Resources.OpenFolderYellow)
                    .Add(My.Resources.ClosedFolderGray)
                    .Add(My.Resources.OpenFolderGray)
                    .Add(My.Resources.ClosedFolderBlue)
                    .Add(My.Resources.OpenFolderBlue)
                End With
                Me.ImageList = Me._FolderImages
                Me._DefaultImageIndex = 0
                Me._DefaultSelectedIndex = 1
            End If
        Else
            If (Me.ImageList Is Me._FolderImages) Then
                Me.ImageList = Nothing
                Me._DefaultImageIndex = -1
                Me._DefaultSelectedIndex = -1
            End If
        End If
    End Sub

#End Region

#Region " Disabled Code "

    'Private Function _findNode(ByVal aStartingNode As TreeListNode, ByVal aRowIndexToFind As Integer, ByVal aPriorRows As Integer) As TreeListNode
    '   If (Not aStartingNode.IsVirtualRootNode) Then
    '      Dim FirstRow As Integer = aRowIndexToFind - aPriorRows

    '      If (FirstRow <= (aStartingNode.VisibleChildren + 1)) Then
    '         If (FirstRow = 1) Then Return aStartingNode
    '         aPriorRows += 1
    '      Else
    '         Return Nothing
    '      End If
    '   End If

    '   For Each Nde As TreeListNode In aStartingNode.Nodes
    '      If (Me._findNode(Nde, aRowIndexToFind, aPriorRows) IsNot Nothing) Then Return Nde
    '      aPriorRows += Nde.VisibleChildren + 1
    '   Next

    '   'SHOULD NOT HAVE GOTTEN THIS FAR
    '   Return Nothing
    'End Function


    'Protected Overrides Sub OnSubControlMouseDown(ByVal sender As Object, ByVal aMouseArg As System.Windows.Forms.MouseEventArgs)
    '   Dim Node As TreeListNode = TreeListNode.Parse(Sender)

    '   Me.SelectedNodes.Clear()

    '   Node.Focused = True
    '   Node.Selected = True
    '   If (aMouseArg.Clicks >= 2) Then
    '      Node.Toggle()
    '      Me.Invalidate(Me.ClientRectangle)
    '   End If
    'End Sub

#End Region

End Class

Public Enum NodeGridLineStyle
    None
    AsListView
    Custom
End Enum
