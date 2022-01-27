Imports WPFProjectCore

Namespace DataClasses
    ''' <summary>
    ''' Интерфейс для формул расчета тиража
    ''' </summary>
    Public Interface ICalculationFormula
        Function GetCalculationSumm(printCopyCount As Integer, costPriceForOne As Double) As Double
    End Interface
    ''' <summary>
    ''' Базовая формула для расчета типографских тиражей
    ''' </summary>
    Public Class StandartCalculationFormula
        Implements ICalculationFormula
#Region "Реализация интерфейса"
        ''' <summary>
        ''' Возвращает стоимость тиража
        ''' </summary>
        ''' <param name="printCopyCount"></param>
        ''' <param name="costPriceForOne"></param>
        ''' <returns>
        ''' % наценки = 150 - сумма себестоимости/150 (если результат меньше 40, то ставим 40)
        ''' </returns>
        Public Function GetCalculationSumm(printCopyCount As Integer, costPriceForOne As Double) As Double Implements ICalculationFormula.GetCalculationSumm
            Dim result As Double = 0
            Dim costPriceForAll As Double = printCopyCount * costPriceForOne
            Dim percent As Double = 115 - costPriceForAll / 115
            If percent < 40 Then percent = 40
            result = costPriceForAll + costPriceForAll / 100 * percent
            Return Math.Round(result)
        End Function
#End Region
    End Class
    ''' <summary>
    ''' Формула для расчета печати сигнальника
    ''' </summary>
    Public Class SignalCalculationFormula
        Implements ICalculationFormula
#Region "Реализация интерфейса"
        ''' <summary>
        ''' Возвращает стоимость тиража
        ''' </summary>
        ''' <param name="printCopyCount"></param>
        ''' <param name="costPriceForOne"></param>
        ''' <returns>
        ''' При себестоимости меньше 1000 р., добавляется 300 р. к сумме на приладку и затраты времени оменеджера
        ''' </returns>
        Public Function GetCalculationSumm(printCopyCount As Integer, costPriceForOne As Double) As Double Implements ICalculationFormula.GetCalculationSumm
            Dim result As Double = 0
            result = printCopyCount * costPriceForOne * 2.5
            If result < 1000 Then result = result + 300
            Return Math.Round(result)
        End Function
#End Region
    End Class
End Namespace