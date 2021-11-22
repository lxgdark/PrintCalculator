Imports System.Globalization
Imports PrintCalculator.Workers
''' <summary>
''' Конвертирует английское название текущей страницы в русское
''' </summary>
Public Class PageEngTitleToRusConverter
    Inherits ConvertorBase(Of PageEngTitleToRusConverter)
    Public Overrides Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object
        Select Case CType(value, AppCore.CurentSelectedPageEnum)
            Case AppCore.CurentSelectedPageEnum.Home
                Return "Главная"
            Case AppCore.CurentSelectedPageEnum.Settings
                Return "Настройки приложения"
            Case AppCore.CurentSelectedPageEnum.Theme
                Return "Настройки цвета"
            Case Else
                Return "Неизвестная страница"
        End Select
    End Function
End Class
''' <summary>
''' Конвертирует код раздела каталога в понятное наименование
''' </summary>
Public Class CataloGroupCodeToNameConverter
    Inherits ConvertorBase(Of CataloGroupCodeToNameConverter)
    Public Overrides Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object
        Dim str As String() = value.ToString.Split("$".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        If str.Length = 4 Then
            Return str(3)
        Else
            Return value
        End If
    End Function
End Class