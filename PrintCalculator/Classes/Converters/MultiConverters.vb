Public Class StructureUnitCountSRA3Converter
    Inherits MultiConvertorBase(Of StructureUnitCountSRA3Converter)
    Public Overrides Function Convert(values() As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object
        '0 - количество
        '1 - единица измерения
        '2 - флаг отображать ли в срках
        If values(2) Then
            If values(1).ToString.ToUpper.StartsWith("L") Then
                Return (values(0) * Workers.PaperSizeWorker.SizeInSizeCount(Workers.PaperSizeWorker.GetSheetSize(values(1)), New Size(320, 450))).ToString
            End If
        End If
        Return values(0).ToString
    End Function
End Class
Public Class StructureUnitNameSRA3Converter
    Inherits MultiConvertorBase(Of StructureUnitNameSRA3Converter)
    Public Overrides Function Convert(values() As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object
        '0 - единица измерения
        '1 - флаг отображать ли в срках
        If values(1) And values(0).ToString.ToUpper.StartsWith("L") Then
            Return "SRA3"
        End If
        Return values(0).ToString
    End Function
End Class