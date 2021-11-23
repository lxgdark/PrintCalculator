Imports PrintCalculator.DataClasses

Class CreatePersonalCatalogItemPopupPage
    Private Calculation As [Delegate]
    Dim catalogItem As CatalogItem
    Public Sub SetParametr(_catalogItem As CatalogItem, calculationSub As [Delegate])
        'Сохраняем делегат, который нужно вызвать по завершению
        Calculation = calculationSub
        'Созраняем ссылку на текущю составную часть заказа
        catalogItem = _catalogItem
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        CatalogItemNameTextBox.Focus()
    End Sub

    Private Sub CloseParametrButton_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
