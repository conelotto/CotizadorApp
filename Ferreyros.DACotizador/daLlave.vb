Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daLlave
    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function BuscarCodigoLinea(ByVal cnnSql As SqlConnection, ByVal idProducto As String, ByVal obeLinea As beLinea, ByRef ListaLlave As List(Of beLlave)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspLLaveBuscarCodigoLinea
            cmdSql.Parameters.Add(uData.CreaParametro("@CodigoLinea", obeLinea.DescripcionCodigo, SqlDbType.VarChar, 20))
            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", idProducto, SqlDbType.VarChar, 100))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                Dim _Llave As String = dr.GetOrdinal("Llave")
                Dim _Valor As String = dr.GetOrdinal("Valor")
                Dim _Campo As String = dr.GetOrdinal("Campo")
                Dim _Dependencia As String = dr.GetOrdinal("Dependencia")

                Dim ebeLlave As beLlave = Nothing

                While dr.Read()
                    ebeLlave = New beLlave

                    With ebeLlave
                        If Not dr.IsDBNull(_Llave) Then .DescripcionCorta = dr.GetValue(_Llave).ToString()
                        If Not dr.IsDBNull(_Valor) Then .DescripcionLarga = dr.GetValue(_Valor).ToString()
                        If Not dr.IsDBNull(_Campo) Then .Campo = dr.GetValue(_Campo).ToString()
                        If Not dr.IsDBNull(_Dependencia) Then .Dependencia = dr.GetValue(_Dependencia).ToString()
                    End With
                    ListaLlave.Add(ebeLlave)
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
