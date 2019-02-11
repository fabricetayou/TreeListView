Namespace Delegates

    ''' <summary>
    ''' Delegate that handles events on the ContainerListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub ContainerListViewEventHandler(ByVal sender As Object, ByVal e As ContainerListViewEventArgs)

    ''' <summary>
    ''' Represents the method that will handle the BeforeCheck and BeforeSelect event of a ContainerListView.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub ContainerListViewCancelEventHandler(ByVal sender As Object, ByVal e As ContainerListViewCancelEventArgs)

    ''' <summary>
    ''' Handles events on the ContextMenu.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub ContextMenuEventHandler(ByVal sender As Object, ByVal e As MouseEventArgs)

    ''' <summary>
    ''' Common delegate that handles ColumnHeader events.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub HeaderMenuEventHandler(ByVal sender As Object, ByVal e As ContainerColumnHeaderEventArgs)

    ''' <summary>
    ''' Common ItemsChanged delegate used by collections.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub ItemsChangedEventHandler(ByVal sender As Object, ByVal e As ItemActionEventArgs)

    ''' <summary>
    ''' Represents the method that will handle the BeforeCheck, BeforeCollapse, BeforeExpand, or BeforeSelect event of a TreeView.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub TreeListViewCancelEventHandler(ByVal sender As Object, ByVal e As TreeListViewCancelEventArgs)

    ''' <summary>
    ''' Delegate that handles events on the TreeListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub TreeListViewEventHandler(ByVal sender As Object, ByVal e As TreeListViewEventArgs)

    ''' <summary>
    ''' Represents the method that will handle the ItemDrag event of a TreeListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub TreeListViewItemDragEventHandler(ByVal sender As Object, ByVal e As TreeListViewItemDragEventArgs)

    ''' <summary>
    ''' Represents the method that will handle the ItemDrag event of a TreeListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub TreeListViewItemDropEventHandler(ByVal sender As Object, ByVal e As TreeListViewItemDropEventArgs)

    ''' <summary>
    ''' Represents the method that will handle the BeforeLabelEdit and AfterLabelEdit events of a TreeListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Delegate Sub TreeListNodeLabelEditEventHandler(ByVal sender As Object, ByVal e As TreeListNodeLabelEditEventArgs)

End Namespace