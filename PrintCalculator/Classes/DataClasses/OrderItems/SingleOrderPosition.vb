Imports PrintCalculator.DataClasses
Imports WPFProjectCore
Namespace DataClasses
    ''' <summary>
    ''' Описывает отдельную позицию заказа
    ''' Применяется в стандартной состовной части как список доп.обработок и в отдельной состовной части как основной параметр
    ''' 
    ''' !!!Нужно доделать так, чтобы при выборе из каталога услуги, требующей доп.материалов отображались дополнительные настройки
    ''' </summary>
    Public Class SingleOrderPosition
        Inherits NotifyProperty_Base(Of SingleOrderPosition)
#Region "Свойства"
#Region "Внутренние"
        Private basicCatalogItemValue As New CatalogItem
        Private materialCatalogItemValue As New CatalogItem
        Private countValue As Double = 1
        Private isProServiceValue As Boolean = False
        Private isPersonalItemValue As Boolean = False
#End Region
        ''' <summary>
        '''Позиция каталога отвечающая за  доп. обработку 
        ''' </summary>
        ''' <returns></returns>
        Public Property BasicCatalogItem As CatalogItem
            Get
                Return basicCatalogItemValue
            End Get
            Set(value As CatalogItem)
                basicCatalogItemValue = value
                OnPropertyChanged(NameOf(BasicCatalogItem))
            End Set
        End Property
        ''' <summary>
        ''' Дополнительная позиция каталога для случая когда к услуге нужно выбрать материал
        ''' </summary>
        ''' <returns></returns>
        Public Property MaterialCatalogItem As CatalogItem
            Get
                Return materialCatalogItemValue
            End Get
            Set(value As CatalogItem)
                materialCatalogItemValue = value
                OnPropertyChanged(NameOf(MaterialCatalogItem))
            End Set
        End Property
        ''' <summary>
        ''' Значение количества указанной позиции
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
        ''' Флаг указывающий на то, является ли выбранная основная позиция каталога специальной услугой
        ''' </summary>
        ''' <returns></returns>
        Public Property IsProService As Boolean
            Get
                Return isProServiceValue
            End Get
            Set(value As Boolean)
                isProServiceValue = value
                OnPropertyChanged(NameOf(IsProService))
            End Set
        End Property
        ''' <summary>
        ''' Флаг указывающий на то создаа ли данная позиция в пручную (а не выбрана из каталога)
        ''' </summary>
        ''' <returns></returns>
        Public Property IsPersonalItem As Boolean
            Get
                Return isPersonalItemValue
            End Get
            Set(value As Boolean)
                isPersonalItemValue = value
                OnPropertyChanged(NameOf(IsPersonalItem))
            End Set
        End Property
#End Region
        ''' <summary>
        ''' Проверяет все ли параметры заданы для просчета себестоимости
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValideCalculation() As Boolean
            IsProService = BasicCatalogItem.ItemCategory = CatalogItem.ItemCategoryEnum.PROSERVICE
            Return BasicCatalogItem.CostPrice > 0 And (Not IsProService OrElse (IsProService And MaterialCatalogItem.CostPrice > 0))
        End Function
        ''' <summary>
        ''' Возврачает себестоимость позиции
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCostPrice() As Double
            Return (BasicCatalogItem.CostPrice + IIf(IsProService, MaterialCatalogItem.CostPrice, 0)) * Count
        End Function
    End Class
End Namespace