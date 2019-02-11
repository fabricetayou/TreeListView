Imports System.Runtime.InteropServices

Namespace Helpers

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' VisualStyles Class.
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Friend Class VisualStyles

#Region " Field Declarations "

        ''' <summary>
        ''' Structure RECT for VisualStyles.
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure RECT
            <FieldOffset(12)> _
            Private _Bottom As Integer
            <FieldOffset(0)> _
            Private _Left As Integer
            <FieldOffset(8)> _
            Private _Right As Integer
            <FieldOffset(4)> _
            Private _Top As Integer

            ''' <summary>
            ''' Represents an empty RECT structure.
            ''' </summary>
            ''' <remarks></remarks>
            Public Shared ReadOnly Empty As RECT

            ''' <summary>
            ''' Creates a New instance of RECT.
            ''' </summary>
            ''' <param name="Rect">The Rectangle to initialize the structure with.</param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Rect As Rectangle)
                Me._Bottom = Rect.Bottom
                Me._Left = Rect.Left
                Me._Right = Rect.Right
                Me._Top = Rect.Top
            End Sub

            ''' <summary>
            ''' Creates a New instance of RECT.
            ''' </summary>
            ''' <param name="aLeft">The x-coordinate of the left edge of the rectangle.</param>
            ''' <param name="aTop">The y-coordinate of the top edge of the Rectangle</param>
            ''' <param name="aRight">The x-coordinate of the right edge of the rectangle.</param>
            ''' <param name="aBottom">The y-coordinate of the bottom edge of the Rectangle.</param>
            ''' <remarks></remarks>
            Public Sub New(ByVal aLeft As Integer, ByVal aTop As Integer, ByVal aRight As Integer, ByVal aBottom As Integer)
                Me._Left = aLeft
                Me._Right = aRight
                Me._Top = aTop
                Me._Bottom = aBottom
            End Sub

            ''' <summary>
            ''' Gets the y-coordinate of the bottom edge of the Rectangle
            ''' </summary>
            ''' <value>An Integer value.</value>
            ''' <remarks></remarks>
            Public ReadOnly Property Bottom() As Integer
                Get
                    Return Me._Bottom
                End Get
            End Property

            ''' <summary>
            ''' Gets the y-coordinate of the top edge of the Rectangle
            ''' </summary>
            ''' <value>An Integer value.</value>
            ''' <remarks></remarks>
            Public ReadOnly Property Top() As Integer
                Get
                    Return Me._Top
                End Get
            End Property

            ''' <summary>
            ''' Gets the x-coordinate of the left edge of the Rectangle
            ''' </summary>
            ''' <value>An Integer value.</value>
            ''' <remarks></remarks>
            Public ReadOnly Property Left() As Integer
                Get
                    Return Me._Left
                End Get
            End Property

            ''' <summary>
            ''' Gets the x-coordinate of the right edge of the Rectangle
            ''' </summary>
            ''' <value>An Integer value.</value>
            ''' <remarks></remarks>
            Public ReadOnly Property Right() As Integer
                Get
                    Return Me._Right
                End Get
            End Property

        End Structure

#End Region

