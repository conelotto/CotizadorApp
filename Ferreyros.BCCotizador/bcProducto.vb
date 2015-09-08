Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador
Imports System.Data.SqlClient
Public Class bcProducto

    Private dProducto As daProducto = Nothing
    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Sub ProductoListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beProducto))

        Try
            dProducto = New daProducto
            dProducto.ProductoListar(eConexion, eCotizacion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Function BuscarIdCotizacion(ByVal strConexion As String, ByVal obeProducto As beProducto, ByRef ListabeProducto As List(Of beProducto)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProducto As New daProducto
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProducto.BuscarIdCotizacion(CnnSql, obeProducto, ListabeProducto) Then
                    Throw New Exception(odaProducto.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function

    Public Function BuscarNumeroCotizacion(ByVal strConexion As String, ByVal obeProducto As beProducto, ByRef ListabeProducto As List(Of beProducto)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProducto As New daProducto
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProducto.BuscarNumeroCotizacion(CnnSql, obeProducto, ListabeProducto) Then
                    Throw New Exception(odaProducto.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function
End Class
