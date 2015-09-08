Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcHomologacion
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private p_strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property
    Public Function Insertar(ByVal strConexion As String, ByRef obeHomologacion As beHomologacion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaHomologacion As New daHomologacion
            If Not odaHomologacion.Insertar(CnnSql, obeHomologacion) Then
                Throw New Exception(odaHomologacion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function

    Public Function Actualizar(ByVal strConexion As String, ByVal obeHomologacion As beHomologacion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaHomologacion As New daHomologacion
            If Not odaHomologacion.Actualizar(CnnSql, obeHomologacion) Then
                Throw New Exception(odaHomologacion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function

    Public Function Eliminar(ByVal strConexion As String, ByVal obeHomologacion As beHomologacion, ByRef eValidacion As beValidacion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaHomologacion As New daHomologacion
            If Not odaHomologacion.Eliminar(CnnSql, obeHomologacion, eValidacion) Then
                Throw New Exception(odaHomologacion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function

    Public Function BuscarTabla(ByVal strConexion As String, ByVal obeHomologacion As beHomologacion, ByRef ListabeHomologacion As List(Of beHomologacion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaHomologacion As New daHomologacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaHomologacion.BuscarTabla(CnnSql, obeHomologacion, ListabeHomologacion) Then
                    Throw New Exception(odaHomologacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function BuscarTablaSC(ByVal strConexion As String, ByVal obeHomologacion As beHomologacion, ByRef ListabeHomologacion As List(Of beHomologacion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaHomologacion As New daHomologacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaHomologacion.BuscarTablaSC(CnnSql, obeHomologacion, ListabeHomologacion) Then
                    Throw New Exception(odaHomologacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function Listar(ByVal strConexion As String, ByRef ListabeHomologacion As List(Of beHomologacion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaHomologacion As New daHomologacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaHomologacion.Listar(CnnSql, ListabeHomologacion) Then
                    Throw New Exception(odaHomologacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

End Class