#Region " Methods "

        ''' <summary>
        ''' Closes the Theme handle.
        ''' </summary>
        ''' <param name="aTheme">The Theme handle to close.</param>
        ''' <returns><c>TRUE</c> if the handle was closed successfully; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function CloseThemeData(ByVal aTheme As IntPtr) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aTheme"></param>
        ''' <param name="aHDC"></param>
        ''' <param name="aPart"></param>
        ''' <param name="aState"></param>
        ''' <param name="Rect"></param>
        ''' <param name="aClipRect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function DrawThemeBackground(ByVal aTheme As IntPtr, ByVal aHDC As IntPtr, ByVal aPart As Integer, ByVal aState As Integer, ByRef Rect As RECT, ByRef aClipRect As RECT) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aTheme"></param>
        ''' <param name="aHDC"></param>
        ''' <param name="aPart"></param>
        ''' <param name="aState"></param>
        ''' <param name="aDestinatinRect"></param>
        ''' <param name="aEdge"></param>
        ''' <param name="aFlags"></param>
        ''' <param name="aContentRect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function DrawThemeEdge(ByVal aTheme As IntPtr, ByVal aHDC As IntPtr, ByVal aPart As Integer, ByVal aState As Integer, ByRef aDestinatinRect As RECT, ByVal aEdge As Integer, ByVal aFlags As Integer, ByRef aContentRect As RECT) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aTheme"></param>
        ''' <param name="aHDC"></param>
        ''' <param name="aPart"></param>
        ''' <param name="aState"></param>
        ''' <param name="Rect"></param>
        ''' <param name="aImageList"></param>
        ''' <param name="aImageIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function DrawThemeIcon(ByVal aTheme As IntPtr, ByVal aHDC As IntPtr, ByVal aPart As Integer, ByVal aState As Integer, ByRef Rect As RECT, ByVal aImageList As IntPtr, ByVal aImageIndex As Integer) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aTheme"></param>
        ''' <param name="aHDC"></param>
        ''' <param name="Rect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function DrawThemeParentBackground(ByVal aTheme As IntPtr, ByVal aHDC As IntPtr, ByRef Rect As RECT) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aTheme"></param>
        ''' <param name="aHDC"></param>
        ''' <param name="aPart"></param>
        ''' <param name="aState"></param>
        ''' <param name="aText"></param>
        ''' <param name="aCharCount"></param>
        ''' <param name="aTextFlags"></param>
        ''' <param name="aTextFlags2"></param>
        ''' <param name="Rect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function DrawThemeText(ByVal aTheme As IntPtr, ByVal aHDC As IntPtr, ByVal aPart As Integer, ByVal aState As Integer, <MarshalAs(UnmanagedType.LPTStr)> ByVal aText As String, ByVal aCharCount As Integer, ByVal aTextFlags As UInt32, ByVal aTextFlags2 As UInt32, ByRef Rect As RECT) As Boolean

        End Function

        ''' <summary>
        ''' Determines if the current application is capable of running visual styles.
        ''' </summary>
        ''' <returns><c>TRUE</c> if the application can run visual styles; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function IsAppThemed() As Boolean

        End Function

        ''' <summary>
        ''' Determines if the current application's visual style is active.
        ''' </summary>
        ''' <returns><c>TRUE</c> if visual styles are active; otherwise <c>FALSE</c>.</returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function IsThemeActive() As Boolean

        End Function

        ''' <summary>
        ''' Opens the theme data.
        ''' </summary>
        ''' <param name="aHandle">The handle of the control/window to be themed.</param>
        ''' <param name="aControlList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function OpenThemeData(ByVal aHandle As IntPtr, <MarshalAs(UnmanagedType.LPTStr)> ByVal aControlList As String) As IntPtr

        End Function

        ''' <summary>
        ''' ?
        ''' </summary>
        ''' <param name="aHandle"></param>
        ''' <param name="aSubAppName"></param>
        ''' <param name="aSubIDList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DllImport("uxtheme.dll")> _
      Public Shared Function SetWindowTheme(ByVal aHandle As IntPtr, ByVal aSubAppName As String, ByVal aSubIDList As String) As Boolean

        End Function

#End Region

    End Class

    Friend Class DragHelpers

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of DragHelpers.
        ''' </summary>
        ''' <remarks></remarks>
        Shared Sub New()
            InitCommonControls()
        End Sub

#End Region

#Region " Methods "

        <DllImport("comctl32.dll")> _
        Public Shared Function ImageList_BeginDrag(ByVal aTrack1 As IntPtr, ByVal aTrack2 As Integer, ByVal aDx As Integer, ByVal aDy As Integer) As Boolean

        End Function

        <DllImport("comctl32.dll")> _
        Public Shared Function ImageList_DragEnter(ByVal aHwnd As IntPtr, ByVal aX As Integer, ByVal aY As Integer) As Boolean

        End Function

        <DllImport("comctl32.dll")> _
        Public Shared Function ImageList_DragLeave(ByVal aHwnd As IntPtr) As Boolean

        End Function

        <DllImport("comctl32.dll")> _
        Public Shared Function ImageList_DragMove(ByVal aX As Integer, ByVal aY As Integer) As Boolean

        End Function

        <DllImport("comctl32.dll")> _
        Public Shared Function ImageList_DragShowNolock(ByVal aShow As Boolean) As Boolean

        End Function

        <DllImport("comctl32.dll")> _
        Public Shared Sub ImageList_EndDrag()

        End Sub

        <DllImport("comctl32.dll")> _
        Public Shared Function InitCommonControls() As Boolean

        End Function


#End Region

    End Class

End Namespace