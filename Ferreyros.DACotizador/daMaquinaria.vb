Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras

Public Class daMaquinaria

    Private uData As New Utiles.Datos

    Public Sub MaquinariaListar(ByVal eConexion As String, ByVal Cotizacion As beCotizacion, ByVal Producto As beProducto, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beMaquinaria))

        Dim conexion As SqlConnection = Nothing
        Dim cmdMaquinaria As SqlCommand = Nothing

        Try
            conexion = New SqlConnection(eConexion)
            conexion.Open()
            cmdMaquinaria = conexion.CreateCommand
            cmdMaquinaria.CommandText = objBBDD.StoreProcedure.MaquinariaListar
            cmdMaquinaria.CommandType = CommandType.StoredProcedure
            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdCotizacionSap", Cotizacion.IdCotizacionSap, SqlDbType.VarChar, 20))
            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdPosicionSap", Producto.IdPosicion, SqlDbType.VarChar, 20))
            cmdMaquinaria.Parameters.Add(uData.CreaParametro("@IdProductoSap", Producto.IdProductoSap, SqlDbType.VarChar, 20))

            Using Reader As IDataReader = cmdMaquinaria.ExecuteReader
                Dim _IdMaquinaria As Integer = Reader.GetOrdinal("IdMaquinaria")
                Dim _Item As Integer = Reader.GetOrdinal("Item")
                Dim _MaquinaNueva As Integer = Reader.GetOrdinal("MaquinaNueva")
                Dim _Familia As Integer = Reader.GetOrdinal("Familia")
                Dim _FamiliaOt As Integer = Reader.GetOrdinal("FamiliaOt")
                Dim _ModeloBase As Integer = Reader.GetOrdinal("ModeloBase")
                Dim _ModeloBaseOt As Integer = Reader.GetOrdinal("ModeloBaseOt")
                Dim _Modelo As Integer = Reader.GetOrdinal("Modelo")
                Dim _Prefijo As Integer = Reader.GetOrdinal("Prefijo")
                Dim _PrefijoOt As Integer = Reader.GetOrdinal("PrefijoOt")
                Dim _NumeroMaquinas As Integer = Reader.GetOrdinal("NumeroMaquinas")
                Dim _NumeroSerie As Integer = Reader.GetOrdinal("NumeroSerie")
                Dim _NumeroSerieOt As Integer = Reader.GetOrdinal("NumeroSerieOt")
                Dim _HorometroInicial As Integer = Reader.GetOrdinal("HorometroInicial")
                Dim _HorometroFin As Integer = Reader.GetOrdinal("HorometroFin")
                Dim _FechaHorometro As Integer = Reader.GetOrdinal("FechaHorometro")
                Dim _HorasPromedioUso As Integer = Reader.GetOrdinal("HorasPromedioUso")
                Dim _CodDepartamento As Integer = Reader.GetOrdinal("CodDepartamento")
                Dim _Departamento As Integer = Reader.GetOrdinal("Departamento")
                Dim _Renovacion As Integer = Reader.GetOrdinal("Renovacion")
                Dim _RenovacionValida As Integer = Reader.GetOrdinal("RenovacionValida")
                Dim _PrecioNegociado As Integer = Reader.GetOrdinal("PrecioNegociado")
                Dim Maquinaria As beMaquinaria = Nothing
                While Reader.Read
                    Maquinaria = New beMaquinaria
                    If Not Reader.IsDBNull(_IdMaquinaria) Then Maquinaria.codigo = Reader.GetValue(_IdMaquinaria)
                    If Not Reader.IsDBNull(_Item) Then Maquinaria.item = Reader.GetValue(_Item)
                    If Not Reader.IsDBNull(_MaquinaNueva) Then Maquinaria.maquinaNueva = Reader.GetValue(_MaquinaNueva)
                    If Not Reader.IsDBNull(_Familia) Then Maquinaria.familia = Reader.GetValue(_Familia)
                    If Not Reader.IsDBNull(_FamiliaOt) Then Maquinaria.familiaOt = Reader.GetValue(_FamiliaOt)
                    If Not Reader.IsDBNull(_ModeloBase) Then Maquinaria.modeloBase = Reader.GetValue(_ModeloBase)
                    If Not Reader.IsDBNull(_ModeloBaseOt) Then Maquinaria.modeloBaseOt = Reader.GetValue(_ModeloBaseOt)
                    If Not Reader.IsDBNull(_Modelo) Then Maquinaria.modelo = Reader.GetValue(_Modelo)
                    If Not Reader.IsDBNull(_Prefijo) Then Maquinaria.prefijo = Reader.GetValue(_Prefijo)
                    If Not Reader.IsDBNull(_PrefijoOt) Then Maquinaria.prefijoOt = Reader.GetValue(_PrefijoOt)
                    If Not Reader.IsDBNull(_NumeroMaquinas) Then Maquinaria.numeroMaquinas = Reader.GetValue(_NumeroMaquinas)
                    If Not Reader.IsDBNull(_NumeroSerie) Then Maquinaria.numeroSerie = Reader.GetValue(_NumeroSerie)
                    If Not Reader.IsDBNull(_NumeroSerieOt) Then Maquinaria.numeroSerieOt = Reader.GetValue(_NumeroSerieOt)
                    If Not Reader.IsDBNull(_HorometroInicial) Then Maquinaria.horometroInicial = Reader.GetValue(_HorometroInicial)
                    If Not Reader.IsDBNull(_HorometroFin) Then Maquinaria.horometroFinal = Reader.GetValue(_HorometroFin)
                    If Not Reader.IsDBNull(_FechaHorometro) Then Maquinaria.fechaHorometro = Reader.GetValue(_FechaHorometro)
                    If Not Reader.IsDBNull(_HorasPromedioUso) Then Maquinaria.horasPromedioMensual = Reader.GetValue(_HorasPromedioUso)
                    If Not Reader.IsDBNull(_CodDepartamento) Then Maquinaria.codDepartamento = Reader.GetValue(_CodDepartamento)
                    If Not Reader.IsDBNull(_Departamento) Then Maquinaria.departamento = Reader.GetValue(_Departamento)
                    If Not Reader.IsDBNull(_Renovacion) Then Maquinaria.renovacion = Reader.GetValue(_Renovacion)
                    If Not Reader.IsDBNull(_RenovacionValida) Then Maquinaria.renovacionValida = Reader.GetValue(_RenovacionValida)
                    If Not Reader.IsDBNull(_PrecioNegociado) Then Maquinaria.montoItem = Reader.GetValue(_PrecioNegociado)
                    lResult.Add(Maquinaria)
                End While
            End Using
            eValidacion.validacion = True
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        Finally
            If conexion.State = ConnectionState.Open Then conexion.Close()
        End Try

    End Sub

End Class
