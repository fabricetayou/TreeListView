Imports System.Runtime.InteropServices

Namespace Helpers

    ''' <summary>
    ''' Static Tools Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Tools

#Region " Methods "

        ''' <summary>
        ''' Determines if an object has been clicked.
        ''' </summary>
        ''' <param name="e">The MouseEventArg where the user clicked.</param>
        ''' <param name="aKeysEnum">The Enumerator of the Keys in the HashTable to check.</param>
        ''' <param name="aValsEnum">The Enumerator of the Values in the HashTable to check.</param>
        ''' <returns>An Object if a match was found; otherwise <c>NOTHING</c>.</returns>
        ''' <remarks>The HashTable must contain Rectangles as Keys!</remarks>
        Friend Shared Function EvaluateObject(ByVal e As MouseEventArgs, ByVal aKeysEnum As IEnumerator, ByVal aValsEnum As IEnumerator) As Object
            Dim Rect As Rectangle

            While (aKeysEnum.MoveNext AndAlso aValsEnum.MoveNext)
                Rect = DirectCast(aKeysEnum.Current, Rectangle)

                If (Rect.Left <= e.X AndAlso (Rect.Left + Rect.Width) >= e.X) AndAlso (Rect.Top <= e.Y AndAlso (Rect.Top + Rect.Height) >= e.Y) Then
                    Return aValsEnum.Current
                End If
            End While

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the Width of a string using a graphics object.
        ''' </summary>
        ''' <param name="aText">The Text to measure.</param>
        ''' <param name="fontVal">The Font to measure the Text with.</param>
        ''' <returns>An Integer representing the text width.</returns>
        ''' <remarks>This function uses a default graphics object (from an Image object) to measure the Text.</remarks>
        Public Shared Function GetStringWidth(ByVal aText As String, ByVal fontVal As Font) As Integer
            Static Dim Gr As Graphics = Graphics.FromImage(New Bitmap(32, 32))
            Dim Sz As SizeF = Gr.MeasureString(aText, fontVal)

            Return CType(Sz.Width, Integer)
        End Function

        ''' <summary>
        ''' Calculates the Size of the displayed String.
        ''' </summary>
        ''' <param name="aGraph">A Graphics object.</param>
        ''' <param name="aText">The Text to measure.</param>
        ''' <param name="fontVal">The Font to include for measuring.</param>
        ''' <returns>A SizeF structure.</returns>
        ''' <remarks></remarks>
        Public Shared Function MeasureDisplayString(ByVal aGraph As Graphics, ByVal aText As String, ByVal fontVal As Font) As SizeF
            Const Width As Integer = 32
            Dim Bmap As Bitmap = New Bitmap(Width, 1, aGraph)
            Dim Anagra As Graphics = Graphics.FromImage(Bmap)
            Dim Sz As SizeF = aGraph.MeasureString(aText, fontVal)
            Dim MWidth As Integer = CType(Sz.Width, Integer)

            If (Anagra IsNot Nothing) Then
                Anagra.Clear(Color.White)
                Anagra.DrawString(aText & "|", fontVal, Brushes.Black, CType(Width - MWidth, Single), -(CType(fontVal.Height / 2, Single)))

                For I As Integer = (Width - 1) To 0 Step -1
                    MWidth -= 1
                    If (Bmap.GetPixel(I, 0).R = 0) Then Exit For
                Next
            End If

            Return New SizeF(MWidth, Sz.Height)
        End Function

        ''' <summary>
        ''' Truncates text.
        ''' </summary>
        ''' <param name="aText">The Text to truncate.</param>
        ''' <param name="aWidth">The Width of the rectangle.</param>
        ''' <param name="aOffSet">An Offset.</param>
        ''' <param name="aGr">A Graphics object to perform the measurement.</param>
        ''' <returns>A truncated string value.</returns>
        ''' <remarks></remarks>
        Public Shared Function TruncateString(ByVal aText As String, ByVal fontVal As Font, ByVal aWidth As Integer, ByVal aOffSet As Integer, ByVal aGr As Graphics) As String
            Dim Sz As SizeF
            Dim Swid, Length As Integer
            Dim Sprint As String = String.Empty

            Try
                Sz = aGr.MeasureString(aText, fontVal)
                Swid = CType(Sz.Width, Integer)
                Length = aText.Length

                While (Length > 0 AndAlso Swid > (aWidth - aOffSet))
                    Sz = aGr.MeasureString(aText.Substring(0, Length), fontVal)
                    Swid = CType(Sz.Width, Integer)
                    Length -= 1
                End While

                If (Length < aText.Length) Then
                    Dim Val As Integer = Length - 3

                    If (Val <= 0) Then Sprint = aText.Substring(0, 1) & "..." Else Sprint = aText.Substring(0, Val) & "..."
                Else
                    Sprint = aText.Substring(0, Length)
                End If

            Catch ex As Exception
                'DO <c>NOTHING</c>
            End Try

            Return Sprint
        End Function

#End Region

    End Class

End Namespace