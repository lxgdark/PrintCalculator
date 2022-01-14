Imports System.IO
Imports Microsoft.Win32
Imports System.Xml.Serialization
Imports PrintCalculator.DataClasses
Imports PrintCalculator.Workers
Imports Xceed.Wpf.Toolkit
Class OrderPage
    Private MeContext As StandartOrderPage
    Private Delegate Sub CalculationDelegate()
    Dim isPageOpen As Boolean = False
#Region "Общее"
    ''' <summary>
    ''' Загрузка страницы
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        'Задоем контекст данных
        MeContext = Me
        DataContext = MeContext
        'Если страница уже открыта, то выходим из процедуры
        If MeContext.isPageOpen Then Exit Sub
        MeContext.isPageOpen = True
        'Подписываемся на изменение коллекции вызывая пересчет заказа
        AddHandler MeContext.OrderItemList.CollectionChanged, AddressOf Calculation
        'Добавляем стартовую составную часть, если это не копия другой страницы
        If Not MeContext.IsCopyPage Then AddStandardOrderItemButton_Click()
        Calculation()
        'Добавляем стартовый расчет тиража
        AddPrintCopyCalculationButton_Click()
    End Sub
    ''' <summary>
    ''' Размер колонок на страницу
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Thumb_DragDelta(sender As Object, e As Controls.Primitives.DragDeltaEventArgs)
        Dim p As Double = e.HorizontalChange * 100 / GlobalGrid.ActualWidth
        Dim w As Double = MeContext.LeftPanelWidth.Value + Math.Round(p, 2)
        If w > 65 Then
            w = 65
        ElseIf w < 40 Then
            w = 40
        End If
        MeContext.LeftPanelWidth = New GridLength(w, GridUnitType.Star)
        MeContext.RightPanelWidth = New GridLength(100 - w, GridUnitType.Star)
    End Sub
    ''' <summary>
    ''' Производит матиматические операции внутри полей ввода
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub UpDown_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            sender.Value = OtherFunctions.GetFormulaRezult(sender.Text.Replace(",", "."))
        End If
    End Sub
