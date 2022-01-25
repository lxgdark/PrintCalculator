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
        HeightPaperSize.Value = Workers.PaperSizeWorker.GetSheetSize(catalogItem.Unit).Height
        WidthPaperSize.Value = Workers.PaperSizeWorker.GetSheetSize(catalogItem.Unit).Width
        CostPrice.Value = catalogItem.CostPrice
    End Sub

    Private Sub CloseParametrButton_Click(sender As Object, e As RoutedEventArgs)
        If PaperNameTextBox.Text = "" Or CostPrice.Value <= 0 Then Exit Sub
        catalogItem.Name = PaperNameTextBox.Text
        catalogItem.Unit = "L" & WidthPaperSize.Value & "x" & HeightPaperSize.Value
        catalogItem.CostPrice = CostPrice.Value
        Windows.Application.Current.Dispatcher.Invoke(Calculation)
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        PaperNameTextBox.Focus()
    End Sub
End Class
