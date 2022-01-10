Imports System.Windows.Controls.Primitives
Imports PrintCalculator.DataClasses
Class CatalogItemSelectionPopupPage
    Dim CatalogListSource As CollectionViewSource
    Dim SecondCatalogListSource As CollectionViewSource
    Private Calculation As [Delegate]
    Dim catalogItem As CatalogItem
    Dim IsInsertFilter As Boolean = True
#Region "Загрузка окна"
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        'Устанавливаем фокус в поле посика
        FindTextBox.Focus()
    End Sub
    ''' <summary>
    ''' Установка стартовых параметров
    ''' </summary>
    Public Sub SetParametr(_catalogItem As CatalogItem, calculationSub As [Delegate])
        'Задаем флаг входящей фильтрацйии. Если он True, то каталог дополнительно фильтруется по разделу и тегу
        IsInsertFilter = True
        'Определяем ссылки на каталоги, для фильтрации
        CatalogListSource = TryFindResource("CatalogSource")
        SecondCatalogListSource = TryFindResource("SecondCatalogSource")
        'Сохраняем делегат, который нужно вызвать по завершению
        Calculation = calculationSub
        'Созраняем ссылку на текущю составную часть заказа
        catalogItem = _catalogItem
        'Определяем категорю позиций отображаемых в каталоге
        ItemCategory = _catalogItem.ItemCategory
        ItemTag = _catalogItem.ItemTag
        AddHandler CatalogListSource.Filter, AddressOf FilterCatalog
    End Sub
    Public Sub SetParametr(_catalogItem As SinglePositionInOrder, calculationSub As [Delegate])
        'Задаем флаг входящей фильтрацйии. Если он True, то каталог дополнительно фильтруется по разделу и тегу
        IsInsertFilter = False
        'Определяем ссылки на каталоги, для фильтрации
        CatalogListSource = TryFindResource("CatalogSource")
        SecondCatalogListSource = TryFindResource("SecondCatalogSource")
        'Сохраняем делегат, который нужно вызвать по завершению
        Calculation = calculationSub
        'Созраняем ссылку на текущю составную часть заказа
        catalogItem = _catalogItem.BasicCatalogItem
        'AddHandler CatalogListSource.Filter, AddressOf FilterCatalog
    End Sub
#End Region

#Region "Свойства"
#Region "Внутренние"
#End Region
    ''' <summary>
    ''' Категория выбираемой позиции каталога
    ''' </summary>
    ''' <returns></returns>
    Public Property ItemCategory As CatalogItem.ItemCategoryEnum = CatalogItem.ItemCategoryEnum.NONE
    ''' <summary>
    ''' Тэг подраздела
    ''' </summary>
    ''' <returns></returns>
    Public Property ItemTag As String
#End Region
#Region "Поиск по каталогу"
    ''' <summary>
    ''' Возникает при изменении текста поиска
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FindTextBox_TextChanged(sender As TextBox, e As TextChangedEventArgs)
        'Добавляем ссылку на событие фильстрации
        AddHandler CatalogListSource.Filter, AddressOf FilterCatalog
    End Sub
    ''' <summary>
    ''' Происходит при нажатии на кнопку очиски текста фильтрации
    ''' </summary>
    Private Sub ClearFindTextButton_Click()
        'Очищаем текст фильтрации
        FindTextBox.Text = ""
    End Sub
    ''' <summary>
    ''' Фильтрует каталог по заданным параметрам
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FilterCatalog(sender As Object, e As FilterEventArgs)
        'Определяем текущую проверяемую позицию каталога
        Dim citem As CatalogItem = CType(e.Item, CatalogItem)
        'Если она не пустая
        If Not (citem Is Nothing) Then
            'Определяем переменную результата фильтрации (да/нет)
            Dim result As Boolean = True
            If IsInsertFilter Then
                'Проверяем соответсвуют ли текущая позиция каталога фильтрации по категории и тегу
                result = result AndAlso citem.ItemCategory = ItemCategory Or (ItemCategory = CatalogItem.ItemCategoryEnum.NONE)
                result = result AndAlso citem.ItemTag = ItemTag Or (ItemTag = "")
            End If
            'Разбиваем строку поиска на слова
            Dim strmass() As String = FindTextBox.Text.ToLower.Split(" ".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            'Проходим по массиву слов и проверяем удовлетворяет ли поиск данным словам
            For Each x In strmass
                'Суммируем резултат
                result = result AndAlso citem.Name.ToLower.IndexOf(x) > -1 Or citem.GroupCode.ToLower.IndexOf(x) > -1
            Next
            'Возвращаем результат
            e.Accepted = result
        End If
    End Sub
    ''' <summary>
    ''' Происходит при нажатии клввишь в строке поиска
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FindTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            If CatalogListBox.Items.Count > 0 Then
                CatalogListBox.SelectedIndex = 0
                SelectedItem()
            End If
        End If
    End Sub
#End Region

#Region "Работа с позициями позиции"
    Private Sub CatalogListBoxContextMenu_AddFavorite(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub CatalogListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If SecondCatalogListBox Is Nothing Then Exit Sub
        If CType(CatalogListBox.SelectedItem, CatalogItem).ItemCategory = CatalogItem.ItemCategoryEnum.SERVICE Then
            SecondCatalogListBox.Visibility = Visibility.Visible
        Else
            SecondCatalogListBox.Visibility = Visibility.Collapsed
        End If
    End Sub
#End Region
#Region "Фиксация выбора"
    ''' <summary>
    ''' Возникает при нажатии кнопки выбора
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectButton_Click(sender As Object, e As RoutedEventArgs)
        SelectedItem()
    End Sub
    ''' <summary>
    ''' Вызывается элементами выбора позиции каталога
    ''' </summary>
    Private Sub SelectedItem()
        'Если в каталоге есть выбранный элемент...
        If CatalogListBox.SelectedIndex > -1 Then
            'Удаляем ссылку на фильтрацию (для будущих открытий окна)
            RemoveHandler CatalogListSource.Filter, AddressOf FilterCatalog
            'Сохраняем выбранную позицию
            catalogItem.SetPropertys(CatalogListBox.SelectedItem)
            'Очищаем поле поиска
            ClearFindTextButton_Click()
            'Вызываем переданную процедуру завершения
            Windows.Application.Current.Dispatcher.Invoke(Calculation)
        End If
    End Sub
#End Region
End Class