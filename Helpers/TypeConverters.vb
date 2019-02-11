Imports System.Reflection
Imports System.ComponentModel.Design.Serialization

Namespace TypeConverters

    ''' <summary>
    ''' ContainerColumnHeaderConverter Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerColumnHeaderConverter
        Inherits TypeConverter

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerColumnHeaderConverter.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overloaded. Returns whether this converter can convert the object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aDestType">A Type that represents the type you want to convert to.</param>
        ''' <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function CanConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aDestType As System.Type) As Boolean
            Return (aDestType Is GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(aContext, aDestType))
        End Function

        ''' <summary>
        ''' Overloaded. Converts the given aValue object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aCulture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current aCulture is assumed. </param>
        ''' <param name="aValue">The Object to convert. </param>
        ''' <param name="aDestType">The Type to convert the aValue parameter to. </param>
        ''' <returns>An Object that represents the converted aValue.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function ConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aCulture As System.Globalization.CultureInfo, ByVal aValue As Object, ByVal aDestType As System.Type) As Object
            If (aDestType Is GetType(InstanceDescriptor) AndAlso (aValue IsNot Nothing AndAlso aValue.GetType Is GetType(ContainerColumnHeader))) Then
                Dim CH As ContainerColumnHeader = DirectCast(aValue, ContainerColumnHeader)
                Dim CI As ConstructorInfo = CH.GetType.GetConstructor(Type.EmptyTypes)

                If (CI IsNot Nothing) Then Return New InstanceDescriptor(CI, Nothing, False)
            End If

            Return MyBase.ConvertTo(aContext, aCulture, aValue, aDestType)
        End Function

#End Region

    End Class

    ''' <summary>
    ''' ContainerListViewItemConverter Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewItemConverter
        Inherits TypeConverter

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewItemConverter.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overloaded. Returns whether this converter can convert the object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aDestType">A Type that represents the type you want to convert to.</param>
        ''' <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function CanConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aDestType As System.Type) As Boolean
            Return (aDestType Is GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(aContext, aDestType))
        End Function

        ''' <summary>
        ''' Overloaded. Converts the given aValue object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aCulture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current aCulture is assumed. </param>
        ''' <param name="aValue">The Object to convert. </param>
        ''' <param name="aDestType">The Type to convert the aValue parameter to. </param>
        ''' <returns>An Object that represents the converted aValue.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function ConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aCulture As System.Globalization.CultureInfo, ByVal aValue As Object, ByVal aDestType As System.Type) As Object
            If (aDestType Is GetType(InstanceDescriptor) AndAlso aValue.GetType Is GetType(ContainerListViewItem)) Then
                Dim CI As ConstructorInfo = Nothing
                Dim SubItms() As ContainerListViewItem.ContainerListViewSubItem = Nothing
                Dim Clvi As ContainerListViewItem = DirectCast(aValue, ContainerListViewItem)

                If (Clvi.SubItems.Count > 0) Then
                    ReDim SubItms(Clvi.SubItems.Count - 1)
                    Clvi.SubItems.CopyTo(SubItms, 0)
                End If

                CI = Clvi.GetType.GetConstructor(New Type() {GetType(String), GetType(Integer), GetType(Color), GetType(Color), GetType(Font), GetType(ContainerListViewItem.ContainerListViewSubItem())})
                If (CI IsNot Nothing) Then
                    Dim Pars() As Object = {Clvi.Text, Clvi.ImageIndex, Clvi.BackColor, Clvi.ForeColor, Clvi.Font, SubItms}

                    Return New InstanceDescriptor(CI, Pars, False)
                End If
            End If

            Return MyBase.ConvertTo(aContext, aCulture, aValue, aDestType)
        End Function

