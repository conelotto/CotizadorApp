Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras

Public Class daMaquinaria

    Private uData As New Utiles.Datos
    Private uConfig As New Utiles.uConfiguracion
    Private Command As SqlCommand = Nothing

    Public Sub InsertarMaquinaria(ByVal Connection As SqlConnection, ByVal Transaction As SqlTransaction, _
                                  ByVal IdProducto As Integer, ByVal Item As Integer, ByVal Usuario As String, _
                                  ByVal Maquinaria As beMaquinaria)

        Command = Connection.CreateCommand
        Command.Transaction = Transaction
        Command.CommandText = "uspServicioMaquinariaInsertar"
        Command.CommandType = CommandType.StoredProcedure

        With Command.Parameters
            .Add(uData.CreaParametro("@IdProducto", IdProducto, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Item", Item, SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Familia", Maquinaria.Familia, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@ModeloBase", Maquinaria.ModeloBase, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@Modelo", Maquinaria.Modelo, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@Prefijo", Maquinaria.Prefijo, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@MaquinaNueva", uConfig.fc_ConvertirBoolean(Maquinaria.MaquinaNueva), SqlDbType.Bit, 10))
            .Add(uData.CreaParametro("@NumeroSerie", Maquinaria.NumeroSerie, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@HorometroInicial", uConfig.fc_ConvertirEntero(Maquinaria.HorometroInicial), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@FechaHorometro", uConfig.fc_ConvertirFecha(Maquinaria.FechaHorometro), SqlDbType.VarChar, 10))
            .Add(uData.CreaParametro("@HorasPromedioUso", uConfig.fc_ConvertirEntero(Maquinaria.HorasPromedioMensual), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@HorometroFin", uConfig.fc_ConvertirEntero(Maquinaria.HorometroFinal), SqlDbType.Int, 15))
            .Add(uData.CreaParametro("@Renovacion", uConfig.fc_ConvertirBoolean(Maquinaria.Renovacion), SqlDbType.Bit, 10))
            .Add(uData.CreaParametro("@RenovacionValida", uConfig.fc_ConvertirBoolean(Maquinaria.RenovacionValida), SqlDbType.Bit, 10))
            .Add(uData.CreaParametro("@CodDepartamento", Maquinaria.CodDepartamento, SqlDbType.VarChar, 30))
            .Add(uData.CreaParametro("@Departamento", Maquinaria.Departamento, SqlDbType.VarChar, 100))
            .Add(uData.CreaParametro("@Usuario", Usuario, SqlDbType.VarChar, 15))
        End With

        Command.ExecuteNonQuery()

    End Sub


End Class
