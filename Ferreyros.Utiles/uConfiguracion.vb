Imports System
Imports System.Configuration

Public Class uConfiguracion

    Public Sub extraerAppSettings(ByVal key As String, ByRef result As String)

        If String.IsNullOrEmpty(key) Then
            Return
        End If

        result = System.Configuration.ConfigurationManager.AppSettings(key).ToString

    End Sub

    Public Function fc_ConvertirFecha(ByVal fecha As String) As String

        Dim lResult As String = Nothing
        Dim vDia As String = String.Empty
        Dim vMes As String = String.Empty
        Dim vAnho As String = String.Empty

        If Not String.IsNullOrEmpty(fecha) AndAlso fecha.Length = 8 Then
             
            Dim numero As Integer = 0
            Try
                numero = CInt(fecha)
            Catch ex As Exception
                lResult = Nothing
                Return lResult
            End Try
            If numero > 0 Then
                vAnho = Left(fecha, 4)
                vMes = Mid(fecha, 5, 2)
                vDia = Right(fecha, 2)
                lResult = String.Concat(vAnho, "-", vMes, "-", vDia)
            End If
        End If

        Return lResult

    End Function

    Public Function fc_ConvertirFechaSAP(ByVal fecha As String) As String

        Dim lResult As String = Nothing
        Dim vDia As String = String.Empty
        Dim vMes As String = String.Empty
        Dim vAnho As String = String.Empty

        If IsDate(fecha) Then
            vAnho = Year(CDate(fecha))
            vMes = Month(CDate(fecha))
            vDia = Day(CDate(fecha))
            If vMes.Length = 1 Then
                vMes = "0" + vMes
            End If
            If vDia.Length = 1 Then
                vDia = "0" + vDia
            End If
        End If

        lResult = vAnho + vMes + vDia

        Return lResult

    End Function

    Public Function fc_ConvertirBoolean(ByVal valor As String) As Boolean

        Dim lResult As Boolean = False

        'If Not String.IsNullOrEmpty(valor) AndAlso (UCase(valor) = "SI" OrElse UCase(valor) = "YES" OrElse UCase(valor) = "TRUE" OrElse valor = "1") Then
        '    lResult = True
        'End If
        If Not String.IsNullOrEmpty(valor) Then
            Select Case valor.ToUpper()
                Case "SI", "YES", "TRUE", "1", "01"
                    lResult = True
                Case Else
                    lResult = False
            End Select
        End If
        
        Return lResult

    End Function

    Public Function fc_ConvertirBooleanSAP(ByVal valor As String) As String

        Dim lResult As String = "NO"

        If Not String.IsNullOrEmpty(valor) AndAlso CBool(valor) Then
            lResult = "SI"
        End If

        Return lResult

    End Function

    Public Function fc_cadenaAleatoria() As String

        Dim lResult As String = String.Empty
        Dim objRandom As New Random
    
        For i As Int16 = 0 To 8
            lResult = String.Concat(lResult, objRandom.Next(0, 10).ToString())
        Next

        Return lResult

    End Function

    Public Function fc_ConvertirEntero(ByVal valor As String) As Integer

        Dim lResult As Integer = 0

        Try
            If valor Is Nothing Then
                Return lResult
            End If

            If String.IsNullOrEmpty(valor) OrElse Not IsNumeric(valor) Then
                Return lResult
            Else
                Return Int(valor)
            End If
        Catch ex As Exception

        End Try
        Return lResult

    End Function

    Public Function fc_ConvertirDouble(ByVal valor As String) As Double

        Dim lResult As Double = 0

        Try
            If valor Is Nothing Then
                Return lResult
            End If

            If String.IsNullOrEmpty(valor) OrElse Not IsNumeric(valor) Then
                Return lResult
            Else
                Try
                    lResult = CDbl(valor)
                Catch ex As Exception
                    lResult = 0
                End Try
            End If
        Catch ex As Exception

        End Try
        Return lResult
    End Function

End Class
