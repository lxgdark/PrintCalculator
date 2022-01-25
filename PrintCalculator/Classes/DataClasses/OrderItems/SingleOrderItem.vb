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
            ProductCostPrice = Item.GetCostPrice
        End Sub
        ''' <summary>
        ''' Возвращает составные части наменклатуры
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetProductStructureList() As List(Of ProductStructureInformer)
            Dim result As New List(Of ProductStructureInformer)
            Dim resultCatalogItem As CatalogItem
            If Item.BasicCatalogItem.ItemCategory <> CatalogItem.ItemCategoryEnum.MATERIAL Then
                resultCatalogItem = Item.MaterialCatalogItem
            Else
                resultCatalogItem = Item.BasicCatalogItem
            End If
            Dim psi As New ProductStructureInformer With {
                    .Code = resultCatalogItem.Code,
                    .Name = resultCatalogItem.Name,
                    .Unit = resultCatalogItem.Unit,
                    .Count = Item.Count}
            result.Add(psi)
            Return result
        End Function
#End Region
    End Class
End Namespace