Namespace EventArgClasses

    ''' <summary>
    ''' ContainerColumnHeaderEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerColumnHeaderEventArgs
        Inherits EventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerColumnHeaderEventArgs.
        ''' </summary>
        ''' <param name="aCol">The ContainerContainerColumnHeader to intialize the class with.</param>
        ''' <param name="e">The MouseEventArgs to initialize the class with.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aCol As ContainerColumnHeader, ByVal e As MouseEventArgs)
            Me._Col = aCol
            Me._E = e
        End Sub

#End Region

#Region " Field Declarations "
        Private _Col As ContainerColumnHeader
        Private _E As MouseEventArgs
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Column that was clicked.
        ''' </summary>
        ''' <value>A ContainerColumnHeader object.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Column() As ContainerColumnHeader
            Get
                Return Me._Col
            End Get
        End Property

        ''' <summary>
        ''' The MouseEventArg created when the Column was clicked.
        ''' </summary>
        ''' <value>A MouseEventArgs object.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property MouseArg() As MouseEventArgs
            Get
                Return Me._E
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' ContainerListViewCancelEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewCancelEventArgs
        Inherits EventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewCancelEventArgs.
        ''' </summary>
        ''' <param name="aItem">The ContainerListViewObject that the event is responding to.</param>
        ''' <param name="aCancel"><c>TRUE</c> to cancel the event; otherwise, <c>FALSE</c>.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aItem As ContainerListViewObject, ByVal aCancel As Boolean)
            Me._Item = aItem
            Me._Cancel = aCancel
        End Sub

#End Region

#Region " Field Declarations "
        Private _Cancel As Boolean = False
        Private _Item As ContainerListViewObject
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets a value indicating whether the event should be canceled.
        ''' </summary>
        ''' <value><c>TRUE</c> if the event should be canceled; otherwise, <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        Public Property Cancel() As Boolean
            Get
                Return Me._Cancel
            End Get
            Set(ByVal Value As Boolean)
                Me._Cancel = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the ContainerListViewObject that has been checked or selected.
        ''' </summary>
        ''' <value>The ContainerListViewObject that has been checked or selected.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Item() As ContainerListViewObject
            Get
                Return Me._Item
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' ContainerListViewEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewEventArgs
        Inherits EventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Initializes a new instance of the ContainerListViewEventArgs class.
        ''' </summary>
        ''' <param name="aItem">The ContainerListViewObject that the event is responding to.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aItem As ContainerListViewObject)
            Me._Item = aItem
        End Sub

#End Region

#Region " Field Declarations "
        Private _Item As ContainerListViewObject
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the ContainerListViewObject that has been checked or selected.
        ''' </summary>
        ''' <value>The ContainerListViewObject that has been checked or selected.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Item() As ContainerListViewObject
            Get
                Return Me._Item
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' ItemActionEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemActionEventArgs
        Inherits EventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ItemActionEventArgs.
        ''' </summary>
        ''' <param name="aIndex">The Index of the Item that changed.</param>
        ''' <param name="aAction">The Action the collection is performing.</param>
        ''' <param name="aVal">The New value of the object the action is performed on.</param>
        ''' <param name="aPrevVal">The Previous value of the object before it changed.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aIndex As Integer, ByVal aAction As Enums.CollectionActions, ByVal aVal As Object, Optional ByVal aPrevVal As Object = Nothing)
            Me._Index = aIndex
            Me._Action = aAction
            Me._NewVal = aVal
            Me._OldVal = aPrevVal
        End Sub

#End Region

#Region " Field Declarations "
        Private _Index As Integer = -1
        Private _NewVal As Object = Nothing
        Private _OldVal As Object = Nothing
        Private _Action As Enums.CollectionActions = Enums.CollectionActions.Nothing
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Action performed on the Item.
        ''' </summary>
        ''' <value>An enumerated CollectionActions value.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Action() As Enums.CollectionActions
            Get
                Return Me._Action
            End Get
        End Property

        ''' <summary>
        ''' Gets the Index of the Item that was changed.
        ''' </summary>
        ''' <value>An Integer representing the Index of the Item changed.</value>
        ''' <remarks>If the Action type is CollectionActions.Clearing or CollectionActions.Cleared, the Index will be -1.</remarks>
        Public ReadOnly Property Index() As Integer
            Get
                Return _Index
            End Get
        End Property

        ''' <summary>
        ''' Gets the Previous value of the Item.
        ''' </summary>
        ''' <value>An object.  This can be <c>NOTHING</c>.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property PreviousValue() As Object
            Get
                Return Me._OldVal
            End Get
        End Property

        ''' <summary>
        ''' Gets the Value of the Item.
        ''' </summary>
        ''' <value>An object.</value>
        ''' <remarks>If the Action type is CollectionActions.Clearing or CollectionActions.Cleared, the Value will be an Ilist representing the items in the collection.</remarks>
        Public ReadOnly Property Value() As Object
            Get
                Return Me._NewVal
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' TreeListNodeEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListNodeLabelEditEventArgs
        Inherits EventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListNodeEventArgs.
        ''' </summary>
        ''' <param name="aNode">The tree node containing the text to edit.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aNode As TreeListNode)
            Me._Node = aNode
        End Sub

        ''' <summary>
        ''' Creates a New instance of TreeListNodeEventArgs.
        ''' </summary>
        ''' <param name="aNode">The tree node containing the text to edit.</param>
        ''' <param name="aText">The new text to associate with the tree node.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aNode As TreeListNode, ByVal aText As String)
            Me._Node = aNode
            Me._Label = aText
        End Sub

