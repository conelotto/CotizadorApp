Imports System.Data.SqlClient

Public Class daHomologacion
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarTabla(ByVal cnnSql As SqlConnection, ByVal obeHomologacion As beHomologacion, ByRef ListaHomologacion As List(Of beHomologacion)) As Boolean
        Dim blnValido As Boolean = False
        strError = String.Empty
        Try
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = "uspHomologacionBuscarTabla_SC"

            cmdSql.Parameters.Add(uData.CreaParametro("@Tabla", obeHomologacion.Tabla, SqlDbType.VarChar, 50))
            cmdSql.Parameters.Add(uData.CreaParametro("@ValorSAP", obeHomologacion.ValorSap, SqlDbType.VarChar, 100))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdHomologacion As String = dr.GetOrdinal("IdHomologacion")
                Dim _Tabla As String = dr.GetOrdinal("Tabla")
                Dim _Descripcion As String = dr.GetOrdinal("Descripcion")
                Dim _ValorSap As String = dr.GetOrdinal("ValorSap")
                Dim _ValorCotizador As String = dr.GetOrdinal("ValorCotizador")

                Dim ebeHomologacionNew As beHomologacion = Nothing

                While dr.Read()
                    ebeHomologacionNew = New beHomologacion

                    With ebeHomologacionNew
                        If Not dr.IsDBNull(_IdHomologacion) Then .IdHomologacion = dr.GetValue(_IdHomologacion).ToString
                        If Not dr.IsDBNull(_Tabla) Then .Tabla = dr.GetValue(_Tabla).ToString()
                        If Not dr.IsDBNull(_Descripcion) Then .Descripcion = dr.GetValue(_Descripcion).ToString()
                        If Not dr.IsDBNull(_ValorSap) Then .ValorSap = dr.GetValue(_ValorSap).ToString()
                        If Not dr.IsDBNull(_ValorCotizador) Then .ValorCotizador = dr.GetValue(_ValorCotizador).ToString()
                    End With
                    ListaHomologacion.Add(ebeHomologacionNew)
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
