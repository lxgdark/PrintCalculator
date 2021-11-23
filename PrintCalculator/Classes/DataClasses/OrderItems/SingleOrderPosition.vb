Imports PrintCalculator.DataClasses
Imports WPFProjectCore
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
    Private countInProductValue As Double = 1
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
    ''' Значение количества указанной позиции
    ''' </summary>
    ''' <returns></returns>
    Public Property CountInProduct As Double
        Get
            Return countInProductValue
        End Get
        Set(value As Double)
            countInProductValue = value
            OnPropertyChanged(NameOf(CountInProduct))
        End Set
    End Property
#End Region
    '''' <summary>
    '''' Возврачает себестоимость позиции
    '''' </summary>
    '''' <returns></returns>
    'Public Function GetCostPrice() As Double
    '    ' Return CatalogItem.CostPrice * CountInProduct
    'End Function
End Class
