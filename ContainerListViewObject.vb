Imports System.Runtime.Serialization

''' <summary>
''' ContainerListViewObject Class.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class ContainerListViewObject
    Implements ICloneable

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New instance of ContainerListViewObject.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub New()
        Me._preInit()
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerListViewObject.
    ''' </summary>
    ''' <param name="aItemText">The text to display for the item.</param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal aItemText As String)
        Me._Text = aItemText
        Me._preInit()
    End Sub

#End Region

#Region " Field Declarations "
    Private _AllowSelect As Boolean = True
    Private _BckClr As Color = Color.Transparent
    Private _EditBox As RichTextBox
    Private _EditedItem As ContainerListViewSubItem
    Private _EditedRect As Rectangle
    Private _Focused As Boolean
    Private _Font As Font = Control.DefaultFont
    Private _FontChangedByBase As Boolean = True
    Private _ForeClr As Color = SystemColors.WindowText
    Private _ForeClrChangedByBase As Boolean = True
    Private _Hovered As Boolean
    Private _ImgIndex As Integer = -1
    Private _IsChecked As Boolean
    Private _IsCheckEnabled As Boolean = True
    Private _IsCheckVisible As Boolean = True
    Private _SubItems As New ContainerListViewSubItemCollection(Me)
    Private _Tag As Object
    Private _Text As String = String.Empty
    Private _TextAlgn As HorizontalAlignment = HorizontalAlignment.Left
    Private _UseItemStyle As Boolean = True

    ''' <summary>
    ''' The ContainerListView that owns the ContainerListViewObject.
    ''' </summary>
    ''' <remarks></remarks>
    Protected _LstView As ContainerListView
#End Region

#Region " Events "

#End Region