#End Region
#Region "Работа с составными частями"
    ''' <summary>
    ''' Добавление стандартной составной части
    ''' </summary>
    Private Sub AddStandardOrderItemButton_Click()
        'Добавляем стандартную составную часть
        Dim soi As New StandartOrderItem
        'Добавляем в стартовую составную часть позиции каталога по умолчанию (для сокращения времени работы менеджера)
        For Each item In My.AppCore.CatalogList
            If item.Name = "Печать 4+0" Then soi.PrintItem.SetPropertys(item)
            If item.Name = "Резка в размер" Then soi.CutItem.SetPropertys(item)
        Next
        MeContext.OrderItemList.Add(soi)
        'Вызываем стартовый просчет внутри составной части
        soi.Calculation()
    End Sub
    ''' <summary>
    ''' Происходит при нажатии правой кнопки мыши на состанв
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OrderItem_PreviewMouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        'Открываем контекстое меню работы с составной частью
        OrderItemListContextMenu.Tag = sender.Tag
        OrderItemListContextMenu.IsOpen = True
    End Sub
    ''' <summary>
    ''' Происходит при нажатии пункта меню "Копировать" в списке составных частей
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OrderItemListContextMenu_ClickItemCopy(sender As Object, e As RoutedEventArgs)
        'Добавляем стандартную составную часть
        Dim boi As BaseOrderItem
        'Извлекаем наименование класса составной части
        Dim T() As String = OrderItemListContextMenu.Tag.GetType.ToString.Split(".".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        boi = StandartOrderPage.GetOrderItemByType(T(T.Length - 1))
        'Копируем значения из копируемой составной части
        boi.SetPropertys(OrderItemListContextMenu.Tag)
        MeContext.OrderItemList.Add(boi)
        'Вызываем стартовый просчет внутри составной части
        boi.Calculation()
    End Sub
    ''' <summary>
    ''' Происходит при нажатии пункта меню "Удалить" в списке составных частей
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub OrderItemListContextMenu_ClickItemKill(sender As MenuItem, e As RoutedEventArgs)
        If Await My.MessageWorker.ShowMessage("Удалить составную часть?",, MessageWorker.GetStandartYesNoOptions) Then
            MeContext.OrderItemList.Remove(OrderItemListContextMenu.Tag)
        End If
    End Sub
#End Region
#Region "Стандартная составная часть"
    ''' <summary>
    ''' Настройка размера бумаги
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PrintPaperSizeParametrButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New PaperSizeParametrPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).PrintPaperSize, False, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Задание персональной бумаги, отсутствующей в каталоге
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SetPersonalPaper_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CreatePersonalPaperPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).PaperItem, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Настройка размера изделия
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ProductSizeParametrButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New PaperSizeParametrPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).ProductSize, True, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Открытие предпросмотра раскладки
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ProductLayoutButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New ProductLayoutPopupPage
        page.SetParametr(sender.Tag)
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Выбор бумаги
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectPaperButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CatalogItemSelectionPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).PaperItem, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Выбр типа печати
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectPrintButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CatalogItemSelectionPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).PrintItem, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Выбор типа резки
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectCutButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CatalogItemSelectionPopupPage
        page.SetParametr(CType(sender.Tag, StandartOrderItem).CutItem, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
#Region "Доп. позиции стандартной составной части"
    ''' <summary>
    ''' Добвавление новой доп. позиции
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddDopItemButton_Click(sender As Object, e As RoutedEventArgs)
        Dim sop As New SinglePositionInOrder
        CType(sender.Tag, StandartOrderItem).OtherOrderPositionList.Add(sop)
        Calculation()
        SelectOthetCatalogItemButton_Click(New Button With {.Tag = sop}, Nothing)
        OrderItemsScrollViewer.ScrollToEnd()
    End Sub
    ''' <summary>
    ''' Выбор доп. позиции в каталоге
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectOthetCatalogItemButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CatalogItemSelectionPopupPage
        page.SetParametr(sender.Tag, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Удаление доп. позиции каталога
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub KillOthetCatalogItemButton_Click(sender As Object, e As RoutedEventArgs)
        If Await My.MessageWorker.ShowMessage("Удалить доп. обработку?",, MessageWorker.GetStandartYesNoOptions) Then
            For Each l In MeContext.OrderItemList
                If TypeOf l Is StandartOrderItem Then
                    CType(l, StandartOrderItem).OtherOrderPositionList.Remove(sender.Tag)
                    Calculation()
                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' Открытие помощника в расчете количества доп. позиции
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub СalculationOthetCatalogItemButton_Click(sender As Object, e As RoutedEventArgs)
        'В разработке
    End Sub
#End Region
#End Region
#Region "Отдельная составная часть"
    Private Sub AddOneCatalogOrderItemButton_Click(sender As Object, e As RoutedEventArgs)
        'Добавляем отдельную составную часть
        Dim ocpoi As New SingleOrderItem
        MeContext.OrderItemList.Add(ocpoi)
        'Вызываем открытие каталога для выбора значения
        SelectOneCatalogItemButton_Click(New Button With {.Tag = ocpoi}, Nothing)
        ''Вызываем стартовый просчет внутри составной части
        ocpoi.Calculation()
        OrderItemsScrollViewer.ScrollToEnd()
    End Sub
    ''' <summary>
    ''' Поисходит при нажатии выбора позиции из каталога
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectOneCatalogItemButton_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CatalogItemSelectionPopupPage
        page.SetParametr(sender.Tag.Item, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
    ''' <summary>
    ''' Происходит при нажатии кнопки расчета количества в отдельной позиции
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub СalculationOneCatalogItemButton_Click(sender As Object, e As RoutedEventArgs)
        'В разработке
    End Sub

#End Region
#Region "Дополнительные варианты составных частей"
    ''' <summary>
    ''' Происходит при нажатии кнопки открытия дополнительных вариантов составныхч частей
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddOtherOrderItemButton_Click(sender As Object, e As RoutedEventArgs)
        OtherOrderItemPopupList.IsOpen = True
    End Sub

    Private Sub AddPersonalOrderItemButton_Click(sender As Object, e As RoutedEventArgs)
        'Добавляем отдельную составную часть
        Dim ocpoi As New SingleOrderItem
        ocpoi.Item.IsPersonalItem = True
        ocpoi.ItemHeader = "Отдельная составная часть заданная вручную"
        MeContext.OrderItemList.Add(ocpoi)
        'Вызываем открытие страницы создания/редактирования новой позиции
        SetPersonalItem_Click(New Button With {.Tag = ocpoi}, Nothing)
        'Вызываем стартовый просчет внутри составной части
        ocpoi.Calculation()
        OrderItemsScrollViewer.ScrollToEnd()
        OtherOrderItemPopupList.IsOpen = False
    End Sub

    Private Sub SetPersonalItem_Click(sender As Object, e As RoutedEventArgs)
        Dim page As New CreatePersonalCatalogItemPopupPage
        page.SetParametr(sender.Tag.Item, New CalculationDelegate(AddressOf Calculation))
        OrderItemParameterFrame.Content = page
        OrderItemParameterPopup.IsOpen = True
    End Sub
#End Region
#Region "Расчет тиража"
    ''' <summary>
    ''' Добавление нового расчета для тиража
    ''' </summary>
    Public Sub AddPrintCopyCalculationButton_Click()
        MeContext.PrintCopyCountList.Add(New PrintCopyCountItem With {.CurrentCalculationFormula = New StandartCalculationFormula, .CostPriceForOne = MeContext.ProductCostPrice, .MinPrintCount = MeContext.MinPrintCopy})
    End Sub
    ''' <summary>
    ''' Удаление расчета для тиража
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PrintCopyCountItem_PreviewMouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        MeContext.PrintCopyCountList.Remove(sender.Tag)
    End Sub
