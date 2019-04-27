
''' <summary>
''' ContainerListView Control.
''' Provides a listview control in detail mode that provides containers for each cell in a row/column.
''' </summary>
''' <remarks>The container can hold almost any object that derives directly or indirectly from Control.</remarks>
<DefaultProperty("Items"), _
 DefaultEvent("AfterSelect"), _
 XmlSchemaProvider("CreateSchema"), _
 Designer(GetType(ContainerListViewDesigner)), _
 ToolboxBitmap(GetType(ContainerListView), "ContainerListView.png")> _
Public Class ContainerListView
    Inherits Control
    Implements IXmlSerializable

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New Instance of ContainerListView.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        Me._preInit()
    End Sub

    ''' <summary>Form overrides dispose to clean up the component list.
    ''' </summary>
    ''' <param name="disposing"><c>TRUE</c> if disposing; otherwise <c>FALSE</c>.</param>
    ''' <remarks></remarks>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing) Then Me._detachHandlers()
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region " Field Declarations "
    Private _Activation As ItemActivation = ItemActivation.Standard
    Private _AllowCheckBoxes As Boolean
    Private _AllowMultiSelectActivation As Boolean
    Private _AllowColumnResize As Boolean = True
    Private _AlphaComponent As Integer = 65
    Private _Borderstyle As BorderStyleType = Enums.BorderStyleType.Fixed3D
    Private _BorderWidth As Integer = 2
    Private _Brush_ColTracking As SolidBrush
    Private _CheckBoxSelection As ItemActivation = ItemActivation.Standard
    Private _CheckBoxStyle As CheckBoxStyle = CheckBoxStyle.Flat
    Private _CheckBoxType As CheckBoxType = CheckBoxType.CheckBox
    Private _CheckedItems As CheckedContainerListViewObjectCollection
    Private _ColScalePos As Integer
    Private _ColScaleWid As Integer
    Private _ColSortColor As Color = Color.PaleGoldenrod
    Private _ColSortEnabled As Boolean
    Private _ColTrackColor As Color = Color.WhiteSmoke
    Private _Columns As New ContainerColumnHeaderCollection(Me)
    Private _DisabledColor As Color = Color.Gainsboro
    Private _DoColTracking As Boolean
    Private _DoRowTracking As Boolean
    Private _EditBackColor As Color = SystemColors.Info
    Private _EditingObj As ContainerListViewObject
    Private _EnsureVisible As Boolean = True
    Private _FirstSelected As Integer = -1
    Private _FocusedIndex As Integer = -1
    Private _FocusedItem As ContainerListViewItem
    Private _FullRowSelect As Boolean = True
    Private _GridLineColor As Color = Color.Silver
    Private _GridLinePen As Pen
    Private _GridLineType As GridLineSelections = GridLineSelections.Both
    Private _HeaderMenu As ContextMenuStrip
    Private _HeaderRect As Rectangle
    Private _HeaderStyle As ColumnHeaderStyle = ColumnHeaderStyle.Clickable
    Private _HdrBuffer As Integer = 20
    Private _HeaderHeight As Integer = _HdrBuffer
    Private _HideCols As Boolean = True
    Private _HideSelection As Boolean
    Private _HiddenCols As HiddenColumnsCollection
    Private _HoverSelection As Boolean
    Private _HscrollBar As New HScrollBar
    Private _ImageList As ImageList
    Private _IsFocused As Boolean
    Private _Items As New ContainerListViewItemCollection(Me)
    Private _LastClickedPoint As Point
    Private _LastColPressed As ContainerColumnHeader
    Private _LastRowHovered As ContainerListViewItem
    Private _LineEnd As Point
    Private _LineStart As Point
    Private _Logging As Boolean
    Private _MultiSelect As Boolean
    Private _ResizeCursor As Cursor = Cursors.VSplit
    Private _RightMouseSelects As Boolean = True
    Private _RowHeight As Integer = 14
    Private _RowTrackColor As Color = Color.WhiteSmoke
    Private _RowSelectColor As Color = SystemColors.Highlight
    Private _RowsRect As Rectangle
    Private _ScaledCol As Integer = -1
    Private _Scrollable As Boolean = True
    Private _SelectedIndexes As New SelectedIndexCollection(Me)
    Private _SelectedItems As New SelectedContainerListViewObjectCollection(Me)
    Private _SortComparer As IComparer
    Private _Sorting As SortOrder = SortOrder.None
    Private _UpdateTransactions As Boolean
    Private _VisualStyles As Boolean = True
    Private _VscrollBar As New VScrollBar

    'TODO:  Not Supported yet.  ONLY IMPLEMENTED AS PROPERTIES.
    Private _AllowColumnReorder As Boolean
    Private _TrackHistory As Boolean

    ''' <summary>
    ''' The variable that represents the HashTable used to hold the Rectangles of the CheckBoxes.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _CheckBoxRects As New Hashtable
    ''' <summary>
    ''' The variable that represents whether Column scaling is set.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _ColScaleMode As Boolean
    ''' <summary>
    ''' The variable that represents the default IComparer when sorting.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _DefaultComparer As New ContainerListViewComparer
    ''' <summary>
    ''' The variable that represents the last GridColumn that was hovered over by the mouse.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _LastColHovered As ContainerColumnHeader
    ''' <summary>
    ''' The variable that represents the last GridColumn that was selected and painted.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _LastPaintedSortCol As Integer = -1
    ''' <summary>
    ''' The variable that represents how, if enabled, a user is selecting multiple items.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _MultiSelectMode As MultiSelectModes = MultiSelectModes.Single
    ''' <summary>
    ''' Used by derived classes that override the OnMouseDown event.  This will prevent the base ContainerListView class from 
    ''' processing any row clicks that the derived classes need to handle.
    ''' </summary>
    ''' <remarks>Yeah, I know, kind of a cheesy hack but my brain can't think of how to handle it differently right now.</remarks>
    Protected _ProcessRows As Boolean = True
    ''' <summary>
    ''' The variable that represents the last selected GridColumn for sorting purposes.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _SelectedCol As ContainerColumnHeader

    ''' <summary>
    ''' Enumerated type specifying what GridLines to show.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum GridLineSelections
        ''' <summary>
        ''' Do not show any gridlines.
        ''' </summary>
        ''' <remarks></remarks>
        None
        ''' <summary>
        ''' Show both Column and Row gridlines.
        ''' </summary>
        ''' <remarks></remarks>
        Both
        ''' <summary>
        ''' Show only Column gridlines.
        ''' </summary>
        ''' <remarks></remarks>
        Column
        ''' <summary>
        ''' Show only Row gridlines.
        ''' </summary>
        ''' <remarks></remarks>
        Row
    End Enum

    ''' <summary>
    ''' Enumerated values specifying possible Selection modes.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Enum MultiSelectModes
        ''' <summary>
        ''' The control only allows Single row selection.
        ''' </summary>
        ''' <remarks></remarks>
        [Single]
        ''' <summary>
        ''' A range of rows can be selected by holding down the Shift key, selecting an individual row (with mouse) and then
        ''' selecting another row (with mouse).  The rows inbetween the two initially selected rows will also be selected.
        ''' </summary>
        ''' <remarks></remarks>
        Range
        ''' <summary>
        ''' Multiple rows can be selected by holding down the Ctrl key and selecting the desired rows with the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        Selective
    End Enum

#End Region

#Region " Events "

    ''' <summary>
    ''' Occurs after the state of the CheckBox changes on a ContainerListViewObject.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AfterCheckStateChanged As ContainerListViewEventHandler

    ''' <summary>
    ''' Occurs after the ContainerListViewObject or one of it's SubItems has been edited by the user.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AfterEdit As ContainerListViewEventHandler

    ''' <summary>
    ''' Occurs after the ContainerListViewObject is selected.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AfterSelect As ContainerListViewEventHandler

    ''' <summary>
    ''' Occurs before the state of the CheckBox changes on a ContainerListViewObject.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeforeCheckStateChanged As ContainerListViewCancelEventHandler

    ''' <summary>
    ''' Occurs when the user starts editing the label of a ContainerListViewObject or one of it's SubItems.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeforeEdit As ContainerListViewCancelEventHandler

    ''' <summary>
    ''' Occurs before the ContainerListViewObject is selected.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeforeSelect As ContainerListViewCancelEventHandler

    ''' <summary>
    ''' Occurs when a ColumnHeader is clicked. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ColumnHeaderClicked As HeaderMenuEventHandler

    ''' <summary>
    ''' Occurs when the value of the ContextMenu property changes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ColumnHeaderContextMenuChanged As EventHandler

    ''' <summary>
    ''' Occurs right before the ContextMenu for the ContainerColumnHeaders is be displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ColumnHeaderContextMenuRequested As ContextMenuEventHandler

    ''' <summary>
    ''' Occurs right before the ContextMenu for the control is displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ContextMenuRequested As ContextMenuEventHandler

    ''' <summary>
    ''' Occurs when an item is activated.
    ''' </summary>
    ''' <remarks>
    ''' The ItemActivate event occurs when the user activates one or more items in the ContainerListView control. 
    ''' The user can activate an item with either a single-click or double-click, or, depending on the value of the 
    ''' ActivationType property, with the keyboard (Standard). From within the event handler for the ItemActivate event, 
    ''' you can reference the SelectedItems or SelectedIndexes properties to access the collection of items selected in the ContainerListView to determine which items are being activated.
    ''' </remarks>
    Public Event ItemActivate As EventHandler