#End Region

#Region " Field Declarations "
        Private _Cancel As Boolean = False
        Private _Label As String = String.Empty
        Private _Node As TreeListNode
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets a value indicating whether the edit has been canceled.
        ''' </summary>
        ''' <value><c>TRUE</c> if the edit has been canceled; otherwise, <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        Public Property CancelEdit() As Boolean
            Get
                Return Me._Cancel
            End Get
            Set(ByVal Value As Boolean)
                Me._Cancel = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the new text to associate with the tree node.
        ''' </summary>
        ''' <value>The string value that represents the TreeListNode label.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Label() As String
            Get
                Return Me._Label
            End Get
        End Property

        ''' <summary>
        ''' Gets the tree node containing the text to edit.
        ''' </summary>
        ''' <value>A TreeNode that represents the tree node containing the text to edit.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Node() As TreeListNode
            Get
                Return Me._Node
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' TreeListViewCancelEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListViewCancelEventArgs
        Inherits ContainerListViewCancelEventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListViewCancelEventArgs.
        ''' </summary>
        ''' <param name="aNode">The TreeListNode that the event is responding to.</param>
        ''' <param name="aCancel"><c>TRUE</c> to cancel the event; otherwise, <c>FALSE</c>.</param>
        ''' <param name="aAction">The type of TreeViewAction that raised the event.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aNode As TreeListNode, ByVal aCancel As Boolean, ByVal aAction As TreeViewAction)
            MyBase.New(aNode, aCancel)
            Me._Action = aAction
        End Sub

#End Region

#Region " Field Declarations "
        Private _Action As TreeViewAction = TreeViewAction.Expand
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the type of action that raised the event.
        ''' </summary>
        ''' <value>The type of TreeViewAction that raised the event.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Action() As TreeViewAction
            Get
                Return Me._Action
            End Get
        End Property

        ''' <summary>
        ''' Shadowed.  Gets the TreeListNode that has been checked, expanded, collapsed, or selected.
        ''' </summary>
        ''' <value>The TreeListNode that has been checked, expanded, collapsed, or selected.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Item() As TreeListNode
            Get
                Return DirectCast(MyBase.Item, TreeListNode)
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' Class TreeListViewItemDragEventArgs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListViewItemDragEventArgs
        Inherits TreeListViewDragEventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListViewItemDropEventArgs
        ''' </summary>
        ''' <param name="aButton">An enumerated MouseButtons value.</param>
        ''' <param name="aMousePos">The current position of the Mouse pointer in client coordinates.</param>
        ''' <param name="aNodes">An array of nodes being dragged.</param>
        ''' <param name="aCancel">Optional.  <c>TRUE</c> to cancel the event; otherwise <c>FALSE</c>.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aButton As MouseButtons, ByVal aMousePos As Point, ByVal aNodes() As TreeListNode, ByVal keyState As Integer, Optional ByVal aCancel As Boolean = False)
            MyBase.New(aButton, aMousePos, aNodes, keyState)
            Me._Cancel = aCancel
        End Sub

#End Region

#Region " Field Declarations "
        Private _Cancel As Boolean
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets a value that determines if the event should be cancelled..
        ''' </summary>
        ''' <value><c>TRUE</c> to cancel the event; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        Public Property Cancel() As Boolean
            Get
                Return Me._Cancel
            End Get
            Set(ByVal Value As Boolean)
                Me._Cancel = Value
            End Set
        End Property

#End Region

    End Class

    ''' <summary>
    ''' Class TreeListViewItemDropEventArgs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListViewItemDropEventArgs
        Inherits TreeListViewDragEventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListViewItemDropEventArgs
        ''' </summary>
        ''' <param name="aButton">An enumerated MouseButtons value.</param>
        ''' <param name="aMousePos">The current position of the Mouse pointer in client coordinates.</param>
        ''' <param name="aNodes">An array of nodes being dragged.</param>
        ''' <param name="aTargetNode">The TreeListNode the data is being dropped on.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aButton As MouseButtons, ByVal aMousePos As Point, ByVal aNodes() As TreeListNode, ByVal aTargetNode As TreeListNode, ByVal keyState As Integer)
            MyBase.New(aButton, aMousePos, aNodes, keyState)
            Me._DropNode = aTargetNode
        End Sub

