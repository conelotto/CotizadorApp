Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports System.IO
Imports System.Data.SqlClient

Public Class frmAdmCargaArchivoRS
    Inherits System.Web.UI.Page

    Private Shared eValidacion As beValidacion = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If String.IsNullOrEmpty(Request.QueryString("transf")) Then
                Dim urlAdd As String = ""

                If String.IsNullOrEmpty(Request.QueryString("cod")) Then
                    urlAdd = "?transf=SI"
                End If
                Response.Redirect(Request.Path + urlAdd)
            End If

        End If
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function CargarArchivo(ByVal TipoCarga As String, _
                                                          ByVal TipoRS As String, _
                                                          ByVal Archivo As String, _
                                                          ByVal idLinea As String, _
                                                          ByVal Linea As String) As String

        Dim strValorReturn As String = String.Empty
        If Not String.IsNullOrEmpty(Archivo) Then
            If TipoCarga = "borrar" Then

                Dim strMensajeSubir As String = SubirArchivoTemporal(Archivo, TipoRS, False, idLinea, Linea)
                strValorReturn = strMensajeSubir

            ElseIf TipoCarga = "anadir" Then

                Dim strMensajeSubir As String = SubirArchivoTemporal(Archivo, TipoRS, True, idLinea, Linea)
                strValorReturn = strMensajeSubir

            End If
        End If
        Return strValorReturn
    End Function

    Public Shared Function SubirArchivoTemporal(ByVal NombreArchivoOrigen As String, ByVal TipoRS As String, _
                                                ByVal anadir As Boolean, ByVal idLinea As String, ByVal Linea As String) As String

        Dim strResultado As String = "1"
        Dim strMensaje As String = String.Empty
        Try

            Dim intTotalColumnasFijas As Integer = TarifasRS.Columns.Count - 2 'No se cuenta posicion 0, ni IdTarifa
            Dim dtTarifasRS As New DataTable
            Dim drFilaTarifasRS As DataRow
            Dim dtTarifasRS_CG As New DataTable
            Dim drFilaTarifasRS_CG As DataRow
            Dim dtTarifasRS_CE As New DataTable
            Dim drFilaTarifasRS_CE As DataRow
            'Dim dtTarifasRS_IN As New DataTable
            'Dim drFilaTarifasRS_IN As DataRow
            Dim dtPartesRS As New DataTable
            Dim drFilaPartesRS As DataRow
            Dim oAdminFTP As New AdminFTP()
            Dim urldestino As String = Modulo.strUrlFtpArchivoTemporal
            Dim extension As String = IO.Path.GetExtension(NombreArchivoOrigen)
            Dim urlOrigen As String = Modulo.strUrlFtpArchivoTemporal
            Dim NombreArchivoDestino As String = TipoRS.Trim + extension
            Dim NombreArchivoDestinoxLinea As String = TipoRS.Trim + "_" + Linea + extension
            Dim dtTarifas As New DataTable
            Dim dtPartes As New DataTable
            Dim urlDestinoTarifasRS = Modulo.strUrlFtpArchivoTarifaRS
            Dim urldestinoPartesRS = Modulo.strUrlFtpArchivoDetallePartesRS

            If TipoRS = "tarifas" Then

                strMensaje = oAdminFTP.CopiarFichero(urlOrigen, urlDestinoTarifasRS, NombreArchivoOrigen, NombreArchivoDestino)

                If strMensaje = "1" Then
                    strMensaje = oAdminFTP.CopiarFichero(urlOrigen, urlDestinoTarifasRS, NombreArchivoOrigen, NombreArchivoDestinoxLinea)
                    Dim mem As MemoryStream = oAdminFTP.ObtenerArchivo(urlDestinoTarifasRS, NombreArchivoDestino)
                    dtTarifas = CargaRS.ExcelToDataTable(mem)
                    Dim totalfilas As Integer = dtTarifas.Rows.Count
                    Dim totalColumnas As Integer = dtTarifas.Columns.Count

                    If totalfilas > 0 Then
                        dtTarifasRS = TarifasRS()
                        dtTarifasRS_CG = TarifasRS_CG()
                        dtTarifasRS_CE = TarifasRS_CE()
                        'dtTarifasRS_IN = TarifasRS_IN()
                        Dim fila As Integer = 1
                        If anadir Then
                            fila = obtenerUltimoRegistro("TarifasRS", TipoRS) + 1
                            eliminarDataTarifaxLinea(idLinea)
                        Else
                            eliminarData("TarifasRS_CG")
                            eliminarData("TarifasRS_CE")
                            'eliminarData("TarifasRS_IN")
                            eliminarData("TarifasRS")
                        End If

                        Dim sw As Boolean = True
                        Dim swCE As Boolean = True
                        Dim idCG(totalColumnas - 1) As Integer
                        Dim idCE(totalColumnas - 1) As Integer
                        'Dim idIN(totalColumnas - 1) As Integer

                        For Each drFila As DataRow In dtTarifas.Rows
                            If sw Then 'lee la primera fila de la tabla, nombre de columna
                                For k As Integer = intTotalColumnasFijas + 1 To totalColumnas - 1
                                    Dim indicador As String = drFila.Item(k).ToString.Substring(0, 2)
                                    Select Case indicador
                                        Case "CG"
                                            idCG(k) = k
                                            Exit Select
                                        Case "CE"
                                            idCE(k) = k
                                            Exit Select
                                            'Case "IN"
                                            '    idIN(k) = k
                                            '    Exit Select
                                        Case Else

                                    End Select
                                Next
                                sw = False
                            Else
                                drFilaTarifasRS = dtTarifasRS.NewRow
                                For i As Integer = 0 To totalColumnas - 1
                                    If i <= intTotalColumnasFijas Then 'indice max para tarifasRS
                                        drFilaTarifasRS.Item(i + 1) = drFila.Item(i)
                                    ElseIf idCE.Length > intTotalColumnasFijas AndAlso idCE(i) > 0 Then ' registros de TarifasRS_CE
                                        If Not drFila.Item(i).ToString.Trim.Equals("") Then
                                            If swCE Then
                                                drFilaTarifasRS_CE = dtTarifasRS_CE.NewRow
                                                drFilaTarifasRS_CE.Item(1) = drFila.Item(i).ToString.Trim
                                                swCE = False
                                            Else
                                                drFilaTarifasRS_CE.Item(0) = fila
                                                drFilaTarifasRS_CE.Item(2) = drFila.Item(i)
                                                dtTarifasRS_CE.Rows.Add(drFilaTarifasRS_CE)
                                                swCE = True
                                            End If
                                        End If
                                    ElseIf idCG.Length > intTotalColumnasFijas AndAlso idCG(i) > 0 Then 'registros de Tarifas_CG
                                        If Not drFila.Item(i).ToString.Trim.Equals("") Then
                                            drFilaTarifasRS_CG = dtTarifasRS_CG.NewRow
                                            drFilaTarifasRS_CG.Item(0) = fila
                                            drFilaTarifasRS_CG.Item(1) = drFila.Item(i)
                                            dtTarifasRS_CG.Rows.Add(drFilaTarifasRS_CG)
                                        End If
                                        'ElseIf idIN.Length > strTotalFilasRS AndAlso idIN(i) > 0 Then 'registros de Tarifas_IN
                                        '    If Not drFila.Item(i).ToString.Trim.Equals("") Then
                                        '        drFilaTarifasRS_IN = dtTarifasRS_IN.NewRow
                                        '        drFilaTarifasRS_IN.Item(0) = fila
                                        '        drFilaTarifasRS_IN.Item(1) = drFila.Item(i)
                                        '        dtTarifasRS_IN.Rows.Add(drFilaTarifasRS_IN)
                                        '    End If
                                    End If

                                Next
                                drFilaTarifasRS.Item(0) = fila.ToString
                                dtTarifasRS.Rows.Add(drFilaTarifasRS)
                                fila = fila + 1
                            End If
                        Next
                        EnviarDataTable(dtTarifasRS, "TarifasRS")
                        EnviarDataTable(dtTarifasRS_CG, "TarifasRS_CG")
                        EnviarDataTable(dtTarifasRS_CE, "TarifasRS_CE")
                        'EnviarDataTable(dtTarifasRS_IN, "TarifasRS_IN")
                    End If
                Else
                    strResultado = "Error al cargar TarifasRS."
                End If

            ElseIf TipoRS = "partes" Then

                strMensaje = oAdminFTP.CopiarFichero(urlOrigen, urldestinoPartesRS, NombreArchivoOrigen, NombreArchivoDestino)

                If strMensaje = "1" Then
                    strMensaje = oAdminFTP.CopiarFichero(urlOrigen, urldestinoPartesRS, NombreArchivoOrigen, NombreArchivoDestinoxLinea)

                    Dim mem As MemoryStream = oAdminFTP.ObtenerArchivo(urldestinoPartesRS, NombreArchivoDestino)
                    dtPartes = CargaRS.ExcelToDataTable(mem)

                    'Quitamos los nombres de las columnas
                    dtPartes.Rows.RemoveAt(0)

                    dtPartesRS = PartesRS()

                    Dim fila As Integer = 1
                    If anadir Then
                        fila = obtenerUltimoRegistro("DetallePartesRS", TipoRS) + 1
                        eliminarDataPartesxLinea(idLinea)
                    Else
                        eliminarData("DetallePartesRS")
                    End If
                    Dim totalcolumnas As Integer = dtPartes.Columns.Count
                    Dim sw As Boolean = True
                    For Each drFila As DataRow In dtPartes.Rows
                        drFilaPartesRS = dtPartesRS.NewRow
                        For i As Integer = 0 To totalcolumnas - 1
                            drFilaPartesRS.Item(i + 1) = drFila.Item(i)
                        Next
                        drFilaPartesRS.Item(0) = fila.ToString
                        dtPartesRS.Rows.Add(drFilaPartesRS)
                        fila = fila + 1
                    Next
                    EnviarDataTable(dtPartesRS, "DetallePartesRS")
                Else
                    strResultado = "Error al cargar DetallePartesRS"
                End If

            End If
        Catch ex As Exception
            strResultado = "Error al subir el Archivo. Revise el archivo. -" + ex.Message + "-" + strMensaje
        End Try
        Return strResultado
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GenerarNombreArchivo(ByVal NombreArchivo As String) As String
        Dim NombreGenerado As String = String.Empty
        NombreGenerado = Modulo.GenerarNombreArchivo(NombreArchivo)
        Return NombreGenerado
    End Function

    Private Shared Function TarifasRS() As DataTable

        Dim Tarifas As DataTable = New DataTable("TarifasRS")

        'Se agrgaron 2 campos "Marca", "Tipo" -- csd 22/05
        Dim nombreColumnas() As String =
            {"IdTarifa", "Linea", "CodPlan", "NombrePlan", "Aplicacion", "Marca", "Tipo",
             "Familia", "Modelo", "ModeloBase", "Prefijo", "Motor", "PrefijoMotor",
             "ServFacturar", "KitDBS", "PrecioKit", "TarifaServ", "SOS", "SubTotal",
             "Cantidad", "Total", "Estado"}

        For i As Integer = 0 To nombreColumnas.Length - 1
            Dim columna As New DataColumn
            columna.DataType = System.Type.GetType("System.String")
            columna.ColumnName = nombreColumnas(i)
            Tarifas.Columns.Add(columna)
        Next

        TarifasRS = Tarifas
    End Function

    Private Shared Function TarifasRS_CE() As DataTable

        Dim CaracteristicasGenerales As DataTable = New DataTable("CaracteristicasEspecificas")

        Dim nombreColumnas() As String =
            {"IdTarifa", "Nombre", "Descripcion"}

        For i As Integer = 0 To nombreColumnas.Length - 1
            Dim columna As DataColumn = New DataColumn()
            columna.DataType = System.Type.GetType("System.String")
            columna.ColumnName = nombreColumnas(i)
            CaracteristicasGenerales.Columns.Add(columna)
        Next

        TarifasRS_CE = CaracteristicasGenerales

    End Function

    Private Shared Function TarifasRS_CG() As DataTable

        Dim CaracteristicasEspeciales As DataTable = New DataTable("CaracteristicasGenerales")

        Dim nombreColumnas() As String =
           {"IdTarifa", "Descripcion"}

        For i As Integer = 0 To nombreColumnas.Length - 1
            Dim columna As DataColumn = New DataColumn()
            columna.DataType = System.Type.GetType("System.String")
            columna.ColumnName = nombreColumnas(i)
            CaracteristicasEspeciales.Columns.Add(columna)
        Next

        TarifasRS_CG = CaracteristicasEspeciales
    End Function

    Private Shared Function TarifasRS_IN() As DataTable

        Dim Incluye As DataTable = New DataTable("Incluye")

        Dim columna1 As DataColumn = New DataColumn()
        columna1.DataType = System.Type.GetType("System.String")
        columna1.ColumnName = "IdTarifa"
        Incluye.Columns.Add(columna1)

        Dim Columna2 As DataColumn = New DataColumn()
        Columna2.DataType = System.Type.GetType("System.String")
        Columna2.ColumnName = "Descripcion"
        Incluye.Columns.Add(Columna2)

        TarifasRS_IN = Incluye
    End Function

    Private Shared Function PartesRS() As DataTable

        Dim Partes As DataTable = New DataTable("Partes")

        'Se agrgaron 2 campos "Marca", "Tipo" -- csd 22/05
        'Se agrego 1 campo "CodPlan" -- csd 09/07
        Dim nombreColumnas() As String =
            {"IdPartes", "Linea", "CodPlan", "Familia", "Aplicacion", "Marca", "Tipo",
             "Motor", "ModeloBase", "Modelo", "Prefijo", "CompQty",
             "NombrePlan", "KitDBS", "Categoria", "DetalleRO", "DetalleJO",
             "Cantidad", "Grupo", "CodSOS", "DescripcionSOS"}

        For i As Integer = 0 To nombreColumnas.Length - 1
            Dim columna As New DataColumn
            columna.DataType = System.Type.GetType("System.String")
            columna.ColumnName = nombreColumnas(i)
            Partes.Columns.Add(columna)
        Next

        PartesRS = Partes
    End Function

    Private Shared Sub EnviarDataTable(ByVal dt As DataTable, ByVal nombreTabla As String)
        Using cn As New SqlConnection(Modulo.strConexionSql), bulk As New SqlBulkCopy(cn, SqlBulkCopyOptions.TableLock, Nothing)
            cn.Open()
            bulk.DestinationTableName = nombreTabla
            bulk.WriteToServer(dt)
        End Using
    End Sub

    Private Shared Function obtenerUltimoRegistro(ByVal nombreTabla As String, ByVal tipoRS As String) As Integer
        Dim cmd As New SqlCommand
        Dim retorno As Integer
        Dim ColumnaID As String = ""
        If tipoRS = "tarifas" Then
            ColumnaID = "IdTarifas"
        ElseIf tipoRS = "partes" Then
            ColumnaID = "IdPartes"
        End If
        Using cn As New SqlConnection(Modulo.strConexionSql)
            cmd.CommandText = "SELECT isnull(MAX(cast(" + ColumnaID + " as int)),0) FROM " + nombreTabla
            cmd.CommandType = CommandType.Text
            cmd.Connection = cn

            cn.Open()
            retorno = CType(cmd.ExecuteScalar(), Integer)
            cn.Close()
        End Using
        Return retorno
    End Function

    Private Shared Sub eliminarData(ByVal nombreTabla As String)
        Dim deleteCmd As String = "DELETE FROM " + nombreTabla
        Using cn As New SqlConnection(Modulo.strConexionSql)
            Dim myCommand As SqlCommand = New SqlCommand(deleteCmd, cn)
            myCommand.Connection.Open()
            Try
                myCommand.ExecuteNonQuery()
            Catch ex As SqlException
                Throw
            End Try
            myCommand.Connection.Close()
        End Using
    End Sub

    Private Shared Sub eliminarDataTarifaxLinea(ByVal Linea As String)
        Dim cLineaResponsable As New bcLineaResponsable
        Dim eLineaResponsable As New beLineaResponsable
        eValidacion = New beValidacion

        eValidacion.flag = "3"
        eLineaResponsable.Linea = Linea
        cLineaResponsable.LineaResponsableMant(Modulo.strConexionSql, eLineaResponsable, eValidacion)
    End Sub

    Private Shared Sub eliminarDataPartesxLinea(ByVal Linea As String)
        Dim cLineaResponsable As New bcLineaResponsable
        Dim eLineaResponsable As New beLineaResponsable
        eValidacion = New beValidacion

        eValidacion.flag = "4"
        eLineaResponsable.Linea = Linea
        cLineaResponsable.LineaResponsableMant(Modulo.strConexionSql, eLineaResponsable, eValidacion)
    End Sub

    <System.Web.Services.WebMethod(enableSession:=True)> _
    Public Shared Function IndicarRoles(ByVal usuario As String) As beValidacion

        Dim cLineaResponsable As New bcLineaResponsable
        Dim eLineaResponsable As New beLineaResponsable
        eValidacion = New beValidacion

        usuario = AdminSeguridad.DesEncriptar(usuario)
        eLineaResponsable.Usuario = usuario
        eValidacion.flag = "1"

        cLineaResponsable.LineaResponsableMant(Modulo.strConexionSql, eLineaResponsable, eValidacion)
        eValidacion.usuario = usuario
        If eValidacion.validacion Then
            eValidacion.mensaje = "El usuario " & usuario & " puede realizar cambios solo en las sgtes. Lineas:"
            eValidacion.usuario = usuario
        Else
            eValidacion.mensaje = "El usuario " & usuario & " no se encuentra registrado en la tabla de Responsable por Lineas."
        End If

        Return eValidacion

    End Function

    <System.Web.Services.WebMethod(enableSession:=True)>
    Public Shared Function llenarCombo(ByVal usuario As String)
        Dim cLineaResponsable As New bcLineaResponsable
        Dim eLineaResponsable As New beLineaResponsable
        eValidacion = New beValidacion

        eValidacion.flag = "2"
        eLineaResponsable.Usuario = usuario

        Dim l_LineaResponsable As New List(Of beLineaResponsable)
        Dim lista As New List(Of EstructuraDatos.combo)

        Try
            cLineaResponsable.listarCombo(Modulo.strConexionSql, eLineaResponsable, eValidacion, l_LineaResponsable)
            For Each drFila As beLineaResponsable In l_LineaResponsable
                Dim newrow = New EstructuraDatos.combo
                newrow.id = drFila.Linea
                newrow.Descripcion = drFila.DescripcionLinea
                lista.Add(newrow)
            Next
        Catch ex As Exception

        End Try

        Return lista
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function EliminarArchivoTemporal(ByVal NombreArchivo As String) As String
        Dim strReturn As String = "-1"
        Dim oAdminFTP As New AdminFTP()
        Dim urldestino As String = String.Concat(Modulo.strUrlFtpArchivoTemporal, NombreArchivo)

        If oAdminFTP.existeObjeto(urldestino) Then
            oAdminFTP.eliminarFichero(urldestino)
            strReturn = "1"
        End If

        Return strReturn
    End Function
End Class
