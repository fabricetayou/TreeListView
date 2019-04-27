Imports WinControls.ListView.ContainerListViewItem

''' <summary>
''' TreeListNode Class.
''' </summary>
''' <remarks></remarks>
<Serializable(), _
 DesignTimeVisible(False), _
 TypeConverter(GetType(TreeListNodeConverter))> _
Public Class TreeListNode
    Inherits ContainerListViewObject

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New instance of TreeListNode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        Me._preInit()
    End Sub

    ''' <summary>
    ''' Creates a New instance of TreeListNode.
    ''' </summary>
    ''' <param name="aText">The Text of the new TreeListNode.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aText As String)
        MyBase.New(aText)
        Me._preInit()
    End Sub

    ''' <summary>
    ''' Creates a New instance of TreeListNode.
    ''' </summary>
    ''' <param name="aText">The Text of the new TreeListNode.</param>
    ''' <param name="aImageIndex">The ImageIndex of the TreeListNode.</param>
    ''' <param name="aSelImageIndex">The SelectedImageIndex of the TreeListNode.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aText As String, ByVal aImageIndex As Integer, ByVal aSelImageIndex As Integer)
        MyBase.New(aText)
        Me.ImageIndex = aImageIndex
        Me._SelImageIndex = aSelImageIndex
        Me._preInit()
    End Sub

    ''' <summary>
    ''' Creates a New instance of TreeListNode.
    ''' </summary>
    ''' <param name="aTree">The TreeListView that owns this TreeListNode.</param> 
    ''' <remarks>This constructor is used for creating the TreeListView's Nodes collection.</remarks>
    Protected Friend Sub New(ByVal aTree As TreeListView)
        MyBase.New()

        If (aTree Is Nothing) Then Throw New NullReferenceException("aTree cannot be Nothing")
        Me._IsVirtualRoot = True
        Me._LstView = aTree
        Me._preInit()
    End Sub

#End Region

#Region " Field Declarations "
    Private _Branch As TreeListNodeBranch
    Private _ChildCount As Integer
    Private _ChildVisibleCount As Integer
    Private _Expanded As Boolean
    Private _ExpdCount As Integer
    Private _IsVirtualRoot As Boolean
    Private _Nodes As TreeListNodeCollection
    Private _Parent As TreeListNode
    Private _Selected As Boolean
    Private _SelImageIndex As Integer = -1
#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets the branch that this TreeListNode represents (this node and ALL child nodes).
    ''' </summary>
    ''' <value>A TreeListNodeBranch object.</value>
    ''' <remarks>
    ''' A branch consists of the current node and ALL child nodes(recursive) beneath it.  The current TreeListNode is considered the 
    ''' root of the branch.
    ''' </remarks>
    <Browsable(False)> _
    Public ReadOnly Property Branch() As TreeListNodeBranch
        Get
            Return Me._Branch
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets the count of all the children.
    ''' </summary>
    ''' <value>An integer.</value>
    ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
    <Browsable(False), _
     EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Property ChildrenCount() As Integer
        Get
            Return Me._ChildCount
        End Get
        Set(ByVal Value As Integer)
            Me._ChildCount = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the count of the potentially visible children.
    ''' </summary>
    ''' <value>An integer.</value>
    ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
    <Browsable(False), _
     EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Property ExpandedCount() As Integer
        Get
            Return Me._ExpdCount
        End Get
        Set(ByVal Value As Integer)
            Me._ExpdCount = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the first Child in the collection.
    ''' </summary>
    ''' <value>The first Child.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property FirstChildNode() As TreeListNode
        Get
            If (Me._Nodes.Count > 0) Then Return Me._Nodes(0)

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the path from the Root TreeListNode to the current TreeListNode.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property FullPath() As String
        Get
            Return Me._getFullPath
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Determines if the current TreeListNode has a parent node.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Overrides ReadOnly Property HasParent() As Boolean
        Get
            Return (Me._Parent IsNot Nothing)
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Gets the zero-based position of the TreeListNode within the TreeListNode collection.
    ''' </summary>
    ''' <value>
    ''' The zero-based index of the item within Collection. If the item is not associated with a collection control, this property returns -1.
    ''' </value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Overrides ReadOnly Property Index() As Integer
        Get
            If (Me.HasParent) Then Return Me._Parent.Nodes.IndexOf(Me)

            Return -1
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if a TreeListNode is in an expanded state.
    ''' </summary>
    ''' <value><c>TRUE</c> if the TreeListNode is in an expanded state;  otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property IsExpanded() As Boolean
        Get
            Return Me._Expanded
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if the TreeListNode is a root (topmost) Node.
    ''' </summary>
    ''' <value><c>TRUE</c> if the TreeListNode is the root (topmost) node in the heirarchy.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property IsRootNode() As Boolean
        Get
            If (Not Me.HasParent OrElse Me._Parent.IsVirtualRootNode) Then Return True

            Return False
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if the TreeListNode is a virtual rootNode.
    ''' </summary>
    ''' <value><c>TRUE</c> if the TreeListNode is the virtual root node in the heirarchy.</value>
    ''' <remarks>
    ''' Virtual root nodes are never displayed in the TreeListView control.  Virtual roots are the owner of the TreeListNodeCollection of the TreeListView.  
    ''' Anyone using this property outside of the TreeListView or TreeListNode class will always get a result of <c>FALSE</c>.  
    ''' This is a convenience property only and is used by the inner methods of the TreeListNode and TreeListView classes.
    ''' </remarks>
    <Browsable(False), _
     EditorBrowsable(EditorBrowsableState.Never)> _
    Protected Friend ReadOnly Property IsVirtualRootNode() As Boolean
        Get
            Return Me._IsVirtualRoot
        End Get
    End Property

    ''' <summary>
    ''' Gets the Last child in the list.
    ''' </summary>
    ''' <value>The Last child.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property LastChildNode() As TreeListNode
        Get
            If (Me._Nodes.Count > 0) Then Return Me._Nodes(Me._Nodes.Count - 1)

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the zero-based Level the Node is at from the RootNode.
    ''' </summary>
    ''' <value>
    ''' An Integer representing the zero-based Level of the Node from the RootNode.
    ''' </value>
    ''' <remarks>
    ''' For Example, if a Nodes' path is 'One\Two\Three', with 'Three' being the current Node, then the current Node's Level is 2 (not 3) because the levels are zero-based.
    ''' You could also look at it as being 2 nodes away from the RootNode.
    ''' </remarks>
    <Browsable(False)> _
    Public ReadOnly Property Level() As Integer
        Get
            Return (Me.GetNodesInPath.Length - 1)
        End Get
    End Property

    ''' <summary>
    ''' Gets the child Nodes of the current Node.
    ''' </summary>
    ''' <value>A TreeListNode collection.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Description("The collection of ChildNodes of the current Node."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     Editor(GetType(CollectionEditor), GetType(UITypeEditor))> _
    Public ReadOnly Property Nodes() As TreeListNodeCollection
        Get
            Return Me._Nodes
        End Get
    End Property

    ''' <summary>
    ''' The topmost Node that this Node and all Nodes in it's subtree are directly or indirectly rooted to.
    ''' </summary>
    ''' <value>A TreeListNode representing the topmost Node.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property RootNode() As TreeListNode
        Get
            Dim Arr() As TreeListNode = Me.GetNodesInPath

            If (Arr IsNot Nothing AndAlso Arr.Length > 0) Then Return Arr(0)

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the zero-based Index of the virtual Row the node belongs to in the TreeListView.  This is NOT an index into the TreeListNode collection!
    ''' </summary>
    ''' <value>An Integer value representing the virtual Row the node belongs to in the TreeListView.  If the node is not a member of a TreeListView OR it is not visible, then -1 is returned.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property RowIndex() As Integer
        Get
            Return Me._getRowIndex
        End Get
    End Property

    ''' <summary>
    ''' Gets the Next Node in the collection.
    ''' </summary>
    ''' <value>A TreeListNode representing the Next node in the collection.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property NextSiblingNode() As TreeListNode
        Get
            If (Me.HasParent AndAlso Me.Index < (Me._Parent.Nodes.Count - 1)) Then Return Me._Parent.Nodes(Me.Index + 1)

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the ParentNode of the current node.
    ''' </summary>
    ''' <value>A TreeListNode object.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property ParentNode() As TreeListNode
        Get
            If (Not Me.HasParent OrElse Me._Parent.IsVirtualRootNode) Then Return Nothing Else Return Me._Parent
        End Get
    End Property

    ''' <summary>
    ''' Gets the Previous node in the collection.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property PreviousSiblingNode() As TreeListNode
        Get
            If (Me.HasParent AndAlso Me.Index > 0) Then Return Me._Parent.Nodes(Me.Index - 1)

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Gets or sets a value indicating whether the item is selected.
    ''' </summary>
    ''' <value><c>TRUE</c> if the item is selected; otherwise, <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DefaultValue(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Selected() As Boolean
        Get
            Return Me._Selected
        End Get
        Set(ByVal Value As Boolean)
            If (Me.AllowSelection) Then
                If (Not Me._Selected.Equals(Value)) Then
                    Dim InvalidTree As Boolean = Me.ListView Is Nothing

                    If (Not Me._Selected) Then
                        Dim Cancel As Boolean = False

                        If (Not InvalidTree) Then Cancel = Me.ListView.ContainerListViewBeforeSelect(Me)
                        If (Not Cancel) Then
                            Me._Selected = True

                            If (Not InvalidTree) Then
                                If (Not Me.ListView.MultiSelect) Then
                                    Me.ListView.SelectedItems.Clear()
                                    Me.Focused = True
                                End If
                                Me.ListView.SelectedItems.Add(Me)
                                Me.ListView.ContainerListViewAfterSelect(Me)
                            End If
                        End If
                    Else
                        If (Not InvalidTree) Then Me.ListView.SelectedItems.Remove(Me)
                        Me._Selected = False
                    End If

                    If (Not InvalidTree) Then Me.ListView.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the index of the image that is displayed for the TreeListNode when it is in a selected state.
    ''' </summary>
    ''' <value>The zero-based index of the image in the ImageList that is displayed for the TreeListNode when it is in a selected state. The default is -1.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(-1), _
     Description("The index of the image that is displayed for the TreeListNode when it is in a selected state."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     TypeConverter(GetType(ImageIndexConverter)), _
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(UITypeEditor))> _
    Public Property SelectedImageIndex() As Integer
        Get
            Return Me._SelImageIndex
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._SelImageIndex.Equals(Value)) Then Me._SelImageIndex = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the parent TreeListView the TreeListNode is assigned to.
    ''' </summary>
    ''' <value>A TreeListView object; othewise <c>NOTHING</c> if the node is not assigned.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Shadows ReadOnly Property ListView() As TreeListView
        Get
            Return DirectCast(Me._LstView, TreeListView)
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets the count of the visible children.
    ''' </summary>
    ''' <value>An integer.</value>
    ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
    <Browsable(False), _
     EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Property VisibleChildren() As Integer
        Get
            Return Me._ChildVisibleCount
        End Get
        Set(ByVal Value As Integer)
            Me._ChildVisibleCount = Value
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Check or UnCheck all the nodes in the Node collection.
    ''' </summary>
    ''' <param name="aChecked"><c>TRUE</c> to check all the nodes; otherwise <c>FALSE</c> to uncheck them.</param>
    ''' <param name="aImmediateOnly">Optional.  <c>TRUE</c> to check only the immediate children; otherwise <c>FALSE</c> to check all children.</param>
    ''' <remarks></remarks>
    Public Sub CheckNodes(ByVal aChecked As Boolean, Optional ByVal aImmediateOnly As Boolean = True)
        If (Me._LstView IsNot Nothing) Then Me._LstView.BeginUpdate()
        Me.Checked = aChecked
        For Each Nde As TreeListNode In Me._Nodes
            Nde.Checked = aChecked
            If (Not aImmediateOnly) Then Nde.CheckNodes(aChecked, aImmediateOnly)
        Next
        If (Me._LstView IsNot Nothing) Then Me._LstView.EndUpdate()
    End Sub

    ''' <summary>
    ''' Overriden.  Copies the TreeListNode and the entire subtree rooted at this node.
    ''' </summary>
    ''' <returns>A cloned TreeListNode and it's entire subtree.</returns>
    ''' <remarks></remarks>
    Public Overrides Function Clone() As Object
        Dim CloneNode As New TreeListNode

        With CloneNode
            .BackColor = Me.BackColor
            .CheckBoxEnabled = Me.CheckBoxEnabled
            .CheckBoxVisible = Me.CheckBoxVisible
            .Checked = Me.Checked
            .Font = Me.Font
            .ForeColor = Me.ForeColor
            .ImageIndex = Me.ImageIndex
            .SelectedImageIndex = Me._SelImageIndex
            'If .ImageIndex < 0 Then .ImageIndex = Me.ImageIndex
            'If .SelectedImageIndex < 0 Then .SelectedImageIndex = Me._SelImageIndex
            .Tag = Me.Tag
            .Text = Me.Text
            .UseItemStyleForSubItems = Me.UseItemStyleForSubItems
        End With
        'CLONE CHILD NODES
        For Each Node As TreeListNode In Me._Nodes
            CloneNode.Nodes.Add(DirectCast(Node.Clone, TreeListNode))
        Next

        'CLONE THE SUBITEMS
        For Each SI As ContainerListViewSubItem In Me.SubItems
            CloneNode.SubItems.Add(DirectCast(SI.Clone, ContainerListViewSubItem))
        Next

        Return CloneNode
    End Function

    ''' <summary>
    ''' Collapses the TreeListNode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Collapse()
        If (Me._Expanded) Then
            Dim Cancel As Boolean = False

            If (Me.ListView IsNot Nothing) Then Cancel = (Me.ListView.TreeListViewBeforeCollapse(Me))

            If (Not Cancel) Then
                Me._Expanded = False
                Me.PropagateNodeChange(0, -(Me._ChildVisibleCount), -(Me.Nodes.Count))

                If (Me.ListView IsNot Nothing) Then
                    Me.ListView.TreeListViewAfterCollapse(Me)
                    Me.ListView.Invalidate()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Collapses the current Node and all of its children.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CollapseAll()
        For Each Node As TreeListNode In Me._Nodes
            Node.CollapseAll()
        Next

        Me.Collapse()
    End Sub

    ''' <summary>
    ''' Expands the current TreeListNode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Expand()
        If (Not Me._Expanded) Then
            Dim Cancel As Boolean = False

            If (Me.ListView IsNot Nothing) Then Cancel = (Me.ListView.TreeListViewBeforeExpand(Me))

            If (Not Cancel) Then
                Me._Expanded = True
                Me._ChildVisibleCount = 0
                Me.PropagateNodeChange(0, Me._getVisibleNodesCount, Me._Nodes.Count)

                If (Me.ListView IsNot Nothing) Then
                    Me.ListView.TreeListViewAfterExpand(Me)
                    Me.ListView.Invalidate()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Expands the current TreeListNode and all of its children.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExpandAll()
        Me.Expand()

        For Each Node As TreeListNode In Me._Nodes
            Node.ExpandAll()
        Next
    End Sub

    ''' <summary>
    ''' Returns the number of child tree nodes.
    ''' </summary>
    ''' <param name="aIncludeSubTrees"><c>TRUE</c> if the resulting count includes all nodes indirectly rooted at this node; otherwise <c>FALSE</c>.</param>
    ''' <returns>An integer representing the number of child nodes.</returns>
    ''' <remarks></remarks>
    Public Function GetNodeCount(ByVal aIncludeSubTrees As Boolean) As Integer
        If (aIncludeSubTrees) Then Return Me.ChildrenCount

        Return Me.Nodes.Count
    End Function

    ''' <summary>
    ''' Returns all the Nodes in the path from the RootNode to the current Node.  The Nodes are indexed in the array starting from the RootNode.
    ''' </summary>
    ''' <returns>a TreeListNode array containing the Nodes in the path of the current node.</returns>
    ''' <remarks></remarks>
    Public Function GetNodesInPath() As TreeListNode()
        Dim Nodes As New ArrayList

        'CHECK TO MAKE SURE THAT WE DON'T GET THE VIRTUAL PARENT (THE TREEVIEW'S NODE COLLECTION OWNER) ALSO.
        If (Me.HasParent AndAlso Not Me._Parent.IsVirtualRootNode) Then Nodes.AddRange(Me._Parent.GetNodesInPath)
        Nodes.Add(Me)

        Return DirectCast(Nodes.ToArray(GetType(TreeListNode)), TreeListNode())
    End Function

    ''' <summary>
    ''' Returns a value that determines if the specified node is a child (direct or indirect) somewhere in this Node's branch.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode to search for in the path of the current node.</param>
    ''' <returns><c>TRUE</c> if the specified TreeListNode is in the current node's branch; otherwise <c>FALSE</c> if it is not.</returns>
    ''' <remarks>
    ''' This is the opposite of IsNodeInPath.  Instead of traversing up the nodes to find the path, this function traverses down (starting from this node) 
    ''' to determine if the specified node is a child somewhere in the branch.
    ''' </remarks>
    Public Function IsNodeInBranch(ByVal aNode As TreeListNode) As Boolean
        Dim NodeArray() As TreeListNode = Me.Branch.GetBranchNodes

        If (NodeArray IsNot Nothing) Then
            For Each Nde As TreeListNode In NodeArray
                If (Nde Is aNode) Then Return True
            Next
        End If

        Return False
    End Function

    ''' <summary>
    ''' Returns a value that determines if the specified node is in the path of the current node.
    ''' </summary>
    ''' <param name="aNode">The TreeListNode to search for in the path of the current node.</param>
    ''' <returns><c>TRUE</c> if the specified TreeListNode is in the current node's path; otherwise <c>FALSE</c> if it is not.</returns>
    ''' <remarks>
    ''' This is the opposite of IsNodeInBranch.  Instead of traversing down the nodes to find the path, this function traverses up (starting from this node) 
    ''' to determine if the specified node is somewhere in this node's path.
    ''' </remarks>
    Public Function IsNodeInPath(ByVal aNode As TreeListNode) As Boolean
        Dim NodeArray() As TreeListNode = Me.GetNodesInPath

        If (NodeArray IsNot Nothing) Then
            For Each Node As TreeListNode In NodeArray
                If (Node Is aNode) Then Return True
            Next
        End If

        Return False
    End Function

    ''' <summary>
    ''' Returns a TreeListNode initialized with the property data in the XmlNode.
    ''' </summary>
    ''' <param name="aXmlNode">The XmlNode containing the property data.</param>
    ''' <returns>
    ''' A TreeListNode set with the passed in property data; otherwise, a new TreeListNode set with defaults.
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Shadows Function Parse(ByVal aXmlNode As XmlNode) As TreeListNode
        Dim Clvi As New TreeListNode()

        ContainerListViewObject.Parse(Clvi, aXmlNode)

        Return Clvi
    End Function

    ''' <summary>
    ''' Informs the Node (and it's parent) of any changes in it's children.  For internal use only.
    ''' </summary>
    ''' <param name="aTotalCountDelta">The Total number of children.</param>
    ''' <param name="aVisibleChildrenDelta">The total number of visible children.</param>
    ''' <param name="expChildren">The number of currently expanded children.</param>
    ''' <remarks>This code is for internal use only and is not intended to be called from your code.  Calling this method externally may have an adverse effect on code that uses this class.</remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub PropagateNodeChange(ByVal aTotalCountDelta As Integer, ByVal aVisibleChildrenDelta As Integer, ByVal expChildren As Integer)
        Me._ChildCount += aTotalCountDelta
        Me._ChildVisibleCount += aVisibleChildrenDelta
        Me._ExpdCount += expChildren

        If (Me.HasParent) Then Me._Parent.PropagateNodeChange(aTotalCountDelta, aVisibleChildrenDelta, expChildren)
    End Sub

    ''' <summary>
    ''' Removes the current tree node from the TreeListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        'REMOVE FROM THE MAIN COLLECTION.  THE TREELISTNODECOLLECTION, IN TURN, WILL CALL THE SETTREE METHOD AND PASS IN <c>NOTHING</c>.
        If (Me.HasParent) Then Me._Parent.Nodes.Remove(Me)
    End Sub

    ''' <summary>
    ''' Checks or unchecks all child TreeListNodes.
    ''' </summary>
    ''' <param name="aValue"><c>TRUE</c> to check the child nodes; otherwise <c>FALSE</c> to uncheck them.</param>
    ''' <param name="aIncludeAllChildNodes"><c>TRUE</c> to go down and include all child nodes; <c>FALSE</c> to check only the immediate child nodes.</param>
    ''' <remarks></remarks>
    Public Sub SetChildNodesCheckBox(ByVal aValue As Boolean, ByVal aIncludeAllChildNodes As Boolean)
        If (Me._LstView IsNot Nothing) Then Me._LstView.BeginUpdate()

        Me.Checked = aValue
        For Each Nde As TreeListNode In Me._Nodes
            Nde.Checked = aValue

            If (aIncludeAllChildNodes) Then Nde.SetChildNodesCheckBox(aValue, aIncludeAllChildNodes)
        Next
        If (Me._LstView IsNot Nothing) Then Me._LstView.EndUpdate()
    End Sub

    ''' <summary>
    ''' Toggles the current Node.  If the Node is expanded it will collapse and vice-versa.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Toggle()
        If (Me._Expanded) Then Me.Collapse() Else Me.Expand()
    End Sub

    ''' <summary>
    ''' Sets the ParentNode of the current Node.
    ''' </summary>
    ''' <param name="aPNode">The ParentNode of the current Node.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub SetParentNode(ByVal aPNode As TreeListNode)
        Me._Parent = aPNode
    End Sub

    ''' <summary>
    ''' Assigns the TreeView_PA that the Item belongs to.
    ''' </summary>
    ''' <param name="aTreeView">The TreeView_PA that owns the item.</param>
    ''' <remarks>
    ''' This code is for internal use only and is not intended to be called from your code.  
    ''' Calling this method externally may have an adverse effect on code that uses this class.
    ''' </remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Overrides Sub SetParent(ByVal aTreeView As ContainerListView)
        If (Not aTreeView Is Me.ListView) Then
            If (Me.ListView IsNot Nothing) Then

                'REMOVE FROM THE OTHER COLLECTIONS IF NECESSARY
                If (Me.Checked) Then Me.ListView.CheckedItems.Remove(Me)
                If (Me.Selected) Then Me.ListView.SelectedItems.Remove(Me)
            End If

            'ASSIGN THE TREE AND ADD TO ANY NECESSARY COLLECTIONS
            Me._LstView = aTreeView
            If (Me.ListView IsNot Nothing) Then
                'Me.ImageIndex = Me.ListView.DefaultImageIndex
                'Me._SelImageIndex = Me.ListView.DefaultSelectedImageIndex
                If Me.ImageIndex < 0 Then Me.ImageIndex = Me.ListView.DefaultImageIndex
                If Me.SelectedImageIndex < 0 Then Me.SelectedImageIndex = Me.ListView.DefaultSelectedImageIndex

                If (Me.Checked) Then Me.ListView.CheckedItems.Add(Me)
                If (Me.Selected) Then Me.ListView.SelectedItems.Add(Me)
            End If

            'RESET THE SUBITEMS
            For Each SI As ContainerListViewSubItem In Me.SubItems
                SI.SetParentOwner(aTreeView)
            Next

            'RESET THE CHILD NODES
            For Each Node As TreeListNode In Me._Nodes
                Node.SetParent(Me.ListView)
            Next
        End If
    End Sub

#End Region

#Region " Procedures "

    Private Function _getFullPath() As String
        Dim Sep As String = "\"

        'GET OUR SEPARATOR IF WE ARE ATTACHED TO A LISTVIEW.  OTHERWISE USE THE DEFAULT.
        If (Me._LstView IsNot Nothing) Then Sep = DirectCast(Me._LstView, TreeListView).PathSeparator

        'CHECK TO MAKE SURE THAT WE DON'T GET THE VIRTUAL PARENT (THE TREEVIEW'S NODE COLLECTION OWNER) ALSO.
        If (Me.HasParent AndAlso Not Me._Parent.IsVirtualRootNode) Then Return Me._Parent.FullPath & Sep & Me.Text

        Return Me.Text
    End Function

    Private Function _getRowIndex() As Integer
        If (Me.ListView IsNot Nothing) Then
            Dim RowCount As Integer
            Dim TN As TreeListNode
            Dim Nodes() As TreeListNode = Me.GetNodesInPath

            'TALLY UP THE VISIBLE NODES PRIOR TO THIS NODE
            For I As Integer = 0 To (Me.RootNode.Index - 1)
                RowCount += Me.ListView.Nodes(I).VisibleChildren + 1
            Next

            For I As Integer = 0 To Nodes.GetUpperBound(0)
                TN = Nodes(I)

                If (I > 0) Then
                    For J As Integer = 0 To (TN.Index - 1)
                        RowCount += TN.ParentNode.Nodes(J).VisibleChildren + 1
                    Next
                    RowCount += 1
                End If
            Next

            Return RowCount
        End If

        Return -1
    End Function

    Private Function _getVisibleNodesCount() As Integer
        Dim VCount As Integer = Me._ChildVisibleCount

        For Each Nde As TreeListNode In Me._Nodes
            'ADD THE NUMBER OF EXPANDED NODES BENEATH THIS NODE
            If (Nde.IsExpanded) Then VCount += Nde.ExpandedCount
            'THIS NODE IS NOW VISIBLE SO ADD IT ALSO
            VCount += 1
        Next

        Return VCount
    End Function

    Private Sub _preInit()
        If (Me.IsVirtualRootNode) Then
            Me._Nodes = New TreeListNodeCollection(Me, Me.ListView)
        Else
            Me._Nodes = New TreeListNodeCollection(Me)
        End If

        'MUST SET THIS AFTER THE COLLECTION HAS BEEN CREATED
        Me._Branch = New TreeListNodeBranch(Me)
    End Sub

#End Region

#Region " Sub Classes "

    ''' <summary>
    ''' TreeListNodeBranch Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListNodeBranch

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListNodeBranch.
        ''' </summary>
        ''' <param name="aNode">The root TreeListNode of the Branch.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aNode As TreeListNode)
            If (aNode Is Nothing) Then Throw New NullReferenceException("aNode cannot be NULL.")

            Me._RootNode = aNode
            Me._preInit()
        End Sub

#End Region

#Region " Field Declarations "
        Private _BckClr As Color = SystemColors.Highlight
        Private _ForClr As Color = Color.Blue
        Private _Font As Font
        Private _HighLighted As Boolean = False
        Private _Mapping As Specialized.HybridDictionary
        Private _RootNode As TreeListNode

        Private Structure OriginalValues
            Private _BackColor As Color
            Private _ForeColor As Color
            Private _Font As Font
            Private _UseForSubItems As Boolean

            Public Sub New(ByVal aBckClr As Color, ByVal aForClr As Color, ByVal fontVal As Font, ByVal aUseForSubItems As Boolean)
                Me._BackColor = aBckClr
                Me._ForeColor = aForClr
                Me._Font = fontVal
                Me._UseForSubItems = aUseForSubItems
            End Sub

            Public ReadOnly Property BackColor() As Color
                Get
                    Return Me._BackColor
                End Get
            End Property

            Public ReadOnly Property Font() As Font
                Get
                    Return Me._Font
                End Get
            End Property

            Public ReadOnly Property ForeColor() As Color
                Get
                    Return Me._ForeColor
                End Get
            End Property

            Public ReadOnly Property UseItemStyleForSubItems() As Boolean
                Get
                    Return Me._UseForSubItems
                End Get
            End Property

        End Structure

#End Region

#Region " Events "

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets the highlighted branch BackColor.
        ''' </summary>
        ''' <value>A Color value.</value>
        ''' <remarks></remarks>
        Public Property HighLightBackColor() As Color
            Get
                Return Me._BckClr
            End Get
            Set(ByVal Value As Color)
                If (Not Me._BckClr.Equals(Value)) Then
                    Me._BckClr = Value
                    If (Me._HighLighted) Then Me._highLightChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets a value that determines if the branch should be highlighted or not.
        ''' </summary>
        ''' <value><c>TRUE</c> if the branch should be highlighted; otherwise <c>FALSE</c>.</value>
        ''' <remarks>Highlighted DOES NOT mean the nodes are selected.</remarks>
        Public Property HighLighted() As Boolean
            Get
                Return Me._HighLighted
            End Get
            Set(ByVal Value As Boolean)
                If (Not Me._HighLighted.Equals(Value)) Then
                    Me._HighLighted = Value

                    If (Me._HighLighted) Then Me._rebuildMapper()
                    Me._highLightChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the highlighted branch Font.
        ''' </summary>
        ''' <value>A Font value.</value>
        ''' <remarks></remarks>
        Public Property HighLightFont() As Font
            Get
                Return Me._Font
            End Get
            Set(ByVal Value As Font)
                If (Not Me._Font.Equals(Value)) Then
                    Me._Font = Value
                    If (Me._HighLighted) Then Me._highLightChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the highlighted branch ForeColor.
        ''' </summary>
        ''' <value>A Color value.</value>
        ''' <remarks></remarks>
        Public Property HighLightForeColor() As Color
            Get
                Return Me._ForClr
            End Get
            Set(ByVal Value As Color)
                If (Not Me._ForClr.Equals(Value)) Then
                    Me._ForClr = Value
                    If (Me._HighLighted) Then Me._highLightChanged()
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Returns every TreeListNode belonging to this branch.
        ''' </summary>
        ''' <returns>An array of TreeListNodes.</returns>
        ''' <remarks></remarks>
        Public Function GetBranchNodes() As TreeListNode()
            Dim ArrList As New ArrayList

            ArrList.Add(Me._RootNode)
            For Each Nde As TreeListNode In Me._RootNode.Nodes
                Me._fetchBranchNodes(ArrList, Nde)
            Next

            Return DirectCast(ArrList.ToArray(GetType(TreeListNode)), TreeListNode())
        End Function

        ''' <summary>
        ''' Refreshes all branch TreeListNodes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Refresh()
            Me._refreshMapper()
        End Sub

        ''' <summary>
        ''' Sets the specified Property on all Branch nodes.
        ''' </summary>
        ''' <param name="aPropName">The property name whose value to set on all branch nodes.</param>
        ''' <param name="aValue">The value to set.</param>
        ''' <remarks></remarks>
        Public Sub SetProperty(ByVal aPropName As String, ByVal aValue As Object)
            Dim Name As String = aPropName.Trim

            If (Not Name.Equals(String.Empty)) Then
                Dim Prop As PropertyInfo = Me._RootNode.GetType.GetProperty(Name)

                If (Prop IsNot Nothing AndAlso Prop.CanWrite) Then
                    Dim Ndes() As TreeListNode = Me.GetBranchNodes

                    If (Me._RootNode.ListView IsNot Nothing) Then Me._RootNode.ListView.BeginUpdate()

                    For Each Nde As TreeListNode In Ndes
                        Prop.SetValue(Nde, aValue, Nothing)
                    Next

                    If (Me._RootNode.ListView IsNot Nothing) Then Me._RootNode.ListView.EndUpdate()
                End If
            End If
        End Sub

#End Region

#Region " Procedures "

        Private Sub _fetchBranchNodes(ByVal aList As ArrayList, ByVal aNode As TreeListNode)
            aList.Add(aNode)
            For Each Nde As TreeListNode In aNode.Nodes
                Me._fetchBranchNodes(aList, Nde)
            Next
        End Sub

        Private Sub _highLightChanged()
            Dim Nde As TreeListNode

            If (Me._RootNode.ListView IsNot Nothing) Then Me._RootNode.ListView.BeginUpdate()

            If (Me._HighLighted) Then
                For Each Nde In Me._Mapping.Keys
                    With Nde
                        .UseItemStyleForSubItems = False
                        .BackColor = Me._BckClr
                        .Font = Me._Font
                        .ForeColor = Me._ForClr
                    End With
                Next
            Else
                Dim OV As OriginalValues

                For Each Nde In Me._Mapping.Keys
                    OV = DirectCast(Me._Mapping(Nde), OriginalValues)

                    With Nde
                        .UseItemStyleForSubItems = OV.UseItemStyleForSubItems
                        .BackColor = OV.BackColor
                        .Font = OV.Font
                        .ForeColor = OV.ForeColor
                    End With
                Next
            End If

            If (Me._RootNode.ListView IsNot Nothing) Then Me._RootNode.ListView.EndUpdate()
        End Sub

        Private Sub _preInit()
            Me._Font = Me._RootNode.Font
            Me._rebuildMapper()
        End Sub

        Private Sub _rebuildMapper()
            Dim Ndes As TreeListNode() = Me.GetBranchNodes

            If (Me._Mapping Is Nothing) Then Me._Mapping = New Specialized.HybridDictionary(Ndes.Length, False) Else Me._Mapping.Clear()

            For Each Nde As TreeListNode In Ndes
                Dim OV As New OriginalValues(Nde.BackColor, Nde.ForeColor, Nde.Font, Nde.UseItemStyleForSubItems)

                Me._Mapping.Add(Nde, OV)
            Next
        End Sub

        Private Sub _refreshMapper()
            Dim Ndes() As TreeListNode

            'REBUILD THE MAPPER IF IT IS <c>NOTHING</c> OR EMPTY INSTEAD
            If (Me._Mapping Is Nothing OrElse Me._Mapping.Count = 0) Then
                Me._rebuildMapper()
                Exit Sub
            End If

            'ADD ANY NODES (THAT ARE NOT THERE) TO THE MAPPER
            Ndes = Me.GetBranchNodes
            For Each Nde As TreeListNode In Ndes
                If (Not Me._Mapping.Contains(Nde)) Then
                    Dim OV As New OriginalValues(Nde.BackColor, Nde.ForeColor, Nde.Font, Nde.UseItemStyleForSubItems)

                    Me._Mapping.Add(Nde, OV)
                End If
            Next

            'HIGHLIGHT OR NOT
            Me._highLightChanged()
        End Sub

#End Region

    End Class

#End Region

End Class