#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or Sets a value that specifies the action that activates an item.
    ''' </summary>
    ''' <value>A ItemActivation enumeration.  The default is ItemActivation.Standard</value>
    ''' <remarks>
    ''' The ActivationType property allows you to specify how the user will activate an item in the ContainerListView control. Activating an item in a ContainerListView 
    ''' is different from simply selecting an item. When an item is selected, an action is typically performed in an event handler for the ItemActivate event. For example, 
    ''' when an item is activated you might open a file or display a dialog box that allows the item to be edited. Typically, an item is double-clicked by the user to 
    ''' activate it. If the Activation property is set to ItemActivation.OneClick, clicking the item once activates it. Setting the Activation property to 
    ''' ItemActivation.TwoClick is different from the standard double-click because the two clicks can have any duration between them.  If the Activation property is set
    ''' to ItemActivation.Standard, highlighting the Node and pressing Enter/Return will activate the item(s).  If AllowMultiSelectActivation property is set to <c>TRUE</c>, 
    ''' the ItemActivate event will only fire if the ActivationType is set to ItemActivation.Standard.
    ''' </remarks>
    <Category("Behavior"), _
     DefaultValue(GetType(ItemActivation), "Standard"), _
     Description("Specifies the action that activates an item.")> _
    Public Property ActivationType() As ItemActivation
        Get
            Return Me._Activation
        End Get
        Set(ByVal Value As ItemActivation)
            Me._Activation = Value
        End Set
    End Property

    ''' <summary>
    ''' <c>NOT SUPPORTED</c>. 
    ''' Gets or Sets a value that specifies whether ColumnHeaders may be reordered.
    ''' </summary>
    ''' <value><c>TRUE</c> if reordering of columns is allowed; otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Not Supported"), _
     DefaultValue(False), _
     Description("Not Supported." & ControlChars.CrLf & "Specifies whether ColumnHeaders may be reordered.")> _
    Public Property AllowColumnReOrder() As Boolean
        Get
            Return Me._AllowColumnReorder
        End Get
        Set(ByVal Value As Boolean)
            Me._AllowColumnReorder = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether Columns can be resized by the user.
    ''' </summary>
    ''' <value><c>TRUE</c> if a user is allowed to resize columns; otherwise <c>FALSE</c>.  The default is <c>TRUE</c>.</value>
    ''' <remarks>This property enables/disables column resizing on all columns.  Individual columns can be enabled/disabled on the ColumnHeader itself.</remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether Columns can be resized by the user.")> _
    Public Property AllowColumnResize() As Boolean
        Get
            Return Me._AllowColumnResize
        End Get
        Set(ByVal Value As Boolean)
            Me._AllowColumnResize = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the control will Allow Columns to be Hidden.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control will allow Columns to be hidden; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines if the control will Allow Columns to be Hidden.")> _
    Public Property AllowHiddenColumns() As Boolean
        Get
            Return Me._HideCols
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._HideCols.Equals(Value)) Then
                Me._HideCols = Value

                If (Me._HideCols = False) Then
                    Me.BeginUpdate()
                    For I As Integer = (Me.HiddenColumns.Count - 1) To 0 Step -1
                        Me.HiddenColumns(I).Hidden = False
                    Next
                    Me.EndUpdate()
                End If

                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the ItemActivation event fires if a user is in MultiSelect mode and presses the Enter/Return key.
    ''' </summary>
    ''' <value><c>TRUE</c> if ItemActivation fires on Enter/Return key while in MultiSelect mode;  otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determines if the ItemActivation event fires if a user is in MultiSelect mode and presses the Enter/Return key.")> _
    Public Property AllowMultiSelectActivation() As Boolean
        Get
            Return Me._AllowMultiSelectActivation
        End Get
        Set(ByVal Value As Boolean)
            Me._AllowMultiSelectActivation = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the opacity of all the background, selection, tracking and sorting colors for the control and all items.  Valid values are between 0 and 255.
    ''' </summary>
    ''' <value>An Integer representing the alpha number used for opacity.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(65), _
     Description("Specifies the opacity of all the background, selection, tracking and sorting colors for the control and all items.  Valid values are between 0 and 255.")> _
    Public Property AlphaComponent() As Integer
        Get
            Return Me._AlphaComponent
        End Get
        Set(ByVal Value As Integer)
            If (Value < 0 OrElse Value > 255) Then
                Throw New ArgumentException("The Alpha value must be between 0 and 255")
            End If

            If (Value <> Me._AlphaComponent) Then Me._AlphaComponent = Value
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  The background color used to display text and graphics in the control.
    ''' </summary>
    ''' <value>A Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Window"), _
     Description("The background color used to display text and graphics in the control.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal Value As Color)
            MyBase.BackColor = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies what style the Border the control has.
    ''' </summary>
    ''' <value>An enumerated Enums.BorderStyleType value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(BorderStyleType), "Fixed3D"), _
     Description("Specifies what style the Border the control has.")> _
    Public Property BorderStyle() As BorderStyleType
        Get
            Return Me._Borderstyle
        End Get
        Set(ByVal Value As BorderStyleType)
            If (Not Me._Borderstyle.Equals(Value)) Then
                Me._Borderstyle = Value
                If (Me._Borderstyle = Windows.Forms.BorderStyle.Fixed3D) Then Me._BorderWidth = 2 Else Me._BorderWidth = 1
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if CheckBoxes will be displayed next to TreeListNodes in the TreeListView control.
    ''' </summary>
    ''' <value><c>TRUE</c> if checkboxes will be displayed next to TreeListNodes, otherwise <c>FALSE</c>.  The Default value is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(False), _
     Description("Determines if CheckBoxes will be displayed next to TreeListNodes in the TreeListView control.")> _
    Public Property CheckBoxes() As Boolean
        Get
            Return Me._AllowCheckBoxes
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._AllowCheckBoxes.Equals(Value)) Then
                Me._AllowCheckBoxes = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines how many clicks the user must press before the CheckBox will be selected/deselected.
    ''' </summary>
    ''' <value>An enumerated ItemActivation value.  Default is Standard (two-clicks).</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(GetType(ItemActivation), "Standard"), _
     Description("Determines how many clicks the user must press before the CheckBox will be selected/deselected.")> _
    Public Property CheckBoxSelection() As ItemActivation
        Get
            Return Me._CheckBoxSelection
        End Get
        Set(ByVal Value As ItemActivation)
            Me._CheckBoxSelection = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines the style of CheckBox if VisualStyles is NOT enabled.
    ''' </summary>
    ''' <value>An enumerated CheckBoxStyle value.  The Default is Flat.</value>
    ''' <remarks>Setting this property has no effect if the VisualStyles property is set to <c>TRUE</c>.</remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(CheckBoxStyle), "Flat"), _
     Description("Determines the style of CheckBox if VisualStyles is NOT enabled.")> _
    Public Property CheckBoxStyle() As CheckBoxStyle
        Get
            Return Me._CheckBoxStyle
        End Get
        Set(ByVal Value As CheckBoxStyle)
            If (Not Me._CheckBoxStyle.Equals(Value)) Then
                Me._CheckBoxStyle = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the type of CheckBox to display.
    ''' </summary>
    ''' <value>An enumerated CheckBoxType value.  Default is CheckBox.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(CheckBoxType), "CheckBox"), _
     Description("The type of CheckBox to display.")> _
    Public Property CheckBoxType() As CheckBoxType
        Get
            Return Me._CheckBoxType
        End Get
        Set(ByVal Value As CheckBoxType)
            If (Not Me._CheckBoxType.Equals(Value)) Then
                Me._CheckBoxType = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the Items that are Checked in the control.
    ''' </summary>
    ''' <value>A CheckedContainerListViewItemCollection.</value>
    ''' <remarks></remarks>
    <XmlIgnore(), _
     Browsable(False)> _
    Public ReadOnly Property CheckedItems() As CheckedContainerListViewObjectCollection
        Get
            Return Me._CheckedItems
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets the ContextMenuStrip displayed when a Header is right-clicked.
    ''' </summary>
    ''' <value>A ContextMenuStrip.</value>
    ''' <remarks></remarks>
    <XmlIgnore(), _
     Category("Behavior"), _
     DefaultValue(GetType(ContextMenuStrip), Nothing), _
     Description("The ContextMenuStrip displayed when a ColumnHeader is right-clicked.")> _
    Public Property ColumnHeaderContextMenuStrip() As ContextMenuStrip
        Get
            Return Me._HeaderMenu
        End Get
        Set(ByVal Value As ContextMenuStrip)
            If (Not Value Is Me._HeaderMenu) Then
                Me._HeaderMenu = Value
                Me.OnColumnHeaderContextMenuChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies whether to show Column headers and whether they respond to Mouse clicks.
    ''' </summary>
    ''' <value>A ColumnHeaderStyle enumeration.</value>
    ''' <remarks>
    ''' Setting this property to ColumnHeaderStyles.NonClickable or ColumnHeaderStyles.None will set the ColumnSortColorEnabled 
    ''' property to <c>FALSE</c>.
    ''' </remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(ColumnHeaderStyle), "Clickable"), _
     Description("Specifies whether to show Column headers and whether they respond to Mouse clicks.")> _
    Public Property ColumnHeaderStyles() As ColumnHeaderStyle
        Get
            Return Me._HeaderStyle
        End Get
        Set(ByVal Value As ColumnHeaderStyle)
            If (Not Me._HeaderStyle.Equals(Value)) Then
                Me._HeaderStyle = Value

                'DO SOME OTHER PROCESSING
                If (Me._HeaderStyle = ColumnHeaderStyle.Nonclickable OrElse Me._HeaderStyle = ColumnHeaderStyle.None) Then
                    Me._ColSortEnabled = False
                End If
                Me.Invalidate(Me.ClientRectangle)

                If (Me._HeaderStyle = ColumnHeaderStyle.None) Then Me._HdrBuffer = 0 Else Me._HdrBuffer = _HeaderHeight
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a height of the Listview header
    ''' </summary>
    ''' <value>The height</value>
    <Category("Appearance"),
     DefaultValue(GetType(Integer), "20"),
     Description("Specifies the height of the Listview header")>
    Public Property ColumnHeaderHeight() As Integer
        Get
            Return Me._HdrBuffer
        End Get
        Set(ByVal Value As Integer)
            If (_HeaderHeight = Value) Then Return
            _HeaderHeight = Value
            If (Me._HeaderStyle = ColumnHeaderStyle.None) Then Me._HdrBuffer = 0 Else Me._HdrBuffer = _HeaderHeight
            Me.Invalidate(Me.ClientRectangle)
        End Set
    End Property

    ''' <summary>
    ''' Gets the Columns of the control.
    ''' </summary>
    ''' <value>a ContainerColumnHeaderCollection.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Description("The Columns of the control."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     Editor(GetType(CollectionEditor), GetType(UITypeEditor))> _
    Public ReadOnly Property Columns() As ContainerColumnHeaderCollection
        Get
            Return Me._Columns
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies the Color used for the currently selected sorting column.
    ''' </summary>
    ''' <value>A System.Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "PaleGoldenrod"), _
     Description("Specifies the Color used for the currently selected sorting column.")> _
    Public Property ColumnSortColor() As Color
        Get
            Return Me._ColSortColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._ColSortColor.Equals(Value)) Then
                Me._ColSortColor = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies whether to enable drawing the selected Column the ColumnSortColor.
    ''' </summary>
    ''' <value><c>TRUE</c> if the selected Column will be painted the ColumnSortColor; otherwise <c>FALSE</c>.  The default is <c>FALSE</c>.</value>
    ''' <remarks>Setting this property is only effective if the ColumnHeaderStyles property is set to ColumnHeaderStyle.Clickable.</remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Specifies whether to enable drawing the selected Column the ColumnSortColor.")> _
    Public Property ColumnSortColorEnabled() As Boolean
        Get
            Return Me._ColSortEnabled
        End Get
        Set(ByVal Value As Boolean)
            If (Me._HeaderStyle = ColumnHeaderStyle.Clickable) Then
                If (Not Me._ColSortColor.Equals(Value)) Then
                    Me._ColSortEnabled = Value
                    Me.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that highlights the Column when the Mouse hovers of the header.
    ''' </summary>
    ''' <value><c>TRUE</c> if tracking is enabled;  otherwise <c>FALSE</c>.  Default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Highlights the Column when the Mouse hovers of the header.")> _
    Public Property ColumnTracking() As Boolean
        Get
            Return Me._DoColTracking
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._DoColTracking.Equals(Value)) Then
                Me._DoColTracking = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Color used for column hot-tracking
    ''' </summary>
    ''' <value>A System.Color used for hot-tracking.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "WhiteSmoke"), _
     Description("The Color used for column hot-tracking")> _
    Public Property ColumnTrackingColor() As Color
        Get
            Return Me._ColTrackColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._ColTrackColor.Equals(Value)) Then
                Me._ColTrackColor = Value
                Me._Brush_ColTracking = New SolidBrush(Color.FromArgb(Me._AlphaComponent, Me._ColTrackColor))
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the ContextMenu displayed when the control is right-clicked.
    ''' </summary>
    ''' <value>A ContextMenu.</value>
    ''' <remarks>
    ''' While Microsoft has the ContextMenu available for backwards compatibility, this control does not support it so please use the ContextMenuStrip 
    ''' instead.
    ''' </remarks>
    <Category("Behavior"), _
     Description("The ContextMenu displayed when the control is right-clicked."), _
     EditorBrowsable(EditorBrowsableState.Never), _
     Obsolete("While Microsoft has the ContextMenu available for backwards compatibility, this control does not support it so please use the ContextMenuStrip " & _
              "instead.", True)> _
    Public NotOverridable Overrides Property ContextMenu() As ContextMenu
        Get
            Return Nothing
        End Get
        Set(ByVal Value As ContextMenu)

        End Set
    End Property

    ''' <summary>
    ''' Gets the default IComparer to use when sorting ContainerListsViewObjects.
    ''' </summary>
    ''' <value>An object that implements the IComparer interface.</value>
    ''' <remarks>This Comparer will be used if the 'SortComparer' property is <c>NOTHING</c>.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property DefaultComparer() As IComparer
        Get
            Return Me._DefaultComparer
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines the Color of the control when the Enabled property is False.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Gainsboro"), _
     Description("Determines the Color of the control when the Enabled property is False.")> _
    Public Property DisabledColor() As Color
        Get
            Return Me._DisabledColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._DisabledColor.Equals(Value)) Then
                Me._DisabledColor = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the background color of the ContainerListViewObject when it is in an edit state.
    ''' </summary>
    ''' <value>A Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Info"), _
     Description("The background color of the ContainerListViewObject when it is in an edit state.")> _
    Public Property EditBackColor() As Color
        Get
            Return Me._EditBackColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._EditBackColor.Equals(Value)) Then
                Me._EditBackColor = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the current ContainerListViewObject that is itself being edited or has one of it's subitems being edited.
    ''' </summary>
    ''' <value>A ContainerListViewObject.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property EditedObject() As ContainerListViewObject
        Get
            Return Me._EditingObj
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies wether the selected item is always visible
    ''' </summary>
    ''' <value><c>TRUE</c> if the selected Item is always visible;  otherwise FALSe.  Default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Specifies wether the selected item is always visible")> _
    Public Property EnsureVisible() As Boolean
        Get
            Return Me._EnsureVisible
        End Get
        Set(ByVal Value As Boolean)
            Me._EnsureVisible = Value
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the Font used to display text in the control.
    ''' </summary>
    ''' <value>A Font value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Font), "System.Drawing.SystemFonts.DefaultFont"), _
     Description("The Font used to display text in the control.")> _
    Public Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal Value As Font)
            If (Not MyBase.Font.Equals(Value)) Then
                MyBase.Font = Value
                Me._baseFontChanged()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the foreground color used to display text and graphics in the control.
    ''' </summary>
    ''' <value>A Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "WindowText"), _
     Description("The foreground color used to display text and graphics in the control.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(ByVal Value As Color)
            If (Not MyBase.ForeColor.Equals(Value)) Then
                MyBase.ForeColor = Value
                Me._baseForeColorChanged()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether to highlight the full row or just the label when selecting an item.
    ''' </summary>
    ''' <value><c>TRUE</c> if FullRow selecting is enabled;  otherwise <c>FALSE</c>.  Default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether to highlight the full row or just the label when selecting an item.")> _
    Public Property FullRowSelect() As Boolean
        Get
            Return Me._FullRowSelect
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._FullRowSelect.Equals(Value)) Then
                Me._FullRowSelect = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Color used for Gridlines.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Silver"), _
     Description("Specifies the Color used for Gridlines.")> _
    Public Property GridLineColor() As Color
        Get
            Return Me._GridLineColor
        End Get
        Set(ByVal Value As Color)
            If (Not Me._GridLineColor.Equals(Value)) Then
                Me._GridLineColor = Value
                Me._GridLinePen = New Pen(Me._GridLineColor, 1.0F)
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the Pen used to draw GridLines.
    ''' </summary>
    ''' <value>A Pen object.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Protected ReadOnly Property GridLinePen() As Pen
        Get
            Return Me._GridLinePen
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines which gridlines will be shown.
    ''' </summary>
    ''' <value>An enumerated type specifying which gridlines are shown on the control.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(GridLineSelections), "Both"), _
     Description("Determines which gridlines will be shown.")> _
    Public Property GridLines() As GridLineSelections
        Get
            Return Me._GridLineType
        End Get
        Set(ByVal Value As GridLineSelections)
            If (Not Me._GridLineType.Equals(Value)) Then
                Me._GridLineType = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the size of the Header buffer for drawing purposes.
    ''' </summary>
    ''' <value>An Integer.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Protected Friend ReadOnly Property HeaderBuffer() As Integer
        Get
            Return Me._HdrBuffer
        End Get
    End Property

    ''' <summary>
    ''' Gets the Columns that are Hidden in the control.
    ''' </summary>
    ''' <value>A HiddenColumnsCollection.</value>
    ''' <remarks></remarks>
    <XmlIgnore(), _
     Browsable(False)> _
    Public ReadOnly Property HiddenColumns() As HiddenColumnsCollection
        Get
            Return Me._HiddenCols
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether to hide selected rows when the control loses focus.
    ''' </summary>
    ''' <value><c>TRUE</c> if selected rows are hidden upon losing focus; otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determines whether to hide selected rows when the control loses focus.")> _
    Public Property HideSelection() As Boolean
        Get
            Return Me._HideSelection
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._HideSelection.Equals(Value)) Then
                Me._HideSelection = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the Horizontal scroll bar used by the control.
    ''' </summary>
    ''' <value>An HScrollBar object.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property HScroll() As HScrollBar
        Get
            Return Me._HscrollBar
        End Get
    End Property

    ''' <summary>
    ''' <c>NOT SUPPORTED</c>. 
    ''' Gets or Sets a value that determines whether to automatically select a row when the mouse is hovered over it for a short time.
    ''' </summary>
    ''' <value><c>TRUE</c> if hovering is enabled;  otherwise <c>FALSE</c>. Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Not Supported"), _
     DefaultValue(False), _
     Description("Not Supported." & ControlChars.CrLf & "Determines whether to automatically select a row when the mouse is hovered over it for a short time.")> _
    Public Property HoverSelection() As Boolean
        Get
            Return Me._HoverSelection
        End Get
        Set(ByVal Value As Boolean)
            Me._HoverSelection = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the ImageList used for displaying Images in the control.
    ''' </summary>
    ''' <value>An ImageList object.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     DefaultValue(GetType(ImageList), Nothing), _
     Description("The ImageList used for displaying Images in the control.")> _
    Public Property ImageList() As ImageList
        Get
            Return Me._ImageList
        End Get
        Set(ByVal Value As ImageList)
            Me._ImageList = Value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets a value that determines if the control is currently being updated.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control is in Update mode; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property InUpdateMode() As Boolean
        Get
            Return Me._UpdateTransactions
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that specifies if the control has focus.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control has focus; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Protected ReadOnly Property IsFocused() As Boolean
        Get
            Return Me._IsFocused
        End Get
    End Property

    ''' <summary>
    ''' Gets the Items contained in the control.
    ''' </summary>
    ''' <value>A ContainerListViewItemCollection.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Description("The Items contained in the control."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     Editor(GetType(CollectionEditor), GetType(UITypeEditor))> _
    Public Overridable ReadOnly Property Items() As ContainerListViewItemCollection
        Get
            Return Me._Items
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a value that determines whether logging is enabled or disabled on the control.
    ''' </summary>
    ''' <value><c>TRUE</c> if logging is enabled; otherwise, <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <returns><c>TRUE</c> if logging is enabled; otherwise, <c>FALSE</c>.  Default is <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <Category("Not Supported"), _
     DefaultValue(False), _
     Description("Not Supported." & ControlChars.CrLf & "Determines whether logging is enabled or disabled on the control.")> _
    Public Overridable Property Logging() As Boolean
        Get
            Return Me._Logging
        End Get
        Set(ByVal Value As Boolean)
            Me._Logging = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether the control will allow multiple selections.
    ''' </summary>
    ''' <value><c>TRUE</c> if multiple selection is enabled; otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks>
    ''' If MultiSelect is changed to <c>FALSE</c> and multiple items are currently selected, then all selected items are 
    ''' cleared except for the last item that was selected when MultiSelect was enabled.
    ''' </remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determines whether the control will allow multiple selections.")> _
    Public Property MultiSelect() As Boolean
        Get
            Return Me._MultiSelect
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._MultiSelect.Equals(Value)) Then
                Me._MultiSelect = Value
                Me.Invalidate(Me.ClientRectangle)

                If (Not Me._MultiSelect) Then
                    If (Me._SelectedItems.Count > 1) Then
                        'ONLY HOLD ON TO THE LAST SELECTED ITEM
                        Dim Itm As ContainerListViewObject = Me._SelectedItems(Me._SelectedItems.Count - 1)

                        Me.BeginUpdate()
                        Me._SelectedItems.Clear()
                        Me._SelectedIndexes.Clear()
                        Itm.Selected = True
                        Me.EndUpdate()
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Cursor to display when a ColumnHeader is being resized.
    ''' </summary>
    ''' <value>A Cursor.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Cursor), "VSplit"), _
     Description("The Cursor to display when a ColumnHeader is being resized.")> _
    Public Property ResizeCursor() As Cursor
        Get
            Return Me._ResizeCursor
        End Get
        Set(ByVal Value As Cursor)
            Me._ResizeCursor = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if a Right mouse-click selects a ContainerListViewObject also.
    ''' </summary>
    ''' <value><c>TRUE</c> if a right mouse-click selects a ContainerListViewObject; otherwise <c>FALSE</c>.  The Default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines if a Right mouse-click selects a TreeListNode also.")> _
    Public Property RightMouseSelects() As Boolean
        Get
            Return Me._RightMouseSelects
        End Get
        Set(ByVal Value As Boolean)
            Me._RightMouseSelects = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the RowHeight of the Items in the listview.
    ''' </summary>
    ''' <value>An Integer representing the row height.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(14), _
     Description("Sets the RowHeight of the Items in the listview.")> _
    Public Property RowHeight() As Integer
        Get
            Return Me._RowHeight
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._RowHeight.Equals(Value)) Then
                Me._RowHeight = Math.Abs(Value)
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies the color used for selected rows.
    ''' </summary>
    ''' <value>A System.Drawing.Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Highlight"), _
     Description("Specifies the color used for selected rows.")> _
    Public Property RowSelectColor() As Color
        Get
            Return Me._RowSelectColor
        End Get
        Set(ByVal Value As Color)
            Me._RowSelectColor = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the current rectangle representing the area where the visible Rows are located.
    ''' </summary>
    ''' <value>A Rectangle representing the visible Rows rectangle.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Protected ReadOnly Property RowsRectangle() As Rectangle
        Get
            Return Me._RowsRect
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the control Highlights a row when the Mouse hovers over it.
    ''' </summary>
    ''' <value><c>TRUE</c> if enabled; otherwise <c>FALSE</c>.  Default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determines if the control Highlights a row when the Mouse hovers over it.")> _
    Public Property RowTracking() As Boolean
        Get
            Return Me._DoRowTracking
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._DoRowTracking.Equals(Value)) Then
                Me._DoRowTracking = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that specifies the Color used for row hot-tracking.
    ''' </summary>
    ''' <value>A Color.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "WhiteSmoke"), _
     Description("Specifies the Color used for row hot-tracking.")> _
    Public Property RowTrackingColor() As Color
        Get
            Return Me._RowTrackColor
        End Get
        Set(ByVal Value As Color)
            Me._RowTrackColor = Value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the control will display scroll bars if it contains more items than can fit in the client area.
    ''' </summary>
    ''' <value><c>TRUE</c> if scroll bars are visible;  othewise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Indicates whether the control will display scroll bars if it contains more items than can fit in the client area.")> _
    Public Property Scrollable() As Boolean
        Get
            Return Me._Scrollable
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._Scrollable.Equals(Value)) Then
                Me._Scrollable = Value
                Me.Invalidate(Me.ClientRectangle)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the indexes of the Selected items in the control.
    ''' </summary>
    ''' <value>A collection of selected Indices</value>
    ''' <remarks></remarks>
    <XmlIgnore(), _
     Browsable(False)> _
    Public ReadOnly Property SelectedIndexes() As SelectedIndexCollection
        Get
            Return Me._SelectedIndexes
        End Get
    End Property

    ''' <summary>
    ''' Gets the Items that are selected in the control.
    ''' </summary>
    ''' <value>A SelectedContainerListViewItemCollection.</value>
    ''' <remarks></remarks>
    <XmlIgnore(), _
     Browsable(False)> _
    Public ReadOnly Property SelectedItems() As SelectedContainerListViewObjectCollection
        Get
            Return Me._SelectedItems
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets the sorting comparer for the control.
    ''' </summary>
    ''' <value>An IComparer that represents the sorting comparer for the control.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DefaultValue(GetType(IComparer), Nothing)> _
    Public Property SortComparer() As IComparer
        Get
            Return Me._SortComparer
        End Get
        Set(ByVal Value As IComparer)
            Me._SortComparer = Value
        End Set
    End Property

    ''' <summary>
    ''' Indicates the manner in which items are to be sorted.
    ''' </summary>
    ''' <value>A SortOrder enumeration indicating how items are sorted.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(GetType(SortOrder), "None"), _
     Description("Indicates the manner in which items are to be sorted.")> _
    Public Property Sorting() As SortOrder
        Get
            Return Me._Sorting
        End Get
        Set(ByVal Value As SortOrder)
            Me._Sorting = Value
        End Set
    End Property

    ''' <summary>
    ''' Overriden.  Gets or Sets the Text associated with the Control.
    ''' </summary>
    ''' <value>A String value.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DefaultValue(GetType(String), ""), _
     EditorBrowsable(EditorBrowsableState.Advanced), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal Value As String)
            MyBase.Text = Value
        End Set
    End Property

    ''' <summary>
    ''' <c>NOT SUPPORTED</c>. 
    ''' Gets or Sets a value that determines if the control should keep track of user-selected Items.
    ''' </summary>
    ''' <value><c>TRUE</c> if the control should track the history of user-selected items;  otherwise <c>FALSE</c> if not.  The default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Not Supported"), _
     DefaultValue(False), _
     Description("Not Supported." & ControlChars.CrLf & "Determines if the control should keep track of user-selected Items.")> _
    Public Property TrackHistory() As Boolean
        Get
            Return Me._TrackHistory
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._TrackHistory.Equals(Value)) Then
                Me._TrackHistory = Value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that indicates whether the control should be drawn using WindowsXP visual styles.
    ''' </summary>
    ''' <value><c>TRUE</c> if WindowsXP visual styles are applied;  othewise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(True), _
     Description("Indicates whether the control should be drawn using WindowsXP visual styles.")> _
    Public Property VisualStyles() As Boolean
        Get
            Return Me._VisualStyles
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._VisualStyles.Equals(Value)) Then
                Me._VisualStyles = Value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the Vertical scroll bar used by the control.
    ''' </summary>
    ''' <value>A VScrollBar object.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property VScroll() As VScrollBar
        Get
            Return Me._VscrollBar
        End Get
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Prevents the control from drawing until the EndUpdate method is called.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BeginUpdate()
        Me._UpdateTransactions = True
    End Sub

    ''' <summary>
    ''' Determines if a CheckBox has been clicked on an Object.
    ''' </summary>
    ''' <param name="aHashTableToCheck">The HashTable containing the Rectangles(Keys) associated to the Objects(Values).</param>
    ''' <param name="e">The MouseEventArg where the user clicked.</param>
    ''' <param name="aObject">The Object to set if a match was found in the HashTable.</param>
    ''' <returns><c>TRUE</c> if a match was found; otherwise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    Protected Function CheckBoxClicked(ByVal aHashTableToCheck As Hashtable, ByVal e As MouseEventArgs, ByRef aObject As Object) As Boolean
        Dim Result As Boolean = False

        If (aHashTableToCheck IsNot Nothing) Then
            aObject = Tools.EvaluateObject(e, aHashTableToCheck.Keys.GetEnumerator, aHashTableToCheck.Values.GetEnumerator)
            Result = (aObject IsNot Nothing)
        End If

        Return Result
    End Function

    ''' <summary>
    ''' Calls the OnAfterEdit method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewObject calling the method.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ContainerListViewAfterEdit(ByVal aItem As ContainerListViewObject)
        If (aItem IsNot Nothing) Then Me.OnAfterEdit(New ContainerListViewEventArgs(aItem))
    End Sub

    ''' <summary>
    ''' Calls the OnAfterSelect method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewItem calling the method.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ContainerListViewAfterSelect(ByVal aItem As ContainerListViewObject)
        If (aItem IsNot Nothing) Then Me.OnAfterSelect(New ContainerListViewEventArgs(aItem))
    End Sub

    ''' <summary>
    ''' Calls the OnBeforeEdit method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewObject calling the method.</param>
    ''' <returns><c>TRUE</c> if the call to the 'OnAfter' equivalent method should be cancelled; othewise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Function ContainerListViewBeforeEdit(ByVal aItem As ContainerListViewObject) As Boolean
        If (aItem IsNot Nothing) Then
            Dim Arg As New ContainerListViewCancelEventArgs(aItem, False)

            Me.OnBeforeEdit(Arg)
            Return Arg.Cancel
        End If

        Return True
    End Function

    ''' <summary>
    ''' Calls the OnBeforeSelect method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewItem calling the method.</param>
    ''' <returns><c>TRUE</c> if the call to the 'OnAfter' equivalent method should be cancelled; othewise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Function ContainerListViewBeforeSelect(ByVal aItem As ContainerListViewObject) As Boolean
        If (aItem IsNot Nothing) Then
            Dim Arg As New ContainerListViewCancelEventArgs(aItem, False)
            Me.OnBeforeSelect(Arg)

            Return Arg.Cancel
        End If

        Return True
    End Function

    ''' <summary>
    ''' Calls the OnAfterCheckStateChanged method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewObject calling the method.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ContainerListViewAfterCheckStateChanged(ByVal aItem As ContainerListViewObject)
        If (aItem IsNot Nothing) Then Me.OnAfterCheckStateChanged(New ContainerListViewEventArgs(aItem))
    End Sub

    ''' <summary>
    ''' Calls the OnBeforeCheckStateChanged method.
    ''' </summary>
    ''' <param name="aItem">The ContainerListViewObject calling the method.</param>
    ''' <returns><c>TRUE</c> if the call to the 'OnAfterCheckStateChanged' equivalent method should be cancelled; othewise <c>FALSE</c>.</returns>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Function ContainerListViewBeforeCheckStateChanged(ByVal aItem As ContainerListViewObject) As Boolean
        If (aItem IsNot Nothing) Then
            Dim Arg As New ContainerListViewCancelEventArgs(aItem, False)
            Me.OnBeforeCheckStateChanged(Arg)

            Return Arg.Cancel
        End If

        Return True
    End Function

    ''' <summary>
    ''' Creates the Schema for the class.
    ''' </summary>
    ''' <param name="aSchemaSet"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' See the <see cref="T:System.Xml.Serialization.IXmlSerializable"/> interface and the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> for more details.
    ''' <para>
    ''' This method is intentionally hidden from view and only used by xml serializers.
    ''' </para>
    ''' </remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shared Function CreateSchema(ByVal aSchemaSet As XmlSchemaSet) As XmlQualifiedName
        Return New XmlQualifiedName()
    End Function

    ''' <summary>
    ''' Determines the background color of the <see cref="ContainerListViewObject"/>.
    ''' </summary>
    ''' <param name="aObj">The object whose background color to determine.</param>
    ''' <param name="aSubItem">Optional.  A SubItem.</param>
    ''' <returns>A Color based on the state of aObj.</returns>
    ''' <remarks></remarks>
    Protected Overridable Function DetermineBackGroundColor(ByVal aObj As ContainerListViewObject, Optional ByVal aSubItem As ContainerListViewObject.ContainerListViewSubItem = Nothing) As Color
        Return Me._determineBackGroundColor(aObj, aSubItem)
    End Function

    ''' <summary>
    ''' Draws the CheckBoxes (if visible) of any ContainerListViewObject.
    ''' </summary>
    ''' <param name="aGr">The Graphics object used to paint the CheckBox.</param>
    ''' <param name="Rect">The Rectangle representing the area to paint the CheckBox in.</param>
    ''' <param name="aObj">The ContainerListViewObject whose CheckBox is to be drawn.</param>
    ''' <remarks></remarks>
    Protected Sub DrawObjectCheckBox(ByVal aGr As Graphics, ByVal Rect As Rectangle, ByVal aObj As ContainerListViewObject)
        If (Me._VisualStyles) Then
            Dim Hdc As IntPtr
            Dim Theme As IntPtr
            Dim State As Integer
            Dim Rect1, Rect2 As VisualStyles.RECT
            Dim MsPnt As Point = Me.PointToClient(Control.MousePosition)

            'DRAW THE HEADER BACKGROUND
            Try
                'SET THE VARIABLES
                Hdc = aGr.GetHdc()
                Theme = Helpers.VisualStyles.OpenThemeData(Me.Handle, "BUTTON")
                Rect1 = New VisualStyles.RECT(Rect)
                Rect2 = New VisualStyles.RECT(Rect)

                'DETERMINE THE STATE
                If (aObj.Checked) Then
                    If (Not aObj.CheckBoxEnabled) Then
                        State = 8 'RBS_CHECKEDDISABLED
                    ElseIf (Rect.Contains(MsPnt.X, MsPnt.Y)) Then
                        State = 6 'RBS_CHECKEDHOT 
                    Else
                        State = 5 'RBS_CHECKEDNORMAL
                    End If
                Else
                    If (Not aObj.CheckBoxEnabled) Then
                        State = 4 'RBS_UNCHECKEDDISABLED
                    ElseIf (Rect.Contains(MsPnt.X, MsPnt.Y)) Then
                        State = 2 'RBS_UNCHECKEDHOT
                    Else
                        State = 1 'RBS_UNCHECKEDNORMAL 
                    End If
                End If

                'DO THE DRAWING
                If (Me._CheckBoxType = CheckBoxType.CheckBox) Then
                    'BP_CHECKBOX
                    Helpers.VisualStyles.DrawThemeBackground(Theme, Hdc, 3, State, Rect1, Rect2)
                Else
                    'BP_RADIOBUTTON
                    Helpers.VisualStyles.DrawThemeBackground(Theme, Hdc, 2, State, Rect1, Rect2)
                End If
            Catch ex As Exception
                'DO NOTHING FOR NOW
            Finally
                'RELEASE THE RESOURCES
                If (Not IntPtr.Zero.Equals(Hdc)) Then aGr.ReleaseHdc(Hdc)
                If (Not IntPtr.Zero.Equals(Theme)) Then Helpers.VisualStyles.CloseThemeData(Theme)
            End Try
        Else
            Dim BS As ButtonState
            Dim Style As ButtonState = ButtonState.Flat

            If (Me._CheckBoxStyle <> CheckBoxStyle.Flat) Then Style = ButtonState.Normal
            If (aObj.CheckBoxEnabled) Then
                If (aObj.Checked) Then BS = (Style Or ButtonState.Checked) Else BS = Style
            Else
                If (aObj.Checked) Then BS = (Style Or ButtonState.Checked Or ButtonState.Inactive) Else BS = (Style Or ButtonState.Inactive)
            End If

            'DRAW THE CHECKBOX
            If (Me._CheckBoxType = CheckBoxType.CheckBox) Then
                ControlPaint.DrawCheckBox(aGr, Rect, BS)
            Else
                ControlPaint.DrawRadioButton(aGr, Rect, BS)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Resumes drawing of the list view control after drawing is suspended by the BeginUpdate method.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EndUpdate()
        Me._UpdateTransactions = False
        Me._generateHeaderRect()
        Me._generateViewableRowsRectangle()
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Ensures that the VSCroll's 'LargeChange' property is always divisible by the RowHeight.
    ''' </summary>
    ''' <param name="aCurrentValue">The current value of the VSCroll.</param>
    ''' <param name="aIsHScrollVisible"><c>TRUE</c> if the HScroll is also visible; otherwise <c>FALSE</c>.</param>
    ''' <returns>An Integer representing the new LargeChange value.</returns>
    ''' <remarks></remarks>
    Protected Function EnsureVerticalScroll(ByVal aCurrentValue As Integer, ByVal aIsHScrollVisible As Boolean) As Integer
        If (aIsHScrollVisible) Then
            Return (aCurrentValue - (aCurrentValue Mod Me.RowHeight))
        Else
            Return ((aCurrentValue + Me.HScroll.Height) - ((aCurrentValue + Me.HScroll.Height) Mod Me.RowHeight))
        End If
    End Function

    ''' <summary>
    ''' Returns the bounding Rectangle of the ContainerListViewObject only.
    ''' </summary>
    ''' <param name="aObj">The ContainerListViewObject whose rectangle to return.</param>
    ''' <returns>A Rectangle.</returns>
    ''' <remarks></remarks>
    Protected Friend Overridable Function GetItemBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        If (aObj Is Nothing) Then Throw New NullReferenceException("aObj cannot be NULL.")

        Return Me._getBounds(aObj)
    End Function

    ''' <summary>
    ''' Returns the bounding Rectangle of the ContainerListViewObject and it's SubItems.
    ''' </summary>
    ''' <param name="aObj">The ContainerListViewObject whose rectangle to return.</param>
    ''' <returns>A Rectangle.</returns>
    ''' <remarks></remarks>
    Protected Friend Overridable Function GetCompleteItemBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        If (aObj Is Nothing) Then Throw New NullReferenceException("aObj cannot be NULL.")

        Return Me._getCompleteBounds(aObj)
    End Function

    ''' <summary>
    ''' Returns a ContainerListViewObject at the specified point.
    ''' </summary>
    ''' <param name="aPoint">The point at which to check for a SubItem.</param>
    ''' <returns>A ContainerListViewObject ojbect.</returns>
    ''' <remarks></remarks>
    Public Overridable Overloads Function GetItemAt(ByVal aPoint As Point) As ContainerListViewObject
        For Each Itm As ContainerListViewItem In Me._Items
            If (Itm.CompleteBounds.Contains(aPoint)) Then Return Itm
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns a ContainerListViewObject at the specified point.
    ''' </summary>
    ''' <param name="aX">The X coordinate.</param>
    ''' <param name="aY">The Y coordinate.</param>
    ''' <returns>A ContainerListViewObject ojbect.</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetItemAt(ByVal aX As Integer, ByVal aY As Integer) As ContainerListViewObject
        Return Me.GetItemAt(New Point(aX, aY))
    End Function

    ''' <summary>
    ''' Reserved.
    ''' This function is reserved.  Apply the XmlSchemaProviderAttribute to the class instead
    ''' </summary>
    ''' <returns> An <c>XmlSchema</c> that describes the XML representation of the object that is produced by the IXmlSerializable.WriteXmlmethod 
    ''' and consumed by the IXmlSerializable.ReadXml method.</returns>
    ''' <remarks>
    ''' See the IXmlSerializable interface for more information.
    ''' </remarks>
    <EditorBrowsable(EditorBrowsableState.Never), _
     Obsolete("This method will soon be removed from the .NET Framework", True)> _
    Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema
        Throw New NotImplementedException("This method is not supported by the .NET Framework")
    End Function

    ''' <summary>
    ''' Returns the Sum of all the Column widths in the control, including hidden columns.
    ''' </summary>
    ''' <returns>An Integer representing the combined width of all the columns.</returns>
    ''' <remarks></remarks>
    Public Function GetSumOfAllColumnWidths() As Integer
        Dim Wdth As Integer = 0

        For Each Col As ContainerColumnHeader In Me._Columns
            Wdth += Col.Width
        Next

        Return Wdth
    End Function

    ''' <summary>
    ''' Returns the Sum of all the visible Column widths in the control.
    ''' </summary>
    ''' <returns>An Integer representing the combined width of all the visible columns.</returns>
    ''' <remarks></remarks>
    Public Function GetSumOfVisibleColumnWidths() As Integer
        Dim Wdth As Integer = 0

        For Each Col As ContainerColumnHeader In Me._Columns
            If (Not Col.Hidden) Then Wdth += Col.Width
        Next

        Return Wdth
    End Function

    ''' <summary>
    ''' Notifies the control that one of it's ContainerColumnHeaders has been resized.
    ''' </summary>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Friend Sub HeaderResized(col As ContainerColumnHeader)
        Me._generateHeaderRect()
        Me._generateViewableRowsRectangle()
        RaiseEvent ColumnWidthChanged(Me, New ColumnWidthChangedEventArgs(col.Index))
    End Sub
    Public Event ColumnWidthChanged As ColumnWidthChangedEventHandler

    ''' <summary>
    ''' Occurs when the ScrollBars need to be setup and displayed by the control.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAdjustScrollBars()
        Me._adjustScrollBars()
    End Sub

    ''' <summary>
    ''' Raises the AfterCheckStateChanged event.
    ''' </summary>
    ''' <param name="e">A ContainerListViewEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterCheckStateChanged(ByVal e As ContainerListViewEventArgs)
        RaiseEvent AfterCheckStateChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the AfterEdit event.
    ''' </summary>
    ''' <param name="e">A ContainerListViewEventArgs containing the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterEdit(ByVal e As ContainerListViewEventArgs)
        RaiseEvent AfterEdit(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the AfterSelect event.
    ''' </summary>
    ''' <param name="e">A ContainerListViewEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnAfterSelect(ByVal e As ContainerListViewEventArgs)
        RaiseEvent AfterSelect(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the BeforeEdit event.
    ''' </summary>
    ''' <param name="e">The ContainerListViewCancelEventArgs containing the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeEdit(ByVal e As ContainerListViewCancelEventArgs)
        RaiseEvent BeforeEdit(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the BeforeSelect event.
    ''' </summary>
    ''' <param name="e">A ContainerListViewCancelEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeSelect(ByVal e As ContainerListViewCancelEventArgs)
        RaiseEvent BeforeSelect(Me, e)
    End Sub

    ''' <summary>
    ''' Occurs whenever OnKeyDown is called.  Checks if any Shift or Control keys are also pressed.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnCheckShiftState(ByVal e As KeyEventArgs)
        If (Me._MultiSelect) Then
            If (e.KeyCode = Keys.ControlKey) Then
                Me._MultiSelectMode = MultiSelectModes.Selective
            ElseIf (e.KeyCode = Keys.ShiftKey) Then
                Me._MultiSelectMode = MultiSelectModes.Range
            End If
        End If
    End Sub

    ''' <summary>
    ''' Raises the BeforeCheckStateChanged event.
    ''' </summary>
    ''' <param name="e">A ContainerListViewCancelEventArg that contains the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnBeforeCheckStateChanged(ByVal e As ContainerListViewCancelEventArgs)
        RaiseEvent BeforeCheckStateChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the ColumnHeaderClicked event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnColumnHeaderClicked(ByVal e As ContainerColumnHeaderEventArgs)
        RaiseEvent ColumnHeaderClicked(Me, e)
        If (Me._Sorting <> SortOrder.None) Then Me.OnSort(e.Column.Index)
    End Sub

    ''' <summary>
    ''' Raises the ColumnHeaderContextMenuChanged event.
    ''' </summary>
    ''' <param name="e">The EventArg containing the data.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnColumnHeaderContextMenuChanged(ByVal e As EventArgs)
        RaiseEvent ColumnHeaderContextMenuChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the ColumnHeaderContextMenuRequested event.
    ''' </summary>
    ''' <param name="e">The MouseArg where the ColumnHeaderContextMenu should be displayed.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnColumnHeaderContextMenuRequested(ByVal e As MouseEventArgs)
        RaiseEvent ColumnHeaderContextMenuRequested(Me, e)
    End Sub

    ''' <summary>
    ''' Raises the ContextMenuRequested event.
    ''' </summary>
    ''' <param name="e">The MouseArg where the ContextMenu should be displayed.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnContextMenuRequested(ByVal e As MouseEventArgs)
        RaiseEvent ContextMenuRequested(Me, e)
    End Sub

    ''' <summary>
    ''' Draws the BackGround of the control.
    ''' </summary>
    ''' <param name="aGr">A Graphics object to draw with.</param>
    ''' <param name="Rect">A Rectangle to draw in.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnDrawBackGround(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Me.Enabled) Then
            'DRAW THE BACKGROUND
            aGr.FillRectangle(New SolidBrush(Me.BackColor), Rect)

            'DRAW THE BACKGROUND IMAGE
            If (Me.BackgroundImage IsNot Nothing) Then
                Dim HdrOffset As Integer = 0

                If (Me._HeaderStyle <> ColumnHeaderStyle.None) Then HdrOffset = Me._HdrBuffer
                aGr.DrawImage(Me.BackgroundImage, Rect.X, Rect.Y + HdrOffset, Rect.Width, Rect.Height - HdrOffset)
            End If

            'SELECTED COLUMN
            If (Me._HeaderStyle = ColumnHeaderStyle.Clickable) Then
                If (Me._SelectedCol IsNot Nothing AndAlso Not Me._SelectedCol.Hidden AndAlso Me._ColSortEnabled AndAlso Me._LastPaintedSortCol <> Me._SelectedCol.Index) Then
                    Dim SelColRect As Rectangle = Me._SelectedCol.Bounds

                    aGr.FillRectangle(New SolidBrush(Color.FromArgb(Me._AlphaComponent, Me.ColumnSortColor)), SelColRect.X, SelColRect.Y + Me._HdrBuffer, SelColRect.Width, Rect.Height - Me._HdrBuffer + 2)
                End If
            End If

            'HOT-TRACKED COLUMN
            If (Me._LastColHovered IsNot Nothing AndAlso Not Me._LastColHovered.Hidden AndAlso Me._DoColTracking) Then
                Dim TrackRect As Rectangle = Me._LastColHovered.Bounds

                aGr.FillRectangle(Me._Brush_ColTracking, TrackRect.X, TrackRect.Y + Me._HdrBuffer, TrackRect.Width, Rect.Height - Me._HdrBuffer + 2)
            End If
        Else
            aGr.FillRectangle(New SolidBrush(Me._DisabledColor), Rect)
        End If
    End Sub

    ''' <summary>
    ''' Draws the Column GridLines of the control.
    ''' </summary>
    ''' <param name="aGr">A Graphics object to draw with.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnDrawColumnGridLines(ByVal aGr As Graphics)
        If (Me._GridLineType <> GridLineSelections.None) Then
            'VERTICAL LINES
            If (Me._GridLineType = GridLineSelections.Both OrElse Me._GridLineType = GridLineSelections.Column) Then
                Dim CH As ContainerColumnHeader
                Dim Rect As Rectangle = Me.ClientRectangle

                aGr.Clip = New Region(Rect)
                For Each CH In Me._Columns
                    Dim ColRect As Rectangle = CH.Bounds
                    aGr.DrawLine(Me._GridLinePen, ColRect.Right - 1, Me._HdrBuffer + 2, ColRect.Right - 1, Rect.Height - 2)
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' Draws the Rows of the control.
    ''' </summary>
    ''' <param name="aGr">A Graphics object to draw with.</param>
    ''' <param name="Rect">A Rectangle to draw in.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnDrawRows(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Not Me._UpdateTransactions AndAlso Me._Items.Count > 0 AndAlso Me.Columns.Count > 0) Then
            Dim TotalRend As Integer = 0
            Dim ClviDraw As ContainerListViewItem
            Dim MaxRend As Integer = 0
            Dim Prior As Integer = CType(Me.VScroll.Value / Me.RowHeight, Integer)

            'DETERMINE THE TOTAL # OF ITEMS THAT CAN BE RENDERED IN THE CLIENT RECTANGLE
            If (Me._HscrollBar.Visible) Then
                MaxRend = CType(Math.Ceiling((Rect.Height - (Me._HdrBuffer + 2) - Me._HscrollBar.Height) / Me.RowHeight), Integer)
            Else
                MaxRend = CType(Math.Ceiling((Rect.Height - (Me._HdrBuffer + 2)) / Me.RowHeight), Integer)
            End If

            'GET THE FIRST ITEM TO DRAW
            If (Prior <= 0) Then ClviDraw = Me._Items(0) Else ClviDraw = Me._Items(Prior)

            'CLEAR EXISTING DATA
            Me._CheckBoxRects.Clear()

            'START DRAWING THE ROWS
            While (ClviDraw IsNot Nothing AndAlso MaxRend > TotalRend)
                Me._renderItemRows(ClviDraw, aGr, Rect, TotalRend)
                TotalRend += 1

                If (ClviDraw.Index + 1 = Me._Items.Count) Then ClviDraw = Nothing Else ClviDraw = Me._Items(ClviDraw.Index + 1)
            End While

            'REFRESH THE OBJECT IN AN EDIT STATE
            If (Me._EditingObj IsNot Nothing) Then Me._EditingObj.RefreshEditing()

            'RENDER THE COLUMN GRIDLINES AND ADJUST THE SCROLLBARS
            Me.OnDrawColumnGridLines(aGr)
            Me.OnAdjustScrollBars()
        End If
    End Sub

    ''' <summary>
    ''' Raises the GotFocus event.
    ''' </summary>
    ''' <param name="e">An EventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)

        Me._IsFocused = True
        Me.Invalidate(Me.ClientRectangle)
    End Sub

    ''' <summary>
    ''' Raises the ItemActivate event.
    ''' </summary>
    ''' <param name="e">An EventArg.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnItemActivate(ByVal e As EventArgs)
        RaiseEvent ItemActivate(Me, e)
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the WinControls.TreeListview.ContainerListView.KeyDown event.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs containing the data.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        Me.OnCheckShiftState(e)

        Select Case e.KeyCode
            Case Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown
                Me.OnPageKeys(e)

            Case Keys.Up, Keys.Down
                Me.OnUpDownKeys(e)

            Case Keys.Return, Keys.Enter
                If (Me._Activation = ItemActivation.Standard AndAlso (Not Me._MultiSelect OrElse (Me._MultiSelect AndAlso Me._AllowMultiSelectActivation))) Then
                    Me.OnItemActivate(EventArgs.Empty)
                End If

            Case Keys.Space
                Me.OnSpaceBarKey(e)

            Case Keys.Escape
                Me.SelectedItems.Clear()

            Case Keys.A
                If (e.Control) Then Me.SelectAll()

            Case Else
                MyBase.OnKeyDown(e)
        End Select
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the KeyUp event
    ''' </summary>
    ''' <param name="e">A KeyEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        If (Not e.Shift) Then Me._MultiSelectMode = MultiSelectModes.Single
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the LostFocus event.
    ''' </summary>
    ''' <param name="e">An EventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        MyBase.OnLostFocus(e)

        Me._IsFocused = False
        Me.Invalidate(Me.ClientRectangle)
    End Sub

    ''' <summary>
    ''' Overriden.  Raises the MouseDown event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        'MISC
        Me._LastClickedPoint = New Point(e.X, e.Y)

        'DETERMINE IF A HEADER WAS PRESSED
        If (Me._HeaderStyle <> ColumnHeaderStyle.None) Then
            'If (Me._HeaderRect.Contains(e.X, e.Y)) Then
                'If (Me._HeaderRect.Contains(e.X, e.Y)) Then
                Dim Col As ContainerColumnHeader

                For I As Integer = 0 To (Me._Columns.Count - 1)
                    Col = Me.Columns(I)
                    Col.Pressed = False

                    If (Col.SizingBounds.Left <= e.X AndAlso Col.SizingBounds.Right >= e.X AndAlso e.Button = Windows.Forms.MouseButtons.Left) Then
                        'If (Col.SizingBounds.Contains(e.X, e.Y) AndAlso e.Button = Windows.Forms.MouseButtons.Left) Then
                        'AUTOSIZE COLUMN
                        If (e.Clicks = 2 AndAlso e.Button = Windows.Forms.MouseButtons.Left) AndAlso
                            (Me._Items.Count > 0 OrElse (TypeOf Me Is TreeListView AndAlso DirectCast(Me, TreeListView).Nodes.Count > 0)) Then
                            Dim Mwid, Twid As Integer

                            'PROCESS THE ITEMS
                            Me.OnProcessColumnMouseDownItems(I, Twid, Mwid)

                            'NOW WE NEED TO MEASURE THE COLUMN TEXT AND COMPARE THAT
                            Twid = Tools.GetStringWidth(Col.Text.ToUpper, Col.Font) + 4
                            If (Twid > Mwid) Then Mwid = Twid
                            Col.Width = Mwid
                        ElseIf (Me._AllowColumnResize AndAlso Col.AllowResize) Then
                            Dim TmpInt As Integer = 0

                            'MISC
                            Me._ColScaleMode = True
                            Me._ColScaleWid = Col.Bounds.Width
                            Me._ScaledCol = I

                            'DRAW THE INITIAL LINE WHEN THEY ARE ADJUSTING THE SIZE OF A COLUMN
                            Windows.Forms.Cursor.Current = Me._ResizeCursor
                            Me._LineStart = Me.PointToScreen(New Point(e.X, 2))
                            If (Me._HscrollBar.Visible) Then TmpInt = Me.Height - 19 Else TmpInt = Me.Height - 2
                            Me._LineEnd = Me.PointToScreen(New Point(e.X, TmpInt))

                            ControlPaint.DrawReversibleLine(Me._LineStart, Me._LineEnd, Me.BackColor)
                        End If

                    'WE FOUND OUR MATCH SO WE DON'T NEED TO GO ANY FURTHER
                    Me.Invalidate()
                    Exit Sub
                    'Exit For
                ElseIf (Col.Bounds.Contains(e.X, e.Y) AndAlso Not Col.SizingBounds.Contains(e.X, e.Y)) Then
                        If (e.Button = Windows.Forms.MouseButtons.Left) Then
                            'DESIGNMODE SUPPORT CALL
                            If (Me.DesignMode) Then Me.ProcessDesignModeColumnClick(e)

                            Col.Pressed = True
                            Me._LastColPressed = Col
                        End If

                        'WE FOUND OUR MATCH SO RAISE THE EVENT THEN EXIT THE FOR LOOP
                        Me.OnColumnHeaderClicked(New ContainerColumnHeaderEventArgs(Col, e))
                    Me.Invalidate()
                    Exit Sub
                    'Exit For
                End If
                Next

            'End If
        End If

            'ROWS
            If (Me._ProcessRows) Then
            If (e.Button = Windows.Forms.MouseButtons.Left OrElse (Me._RightMouseSelects AndAlso e.Button = Windows.Forms.MouseButtons.Right)) Then
                If (Me._Items.Count > 0 AndAlso Me._RowsRect.Contains(e.X, e.Y)) Then
                    Dim TestObj As Object = Nothing

                    If (Me.CheckBoxClicked(Me._CheckBoxRects, e, TestObj)) Then
                        If (e.Button = Windows.Forms.MouseButtons.Left) AndAlso
                           ((e.Clicks = 2 AndAlso (Me._CheckBoxSelection = ItemActivation.Standard OrElse Me._CheckBoxSelection = ItemActivation.TwoClick)) OrElse
                             e.Clicks = 1 AndAlso (Me._CheckBoxSelection = ItemActivation.OneClick)) Then
                            Dim ClItem As ContainerListViewItem = DirectCast(TestObj, ContainerListViewItem)

                            If (ClItem.CheckBoxEnabled) Then ClItem.Checked = Not ClItem.Checked
                        End If
                    ElseIf (Me._getSelectedRowIndex(e.Y) >= 0) Then
                        Dim SelIndex As Integer = Me._getSelectedRowIndex(e.Y)

                        Select Case Me._MultiSelectMode
                            Case MultiSelectModes.Single, MultiSelectModes.Range
                                Me._moveToIndex(SelIndex)
                            Case MultiSelectModes.Selective
                                Me._selectiveSelection(SelIndex)
                        End Select

                        'ACTIVATE THE ITEM(S) IF PERMITTED
                        If (Not Me._MultiSelect OrElse (Me._MultiSelect AndAlso Me._AllowMultiSelectActivation)) Then
                            If (e.Clicks = 1 AndAlso Me._Activation = ItemActivation.OneClick) OrElse (e.Clicks = 2 AndAlso Me._Activation = ItemActivation.TwoClick) Then
                                Me.OnItemActivate(EventArgs.Empty)
                            End If
                        End If
                    Else
                        Me.SelectedItems.Clear()
                        Me.Invalidate()
                    End If
                Else
                    Me.SelectedItems.Clear()
                    Me.Invalidate()
                End If
            End If
        Else
            'IF A COLUMN WAS PRESSED, THEN WE NEVER WOULD HAVE GOTTEN TO THIS POINT.  SO NOW SET IT BACK TO <c>TRUE</c> SO DERIVED CLASSES CAN 
            'PROCESS ANY ROW CLICKS
            Me._ProcessRows = True
        End If
    End Sub

    ''' <summary>
    ''' Raises the MouseMove event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        If (Me._LastRowHovered IsNot Nothing) Then
            Me._LastRowHovered.Hovered = False
            Me._LastRowHovered = Nothing
        End If

        'IF THE MOUSEBUTTON IS PRESSED DOWN ON A HEADER COLUMN, MOVING WILL ATTEMPT TO MOVE THE POSITION OF THAT COLUMN
        If (Me._LastColPressed IsNot Nothing AndAlso Me._AllowColumnReorder) Then
            If (Math.Abs(e.X - Me._LastClickedPoint.X) > 3) Then
                'ControlPaint.DrawReversibleFrame(New Rectangle(e.X, e.Y, Me._LastColPressed.Width, Me.RowHeight), Me.BackColor, FrameStyle.Thick)
                'TODO: Not Supported.  NEED CODE HERE - SET THE RECTANGLE FOR DRAG POS
            End If
        ElseIf (Me._ColScaleMode AndAlso Me._AllowColumnResize AndAlso Me._Columns(Me._ScaledCol).AllowResize) Then
            Dim Scol As ContainerColumnHeader = Me.Columns(Me._ScaledCol)
            Dim EndX As Integer = 0

            If (Me._VscrollBar.Visible) Then EndX = Me._VscrollBar.Width + 2 Else EndX = 2
            Me._LastColHovered = Nothing
            Windows.Forms.Cursor.Current = Me._ResizeCursor
            Me._ColScalePos = e.X - Me._LastClickedPoint.X

            If ((Me._ColScalePos + Me._ColScaleWid) <= 0) Then
                If (Scol.Index = 0) Then Scol.Width = 2 Else Scol.Width = 1
            Else
                Scol.Width = Me._ColScalePos + Me._ColScaleWid
            End If

            'REVERSE THE INITIAL COLUMN MOVE LINE WHEN THEY INITIALLY CLICKED DOWN
            If (e.X > Scol.Bounds.X AndAlso e.X <= (Me.ClientRectangle.Width - EndX)) Then
                Dim TmpInt As Integer = 0

                ControlPaint.DrawReversibleLine(Me._LineStart, Me._LineEnd, Me.BackColor)

                'SET THE NEW COORDINATES
                Me._LineStart = Me.PointToScreen(New Point(e.X, 2))
                If (Me._HscrollBar.Visible) Then TmpInt = Me.Height - 19 Else TmpInt = Me.Height - 2
                Me._LineEnd = Me.PointToScreen(New Point(e.X, TmpInt))
                'NOW PAINT THE NEW LINE
                ControlPaint.DrawReversibleLine(Me._LineStart, Me._LineEnd, Me.BackColor)
            End If
        Else
            If (Me._Columns.Count > 0 AndAlso Me._HeaderStyle <> ColumnHeaderStyle.None) Then
                Windows.Forms.Cursor.Current = Cursors.Default

                'If (Me._HeaderRect.Contains(e.X, e.Y)) Then
                For I As Integer = 0 To (Me._Columns.Count - 1)
                        Dim Col As ContainerColumnHeader = Me._Columns(I)

                    If (Col.Bounds.Left < e.X AndAlso Col.Bounds.Right < e.X) Then
                        'If (Col.Bounds.Contains(e.X, e.Y)) Then
                        Col.Hovered = True
                        Me._LastColHovered = Col
                    Else
                        Col.Hovered = False
                        End If

                    If (Me._AllowColumnResize AndAlso Col.AllowResize AndAlso Col.SizingBounds.Left <= e.X AndAlso Col.SizingBounds.Right >= e.X) Then
                        'If (Me._AllowColumnResize AndAlso Col.AllowResize AndAlso Col.SizingBounds.Contains(e.X, e.Y)) Then
                        Windows.Forms.Cursor.Current = Me._ResizeCursor
                    End If
                Next

                    Me.Invalidate()
                    Exit Sub
                    'End If
                End If

            If (Me._LastColHovered IsNot Nothing) Then
                Me._LastColHovered.Hovered = False
                Me._LastColHovered = Nothing
                Me.Invalidate()
            End If

            If (Me._Items.Count > 0) Then
                If (Me._RowsRect.Contains(e.X, e.Y)) Then
                    Dim SelIndex As Integer = Me._getSelectedRowIndex(e.Y)

                    If (SelIndex <> -1) Then
                        If (Me._LastRowHovered IsNot Nothing) Then Me._LastRowHovered.Hovered = False
                        Me._LastRowHovered = Me._Items(SelIndex)
                        Me._LastRowHovered.Hovered = True
                    End If
                    Me.Invalidate()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Raises the MouseUp event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        'MISC
        Me._LastClickedPoint = Point.Empty

        If (Me._ColScaleMode) Then
            Me._ColScaleMode = False
            Me._ColScalePos = 0
            Me._ScaledCol = -1
            Me._ColScaleWid = 0

            Me.OnAdjustScrollBars()
            Me.Invalidate()
        End If

        If (Me._LastColPressed IsNot Nothing) Then
            Me._LastColPressed.Pressed = False

            'IF THE _LASTCOLPRESSED IS SOMETHING AND _LASTCOLHOVERED IS NOTHING, THEN WE HAVE A PROBLEM.  SET THE _LASTCOLHOVERED TO
            'THE _LASTCOLPRESSED IN THIS SITUATION
            If (Me._LastColHovered Is Nothing) Then Me._LastColHovered = Me._LastColPressed

            If (Me._LastColHovered.Bounds.Contains(e.X, e.Y) AndAlso Not Me._LastColPressed.SizingBounds.Contains(e.X, e.Y) AndAlso e.Button = Windows.Forms.MouseButtons.Left) Then
                'ORDER IS IMPORTANT HERE.  SET THE LASTPAINTED COLUMN FIRST
                If (Me._SelectedCol IsNot Nothing) Then
                    If (Me._LastPaintedSortCol = Me._SelectedCol.Index) Then
                        Me._LastPaintedSortCol = -1
                    Else
                        Me._LastPaintedSortCol = Me._SelectedCol.Index
                    End If
                End If
                'CHANGE THE CURRENTLY SELECTED COLUMN.  
                Me._SelectedCol = Me._LastColPressed
            End If
        End If
        Me._LastColPressed = Nothing

        'CHECK FOR A CONTEXT CLICK AND RAISE THE APPROPRIATE CONTEXTMENU
        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            If (Me._HeaderRect.Contains(e.X, e.Y)) Then
                If (Me._HeaderMenu IsNot Nothing) Then
                    Me.OnColumnHeaderContextMenuRequested(e)
                    Me._HeaderMenu.Show(Me, New Point(e.X, e.Y))
                End If
            Else
                If (Me.ContextMenuStrip IsNot Nothing) Then
                    Me.OnContextMenuRequested(e)
                    Me.ContextMenuStrip.Show(Me, New Point(e.X, e.Y))
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Raises the MouseWheel event.
    ''' </summary>
    ''' <param name="e">A MouseEventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
        Dim HsDelta As Integer = Me._HscrollBar.Value - Me._HscrollBar.SmallChange * CType(e.Delta / 100, Integer)
        Dim VsDelta As Integer = Me._VscrollBar.Value - Me._VscrollBar.SmallChange * CType(e.Delta / 100, Integer)

        If (e.Delta > 0) Then
            If (Me._VscrollBar.Visible OrElse Not Me._Scrollable) Then
                If (VsDelta < 0) Then Me._VscrollBar.Value = 0 Else Me._VscrollBar.Value = VsDelta
            ElseIf (Me._HscrollBar.Visible) Then
                If (HsDelta < 0) Then Me._HscrollBar.Value = 0 Else Me._HscrollBar.Value = HsDelta
            End If
        ElseIf (e.Delta < 0) Then
            Dim HsVal As Integer = Me._HscrollBar.Maximum - Me._HscrollBar.LargeChange
            Dim VsVal As Integer = Me._VscrollBar.Maximum - Me._VscrollBar.LargeChange

            If (Me._VscrollBar.Visible OrElse Not Me._Scrollable) Then
                If (VsDelta > VsVal) Then Me._VscrollBar.Value = VsVal Else Me._VscrollBar.Value = VsDelta
            ElseIf (Me._HscrollBar.Visible) Then
                If (HsDelta > HsVal) Then Me._HscrollBar.Value = HsVal Else Me._HscrollBar.Value = HsDelta
            End If
        End If
    End Sub

    ''' <summary>
    ''' Overriden.  Paints the control.
    ''' </summary>
    ''' <param name="e">A PaintEventArg to pass.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim Gr As Graphics = e.Graphics
        Dim Rect As Rectangle = Me.ClientRectangle

        Me.OnDrawBackGround(Gr, Rect)
        Me.OnDrawRows(Gr, Rect)
        Me._drawColumnHeaders(Gr, Rect)
        Me._drawBorder(Gr, Rect)
        Me._drawExtra(Gr, Rect)
        Me.OnAdjustScrollBars()
    End Sub

    ''' <summary>
    ''' Occurs when the PageUp, PageDown, Home, or End Keys are pressed.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnPageKeys(ByVal e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Home
                If (Me._VscrollBar.Visible) Then Me._VscrollBar.Value = 0
                If (Me._HscrollBar.Visible) Then Me._HscrollBar.Value = 0
                Me._moveToIndex(0)

            Case Keys.End
                If (Me._VscrollBar.Visible) Then
                    Me._VscrollBar.Value = Me._VscrollBar.Maximum - Me._VscrollBar.LargeChange
                End If
                Me._moveToIndex(Me._Items.Count - 1)

            Case Keys.PageUp
                If (Me._VscrollBar.Visible) Then
                    If (Me._VscrollBar.LargeChange > Me._VscrollBar.Value) Then
                        Me._VscrollBar.Value = 0
                    Else
                        Me._VscrollBar.Value = Me._VscrollBar.LargeChange
                    End If
                    Me._moveToIndex(CType(Math.Round(Me.VScroll.Value / Me.RowHeight), Integer))
                Else
                    Me._moveToIndex(0)
                End If

            Case Keys.PageDown
                If (Me._VscrollBar.Visible) Then
                    Dim Diff As Integer = 0

                    If (Me._HscrollBar.Visible) Then Diff = 2 Else Diff = 3

                    If ((Me._VscrollBar.Value + Me._VscrollBar.LargeChange) > (Me._VscrollBar.Maximum - Me._VscrollBar.LargeChange)) Then
                        Me._VscrollBar.Value = Me._VscrollBar.Maximum - Me._VscrollBar.LargeChange
                    Else
                        Me._VscrollBar.Value = Me._VscrollBar.Value + Me._VscrollBar.LargeChange
                    End If
                    Me._moveToIndex((CType(Math.Round(Me.VScroll.Value / Me.RowHeight), Integer) + CType(Math.Round((Me.VScroll.LargeChange / Me.RowHeight)), Integer) - Diff))
                Else
                    Me._moveToIndex(Me._Items.Count - 1)
                End If
        End Select

        e.Handled = True
    End Sub

    ''' <summary>
    ''' Occurs on the MouseDown when the control is checking to see if a ColumnHeader was clicked.
    ''' </summary>
    ''' <param name="aColIndex">The Index of the ColumnHeader that is currently being checked/processed.</param>
    ''' <param name="aTwid">A variable being used to check the width of the current subitem string.</param>
    ''' <param name="aMwid">A variable being used to keep track of the largest aTwid (the largest string processed).</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnProcessColumnMouseDownItems(ByVal aColIndex As Integer, ByRef aTwid As Integer, ByVal aMwid As Integer)
        For J As Integer = 0 To (Me._Items.Count - 1)
            Dim Clvi As ContainerListViewItem = Me._Items(J)

            If (aColIndex > 0 AndAlso aColIndex <= Clvi.SubItems.Count) Then
                Dim ThisFont As Font

                If (Clvi.UseItemStyleForSubItems) Then ThisFont = Clvi.Font Else ThisFont = Clvi.SubItems(aColIndex - 1).Font
                aTwid = Tools.GetStringWidth(Clvi.SubItems(aColIndex - 1).Text.ToUpper, ThisFont) + 4
            Else
                aTwid = Tools.GetStringWidth(Clvi.Text.ToUpper, Clvi.Font) + 4
            End If

            If (aTwid > aMwid) Then aMwid = aTwid
        Next
    End Sub

    ''' <summary>
    ''' Raises the Resize event.
    ''' </summary>
    ''' <param name="e">an EventArg.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)

        Me._generateHeaderRect()
        Me._generateViewableRowsRectangle()
    End Sub

    ''' <summary>
    ''' Occurs when the ScrollBars are scrolled.
    ''' </summary>
    ''' <param name="Sender">The object that raised the event.</param>
    ''' <param name="e">An EventArg.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnScroll(ByVal sender As Object, ByVal e As EventArgs)
        If (Me._Scrollable) Then
            Me._endAnyEdits()
            Me.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Sets focus to the specified object.
    ''' </summary>
    ''' <param name="aClObj">The ContainerListViewObject to set focus to.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSetFocusedObject(ByVal aClObj As ContainerListViewObject)
        Dim Clvi As ContainerListViewItem = DirectCast(aClObj, ContainerListViewItem)

        If (Me._FocusedItem IsNot Nothing) Then
            Me._FocusedItem.Focused = False
            Me._FocusedIndex = -1
        End If
        Me._FocusedItem = Clvi
        Me._FocusedIndex = Clvi.Index
    End Sub

    ''' <summary>
    ''' Occurs when the control needs to be sorted.
    ''' </summary>
    ''' <param name="aIndex">The zero-based index of the column to sort on.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSort(ByVal aIndex As Integer)
        Me._Items.Sort()
    End Sub

    ''' <summary>
    ''' Handles the SpaceBar key which Checks/UnChecks checkboxes on the Item.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSpaceBarKey(ByVal e As KeyEventArgs)
        If (e.Shift) Then
            For Each ClObj As ContainerListViewObject In Me._SelectedItems
                If (ClObj.CheckBoxVisible AndAlso ClObj.CheckBoxEnabled) Then ClObj.Checked = Not ClObj.Checked
            Next
        Else
            Me._SelectedItems.Clear()
            Me._FocusedIndex = Me._FocusedItem.Index
            Me._FirstSelected = Me._FocusedIndex
            Me._FocusedItem.Selected = True
            Me._FocusedItem.Checked = Not Me._FocusedItem.Checked
            Me._showSelectedItems()
            Me._makeSelectedVisible()
        End If

        e.Handled = True
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' Occurs when the Up or Down Keys are pressed.
    ''' </summary>
    ''' <param name="e">A KeyEventArgs.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnUpDownKeys(ByVal e As KeyEventArgs)
        If (Me._FocusedItem IsNot Nothing AndAlso Me._Items.Count > 0) Then
            Dim Index As Integer = Me._FocusedIndex

            Select Case e.KeyCode
                Case Keys.Down
                    Index += 1
                Case Keys.Up
                    Index -= 1
            End Select

            Me._moveToIndex(Index)
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Processes Column clicks if the control is in DesignMode.
    ''' </summary>
    ''' <param name="e">The MouseArg containing the data.</param>
    ''' <remarks>Calling this method will allow for design-time support of the control's columns.</remarks>
    Protected Sub ProcessDesignModeColumnClick(ByVal e As MouseEventArgs)
        If (Me.DesignMode) Then
            For Each Col As ContainerColumnHeader In Me._Columns
                If (Col.Bounds.Contains(e.X, e.Y)) Then
                    Dim SelList As New ArrayList(1)
                    Dim Svc As ISelectionService = DirectCast(Me.GetService(GetType(ISelectionService)), ISelectionService)

                    SelList.Add(Col)
                    Svc.SetSelectedComponents(SelList, SelectionTypes.Auto)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Generates an object from its XML representation. 
    ''' </summary>
    ''' <param name="aReader">The XmlReader stream from which the object is deserialized.</param>
    ''' <remarks>
    ''' See the <see cref="T:System.Xml.Serialization.IXmlSerializable"/> interface for more details.
    ''' </remarks>
    Public Sub ReadXml(ByVal aReader As XmlReader) Implements IXmlSerializable.ReadXml
        Me.BeginUpdate()
        Me._readXml(aReader, Me, String.Empty)
        aReader.Close()
        Me.EndUpdate()
    End Sub

    ''' <summary>
    ''' Selects all Items in the control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub SelectAll()
        If (Me._MultiSelect) Then
            Me.BeginUpdate()
            For Each Itm As ContainerListViewItem In Me._Items
                Itm.Selected = True
            Next
            Me.EndUpdate()
        End If
    End Sub

    ''' <summary>
    ''' Sets the ContainerListViewObject currently in edit mode.
    ''' </summary>
    ''' <param name="aClObj">The ContainerListViewObject to set.</param>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Protected Friend Sub SetEditedObject(ByVal aClObj As ContainerListViewObject)
        Me._EditingObj = aClObj
    End Sub

    ''' <summary>
    ''' Sets the Focused Item.
    ''' </summary>
    ''' <param name="aClObj"></param>
    ''' <remarks></remarks>
    Protected Friend Sub SetFocusedObject(ByVal aClObj As ContainerListViewObject)
        Me.OnSetFocusedObject(aClObj)
    End Sub

    ''' <summary>
    ''' Sort all of the Items in the control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Sort()
        Me.Sort(0)
    End Sub

    ''' <summary>
    ''' Sort all of the Items in the control.
    ''' </summary>
    ''' <param name="aIndex">The zero-based column index to sort on.</param>
    ''' <remarks></remarks>
    Public Overloads Sub Sort(ByVal aIndex As Integer)
        If (Me._Sorting <> SortOrder.None) Then
            If (Me._Columns.Count > 0 AndAlso aIndex < Me._Columns.Count) Then
                Me.BeginUpdate()
                Me._DefaultComparer.ColumnIndex = aIndex
                Me._DefaultComparer.Order = Me._Sorting
                Me.OnSort(aIndex)
                Me.EndUpdate()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Processes Windows messages.
    ''' </summary>
    ''' <param name="m">A Windows message.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        'THIS LINE MAKES ARROW AND TAB KEY EVENTS CAUSE OnKeyXXX EVENTS TO FIRE
        If (m.Msg = CType(Enums.WinMsgs.WM_GETDLGCODE, Integer)) Then
            m.Result = New IntPtr(CType(Enums.DialogCodes.DLGC_WANTCHARS, Integer) Or CType(Enums.DialogCodes.DLGC_WANTARROWS, Integer) Or m.Result.ToInt32)
        End If
    End Sub

    ''' <summary>
    ''' Converts an object into its XML representation.
    ''' </summary>
    ''' <param name="aWriter">The XmlWriter stream to which the object is serialized.</param>
    ''' <remarks>See the <see cref="T:System.Xml.Serialization.IXmlSerializable"/> interface for more details.</remarks>
    Public Sub WriteXml(ByVal aWriter As XmlWriter) Implements IXmlSerializable.WriteXml
        'SET SOME SPECIFIC SETTINGS
        With aWriter
            .WriteComment(" " & Date.Now.ToString("MM/dd/yyyy") & " ")
            Me._writeXml(Me, aWriter)

            .Flush()
            .Close()
        End With
    End Sub

#End Region

#Region " Procedures "

    Private Sub _adjustScrollBars()
        If (Me._Items.Count > 0 OrElse (Me.Columns.Count > 0 AndAlso Not Me._ColScaleMode)) Then
            Dim ColWdths As Integer = Me.GetSumOfVisibleColumnWidths
            Dim Rect As Rectangle = Me.ClientRectangle
            Dim RowsHeight As Integer = Me._Items.Count * Me.RowHeight
            Dim ShowVert, ShowHoriz As Boolean
            Dim VertVal, HorizVal As Integer

            'MISC
            VertVal = (Rect.Height - Me.HScroll.Height - 2)
            HorizVal = (Rect.Width - Me.VScroll.Width - 2)

            'SET THE VSCROLL
            With Me._VscrollBar
                .Left = (Rect.Left + Rect.Width - .Width - 2)
                .Top = Rect.Top + 2
                .SmallChange = Me.RowHeight
                .Maximum = RowsHeight
            End With

            'SET THE HSCROLL
            With Me._HscrollBar
                .Left = Rect.Left + 2
                .Top = Rect.Top + Rect.Height - .Height - 2
                Try
                    If (Me._Columns.Count > 0) Then
                        .SmallChange = Me.GetSumOfAllColumnWidths \ Me._Columns.Count
                    Else
                        .SmallChange = 0
                    End If
                Catch ex As Exception
                    .SmallChange = 0
                End Try
                .Maximum = ColWdths
            End With

            'DO SOME CHECKS
            If (Me._Scrollable AndAlso RowsHeight > VertVal) Then ShowVert = True Else ShowVert = False
            If (Me._Scrollable AndAlso ColWdths > HorizVal) Then ShowHoriz = True Else ShowHoriz = False
            If (Not Me._Scrollable) Then
                ShowVert = False
                ShowHoriz = False
            End If

            If (ShowVert And ShowHoriz) Then
                Me._VscrollBar.Height = VertVal
                If (VertVal > 0) Then Me._VscrollBar.LargeChange = Me.EnsureVerticalScroll(VertVal, True) Else Me._VscrollBar.LargeChange = 0

                Me._HscrollBar.Width = HorizVal
                If (HorizVal > 0) Then Me._HscrollBar.LargeChange = HorizVal Else Me._HscrollBar.LargeChange = 0

                Me._HscrollBar.Show()
                Me._VscrollBar.Show()
            ElseIf (ShowVert AndAlso Not ShowHoriz) Then
                Me._HscrollBar.Hide()
                Me._HscrollBar.Value = 0
                If (HorizVal > 0) Then Me._HscrollBar.LargeChange = HorizVal Else Me._HscrollBar.LargeChange = 0

                Me._VscrollBar.Height = VertVal + Me._HscrollBar.Height - 2
                If (VertVal > 0) Then Me._VscrollBar.LargeChange = Me.EnsureVerticalScroll(VertVal, False) Else Me._VscrollBar.LargeChange = 0
                Me._VscrollBar.Show()
            ElseIf (Not ShowVert AndAlso ShowHoriz) Then
                Me._VscrollBar.Hide()
                Me._VscrollBar.Value = 0
                If (VertVal > 0) Then Me._VscrollBar.LargeChange = Me.EnsureVerticalScroll(VertVal, True) Else Me._VscrollBar.LargeChange = 0

                Me._HscrollBar.Width = HorizVal + Me._VscrollBar.Width - 2
                If (HorizVal > 0) Then Me._HscrollBar.LargeChange = HorizVal Else Me._HscrollBar.LargeChange = 0
                Me.HScroll.Show()
            Else
                Me._VscrollBar.Hide()
                Me._VscrollBar.Value = 0
                If (VertVal > 0) Then Me._VscrollBar.LargeChange = Me.EnsureVerticalScroll(VertVal, False) Else Me._VscrollBar.LargeChange = 0

                Me._HscrollBar.Hide()
                Me._HscrollBar.Value = 0
                If (HorizVal > 0) Then Me._HscrollBar.LargeChange = HorizVal Else Me._HscrollBar.LargeChange = 0
            End If
        End If
    End Sub

    Private Sub _attachHandlers()
        AddHandler Me._HscrollBar.ValueChanged, AddressOf Me.OnScroll
        AddHandler Me._VscrollBar.ValueChanged, AddressOf Me.OnScroll
        AddHandler Me._Columns.ItemsChanged, AddressOf Me._columnCollectionChangedHandler
    End Sub

    Private Sub _baseFontChanged()
        Me.BeginUpdate()
        For Each Itm As ContainerListViewItem In Me._Items
            Itm.ChangeGlobalFont(Me.Font)

            'NOW DO THE SUBITEMS
            For Each SU As ContainerListViewObject.ContainerListViewSubItem In Itm.SubItems
                SU.ChangeGlobalFont(Me.Font)
            Next
        Next
        Me.EndUpdate()
    End Sub

    Private Sub _baseForeColorChanged()
        Me.BeginUpdate()
        For Each Itm As ContainerListViewItem In Me._Items
            Itm.ChangeGlobalForeColor(Me.ForeColor)

            'NOW DO THE SUBITEMS
            For Each SU As ContainerListViewObject.ContainerListViewSubItem In Itm.SubItems
                SU.ChangeGlobalForeColor(Me.ForeColor)
            Next
        Next
        Me.EndUpdate()
    End Sub

    Private Sub _columnCollectionChangedHandler(ByVal sender As Object, ByVal e As ItemActionEventArgs)
        If (Not Me._UpdateTransactions AndAlso e.Action <> Enums.CollectionActions.Clearing) Then
            Me._generateHeaderRect()
            Me._generateViewableRowsRectangle()
            Me.Invalidate()
        End If
    End Sub

    Private Sub _detachHandlers()
        RemoveHandler Me._HscrollBar.ValueChanged, AddressOf OnScroll
        RemoveHandler Me._VscrollBar.ValueChanged, AddressOf OnScroll
        RemoveHandler Me._Columns.ItemsChanged, AddressOf Me._columnCollectionChangedHandler
    End Sub

    Private Function _determineBackGroundColor(ByVal aObj As ContainerListViewObject, Optional ByVal aSubItem As ContainerListViewObject.ContainerListViewSubItem = Nothing) As Color
        Dim BrushColor As Color = Color.Empty

        'LISTED IN ORDER OF PRECEDENCE
        If (Not Me.Enabled) Then
            BrushColor = Me._DisabledColor

        ElseIf (Me._DoRowTracking And aObj.Hovered) Then
            BrushColor = Color.FromArgb(Me._AlphaComponent, Me._RowTrackColor)
            If (aSubItem IsNot Nothing AndAlso Not Me._FullRowSelect) Then
                If (aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aObj) Else BrushColor = Me._getItemColor(aSubItem)
            End If

        ElseIf (aObj.Selected AndAlso Me.IsFocused) Then
            BrushColor = Color.FromArgb(Me._AlphaComponent, Me._RowSelectColor)
            If (aSubItem IsNot Nothing AndAlso Not Me._FullRowSelect) Then
                If (aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aObj) Else BrushColor = Me._getItemColor(aSubItem)
            End If

        ElseIf (aObj.Selected AndAlso Not Me.IsFocused AndAlso Not Me._HideSelection) Then
            BrushColor = Color.FromArgb(Me._AlphaComponent, Me._DisabledColor)
            If (aSubItem IsNot Nothing AndAlso Not Me._FullRowSelect) Then
                If (aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aObj) Else BrushColor = Me._getItemColor(aSubItem)
            End If

        ElseIf (Me._DoColTracking AndAlso Me._LastColHovered IsNot Nothing) Then
            If (Me._LastColHovered.Index = 0) Then BrushColor = Color.Transparent Else BrushColor = Me._getItemColor(aObj)

            If (aSubItem IsNot Nothing) Then
                If (aSubItem.Index = (Me._LastColHovered.Index - 1)) Then
                    BrushColor = Color.FromArgb(Me._AlphaComponent, Me._ColTrackColor)
                Else
                    If (aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aObj) Else BrushColor = Me._getItemColor(aSubItem)
                End If
            End If

        ElseIf (Me._ColSortEnabled AndAlso Me._SelectedCol IsNot Nothing AndAlso Me._LastPaintedSortCol <> Me._SelectedCol.Index) Then
            If (Me._SelectedCol.Index = 0) Then BrushColor = Color.FromArgb(Me._AlphaComponent, Me._ColSortColor) Else BrushColor = Me._getItemColor(aObj)

            If (aSubItem IsNot Nothing) Then
                If (aSubItem.Index = (Me._SelectedCol.Index - 1)) Then
                    BrushColor = Color.FromArgb(Me._AlphaComponent, Me._ColSortColor)
                Else
                    If (aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aObj) Else BrushColor = Me._getItemColor(aSubItem)
                End If
            End If

        Else
            BrushColor = Me._getItemColor(aObj)
            If (aSubItem IsNot Nothing AndAlso Not aObj.UseItemStyleForSubItems) Then BrushColor = Me._getItemColor(aSubItem)
        End If

        Return BrushColor
    End Function

    Private Sub _drawBorder(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Me._VisualStyles) Then
            Dim OldReg As Region = aGr.Clip

            'THIS IS NOT REALLY XP STYLE.  JUST SIMULATED
            aGr.Clip = New Region(Rect)
            aGr.DrawRectangle(New Pen(Color.CornflowerBlue), Rect.Left, Rect.Top, Rect.Width - 1, Rect.Height - 1)
            aGr.DrawRectangle(New Pen(Me.BackColor), Rect.Left + 1, Rect.Top + 1, Rect.Width - 3, Rect.Height - 3)
            aGr.Clip = OldReg
        Else
            Select Case Me.BorderStyle
                Case CType(Windows.Forms.BorderStyle.FixedSingle, BorderStyleType)
                    aGr.DrawRectangle(SystemPens.ControlDarkDark, Rect.Left, Rect.Top, Rect.Width, Rect.Height)
                Case CType(Border3DStyle.Bump, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.Bump)
                Case CType(Border3DStyle.Etched, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.Etched)
                Case CType(Border3DStyle.Flat, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.Flat)
                Case CType(Border3DStyle.Raised, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.Raised)
                Case CType(Border3DStyle.RaisedInner, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.RaisedInner)
                Case CType(Border3DStyle.RaisedOuter, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.RaisedOuter)
                Case CType(Border3DStyle.Sunken, BorderStyleType), CType(Windows.Forms.BorderStyle.Fixed3D, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.Sunken)
                Case CType(Border3DStyle.SunkenInner, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.SunkenInner)
                Case CType(Border3DStyle.SunkenOuter, BorderStyleType)
                    ControlPaint.DrawBorder3D(aGr, Rect.Left, Rect.Top, Rect.Width, Rect.Height, Border3DStyle.SunkenOuter)
            End Select
        End If
    End Sub

    Private Sub _drawExtra(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Me._HscrollBar.Visible AndAlso Me._VscrollBar.Visible) Then
            aGr.ResetClip()
            aGr.FillRectangle(SystemBrushes.Control, Rect.Width - Me._VscrollBar.Width - Me._BorderWidth, Rect.Height - Me._HscrollBar.Height - Me._BorderWidth, Me._VscrollBar.Width, Me._HscrollBar.Height)
        End If
    End Sub
    Public Property ColumnForeColor() As Color = SystemColors.WindowText
    Public Property ColumnBackColor() As Color = SystemColors.Control
    Private Sub _drawColumnHeaders(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Me._VisualStyles) Then
            Me._drawHeadersXPStyled(aGr, Rect)
        ElseIf (Me._HeaderStyle <> ColumnHeaderStyle.None) Then
            Dim LastCol As ContainerColumnHeader = Nothing

            aGr.FillRectangle(New SolidBrush(ColumnBackColor), Rect.Left, Rect.Top, Rect.Width, Me._HdrBuffer)
            'aGr.FillRectangle(New SolidBrush(ColumnBackColor), Rect.Left + 2, Rect.Top + 2, Rect.Width - 2, Me._HdrBuffer)
            'Me.CalcSpringWids(Rect)

            'RENDER COLUMN HEADERS AND TRAILING COLUMN HEADERS
            For Each CH As ContainerColumnHeader In Me._Columns
                If (Not CH.Hidden) Then
                    Dim ColRect As Rectangle = CH.Bounds
                    Dim bkClr As Color = CH.HeaderBackColor
                    If bkClr = SystemColors.Control Then bkClr = ColumnBackColor
                    Dim frClr As Color = CH.HeaderForeColor
                    If frClr = SystemColors.WindowText Then frClr = ColumnForeColor
                    LastCol = CH
                    aGr.Clip = New Region(ColRect)
                    aGr.FillRectangle(New SolidBrush(bkClr), ColRect.Left, ColRect.Top, ColRect.Width, Me._HdrBuffer)
                    If (ColRect.Left < (Rect.Left + Rect.Width - 2)) Then
                        Dim ImageSpacing As Integer = 0
                        Dim TextBrush As Brush = New SolidBrush(frClr)
                        Dim TempStr As String = Tools.TruncateString(CH.Text, CH.Font, CH.Width, 8, aGr)
                        Dim StrSize As SizeF = Helpers.Tools.MeasureDisplayString(aGr, TempStr, CH.Font)

                        'RENDER THE COLUMNHEADER
                        'ControlPaint.fil(aGr, ColRect, ButtonState.Flat)
                        'If (Me._HeaderStyle = ColumnHeaderStyle.Clickable AndAlso CH.Pressed) Then
                        '    ControlPaint.DrawButton(aGr, ColRect, ButtonState.Flat)
                        'Else
                        '    ControlPaint.DrawButton(aGr, ColRect, ButtonState.Normal)
                        'End If

                        'DETERMINE IF WE SHOULD DRAW AN IMAGE
                        If (CH.ImageList IsNot Nothing AndAlso CH.ImageIndex >= 0 AndAlso CH.ImageIndex < CH.ImageList.Images.Count) Then
                            ImageSpacing = 18
                        End If

                        Select Case CH.TextAlign
                            Case HorizontalAlignment.Left
                                Dim Offset As Integer = 0

                                If (ColRect.X <= 0) Then Offset = 2
                                If (ImageSpacing > 0) Then
                                    aGr.DrawImage(Me._ImageList.Images.Item(CH.ImageIndex), ColRect.X + Offset + 2, ColRect.Top + 1, 16, 16)
                                End If
                                aGr.DrawString(TempStr, CH.Font, TextBrush, ColRect.X + Offset + 2 + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2) + 2)
                            Case HorizontalAlignment.Right
                                Dim Var As Integer = ColRect.Right - 3 - ImageSpacing - CType(StrSize.Width, Integer)

                                If (ImageSpacing > 0) Then aGr.DrawImage(Me._ImageList.Images(CH.ImageIndex), Var, ColRect.Top + 1, 16, 16)
                                aGr.DrawString(TempStr, CH.Font, TextBrush, Var + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2) + 2)

                            Case HorizontalAlignment.Center
                                Dim Var As Integer = ColRect.X + ((ColRect.Width - (ImageSpacing + CType(StrSize.Width, Integer))) \ 2)

                                If (ImageSpacing > 0) Then aGr.DrawImage(Me._ImageList.Images(CH.ImageIndex), Var + 2, ColRect.Top + 1, 16, 16)
                                aGr.DrawString(TempStr, CH.Font, TextBrush, Var + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2) + 2)
                        End Select
                    End If
                End If
            Next

            'RENDER ONLY TRAILING COLUMN HEADER IF THE END OF THE LAST COLUMN ENDS BEFORE THE BOUNDARY OF THE LISTVIEW
            aGr.Clip = New Region(New Rectangle(Rect.Left + 2, Rect.Top + 2, Rect.Width - 5, Rect.Top + Me._HdrBuffer))
            If (Me._Columns.Count > 0 AndAlso LastCol IsNot Nothing) Then
                Dim LastColRect As Rectangle = LastCol.Bounds

                If (LastColRect.Right < Rect.Right) Then
                    aGr.FillRectangle(New SolidBrush(Me.BackColor), LastColRect.Right, Rect.Top + 2, (Rect.Right - LastColRect.Right), Me._HdrBuffer)
                End If
            Else
                aGr.FillRectangle(New SolidBrush(Me.BackColor), aGr.ClipBounds)
            End If
        End If
    End Sub

    Private Sub _drawHeadersXPStyled(ByVal aGr As Graphics, ByVal Rect As Rectangle)
        If (Me._HeaderStyle <> ColumnHeaderStyle.None) Then
            Dim Hdc As IntPtr
            Dim Theme As IntPtr
            Dim Last As Integer = 2
            Dim CH As ContainerColumnHeader
            Dim Lp As Integer = Rect.Left
            Dim Tp As Integer = Rect.Top + 2
            Dim LpScr As Integer = Lp - Me.HScroll.Value
            Dim Rect1, Rect2 As VisualStyles.RECT

            'Me.CalcSpringWids(Rect)
            Dim RectWdth As Integer = Rect.Left + Rect.Width

            For I As Integer = 0 To (Me._Columns.Count - 1)
                Dim Wdth, ImageSpacing As Integer
                Dim Var As Integer = LpScr + Last

                CH = Me._Columns(I)
                If (Not CH.Hidden) Then
                    Dim State As Integer
                    Dim ColRect As Rectangle = CH.Bounds
                    Dim TextBrush As Brush = New SolidBrush(CH.ForeColor)
                    Dim TempStr As String = Tools.TruncateString(CH.Text, CH.Font, CH.Width, 8, aGr)
                    Dim StrSize As SizeF = Helpers.Tools.MeasureDisplayString(aGr, TempStr, CH.Font)

                    'SET THE CURRENT STATE OF THE COLUMN
                    If (Me._HeaderStyle = ColumnHeaderStyle.Clickable AndAlso CH.Pressed) Then
                        State = 3 'PRESSED
                    ElseIf (Me._HeaderStyle <> ColumnHeaderStyle.None AndAlso CH.Hovered) Then
                        State = 2 'HOT
                    Else
                        State = 1 'NORMAL
                    End If

                    'DRAW THE HEADER BACKGROUND
                    Try
                        'SET THE VARIABLES
                        Hdc = aGr.GetHdc()
                        Theme = Helpers.VisualStyles.OpenThemeData(Me.Handle, "HEADER")
                        Rect1 = New VisualStyles.RECT(ColRect.Left, ColRect.Top, ColRect.Right, ColRect.Bottom - 2)
                        Rect2 = New VisualStyles.RECT(Lp, Tp, Rect.Width - 6, Me.HeaderBuffer)

                        'DO THE DRAWING
                        Helpers.VisualStyles.DrawThemeBackground(Theme, Hdc, 1, State, Rect1, Rect2)
                    Catch ex As Exception
                        'DO <c>NOTHING</c> FOR NOW
                    Finally
                        'RELEASE THE RESOURCES
                        If (Not IntPtr.Zero.Equals(Hdc)) Then aGr.ReleaseHdc(Hdc)
                        If (Not IntPtr.Zero.Equals(Theme)) Then Helpers.VisualStyles.CloseThemeData(Theme)
                    End Try

                    'SET THE NEW CLIP
                    If ((Rect.Left + Last + CH.Width) > RectWdth) Then
                        Wdth = (Rect.Width - (Rect.Left + Last)) - 4
                    Else
                        Wdth = CH.Width - 2
                    End If
                    aGr.Clip = New Region(New Rectangle(Var + 2, Tp, Wdth + Me._HscrollBar.Value, Rect.Top + Me._HdrBuffer))

                    'DETERMINE IF WE SHOULD DRAW AN IMAGE
                    If (Me._ImageList IsNot Nothing AndAlso (CH.ImageIndex >= 0 AndAlso CH.ImageIndex < Me._ImageList.Images.Count)) Then
                        ImageSpacing = 16
                    Else
                        ImageSpacing = 0
                    End If

                    'DRAW IMAGE AND TEXT
                    Select Case CH.TextAlign
                        Case HorizontalAlignment.Left
                            If (ImageSpacing > 0) Then aGr.DrawImage(Me._ImageList.Images(CH.ImageIndex), ColRect.X + 4, ColRect.Top, 14, 14)
                            aGr.DrawString(TempStr, CH.Font, TextBrush, ColRect.X + 4 + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2))

                        Case HorizontalAlignment.Right
                            Dim Var1 As Integer = ColRect.Right - 3 - ImageSpacing - CType(StrSize.Width, Integer)

                            If (ImageSpacing > 0) Then aGr.DrawImage(Me._ImageList.Images(CH.ImageIndex), Var1, ColRect.Top, 14, 14)
                            aGr.DrawString(TempStr, CH.Font, TextBrush, Var1 + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2))

                        Case HorizontalAlignment.Center
                            Dim Var1 As Integer = ColRect.X + ((ColRect.Width - (ImageSpacing + CType(StrSize.Width, Integer))) \ 2)

                            If (ImageSpacing > 0) Then aGr.DrawImage(Me._ImageList.Images(CH.ImageIndex), Var1 + 2, ColRect.Top, 14, 14)
                            aGr.DrawString(TempStr, CH.Font, TextBrush, Var1 + ImageSpacing, ((ColRect.Height - StrSize.Height) / 2))
                    End Select

                    Last += CH.Width
                End If
            Next

            'RENDER THE TRAILING COLUMNHEADER ONLY IF THE END OF THE LAST COLUMN ENDS BEFORE THE BOUNDARY OF THE CONTROL
            If ((Rect.Left + Last + 2 - Me._HscrollBar.Value) > (Rect.Left + Rect.Width)) Then
                Try
                    'SET VARIABLES
                    Hdc = aGr.GetHdc
                    Theme = Helpers.VisualStyles.OpenThemeData(Me.Handle, "HEADER")
                    Rect1 = New VisualStyles.RECT(LpScr + Last, Tp, Rect.Width - Last - 2 + Me._HscrollBar.Value, Me._HdrBuffer)
                    Rect2 = New VisualStyles.RECT(Rect.Left, Tp, Rect.Width, Me._HdrBuffer)

                    'DO THE DRAWING
                    Helpers.VisualStyles.DrawThemeBackground(Theme, Hdc, 1, 1, Rect1, Rect2)
                Catch ex As Exception
                    'DO <c>NOTHING</c> FOR NOW
                Finally
                    'RELEASE THE RESOURCES
                    If (Not IntPtr.Zero.Equals(Hdc)) Then aGr.ReleaseHdc(Hdc)
                    If (Not IntPtr.Zero.Equals(Theme)) Then Helpers.VisualStyles.CloseThemeData(Theme)
                End Try
            End If
        End If
    End Sub

    Private Sub _endAnyEdits()
        If (Me._EditingObj IsNot Nothing) Then Me._EditingObj.EndEdit()
    End Sub

    Private Sub _generateHeaderRect()
        Dim Rect As Rectangle = Me.ClientRectangle

        Me._HeaderRect = New Rectangle(Rect.Left + 2 - Me._HscrollBar.Value, Rect.Top + 2, Rect.Width - 4, Me._HeaderHeight)
    End Sub

    Private Sub _generateViewableRowsRectangle()
        Dim Val As Integer
        Dim Width As Integer
        Dim ColWdths As Integer = Me.GetSumOfVisibleColumnWidths
        Dim Rect As Rectangle = Me.ClientRectangle

        If (ColWdths >= Rect.Width) Then Width = Rect.Width - 4 Else Width = ColWdths
        If (Me._HeaderStyle = ColumnHeaderStyle.None) Then Val = 2 Else Val = 2 + _HeaderHeight
        Me._RowsRect = New Rectangle(Rect.Left + 2 - Me._HscrollBar.Value, Rect.Top + Val, Width, Rect.Height - Val)
    End Sub

    Private Function _getBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        Dim Rect As Rectangle = Rectangle.Empty

        If (Me._Columns.Count > 0 AndAlso Not Me._Columns(0).Hidden) Then
            Dim Ypos As Integer = 2 + Me._HdrBuffer - Me._VscrollBar.Value

            Rect = New Rectangle(Me._Columns(0).Bounds.X, Ypos + (aObj.Index * Me._RowHeight), Me._Columns(0).Width, Me._RowHeight)
        End If

        Return Rect
    End Function

    Private Function _getCompleteBounds(ByVal aObj As ContainerListViewObject) As Rectangle
        Dim Rect As Rectangle = Rectangle.Empty
        Dim Sum As Integer = Me.GetSumOfVisibleColumnWidths + (Me._Columns.Count - 1)

        If (Sum > 0) Then
            Dim XPos As Integer = Me.ClientRectangle.X '+ 2
            Dim YPos As Integer = 2 + Me._HdrBuffer - Me._VscrollBar.Value

            Rect = New Rectangle(XPos, YPos + (aObj.Index * Me._RowHeight), Sum, Me._RowHeight)
        End If

        Return Rect
    End Function

    Private Function _getExcludedProps() As Specialized.StringCollection
        Dim ExcludedProps As New Specialized.StringCollection()

        With ExcludedProps
            'PROPERTIES WE NEVER WANT TO SERIALIZE
            .Add("bindingcontext")
            .Add("bounds")
            .Add("clientsize")
            .Add("controls")
            .Add("contextmenu")
            .Add("databindings")
            .Add("imagelist")
            .Add("left")
            .Add("location")
            .Add("margin")
            .Add("name")
            .Add("parent")
            .Add("region")
            .Add("top")
        End With

        Return ExcludedProps
    End Function

    Private Function _getItemColor(ByVal aObj As ContainerListViewObject) As Color
        Dim RetVal As Color = aObj.BackColor

        If (Not aObj.BackColor.Equals(Color.Transparent)) Then RetVal = Color.FromArgb(Me._AlphaComponent, aObj.BackColor)

        Return RetVal
    End Function

    Private Function _getItemColor(ByVal aSubItem As ContainerListViewObject.ContainerListViewSubItem) As Color
        Dim RetVal As Color = aSubItem.BackColor

        If (Not aSubItem.BackColor.Equals(Color.Transparent)) Then RetVal = Color.FromArgb(Me._AlphaComponent, aSubItem.BackColor)

        Return RetVal
    End Function

    Private Function _getSelectedRowIndex(ByVal aYpos As Integer) As Integer
        Dim SelIndex As Integer = 0
        Dim Var1 As Integer = -(2 + Me._HdrBuffer - Me._VscrollBar.Value) + aYpos
        Dim Var3 As Double = Var1 / Me.RowHeight

        SelIndex = CType(Microsoft.VisualBasic.Int(Var3), Integer)

        If (SelIndex < Me._Items.Count AndAlso SelIndex >= 0) Then Return SelIndex Else Return -1
    End Function

    Private Sub _makeSelectedVisible()
        If (Me._FocusedIndex > -1 AndAlso Me._EnsureVisible) Then
            Dim Clvi As ContainerListViewItem = Me._Items(Me._FocusedIndex)

            If (Clvi IsNot Nothing AndAlso Clvi.Focused AndAlso Clvi.Selected) Then
                Dim Rect As Rectangle = Me.ClientRectangle
                Dim Pos As Integer = Rect.Top + (Me._RowHeight * Clvi.Index) + Me._HdrBuffer + 2 - Me._VscrollBar.Value

                Try
                    If (Pos + (Me._RowHeight * 2) > Rect.Top + Rect.Height) Then
                        Me._VscrollBar.Value += Math.Abs((Rect.Top + Rect.Height) - (Pos + (Me._RowHeight * 2)))
                    ElseIf (Pos < (Rect.Top + Me._HdrBuffer)) Then
                        Me._VscrollBar.Value -= Math.Abs(Rect.Top + Me._HdrBuffer - Pos)
                    End If
                Catch ex As Exception
                    If (Me._VscrollBar.Value > Me._VscrollBar.Maximum) Then
                        Me._VscrollBar.Value = Me._VscrollBar.Maximum
                    ElseIf (Me._VscrollBar.Value < Me._VscrollBar.Minimum) Then
                        Me._VscrollBar.Value = Me._VscrollBar.Minimum
                    End If
                End Try
            End If
        End If
    End Sub

    Private Sub _moveToIndex(ByVal aIndex As Integer)
        If (aIndex < 0 OrElse aIndex >= Me._Items.Count OrElse aIndex = Me._FocusedIndex) Then Exit Sub

        Me._SelectedItems.Clear()
        Me._FocusedIndex = aIndex
        If (Me._MultiSelectMode = MultiSelectModes.Single OrElse Me._FirstSelected = -1) Then
            Me._FirstSelected = Me._FocusedIndex
            Me._Items(Me._FocusedIndex).Focused = True
            Me._FocusedItem = Me._Items(Me._FocusedIndex)
        End If

        Me._showSelectedItems()
        Me._makeSelectedVisible()
        Me.Invalidate()
    End Sub

    Private Sub _preInit()
        'MISC
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.Opaque Or ControlStyles.UserPaint Or
           ControlStyles.DoubleBuffer Or ControlStyles.Selectable Or ControlStyles.UserMouse, True)
        Me._CheckedItems = New CheckedContainerListViewObjectCollection(Me)
        Me._HiddenCols = New HiddenColumnsCollection(Me)
        Me.Size = New Size(200, 150)
        MyBase.Font = New Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular)
        MyBase.BackColor = SystemColors.Window
        MyBase.ForeColor = SystemColors.WindowText

        'SET VISUAL STYLES (THIS CAN ALWAYS BE OVERRIDEN IF NECESSARY)
        Me._VisualStyles = (Helpers.VisualStyles.IsThemeActive AndAlso Helpers.VisualStyles.IsAppThemed)

        'SET THE HSCROLL
        With Me._HscrollBar
            .Parent = Me
            .Minimum = 0
            .Maximum = 0
            .SmallChange = 10
            .Hide()
        End With

        'SET THE VSCROLL
        With Me._VscrollBar
            .Parent = Me
            .Minimum = 0
            .Maximum = 0
            .SmallChange = Me._RowHeight
            .Hide()
        End With

        'INITIALIZE THE BRUSHES AND PEN
        Me._Brush_ColTracking = New SolidBrush(Color.FromArgb(Me._AlphaComponent, Me._ColTrackColor))
        Me._GridLinePen = New Pen(Me.GridLineColor, 1.0F)

        'ATTACH ANY HANDLERS AND CREATE THE HEADER
        Me._attachHandlers()
        Me._generateHeaderRect()
    End Sub

    Private Sub _readXml(ByVal aReader As XmlReader, ByVal aObject As Object, ByVal aExitName As String)
        Dim ExcProps As Specialized.StringCollection = Nothing
        Dim Prop As PropertyInfo = Nothing
        Dim PropName As String = String.Empty
        Dim PropVal As Object = Nothing
        Dim NewVal As Object = Nothing

        While aReader.Read()
            If (aReader.NodeType = XmlNodeType.EndElement AndAlso aExitName.Equals(aReader.Name)) Then
                Exit Sub
            ElseIf (aReader.NodeType = XmlNodeType.Element) Then
                'SET VARS
                PropName = aReader.Name

                'GET THE SERIALIZATION TYPE
                If (PropName.ToLower.Equals("treelistview") OrElse PropName.ToLower.Equals("containerlistview")) Then
                    ExcProps = Me._getExcludedProps()
                Else
                    Prop = aObject.GetType.GetProperty(PropName)

                    If (Prop IsNot Nothing AndAlso (ExcProps Is Nothing OrElse Not ExcProps.Contains(PropName.ToLower()))) Then
                        PropVal = Prop.GetValue(aObject, Nothing)

                        'PROCESS COLLECTIONS DIFFERENTLY
                        If (TypeOf PropVal Is ICollection) Then
                            Dim AddMethod As MethodInfo = Nothing
                            Dim Count As Integer = CType(aReader.Item(1), Integer)

                            For I As Integer = 0 To (Count - 1)
                                'SINCE ICOLLECTION HAS A PROPERTY NAMED "ITEM", I CAN DERIVE THE COLLECTION CONTENTS FROM THAT
                                'AND CREATE MY INSTANCE
                                NewVal = Activator.CreateInstance(PropVal.GetType.GetProperty("Item").PropertyType)

                                If (NewVal IsNot Nothing) Then
                                    'CREATE THE ADD METHOD IF WE HAVEN'T ALREADY
                                    If (AddMethod Is Nothing) Then
                                        Dim AddType(0) As Type

                                        AddType(0) = NewVal.GetType()
                                        AddMethod = PropVal.GetType.GetMethod("Add", AddType)
                                    End If

                                    'READ
                                    AddMethod.Invoke(PropVal, New Object() {NewVal})
                                    aReader.Read()
                                    aReader.Read()
                                    Me._readXml(aReader, NewVal, aReader.Name)
                                End If
                            Next
                        Else
                            Me._setValue(aReader, aObject, Prop, PropVal)
                        End If
                    End If
                End If
            End If
        End While
    End Sub

    Private Sub _renderItemRows(ByVal aItem As ContainerListViewItem, ByVal aGr As Graphics, ByVal Rect As Rectangle, ByVal aTotalRend As Integer)
        Dim TempVar1 As Integer = Rect.Top + (Me.RowHeight * aTotalRend)

        'ONLY RENDER ROW IF VISIBLE TO USER
        If ((TempVar1 + Me.RowHeight) >= (Rect.Top + 2)) AndAlso (TempVar1 < (Rect.Top + Rect.Height)) Then
            Dim ChkBoxX As Integer = (Rect.Left + 2) - Me._HscrollBar.Value 'LEFT VIEWPORT POSITION LESS SCROLLBAR POS
            Dim aBrush As SolidBrush = New SolidBrush(aItem.ForeColor)
            Dim CheckBuffer, IconBuffer, StrWidth, HScrollVisible As Integer
            Dim aItemRect As Rectangle = Me.GetCompleteItemBounds(aItem)
            Dim ItemRect As Rectangle = Me.GetItemBounds(aItem)
            Dim TempStr As String
            Dim SelRowRect As Rectangle
            Dim SBrush As New SolidBrush(Me._determineBackGroundColor(aItem))

            'MISC
            If (Me._AllowCheckBoxes AndAlso aItem.CheckBoxVisible) Then CheckBuffer = 18 Else CheckBuffer = 0
            If (Me._ImageList IsNot Nothing) Then
                If (aItem.ImageIndex >= 0 AndAlso aItem.ImageIndex < Me._ImageList.Images.Count) Then IconBuffer = 18 Else IconBuffer = 0
            End If
            If (Me._HscrollBar.Visible) Then HScrollVisible = Me._HscrollBar.Height Else HScrollVisible = 0

            'MISC 
            aGr.Clip = New Region(aItemRect)
            TempStr = Tools.TruncateString(aItem.Text, aItem.Font, ItemRect.Width, 16, aGr)
            StrWidth = CType(Helpers.Tools.MeasureDisplayString(aGr, TempStr, aItem.Font).Width, Integer)
            If (Not Me._FullRowSelect) Then
                SelRowRect = ItemRect
            ElseIf (aItemRect.Width < (Rect.Width - 4) OrElse Me._HscrollBar.Visible) Then
                SelRowRect = aItemRect
            Else
                SelRowRect = New Rectangle(aItemRect.X, aItemRect.Y, Rect.Width - 4, aItemRect.Height)
            End If

            'RENDER ITEM BACKGROUND
            aGr.FillRectangle(SBrush, ItemRect)

            'RENDER SELECTED ITEM
            If (Me._FullRowSelect AndAlso (aItem.Selected OrElse aItem.Hovered)) Then aGr.FillRectangle(SBrush, SelRowRect)

            'SET THE CLIP
            aGr.Clip = New Region(New Rectangle(aItemRect.X, aItemRect.Y, ItemRect.Width - 1, aItemRect.Height))

            'DRAW THE CHECKBOX IF NECESSARY
            If (CheckBuffer <> 0) Then
                Dim CkBxRect As Rectangle

                'DETERMINE THE CHECKBOX RECTANGLE
                Select Case aItem.TextAlign
                    Case HorizontalAlignment.Left
                        CkBxRect = New Rectangle(ChkBoxX + 1, aItemRect.Y, 16, aItemRect.Height - 1)

                    Case HorizontalAlignment.Right
                        CkBxRect = New Rectangle(ChkBoxX + (ItemRect.Width - 1) - StrWidth - CheckBuffer - IconBuffer - 3, aItemRect.Y, 16, aItemRect.Height - 1)

                    Case HorizontalAlignment.Center
                        CkBxRect = New Rectangle(ChkBoxX + ((ItemRect.Width - 1 + CheckBuffer + IconBuffer) \ 2) - (StrWidth \ 2) - IconBuffer - CheckBuffer, aItemRect.Y, 16, aItemRect.Height - 1)
                End Select

                'DRAW THE CHECKBOX AND ADD IT TO THE HASHTABLE
                Me.DrawObjectCheckBox(aGr, CkBxRect, aItem)
                Me._CheckBoxRects.Add(CkBxRect, aItem)
            End If

            'DRAW THE ICON IF AVAILABLE
            If (IconBuffer <> 0) Then
                Select Case aItem.TextAlign
                    Case HorizontalAlignment.Left
                        Dim TmpInt As Integer = 0

                        If (aItem.CheckBoxVisible) Then TmpInt = 1 Else TmpInt = 2
                        aGr.DrawImage(Me._ImageList.Images(aItem.ImageIndex), ChkBoxX + TmpInt + CheckBuffer, aItemRect.Y, 16, aItemRect.Height - 1)

                    Case HorizontalAlignment.Right
                        aGr.DrawImage(Me._ImageList.Images(aItem.ImageIndex), ChkBoxX + (ItemRect.Width - 1) - StrWidth - IconBuffer - 3, aItemRect.Y, 16, aItemRect.Height - 1)

                    Case HorizontalAlignment.Center
                        aGr.DrawImage(Me._ImageList.Images(aItem.ImageIndex), ChkBoxX + ((ItemRect.Width - 1 + CheckBuffer + IconBuffer) \ 2) - (StrWidth \ 2) - IconBuffer, aItemRect.Y, 16, aItemRect.Height - 1)
                End Select
            End If

            'RENDER THE TEXT
            Select Case aItem.TextAlign
                Case HorizontalAlignment.Left
                    aGr.DrawString(TempStr, aItem.Font, aBrush, ChkBoxX + CheckBuffer + IconBuffer, aItemRect.Y)

                Case HorizontalAlignment.Right
                    aGr.DrawString(TempStr, aItem.Font, aBrush, ChkBoxX + (ItemRect.Width - 1) - StrWidth - 3, aItemRect.Y)

                Case HorizontalAlignment.Center
                    aGr.DrawString(TempStr, aItem.Font, aBrush, ChkBoxX + ((ItemRect.Width - 1 + CheckBuffer + IconBuffer) \ 2) - (StrWidth \ 2), aItemRect.Y)
            End Select

            'RENDER SUBITEMS
            If (Me._Columns.Count > 0) Then
                Dim K As Integer = 0

                While (K < aItem.SubItems.Count AndAlso K < (Me._Columns.Count - 1))
                    Dim aCol As ContainerColumnHeader = Me._Columns(K)
                    Dim SubItem As ContainerListViewObject.ContainerListViewSubItem = aItem.SubItems(K)
                    Dim SubItemRect As Rectangle = SubItem.Bounds
                    Dim SubItemBrush As SolidBrush = New SolidBrush(Me._determineBackGroundColor(aItem, SubItem))
                    Dim SubItemAlign As HorizontalAlignment
                    Dim Offset As Integer = 0

                    'MISC
                    If (Me._GridLineType = ContainerListView.GridLineSelections.Both OrElse Me._GridLineType = ContainerListView.GridLineSelections.Row) Then Offset = 1

                    'SET THE CLIP
                    aGr.Clip = New Region(SubItemRect)

                    'DRAW THE BACKGROUND
                    aGr.FillRectangle(SubItemBrush, SubItemRect.X, SubItemRect.Y, SubItemRect.Width, SubItemRect.Height - Offset)

                    'ONLY RENDER SUBITEM IF VISIBLE
                    aGr.Clip = New Region(SubItemRect)
                    If (SubItem.Control IsNot Nothing) Then
                        With SubItem.Control
                            .Location = New Point(ChkBoxX + SubItemRect.X + 2, SubItemRect.Y + 2)
                            .ClientSize = New Size(SubItemRect.Width - 5, SubItemRect.Height - 3)
                            .Parent = Me
                            .Visible = True
                            .BringToFront()
                            .Refresh()
                        End With
                    Else
                        Dim DispWidth As Integer
                        Dim SubItemFont As Font

                        If (aItem.UseItemStyleForSubItems) Then
                            SubItemBrush = aBrush
                            SubItemFont = aItem.Font
                            SubItemAlign = aItem.TextAlign
                        Else
                            SubItemBrush = New SolidBrush(SubItem.ForeColor)
                            SubItemFont = SubItem.Font
                            SubItemAlign = SubItem.TextAlign
                        End If
                        TempStr = Tools.TruncateString(SubItem.Text, SubItemFont, SubItemRect.Width, 12, aGr)
                        DispWidth = CType(Helpers.Tools.MeasureDisplayString(aGr, TempStr, SubItemFont).Width, Integer)

                        Select Case SubItemAlign
                            Case HorizontalAlignment.Left
                                Dim TmpInt As Integer = 0

                                If (SubItemRect.X <= 0) Then TmpInt = 2
                                aGr.DrawString(TempStr, SubItemFont, SubItemBrush, SubItemRect.X + 1 + TmpInt, SubItemRect.Y)

                            Case HorizontalAlignment.Right
                                Dim OS As Integer = 0

                                If (SubItemFont.Bold) Then OS = 4 Else OS = 2
                                aGr.DrawString(TempStr, SubItemFont, SubItemBrush, SubItemRect.X + SubItemRect.Width - DispWidth - OS, SubItemRect.Y)

                            Case HorizontalAlignment.Center
                                aGr.DrawString(TempStr, SubItemFont, SubItemBrush, CType(SubItemRect.X + (SubItemRect.Width / 2) - (DispWidth / 2), Single), SubItemRect.Y)
                        End Select
                    End If

                    K += 1
                End While
            End If

            'SET THE NEW CLIP
            aGr.Clip = New Region(aItemRect)

            'RENDER THE ROW GRIDLINES
            If (Me._GridLineType = ContainerListView.GridLineSelections.Both OrElse Me._GridLineType = ContainerListView.GridLineSelections.Row) Then
                aGr.DrawLine(Me._GridLinePen, aItemRect.Left + 2, aItemRect.Bottom - 1, aItemRect.Right + 1, aItemRect.Bottom - 1)
            End If

            'DRAW THE FOCUS RECTANGLE
            If (Me._SelectedItems.Count > 1 AndAlso Me._IsFocused AndAlso aItem.Focused) Then
                Dim NewRect As Rectangle = New Rectangle(SelRowRect.Left + 2, SelRowRect.Y, SelRowRect.Width - 3, SelRowRect.Height)
                ControlPaint.DrawFocusRectangle(aGr, NewRect, aItem.ForeColor, aItem.BackColor)
            End If
        End If
    End Sub

    ''' <summary>
    ''' This is a special case and will be used when the user clicks on an item
    ''' while the control key is pressed or presses the space bar button while an
    ''' item has the focus.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _selectiveSelection(ByVal aIndex As Integer)
        Dim Clvi As ContainerListViewItem = Me._Items(aIndex)

        'UNFOCUS THE PREVIOUSLY FOCUSED ITEM
        If (Me._FocusedIndex >= 0 AndAlso Me._FocusedIndex < Me._Items.Count) Then Me._Items(Me._FocusedIndex).Focused = False

        If (Clvi.Selected) Then
            Clvi.Focused = False
            Clvi.Selected = False
            If (Me._FocusedItem Is Clvi) Then Me._FocusedItem = Nothing
        Else
            Clvi.Focused = True
            Clvi.Selected = True
        End If

        Me._makeSelectedVisible()
        Me.Invalidate()
    End Sub

    Private Sub _setValue(ByVal aReader As XmlReader, ByVal aObject As Object, ByVal aProp As PropertyInfo, ByVal aPropVal As Object)
        Dim NewVal As Object = Nothing
        Dim ParsMeth As MethodInfo = Nothing
        Dim SplitArr() As String = Nothing

        ParsMeth = aProp.PropertyType.GetMethod("Parse", New Type() {GetType(String)})

        If (ParsMeth IsNot Nothing) Then
            NewVal = ParsMeth.Invoke(aPropVal, New Object() {aReader.ReadString()})
        Else
            'FILTER THE SAME TYPES WE DID IN THE WRITEXML
            If (TypeOf aPropVal Is [Enum]) Then
                NewVal = [Enum].Parse(aProp.PropertyType, aReader.ReadString(), True)
            ElseIf (TypeOf aPropVal Is Color) Then
                NewVal = Color.FromArgb(CType(aReader.ReadString, Integer))
            ElseIf (TypeOf aPropVal Is Font) Then
                Dim FontData() As String = aReader.ReadString().Split(New String() {"|"}, StringSplitOptions.None)

                NewVal = New Font(FontData(0), CType(FontData(1), Single), CType(CType(FontData(2), Integer), FontStyle))
            ElseIf (TypeOf aPropVal Is Size) Then
                NewVal = Size.Empty
                SplitArr = aReader.ReadString.Split(Char.Parse(","))

                If (SplitArr.Length = 2) Then NewVal = New Size(CType(SplitArr(0), Integer), CType(SplitArr(1), Integer))
            ElseIf (TypeOf aPropVal Is Padding) Then
                NewVal = Padding.Empty
                SplitArr = aReader.ReadString.Split(Char.Parse(","))

                If (SplitArr.Length = 4) Then
                    NewVal = New Padding(CType(SplitArr(0), Integer), CType(SplitArr(1), Integer), CType(SplitArr(2), Integer), CType(SplitArr(3), Integer))
                End If
            ElseIf (TypeOf aPropVal Is Cursor) Then
                NewVal = New Cursor(New IntPtr(CType(aReader.ReadString(), Integer)))
            Else
                NewVal = aReader.ReadString()
            End If
        End If

        'SET THE VALUE
        If (NewVal IsNot Nothing) Then
            Try
                aProp.SetValue(aObject, NewVal, Nothing)
            Catch ex As Exception
                'DO NOTHING
                Debug.WriteLine("Method _setValue cannot set property " & aProp.Name)
            End Try
        End If
    End Sub

    Private Sub _showSelectedItems()
        If (Me._FirstSelected = Me._FocusedIndex) Then
            Me._Items(Me._FirstSelected).Selected = True
        ElseIf (Me._FirstSelected > Me._FocusedIndex) Then
            For I As Integer = Me._FocusedIndex To Me._FirstSelected
                Me._Items(I).Selected = True
            Next
        ElseIf (Me._FirstSelected < Me._FocusedIndex) Then
            For I As Integer = Me._FirstSelected To Me._FocusedIndex
                Me._Items(I).Selected = True
            Next
        End If
    End Sub

    Private Sub _writeXml(ByVal aObject As Object, ByVal aWriter As XmlWriter)
        Dim Atts() As Object = Nothing
        Dim DefVal As DefaultValueAttribute
        Dim ExcludedProps As Specialized.StringCollection
        Dim MyProps() As PropertyInfo
        Dim ObjTyp As Type = aObject.GetType()
        Dim PropVal As Object
        Dim TmpFont01 As Font
        Dim TmpFont02 As Font
        Dim Val As String = String.Empty
        Dim WriteProp As Boolean = False

        'GET THE PROPERTIES AND SORT
        ExcludedProps = Me._getExcludedProps()
        MyProps = aObject.GetType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
        Array.Sort(MyProps, New PropInfoViewComparer(SortOrder.Ascending))

        'START WRITING
        With aWriter
            .WriteStartElement(ObjTyp.Name, ObjTyp.Namespace)

            'GET ALL THE PROPERTY VALUES
            For Each PI As PropertyInfo In MyProps
                WriteProp = False
                Atts = PI.GetCustomAttributes(GetType(XmlIgnoreAttribute), False)

                'ONLY CONTINUE IF THE XMLIGNORE ATTRIBUTE IS NOT PRESENT
                If (Atts.Length = 0) Then
                    If (Not ExcludedProps.Contains(PI.Name.ToLower())) Then
                        PropVal = PI.GetValue(aObject, Nothing)

                        If (TypeOf PropVal Is ICollection) Then
                            Dim IE As IEnumerator
                            Dim IEnum As IEnumerable = DirectCast(PropVal, IEnumerable)

                            .WriteStartElement(PI.Name)
                            .WriteAttributeString("type", PI.PropertyType.Name)
                            .WriteAttributeString("count", DirectCast(PropVal, ICollection).Count.ToString())

                            IE = IEnum.GetEnumerator()
                            While IE.MoveNext
                                Me._writeXml(IE.Current, aWriter)
                            End While
                            .WriteEndElement()
                        ElseIf (PI.CanWrite AndAlso Not PI.PropertyType.IsInterface) Then
                            Atts = PI.GetCustomAttributes(GetType(DefaultValueAttribute), False)

                            If (Atts.Length > 0) Then
                                DefVal = DirectCast(Atts(0), DefaultValueAttribute)

                                If (Not (DefVal.Value Is Nothing And PropVal Is Nothing)) Then
                                    'IF ONE OR THE OTHER IS NOTHING THEN WRITE BECAUSE THEY ARE DIFFERENT
                                    WriteProp = DefVal.Value Is Nothing OrElse PropVal Is Nothing

                                    If (Not WriteProp) Then
                                        'FILTER CERTAIN TYPE(S)
                                        If (TypeOf DefVal.Value Is Font) Then
                                            TmpFont01 = DirectCast(DefVal.Value, Font)
                                            TmpFont02 = DirectCast(PropVal, Font)

                                            WriteProp = Not (TmpFont01.Name.Equals(TmpFont02.Name) AndAlso _
                                                             TmpFont01.Size.Equals(TmpFont02.Size) AndAlso _
                                                             TmpFont01.Style.Equals(TmpFont02.Style))
                                        Else
                                            WriteProp = (Not DefVal.Value.Equals(PropVal))
                                        End If
                                    End If
                                End If
                            Else
                                WriteProp = True
                            End If

                            'WRITE THE PROPERTY VALUE
                            If (WriteProp) Then
                                If (PropVal Is Nothing) Then Val = String.Empty Else Val = PropVal.ToString

                                'FILTER CERTAIN TYPES
                                If (TypeOf PropVal Is Color) Then
                                    Val = DirectCast(PropVal, Color).ToArgb.ToString()
                                ElseIf (TypeOf PropVal Is Font) Then
                                    TmpFont01 = DirectCast(PropVal, Font)
                                    Val = TmpFont01.FontFamily.Name & "|" & TmpFont01.Size.ToString() & "|" & CType(TmpFont01.Style, Integer).ToString()
                                ElseIf (TypeOf PropVal Is [Enum]) Then
                                    Val = DirectCast(PropVal, Integer).ToString()
                                ElseIf (TypeOf PropVal Is Control) Then
                                    Val = DirectCast(PropVal, Control).GetType.ToString()
                                ElseIf (TypeOf PropVal Is Cursor) Then
                                    Val = DirectCast(PropVal, Cursor).Handle.ToString()
                                ElseIf (TypeOf PropVal Is Size) Then
                                    Dim TmpSize As Size = DirectCast(PropVal, Size)

                                    Val = TmpSize.Width.ToString() & "," & TmpSize.Height.ToString()
                                ElseIf (TypeOf PropVal Is Padding) Then
                                    Dim TmpPad As Padding = DirectCast(PropVal, Padding)

                                    Val = TmpPad.Left.ToString() & "," & TmpPad.Top.ToString() & "," & TmpPad.Right.ToString() & "," & TmpPad.Bottom.ToString()
                                End If

                                'WRITE THE ELEMENT
                                .WriteElementString(PI.Name, Val)
                            End If
                        End If
                    End If
                End If
            Next

            .WriteEndElement()
        End With
    End Sub

#End Region

#Region " Disabled Code "

    'Protected Overridable Sub OnSubControlMouseDown(ByVal sender As Object, ByVal aMouseArg As MouseEventArgs)
    '   Dim TempClvi As ContainerListViewItem
    '   Dim Clvi As ContainerListViewItem = DirectCast(Sender, ContainerListViewItem)

    '   If (Me.MultiSelectMode = MultiSelectModes.Single) Then
    '      Me.SelectedItems.Clear()

    '      For I As Integer = 0 To (Me._Items.Count - 1)
    '         TempClvi = Me._Items(I)

    '         With TempClvi
    '            .Focused = False
    '            .Selected = False

    '            If (.Equals(Clvi)) Then
    '               .Focused = True
    '               .Selected = True
    '            End If
    '         End With

    '         Me._FocusedIndex = I
    '      Next
    '   ElseIf (Me.MultiSelectMode = MultiSelectModes.Range) Then
    '      'TODO:  <c>NOTHING</c> FOR NOW.  IMPLEMENT LATER
    '   ElseIf (Me.MultiSelectMode = MultiSelectModes.Selective) Then
    '      For I As Integer = 0 To (Me._Items.Count - 1)
    '         Me._Items(I).Focused = False
    '      Next

    '      If (Clvi.Selected) Then
    '         With Clvi
    '            .Focused = False
    '            .Selected = False
    '         End With
    '      Else
    '         With Clvi
    '            .Focused = True
    '            .Selected = True
    '         End With
    '      End If
    '      Me._FocusedIndex = Me._Items.IndexOf(Clvi)
    '   End If

    '   Me.Invalidate()
    'End Sub

#End Region

End Class
