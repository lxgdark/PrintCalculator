Imports PrintCalculator.DataClasses

Class CreatePersonalCatalogItemPopupPage
    Private Calculation As [Delegate]
    Dim catalogItem As CatalogItem
    Dim secondcatalogItem As CatalogItem
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        PositionNameTextBox.Focus()
    End Sub

    Public Sub SetParametr(_catalogItem As SinglePositionInOrder, calculationSub As [Delegate])
        'Сохраняем делегат, который нужно вызвать по завершению
        Calculation = calculationSub
        'Сохраняем ссылки на текущие составные части
        catalogItem = _catalogItem.BasicCatalogItem
        secondcatalogItem = _catalogItem.MaterialCatalogItem
        'Наименование основной позиции
        PositionNameTextBox.Text = catalogItem.Name
        'Наименование вспомогательной позиции
        SecondPositionNameTextBox.Text = secondcatalogItem.Name
        'Себестоимость основной позиции
        CostPrice.Value = catalogItem.CostPrice
        'Себестоимость вспомогательной позиции
        SecondCostPrice.Value = secondcatalogItem.CostPrice
        'Если имя вспомогательной позиции не пустое
        If secondcatalogItem.Name <> "" Then
            'Устанавливаем флаг, что задан вспомогательный материал
            IsSecondMaterialCheck.IsChecked = True
            'Устанавливаем единицу измерения от вспомогательного материала
            UnitNameComboBox.SelectedItem = secondcatalogItem.Unit
        Else
            'Если вспопмогательная позиция не задавалась, то единица измерения бертся из основной позиции
            UnitNameComboBox.SelectedItem = catalogItem.Unit
        End If
    End Sub

    Private Sub CloseParametrButton_Click(sender As Object, e As RoutedEventArgs)
        If PositionNameTextBox.Text = "" Or CostPrice.Value <= 0 Then Exit Sub
        If IsSecondMaterialCheck.IsChecked And (SecondPositionNameTextBox.Text = "" Or SecondCostPrice.Value <= 0) Then Exit Sub
        catalogItem.Name = PositionNameTextBox.Text
        catalogItem.CostPrice = CostPrice.Value
        If IsSecondMaterialCheck.IsChecked Then
            secondcatalogItem.Name = SecondPositionNameTextBox.Text
            secondcatalogItem.CostPrice = SecondCostPrice.Value
            secondcatalogItem.Unit = UnitNameComboBox.SelectedItem
        Else
            secondcatalogItem.Name = ""
            secondcatalogItem.CostPrice = 0
            catalogItem.Unit = UnitNameComboBox.SelectedItem
        End If
        Windows.Application.Current.Dispatcher.Invoke(Calculation)
    End Sub
End Class