Imports WPFProjectCore
Namespace DataClasses
    Public Class ProductStructureInformer
        Inherits NotifyProperty_Base(Of ProductStructureInformer)

#Region "Свойства"
#Region "Внутренние"
        Private nameValue As String = ""
        Private codeValue As String = ""
        Private countValue As Double = 0
        Private unitValue As String = ""
#End Region
        ''' <summary>
        ''' Наименование наменклатуры
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
            Get
                Return nameValue
            End Get
            Set(value As String)
                nameValue = value
                OnPropertyChanged(NameOf(Name))
            End Set
        End Property
        ''' <summary>
        ''' Код в каталоге
        ''' </summary>
        ''' <returns></returns>
        Public Property Code As String
            Get
                Return codeValue
            End Get
            Set(value As String)
                codeValue = value
                OnPropertyChanged(NameOf(Code))
            End Set
        End Property
        ''' <summary>
        ''' Количество
        ''' </summary>
        ''' <returns></returns>
        Public Property Count As Double
            Get
                Return countValue
            End Get
            Set(value As Double)
                countValue = value
                OnPropertyChanged(NameOf(Count))
            End Set
        End Property
        ''' <summary>
        ''' Единица измерения
        ''' </summary>
        ''' <returns></returns>
        Public Property Unit As String
            Get
                Return unitValue
            End Get
            Set(value As String)
                unitValue = value
                OnPropertyChanged(NameOf(Unit))
            End Set
        End Property

#End Region
    End Class
End Namespace