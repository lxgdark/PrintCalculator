Imports PrintCalculator.DataClasses
Class CreatePersonalPaperPopupPage
    Private Calculation As [Delegate]
    Dim catalogItem As CatalogItem
    Public Sub SetParametr(_catalogItem As CatalogItem, calculationSub As [Delegate])
        'Сохраняем делегат, который нужно вызвать по завершению
        Calculation = calculationSub
        'Созраняем ссылку на текущю составную часть заказа
        catalogItem = _catalogItem

        PaperNameTextBox.Text = catalogItem.Name
        HeightPaperSize.Value = BaseOrderItem.GetSheetSize(catalogItem.Unit).Height
        WidthPaperSize.Value = BaseOrderItem.GetSheetSize(catalogItem.Unit).Width
        CostPrice.Value = catalogItem.CostPrice
    End Sub

    Private Sub CloseParametrButton_Click(sender As Object, e As RoutedEventArgs)
        catalogItem.Name = PaperNameTextBox.Text
        catalogItem.Unit = "L" & WidthPaperSize.Value & "x" & HeightPaperSize.Value
        catalogItem.CostPrice = CostPrice.Value
        Windows.Application.Current.Dispatcher.Invoke(Calculation)
    End Sub
End Class
