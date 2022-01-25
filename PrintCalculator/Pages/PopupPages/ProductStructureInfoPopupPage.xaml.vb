Imports PrintCalculator.DataClasses

Class ProductStructureInfoPopupPage
    Public Sub SetParametr(_parametr As StandartOrderPage)
        ComposPerQuantityTextBox.Text = _parametr.MinPrintCopy
        For Each l As BaseOrderItem In _parametr.OrderItemList
            For Each psi In l.GetProductStructureList
                psi.Count *= _parametr.MinPrintCopy
                StructureList.Items.Add(psi)
            Next
        Next
    End Sub
End Class