#Region " MustOverrides "

    ''' <summary>
    ''' Creates a new Object that is a copy of the current instance.
    ''' </summary>
    ''' <returns>An object that is a Clone of the current instance.</returns>
    ''' <remarks></remarks>
    Public MustOverride Function Clone() As Object Implements System.ICloneable.Clone

    ''' <summary>
    ''' Gets a value that determines if the ContainerListViewObject has a parent.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ContainerListViewObject" /> has a parent; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    Public MustOverride ReadOnly Property HasParent() As Boolean

    ''' <summary>
    ''' Gets the zero-based position of the ContainerListViewObject within the collection.
    ''' </summary>
    ''' <value>
    ''' The zero-based index of the item within Collection. If the item is not associated with a collection control, this property returns -1.
    ''' </value>
    ''' <remarks></remarks>
    Public MustOverride ReadOnly Property Index() As Integer

    ''' <summary>
    ''' Gets or Sets a value that determines if the ContainerListViewObject is in a selected state.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ContainerListViewObject" /> is selected; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <DefaultValue(False)> _
    Public MustOverride Property Selected() As Boolean

    ''' <summary>
    ''' Assigns the ListView that the Item belongs to.
    ''' </summary>
    ''' <param name="aLView">The ContainerListView" /> that owns the item.</param>
    ''' <remarks>This code is for internal use only and is not intended to be called from your code.  Calling this method externally may have an adverse effect on code that uses this class.</remarks>
    Friend MustOverride Sub SetParent(ByVal aLView As ContainerListView)

#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or Sets a value that Determines whether the ContainerListViewObject can be selected.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ContainerListViewObject can be selected; otherwise <c>FALSE</c>.  Default is <c>TRUE</c>.</value>
    ''' <remarks>
    ''' In the case of TreeListNodes, AllowSelection is convenient if you want a user to be able to expand and collapse TreeListNodes 
    ''' but not allow them to be selected (thereby not being added to the SelectedItems/SelectedIndexes collections).  For example, 
    ''' if you have a parent node that represents a collection and the childnodes of that parentnode represent the items of that 
    ''' collection, you can allow the user to select as many childnodes (or not) as they wish. So if there is processing on the 
    ''' SelectedItems collection you can be guaranteed that the parentnode will not be part of that collection.
    ''' AllowSelection may not hold such importance on ContainerListViewItems but it is supported if needed.
    ''' </remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether the ContainerListViewObject can be selected.")> _
    Public Property AllowSelection() As Boolean
        Get
            Return Me._AllowSelect
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._AllowSelect.Equals(Value)) Then
                If (Not Value) Then Me.Selected = False
                Me._AllowSelect = Value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the BackColor of the Item.
    ''' </summary>
    ''' <value>A System.Drawing.Color value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "Transparent"), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     Description("Determines the BackColor of the Item.")> _
    Public Overridable Property BackColor() As Color
        Get
            Return Me._BckClr
        End Get
        Set(ByVal Value As Color)
            If (Not Me._BckClr.Equals(Value)) Then
                Me._BckClr = Value
                If (Me._LstView IsNot Nothing) Then Me._LstView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the bounding rectangle of just the ContainerListViewObject.
    ''' </summary>
    ''' <value>A Rectangle representing the bounding rectangle; otherwise <c>NOTHING</c> if the Item is not associated to a ContainerListView.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Bounds() As Rectangle
        Get
            Dim Rect As Rectangle

            If (Me._LstView IsNot Nothing) Then Rect = Me._LstView.GetItemBounds(Me)

            Return Rect
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the checkbox is enabled or disabled.
    ''' </summary>
    ''' <value><c>TRUE</c> if the CheckBox is enabled; otherwise <c>FALSE</c>.  The default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines if the checkbox is enabled or disabled.")> _
     Public Overridable Property CheckBoxEnabled() As Boolean
        Get
            Return Me._IsCheckEnabled
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._IsCheckEnabled.Equals(Value)) Then
                Me._IsCheckEnabled = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines whether the CheckBox is visible for the ContainerListViewObject.
    ''' </summary>
    ''' <value><c>TRUE</c> if the CheckBox is visible for the ContainerListViewObject; otherwise <c>FALSE</c>.</value>
    ''' <remarks>If CheckBoxes=<c>FALSE</c> on the ContainerListView then setting this property has no effect.</remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether the CheckBox is visible for the ContainerListViewObject.")> _
    Public Overridable Property CheckBoxVisible() As Boolean
        Get
            Return Me._IsCheckVisible
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._IsCheckVisible.Equals(Value)) Then
                Me._IsCheckVisible = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the item is checked.
    ''' </summary>
    ''' <value><c>TRUE</c> if the item is checked; otherwise, <c>FALSE</c>. The default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <DefaultValue(False), _
     Category("Appearance"), _
     Description("Indicates whether the Item is checked.")> _
    Public Property Checked() As Boolean
        Get
            Return Me._IsChecked
        End Get
        Set(ByVal Value As Boolean)
            If (Me._IsCheckEnabled AndAlso Not Me._IsChecked.Equals(Value)) Then
                Dim Cancel As Boolean = False
                Dim InvalidView As Boolean = Me.ListView Is Nothing

                'MAKE THE CALL TO THE EVENT FIRST BEFORE CONTINUING
                If (Not InvalidView) Then Cancel = Me.ListView.ContainerListViewBeforeCheckStateChanged(Me)

                'IF IT WAS NOT CANCELLED, PROCESS ACCORDINGLY
                If (Not Cancel) Then
                    If (Not Me._IsChecked) Then
                        Me._IsChecked = True
                        If (Not InvalidView) Then Me.ListView.CheckedItems.Add(Me)
                    Else
                        If (Not InvalidView) Then Me.ListView.CheckedItems.Remove(Me)
                        Me._IsChecked = False
                    End If

                    Me.ListView.ContainerListViewAfterCheckStateChanged(Me)
                    If (Not InvalidView) Then Me.ListView.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the bounding rectangle of the ContainerListViewObject, including it's SubItems.
    ''' </summary>
    ''' <value>A Rectangle representing the bounding rectangle of the ContainerListViewObject and all it's subitems; otherwise <c>NOTHING</c> if the Item is not associated to a ContainerListView.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property CompleteBounds() As Rectangle
        Get
            If (Me.ListView IsNot Nothing) Then Return Me.ListView.GetCompleteItemBounds(Me)

            Return Rectangle.Empty
        End Get
    End Property

    ''' <summary>
    ''' Gets the ContainerListViewSubItem being edited.
    ''' </summary>
    ''' <value>A ContainerListViewSubItem.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property EditedSubItem() As ContainerListViewSubItem
        Get
            Return Me._EditedItem
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the item has focus within the list view control.
    ''' </summary>
    ''' <value><c>TRUE</c> if the item has focus; otherwise, <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     EditorBrowsable(EditorBrowsableState.Never), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Friend Property Focused() As Boolean
        Get
            Return Me._Focused
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._Focused.Equals(Value)) Then
                Me._Focused = Value
                If (Me.ListView IsNot Nothing AndAlso Me._Focused) Then Me.ListView.SetFocusedObject(Me)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the font of the text displayed by the item.
    ''' </summary>
    ''' <value>
    ''' The Font to apply to the text displayed by the control. The default is the value of the DefaultFont property (from Control) if the ContainerListViewObject is not associated with a ContainerListView control; otherwise the font specified in the Font property for the ContainerListView control is used.
    ''' </value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     Localizable(True), _
     DefaultValue(GetType(Font), "System.Drawing.SystemFonts.DefaultFont"), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     Description("Determines the font of the text displayed by the item.")> _
    Public Overridable Property Font() As Font
        Get
            Return Me._Font
        End Get
        Set(ByVal Value As Font)
            If (Not Me._Font.Equals(Value)) Then
                Me._FontChangedByBase = False
                Me._Font = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the foreground color of the item's text.
    ''' </summary>
    ''' <value>A Color that represents the foreground color of the item's text.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(GetType(Color), "WindowText"), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     Description("Determines the ForeColor of the the Item's text.")> _
    Public Overridable Property ForeColor() As Color
        Get
            Return Me._ForeClr
        End Get
        Set(ByVal Value As Color)
            If (Not Me._ForeClr.Equals(Value)) Then
                Me._ForeClrChangedByBase = False
                Me._ForeClr = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets a value that determines if the ContainerListViewObject's BackColor is the default.
    ''' </summary>
    ''' <value><c>TRUE</c> if the BackColor is the default value; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property HasDefaultBackColor() As Boolean
        Get
            Return (Me._BckClr.Equals(Color.Transparent))
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if the ContainerListViewObject's Font is the default.
    ''' </summary>
    ''' <value><c>TRUE</c> if the Font is the default value; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property HasDefaultFont() As Boolean
        Get
            Return (Me._Font.Equals(Control.DefaultFont))
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if the ContainerListViewObject's ForeColor is the default.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ForeColor is the default value; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property HasDefaultForeColor() As Boolean
        Get
            Return (Me._ForeClr.Equals(SystemColors.WindowText))
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets that determines if the ContainerListViewObject is currently hovered by the Mouse pointer.
    ''' </summary>
    ''' <value><c>TRUE</c> if Hovered; otherwise <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DefaultValue(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Protected Friend Property Hovered() As Boolean
        Get
            Return Me._Hovered
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._Hovered.Equals(Value)) Then Me._Hovered = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the index of the image that is displayed for the item.
    ''' </summary>
    ''' <value>The zero-based index of the image in the ImageList that is displayed for the item. The default is -1.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     DefaultValue(-1), _
     Localizable(True), _
     TypeConverter(GetType(System.Windows.Forms.ImageIndexConverter)), _
     Description("The Index of the image that is displayed for the item."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property ImageIndex() As Integer
        Get
            Return Me._ImgIndex
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._ImgIndex.Equals(Value)) Then
                Me._ImgIndex = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the ImageList associated with the ContainerListViewObject.
    ''' </summary>
    ''' <value>An ImageList; otherwise <c>NOTHING</c> if the Item is not associated to a ContainerListView.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property ImageList() As ImageList
        Get
            If (Me.ListView IsNot Nothing) Then Return Me.ListView.ImageList

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets a value that determines if the ContainerListViewObject is currently being edited by the user.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ContainerListViewItem is in an edit state; otherwise <c>FALSE</c>.</value>
    ''' <remarks>This does not include any SubItems that may be in an editing state.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property IsEditing() As Boolean
        Get
            If (Me._LstView IsNot Nothing AndAlso Me._LstView.EditedObject IsNot Nothing) Then
                If (Me._LstView.EditedObject.EditedSubItem Is Nothing) Then Return (Me._LstView.EditedObject Is Me)
            End If

            Return False
        End Get
    End Property

    ''' <summary>
    ''' Gets the ContainerListView control that contains the item.
    ''' </summary>
    ''' <value>A ContainerListView that contains the ContainerListViewObject.</value>
    ''' <remarks>You can use this property to access the ContainerListView control that owns the ContainerListViewObject.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property ListView() As ContainerListView
        Get
            Return Me._LstView
        End Get
    End Property

    ''' <summary>
    ''' Gets the SubItems of the current Item.
    ''' </summary>
    ''' <value>A ContainerListViewSubItem collection.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Description("The SubItems of the ContainerListViewObject."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     Editor(GetType(CollectionEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public ReadOnly Property SubItems() As ContainerListViewSubItemCollection
        Get
            Return _SubItems
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets an object that contains data to associate with the item.
    ''' </summary>
    ''' <value>An object that contains information that is associated with the item.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Bindable(True), _
     Localizable(False), _
     DefaultValue(GetType(Object), Nothing), _
     Description("Data to associate with the item"), _
     TypeConverter(GetType(System.ComponentModel.StringConverter))> _
    Public Property Tag() As Object
        Get
            Return Me._Tag
        End Get
        Set(ByVal Value As Object)
            Me._Tag = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the text of the item.
    ''' </summary>
    ''' <value>The text to display for the item.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     Description("The Text of the Item."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Property Text() As String
        Get
            Return Me._Text
        End Get
        Set(ByVal Value As String)
            If (Not Me._Text.Equals(Value)) Then
                Me._Text = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Alignment of the text within the ContainerListViewObject.
    ''' </summary>
    ''' <value>A HorizontalAlignment enumeration.</value>
    ''' <remarks>
    ''' Note:  Setting this property on a TreeListNode has no effect on the TextAlignment of the text on the Node itself.  
    ''' It will only be used for the Node's subitems instead.
    ''' </remarks>
    <Category("Appearance"), _
     Localizable(True), _
     DefaultValue(GetType(HorizontalAlignment), "Left"), _
     Description("The Alignment of the text within the ContainerListViewObject.")> _
    Public Property TextAlign() As HorizontalAlignment
        Get
            Return Me._TextAlgn
        End Get
        Set(ByVal Value As HorizontalAlignment)
            If (Not Me._TextAlgn.Equals(Value)) Then
                Me._TextAlgn = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the TextAlign, Font, ForeColor, and BackColor properties for the item are used for all its subitems.
    ''' </summary>
    ''' <value>
    ''' <c>TRUE</c> if all subitems use the textalign, font, foreground color, and background color settings of the item; 
    ''' otherwise, <c>FALSE</c>. The default is <c>TRUE</c>.</value>
    ''' <remarks>
    ''' If you do not want to have a uniform background color, foreground color, textalign, and font used for all items and subitems 
    ''' in your ContainerListView control, you can set this property to false. When this property is set to true, any changes made to 
    ''' the subitem's ContainerListViewSubItem.Font, ContainerListViewSubItem.ForeColor, and ContainerListViewSubItem.BackColor,  
    ''' ContainerListViewSubItem.TextAlign properties are ignored, and the values of the item are used instead. You can use this 
    ''' property if you need to specify a different text alignment, text color, background color, or font to be used for a subitem to 
    ''' highlight the item when subitems are displayed in the ContainerListView control.
    ''' </remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether the TextAlign, Font, ForeColor, and BackColor properties for the item are used for all its subitems.")> _
    Public Overridable Property UseItemStyleForSubItems() As Boolean
        Get
            Return Me._UseItemStyle
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._UseItemStyle.Equals(Value)) Then
                Me._UseItemStyle = Value
                If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
            End If
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Begins editing of the ContainerListViewObject.
    ''' </summary>
    ''' <remarks>Editing only occurs if the ContainerListViewObject belongs to a ContainerListView.</remarks>
    Public Overloads Sub BeginEdit()
        If (Me._LstView IsNot Nothing AndAlso Not Me.IsEditing) Then
            'RAISE THE 'BEFORE' EVENT TO SEE IF WE SHOULD CONTINUE...
            If (Not Me._LstView.ContainerListViewBeforeEdit(Me)) Then
                Me._beginEdit(Me._LstView.GetItemBounds(Me), Me._Text, Me._Font, Me._ForeClr)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Begins editing the specified SubItem.
    ''' </summary>
    ''' <param name="aSubItem">The SubItem to begin editing on.</param>
    ''' <remarks></remarks>
    Public Overloads Sub BeginEdit(ByVal aSubItem As ContainerListViewSubItem)
        'MAKE SURE THAT THE SUBITEM BELONGS TO THE PARENT WHO STARTED THE EDITING, THAT THE SUBITEM IS NOT ALREADY IN AN EDIT 
        'STATE, AND THAT THE SUBITEM IS NOT DISPLAYING A CONTROL.
        If (Me._SubItems.Contains(aSubItem)) Then
            If (Me._LstView IsNot Nothing AndAlso Not aSubItem.IsEditing AndAlso aSubItem.Control Is Nothing) Then
                Me._EditedItem = aSubItem

                'RAISE THE 'BEFORE' EVENT TO SEE IF WE SHOULD CONTINUE...
                If (Not Me._LstView.ContainerListViewBeforeEdit(Me)) Then
                    If (Me._UseItemStyle) Then
                        Me._beginEdit(aSubItem.Bounds, aSubItem.Text, Me._Font, Me._ForeClr)
                    Else
                        Me._beginEdit(aSubItem.Bounds, aSubItem.Text, aSubItem.Font, aSubItem.ForeColor)
                    End If
                Else
                    Me._EditedItem = Nothing
                End If
            End If
        Else
            Throw New ArgumentException("aSubItem is not a member of this ContainerListViewObject's subitem collection.")
        End If
    End Sub

    ''' <summary>
    ''' Begins editing the specified SubItem.
    ''' </summary>
    ''' <param name="aSubItemIndex">A Integer representing the index of the subitem to edit.</param>
    ''' <remarks></remarks>
    Public Overloads Sub BeginEdit(ByVal aSubItemIndex As Integer)
        Me.BeginEdit(Me._SubItems(aSubItemIndex))
    End Sub

    ''' <summary>
    ''' Notifies the class that it's Font is being changed by the base ContainerListView.
    ''' </summary>
    ''' <param name="fontVal">The Font to set.</param>
    ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ChangeGlobalFont(ByVal fontVal As Font)
        If (Not Me._Font.Equals(fontVal) AndAlso (Me._FontChangedByBase OrElse Me.HasDefaultFont)) Then
            If (Me.HasDefaultFont) Then Me._FontChangedByBase = True
            Me._Font = fontVal
            If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Notifies the class that it's ForeColor is being changed by the base ContainerListView.
    ''' </summary>
    ''' <param name="aColor">The Color to set.</param>
    ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ChangeGlobalForeColor(ByVal aColor As Color)
        If (Not Me._ForeClr.Equals(aColor) AndAlso (Me._ForeClrChangedByBase OrElse Me.HasDefaultForeColor)) Then
            If (Me.HasDefaultForeColor) Then Me._ForeClrChangedByBase = True
            Me._ForeClr = aColor
            If (Me.ListView IsNot Nothing) Then Me.ListView.Invalidate()
        End If
    End Sub

    ''' <summary>
    ''' Ends ALL editing on this ContainerListViewObject and any of it's subitems.
    ''' </summary>
    ''' <param name="aCancelChanges"><c>TRUE</c> to cancel any changes; otherwise <c>FALSE</c> to keep them.</param>
    ''' <remarks></remarks>
    Public Sub EndEdit(Optional ByVal aCancelChanges As Boolean = False)
        If (Me.IsEditing OrElse Me._EditedItem IsNot Nothing) Then
            Dim Txt As String = Me._EditBox.Text

            'HIDE AND RESET THE EDITCONTROL
            Me._EditBox.Hide()
            Me._LstView.Controls.Remove(Me._EditBox)
            RemoveHandler Me._EditBox.KeyUp, AddressOf Me._editBoxKeyUpHandler
            Me._EditBox.Text = String.Empty

            'SET THE TEXT FOR THE PROPER EDITED ITEM AND THEN RAISE THE 'AFTER' EVENT ON THE CONTAINERLISTVIEW
            If (Not aCancelChanges) Then
                If (Me.IsEditing) Then Me.Text = Txt Else Me._EditedItem.Text = Txt
                Me._LstView.ContainerListViewAfterEdit(Me)
            End If

            'IF IT WAS A SUBITEM, SET IT TO NOTHING.
            'WASN'T SURE IF I SHOULD SET THIS TO NOTHING BEFORE CALLING THE 'AFTER' EVENT, BUT IF I DID THEN THE DEVELOPER WOULD NOT
            'KNOW WHICH SUBITEM WAS EDITED AFTER THE USER FINISHED SINCE THE EVENT ONLY PASSES THE PARENT CONTAINERLISTVIEWOBJECT.
            Me._EditedRect = Rectangle.Empty
            Me._LstView.SetEditedObject(Nothing)
            If (Me._EditedItem IsNot Nothing) Then Me._EditedItem = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Returns a SubItem at the specified point.
    ''' </summary>
    ''' <param name="aPoint">The point at which to check for a SubItem.</param>
    ''' <returns>A ContainerListViewSubItem ojbect.</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetSubItemAt(ByVal aPoint As Point) As ContainerListViewSubItem
        For Each SI As ContainerListViewSubItem In Me._SubItems
            If (SI.Bounds.Contains(aPoint)) Then Return SI
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns a SubItem at the specified point.
    ''' </summary>
    ''' <param name="aX">The X coordinate.</param>
    ''' <param name="aY">The Y coordinate.</param>
    ''' <returns>A ContainerListViewSubItem ojbect.</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetSubItemAt(ByVal aX As Integer, ByVal aY As Integer) As ContainerListViewSubItem
        Return Me.GetSubItemAt(New Point(aX, aY))
    End Function

    ''' <summary>
    ''' Sets the properties of a ContainerListViewObject with the data contained in the XmlNode.
    ''' </summary>
    ''' <param name="aObject">A ContainerListViewObject whose properties to set.</param>
    ''' <param name="aXmlNode">The XmlNode containing the data.</param>
    ''' <remarks></remarks>
    Protected Shared Sub Parse(ByVal aObject As ContainerListViewObject, ByVal aXmlNode As XmlNode)
        If (Not (aObject Is Nothing OrElse aXmlNode Is Nothing)) Then
            Dim XNode As XmlNode = Nothing
            Dim Atts() As Reflection.PropertyInfo = Nothing

            If (aXmlNode IsNot Nothing) Then
                Atts = aObject.GetType.GetProperties(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)

                For Each Prop As Reflection.PropertyInfo In Atts
                    If (Prop.CanWrite) Then
                        XNode = aXmlNode.SelectSingleNode(Prop.Name)

                        If (XNode IsNot Nothing) Then Prop.SetValue(aObject, XNode.Value, Nothing)
                    End If
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' Refreshes the editing control if the ContainerListViewObject or any of it's subitems are in an edit state.
    ''' </summary>
    ''' <remarks></remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub RefreshEditing()
        Me._EditBox.Location = New Point(Me._EditedRect.X, Me._EditedRect.Y)
        Me._EditBox.Size = New Size(Me._EditedRect.Width, Me._EditedRect.Height)
    End Sub

    ''' <summary>
    ''' Overriden.  Returns a String that represents the current Object.
    ''' </summary>
    ''' <returns>A String that represents the current Object. </returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return Me._Text
    End Function

#End Region

#Region " Procedures "

    Private Sub _beginEdit(ByVal aRect As Rectangle, ByVal aText As String, ByVal aFont As Font, ByVal aForeColor As Color)
        'LET'S END THE EDIT OF ANYTHING THAT IS ALREADY IN AN EDIT STATE.
        If (Me._LstView.EditedObject IsNot Nothing) Then Me._LstView.EditedObject.EndEdit()

        'SAVE THE EDIT RECTANGLE
        If (aRect.X = 0) Then
            Me._EditedRect = New Rectangle(aRect.X + 2, aRect.Y, aRect.Width - 3, aRect.Height - 1)
        Else
            Me._EditedRect = New Rectangle(aRect.X, aRect.Y, aRect.Width - 1, aRect.Height - 1)
        End If

        'CREATE THE EDITING CONTROL IF NEEDED
        If (Me._EditBox Is Nothing) Then
            Me._EditBox = New RichTextBox
            With Me._EditBox
                .AcceptsTab = False
                .BorderStyle = BorderStyle.None
                .Font = Me._Font
                .ForeColor = Me._ForeClr
                .DetectUrls = False
                .Multiline = False
                .ScrollBars = RichTextBoxScrollBars.None
            End With
        End If

        'SET THE DYNAMIC FIELDS
        With Me._EditBox
            .BackColor = Me._LstView.EditBackColor
            .Location = New Point(aRect.X, aRect.Y)
            .Size = New Size(Me._EditedRect.Width, Me._EditedRect.Height)
            .Text = aText
            Me._LstView.SetEditedObject(Me)
            Me._LstView.Controls.Add(Me._EditBox)
            .Show()
            .Focus()
            .Select(0, .Text.Length)
            AddHandler .KeyUp, AddressOf Me._editBoxKeyUpHandler
        End With
    End Sub

    Private Sub _editBoxKeyUpHandler(ByVal sender As Object, ByVal e As KeyEventArgs)
        'ESC MEANS CANCEL ANY CHANGES AND END THE EDITING SESSION
        'RETURN/ENTER MEANS END THE EDITING SESSION BUT PERSIST THE CHANGES
        Select Case e.KeyCode
            Case Keys.Escape
                Me.EndEdit(True)

            Case Keys.Return, Keys.Enter
                Me.EndEdit()
        End Select
    End Sub

    Private Sub _preInit()
        'PUT ANY INTIALIZATION CODE IN HERE.  IF YOU DO NOT NEED THIS SUB, THEN YOU CAN SAFELY DELETE IT AND THE CALL IN 
        'THE CONSTRUCTOR ALSO.
    End Sub

#End Region

#Region " Sub Classes "

    ''' <summary>
    ''' ContainerListViewSubItem Class.
    ''' </summary>
    ''' <remarks></remarks>
    <DesignTimeVisible(False), _
     DefaultProperty("Text"), _
     ToolboxItem(False), _
     TypeConverter(GetType(WinControls.ListView.TypeConverters.ContainerListViewSubItemConverter))> _
    Public Class ContainerListViewSubItem
        Implements ICloneable

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItem.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItem.
        ''' </summary>
        ''' <param name="aText">The Text to assign.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aText As String)
            Me._Text = aText
            Me._constructControl(Nothing)
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItem.
        ''' </summary>
        ''' <param name="aControl">The control to display in the SubItem.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aControl As Control)
            Me._constructControl(aControl)
        End Sub

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItem.
        ''' </summary>
        ''' <param name="aText">The text to display for the item.</param>
        ''' <param name="aBackColor">The BackColor of the item.</param>
        ''' <param name="aForeColor">The ForeColor of the item.</param>
        ''' <param name="fontVal">The Font of the Item.</param>
        ''' <param name="aTag">Data associated with the subitem.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal aText As String, ByVal aBackColor As Color, ByVal aForeColor As Color, ByVal fontVal As Font, Optional ByVal aTag As Object = Nothing)
            Me._Text = aText
            Me._Tag = aTag
            Me._BckClr = aBackColor
            Me._ForeClr = aForeColor
            Me._Font = fontVal
        End Sub

