Imports WPFProjectCore
Namespace DataClasses
    Public Class CatalogItem
        Inherits NotifyProperty_Base(Of CatalogItem)
#Region "Свойства"
#Region "Внутренние"
        Private nameValue As String = ""
        Private codeValue As String = ""
        Private costPriceValue As Double = 0
        Private unitValue As String = ""
        Private groupCodeValue As String = ""
        Private commentValue As String = ""
        Private itemCategoryValue As ItemCategoryEnum = ItemCategoryEnum.NONE
        Private itemTagValue As String = ""
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
        ''' Себестоимость
        ''' </summary>
        ''' <returns></returns>
        Public Property CostPrice As Double
            Get
                Return costPriceValue
            End Get
            Set(value As Double)
                costPriceValue = value
                OnPropertyChanged(NameOf(CostPrice))
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
        ''' <summary>
        ''' Код раздела каталога в котором находится позиция
        ''' </summary>
        ''' <returns></returns>
        Public Property GroupCode As String
            Get
                Return groupCodeValue
            End Get
            Set(value As String)
                groupCodeValue = value
                OnPropertyChanged(NameOf(GroupCode))
            End Set
        End Property
        ''' <summary>
        ''' Категория текущей позиции (материал или услуга)
        ''' </summary>
        ''' <returns></returns>
        Public Property ItemCategory As ItemCategoryEnum
            Get
                Return itemCategoryValue
            End Get
            Set(value As ItemCategoryEnum)
                itemCategoryValue = value
                OnPropertyChanged(NameOf(ItemCategory))
            End Set
        End Property
        ''' <summary>
        ''' Комментарий к позиции
        ''' </summary>
        ''' <returns></returns>
        Public Property Comment As String
            Get
                Return commentValue
            End Get
            Set(value As String)
                commentValue = value
                OnPropertyChanged(NameOf(Comment))
            End Set
        End Property
        ''' <summary>
        ''' Тэг пункта каталога, опредиляющий его к подвидам материалов или услуг
        ''' </summary>
        ''' <returns></returns>
        Public Property ItemTag As String
            Get
                Return itemTagValue
            End Get
            Set(value As String)
                itemTagValue = value
                OnPropertyChanged(NameOf(ItemTag))
            End Set
        End Property
#End Region
#Region "Перечеслители и типы"
        ''' <summary>
        ''' Глобальная категория пункта каталога (материал или услуга)
        ''' </summary>
        Public Enum ItemCategoryEnum
            ''' <summary>
            ''' Категория неопеределена или не задана
            ''' </summary>
            NONE = 0
            ''' <summary>
            ''' Материал (бумага, ламинация, пружинки и т.д.)
            ''' </summary>
            MATERIAL = 1
            ''' <summary>
            ''' Стандартная услуга
            ''' </summary>
            SERVICE = 2
        End Enum
#End Region
    End Class
End Namespace