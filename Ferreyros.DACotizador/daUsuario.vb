Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador
Imports System.Text.StringBuilder
Imports System.Text

Public Class daUsuario

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Private spConsulta As New StringBuilder()



    Public Function Buscar(ByVal cnnSql As SqlConnection, ByVal Compania As String, ByVal ValorBusqueda As String, _
                           ByRef dstUsuario As DataSet, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean

        spConsulta = New StringBuilder()
        spConsulta.Append("Select top 15 Matricula as MatriculaUsuario, ")
        spConsulta.Append("Nombre as NombreUsuario ")
        spConsulta.Append("From v_trabajadores_todos Where ")
        spConsulta.Append("compania= '" & Compania & "' and ")
        spConsulta.Append("Matricula like '" & ValorBusqueda & "%'")

        blnValido = False
        strError = String.Empty
        Try

            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = spConsulta.ToString
            cmdSql.CommandType = CommandType.Text
            Dim adpSql As New SqlDataAdapter(cmdSql)
            If dstUsuario Is Nothing Then
                dstUsuario = New DataSet
            End If
            adpSql.Fill(dstUsuario, 0, 20, "MatriculaUsuario")

            spConsulta = New StringBuilder()
            spConsulta.Append("Select top 15 Matricula as MatriculaUsuario, ")
            spConsulta.Append("Nombre as NombreUsuario ")
            spConsulta.Append("From v_trabajadores_todos Where ")
            spConsulta.Append("compania= '" & Compania & "' and ")
            spConsulta.Append("Nombre like '%" & ValorBusqueda & "%'")

            adpSql.SelectCommand.CommandText = spConsulta.ToString

            adpSql.Fill(dstUsuario, 0, 20, "NombreUsuario")

            If dstUsuario.Tables(1).Rows.Count > 0 Then
                dstUsuario.Tables(0).Merge(dstUsuario.Tables(1), True)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function Buscar(ByVal cnnSql As SqlConnection, ByVal eAprobadorUsuario As beAprobadorUsuario, _
                           ByRef dtrUsuario As DataRow, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        Dim strFiltro As String = String.Format("Select Matricula as MatriculaUsuario, Nombre as NombreUsuario,isnull(email_trabajo,'') as CorreoUsuario, compania " & _
                                                "From v_trabajadores_todos Where Matricula ='{0}' ", eAprobadorUsuario.MatriculaUsuario)
        Dim dstUsuario As New DataSet
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = strFiltro
            cmdSql.CommandType = CommandType.Text
            Dim adpSql As New SqlDataAdapter(cmdSql)
            adpSql.Fill(dstUsuario, "Usuario")
            If Not dstUsuario.Tables(0).Rows.Count.Equals(0) Then
                dtrUsuario = dstUsuario.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function


End Class
