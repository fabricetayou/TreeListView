Imports WinControls.ListView.ContainerListViewItem

Namespace Collections

    ''' <summary>
    ''' ContainerListViewObjectCollection Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ContainerListViewObjectCollection
        Inherits CollectionBase

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewObjectCollection.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewItemCollection.
        ''' </summary>
        ''' <param name="aParent">The Owner of the collection.</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal aParent As ContainerListView)
            MyBase.New()

            If (aParent Is Nothing) Then Throw New NullReferenceException("aParent cannot be Nothing.")
            Me._Parent = aParent
        End Sub

#End Region

#Region " Field Declarations "
        Protected _Parent As ContainerListView
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets a value indicating whether the collection is Read-only.
        ''' </summary>
        ''' <value><c>TRUE</c> if the collection is read-only; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        Public ReadOnly Property IsReadOnly() As Boolean
            Get
                Return MyBase.List.IsReadOnly
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Determines if an Item is in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns><c>TRUE</c> if the Item is in the collection; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal aItem As ContainerListViewObject) As Boolean
            Return MyBase.List.Contains(aItem)
        End Function

        ''' <summary>
        ''' Copies the contents of the collection to an array at the specified index.
        ''' </summary>
        ''' <param name="aArray">The array to copy the Items of the collection to.</param>
        ''' <param name="aIndex">The Index to start the copying at.</param>
        ''' <remarks></remarks>
        Public Sub CopyTo(ByVal aArray As Array, ByVal aIndex As Integer)
            MyBase.List.CopyTo(aArray, aIndex)
        End Sub

        ''' <summary>
        ''' Returns the Index of the specified Item in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns>An Integer.</returns>
        ''' <remarks></remarks>
        Public Function IndexOf(ByVal aItem As ContainerListViewObject) As Integer
            Return Me.List.IndexOf(aItem)
        End Function

        ''' <summary>
        ''' Occurs after an add or insert to the collection.
        ''' </summary>
        ''' <param name="aClObj">The Item that was added.</param>
        ''' <param name="aIndex">The Index where the Item was added.</param>
        ''' <remarks></remarks>
        Protected Overridable Sub OnAddProcessing(ByVal aClObj As ContainerListViewObject, ByVal aIndex As Integer)
            Me._validateAdd(aClObj)
        End Sub

        ''' <summary>
        ''' Overriden.  Performs additional custom processes when clearing the contents of the CollectionBase instance.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClear()
            MyBase.OnClear()

            SyncLock (Me.List.SyncRoot)
                If (Me._Parent IsNot Nothing) Then Me._Parent.BeginUpdate()
                For Each ClObj As ContainerListViewObject In Me.List
                    Me.OnRemoveProcessing(ClObj)
                Next
                If (Me._Parent IsNot Nothing) Then Me._Parent.EndUpdate()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Overriden.  Performs additional custom processes after clearing the contents of the CollectionBase instance.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClearComplete()
            MyBase.OnClearComplete()

            If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
        End Sub

        ''' <summary>
        ''' Overriden.  Performs additional custom processes after inserting a new element into the CollectionBase instance.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was added.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnInsertComplete(index, value)

            'FINALIZE THE ADD
            Me.OnAddProcessing(DirectCast(value, ContainerListViewObject), index)
            If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
        End Sub

        ''' <summary>
        ''' Overriden.  Performs additional custom processes after removing an element from the CollectionBase instance.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was removed.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemoveComplete(index, value)

            'FINALIZE THE REMOVE
            Me.OnRemoveProcessing(DirectCast(value, ContainerListViewObject))
            If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
        End Sub

        ''' <summary>
        ''' Occurs after an Item has been removed from the collection.
        ''' </summary>
        ''' <param name="aCObj">The Item that was removed from the collection.</param>
        ''' <remarks></remarks>
        Protected Overridable Sub OnRemoveProcessing(ByVal aCObj As ContainerListViewObject)
            aCObj.SetParent(Nothing)

            For Each Slvi As ContainerListViewItem.ContainerListViewSubItem In aCObj.SubItems
                If (Slvi.Control IsNot Nothing) Then
                    Slvi.Control.Parent.Controls.Remove(Slvi.Control)
                    Slvi.Control.Parent = Nothing
                    Slvi.Control.Visible = False
                End If
            Next
        End Sub

        ''' <summary>
        ''' Overriden.  Performs additional custom processes after setting a value in the CollectionBase instance.
        ''' </summary>
        ''' <param name="index">The Index of the Item changed.</param>
        ''' <param name="oldValue">The old value of the Item.</param>
        ''' <param name="newValue">The new value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            MyBase.OnSetComplete(index, oldValue, newValue)

            'DO PROCESSING FOR THE OLD VALUE
            Me.OnRemoveProcessing(DirectCast(oldValue, ContainerListViewObject))
            'DO PROCESSING FOR THE NEW VALUE
            Me.OnAddProcessing(DirectCast(newValue, ContainerListViewObject), index)
            If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
        End Sub

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to remove.</param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal aItem As ContainerListViewObject)
            Me.List.Remove(aItem)
        End Sub

        ''' <summary>
        ''' Sorts the elements in the entire Collection using the specified comparer.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Sort()
            If (Me._Parent IsNot Nothing) Then
                If (Me._Parent.SortComparer Is Nothing) Then
                    Me.InnerList.Sort(Me._Parent.DefaultComparer)
                Else
                    Me.InnerList.Sort(Me._Parent.SortComparer)
                End If
            Else
                Throw New NullReferenceException("The parent ContainerListView is NOTHING.")
            End If
        End Sub

