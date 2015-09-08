Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Public Class bcArchivoConfiguracion
    Private p_strError As String

    Public Function Insertar(ByVal strConexion As String, ByRef obeArchivoConfiguracion As beArchivoConfiguracion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            If Not odaArchivoConfiguracion.Insertar(CnnSql, obeArchivoConfiguracion) Then
                Throw New Exception(odaArchivoConfiguracion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Function Actualizar(ByVal strConexion As String, ByVal obeArchivoConfiguracion As beArchivoConfiguracion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            If Not odaArchivoConfiguracion.Actualizar(CnnSql, obeArchivoConfiguracion) Then
                Throw New Exception(odaArchivoConfiguracion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Function Eliminar(ByVal strConexion As String, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, ByRef eValidacion As beValidacion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            If Not odaArchivoConfiguracion.Eliminar(CnnSql, obeArchivoConfiguracion, eValidacion) Then
                Throw New Exception(odaArchivoConfiguracion.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Function BuscarPorCriterio(ByVal strConexion As String, ByVal obeArchivoConfiguracion As beArchivoConfiguracion, ByRef lista As List(Of beArchivoConfiguracion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarPorCriterio(CnnSql, obeArchivoConfiguracion, lista) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
    Public Function BuscarId(ByVal strConexion As String, ByRef obeArchivoConfiguracion As beArchivoConfiguracion) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarId(CnnSql, obeArchivoConfiguracion) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
    Public Function BuscarArchivoSeccion(ByVal strConexion As String, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarArchivoSeccion(CnnSql, obeTablaMaestra, lista) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
    Public Function BuscarArchivoProductoGeneral(ByVal strConexion As String, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarArchivoProductoGeneral(CnnSql, obeTablaMaestra, lista) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function BuscarArchivoProducto(ByVal strConexion As String, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarArchivoProducto(CnnSql, obeTablaMaestra, lista) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
    Public Function BuscarIdSeccionCriterio(ByVal strConexion As String, ByVal obeTablaMaestra As beTablaMaestra, ByRef lista As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarIdSeccionCriterio(CnnSql, obeTablaMaestra, lista) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
    Public Function BuscarCodigoYSeccion(ByVal strConexion As String, ByRef obeArchivoConfiguracion As beArchivoConfiguracion, ByRef listaArchivoConfiguracion As List(Of beArchivoConfiguracion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaArchivoConfiguracion As New daArchivoConfiguracion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaArchivoConfiguracion.BuscarCodigoYSeccion(CnnSql, obeArchivoConfiguracion, listaArchivoConfiguracion) Then
                    Throw New Exception(odaArchivoConfiguracion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
End Class
