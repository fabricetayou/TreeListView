''' <summary>
''' ContainerColumnHeader Class.
''' </summary>
''' <remarks></remarks>
<DesignTimeVisible(False), _
 ToolboxItem(False), _
 DefaultProperty("Text"), _
 TypeConverter(GetType(TypeConverters.ContainerColumnHeaderConverter))> _
Public Class ContainerColumnHeader
    Inherits Component
    Implements ICloneable

#Region " Constructors and Destructors "

    ''' <summary>
    ''' Creates a New instance of ContainerColumnHeader.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerColumnHeader.
    ''' </summary>
    ''' <param name="aText">A string to initialize the Text with.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aText As String)
        Me._Text = aText
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerColumnHeader.
    ''' </summary>
    ''' <param name="aText">A string to initialize the Text with.</param>
    ''' <param name="aWidth">An Integer representing the Width.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aText As String, ByVal aWidth As Integer)
        Me._Text = aText
        Me._Wdth = aWidth
    End Sub

    ''' <summary>
    ''' Creates a New instance of ContainerColumnHeader.
    ''' </summary>
    ''' <param name="aText">A string to initialize the Text with.</param>
    ''' <param name="aWidth">An Integer representing the Width.</param>
    ''' <param name="aAlignment">An enumerated HorizontalAligment to initialize the Column with.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aText As String, ByVal aWidth As Integer, ByVal aAlignment As HorizontalAlignment)
        Me._Text = aText
        Me._Wdth = aWidth
        Me._TextAlgn = aAlignment
    End Sub

#End Region

#Region " Field Declarations "
    Private _AllowResize As Boolean = True
    Private _Font As Font = Control.DefaultFont
    Private _ForeClr As Color = SystemColors.WindowText
    Private _HForeClr As Color = SystemColors.WindowText
    Private _HBackClr As Color = SystemColors.Control
    Private _Hovered As Boolean
    Private _ImgIndex As Integer = -1
    Private _ListView As ContainerListView
    Private _Pressed As Boolean
    Private _Tag As Object
    Private _Text As String = String.Empty
    Private _TextAlgn As HorizontalAlignment = HorizontalAlignment.Center
    Private _Visible As Boolean
    Private _Wdth As Integer = 150
#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or Sets a value that determines whether the ColumnHeader can be resized.
    ''' </summary>
    ''' <value><c>TRUE</c> if the ColumnHeader can be resized; otherwise <c>FALSE</c>.  The default is <c>TRUE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     DefaultValue(True), _
     Description("Determines whether the ColumnHeader can be resized.")> _
    Public Property AllowResize() As Boolean
        Get
            Return Me._AllowResize
        End Get
        Set(ByVal Value As Boolean)
            Me._AllowResize = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the bounding rectangle of the ContainerColumnHeader.
    ''' </summary>
    ''' <value>A Rectangle representing the bounding rectangle; otherwise <c>NOTHING</c> if the ContainerColumnHeader is not associated to a TreeListView.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Bounds() As Rectangle
        Get
            Return Me._regenerateRectangle
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the font of the text displayed by the ColumnHeader.
    ''' </summary>
    ''' <value>
    ''' The Font to apply to the text displayed by the control.
    ''' </value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     Localizable(True), _
     DefaultValue(GetType(Font), "Control.DefaultFont"), _
     Description("Determines the font of the text displayed by the item.")> _
    Public Property Font() As Font
        Get
            Return Me._Font
        End Get
        Set(ByVal Value As Font)
            If (Not Me._Font.Equals(Value)) Then
                Me._Font = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the foreground color of the subitem's text.
    ''' </summary>
    ''' <value>A Color that represents the foreground color of the subitem's text.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "WindowText"),
     Description("Determines the ForeColor of the the Node's text.")>
    Public Property ForeColor() As Color
        Get
            Return Me._ForeClr
        End Get
        Set(ByVal Value As Color)
            If (Not Me._ForeClr.Equals(Value)) Then
                Me._ForeClr = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate()
            End If
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the foreground color of the header text.
    ''' </summary>
    ''' <value>A Color that represents the foreground color of the header text.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "WindowText"),
     Description("Determines the ForeColor of the the header text.")>
    Public Property HeaderForeColor() As Color
        Get
            Return Me._HForeClr
        End Get
        Set(ByVal Value As Color)
            If (Not Me._HForeClr.Equals(Value)) Then
                Me._HForeClr = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate()
            End If
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the background color of the header.
    ''' </summary>
    ''' <value>A Color that represents the background color of the header.</value>
    ''' <remarks></remarks>
    <Category("Appearance"),
     DefaultValue(GetType(Color), "Control"),
     Description("Determines the background of the the header.")>
    Public Property HeaderBackColor() As Color
        Get
            Return Me._HBackClr
        End Get
        Set(ByVal Value As Color)
            If (Not Me._HBackClr.Equals(Value)) Then
                Me._HBackClr = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate()
            End If
        End Set
    End Property
    ''' <summary>
    ''' Gets or Sets a value that determines whether the Column is hidden or not.
    ''' </summary>
    ''' <value><c>TRUE</c> if the Column is hidden; otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Category("Behavior"), _
     Localizable(True), _
     DefaultValue(False), _
     Description("Determines whether the Column is hidden or not.")> _
    Public Property Hidden() As Boolean
        Get
            Return Me._Visible
        End Get
        Set(ByVal Value As Boolean)
            If (Not Me._Visible.Equals(Value)) Then
                If (Value = True) Then
                    If (Me._ListView IsNot Nothing AndAlso Me._ListView.AllowHiddenColumns) Then
                        Me._Visible = Value
                        Me._ListView.HiddenColumns.Add(Me)
                        Me._ListView.Invalidate()
                    End If
                Else
                    Me._Visible = Value
                    If (Me._ListView IsNot Nothing AndAlso Me._ListView.HiddenColumns.Contains(Me)) Then
                        Me._ListView.HiddenColumns.Remove(Me)
                        Me._ListView.Invalidate()
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the column is Hovered.
    ''' </summary>
    ''' <value><c>TRUE</c> if the column is hovered;  otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Friend Property Hovered() As Boolean
        Get
            Return Me._Hovered
        End Get
        Set(ByVal Value As Boolean)
            Me._Hovered = Value
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
     RefreshProperties(RefreshProperties.Repaint), _
     TypeConverter(GetType(System.Windows.Forms.ImageIndexConverter)), _
     Description("The Index of the image that is displayed for the item."), _
     Editor("System.Windows.Forms.Design.ImageIndexEditor", GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property ImageIndex() As Integer
        Get
            Return Me._ImgIndex
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._ImgIndex.Equals(Value)) Then
                Me._ImgIndex = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate(Me.Bounds)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the ImageList associated with the ContainerColumnHeader.
    ''' </summary>
    ''' <value>An ImageList; otherwise <c>NOTHING</c> if the Column is not associated to a ContainerListView.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
     Public ReadOnly Property ImageList() As ImageList
        Get
            If (Me._ListView IsNot Nothing) Then Return Me._ListView.ImageList

            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the zero-based Index of the Column in the collection.
    ''' </summary>
    ''' <value>An Integer representing the Index.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Index() As Integer
        Get
            If (Me._ListView IsNot Nothing) Then Return Me._ListView.Columns.IndexOf(Me) Else Return -1

        End Get
    End Property

    ''' <summary>
    ''' Gets the ListView this column belongs to.
    ''' </summary>
    ''' <value>A ContainerListView or <c>NOTHING</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property ListView() As ContainerListView
        Get
            Return Me._ListView
        End Get
    End Property

    ''' <summary>
    ''' Gets or Sets a value that determines if the Column is in a pressed state.
    ''' </summary>
    ''' <value><c>TRUE</c> if the column is pressed; otherwise <c>FALSE</c>.  Default is <c>FALSE</c>.</value>
    ''' <remarks></remarks>
    <Browsable(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Friend Property Pressed() As Boolean
        Get
            Return Me._Pressed
        End Get
        Set(ByVal Value As Boolean)
            Me._Pressed = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the portion of the Column rectangle used for sizing.
    ''' </summary>
    ''' <value>A Rectangle object.</value>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Protected Friend ReadOnly Property SizingBounds() As Rectangle
        Get
            Dim Rect As Rectangle = Me.Bounds

            If (Not Rect.Equals(Rectangle.Empty)) Then Return New Rectangle(Rect.X + Rect.Width - 4, 2, 4, Me.ListView.HeaderBuffer)

            Return Rectangle.Empty
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets an object that contains data to associate with the item.
    ''' </summary>
    ''' <value>An object that contains information that is associated with the item.</value>
    ''' <remarks></remarks>
    <Category("Data"), _
     Localizable(False), _
     Bindable(True), _
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
    ''' Gets or Sets the Title of the ColumnHeader.
    ''' </summary>
    ''' <value>A String value.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     Localizable(True), _
     Description("The Title of the ColumnHeader.")> _
    Public Property Text() As String
        Get
            Return Me._Text
        End Get
        Set(ByVal Value As String)
            If (Not Me._Text.Equals(Value)) Then
                Me._Text = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate(Me.Bounds)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Alignment of the text within the ColumnHeaders.
    ''' </summary>
    ''' <value>A HorizontalAlignment enumeration.</value>
    ''' <remarks></remarks>
    <Category("Appearance"), _
     Localizable(True), _
     DefaultValue(GetType(HorizontalAlignment), "Center"), _
     Description("The Alignment of the text within the ColumnHeaders.")> _
    Public Property TextAlign() As HorizontalAlignment
        Get
            Return Me._TextAlgn
        End Get
        Set(ByVal Value As HorizontalAlignment)
            If (Not Me._TextAlgn.Equals(Value)) Then
                Me._TextAlgn = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the Width (in pixels) of the column.
    ''' </summary>
    ''' <value>An Integer representing the Width of the column.</value>
    ''' <remarks></remarks>
    <Category("Layout"), _
     Localizable(True), _
     DefaultValue(150), _
     Description("The Width (in pixels) of the column.")> _
    Public Property Width() As Integer
        Get
            Return Me._Wdth
        End Get
        Set(ByVal Value As Integer)
            If (Not Me._Wdth.Equals(Value)) Then
                Me._Wdth = Value
                If (Me._ListView IsNot Nothing) Then Me._ListView.HeaderResized(Me)
            End If
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Creates a new Object that is a copy of the current instance.
    ''' </summary>
    ''' <returns>An object that is a Clone of the current instance.</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim CH As New ContainerColumnHeader

        With CH
            .AllowResize = Me._AllowResize
            .Font = Me._Font
            .ForeColor = Me._ForeClr
            .Hidden = Me._Visible
            .ImageIndex = Me._ImgIndex
            .Tag = Me._Tag
            .Text = Me._Text
            .TextAlign = Me._TextAlgn
            .Width = Me._Wdth
        End With

        Return CH
    End Function

    ''' <summary>
    ''' Removes the current ContainerColumnHeader from the ColumnHeader collection of the ListView control it belongs to.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        If (Me._ListView IsNot Nothing) Then Me._ListView.Columns.Remove(Me)
    End Sub

    ''' <summary>
    ''' Sort the Items in the ContainerListView by this column.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Sort()
        If (Me._ListView IsNot Nothing) Then Me._ListView.Sort(Me.Index)
    End Sub

    ''' <summary>
    ''' Assigns the ContainerListView that the Column belongs to.
    ''' </summary>
    ''' <param name="aCont">The ContainerListView that owns the item.</param>
    ''' <remarks>This code is for internal use only and is not intended to be called from your code.  Calling this method externally may have an adverse effect on code that uses this class.</remarks>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Friend Sub SetParent(ByVal aCont As ContainerListView)
        Me._ListView = aCont

        If (Me._ListView IsNot Nothing AndAlso Me._ListView.AllowHiddenColumns AndAlso Me._Visible) Then Me.Hidden = False
    End Sub

    ''' <summary>
    ''' Overriden.  Displays the Text of the column header.
    ''' </summary>
    ''' <returns>A String value.</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return Me._Text
    End Function

#End Region

#Region " Procedures "

    Private Function _regenerateRectangle() As Rectangle
        Dim NewRect As Rectangle

        If (Not Me._Visible AndAlso Me._ListView IsNot Nothing) Then
            Dim Xpos As Integer = -(Me.ListView.HScroll.Value)

            For I As Integer = 0 To (Me.Index - 1)
                If (Not Me._ListView.Columns(I).Hidden) Then Xpos += Me._ListView.Columns(I).Width + 1
            Next

            NewRect = New Rectangle(Xpos, 2, Me.Width, Me._ListView.HeaderBuffer)
        End If

        Return NewRect
    End Function

#End Region

End Class