#End Region

#Region " Procedures "

        Private Sub _validateAdd(ByVal aClObj As ContainerListViewObject)
            If (aClObj.HasParent) Then
                Me.Remove(aClObj)
                Throw New ArgumentException("Cannot add or insert the ContainerListViewObject '" & aClObj.Text & "' in more than one ContainerListViewObjectCollection. You must first remove it from its current collection or clone it.", "aClObj", Nothing)
            End If
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerListViewItem objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewItemCollection
        Inherits ContainerListViewObjectCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewObjectCollection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewItemCollection.
        ''' </summary>
        ''' <param name="aParent">The Owner of the collection.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aParent As ContainerListView)
            MyBase.New(aParent)
        End Sub

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get or set.</param>
        ''' <value></value>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal aIndex As Integer) As ContainerListViewItem
            Get
                Return DirectCast(Me.List.Item(aIndex), ContainerListViewItem)
            End Get
            Set(ByVal Value As ContainerListViewItem)
                Me.List.Item(aIndex) = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds a Item to the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to Add.</param>
        ''' <returns>An Integer representing the zero-based Index within the collection.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal aItem As ContainerListViewItem) As Integer
            Return Me.List.Add(aItem)
        End Function

        ''' <summary>
        ''' Adds a Item to the collection.
        ''' </summary>
        ''' <param name="aText">The Text of the ContainerListViewItem.</param>
        ''' <param name="aImageIndex">Optional.  A Index of the Image to display.</param>
        ''' <returns>The newly added item.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal aText As String, Optional ByVal aImageIndex As Integer = -1) As ContainerListViewItem
            Dim Itm As ContainerListViewItem

            Itm = New ContainerListViewItem(aText, aImageIndex)
            Me.Add(Itm)

            Return Itm
        End Function

        ''' <summary>
        ''' Adds an array of Items.
        ''' </summary>
        ''' <param name="Items">The Items to add.</param>
        ''' <remarks></remarks>
        Public Sub AddRange(ByVal Items() As ContainerListViewItem)
            If (Items IsNot Nothing) Then
                SyncLock (List.SyncRoot)
                    For I As Integer = 0 To Items.GetUpperBound(0)
                        Me.List.Add(Items(I))
                    Next
                End SyncLock
            End If
        End Sub

        ''' <summary>
        ''' Inserts an Item at the specified position.
        ''' </summary>
        ''' <param name="aItem">The ContainerListViewItem to insert.</param>
        ''' <param name="aIndex">The zero-based index at which the item should be inserted.</param>
        ''' <remarks></remarks>
        Public Sub Insert(ByVal aItem As ContainerListViewItem, ByVal aIndex As Integer)
            MyBase.List.Insert(aIndex, aItem)
        End Sub

        ''' <summary>
        ''' Overriden.  Occurs after an add or insert to the collection.
        ''' </summary>
        ''' <param name="aClObj">The Item that was added.</param>
        ''' <param name="aIndex">The Index where the Item was added.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnAddProcessing(ByVal aClObj As ContainerListViewObject, ByVal aIndex As Integer)
            MyBase.OnAddProcessing(aClObj, aIndex)

            aClObj.SetParent(Me._Parent)
            For Each Slvi As ContainerListViewItem.ContainerListViewSubItem In aClObj.SubItems
                If (Slvi.Control IsNot Nothing) Then
                    Slvi.Control.Parent = Me._Parent
                    Slvi.Control.Visible = True
                End If
            Next
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of TreeListNode objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListNodeCollection
        Inherits ContainerListViewObjectCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListNodeCollection.
        ''' </summary>
        ''' <param name="aParentNode">The TreeListNode that owns the collection.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aParentNode As TreeListNode)
            MyBase.New()

            If (aParentNode Is Nothing) Then Throw New NullReferenceException("aParentNode cannot be Nothing.")
            Me._PrntNode = aParentNode
        End Sub

        ''' <summary>
        ''' Creates a New instance of TreeListNodeCollection.
        ''' </summary>
        ''' <param name="aTree">The TreeListView that the collection is a part of.</param>
        ''' <param name="aVirtualNode">The VirtualRootNode that owns the collection.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aVirtualNode As TreeListNode, ByVal aTree As TreeListView)
            MyBase.New(aTree)

            If (aVirtualNode Is Nothing) Then Throw New NullReferenceException("aVirtualNode cannot be Nothing.")
            If (Not aVirtualNode.IsVirtualRootNode) Then Throw New ArgumentException("The TreeListNode must be a VirtualNode")
            Me._PrntNode = aVirtualNode
        End Sub

#End Region

#Region " Field Declarations "
        Private _PrntNode As TreeListNode
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the number of TreeListNodes in the current collection. This DOES NOT include any children of the nodes in the current collection.
        ''' </summary>
        ''' <value>An Integer representing the current count of TreeListNodes in the collection.</value>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Count() As Integer
            Get
                Return MyBase.List.Count
            End Get
        End Property

        ''' <summary>
        ''' Gets or Sets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get or set.</param>
        ''' <value></value>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal aIndex As Integer) As TreeListNode
            Get
                Return DirectCast(Me.List.Item(aIndex), TreeListNode)
            End Get
            Set(ByVal Value As TreeListNode)
                Me.List.Item(aIndex) = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aNode">The TreeListNode to Add.</param>
        ''' <returns>An Integer representing the zero-based Index within the collection.</returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal aNode As TreeListNode) As Integer
            Return Me.List.Add(aNode)
        End Function

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aText">The Text of the new TreeListNode.</param>
        ''' <param name="aImageIndex">Optional.  The ImageIndex of the TreeListNode.</param>
        ''' <param name="aSelImageIndex">Optional.  The SelectedImageIndex of the TreeListNode.</param>
        ''' <returns>A newly created TreeListNode that was just added to the collection.</returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal aText As String, Optional ByVal aImageIndex As Integer = -1, Optional ByVal aSelImageIndex As Integer = -1) As TreeListNode
            Dim Nde As New TreeListNode(aText, aImageIndex, aSelImageIndex)

            Me.Add(Nde)

            Return Nde
        End Function

        ''' <summary>
        ''' Adds an array of Items.
        ''' </summary>
        ''' <param name="aNodes">The array of TreeListNodes to add.</param>
        ''' <remarks></remarks>
        Public Sub AddRange(ByVal aNodes() As TreeListNode)
            If (aNodes IsNot Nothing) Then
                SyncLock (Me.List.SyncRoot)
                    For I As Integer = 0 To aNodes.GetUpperBound(0)
                        Me.List.Add(aNodes(I))
                    Next
                End SyncLock
            End If
        End Sub

        ''' <summary>
        ''' Inserts an Item at the specified position.
        ''' </summary>
        ''' <param name="aNode">The TreeListNode to insert.</param>
        ''' <param name="aIndex">The zero-based index at which the item should be inserted.</param>
        ''' <remarks></remarks>
        Public Sub Insert(ByVal aNode As TreeListNode, ByVal aIndex As Integer)
            MyBase.List.Insert(aIndex, aNode)
        End Sub

        ''' <summary>
        ''' Overriden.  Occurs after an add or insert to the collection.
        ''' </summary>
        ''' <param name="aClObj">The Item that was added.</param>
        ''' <param name="aIndex">The Index where the Item was added.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnAddProcessing(ByVal aClObj As ContainerListViewObject, ByVal aIndex As Integer)
            MyBase.OnAddProcessing(aClObj, aIndex)

            Dim Nde As TreeListNode = DirectCast(aClObj, TreeListNode)

            'ORDER IS IMPORTANT HERE!  ALWAYS SET THE TREE LAST!
            Nde.SetParentNode(Me._PrntNode)
            If (Me._PrntNode IsNot Nothing) Then
                If (Me._PrntNode.IsVirtualRootNode) Then
                    Nde.SetParent(Me._Parent)
                    Me._PrntNode.VisibleChildren += 1
                    Me._PrntNode.ExpandedCount += 1
                Else
                    Nde.SetParent(Me._PrntNode.ListView)
                End If

                Me._PrntNode.PropagateNodeChange(Nde.ChildrenCount + 1, Nde.VisibleChildren, Nde.ExpandedCount)
                If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Overriden.  Does post-processing before an Item has been removed.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was removed.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemove(index, value)

            If (Me._PrntNode IsNot Nothing OrElse Me._Parent IsNot Nothing) Then
                Dim Node As TreeListNode = DirectCast(value, TreeListNode)

                If (Node.Selected) Then
                    Dim TV As TreeListView

                    If (Me._Parent IsNot Nothing) Then TV = DirectCast(Me._Parent, TreeListView) Else TV = Me._PrntNode.ListView

                    If (TV IsNot Nothing AndAlso (Not TV.MultiSelect OrElse (TV.MultiSelect AndAlso Node.Focused))) Then
                        Dim TempNode As TreeListNode = TV.GetPreviousNode(Node)

                        If (TempNode IsNot Nothing) Then
                            TempNode.Selected = True
                            If (Not TempNode.Focused) Then TempNode.Focused = Node.Focused
                        Else
                            TempNode = TV.GetNextNode(Node)
                            If (TempNode IsNot Nothing) Then
                                TempNode.Selected = True
                                If (Not TempNode.Focused) Then TempNode.Focused = Node.Focused
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Overriden.  Occurs after an Item has been removed from the collection.
        ''' </summary>
        ''' <param name="aCObj">The Item that was removed from the collection.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemoveProcessing(ByVal aCObj As ContainerListViewObject)
            Dim Nde As TreeListNode = DirectCast(aCObj, TreeListNode)

            'ORDER IS IMPORTANT HERE!  ALWAYS SET THE TREE LAST!
            Nde.SetParentNode(Nothing)
            MyBase.OnRemoveProcessing(aCObj)

            If (Me._PrntNode IsNot Nothing) Then
                'SPECIAL CASE WHEN THE PARENT IS A VIRTUALROOTNODE
                If (Me._PrntNode.IsVirtualRootNode) Then
                    Me._PrntNode.VisibleChildren -= 1
                    Me._PrntNode.ExpandedCount -= 1
                End If


                '(THIS WAS THE ORIGINAL.  CHANGED ON 5/16/2005 BY TED):  Me._PrntNode.PropagateNodeChange(Nde.ChildrenCount - 1, Nde.VisibleChildren, Nde.ExpandedCount)
                Me._PrntNode.PropagateNodeChange(Nde.ChildrenCount - 1, -(Nde.VisibleChildren), -(Nde.ExpandedCount))
                If (Me._Parent IsNot Nothing AndAlso Not Me._Parent.InUpdateMode) Then Me._Parent.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Overriden.  Sorts the elements in the entire Collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Sort()
            If (Me._PrntNode IsNot Nothing AndAlso Not Me._PrntNode.ListView Is Nothing) Then
                If (Me._PrntNode.ListView.SortComparer Is Nothing) Then
                    Me.InnerList.Sort(Me._PrntNode.ListView.DefaultComparer)
                Else
                    Me.InnerList.Sort(Me._PrntNode.ListView.SortComparer)
                End If
            Else
                MyBase.Sort()
            End If
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerListViewReadOnlyCollection objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ContainerListViewReadOnlyCollection
        Inherits ReadOnlyCollectionBase

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewReadOnlyCollection.
        ''' </summary>
        ''' <param name="aListView">The ContainerListView the collection belongs to.</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal aListView As ContainerListView)
            MyBase.New()

            If (aListView Is Nothing) Then Throw New NullReferenceException("aListView cannot be nothing")
            Me._Owner = aListView
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewReadOnlyCollection.
        ''' </summary>
        ''' <param name="aParent">The ContainerListViewObject the collection belongs to.</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal aParent As ContainerListViewObject)
            MyBase.New()

            If (aParent Is Nothing) Then Throw New NullReferenceException("aParent cannot be nothing")
            Me._Parent = aParent
        End Sub

