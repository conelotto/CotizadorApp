
Imports System.Data.SqlClient
''' <summary>
''' Consulta que deben consumirse desde un servicio SAP
''' </summary>
''' <remarks></remarks>
Public Class daConsultaSAP

    Public Function ListarAdicional(ByVal CnnSql As SqlConnection, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Try

            Dim sqlQuery As String = "select IdAdicional,Nombre, CodigoModelo ,Tipo from Adicional"
            Dim cmm As SqlCommand = CnnSql.CreateCommand()
            cmm.CommandType = CommandType.Text
            cmm.CommandText = sqlQuery
            Dim da As New SqlDataAdapter(cmm)
            da.Fill(dtbConsulta)
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
    Public Function ListarGarantia(ByVal CnnSql As SqlConnection, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Try

            'Dim sqlQuery As String = "select IdGarantia,(cast(IdGarantia as varchar(10)) + Nombre) as Nombre,IdModelo, Meses,Horas,ValorCosto,ValorLista from Garantia "
            Dim sqlQuery As String = "select IdGarantia, Nombre, CodigoModelo, Meses,Horas,ValorCosto,ValorLista from Garantia "
            Dim cmm As SqlCommand = CnnSql.CreateCommand()
            cmm.CommandType = CommandType.Text
            cmm.CommandText = sqlQuery
            Dim da As New SqlDataAdapter(cmm)
            da.Fill(dtbConsulta)
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
    Public Function ListarPlanMantenimiento(ByVal CnnSql As SqlConnection, ByRef dtbConsulta As DataTable) As Boolean
        Dim bolResultado As Boolean
        Try

            Dim sqlQuery As String = "select IdPlanMantenimiento,PlanMantenimiento,CodigoModelo,ValorCosto,ValorLista from PlanMantenimiento  "
            Dim cmm As SqlCommand = CnnSql.CreateCommand()
            cmm.CommandType = CommandType.Text
            cmm.CommandText = sqlQuery
            Dim da As New SqlDataAdapter(cmm)
            da.Fill(dtbConsulta)
        Catch ex As Exception

        End Try
        Return bolResultado
    End Function
End Class
