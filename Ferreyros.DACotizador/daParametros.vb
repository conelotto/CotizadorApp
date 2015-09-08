Imports System.Data
Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports System.Data.OleDb
Imports System.Text

Public Class daParametros

    Private uData As New Utiles.Datos
    Private dstParametro As DataSet

    Public Sub ListarDetalleCsa(ByVal oConexion As String, _
                                ByVal oMaquinaria As beMaquinaria, _
                                ByRef oValidacion As beValidacion, _
                                ByRef oDetalle As List(Of beMaquinaria))


        Try
            Using cnnSql As New SqlConnection(oConexion)
                cnnSql.Open()
                Dim cmdSql As SqlCommand = cnnSql.CreateCommand
                cmdSql.CommandText = objBBDD.StoreProcedure.CotizacionCsaParametros
                cmdSql.CommandType = CommandType.StoredProcedure
                cmdSql.Parameters.Add(uData.CreaParametro("@Tipo", oMaquinaria.flag, SqlDbType.Int))
                cmdSql.Parameters.Add(uData.CreaParametro("@Plan", oMaquinaria.plan, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Familia", oMaquinaria.familia, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@ModeloBase", oMaquinaria.modeloBase, SqlDbType.VarChar, 100))
                cmdSql.Parameters.Add(uData.CreaParametro("@Prefijo", oMaquinaria.prefijo, SqlDbType.VarChar, 100))
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim Rpt As beMaquinaria
                    While dr.Read
                        Rpt = New beMaquinaria
                        If Not dr.IsDBNull(0) Then Rpt.codigo = dr.GetValue(0) ' codigo
                        If Not dr.IsDBNull(1) Then Rpt.descripcion = dr.GetValue(1) ' descripcion
                        'validacion para registros vacios
                        If Not String.IsNullOrEmpty(Rpt.codigo) Then
                            oDetalle.Add(Rpt)
                        End If

                    End While
                    dr.Close()
                End Using
            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try

    End Sub

    Public Sub ListarNroSerie(ByVal oConexion As String, ByVal oLibreria As String, ByVal oMaquinaria As beMaquinaria, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beMaquinaria))

        Try
            Using cnnOleDb As New OleDbConnection(oConexion)
                cnnOleDb.Open()
                Dim cmdSql As OleDbCommand = cnnOleDb.CreateCommand
                cmdSql.CommandText = Query_NroSerie(oLibreria, oMaquinaria.prefijo)
                cmdSql.CommandType = CommandType.Text
                Using dr As IDataReader = cmdSql.ExecuteReader
                    Dim Rpt As beMaquinaria
                    While dr.Read
                        Rpt = New beMaquinaria
                        If Not dr.IsDBNull(0) Then Rpt.codigo = dr.GetValue(0) ' codigo
                        If Not dr.IsDBNull(0) Then Rpt.descripcion = dr.GetValue(0) ' descripcion
                        oDetalle.Add(Rpt)
                    End While
                    dr.Close()
                End Using
            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        End Try
    End Sub

    Private Function Query_NroSerie(ByVal Libreria As String, ByVal Parametro As String) As String

        Dim Query As New StringBuilder
        Query.Append(String.Format("SELECT EQMFS2 FROM {0}.EMPEQPD0 ", Libreria))
        Query.Append("WHERE (INVI='S' OR INVI='F' OR INVI='I' OR INVI='L' OR INVI='R' OR INVI='D' OR INVI='Z') ")
        Query.Append("AND (CUNO<>'MAQBAJA' AND CUNO<>'BAJA' AND CUNO<>'GASTO') ")
        Query.Append("AND LCUNO<>'BAJA' AND LCUNO<>'GASTO' AND LCUNO<>'EXCFS' AND YM<>'1901' ")
        Query.Append(String.Format("AND SUBSTR(EQMFS2,2,3) = '{0}' ", Parametro))
        Query.Append("ORDER BY EQMFS2")

        Return Query.ToString

    End Function

End Class
