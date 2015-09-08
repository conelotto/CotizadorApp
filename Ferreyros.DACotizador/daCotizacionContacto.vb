Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daCotizacionContacto
    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Sub CotizacionContactoListar(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beCotizacionContacto))

        Dim conexion As SqlConnection = Nothing
        Dim comand As SqlCommand = Nothing
        Dim CotizacionContacto As beCotizacionContacto = Nothing
        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            comand = conexion.CreateCommand
            comand.CommandText = objBBDD.StoreProcedure.CotizacionContactoListar
            comand.CommandType = CommandType.StoredProcedure
            comand.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", eCotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            Using dr As IDataReader = comand.ExecuteReader
                Dim _IdCotizacionContacto As Integer = dr.GetOrdinal("IdCotizacionContacto")
                Dim _Nombres As Integer = dr.GetOrdinal("Nombres")
                Dim _Direccion As Integer = dr.GetOrdinal("Direccion")
                Dim _Email As Integer = dr.GetOrdinal("Email")
                Dim _Cargo As Integer = dr.GetOrdinal("Cargo")
                Dim _Telefono As Integer = dr.GetOrdinal("Telefono")
                While dr.Read
                    CotizacionContacto = New beCotizacionContacto
                    If Not dr.IsDBNull(_IdCotizacionContacto) Then CotizacionContacto.IdCotizacionContacto = dr.GetValue(_IdCotizacionContacto)
                    If Not dr.IsDBNull(_Nombres) Then CotizacionContacto.Nombres = dr.GetValue(_Nombres)
                    If Not dr.IsDBNull(_Direccion) Then CotizacionContacto.Direccion = dr.GetValue(_Direccion)
                    If Not dr.IsDBNull(_Email) Then CotizacionContacto.Email = dr.GetValue(_Email)
                    If Not dr.IsDBNull(_Cargo) Then CotizacionContacto.Cargo = dr.GetValue(_Cargo)
                    If Not dr.IsDBNull(_Telefono) Then CotizacionContacto.Telefono = dr.GetValue(_Telefono)
                    lResult.Add(CotizacionContacto)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

    Public Function BuscarIdCotizacion(ByVal cnnSql As SqlConnection, ByVal obeCotizacionContacto As beCotizacionContacto, ByRef ListabeCotizacionContacto As List(Of beCotizacionContacto)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.uspCotizacionContactoBuscarIdCotizacion

            cmdSql.Parameters.Add(uData.CreaParametro("@IdCotizacion", obeCotizacionContacto.IdCotizacion, SqlDbType.Int))

            Using dr As SqlDataReader = cmdSql.ExecuteReader
                Dim _IdCotizacionContacto As String = dr.GetOrdinal("IdCotizacionContacto")
                Dim _IdCotizacion As String = dr.GetOrdinal("IdCotizacion")
                Dim _Nombres As String = dr.GetOrdinal("Nombres")
                Dim _Cargo As String = dr.GetOrdinal("Cargo")
                Dim _Direccion As String = dr.GetOrdinal("Direccion")
                Dim _Telefono As String = dr.GetOrdinal("Telefono")

                Dim ebeCotizacionContacto As beCotizacionContacto = Nothing

                While dr.Read()
                    ebeCotizacionContacto = New beCotizacionContacto

                    With ebeCotizacionContacto
                        If Not dr.IsDBNull(_IdCotizacionContacto) Then .IdCotizacionContacto = dr.GetValue(_IdCotizacionContacto).ToString()
                        If Not dr.IsDBNull(_IdCotizacion) Then .IdCotizacion = dr.GetValue(_IdCotizacion).ToString()
                        If Not dr.IsDBNull(_Nombres) Then .Nombres = dr.GetValue(_Nombres).ToString()
                        If Not dr.IsDBNull(_Cargo) Then .Cargo = dr.GetValue(_Cargo).ToString()
                        If Not dr.IsDBNull(_Direccion) Then .Direccion = dr.GetValue(_Direccion).ToString()
                        If Not dr.IsDBNull(_Telefono) Then .Telefono = dr.GetValue(_Telefono).ToString()
                    End With
                    ListabeCotizacionContacto.Add(ebeCotizacionContacto)
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
