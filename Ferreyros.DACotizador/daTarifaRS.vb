Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daTarifaRS
    Private blnValido As Boolean = False
    Private uData As New Utiles.Datos
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property
    Public Function BuscarCombinacionLlave(ByVal cnnSql As SqlConnection, ByVal obeDatoGeneral As beDatoGeneral, ByRef ListabeTarifaRS As List(Of beTarifaRS)) As Boolean
        Try
            blnValido = False
            Dim cmdSql As SqlCommand = cnnSql.CreateCommand()
            cmdSql.CommandType = CommandType.StoredProcedure
            cmdSql.CommandText = objBBDD.StoreProcedure.UspTarifasRSBuscarCombinacionLLave

            cmdSql.Parameters.Add(uData.CreaParametro("@Condicion", obeDatoGeneral.Campo1, SqlDbType.VarChar, 500))
            cmdSql.Parameters.Add(uData.CreaParametro("@Linea", obeDatoGeneral.Campo2, SqlDbType.VarChar, 50))
            cmdSql.Parameters.Add(uData.CreaParametro("@IdProducto", obeDatoGeneral.Campo3, SqlDbType.VarChar, 20))

            Using dr As SqlDataReader = cmdSql.ExecuteReader

                 
                Dim ebeTarifaRS As beTarifaRS = Nothing

                Dim _IdTarifas As String = dr.GetOrdinal("IdTarifas")
                Dim _Linea As String = dr.GetOrdinal("Linea")
                Dim _CodPlan As String = dr.GetOrdinal("CodPlan")
                Dim _NombrePlan As String = dr.GetOrdinal("NombrePlan")
                Dim _Aplicacion As String = dr.GetOrdinal("Aplicacion")
                Dim _Marca As String = dr.GetOrdinal("Marca")
                Dim _Tipo As String = dr.GetOrdinal("Tipo")
                Dim _Familia As String = dr.GetOrdinal("Familia")
                Dim _Modelo As String = dr.GetOrdinal("Modelo")
                Dim _ModeloBase As String = dr.GetOrdinal("ModeloBase")
                Dim _Prefijo As String = dr.GetOrdinal("Prefijo")
                Dim _Motor As String = dr.GetOrdinal("Motor")
                Dim _PrefijoMotor As String = dr.GetOrdinal("PrefijoMotor")
                Dim _ServFacturar As String = dr.GetOrdinal("ServFacturar")
                Dim _KitDBS As String = dr.GetOrdinal("KitDBS")
                Dim _PrecioKit As String = dr.GetOrdinal("PrecioKit")
                Dim _TarifaServ As String = dr.GetOrdinal("TarifaServ")
                Dim _SOS As String = dr.GetOrdinal("SOS")
                Dim _SubTotal As String = dr.GetOrdinal("SubTotal")
                Dim _Cantidad As String = dr.GetOrdinal("Cantidad")
                Dim _Total As String = dr.GetOrdinal("Total")
                Dim _Estado As String = dr.GetOrdinal("Estado")


                While dr.Read()
                    ebeTarifaRS = New beTarifaRS

                    With ebeTarifaRS 
                        If Not dr.IsDBNull(_IdTarifas) Then .IdTarifas = dr.GetValue(_IdTarifas).ToString()
                        If Not dr.IsDBNull(_Linea) Then .Linea = dr.GetValue(_Linea).ToString()
                        If Not dr.IsDBNull(_CodPlan) Then .CodPlan = dr.GetValue(_CodPlan).ToString()
                        If Not dr.IsDBNull(_NombrePlan) Then .NombrePlan = dr.GetValue(_NombrePlan).ToString()
                        If Not dr.IsDBNull(_Aplicacion) Then .Aplicacion = dr.GetValue(_Aplicacion).ToString()
                        If Not dr.IsDBNull(_Marca) Then .Marca = dr.GetValue(_Marca).ToString()
                        If Not dr.IsDBNull(_Tipo) Then .Tipo = dr.GetValue(_Tipo).ToString()
                        If Not dr.IsDBNull(_Familia) Then .Familia = dr.GetValue(_Familia).ToString()
                        If Not dr.IsDBNull(_Modelo) Then .Modelo = dr.GetValue(_Modelo).ToString()
                        If Not dr.IsDBNull(_ModeloBase) Then .ModeloBase = dr.GetValue(_ModeloBase).ToString()
                        If Not dr.IsDBNull(_Prefijo) Then .Prefijo = dr.GetValue(_Prefijo).ToString()
                        If Not dr.IsDBNull(_Motor) Then .Motor = dr.GetValue(_Motor).ToString()
                        If Not dr.IsDBNull(_PrefijoMotor) Then .PrefijoMotor = dr.GetValue(_PrefijoMotor).ToString()
                        If Not dr.IsDBNull(_ServFacturar) Then .ServFacturar = dr.GetValue(_ServFacturar).ToString()
                        If Not dr.IsDBNull(_KitDBS) Then .KitDBS = dr.GetValue(_KitDBS).ToString()
                        If Not dr.IsDBNull(_PrecioKit) Then .PrecioKit = dr.GetValue(_PrecioKit).ToString()
                        If Not dr.IsDBNull(_TarifaServ) Then .TarifaServ = dr.GetValue(_TarifaServ).ToString()
                        If Not dr.IsDBNull(_SOS) Then .SOS = dr.GetValue(_SOS).ToString()
                        If Not dr.IsDBNull(_SubTotal) Then .SubTotal = dr.GetValue(_SubTotal).ToString()
                        If Not dr.IsDBNull(_Cantidad) Then .Cantidad = dr.GetValue(_Cantidad).ToString()
                        If Not dr.IsDBNull(_Total) Then .Total = dr.GetValue(_Total).ToString()
                        If Not dr.IsDBNull(_Estado) Then .Estado = dr.GetValue(_Estado).ToString()


                    End With
                    ListabeTarifaRS.Add(ebeTarifaRS)
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
