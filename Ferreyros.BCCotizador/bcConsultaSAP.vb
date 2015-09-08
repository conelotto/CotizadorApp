Imports Ferreyros.DACotizador
Imports System.Data.SqlClient
Public Class bcConsultaSAP

    Public Function ListarAdicional(ByVal strConexion As String, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Dim odaConsultaSAP As New daConsultaSAP
        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            If odaConsultaSAP.ListarAdicional(CnnSql, dtbConsulta) Then
                bolResultado = True
            End If
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
    Public Function ListarGarantia(ByVal strConexion As String, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Dim odaConsultaSAP As New daConsultaSAP
        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            If odaConsultaSAP.ListarGarantia(CnnSql, dtbConsulta) Then
                bolResultado = True
            End If
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
    Public Function ListarPlanMantenimiento(ByVal strConexion As String, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Dim odaConsultaSAP As New daConsultaSAP
        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            If odaConsultaSAP.ListarPlanMantenimiento(CnnSql, dtbConsulta) Then
                bolResultado = True
            End If
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
End Class