#End Region
#Region "Действия с расчетом"
    ''' <summary>
    ''' Сохранение текущего расчета
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub SaveOrderButton_Click(sender As Object, e As RoutedEventArgs)
        'Создаем и настраиваем диалог сохранения файла
        Dim sfd As New SaveFileDialog With {
            .Title = "Сохранить расчет",
            .Filter = "Файл расчета (*.tpc)|*.tpc"
        }
        'Находм в списке вкладок
        For Each gpl In My.AppCore.GlobalPagesList
            If gpl.OrderObject Is Me Then
                sfd.FileName = gpl.Header
            End If
        Next
        If Not sfd.ShowDialog() Then Exit Sub
        'Флаг для отслеживания ошибки открытия файла
        Dim iserror As Boolean = False
        Try
            'Переносим сохраняемые данные в соответствующий класс
            Dim s As New SavedOrderObject
            For Each oi In MeContext.OrderItemList
                s.OrderItemList.Add(oi)
            Next
            'Сериалезуем сохраняемый класс
            Dim SavePath As String = sfd.FileName
            If Not IO.File.Exists(SavePath) Then
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath))
            End If
            Dim writer As New XmlSerializer(s.GetType)
            Dim file As New StreamWriter(SavePath)
            writer.Serialize(file, s)
            file.Close()
        Catch ex As Exception
            'Если произошла ошибка, то устанвливаем флаг
            iserror = True
        End Try
        'Если флаг ошибки положительный выдаем соответствующее сообщение
        If iserror Then Await My.MessageWorker.ShowMessage("При сохранении файла произошла ошибка!",, MessageWorker.GetTopMostErrorOptions)
    End Sub
    ''' <summary>
    ''' Копирование расчета на новую вкладку
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CopyOrderButton_Click(sender As Object, e As RoutedEventArgs)
        'Создаем новый объект стандартного расчета
        Dim newSOP As New StandartOrderPage
        'Переносим нужные параметры
        newSOP.SetPropertys(MeContext)
        'Указываем, что это копия
        newSOP.IsCopyPage = True
        'Добавляем в глобальный список страниц новую
        My.AppCore.GlobalPagesList.Add(New GlobalPageWorker With {.Header = "Новый расчет", .IsStartPage = False, .OrderObject = newSOP})
    End Sub
#End Region
    ''' <summary>
    ''' Основная процедура проводящаая расчет заказа
    ''' </summary>
    Public Sub Calculation()
        'Закрываем всплывающее окно
        OrderItemParameterPopup.IsOpen = False
        'Очищаем свойства расчета
        MeContext.ClearPropertys()
        'Проходим по составным частям
        For Each item In MeContext.OrderItemList
            'Вызываем в составных частях процедуру просчета
            item.Calculation()
        Next
        For Each item In MeContext.OrderItemList
            'Проверяем валидность указанных данных в составных частях
            If item.GetIsValidCostPrice Then
                'Если данные валидны, то вычисляем параметры расчета
                MeContext.MinPrintCopy = IIf(item.GetProductCount > MeContext.MinPrintCopy, item.GetProductCount, MeContext.MinPrintCopy)
                MeContext.MinCostPrice += item.GetProductCostPrice * item.GetProductCount
                MeContext.ProductCostPrice += item.GetProductCostPrice
            Else
                'Если хотя бы одна составная часть имеет не все данные для расчета, то обнуляем расчетные данные и выходим из процедуры
                MeContext.ClearPropertys()
                Exit For
            End If
        Next
        'Расчитываем значение для сигнальной формулы
        Dim sinalFormulas As New SignalCalculationFormula
        MeContext.MinPrice = sinalFormulas.GetCalculationSumm(MeContext.MinPrintCopy, MeContext.ProductCostPrice)
        'Расчтываем значения для всего списка тиражей
        For Each pccl In MeContext.PrintCopyCountList
            pccl.CostPriceForOne = MeContext.ProductCostPrice
            pccl.MinPrintCount = MeContext.MinPrintCopy
        Next
    End Sub

End Class