#End Region

    End Class

    ''' <summary>
    ''' ContainerListViewSubItemConverter Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContainerListViewSubItemConverter
        Inherits TypeConverter

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of ContainerListViewSubItemConverter.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overloaded. Returns whether this converter can convert the object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aDestType">A Type that represents the type you want to convert to.</param>
        ''' <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function CanConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aDestType As System.Type) As Boolean
            Return (aDestType Is GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(aContext, aDestType))
        End Function

        ''' <summary>
        ''' Overloaded. Converts the given aValue object to the specified type.
        ''' </summary>
        ''' <param name="aContext">An ITypeDescriptorContext that provides a format aContext. </param>
        ''' <param name="aCulture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current aCulture is assumed. </param>
        ''' <param name="aValue">The Object to convert. </param>
        ''' <param name="aDestType">The Type to convert the aValue parameter to. </param>
        ''' <returns>An Object that represents the converted aValue.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function ConvertTo(ByVal aContext As System.ComponentModel.ITypeDescriptorContext, ByVal aCulture As System.Globalization.CultureInfo, ByVal aValue As Object, ByVal aDestType As System.Type) As Object
            If (aDestType Is GetType(InstanceDescriptor) AndAlso aValue.GetType Is GetType(ContainerListViewItem.ContainerListViewSubItem)) Then
                Dim Clvi As ContainerListViewItem.ContainerListViewSubItem = DirectCast(aValue, ContainerListViewItem.ContainerListViewSubItem)
                Dim CI As ConstructorInfo = Clvi.GetType.GetConstructor(New Type() {GetType(String), GetType(Color), GetType(Color), GetType(Font), GetType(String)})

                If (CI IsNot Nothing) Then
                    Dim Parms() As Object = {Clvi.Text, Clvi.BackColor, Clvi.ForeColor, Clvi.Font, Clvi.Tag}

                    Return New InstanceDescriptor(CI, Parms, False)
                End If
            End If

            Return MyBase.ConvertTo(aContext, aCulture, aValue, aDestType)
        End Function

#End Region

    End Class

    ''' <summary>
    ''' TreeListNodeConverter Class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TreeListNodeConverter
        Inherits TypeConverter

#Region " Constructors and Destructors "

        ''' <summary>
        ''' Creates a New instance of TreeListNodeConverter.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' Overloaded. Returns whether this converter can convert the object to the specified type.
        ''' </summary>
        ''' <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        ''' <param name="destinationType">A Type that represents the type you want to convert to.</param>
        ''' <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean
            Return (destinationType Is GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(context, destinationType))
        End Function

        ''' <summary>
        ''' Overloaded. Converts the given aValue object to the specified type.
        ''' </summary>
        ''' <param name="context">An ITypeDescriptorContext that provides a format context. </param>
        ''' <param name="aCulture">A CultureInfo object. If a null reference (Nothing in Visual Basic) is passed, the current aCulture is assumed. </param>
        ''' <param name="aValue">The Object to convert. </param>
        ''' <param name="destinationType">The Type to convert the aValue parameter to. </param>
        ''' <returns>An Object that represents the converted aValue.</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal aCulture As System.Globalization.CultureInfo, ByVal aValue As Object, ByVal destinationType As System.Type) As Object
            If (destinationType Is GetType(InstanceDescriptor) AndAlso aValue.GetType Is GetType(TreeListNode)) Then
                Dim CI As ConstructorInfo
                Dim Params() As Object = Nothing
                Dim TlNode As TreeListNode = DirectCast(aValue, TreeListNode)

                If (TlNode.ImageIndex.Equals(-1) AndAlso TlNode.SelectedImageIndex.Equals(-1)) Then
                    CI = TlNode.GetType.GetConstructor(New Type() {GetType(String)})
                    If (CI IsNot Nothing) Then Params = New Object() {TlNode.Text}
                Else
                    CI = TlNode.GetType.GetConstructor(New Type() {GetType(String), GetType(Integer), GetType(Integer)})
                    If (CI IsNot Nothing) Then Params = New Object() {TlNode.Text, TlNode.ImageIndex, TlNode.SelectedImageIndex}
                End If

                Return New InstanceDescriptor(CI, Params, False)
            End If

            Return MyBase.ConvertTo(context, aCulture, aValue, destinationType)
        End Function

#End Region

    End Class

End Namespace