#End Region

#Region " Field Declarations "
        ''' <summary>
        ''' Variable that stores the ContainerListView (if any) that owns this collection.
        ''' </summary>
        ''' <remarks></remarks>
        Protected _Owner As ContainerListView
        ''' <summary>
        ''' Variable that stores the ContainerListViewObject that is the parent of this collection.
        ''' </summary>
        ''' <remarks></remarks>
        Protected _Parent As ContainerListViewObject
#End Region

#Region " Methods "

        ''' <summary>
        ''' Removes all Items from the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Friend Overridable Sub Clear()
            MyBase.InnerList.Clear()
        End Sub

        ''' <summary>
        ''' Copies the entire Collection to the compatible array starting at the specified Index.
        ''' </summary>
        ''' <param name="aArray">The Array to copy all Items of the collection to.</param>
        ''' <param name="aIndex">The Index to start copying at.</param>
        ''' <remarks></remarks>
        Public Sub CopyTo(ByVal aArray As Array, ByVal aIndex As Integer)
            MyBase.InnerList.CopyTo(aArray, aIndex)
        End Sub

        ''' <summary>
        ''' Determines if an Item is in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns><c>TRUE</c> if the Item is in the collection; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal aItem As Object) As Boolean
            Return MyBase.InnerList.Contains(aItem)
        End Function

        ''' <summary>
        ''' Returns the Index of the specified Item in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns>An Integer.</returns>
        ''' <remarks></remarks>
        Public Function IndexOf(ByVal aItem As Object) As Integer
            Return MyBase.InnerList.IndexOf(aItem)
        End Function

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of HiddenColumnsCollection ContainerColumnHeaders.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class HiddenColumnsCollection
        Inherits ContainerListViewReadOnlyCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of SelectedIndexCollection.
        ''' </summary>
        ''' <param name="aListView">The ContainerListView the collection belongs to.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aListView As ContainerListView)
            MyBase.New(aListView)
        End Sub

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get.</param>
        ''' <value>A ContainerColumnHeader at the specified index</value>
        ''' <remarks></remarks>
        Default Public ReadOnly Property Item(ByVal aIndex As Integer) As ContainerColumnHeader
            Get
                Return DirectCast(Me.InnerList.Item(aIndex), ContainerColumnHeader)
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aItem">The ContainerColumnHeader to add to the collection.</param>
        ''' <returns>The Index of the Item in the collection; otherwise -1 if the Item was not added.</returns>
        ''' <remarks></remarks>
        Friend Function Add(ByVal aItem As ContainerColumnHeader) As Integer
            If (aItem IsNot Nothing AndAlso Not Me.Contains(aItem)) Then Return Me.InnerList.Add(aItem)

            Return -1
        End Function

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aItem">The ContainerColumnHeader to remove.</param>
        ''' <remarks></remarks>
        Friend Sub Remove(ByVal aItem As ContainerColumnHeader)
            If (aItem IsNot Nothing AndAlso Me.Contains(aItem)) Then Me.InnerList.Remove(aItem)
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerListViewItem objects that are in a checked state.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CheckedContainerListViewObjectCollection
        Inherits ContainerListViewReadOnlyCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of CheckedContainerListViewItemCollection.
        ''' </summary>
        ''' <param name="aListView">The ContainerListView the collection belongs to.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aListView As ContainerListView)
            MyBase.New(aListView)
        End Sub

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get.</param>
        ''' <value>A ContainerListViewItem at the specified index</value>
        ''' <remarks></remarks>
        Default Public ReadOnly Property Item(ByVal aIndex As Integer) As ContainerListViewObject
            Get
                Return DirectCast(Me.InnerList.Item(aIndex), ContainerListViewItem)
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aClObj">The ContainerListViewObject to add to the collection.</param>
        ''' <returns>The Index of the ContainerListViewObject in the collection; otherwise -1 if the ContainerListViewObject was not added.</returns>
        ''' <remarks></remarks>
        Friend Function Add(ByVal aClObj As ContainerListViewObject) As Integer
            If (aClObj IsNot Nothing AndAlso Not Me.Contains(aClObj)) Then Return Me.InnerList.Add(aClObj)

            Return -1
        End Function

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aClObj">The ContainerListViewObject to remove.</param>
        ''' <remarks></remarks>
        Friend Sub Remove(ByVal aClObj As ContainerListViewObject)
            If (aClObj IsNot Nothing AndAlso Me.Contains(aClObj)) Then Me.InnerList.Remove(aClObj)
        End Sub

        ''' <summary>
        ''' Removes all Items from the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows Sub Clear()
            For I As Integer = (MyBase.InnerList.Count - 1) To 0 Step -1
                DirectCast(MyBase.InnerList(I), ContainerListViewObject).Checked = False
            Next
            MyBase.InnerList.Clear()
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerColumnHeader objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerColumnHeaderCollection
        Inherits CollectionBase

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of SelectedContainerListViewItemCollection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Creates a New instance of SelectedContainerListViewItemCollection.
        ''' </summary>
        ''' <param name="aParent">The owner of the collection.</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal aParent As ContainerListView)
            MyBase.New()

            If (aParent Is Nothing) Then Throw New NullReferenceException("aParent cannot be Nothing.")
            Me._Parent = aParent
        End Sub

#End Region

#Region " Field Declarations "
        Private _Parent As ContainerListView
#End Region

#Region " Events "

        ''' <summary>
        ''' Occurs when an has been added, removed, changed, or cleared from the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Friend Event ItemsChanged As ItemsChangedEventHandler

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get or set.</param>
        ''' <value></value>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal aIndex As Integer) As ContainerColumnHeader
            Get
                Return DirectCast(Me.List.Item(aIndex), ContainerColumnHeader)
            End Get
            Set(ByVal Value As ContainerColumnHeader)
                Me.List.Item(aIndex) = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds a Item to the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to Add.</param>
        ''' <returns>An Integer representing the zero-based Index within the collection.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal aItem As ContainerColumnHeader) As Integer
            Return Me.List.Add(aItem)
        End Function

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aText">The Title of the Column.</param>
        ''' <param name="aWidth">The Width of the Column.</param>
        ''' <param name="aAlign">The Text alignment.</param>
        ''' <returns>A newly created ContainerColumnHeader object.</returns>
        ''' <remarks></remarks>
        Public Overloads Function Add(ByVal aText As String, ByVal aWidth As Integer, ByVal aAlign As HorizontalAlignment) As ContainerColumnHeader
            Dim CH As New ContainerColumnHeader

            With CH
                .Text = aText
                .Width = aWidth
                .TextAlign = aAlign
            End With

            SyncLock (Me.List.SyncRoot)
                Me.List.Add(CH)
            End SyncLock

            Return CH
        End Function

        ''' <summary>
        ''' Adds an array of Items.
        ''' </summary>
        ''' <param name="Items">The Items to add.</param>
        ''' <remarks></remarks>
        Public Sub AddRange(ByVal Items() As ContainerColumnHeader)
            If (Items IsNot Nothing) Then
                SyncLock (Me.List.SyncRoot)
                    For I As Integer = 0 To Items.GetUpperBound(0)
                        Me.List.Add(Items(I))
                    Next
                End SyncLock
            End If
        End Sub

        ''' <summary>
        ''' Determines if an Item is in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns><c>TRUE</c> if the Item is in the collection; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal aItem As ContainerColumnHeader) As Boolean
            Return Me.List.Contains(aItem)
        End Function

        ''' <summary>
        ''' Returns the Index of the specified Item in the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to search for.</param>
        ''' <returns>An Integer.</returns>
        ''' <remarks></remarks>
        Public Function IndexOf(ByVal aItem As ContainerColumnHeader) As Integer
            Return Me.List.IndexOf(aItem)
        End Function

        ''' <summary>
        ''' Inserts an Item at the specified position.
        ''' </summary>
        ''' <param name="aItem">The ContainerColumnHeader to insert.</param>
        ''' <param name="aIndex">The zero-based index at which the item should be inserted.</param>
        ''' <remarks></remarks>
        Public Sub Insert(ByVal aItem As ContainerColumnHeader, ByVal aIndex As Integer)
            MyBase.List.Insert(aIndex, aItem)
        End Sub

        ''' <summary>
        ''' Does pre-processing of items in the list before removing them.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClear()
            MyBase.OnClear()

            For Each CH As ContainerColumnHeader In Me.List
                Me._removeProcessing(CH)
            Next

            RaiseEvent ItemsChanged(Me, New ItemActionEventArgs(-1, Enums.CollectionActions.Clearing, Me.List))
        End Sub

        ''' <summary>
        ''' Raises the ItemsChanged event.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClearComplete()
            MyBase.OnClearComplete()

            RaiseEvent ItemsChanged(Me, New ItemActionEventArgs(-1, Enums.CollectionActions.Cleared, Nothing))
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item has been added.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was added.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnInsertComplete(index, value)

            Me._addProcessing(DirectCast(value, ContainerColumnHeader), index)
            RaiseEvent ItemsChanged(Me, New ItemActionEventArgs(index, Enums.CollectionActions.Added, value))
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item has been removed.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was removed.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemoveComplete(index, value)

            Me._removeProcessing(DirectCast(value, ContainerColumnHeader))
            RaiseEvent ItemsChanged(Me, New ItemActionEventArgs(index, Enums.CollectionActions.Removed, value))
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item's value has been changed.
        ''' </summary>
        ''' <param name="index">The Index of the Item changed.</param>
        ''' <param name="oldValue">The old value of the Item.</param>
        ''' <param name="newValue">The new value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            MyBase.OnSetComplete(index, oldValue, newValue)

            Me._removeProcessing(DirectCast(oldValue, ContainerColumnHeader))
            Me._addProcessing(DirectCast(newValue, ContainerColumnHeader), index)

            RaiseEvent ItemsChanged(Me, New ItemActionEventArgs(index, Enums.CollectionActions.Changed, newValue, oldValue))
        End Sub

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aItem">The Item to remove.</param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal aItem As ContainerColumnHeader)
            Me.List.Remove(aItem)
        End Sub

