''' <summary>
''' ContainerListViewItem Class.
''' </summary>
''' <remarks></remarks>
<DesignTimeVisible(False), _
 ToolboxItem(False), _
 DefaultProperty("Text"), _
 TypeConverter(GetType(WinControls.ListView.TypeConverters.ContainerListViewItemConverter))> _
Public Class ContainerListViewItem
    Inherits ContainerListViewObject

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New instance of ContainerListViewItem.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerListViewItem.
    ''' </summary>
    ''' <param name="aItemText">The text to display for the item.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aItemText As String)
        MyBase.New(aItemText)
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerListViewItem.
    ''' </summary>
    ''' <param name="aItemText">The text to display for the item.</param>
    ''' <param name="aImageIndex">The zero-based index of the image within the ImageList associated with the ContainerListView control that contains the item.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aItemText As String, ByVal aImageIndex As Integer)
        MyBase.New(aItemText)
        Me.ImageIndex = aImageIndex
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerListViewItem.
    ''' </summary>
    ''' <param name="aItemText">The text to display for the item.</param>
    ''' <param name="aImageIndex">The zero-based index of the image within the ImageList associated with the ContainerListView control that contains the item.</param>
    ''' <param name="aBackColor">The BackColor of the item.</param>
    ''' <param name="aForeColor">The ForeColor of the item.</param>
    ''' <param name="fontVal">The Font of the Item.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aItemText As String, ByVal aImageIndex As Integer, ByVal aBackColor As Color, ByVal aForeColor As Color, ByVal fontVal As Font)
        MyBase.New(aItemText)

        Me.ImageIndex = aImageIndex
        Me.BackColor = aBackColor
        Me.ForeColor = aForeColor
        Me.Font = fontVal
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerListViewItem.
    ''' </summary>
    ''' <param name="aItemText">The text to display for the item.</param>
    ''' <param name="aImageIndex">The zero-based index of the image within the ImageList associated with the ContainerListView control that contains the item.</param>
    ''' <param name="aBackColor">The BackColor of the item.</param>
    ''' <param name="aForeColor">The ForeColor of the item.</param>
    ''' <param name="fontVal">The Font of the Item.</param>
    ''' <param name="aSubItems">An array of Child subitems to add to the newly created ContainerListViewItem.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aItemText As String, ByVal aImageIndex As Integer, ByVal aBackColor As Color, ByVal aForeColor As Color, ByVal fontVal As Font, ByVal aSubItems() As ContainerListViewSubItem)
        MyBase.New(aItemText)

        Me.ImageIndex = aImageIndex
        Me.BackColor = aBackColor
        Me.ForeColor = aForeColor
        Me.Font = fontVal

        If (aSubItems IsNot Nothing) Then Me.SubItems.AddRange(aSubItems)
    End Sub

#End Region

#Region " Field Declarations "
    Private _Focused As Boolean = False
    Private _Selected As Boolean = False
    Private _StateImgIndex As Integer = -1
#End Region