#End Region

#Region " Field Declarations "
        Private _DropNode As TreeListNode
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the target TreeListNode the data is being dropped on.
        ''' </summary>
        ''' <value>A TreeListNode object.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property TargetNode() As TreeListNode
            Get
                Return Me._DropNode
            End Get
        End Property
#End Region

    End Class

    ''' <summary>
    ''' Class TreeListViewDragEventArgs.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class TreeListViewDragEventArgs
        Inherits ItemDragEventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListViewDragEventArgs
        ''' </summary>
        ''' <param name="aButton">An enumerated MouseButtons value.</param>
        ''' <param name="aMousePos">The current position of the Mouse pointer in client coordinates.</param>
        ''' <param name="aNodes">An array of nodes being dragged.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aButton As MouseButtons, ByVal aMousePos As Point, ByVal aNodes() As TreeListNode, ByVal keyState As Integer)
            MyBase.New(aButton, aNodes)
            Me._MousePosition = aMousePos
            Me._SHIFT = (keyState And 2) = 2
            Me._CONTROL = (keyState And 8) = 8
            Me._ALT = (keyState And 32) = 32
        End Sub

#End Region

#Region " Field Declarations "
        Private _MousePosition As Point
        Private _SHIFT As Boolean
        Private _CONTROL As Boolean
        Private _ALT As Boolean
#End Region

#Region " Properties "

        ''' <summary>
        ''' Shadowed.  Gets the TreeListNodes that are being dragged.
        ''' </summary>
        ''' <value>An array of TreeListNode that are being dragged.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Item() As TreeListNode()
            Get
                Return DirectCast(MyBase.Item, TreeListNode())
            End Get
        End Property

        ''' <summary>
        ''' The current position of the mouse pointer in client coordinates.
        ''' </summary>
        ''' <value>A Point structure.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property MousePosition() As Point
            Get
                Return Me._MousePosition
            End Get
        End Property
        ''' <summary>
        ''' The current position of the mouse pointer in client coordinates.
        ''' </summary>
        ''' <value>A Point structure.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property SHIFT() As Boolean
            Get
                Return Me._SHIFT
            End Get
        End Property
        ''' <summary>
        ''' The current position of the mouse pointer in client coordinates.
        ''' </summary>
        ''' <value>A Point structure.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property CONTROL() As Boolean
            Get
                Return Me._CONTROL
            End Get
        End Property
        ''' <summary>
        ''' The current position of the mouse pointer in client coordinates.
        ''' </summary>
        ''' <value>A Point structure.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property ALT() As Boolean
            Get
                Return Me._ALT
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' TreeListViewEventArgs Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListViewEventArgs
        Inherits ContainerListViewEventArgs

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Initializes a new instance of the TreeListViewEventArgs class.
        ''' </summary>
        ''' <param name="aNode">The TreeListNode that the event is responding to.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aNode As TreeListNode)
            MyBase.New(aNode)
            Me._Action = TreeViewAction.Unknown
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the TreeListViewEventArgs class.
        ''' </summary>
        ''' <param name="aNode">The TreeListNode that the event is responding to.</param>
        ''' <param name="aAction">The type of TreeViewAction that raised the event. </param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aNode As TreeListNode, ByVal aAction As TreeViewAction)
            MyBase.New(aNode)
            Me._Action = aAction
        End Sub

#End Region

#Region " Field Declarations "
        Private _Action As TreeViewAction
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the type of action that raised the event.
        ''' </summary>
        ''' <value>The type of TreeViewAction that raised the event.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property Action() As TreeViewAction
            Get
                Return Me._Action
            End Get
        End Property

        ''' <summary>
        ''' Shadowed.  Gets the TreeListNode that has been checked, expanded, collapsed, or selected.
        ''' </summary>
        ''' <value>The TreeListNode that has been checked, expanded, collapsed, or selected.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Item() As TreeListNode
            Get
                Return DirectCast(MyBase.Item, TreeListNode)
            End Get
        End Property

#End Region

    End Class

End Namespace