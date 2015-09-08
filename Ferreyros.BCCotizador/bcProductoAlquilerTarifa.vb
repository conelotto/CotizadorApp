Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoAlquilerTarifa
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdProducto(ByVal strConexion As String, ByVal obeProductoAlquilerTarifa As beProductoAlquilerTarifa, ByRef ListaProductoAlquilerTarifa As List(Of beProductoAlquilerTarifa)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoAlquilerTarifa As New daProductoAlquilerTarifa
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoAlquilerTarifa.BuscarIdProducto(CnnSql, obeProductoAlquilerTarifa, ListaProductoAlquilerTarifa) Then
                    Throw New Exception(odaProductoAlquilerTarifa.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
End Class