#End Region

#Region " Procedures "

        Private Sub _addProcessing(ByVal aColHdr As ContainerColumnHeader, ByVal aIndex As Integer)
            'LETS CHECK TO MAKE SURE THAT THE COLUMN IS NOT ALREADY ASSIGNED TO ANOTHER COLLECTION.  IF SO, THROW AN ERROR
            If (aColHdr.ListView Is Nothing) Then
                aColHdr.SetParent(Me._Parent)
            Else
                Me.Remove(aColHdr)
                Throw New ArgumentException("Cannot add or insert the ContainerColumnHeader '" & aColHdr.Text & "' in more than one ContainerColumnHeaderCollection. You must first remove it from its current collection or clone it.", "aColHdr", Nothing)
            End If
        End Sub

        Private Sub _removeProcessing(ByVal aClvi As ContainerColumnHeader)
            aClvi.SetParent(Nothing)
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerListViewSubItem objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewSubItemCollection
        Inherits CollectionBase

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItemCollection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItemCollection.
        ''' </summary>
        ''' <param name="aParent">The Parent this collection belongs to.</param>
        ''' <remarks></remarks>
        Protected Friend Sub New(ByVal aParent As ContainerListViewObject)
            If (aParent Is Nothing) Then Throw New NullReferenceException("aParent cannot be Nothing.")
            Me._Parent = aParent
        End Sub

#End Region

#Region " Field Declarations "
        Private _Parent As ContainerListViewObject
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets the SubItem in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get or set.</param>
        ''' <value></value>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal aIndex As Integer) As ContainerListViewSubItem
            Get
                Return DirectCast(Me.List.Item(aIndex), ContainerListViewSubItem)
            End Get
            Set(ByVal Value As ContainerListViewSubItem)
                Me.List.Item(aIndex) = Value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds a SubItem to the collection.
        ''' </summary>
        ''' <param name="aSubItem">The SubItem to Add.</param>
        ''' <returns>An Integer representing the zero-based Index within the collection.</returns>
        ''' <remarks></remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="a")> Public Function Add(ByVal aSubItem As ContainerListViewSubItem) As Integer
            Return Me.List.Add(aSubItem)
        End Function

        ''' <summary>
        ''' Adds a SubItem to the collection.
        ''' </summary>
        ''' <param name="aText">The Text to create a New SubItem for.</param>
        ''' <returns>A newly created ContainerListViewSubItem.</returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal aText As String) As ContainerListViewSubItem
            Dim Slvi As New ContainerListViewSubItem(aText)

            SyncLock (Me.List.SyncRoot)
                Me.List.Add(Slvi)
            End SyncLock

            Return Slvi
        End Function

        ''' <summary>
        ''' Adds a SubItem to the collection.
        ''' </summary>
        ''' <param name="aControl">The control to create a New SubItem for.</param>
        ''' <returns>A newly created ContainerListViewSubItem.</returns>
        ''' <remarks></remarks>
        Public Function Add(ByVal aControl As Control) As ContainerListViewSubItem
            Dim Slvi As New ContainerListViewSubItem(aControl)

            SyncLock (Me.List.SyncRoot)
                Me.List.Add(Slvi)
            End SyncLock

            Return Slvi
        End Function

        ''' <summary>
        ''' Adds an array of Items.
        ''' </summary>
        ''' <param name="Items">The Items to add.</param>
        ''' <remarks></remarks>
        Public Sub AddRange(ByVal Items() As ContainerListViewSubItem)
            If (Items IsNot Nothing) Then
                SyncLock (Me.List.SyncRoot)
                    For I As Integer = 0 To Items.GetUpperBound(0)
                        Me.List.Add(Items(I))
                    Next
                End SyncLock
            End If
        End Sub

        ''' <summary>
        ''' Determines if an Item is in the collection.
        ''' </summary>
        ''' <param name="aSubItem">The Item to search for.</param>
        ''' <returns><c>TRUE</c> if the Item is in the collection; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        Public Function Contains(ByVal aSubItem As ContainerListViewSubItem) As Boolean
            Return Me.List.Contains(aSubItem)
        End Function

        ''' <summary>
        ''' Copies the contents of the collection to an array at the specified index.
        ''' </summary>
        ''' <param name="aArray">The array to copy the Items of the collection to.</param>
        ''' <param name="aIndex">The Index to start the copying at.</param>
        ''' <remarks></remarks>
        Public Sub CopyTo(ByVal aArray As Array, ByVal aIndex As Integer)
            MyBase.List.CopyTo(aArray, aIndex)
        End Sub

        ''' <summary>
        ''' Returns the Index of the specified Item in the collection.
        ''' </summary>
        ''' <param name="aSubItem">The Item to search for.</param>
        ''' <returns>An Integer.</returns>
        ''' <remarks></remarks>
        Public Function IndexOf(ByVal aSubItem As ContainerListViewSubItem) As Integer
            Return Me.List.IndexOf(aSubItem)
        End Function

        ''' <summary>
        ''' Inserts an Item at the specified position.
        ''' </summary>
        ''' <param name="aSubItem">The ContainerListViewSubItem to insert.</param>
        ''' <param name="aIndex">The zero-based index at which the item should be inserted.</param>
        ''' <remarks></remarks>
        Public Sub Insert(ByVal aSubItem As ContainerListViewSubItem, ByVal aIndex As Integer)
            MyBase.List.Insert(aIndex, aSubItem)
        End Sub

        ''' <summary>
        ''' Does pre-processing of items in the list before removing them.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClear()
            MyBase.OnClear()

            For Each Slvi As ContainerListViewSubItem In Me.List
                Me._removeProcessing(Slvi)
            Next
        End Sub

        ''' <summary>
        ''' Raises the ItemsChanged event.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub OnClearComplete()
            MyBase.OnClearComplete()
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item has been added.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was added.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnInsertComplete(index, value)

            Me._addProcessing(DirectCast(value, ContainerListViewSubItem), index)
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item has been removed.
        ''' </summary>
        ''' <param name="index">The Index of the Item that was removed.</param>
        ''' <param name="value">The Value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemoveComplete(index, value)

            Me._removeProcessing(DirectCast(value, ContainerListViewSubItem))
        End Sub

        ''' <summary>
        ''' Does post-processing after an Item's value has been changed.
        ''' </summary>
        ''' <param name="index">The Index of the Item changed.</param>
        ''' <param name="oldValue">The old value of the Item.</param>
        ''' <param name="newValue">The new value of the Item.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            MyBase.OnSetComplete(index, oldValue, newValue)

            Me._removeProcessing(DirectCast(oldValue, ContainerListViewSubItem))
            Me._addProcessing(DirectCast(newValue, ContainerListViewSubItem), index)
        End Sub

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aSubItem">The Item to remove.</param>
        ''' <remarks></remarks>
        Public Sub Remove(ByVal aSubItem As ContainerListViewSubItem)
            Me.List.Remove(aSubItem)
        End Sub

#End Region

#Region " Procedures "

        Private Sub _addProcessing(ByVal aSubItem As ContainerListViewSubItem, ByVal aIndex As Integer)
            'LETS CHECK TO MAKE SURE THAT THE ITEM IS NOT ALREADY ASSIGNED TO ANOTHER COLLECTION.  IF SO, THROW AN ERROR
            If (aSubItem.Parent Is Nothing) Then
                If (Me._Parent IsNot Nothing) Then
                    aSubItem.SetParent(Me._Parent)
                    aSubItem.SetParentOwner(Me._Parent.ListView)
                End If
            Else
                Me.Remove(aSubItem)
                Throw New ArgumentException("Cannot add or insert the ContainerListViewSubItem '" & aSubItem.Text & "' in more than one ContainerListViewSubItemCollection. You must first remove it from its current collection or clone it.", "aSubItem", Nothing)
            End If
        End Sub

        Private Sub _removeProcessing(ByVal aSubItem As ContainerListViewSubItem)
            'BECAUSE THE UNDERLYING PARENT BEING SET IN THE CONTAINERLISTVIEWITEM IS OBJECT, IT DOESN'T MATTER WHAT OVERLOADED
            'PARENT WE USE TO SET IT TO <c>NOTHING</c>.
            aSubItem.SetParent(Nothing)
            aSubItem.SetParentOwner(Nothing)
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of ContainerListViewItem objects that are in a selected state.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SelectedContainerListViewObjectCollection
        Inherits ContainerListViewReadOnlyCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of SelectedContainerListViewItemCollection.
        ''' </summary>
        ''' <param name="aListView">The ContainerListView the collection belongs to.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aListView As ContainerListView)
            MyBase.New(aListView)
        End Sub

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get.</param>
        ''' <value>A ContainerListViewObject at the specified index</value>
        ''' <remarks></remarks>
        Default Public ReadOnly Property Item(ByVal aIndex As Integer) As ContainerListViewObject
            Get
                Return DirectCast(MyBase.InnerList.Item(aIndex), ContainerListViewObject)
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aClObj">The ContainerListViewObject to add to the collection.</param>
        ''' <returns>The Index of the ContainerListViewObject in the collection; otherwise -1 if the ContainerListViewObject was not added.</returns>
        ''' <remarks></remarks>
        Friend Function Add(ByVal aClObj As ContainerListViewObject) As Integer
            If (aClObj IsNot Nothing AndAlso Not Me.Contains(aClObj)) Then Return Me.InnerList.Add(aClObj)

            Return -1
        End Function

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aClObj">The ContainerListViewObject to remove.</param>
        ''' <remarks></remarks>
        Friend Sub Remove(ByVal aClObj As ContainerListViewObject)
            If (aClObj IsNot Nothing AndAlso Me.Contains(aClObj)) Then Me.InnerList.Remove(aClObj)
        End Sub

        ''' <summary>
        ''' Removes all Items from the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows Sub Clear()
            For I As Integer = (MyBase.InnerList.Count - 1) To 0 Step -1
                DirectCast(MyBase.InnerList(I), ContainerListViewObject).Selected = False
            Next
            MyBase.InnerList.Clear()
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Strongly typed collection of Integers (representing Indexes) of ContainerListViewItem objects that are in a selected state.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SelectedIndexCollection
        Inherits ContainerListViewReadOnlyCollection

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of SelectedIndexCollection.
        ''' </summary>
        ''' <param name="aListView">The ContainerListView the collection belongs to.</param>
        ''' <remarks></remarks>
        Friend Sub New(ByVal aListView As ContainerListView)
            MyBase.New(aListView)
        End Sub

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the Item in the Collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the item to get.</param>
        ''' <value>An Integer representing the Index of the SelectedItem.</value>
        ''' <remarks></remarks>
        Default Public ReadOnly Property Item(ByVal aIndex As Integer) As Integer
            Get
                Return DirectCast(Me.InnerList.Item(aIndex), Integer)
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an Item to the collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the ContainerListViewItem to add to the collection.</param>
        ''' <returns>The Index of the ContainerListViewItem in the collection; otherwise -1 if the ContainerListViewItem was not added.</returns>
        ''' <remarks></remarks>
        Friend Function Add(ByVal aIndex As Integer) As Integer
            If (aIndex >= 0 AndAlso Not Me.Contains(aIndex)) Then Return Me.InnerList.Add(aIndex)

            Return -1
        End Function

        ''' <summary>
        ''' Removes an Item from the collection.
        ''' </summary>
        ''' <param name="aIndex">The Index of the ContainerListViewItem to remove.</param>
        ''' <remarks></remarks>
        Friend Sub Remove(ByVal aIndex As Integer)
            If (aIndex >= 0 AndAlso Me.Contains(aIndex)) Then Me.InnerList.Remove(aIndex)
        End Sub

#End Region

    End Class

End Namespace