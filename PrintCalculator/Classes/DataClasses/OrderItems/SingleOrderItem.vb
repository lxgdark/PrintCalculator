Imports System.Collections.ObjectModel
Imports WPFProjectCore
Namespace DataClasses
    Public Class SingleOrderItem
        Inherits BaseOrderItem
#Region "Свойства"
#Region "Внутренние"
        Private itemHeaderValue As String = "Отдельная составная часть"
        Private itemValue As New SinglePositionInOrder
#End Region
        ''' <summary>
        ''' Заголовок состовной части
        ''' </summary>
        ''' <returns></returns>
        Public Property ItemHeader As String
            Get
                Return itemHeaderValue
            End Get
            Set(value As String)
                itemHeaderValue = value
                OnPropertyChanged(NameOf(ItemHeader))
            End Set
        End Property
        ''' <summary>
        '''Позиция каталога отвечающая за  доп. обработку 
        ''' </summary>
        ''' <returns></returns>
        Public Property Item As SinglePositionInOrder
            Get
                Return itemValue
            End Get
            Set(value As SinglePositionInOrder)
                itemValue = value
                OnPropertyChanged(NameOf(SinglePositionInOrder))
            End Set
        End Property
#End Region
#Region "Процедуры и функции"
        Public Overrides Sub Calculation()
            IsValidCostPrice = Item.GetValideCalculation
            ProductCount = Item.Count
            ProductCostPrice = Item.GetCostPrice
        End Sub
        ''' <summary>
        ''' Возвращает составные части наменклатуры
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetProductStructureList() As List(Of ProductStructureInformer)
            Dim result As New List(Of ProductStructureInformer)
            Dim resultCatalogItem As CatalogItem
            If Item.MaterialCatalogItem.Name <> "" Then
                resultCatalogItem = Item.MaterialCatalogItem
            Else
                resultCatalogItem = Item.BasicCatalogItem
            End If
            If resultCatalogItem.ItemCategory = CatalogItem.ItemCategoryEnum.SERVICE Then Return result
            Dim psi As New ProductStructureInformer With {
                    .Code = resultCatalogItem.Code,
                    .Name = resultCatalogItem.Name,
                    .Unit = resultCatalogItem.Unit,
                    .Count = Item.Count}
            result.Add(psi)
            Return result
        End Function
        ''' <summary>
        ''' Возвращает минимальный тираж для данной позиции
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetMinPrintCopy() As Integer
            'Так как это одиночная позиция, то ее минимальный тираж всегда равен 1
            Return 1
        End Function
#End Region
    End Class
End Namespace