#End Region

#Region " Field Declarations "
        Private _BckClr As Color = Color.Transparent
        Private _ChildControl As Control
        Private _Font As Font = Windows.Forms.Control.DefaultFont
        Private _FontChangedByBase As Boolean = True
        Private _ForeClr As Color = SystemColors.WindowText
        Private _ForeClrChangedByBase As Boolean = True
        Private _Parent As ContainerListViewObject
        Private _ParentOwner As ContainerListView
        Private _Tag As Object
        Private _Text As String = "SubItem " & Me.Index
        Private _TextAlgn As HorizontalAlignment = HorizontalAlignment.Left
#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or Sets the BackColor of the SubItem.
        ''' </summary>
        ''' <value>A System.Color value.</value>
        ''' <remarks>
        ''' You can use the BackColor property to change the color displayed behind the subitem text. This property can be used if you want to use different background 
        ''' and foreground color combinations (using the ForeColor property to set the foreground color) to differentiate one subitem from another. For example, you could 
        ''' set the BackColor property to Color.Red to identify subitems that display a negative value.
        ''' If the UseItemStyleForSubItems property of the ContainerListViewItem that owns the subitem is set to true, setting this property has no effect.
        ''' </remarks>
        <Category("Appearance"), _
     DefaultValue(GetType(Color), "Transparent"), _
     Description("Determines the BackColor of the SubItem.")> _
    Public Property BackColor() As Color
            Get
                Return Me._BckClr
            End Get
            Set(ByVal Value As Color)
                If (Not Me._BckClr.Equals(Value)) Then
                    Me._BckClr = Value
                    Me._invalidateParent()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the bounding rectangle of the ContainerListViewSubItem within it's Parent's bounding Rectangle.
        ''' </summary>
        ''' <value>A Rectangle representing the bounding rectangle; otherwise Rect.Empty if there is no parent.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property Bounds() As Rectangle
            Get
                Return Me._regenerateRectangle
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the font of the text displayed by the subitem.
        ''' </summary>
        ''' <value>The Font to apply to the text displayed by the control.</value>
        ''' <remarks>
        ''' You can use this method to change the typeface styles applied to the text of the subitem. If the UseItemStyleForSubItems property of the ContainerListViewItem 
        ''' is set to true, changing this property will have no effect. Because the Font is immutable (meaning that you cannot adjust any of its properties), you can only 
        ''' assign the Font property a new Font. However, you can base the new font on the existing font.
        ''' </remarks>
        <Category("Appearance"), _
         Localizable(True), _
         DefaultValue(GetType(Font), "System.Drawing.SystemFonts.DefaultFont"), _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
         Description("Determines the font of the text displayed by the subitem.")> _
        Public Property Font() As Font
            Get
                Return Me._Font
            End Get
            Set(ByVal Value As Font)
                If (Not Me._Font.Equals(Value)) Then
                    Me._FontChangedByBase = False
                    Me._Font = Value
                    Me._invalidateParent()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the foreground color of the subitem's text.
        ''' </summary>
        ''' <value>A Color that represents the foreground color of the subitem's text.</value>
        ''' <remarks>
        ''' You can use the ForeColor property to change the color of the subitem text. This property can be used if you want to use different background and foreground 
        ''' color combinations (using the BackColor property to set the background color) to differentiate one item from another. For example, you could set the ForeColor 
        ''' property to Color.Red to identify items that have a negative number associated with them.
        ''' If the UseItemStyleForSubItems property of the ContainerListViewItem that owns the subitem is set to true, setting this property has no effect.
        ''' </remarks>
        <Category("Appearance"), _
         DefaultValue(GetType(Color), "WindowText"), _
         Description("Determines the foreground color of the subitem's text.")> _
        Public Property ForeColor() As Color
            Get
                Return Me._ForeClr
            End Get
            Set(ByVal Value As Color)
                If (Not Me._ForeClr.Equals(Value)) Then
                    Me._ForeClrChangedByBase = False
                    Me._ForeClr = Value
                    Me._invalidateParent()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the specifed control to the SubItem.
        ''' </summary>
        ''' <value>A Control.</value>
        ''' <remarks></remarks>
        <Category("Appearance"), _
     DefaultValue(GetType(Control), "Nothing"), _
     Description("Assignes the specifed control to the SubItem.")> _
    Public Property Control() As Control
            Get
                Return Me._ChildControl
            End Get
            Set(ByVal Value As Control)
                If (Not Me._ChildControl Is Value) Then
                    Me._ChildControl = Me._constructControl(Value)
                    Me._invalidateParent()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a value that determines if the ContainerListViewObject's BackColor is the default.
        ''' </summary>
        ''' <value><c>TRUE</c> if the BackColor is the default value; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property HasDefaultBackColor() As Boolean
            Get
                Return (Me._ForeClr.Equals(Color.Transparent))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that determines if the ContainerListViewObject's Font is the default.
        ''' </summary>
        ''' <value><c>TRUE</c> if the Font is the default value; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property HasDefaultFont() As Boolean
            Get
                Return (Me._Font.Equals(Windows.Forms.Control.DefaultFont))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that determines if the ContainerListViewObject's ForeColor is the default.
        ''' </summary>
        ''' <value><c>TRUE</c> if the ForeColor is the default value; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property HasDefaultForeColor() As Boolean
            Get
                Return (Me._ForeClr.Equals(SystemColors.WindowText))
            End Get
        End Property

        ''' <summary>
        ''' Gets the zero-based position of the SubItem in the collection.
        ''' </summary>
        ''' <value>An Integer representing the Index.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property Index() As Integer
            Get
                If (Me.Parent IsNot Nothing) Then Return Me.Parent.SubItems.IndexOf(Me)

                Return -1
            End Get
        End Property

        ''' <summary>
        ''' Gets a value that determines if the ContainerListViewSubItem is currently being edited by the user.
        ''' </summary>
        ''' <value><c>TRUE</c> if the SubItem is in an edit state; otherwise <c>FALSE</c>.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property IsEditing() As Boolean
            Get
                If (Me._ParentOwner IsNot Nothing AndAlso Me._ParentOwner.EditedObject IsNot Nothing) Then
                    Return (Me._ParentOwner.EditedObject.EditedSubItem Is Me)
                End If

                Return False
            End Get
        End Property

        ''' <summary>
        ''' The owner of this SubItem.
        ''' </summary>
        ''' <value>A ContainerListViewObject that the SubItem belongs to.</value>
        ''' <remarks></remarks>
        <Browsable(False)> _
    Public ReadOnly Property Parent() As ContainerListViewObject
            Get
                Return Me._Parent
            End Get
        End Property

        ''' <summary>
        ''' The Object to associate to the item.
        ''' </summary>
        ''' <value>An Object.</value>
        ''' <remarks></remarks>
        <Category("Data"), _
     Bindable(True), _
     Localizable(True), _
     DefaultValue(GetType(Object), Nothing), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
     TypeConverter(GetType(System.ComponentModel.StringConverter))> _
    Public Property Tag() As Object
            Get
                Return Me._Tag
            End Get
            Set(ByVal Value As Object)
                Me._Tag = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the Text contained in the control.
        ''' </summary>
        ''' <value>A String.</value>
        ''' <remarks></remarks>
        <Category("Appearance"), _
         Localizable(True), _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), _
         Description("The Text contained in the control.")> _
        Public Property Text() As String
            Get
                Return Me._Text
            End Get
            Set(ByVal Value As String)
                If (Not Me._Text.Equals(Value)) Then
                    Me._Text = Value
                    Me._invalidateParent()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or Sets the Alignment of the text within the ContainerListViewSubItem.
        ''' </summary>
        ''' <value>A HorizontalAlignment enumeration.</value>
        ''' <remarks>
        ''' You can use the TextAlign property to change the alignment of the subitem text. This property can be used if you want to use different alignments 
        ''' to differentiate one subitem from another. If the UseItemStyleForSubItems property of the ContainerListViewItem that owns 
        ''' the subitem is set to true, setting this property has no effect.
        ''' </remarks>
        <Category("Appearance"), _
     Localizable(True), _
     DefaultValue(GetType(HorizontalAlignment), "Left"), _
     Description("The Alignment of the text within the ContainerListViewObject.")> _
    Public Property TextAlign() As HorizontalAlignment
            Get
                Return Me._TextAlgn
            End Get
            Set(ByVal Value As HorizontalAlignment)
                If (Not Me._TextAlgn.Equals(Value)) Then
                    Me._TextAlgn = Value
                    Me._invalidateParent()
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Notifies the class that it's Font is being changed by the base ContainerListView.
        ''' </summary>
        ''' <param name="fontVal">The Font to set.</param>
        ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
        <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ChangeGlobalFont(ByVal fontVal As Font)
            If (Not Me._Font.Equals(fontVal) AndAlso (Me._FontChangedByBase OrElse Me.HasDefaultFont)) Then
                If (Me.HasDefaultFont) Then Me._FontChangedByBase = True
                Me._Font = fontVal
            End If
        End Sub

        ''' <summary>
        ''' Notifies the class that it's ForeColor is being changed by the base ContainerListView.
        ''' </summary>
        ''' <param name="aColor">The Color to set.</param>
        ''' <remarks>This code is not intended to be called directly from your code.  It is for internal use only.</remarks>
        <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub ChangeGlobalForeColor(ByVal aColor As Color)
            If (Not Me._ForeClr.Equals(aColor) AndAlso (Me._ForeClrChangedByBase OrElse Me.HasDefaultForeColor)) Then
                If (Me.HasDefaultForeColor) Then Me._ForeClrChangedByBase = True
                Me._ForeClr = aColor
            End If
        End Sub

        ''' <summary>
        ''' Creates a new Object that is a copy of the current instance.
        ''' </summary>
        ''' <returns>An object that is a Clone of the current instance.</returns>
        ''' <remarks></remarks>
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim Slvi As New ContainerListViewSubItem

            With Slvi
                .BackColor = Me.BackColor
                .Font = Me.Font
                .ForeColor = Me.ForeColor
                .Control = Me.Control
                .Tag = Me.Tag
                .Text = Me.Text
            End With

            Return Slvi
        End Function

        ''' <summary>
        ''' Returns a ContainerListViewSubItem set with the property data in the XmlNode.
        ''' </summary>
        ''' <param name="aXmlNode">The XmlNode containing the property data.</param>
        ''' <returns>
        ''' A ContainerListViewSubItem set with the passed in property data; otherwise, a new ContainerListViewSubItem set with defaults.
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared Function Parse(ByVal aXmlNode As XmlNode) As ContainerListViewSubItem
            Dim Clvsi As New ContainerListViewSubItem

            If (aXmlNode IsNot Nothing) Then
                Dim XNode As XmlNode = Nothing
                Dim Atts() As Reflection.PropertyInfo = Nothing

                If (aXmlNode IsNot Nothing) Then
                    Atts = Clvsi.GetType.GetProperties(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)

                    For Each Prop As Reflection.PropertyInfo In Atts
                        If (Prop.CanWrite) Then
                            XNode = aXmlNode.SelectSingleNode(Prop.Name)

                            If (XNode IsNot Nothing) Then Prop.SetValue(Clvsi, XNode.Value, Nothing)
                        End If
                    Next
                End If
            End If

            Return Clvsi
        End Function

        ''' <summary>
        ''' Removes the current ContainerListViewSubItem from the ContainerListViewItem.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Remove()
            If (Me.Parent IsNot Nothing) Then Me.Parent.SubItems.Remove(Me)
        End Sub

        ''' <summary>
        ''' Assigns the parent ContainerListViewItem to the SubItem.
        ''' </summary>
        ''' <param name="aParent">The ContainerListViewItem that the SubItem belongs to.</param>
        ''' <remarks>This code is for internal use only and is not intended to be called from your code.  Calling this method externally may have an adverse effect on code that uses this class.</remarks>
        <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Overloads Sub SetParent(ByVal aParent As ContainerListViewObject)
            Me._Parent = aParent
        End Sub

        ''' <summary>
        ''' Sets the Parent ContainerListView.
        ''' </summary>
        ''' <param name="aClvw">A ContainerListView object.</param>
        ''' <remarks></remarks>
        <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub SetParentOwner(ByVal aClvw As ContainerListView)
            Me._ParentOwner = aClvw
            If (Me._ChildControl IsNot Nothing) Then
                If (aClvw IsNot Nothing) Then
                    Me._ChildControl.Parent = aClvw
                Else
                    Me._ChildControl.Visible = False
                    'Me._ChildControl.Parent = Nothing
                End If
            End If
        End Sub

        ''' <summary>
        ''' Overriden.  Returns a string representing the object in the SubItem.
        ''' </summary>
        ''' <returns>A String object.</returns>
        ''' <remarks>If the Control is nothing then the Text property is returned.  Otherwise, the string representation of the type of control displayed in the SubItem is returned.</remarks>
        Public Overrides Function ToString() As String
            If (Me.Control Is Nothing) Then Return Me.Text Else Return Me.Control.ToString
        End Function

#End Region

#Region " Procedures "

        Private Function _constructControl(ByVal aControl As Control) As Control
            'IF THERE IS ALREADY AN EXISTING CONTROL LET'S DO SOME REMOVE PROCESSING
            If (Me._ChildControl IsNot Nothing) Then
                Me._ChildControl.Visible = False

                If (Me._ChildControl.Parent IsNot Nothing) Then Me._ChildControl.Parent.Controls.Remove(Me._ChildControl)
                RemoveHandler Me._ChildControl.Click, AddressOf Me._controlClicked
                RemoveHandler Me._ChildControl.Disposed, AddressOf Me._controlDisposed
            End If

            If (aControl IsNot Nothing) Then
                aControl.Visible = False

                If (aControl.Parent IsNot Nothing) Then aControl.Parent.Controls.Remove(aControl)
                AddHandler aControl.Click, AddressOf Me._controlClicked
                AddHandler aControl.Disposed, AddressOf Me._controlDisposed
            End If

            Return aControl
        End Function

        Private Sub _controlClicked(ByVal sender As Object, ByVal e As EventArgs)
            Me._invalidateParent(True)
        End Sub

        Private Sub _controlDisposed(ByVal sender As Object, ByVal e As EventArgs)
            Me._ChildControl.Visible = False
            'If ( Me._ChildControl.Parent IsNot Nothing) Then Me._ChildControl.Parent.Controls.Remove(Me._ChildControl)
            RemoveHandler Me._ChildControl.Click, AddressOf Me._controlClicked
            Me._ChildControl = Nothing
            Me._invalidateParent()
        End Sub

        Private Sub _invalidateParent(Optional ByVal SelectParent As Boolean = False)
            If (Me.Parent IsNot Nothing) Then
                If (Me.Parent.ListView IsNot Nothing) Then
                    If (SelectParent) Then
                        Me.Parent.ListView.SelectedItems.Clear()
                        Me.Parent.Selected = True
                        Me.Parent.ListView.SetFocusedObject(Me.Parent)
                    End If
                    Me.Parent.ListView.Invalidate()
                End If
            End If
        End Sub

        Private Function _regenerateRectangle() As Rectangle
            Dim Rect As Rectangle = Rectangle.Empty

            If (Me.Parent IsNot Nothing) Then
                Dim PrntRect As Rectangle = Rectangle.Empty

                If (Me._ParentOwner IsNot Nothing AndAlso Me._ParentOwner.Columns.Count > (Me.Index + 1)) Then
                    Dim ColRect As Rectangle = Me._Parent.ListView.Columns(Me.Index + 1).Bounds

                    PrntRect = Me._Parent.CompleteBounds
                    Rect = New Rectangle(ColRect.X - 1, PrntRect.Y, ColRect.Width + 1, PrntRect.Height)
                End If
            End If

            Return Rect
        End Function

#End Region

    End Class

#End Region

End Class