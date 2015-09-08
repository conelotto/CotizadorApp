Public Class Datos
    Public Function CreaParametro(ByVal Nombre As String, ByVal Valor As Object, _
                                 ByVal Tipo As SqlDbType, Optional ByVal Size As Int32 = 0, _
                                 Optional ByVal Direccion As ParameterDirection = ParameterDirection.Input, _
                                 Optional ByVal PermiteNegativo As Boolean = False) As SqlClient.SqlParameter
        Dim Parametro As New SqlClient.SqlParameter(Nombre, Tipo)
        If Not Size.Equals(0) Then Parametro.Size = Size
        Parametro.Direction = Direccion
        Parametro.Value = System.DBNull.Value

        If Valor IsNot Nothing Then
            Dim TipoDato As String = Valor.GetType.ToString

            Select Case TipoDato
                Case "System.String"
                    If Not Valor.Equals(String.Empty) Then
                        Parametro.Value = Valor.ToString.Trim
                    End If
                Case "System.Int16", "System.Int32", "System.Int64", "System.Decimal"
                    If Valor >= 0 Or PermiteNegativo Then
                        Parametro.Value = Valor
                    End If
                Case "System.DateTime", "System.Date"
                    If Valor <> Nothing Then
                        Parametro.Value = Valor
                    End If
                Case "System.Boolean", "System.Byte[]"
                    Parametro.Value = Valor 
                Case Else
                    If Valor <> Nothing Then
                        Parametro.Value = Valor
                    End If
            End Select
        End If
        Return Parametro
    End Function

    Public Function EvaluaNull(ByVal Fila As DataRow, ByVal Campo As String) As String
        Dim Resultado As String = String.Empty
        If Fila IsNot Nothing Then
            If Fila.Table.Columns.Contains(Campo) Then
                If Not Fila.IsNull(Campo) Then
                    Resultado = Fila(Campo).ToString.Trim
                End If
            End If
        End If
        Return Resultado
    End Function
End Class