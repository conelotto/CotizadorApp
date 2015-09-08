Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador

Public Class daAprobadorUsuario
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function Buscar(ByVal cnnSql As SqlConnection, ByVal eAprobadorUsuario As beAprobadorUsuario, _
                           ByRef dstAprobadorUsuario As DataSet, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = objBBDD.StoreProcedure.AprobadorUsuarioBuscar
            cmdSql.CommandType = CommandType.StoredProcedure
            With eAprobadorUsuario
                cmdSql.Parameters.Add(uData.CreaParametro("@IdAprobador", .IdAprobador, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Estado", .Estado, SqlDbType.VarChar, 1))
                Dim adpSql As New SqlDataAdapter(cmdSql)
                If dstAprobadorUsuario Is Nothing Then
                    dstAprobadorUsuario = New DataSet
                End If
                adpSql.Fill(dstAprobadorUsuario, Entidad.AprobadorUsuario.ToString)
            End With
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarPorMatricula(ByVal cnnSql As SqlConnection, ByVal eAprobadorUsuario As beAprobadorUsuario, _
                                       ByRef dstAprobadorUsuario As DataSet, Optional ByVal trnSql As SqlTransaction = Nothing) As Boolean
        blnValido = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand
            If trnSql IsNot Nothing Then
                cmdSql.Transaction = trnSql
            End If
            cmdSql.CommandText = objBBDD.StoreProcedure.AprobadorUsuarioBuscarMatricula
            cmdSql.CommandType = CommandType.StoredProcedure
            With eAprobadorUsuario
                cmdSql.Parameters.Add(uData.CreaParametro("@MatriculaUsuario", .MatriculaUsuario, SqlDbType.VarChar, 4))
                cmdSql.Parameters.Add(uData.CreaParametro("@Estado", .Estado, SqlDbType.VarChar, 1))
                Dim adpSql As New SqlDataAdapter(cmdSql)
                If dstAprobadorUsuario Is Nothing Then
                    dstAprobadorUsuario = New DataSet
                End If
                adpSql.Fill(dstAprobadorUsuario, Entidad.AprobadorUsuario.ToString)
            End With
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

End Class