#Region " Properties "

    ''' <summary>
    ''' Overriden.  Determines if the current TreeListNode has a parent node.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Overrides ReadOnly Property HasParent() As Boolean
        Get
            Return (Me.ListView IsNot Nothing)
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Gets the zero-based position of the ContainerListViewItem within the ContainerListViewItem collection.
    ''' </summary>
    ''' <value>
    ''' The zero-based index of the item within the ListView control's ContainerListView.ContainerListViewItemCollection. If the item is not associated with a ContainerListView control, this property returns -1.
    ''' </value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public Overrides ReadOnly Property Index() As Integer
        Get
            If (Me.ListView IsNot Nothing) Then Return Me.ListView.Items.IndexOf(Me)

            Return -1
        End Get
    End Property

    ''' <summary>
    ''' Overriden.  Gets or sets a value indicating whether the item is selected.
    ''' </summary>
    ''' <value><c>TRUE</c> if the item is selected; otherwise, <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Selected() As Boolean
        Get
            Return Me._Selected
        End Get
        Set(ByVal Value As Boolean)
            If (Me.AllowSelection) Then
                If (Not Me._Selected.Equals(Value)) Then
                    Dim InvalidView As Boolean = Me.ListView Is Nothing

                    If (Not Me._Selected) Then
                        Dim Cancel As Boolean = False

                        If (Not InvalidView) Then Cancel = Me.ListView.ContainerListViewBeforeSelect(Me)
                        If (Not Cancel) Then
                            Me._Selected = True

                            If (Not InvalidView) Then
                                If (Not Me.ListView.MultiSelect) Then
                                    Me.ListView.SelectedItems.Clear()
                                    Me.ListView.SelectedIndexes.Clear()
                                    Me.Focused = True
                                End If
                                Me.ListView.SelectedItems.Add(Me)
                                Me.ListView.SelectedIndexes.Add(Me.Index)
                                Me.ListView.ContainerListViewAfterSelect(Me)
                            End If
                        End If
                    Else
                        If (Not InvalidView) Then
                            Me.ListView.SelectedItems.Remove(Me)
                            Me.ListView.SelectedIndexes.Remove(Me.Index)
                        End If
                        Me._Selected = False
                    End If

                    If (Not InvalidView) Then Me.ListView.Invalidate()
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' <c>NOT SUPPORTED</c>. 
    ''' Gets or sets the index of the state image (an image such as a checked or cleared check box that indicates the state of the item) that is displayed for the item.
    ''' </summary>
    ''' <value>The zero-based index of the state image in the ImageList that is displayed for the item.</value>
    ''' <remarks></remarks>
    <Category("Not Supported"), _
     Localizable(True), _
     DefaultValue(-1), _
     Description("Not Supported." & ControlChars.CrLf & "The index of the state image (an image such as a checked or cleared check box that indicates the state of the item) that is displayed for the item."), _
     TypeConverter(GetType(System.Windows.Forms.ImageIndexConverter)), _
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property StateImageIndex() As Integer
        Get
            Return Me._StateImgIndex
        End Get
        Set(ByVal Value As Integer)
            Me._StateImgIndex = Value
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Overriden.  Creates a new Object that is a copy of the current instance.
    ''' </summary>
    ''' <returns>An object that is a Clone of the current instance.</returns>
    ''' <remarks></remarks>
    Public Overrides Function Clone() As Object
        Dim Clvi As New ContainerListViewItem

        With Clvi
            .BackColor = Me.BackColor
            .CheckBoxEnabled = Me.CheckBoxEnabled
            .CheckBoxVisible = Me.CheckBoxVisible
            .Checked = Me.Checked
            .Font = Me.Font
            .ForeColor = Me.ForeColor
            .ImageIndex = Me.ImageIndex
            .StateImageIndex = Me.StateImageIndex
            .Tag = Me.Tag
            .Text = Me.Text
            .UseItemStyleForSubItems = Me.UseItemStyleForSubItems
        End With

        'CLONE THE SUBITEMS ALSO
        For Each Slvi As ContainerListViewSubItem In Me.SubItems
            Clvi.SubItems.Add(DirectCast(Slvi.Clone, ContainerListViewSubItem))
        Next

        Return Clvi
    End Function

    ''' <summary>
    ''' Returns a ContainerListViewItem set with the property data in the XmlNode.
    ''' </summary>
    ''' <param name="aXmlNode">The XmlNode containing the property data.</param>
    ''' <returns>
    ''' A ContainerListViewItem set with the passed in property data; otherwise, a new ContainerListViewItem set with defaults.
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Shadows Function Parse(ByVal aXmlNode As XmlNode) As ContainerListViewItem
        Dim Clvi As New ContainerListViewItem

        ContainerListViewObject.Parse(Clvi, aXmlNode)

        Return Clvi
    End Function

    ''' <summary>
    ''' Removes the current ContainerListViewItem from the ContainerListView control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        If (Me.ListView IsNot Nothing) Then Me.ListView.Items.Remove(Me)
    End Sub

    ''' <summary>
    ''' Assigns the ListView that the Item belongs to.
    ''' </summary>
    ''' <param name="aLView">The ContainerListView that owns the item.</param>
    ''' <remarks>This code is for internal use only and is not intended to be called from your code.  Calling this method externally may have an adverse effect on code that uses this class.</remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Overrides Sub SetParent(ByVal aLView As ContainerListView)
        If (Not aLView Is Me._LstView) Then
            If (Me._LstView IsNot Nothing) Then
                'REMOVE FROM THE OTHER COLLECTIONS IF NECESSARY
                If (Me.Checked) Then Me._LstView.CheckedItems.Remove(Me)
                If (Me.Selected) Then
                    Me._LstView.SelectedItems.Remove(Me)
                    Me._LstView.SelectedIndexes.Remove(Me.Index)
                End If
            End If

            'ASSIGN THE LISTVIEW AND ADD TO ANY NECESSARY COLLECTIONS
            Me._LstView = aLView
            If (Me._LstView IsNot Nothing) Then
                If (Me.Checked) Then Me._LstView.CheckedItems.Add(Me)
                If (Me.Selected) Then
                    Me._LstView.SelectedItems.Add(Me)
                    Me._LstView.SelectedIndexes.Add(Me.Index)
                End If
            End If

            For Each SI As ContainerListViewSubItem In Me.SubItems
                SI.SetParentOwner(aLView)
            Next
        End If
    End Sub

#End Region

End Class