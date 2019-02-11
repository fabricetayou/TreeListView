Namespace Enums

    ''' <summary>
    '''  Enumerated values specifying possible BorderStyleType of a control or form.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BorderStyleType As Integer
        ''' <summary>
        ''' No border.
        ''' </summary>
        None = 0
        ''' <summary>
        ''' A single line border.
        ''' </summary>
        FixedSingle = 1
        ''' <summary>
        ''' A 3-Dimensional border.
        ''' </summary>
        Fixed3D = 2
        ''' <summary>
        ''' A small rounded border.
        ''' </summary>
        RoundedSmall = 3
        ''' <summary>
        ''' A medium rounded border.
        ''' </summary>
        RoundedMedium = 4
        ''' <summary>
        '''  A bumpy border.
        ''' </summary>
        Bump = 5
        ''' <summary>
        ''' An etched border.
        ''' </summary>
        Etched = 6
        ''' <summary>
        ''' A flat border.
        ''' </summary>
        Flat = 7
        ''' <summary>
        ''' A raised Border.
        ''' </summary>
        Raised = 8
        ''' <summary>
        ''' A border whose inside edges appear raised.
        ''' </summary>
        RaisedInner = 9
        ''' <summary>
        ''' A border whose outside edges appear raised.
        ''' </summary>
        RaisedOuter = 10
        ''' <summary>
        ''' A sunken border.
        ''' </summary>
        Sunken = 11
        ''' <summary>
        ''' A border whose inside edges appear sunken.
        ''' </summary>
        SunkenInner = 12
        ''' <summary>
        ''' A border whose outside edges appear sunken.
        ''' </summary>
        SunkenOuter = 13
        ''' <summary>
        ''' A border on the inside of the control's edges.
        ''' </summary>
        Inset = 14
        ''' <summary>
        ''' A border around the outside of the control.
        ''' </summary>
        Outset = 15
    End Enum

    ''' <summary>
    ''' Enumerated values specifying the style of a CheckBox.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CheckBoxStyle
        ''' <summary>
        ''' The Checkbox style has a flat, two-dimensional appearance.
        ''' </summary>
        ''' <remarks></remarks>
        Flat
        ''' <summary>
        ''' The CheckBox style has a three-dimensional appearance.
        ''' </summary>
        ''' <remarks></remarks>
        Normal
    End Enum

    ''' <summary>
    ''' Enumerated values specifying the type of CheckBox to draw.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CheckBoxType
        ''' <summary>
        ''' A CheckBox is specified.
        ''' </summary>
        ''' <remarks></remarks>
        CheckBox
        ''' <summary>
        ''' A RadioButton is specified.
        ''' </summary>
        ''' <remarks></remarks>
        RadioButton
    End Enum

    ''' <summary>
    ''' Enumerated values specifying Actions of a collection.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CollectionActions
        ''' <summary>
        ''' Nothing has happened to an Item.  Default value.
        ''' </summary>
        ''' <remarks></remarks>
        [Nothing]
        ''' <summary>
        ''' An Item has been Added.
        ''' </summary>
        ''' <remarks></remarks>
        Added
        ''' <summary>
        ''' An Item has changed.
        ''' </summary>
        ''' <remarks></remarks>
        Changed
        ''' <summary>
        ''' The collection has been cleared.
        ''' </summary>
        ''' <remarks></remarks>
        Cleared
        ''' <summary>
        ''' The collection is about to be cleared (no items have been cleared yet).
        ''' </summary>
        ''' <remarks></remarks>
        Clearing
        ''' <summary>
        ''' An Item has been removed.
        ''' </summary>
        ''' <remarks></remarks>
        Removed
    End Enum

    ''' <summary>
    ''' Enumerated values specifying possible DialogCodes (from the WinAPI).
    ''' </summary>
    ''' <remarks></remarks>
    Friend Enum DialogCodes
        ''' <summary>
        ''' The control wants arrow keys.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_WANTARROWS = &H1
        ''' <summary>
        ''' The control wants Tab keys.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_WANTTAB = &H2
        ''' <summary>
        ''' The control wants All keys.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_WANTALLKEYS = &H4
        ''' <summary>
        ''' Pass the message to the Control.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_WANTMESSAGE = &H4
        ''' <summary>
        ''' Understands the EM_SETSEL message.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_HASSETSEL = &H8
        ''' <summary>
        ''' Default PushButton.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_DEFPUSHBUTTON = &H10
        ''' <summary>
        ''' Non_Default PushButton.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_UNDEFPUSHBUTTON = &H20
        ''' <summary>
        ''' The Radio button.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_RADIOBUTTON = &H40
        ''' <summary>
        ''' The control wants WM_CHAR messages.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_WANTCHARS = &H80
        ''' <summary>
        ''' Static item:  DON'T include.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_STATIC = &H100
        ''' <summary>
        ''' A Button item:  it CAN be checked.
        ''' </summary>
        ''' <remarks></remarks>
        DLGC_BUTTON = &H2000
    End Enum

    ''' <summary>
    '''  Enumerated values specifying possible Windows Messages.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Enum WinMsgs
        ''' <summary>
        ''' WM_GETDLGCODE message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETDLGCODE = &H87
        ''' <summary>
        ''' WM_SETREDRAW message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETREDRAW = &HB
        ''' <summary>
        ''' WM_CANCELMODE message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CANCELMODE = &H1F
        ''' <summary>
        ''' WM_NOTIFY message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NOTIFY = &H4E
        ''' <summary>
        ''' WM_KEYDOWN message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYDOWN = &H100
        ''' <summary>
        ''' WM_KEYUP message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYUP = &H101
        ''' <summary>
        ''' WM_CHAR message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHAR = &H102
        ''' <summary>
        ''' WM_SYSKEYDOWN message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSKEYDOWN = &H104
        ''' <summary>
        ''' WM_SYSKEYUP message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSKEYUP = &H105
        ''' <summary>
        ''' WM_COMMAND message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COMMAND = &H111
        ''' <summary>
        ''' WM_MENUCHAR message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUCHAR = &H120
        ''' <summary>
        ''' WM_MOUSEMOVE message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEMOVE = &H200
        ''' <summary>
        ''' WM_LBUTTONDOWN message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_LBUTTONDOWN = &H201
        ''' <summary>
        ''' WM_MOUSELAST message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSELAST = &H20A
        ''' <summary>
        ''' WM_USER message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_USER = &H400
        ''' <summary>
        ''' WM_REFLECT message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_REFLECT = WM_USER + &H1C00
    End Enum

End Namespace