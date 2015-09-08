
Imports System.Data.SqlClient
Imports Ferreyros.BECotizador

Public Class daProductoAccesorio
    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdProducto(ByVal cnnSql As SqlConnection, ByVal obeProductoAccesorio As beProductoAccesorio, ByRef ListaProductoAccesorio As List(Of beProductoAccesorio)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspProductoAccesorioBuscarIdProducto

            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeProductoAccesorio.IdProducto, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdProductoAccesorio As String = dr.GetOrdinal("IdProductoAccesorio")
                Dim _IdAccesorio As String = dr.GetOrdinal("IdAccesorio")
                Dim _CodigoProductoAccesorio As String = dr.GetOrdinal("CodigoProductoAccesorio")
                Dim _NombreProductoAccesorio As String = dr.GetOrdinal("NombreProductoAccesorio")
                Dim _Cantidad As String = dr.GetOrdinal("Cantidad")
                Dim _UnidadMedida As String = dr.GetOrdinal("UnidadMedida")
                Dim _FlatMostrarEspTecnica As String = dr.GetOrdinal("FlatMostrarEspTecnica")

                Dim ebeProductoAccesorio As beProductoAccesorio = Nothing

                While dr.Read()
                    ebeProductoAccesorio = New beProductoAccesorio

                    With ebeProductoAccesorio
                        If Not dr.IsDBNull(_IdProductoAccesorio) Then .IdProductoAccesorio = dr.GetValue(_IdProductoAccesorio).ToString
                        If Not dr.IsDBNull(_IdAccesorio) Then .IdAccesorio = dr.GetValue(_IdAccesorio).ToString()
                        If Not dr.IsDBNull(_CodigoProductoAccesorio) Then .CodigoProductoAccesorio = dr.GetValue(_CodigoProductoAccesorio).ToString()
                        If Not dr.IsDBNull(_NombreProductoAccesorio) Then .NombreProductoAccesorio = dr.GetValue(_NombreProductoAccesorio).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_UnidadMedida) Then .UnidadMedida = dr.GetValue(_UnidadMedida).ToString()
                        If Not dr.IsDBNull(_FlatMostrarEspTecnica) Then .FlatMostrarEspTecnica = dr.GetValue(_FlatMostrarEspTecnica).ToString()
                    End With
                    ListaProductoAccesorio.Add(ebeProductoAccesorio)
                End While
                dr.Close()
            End Using
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
End Class
