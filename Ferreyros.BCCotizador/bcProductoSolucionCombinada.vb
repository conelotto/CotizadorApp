Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoSolucionCombinada
    Private p_strError As String

    Public Function BuscarId(ByVal strConexion As String, ByRef obeProducto As beProducto) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoSolucionCombinada As New daProductoSolucionCombinada
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoSolucionCombinada.BuscarId(CnnSql, obeProducto) Then
                    Throw New Exception(odaProductoSolucionCombinada.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function DevolverCampos(ByVal strConexion As String, ByVal tablas As String, ByRef condiciones As String) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoSolucionCombinada As New daProductoSolucionCombinada
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoSolucionCombinada.DevolverCampos(CnnSql, tablas, condiciones) Then
                    Throw New Exception(odaProductoSolucionCombinada.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function DevolverCondicionesMarcadores(ByVal strConexion As String, ByVal Marcadores As String, ByRef condiciones As String) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoSolucionCombinada As New daProductoSolucionCombinada
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoSolucionCombinada.DevolverCondicionesMarcadores(CnnSql, Marcadores, condiciones) Then
                    Throw New Exception(odaProductoSolucionCombinada.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function DevolverData(ByVal strConexion As String, ByVal tablas As String, ByVal datos As String, ByRef data As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoSolucionCombinada As New daProductoSolucionCombinada
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoSolucionCombinada.DevolverData(CnnSql, tablas, datos, data) Then
                    Throw New Exception(odaProductoSolucionCombinada.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function DevolverDataMarcadores(ByVal strConexion As String, ByVal Marcadores As String, ByVal datos As String, ByRef data As DataTable) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoSolucionCombinada As New daProductoSolucionCombinada
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoSolucionCombinada.DevolverDataMarcadores(CnnSql, Marcadores, datos, data) Then
                    Throw New Exception(odaProductoSolucionCombinada.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Sub SolCombinadaListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beSolucionCombinada))

        Try
            Dim dSolucionCombinada = New daSolucionCombinada
            dSolucionCombinada.SolucionCombinadaListar(eConexion, eCotizacion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
End Class
