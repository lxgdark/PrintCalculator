Imports System.Xml.Serialization
Imports WPFProjectCore
Namespace DataClasses
    ''' <summary>
    ''' Базовый интерфейс для составной части заказа
    ''' </summary>
    Public Interface IBaseOrderItem
        ''' <summary>
        ''' Процедура инициирующая расчет составной части
        ''' </summary>
        Sub Calculation()
        ''' <summary>
        ''' Возвращает количество изделий на лесте
        ''' </summary>
        ''' <returns></returns>
        Function GetProductCount() As Integer
        ''' <summary>
        ''' Возвращает себестоимость составной части (за изделие)
        ''' </summary>
        ''' <returns></returns>
        Function GetProductCostPrice() As Double
        ''' <summary>
        ''' Возвращает флаг. показывающий готовность позиции к расчету
        ''' </summary>
        ''' <returns></returns>
        Function GetIsValidCostPrice() As Boolean
        ''' <summary>
        ''' Функция, которая возвращает список составных частей наменклатуры
        ''' </summary>
        ''' <returns></returns>
        Function GetProductStructureList() As List(Of ProductStructureInformer)
        ''' <summary>
        ''' Возвращает минимальный тираж для данной позиции
        ''' </summary>
        ''' <returns></returns>
        Function GetMinPrintCopy() As Integer
    End Interface
    ''' <summary>
    ''' Базовый класс составной части расчета
    ''' </summary>
    <XmlInclude(GetType(StandartOrderItem))>
    <XmlInclude(GetType(SingleOrderItem))>
    <Serializable>
    Public MustInherit Class BaseOrderItem
        Inherits NotifyProperty_Base(Of BaseOrderItem)
        Implements IBaseOrderItem

#Region "Свойства"
#Region "Внутренние"
        Private productCountValue As Double = 0
        Private productCostPriceValue As Double = 0
        Private isValidCostPriceValue As Boolean = False
#End Region
        ''' <summary>
        ''' Количестве изделий на листе
        ''' </summary>
        ''' <returns></returns>
        Public Property ProductCount As Double
            Get
                Return productCountValue
            End Get
            Set(value As Double)
                productCountValue = value
                OnPropertyChanged(NameOf(ProductCount))
            End Set
        End Property
        ''' <summary>
        ''' Себестоимость состаной части
        ''' </summary>
        ''' <returns></returns>
        Public Property ProductCostPrice As Double
            Get
                Return productCostPriceValue
            End Get
            Set(value As Double)
                productCostPriceValue = value
                OnPropertyChanged(NameOf(ProductCostPrice))
            End Set
        End Property
        ''' <summary>
        ''' Указывает на то, достаточно ли данны для расчета себестоимости
        ''' </summary>
        ''' <returns></returns>
        Public Property IsValidCostPrice As Boolean
            Get
                Return isValidCostPriceValue
            End Get
            Set(value As Boolean)
                isValidCostPriceValue = value
                OnPropertyChanged(NameOf(IsValidCostPrice))
            End Set
        End Property
#End Region
#Region "Процедуры и функции"
        ''' <summary>
        ''' Переопределяемая функция, которая возвращает список составных частей наменклатуры
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetProductStructureList() As List(Of ProductStructureInformer) Implements IBaseOrderItem.GetProductStructureList
        ''' <summary>
        ''' Переопределяемая процедура в которой производятся вычисления данной позиции расчета
        ''' </summary>
        Public MustOverride Sub Calculation() Implements IBaseOrderItem.Calculation
        ''' <summary>
        ''' Возвращает минимальное число копий для данной позиции
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetMinPrintCopy() As Integer Implements IBaseOrderItem.GetMinPrintCopy
        ''' <summary>
        ''' Возвращает количество текущей позиции
        ''' </summary>
        ''' <returns></returns>
        Public Function GetProductCount() As Integer Implements IBaseOrderItem.GetProductCount
            Return ProductCount
        End Function
        ''' <summary>
        ''' Возвращает себестоимость позиции
        ''' </summary>
        ''' <returns></returns>
        Public Function GetProductCostPrice() As Double Implements IBaseOrderItem.GetProductCostPrice
            Return ProductCostPrice
        End Function
        ''' <summary>
        ''' Возвращает минимальный тираж для данной позиции
        ''' </summary>
        ''' <returns></returns>
        Public Function GetIsValidCostPrice() As Boolean Implements IBaseOrderItem.GetIsValidCostPrice
            Return IsValidCostPrice
        End Function
#End Region
    End Class
End Namespace