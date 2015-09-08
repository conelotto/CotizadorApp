Imports System.IO
Imports Ferreyros.BECotizador
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Wordprocessing
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Wordprocessing.BookmarkStart
Imports Ferreyros.BCCotizador
Imports System.Runtime.Serialization.Formatters.Binary
Imports Ferreyros.Utiles.Estructuras
Imports Vt = DocumentFormat.OpenXml.VariantTypes
Imports System.Xml
Imports System.IO.Packaging
'Imports Microsoft.Office.Interop.Word
'Imports DocumentFormat.OpenXml.Spreadsheet


'Imports d = DocumentFormat.OpenXml.Drawing
'Imports dwp = DocumentFormat.OpenXml.Drawing.Wordprocessing
'Imports dpic = DocumentFormat.OpenXml.Drawing.Pictures

Public Class Impresion

    'Propiedades

    Private _TieneDetallePartes As Integer

    Private Shared ReadOnly Property Nomb_TarifaRS() As String
        Get
            Return "TarifaRS_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_TarifaRS_Respaldo() As String
        Get
            Return "TarifaRSRespaldo_"
        End Get
    End Property


    Private Shared ReadOnly Property Nomb_Cotizacion() As Object
        Get
            Return "Cotizacion_"
        End Get
    End Property

    Private Shared ReadOnly Property Nomb_Producto() As String
        Get
            Return "ProductoCSA_"
        End Get
    End Property

    Public Property TieneDetallePartes As Integer
        Get
            Return _TieneDetallePartes
        End Get
        Set(ByVal value As Integer)
            _TieneDetallePartes = value
        End Set
    End Property

    Sub New()
        'RunPropCeldaTabla = RunPropertiesConfigurado(, , , "20") 
    End Sub
    Private objAdminFTP As New AdminFTP
    Private objImpresionExtender As New ImpresionExtender
    Private objImpresionSolCombinada As New ImpresionSolCombinada
    Private objEmail As New AdminEmail

    Private CantidadPagCartaPresentacion As Integer
    Private CantidadPagPropuestaComercial As Integer
    Private CantidadPagCondicionesGenerales As Integer
    Private CantidadPagEspecificacionesTecnicas As Integer
    Private CantidadPagTerminosCondiciones As Integer
    Private CantidadPagPresentacionFSA As Integer
    Private CantidadPagPresentacionMercado As Integer
    Private CantidadPagRequisitosAprobacionCredito As Integer
    Private CantidadPagRequisitosFormalizacionCredito As Integer
    Private CantidadPagFormatoUCMI As Integer

    Private ProductosIncluyenClC As Boolean = False

    Private RunPropCeldaTablaCalculos20 As RunProperties
    Private RunPropCeldaTablaTexto20 As RunProperties
    Private lenghtLetraTablaCalculo20 As String = "20" '"16" 
    Private RunPropCeldaTablaTexto24 As RunProperties
    Private paragraphPropertiesCentro As New ParagraphProperties()
    Private justificacionCentro As New Justification With {.Val = JustificationValues.Center}

    Private TipoCotizacion As String = String.Empty
    Private IdProducto As String = String.Empty

    Private str_mensajeRecorrido As New StringBuilder

    Public Function GenerarDocumentoCotizacionPrevia(ByVal eCotizacion As beCotizacion) As beValidacion
        Dim ebeValidacion As New beValidacion
        Dim obcCotizacion As New bcCotizacion
         
        Dim NombreDocumento As String = String.Empty
        ebeValidacion.validacion = False

        str_mensajeRecorrido.AppendLine("INICIO " & Now.ToString() & " :------------------------------------------------")
        str_mensajeRecorrido.AppendLine(" Nro Cotizacion :  " & eCotizacion.IdCotizacionSap.ToString & " - " & Now.ToString())

        Try
            Dim ListaByteDocumento As New List(Of Byte())
            Dim dtsDatosDocumento As New DataSet
            obcCotizacion.DatosDocumento(Modulo.strConexionSql, eCotizacion, dtsDatosDocumento)
            '---Verificacion de tipo Cotizacion--------------------------------------------------------
            For Each drProducto In dtsDatosDocumento.Tables(Entidad.ResumenPropuesta).Rows
                If drProducto("TipoProducto").ToString() = TipoProducto.ALQUILER Then
                    TipoCotizacion = TipoProducto.ALQUILER
                    Exit For
                End If
            Next

            Select Case TipoCotizacion
                Case TipoProducto.ALQUILER
                    ListaByteDocumento = objImpresionExtender.CotizarAlquilerPrevia(eCotizacion, dtsDatosDocumento)
                Case Else ' Por defecto. Todos los tipos de cotizaciones
            End Select

            Dim r As Byte() = CombinarDocumentos(ListaByteDocumento, True, False)
            Dim msDocGenerado As New MemoryStream
            msDocGenerado.Write(r, 0, r.Length)
            Dim oAdminFTP As New AdminFTP
            Dim RutaDestino As String = Modulo.strUrlFtpCotizacionVersion

            'Nombre para el Archivo de la Nueva version
            NombreDocumento = "Cotizacion Previa.docx" 'eCotizacion.IdCotizacionSap + " - Cotizacion Previa.docx"

            Dim strResultadoSubir As String = "0"
            'Subir el archivo al FTP
            strResultadoSubir = oAdminFTP.SubirArchivo(msDocGenerado, RutaDestino, NombreDocumento)

            ebeValidacion.validacion = True
            ebeValidacion.mensaje = NombreDocumento
            str_mensajeRecorrido.AppendLine("mensaje :" & ebeValidacion.mensaje)
        Catch ex As Exception
            ebeValidacion.validacion = False
            ebeValidacion.mensaje = "Error al generar la Cotización : " + ex.Message.ToString

            str_mensajeRecorrido.AppendLine("  ex : " & ex.Message.ToString & " - " & ex.StackTrace)
            str_mensajeRecorrido.AppendLine("FIN " & Now.ToString() & " :--------------------------------------------------")
            str_mensajeRecorrido.AppendLine(Chr(13))
            Try
                objEmail.EnvioEmail("Error en Impresión de Cotizacion Previa", str_mensajeRecorrido)
            Catch ex1 As Exception
            End Try
        End Try
        Return ebeValidacion
    End Function

    Public Function GenerarDocumentoCotizacion(ByVal listaTablaMaestra As List(Of beTablaMaestra), ByVal dtbSeccionArchivo As DataTable, ByVal eCotizacion As beCotizacion, ByVal session As String) As beValidacion
        Dim ebeValidacion As New beValidacion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        Dim obcCotizacion As New bcCotizacion

        Dim ListaDocImpreso As New List(Of beTablaMaestra)
        Dim NombreDoc As String = String.Empty
        Dim boolCrearExcelDetalleParte As Boolean = False
        Dim NombreDocumento As String = String.Empty
        ebeValidacion.validacion = False

        '--Iniciar Variables Globales ---------
        RunPropCeldaTablaCalculos20 = RunPropertiesConfigurado(, , , "20") '16
        RunPropCeldaTablaTexto20 = RunPropertiesConfigurado(, , , "20") '24
        RunPropCeldaTablaTexto24 = RunPropertiesConfigurado(, , , "24") '18
        'tamaño letra: 24=12 , 18:9 , 
        TieneDetallePartes = 0
        str_mensajeRecorrido.Clear()


        paragraphPropertiesCentro.Append(justificacionCentro)

        '-----------------------------

        str_mensajeRecorrido.AppendLine("INICIO " & Now.ToString() & " :------------------------------------------------")
        str_mensajeRecorrido.AppendLine(" Nro Cotizacion :  " & eCotizacion.IdCotizacionSap.ToString & " - " & Now.ToString())

        'NombreDoc = ObtenerSoloNombDocumento(NombreDocumento)

        Try
            Dim ListaByteDocumento As New List(Of Byte())
            Dim dtsDatosDocumento As New DataSet
            obcCotizacion.DatosDocumento(Modulo.strConexionSql, eCotizacion, dtsDatosDocumento)
            '---Verificacion de tipo Cotizacion--------------------------------------------------------
            For Each drProducto In dtsDatosDocumento.Tables(Entidad.ResumenPropuesta).Rows
                If drProducto("TipoProducto").ToString() = TipoProducto.SOLUCION_COMBINADA Then
                    TipoCotizacion = TipoProducto.SOLUCION_COMBINADA
                    IdProducto = drProducto("IdProductoSAP").ToString()
                    Exit For
                End If

                If drProducto("TipoProducto").ToString() = TipoProducto.ALQUILER Then
                    TipoCotizacion = TipoProducto.ALQUILER

                    For Each drProductoAlquiler In dtsDatosDocumento.Tables(Entidad.ProductoAlquiler).Rows
                        If drProductoAlquiler("CodigoTipoArrendamiento") = TipoProductoAlquiler.LEASING Then
                            TipoCotizacion = TipoProducto.ALQUILER_LEASING
                        End If
                        Exit For
                    Next

                    Exit For
                End If

            Next

            Select Case TipoCotizacion
                Case TipoProducto.ALQUILER_LEASING
                    ListaByteDocumento = objImpresionExtender.CotizarAlquilerLeasing(eCotizacion, dtsDatosDocumento)
                Case Else ' Por defecto. Todos los tipos de cotizaciones
                    For Each eTablaMaestra As beTablaMaestra In listaTablaMaestra
                        If eTablaMaestra.Imprimir.ToUpper() = "SI" Or eTablaMaestra.Imprimir.ToUpper() = "MP" Then
                            Dim objMemoryStream As MemoryStream = Nothing

                            Dim drRegistros() As Data.DataRow
                            'buscar segun : eTablaMaestra.IdSeccion
                            drRegistros = dtbSeccionArchivo.Select("IdSeccion = " + eTablaMaestra.IdSeccion.Trim)

                            If drRegistros.Length > 0 Then
                                Dim drData As DataRow = drRegistros(0)
                                Dim IdArchivoConfig As String = drData.Item("IdArchivoConfig").ToString
                                Dim NombreArchivo As String = drData.Item("NombreArchivo").ToString

                                ' Generar Documento segun el Codigo de seccion
                                Select Case drData.Item("CodigoSeccion").ToString.Trim.ToUpper
                                    Case CodigoSeccion.CartaPresentacion '001: Carta Presentacion:                                
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            ''---controlar si no se encuentra la plantilla ---------------------------------
                                            'If objMemoryStream Is Nothing Then
                                            '    Throw New Exception("No se encontró la plantilla de carta de presentación")
                                            'End If
                                            ''------------------------------------------------------------------------------
                                            Call GenerarCartaPresentacion(objMemoryStream, dtsDatosDocumento.Tables(Entidad.CartaPresentacion), dtsDatosDocumento.Tables(Entidad.ResumenPropuesta), IdArchivoConfig, listaTablaMaestra, dtbSeccionArchivo, session)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.CartaPresentacion, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case CodigoSeccion.PropuestaComercial '"002" 'Propuesta comercial:
                                        objMemoryStream = New MemoryStream
                                        objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                        Call GenerarPropuestaComercial(objMemoryStream, dtsDatosDocumento.Tables(Entidad.ResumenPropuesta), dtsDatosDocumento.Tables(Entidad.AdicionalProducto), dtsDatosDocumento.Tables(Entidad.AccesorioProducto), dtsDatosDocumento.Tables(Entidad.ProductoAlquiler), session)
                                        If objMemoryStream.Length > 0 Then
                                            ListaByteDocumento.Add(objMemoryStream.ToArray())
                                            ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.PropuestaComercial, eTablaMaestra.Nombre))
                                        End If
                                        Exit Select
                                    Case CodigoSeccion.CondicionesGenerales '"003"  ' Condiciones generales o especificas:Generar y adjuntar

                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            Call GenerarCondicionesGenerales(objMemoryStream, dtsDatosDocumento.Tables(Entidad.CondicionesGeneralesPrime), dtsDatosDocumento.Tables(Entidad.AdicionalProducto), dtsDatosDocumento.Tables(Entidad.ResumenPropuesta), dtsDatosDocumento.Tables(Entidad.CondicionesGeneralesAlquiler), session)
                                            Call ConstruirAnexos(objMemoryStream, dtsDatosDocumento.Tables(Entidad.CondicionesGeneralesCSA))
                                            'Call ConstruirExcelDetallePartes(dtsDatosDocumento.Tables(Entidad.CondicionesGeneralesCSA), NombreDoc)
                                            boolCrearExcelDetalleParte = True

                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                'ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.CondicionesGenerales, eTablaMaestra.Nombre))
                                            End If
                                        End If

                                        'Alquileres - Agrega Informacion Adicional
                                        If TipoCotizacion = TipoProducto.ALQUILER Then
                                            Dim objMemoryStreamAdicional As MemoryStream = Nothing
                                            Dim obeHomologacion As New beHomologacion
                                            Dim listaHomologacion As New List(Of beHomologacion)
                                            Dim objImpresionExtender As New ImpresionExtender

                                            obeHomologacion.Tabla = TablaHomologacion.DIR_COTIZACION_ALQUILER_INFOADIC  ' tabla
                                            obeHomologacion.ValorSap = eCotizacion.IdCompania
                                            listaHomologacion.Clear()
                                            listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                                            If listaHomologacion.Count > 0 Then
                                                obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                                                NombreArchivo = obeHomologacion.ValorCotizador
                                            Else
                                                NombreArchivo = String.Empty
                                            End If

                                            Try
                                                If NombreArchivo <> String.Empty Then
                                                    objMemoryStreamAdicional = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
                                                    ListaByteDocumento.Add(objMemoryStreamAdicional.ToArray())
                                                End If
                                            Catch ex As Exception

                                            End Try
                                        End If

                                        ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.CondicionesGenerales, eTablaMaestra.Nombre))

                                        Exit Select
                                    Case CodigoSeccion.EspecificacionTecnica '"004"  ' Especificaciones técnicas de productos y/o servicios: Adjuntar documento
                                        objMemoryStream = New MemoryStream

                                        If TipoCotizacion = TipoProducto.SOLUCION_COMBINADA Then
                                            objImpresionSolCombinada.GenerarEspecificacionSolCombinada(listaTablaMaestra, dtbSeccionArchivo, eCotizacion, objMemoryStream, IdProducto, ListaByteDocumento)

                                            'ListaByteDocumento.Add(objMemoryStream.ToArray())
                                        Else
                                            str_mensajeRecorrido.AppendLine(" 4- Especificaciones Tecnicas - " & Now.ToString())
                                            Dim dtbProducto As New DataTable
                                            Dim obeTablaMaestra As New beTablaMaestra
                                            Dim dtbAdicionalProducto As New DataTable
                                            Dim dtbAccesorioProducto As New DataTable

                                            dtbAdicionalProducto = dtsDatosDocumento.Tables(Entidad.AdicionalProducto)
                                            dtbAccesorioProducto = dtsDatosDocumento.Tables(Entidad.AccesorioProducto)

                                            obeTablaMaestra.IdSeccionCriterio = drData.Item("IdSeccionCriterio").ToString()
                                            obeTablaMaestra.IdTablaMaestra = eCotizacion.IdCotizacion
                                            obeTablaMaestra.Nombre = "ESPECIFICACION" ' Para Filtrar productos de especificacion tecnicas con sus archivos
                                            obcArchivoConfiguracion.BuscarArchivoProducto(Modulo.strConexionSql, obeTablaMaestra, dtbProducto)
                                            Dim PosicionProductos As Integer = 0
                                            For Each drProducto As DataRow In dtbProducto.Rows

                                                PosicionProductos = PosicionProductos + 1
                                                NombreArchivo = drProducto.Item("Archivo").ToString
                                                objMemoryStream = New MemoryStream
                                                objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                                'Solo si se encuentra el archivo de especificacion del producto
                                                If Not objMemoryStream Is Nothing Then
                                                    If objMemoryStream.Length > 0 Then
                                                        '------------------------------------------------------------------------------------------
                                                        Call AdjuntarEspecificacionAdicionales(objMemoryStream, drProducto, dtbAdicionalProducto, PosicionProductos, dtbAccesorioProducto)
                                                        '-------------------------------------------------------------------------------------------
                                                        'ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                        'ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.EspecificacionTecnica, eTablaMaestra.Nombre))
                                                        'Agregamos a la lista de MemoryStreams

                                                        ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                    End If

                                                End If

                                            Next
                                        End If



                                        ''Alquileres - Agrega Informacion Adicional
                                        'If TipoCotizacion = TipoProducto.ALQUILER Then
                                        '    Dim objMemoryStreamAdicional As MemoryStream = Nothing
                                        '    Dim obeHomologacion As New beHomologacion
                                        '    Dim listaHomologacion As New List(Of beHomologacion)
                                        '    Dim objImpresionExtender As New ImpresionExtender

                                        '    obeHomologacion.Tabla = TablaHomologacion.DIR_COTIZACION_ALQUILER_INFOADIC  ' tabla
                                        '    obeHomologacion.ValorSap = eCotizacion.IdCompania
                                        '    listaHomologacion.Clear()
                                        '    listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                                        '    If listaHomologacion.Count > 0 Then
                                        '        obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                                        '        NombreArchivo = obeHomologacion.ValorCotizador
                                        '    Else
                                        '        NombreArchivo = String.Empty
                                        '    End If

                                        '    Try
                                        '        If NombreArchivo <> String.Empty Then
                                        '            objMemoryStreamAdicional = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
                                        '            ListaByteDocumento.Add(objMemoryStreamAdicional.ToArray())
                                        '        End If
                                        '    Catch ex As Exception

                                        '    End Try
                                        'End If


                                        ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.EspecificacionTecnica, eTablaMaestra.Nombre))

                                        Exit Select
                                    Case CodigoSeccion.TerminosCondiciones '"005"  ' Terminos, condiciones generales y definiciones: Adjuntar 
                                        Dim obeTablaMaestra As New beTablaMaestra
                                        obeTablaMaestra.IdSeccionCriterio = drData.Item("IdSeccionCriterio").ToString()
                                        obeTablaMaestra.IdTablaMaestra = eCotizacion.IdCotizacion
                                        obeTablaMaestra.Nombre = "TERMINOS" ' Para Filtrar productos de Terminos y Condiciones
                                        Try
                                            Call GenerarTerminosCondiciones(objMemoryStream, dtsDatosDocumento.Tables(Entidad.TerminosCondiciones), obeTablaMaestra)
                                            If Not objMemoryStream Is Nothing AndAlso objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.TerminosCondiciones, eTablaMaestra.Nombre))
                                            End If
                                        Catch ex As Exception

                                        End Try

                                        Exit Select
                                    Case CodigoSeccion.PresentacionFSA '"006"  ' Presentación FSA: Adjuntar
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            str_mensajeRecorrido.AppendLine(" 6- Presentacion FSA - " & Now.ToString())
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.PresentacionFSA, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case CodigoSeccion.PresentacionMercado '"007"  ' Presentación Mercado: Adjuntar
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            str_mensajeRecorrido.AppendLine(" 7- Presentacion Mercado - " & Now.ToString())
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.PresentacionMercado, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case "008"  'Detalle de producto y/o Servicio:
                                        Exit Select
                                    Case CodigoSeccion.RequisitosAprobacionFormalizacionCredito '009
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            str_mensajeRecorrido.AppendLine(" 9 - Requisitos de Aprobacion de Credito - " & Now.ToString())
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.RequisitosAprobacionFormalizacionCredito, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case CodigoSeccion.FormatoUCMIAnual '010
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            str_mensajeRecorrido.AppendLine(" 10 - Requisitos de Formalizacion de Credito - " & Now.ToString())
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.FormatoUCMIAnual, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case CodigoSeccion.FormatoUCMIEvento '011
                                        If Not String.IsNullOrEmpty(NombreArchivo) Then
                                            str_mensajeRecorrido.AppendLine(" 11 - Formato UCMI - " & Now.ToString())
                                            objMemoryStream = New MemoryStream
                                            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)
                                            If objMemoryStream.Length > 0 Then
                                                ListaByteDocumento.Add(objMemoryStream.ToArray())
                                                ListaDocImpreso.Add(New beTablaMaestra(ListaByteDocumento.Count - 1, CodigoSeccion.FormatoUCMIEvento, eTablaMaestra.Nombre))
                                            End If
                                        End If
                                        Exit Select
                                    Case Else
                                End Select
                            Else ' cuando no existe...
                            End If ' fin de drRegistros.Length > 0
                        End If ' Fin de eTablaMaestra.Imprimir.ToUpper()
                    Next ' Fin de listaTablaMaestra
                    Call GenerarIndiceCartaPresentacion(ListaDocImpreso, ListaByteDocumento)
            End Select

            '-------------------------------------------------------------------------------------------


            'Call GenerarIndiceCartaPresentacion(ListaDocImpreso, ListaByteDocumento)
            Dim r As Byte() = CombinarDocumentos(ListaByteDocumento, True, True)
            Dim msDocGenerado As New MemoryStream
            msDocGenerado.Write(r, 0, r.Length)

            Dim oAdminFTP As New AdminFTP
            Dim RutaDestino As String = Modulo.strUrlFtpCotizacionVersion

            '---- Generar Numero version-----------------------------------
            Dim eCotizacionVersionGenerado As New beCotizacionVersion
            Dim obcCotizacionVersion As New bcCotizacionVersion

            'Obtener la Entidad con la Nueva Version
            eCotizacionVersionGenerado.IdCotizacion = eCotizacion.IdCotizacion
            eCotizacionVersionGenerado.IdCotizacionSap = eCotizacion.IdCotizacionSap
            eCotizacionVersionGenerado = obcCotizacionVersion.ObtenerNuevaVersion(Modulo.strConexionSql, eCotizacionVersionGenerado)
            eCotizacionVersionGenerado.IdCotizacion = eCotizacion.IdCotizacion
            'Nombre para el Archivo de la Nueva version
            NombreDocumento = Modulo.GenerarNombreVersion(eCotizacionVersionGenerado)
            'Crear Excel de detalle de partes
            If boolCrearExcelDetalleParte = True Then
                NombreDoc = ObtenerSoloNombDocumento(NombreDocumento)
                Call ConstruirExcelDetallePartes(dtsDatosDocumento.Tables(Entidad.CondicionesGeneralesCSA), NombreDoc)
            End If
            'Asignar valores a la nueva version
            eCotizacionVersionGenerado.NombreArchivo = NombreDocumento
            'eCotizacionVersionGenerado.NumVersion = eCotizacionVersionGenerado.NumVersion
            If Me.TieneDetallePartes = 1 Then
                eCotizacionVersionGenerado.TieneDetallePartes = 1
            Else
                eCotizacionVersionGenerado.TieneDetallePartes = 0
            End If

            Dim strResultadoSubir As String = "0"

            'Subir el archivo al FTP
            strResultadoSubir = oAdminFTP.SubirArchivo(msDocGenerado, RutaDestino, NombreDocumento)

            ebeValidacion.validacion = True
            ebeValidacion.mensaje = NombreDocumento
            'Grabar en BD SQL
            If strResultadoSubir <> "0" Then
                obcCotizacionVersion.Insertar(Modulo.strConexionSql, eCotizacionVersionGenerado)
            End If
            '-------------------------------------------------------------------
            str_mensajeRecorrido.AppendLine("mensaje :" & ebeValidacion.mensaje)
        Catch ex As Exception
            ebeValidacion.validacion = False
            ebeValidacion.mensaje = "Error al generar la Cotización : " + ex.Message.ToString

            str_mensajeRecorrido.AppendLine("  ex : " & ex.Message.ToString & " - " & ex.StackTrace)
            str_mensajeRecorrido.AppendLine("FIN " & Now.ToString() & " :--------------------------------------------------")
            str_mensajeRecorrido.AppendLine(Chr(13))
            Try
                'Call EscribirLog(str_mensajeRecorrido)
                objEmail.EnvioEmail("Error en Impresión de Cotizacion", str_mensajeRecorrido)
            Catch ex1 As Exception
            End Try
        End Try
        Return ebeValidacion
    End Function

#Region "Generacion de Secciones"
    Private Sub GenerarCartaPresentacion(ByRef objMemoryStream As MemoryStream, ByVal dtbDatos As DataTable, ByVal dtbDatosProductos As DataTable, ByVal IdArchivoConfig As String, ByVal listaTablaMaestra As List(Of beTablaMaestra), ByVal dtbSeccionArchivo As DataTable, ByVal session As String)
        Dim xmlDocumento As New System.Xml.XmlDocument
        Dim xpnDocumento As System.Xml.XPath.XPathNavigator
        Dim xnmDocumento As System.Xml.XmlNamespaceManager
        Dim msPackage As IO.Packaging.PackagePart
        Dim uriPartTarget As Uri
        Dim strCampo As String
        Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"
        Dim ExiteCampoTabla As Boolean = False
        Dim valorTablaDatos As String = String.Empty

        Dim eTelefonoResponsable As New beTelefonoResponsable
        Dim listaTelefonoResponsable As New List(Of beTelefonoResponsable)
        Dim cTelefonoResponsable As New bcTelefonoResponsable
        Dim sw As Boolean = False

        Try
            str_mensajeRecorrido.AppendLine(" 1- Carta de Presentacion - " & Now.ToString())
            'insertando la tabla de indice -- RS 10.01

            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
                '-- Tabla 1: Productos
                Dim tableProducto As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                Dim nfilaProducto As Integer = tableProducto.Elements(Of TableRow).Count
                Dim nfilamaxProducto As Integer = listaTablaMaestra.Count
                Dim iProducto As Integer = 1

                For Each drFila As DataRow In dtbDatosProductos.Rows

                    'Cambios 14/07 - CS - adicion a la descripción
                    Dim eCotizacion As New beCotizacion
                    Dim l_Producto As IEnumerable(Of beProducto)
                    Dim eProducto As beProducto
                    Dim result As String = ""
                    Dim l_eTarifaRS As New List(Of beTarifaRS)
                    Dim eTarifa As New beTarifaRS
                    Dim obeHomologacion As New beHomologacion
                    Dim listaHomologacion As New List(Of beHomologacion)
                    Dim valorHomologacion As String = ""
                    Dim arrayValor As String()
                    Dim cadena As String = drFila.Item("Descripcion") + " "

                    Dim cTarifaRS As New bcTarifaRS
                    Dim eDatoGeneral As New beDatoGeneral

                    Try
                        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
                        l_Producto = eCotizacion.ListaProducto.Where(Function(c) c.IdProducto = drFila.Item("IdProducto"))
                        eProducto = CType(l_Producto.ToList, List(Of beProducto)).First

                        eDatoGeneral.Campo1 = ""
                        eDatoGeneral.Campo2 = drFila.Item("CodLinea")
                        eDatoGeneral.Campo3 = eProducto.IdProductoSap

                        cTarifaRS.BuscarCombinacionLlave(Modulo.strConexionSql, eDatoGeneral, l_eTarifaRS)

                        If Not l_eTarifaRS Is Nothing Then
                            Dim l_TarifaRS As IEnumerable(Of beTarifaRS) = l_eTarifaRS.Where(Function(c) c.IdTarifas = eProducto.IdTarifaRS)
                            eTarifa = CType(l_TarifaRS.ToList, List(Of beTarifaRS)).First

                            obeHomologacion.Tabla = TablaHomologacion.CAMPOS_ADICIONALES_POR_LINEA  ' tabla
                            obeHomologacion.ValorSap = drFila.Item("CodLinea")
                            listaHomologacion.Clear()
                            listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                            If listaHomologacion.Count > 0 Then
                                obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                                valorHomologacion = obeHomologacion.ValorCotizador
                                arrayValor = Split(valorHomologacion, ",")
                                For i = 0 To arrayValor.Length - 1
                                    cadena = cadena + DataBinder.Eval(eTarifa, arrayValor(i)) + " "
                                Next
                            Else
                                valorHomologacion = String.Empty
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    '-------------------------------

                    Try
                        If drFila.Item("IdCompania") = "000000000060" Then
                            cadena = drFila.Item("DescripcionModelo") + " MARCA " + drFila.Item("Marca") + " MODELO " + cadena
                        Else

                        End If
                    Catch ex As Exception

                    End Try

                    If iProducto <= nfilaProducto Then
                        'llenando las filas existentes
                        Dim row As TableRow = tableProducto.Elements(Of TableRow)().ElementAt(iProducto - 1)
                        Dim cell As TableCell = row.Elements(Of TableCell)().ElementAt(0)
                        Dim para As Paragraph = cell.Elements(Of Paragraph)().First()
                        Dim run As Run = para.AppendChild(New Run)
                        'run.AppendChild(New Text(String.Concat(drFila.Item("Descripcion"), " - ", drFila.Item("DescripcionModelo"))))
                        run.AppendChild(New Text(cadena))
                        run.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                        'Alinear a la izquierda
                        Dim paragraphProperties2 As ParagraphProperties = para.Elements(Of ParagraphProperties).FirstOrDefault()
                        If paragraphProperties2 Is Nothing Then
                            paragraphProperties2 = run.AppendChild(New ParagraphProperties)
                        End If
                        Dim justification2 As Justification = New Justification() With {.Val = JustificationValues.Left}
                        paragraphProperties2.Justification = justification2

                    Else
                        'insertando y llegando las filas faltantes
                        Dim row As New TableRow()
                        Dim cell As New TableCell()
                        cell.Append(New TableCellProperties(New TableCellWidth()))

                        Dim para As Paragraph = cell.AppendChild(New Paragraph)
                        Dim run As Run = para.AppendChild(New Run)
                        'run.AppendChild(New Text(String.Concat(drFila.Item("Descripcion"), " - ", drFila.Item("DescripcionModelo"))))
                        run.AppendChild(New Text(cadena))
                        run.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone)

                        'Alinear a la izquierda
                        Dim paragraphProperties2 As ParagraphProperties = para.Elements(Of ParagraphProperties).FirstOrDefault()
                        If paragraphProperties2 Is Nothing Then
                            paragraphProperties2 = run.AppendChild(New ParagraphProperties)
                        End If
                        Dim justification2 As Justification = New Justification() With {.Val = JustificationValues.Left}
                        paragraphProperties2.Justification = justification2

                        row.Append(cell)
                        tableProducto.Append(row)
                    End If
                    iProducto = iProducto + 1

                Next ' Fin de Tabla Productos

                wordDoc.MainDocumentPart.Document.Save()

            End Using

            'Escribir en los marcadores
            Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStream, IO.FileMode.Open, IO.FileAccess.ReadWrite)

                'Recupera el xml del contenido del documento
                uriPartTarget = New Uri("/word/document.xml", UriKind.Relative)
                msPackage = package.GetPart(uriPartTarget)
                xmlDocumento.Load(msPackage.GetStream)

                'Se crea el navegador
                xpnDocumento = xmlDocumento.CreateNavigator()
                xnmDocumento = New System.Xml.XmlNamespaceManager(xpnDocumento.NameTable)
                xnmDocumento.AddNamespace("w", strUri)

                Dim dtbMarcadores As New DataTable
                Dim obcTablaMaestra As New bcTablaMaestra
                Dim ebeMarcadorCotizacion As New beMarcadorCotizacion
                Dim listaMarcador As New List(Of beMarcadorCotizacion)
                Dim xListaTelefonoResponsable As New List(Of beTelefonoResponsable)

                Dim contEliminados As Integer = 0

                ebeMarcadorCotizacion.IdArchivoConfiguracion = IdArchivoConfig
                obcTablaMaestra.MarcadorBuscarIdArchivoConfig(Modulo.strConexionSql, ebeMarcadorCotizacion, listaMarcador)

                If listaMarcador.Count > 0 Then
                    For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                        strCampo = String.Empty
                        If xpnCampo.MoveToChild("name", strUri) Then
                            If xpnCampo.MoveToAttribute("val", strUri) Then
                                strCampo = xpnCampo.Value
                            End If

                            Dim reg = From fila In listaMarcador _
                                     Where fila.NombreMarcadorCotizacion = strCampo Select fila

                            'MARCADORES DINAMICOS
                            If strCampo.ToUpper = "TELEFONO" Then
                                eTelefonoResponsable.IdCotizacion = dtbDatos.Rows(0).Item("IdCotizacion")
                                sw = cTelefonoResponsable.TelefonoResponsableListar(Modulo.strConexionSql, eTelefonoResponsable, listaTelefonoResponsable)
                                If listaTelefonoResponsable.Count > 0 Then
                                    Dim obeHomologacion As New beHomologacion
                                    Dim listaHomologacion As New List(Of beHomologacion)
                                    Dim obcHomologacion As New bcHomologacion
                                    Dim valorHomologacion As String = ""
                                    Dim arrayValor As String()


                                    For Each elemento As beTelefonoResponsable In listaTelefonoResponsable
                                        Dim xTelefonoResponsable As New beTelefonoResponsable
                                        obeHomologacion.Tabla = TablaHomologacion.COD_TIPO_TELEFONO_RESPONSABLE  ' tabla
                                        obeHomologacion.ValorSap = elemento.CodTipoTelefono
                                        listaHomologacion.Clear()
                                        listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                                        xTelefonoResponsable.CodTipoTelefono = elemento.CodTipoTelefono
                                        xTelefonoResponsable.Anexo = elemento.Anexo
                                        xTelefonoResponsable.NroTelefono = elemento.NroTelefono

                                        If listaHomologacion.Count > 0 Then
                                            obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                                            valorHomologacion = obeHomologacion.ValorCotizador
                                            arrayValor = Split(valorHomologacion, "-")

                                            xTelefonoResponsable.xOrden = arrayValor(0)
                                            xTelefonoResponsable.xEtiqueta = arrayValor(1)
                                        Else
                                            valorHomologacion = String.Empty
                                        End If
                                        xListaTelefonoResponsable.Add(xTelefonoResponsable)
                                    Next
                                    xListaTelefonoResponsable = xListaTelefonoResponsable.OrderBy(Function(x) x.xOrden).ToList()
                                End If
                            End If
                            '----

                            If sw Then
                                'Move to w:instrText
                                If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                    'Check FORMTEXT
                                    If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                        'Move to t
                                        If xpnCampo.MoveToFollowing("t", strUri) Then
                                            'Set value
                                        End If
                                    End If
                                End If
                                If xListaTelefonoResponsable.Count > 0 Then
                                    Dim cadena As String = xListaTelefonoResponsable.First.xEtiqueta + " "
                                    cadena = cadena + xListaTelefonoResponsable.First.NroTelefono
                                    If xListaTelefonoResponsable.First.Anexo.Trim <> "" Then
                                        cadena = cadena + " / Anexo: " + xListaTelefonoResponsable.First.Anexo
                                    End If
                                    xpnCampo.SetValue(cadena)
                                    xListaTelefonoResponsable.Remove(xListaTelefonoResponsable.First)
                                Else
                                    xpnCampo.DeleteSelf()
                                    contEliminados = contEliminados + 1
                                End If
                            End If

                            If Not reg.Count.Equals(0) And Not sw Then
                                Try
                                    valorTablaDatos = dtbDatos.Rows(0).Item(reg.First.NombreMarcador)
                                    ExiteCampoTabla = True
                                Catch ex As Exception
                                    ExiteCampoTabla = False
                                End Try

                                If ExiteCampoTabla And Not sw Then
                                    'Move to w:instrText
                                    If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                        'Check FORMTEXT
                                        If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                            'Move to t
                                            If xpnCampo.MoveToFollowing("t", strUri) Then
                                                'Set value
                                                xpnCampo.SetValue(valorTablaDatos)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next

                End If

                'xmlDocumento.Save(msPackage.GetStream(IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))
                'msPackage.GetStream.Flush()

                If listaMarcador.Count > 0 Then
                    For j As Integer = 1 To contEliminados
                        For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                            If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                'Check FORMTEXT
                                If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                    'Move to t
                                    If Not xpnCampo.MoveToFollowing("t", strUri) Then
                                        If xpnCampo.MoveToParent() Then
                                            If xpnCampo.MoveToParent() Then
                                                xpnCampo.DeleteSelf()
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If

                'Actualiza y cierra el documento
                xmlDocumento.Save(msPackage.GetStream(IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))
                msPackage.GetStream.Flush()
                msPackage.GetStream.Close()
            End Using
        Catch ex As Exception
            str_mensajeRecorrido.AppendLine("  ex : " & ex.Message.ToString & " - " & ex.StackTrace)
        End Try

    End Sub

    Private Sub GenerarPropuestaComercial(ByRef objMemoryStream As MemoryStream, ByVal dtbDatos As DataTable, ByVal dtbAdicionalProducto As DataTable, ByVal dtbProductoAccesorio As DataTable, ByVal dtbProductoAlquiler As DataTable, ByVal session As String)


        Dim sumaPresioDolar As Decimal = 0
        Dim sumaPresiosoles As Decimal = 0

        Dim valPrecioUnitSoles As Decimal = 0
        Dim valPrecioUnitDolares As Decimal = 0

        Dim valPrecioVentaTotalSoles As Decimal = 0
        Dim ValPrecioVentaTotalDolares As Decimal = 0

        Dim valIgvSoles As Decimal = 0
        Dim ValIgvDolares As Decimal = 0

        Dim valVentaSoles As String = String.Empty
        Dim valVentaDolares As Decimal = 0

        Dim valTipoCambioImpresion As String = String.Empty
        Dim valTipoCambioCalculo As Decimal = 0

        Dim valPrecioAccesorioSoles As Decimal = 0
        Dim valPrecioAccesorioDolares As Decimal = 0


        Dim OficinaResponsable As String = String.Empty  ' Solo una oficina para todo los productos cuando se crea la cotizacion
        Dim CodigoCompania As String = String.Empty  'el codigo de la compañia de la cotizacion
        Dim listaNombreMarcadores As New List(Of EstructuraDatos) 'Lista de Marcadores

        Dim obeHomologacion As New beHomologacion
        Dim listaHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion

        Dim MostrarGlosaOrvisa As Boolean = False
        Dim EsPrecioSoles As Boolean = False


        str_mensajeRecorrido.AppendLine(" 2- Propuesta Economica - " & Now.ToString())

        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
            '1.- LLENAR DATOS A LA TABLA DEL WORD
            Try
                If dtbDatos.Rows.Count > 0 Then

                    Dim indFilaTablaWord As Integer = 0
                    Dim sw As Boolean = True

                    '--- Recorrido de Los Productos ------------
                    For Each drFila As DataRow In dtbDatos.Rows

                        'Cambios 14/07 - CS - adicion a la descripción
                        Dim eCotizacion As New beCotizacion
                        Dim l_Producto As IEnumerable(Of beProducto)
                        Dim eProducto As beProducto
                        Dim result As String = ""
                        Dim l_eTarifaRS As New List(Of beTarifaRS)
                        Dim eTarifa As New beTarifaRS
                        'Dim obeHomologacion As New beHomologacion
                        'Dim listaHomologacion As New List(Of beHomologacion)
                        Dim valorHomologacion As String = ""
                        Dim arrayValor As String()
                        Dim cadena As String = drFila.Item("Descripcion") + " "

                        Dim cTarifaRS As New bcTarifaRS
                        Dim eDatoGeneral As New beDatoGeneral

                        Try
                            eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
                            l_Producto = eCotizacion.ListaProducto.Where(Function(c) c.IdProducto = drFila.Item("IdProducto"))
                            eProducto = CType(l_Producto.ToList, List(Of beProducto)).First

                            eDatoGeneral.Campo1 = ""
                            eDatoGeneral.Campo2 = drFila.Item("CodLinea")
                            eDatoGeneral.Campo3 = eProducto.IdProductoSap

                            cTarifaRS.BuscarCombinacionLlave(Modulo.strConexionSql, eDatoGeneral, l_eTarifaRS)
                            If Not l_eTarifaRS Is Nothing Then
                                Dim l_TarifaRS As IEnumerable(Of beTarifaRS) = l_eTarifaRS.Where(Function(c) c.IdTarifas = eProducto.IdTarifaRS)
                                eTarifa = CType(l_TarifaRS.ToList, List(Of beTarifaRS)).First

                                obeHomologacion.Tabla = TablaHomologacion.CAMPOS_ADICIONALES_POR_LINEA  ' tabla
                                obeHomologacion.ValorSap = drFila.Item("CodLinea")
                                listaHomologacion.Clear()
                                listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                                If listaHomologacion.Count > 0 Then
                                    obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                                    valorHomologacion = obeHomologacion.ValorCotizador
                                    arrayValor = Split(valorHomologacion, ",")
                                    For i = 0 To arrayValor.Length - 1
                                        cadena = cadena + DataBinder.Eval(eTarifa, arrayValor(i)) + " "
                                    Next
                                Else
                                    valorHomologacion = String.Empty
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                        '-------------------------------

                        Try
                            If drFila.Item("IdCompania") = "000000000060" Then
                                cadena = drFila.Item("DescripcionModelo") + " MARCA " + drFila.Item("Marca") + " MODELO " + cadena
                            End If
                        Catch ex As Exception

                        End Try

                        valTipoCambioImpresion = drFila.Item("ValorTipoCambio")
                        valTipoCambioCalculo = drFila.Item("ValorTipoCambio")
                        OficinaResponsable = drFila.Item("OficinaResponsable")   ' Es la Misma oficina para todos los productos
                        CodigoCompania = drFila.Item("IdCompania")   ' Es la Misma compañia para todos los productos

                        Dim cantidadAdicionales As Integer = 0
                        Dim cantidadAccesorio As Integer = 0
                        Dim tablaPropuesta As Table

                        If indFilaTablaWord = 0 Then
                            tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                        Else
                            tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1).Clone()
                            sw = False
                        End If


                        '---- CALCULOS ------------------------------
                        If valTipoCambioCalculo < 0 Then
                            ' Si es negativo se dividirá con el tipo de cambio
                            valTipoCambioImpresion = "/" & FormatoMoneda((-1) * CDec(valTipoCambioImpresion))
                            valTipoCambioCalculo = 1 / ((-1) * valTipoCambioCalculo)

                            valPrecioUnitSoles = drFila.Item("ValorUnitario")
                            valPrecioUnitDolares = ConvertirATipoCambio(valPrecioUnitSoles, valTipoCambioCalculo) ' valPrecioUnitSoles * valTipoCambioCalculo

                            valVentaSoles = drFila.Item("ValorVenta")
                            valVentaDolares = ConvertirATipoCambio(valVentaSoles, valTipoCambioCalculo)

                            EsPrecioSoles = True

                            If drFila("TipoProducto") = TipoProducto.CSA Then
                                'valIgvSoles = drFila.Item("ValorNeto") * drFila.Item("PorcImpuesto")

                                'Debe Calcularse por el PorcImpuesto
                                valIgvSoles = drFila.Item("ValorNeto") * (0.18)
                                'valIgvSoles = valIgvSoles / drFila.Item("Cantidad")
                                ValIgvDolares = (valIgvSoles * valTipoCambioCalculo)
                            Else
                                valIgvSoles = drFila.Item("ValorImpuesto")
                                'valIgvSoles = valIgvSoles / drFila.Item("Cantidad")
                                ValIgvDolares = (valIgvSoles * valTipoCambioCalculo)
                            End If

                        Else
                            EsPrecioSoles = False
                            valTipoCambioImpresion = FormatoMoneda(valTipoCambioImpresion)

                            valPrecioUnitDolares = drFila.Item("ValorUnitario")
                            valPrecioUnitSoles = ConvertirATipoCambio(valPrecioUnitDolares, valTipoCambioCalculo) 'valPrecioUnitDolares * valTipoCambioCalculo

                            valVentaDolares = drFila.Item("ValorVenta")
                            valVentaSoles = ConvertirATipoCambio(valVentaDolares, valTipoCambioCalculo)

                            'Caluclo del Valor de IGV
                            If drFila("TipoProducto") = TipoProducto.CSA Then
                                'ValIgvDolares = drFila.Item("ValorNeto") * drFila.Item("PorcImpuesto")

                                'Debe Calcularse por el PorcImpuesto
                                ValIgvDolares = drFila.Item("ValorNeto") * (0.18)
                                'ValIgvDolares = ValIgvDolares / drFila.Item("Cantidad")
                                valIgvSoles = ConvertirATipoCambio(ValIgvDolares, valTipoCambioCalculo)

                            Else
                                ValIgvDolares = drFila.Item("ValorImpuesto")
                                'ValIgvDolares = ValIgvDolares / drFila.Item("Cantidad")
                                valIgvSoles = ConvertirATipoCambio(ValIgvDolares, valTipoCambioCalculo)
                            End If

                        End If

                        'Calculo de Precio venta Total
                        'ValPrecioVentaTotalDolares = (valPrecioUnitDolares + ValIgvDolares) '* drFila.Item("Cantidad")
                        'valPrecioVentaTotalSoles = (valPrecioUnitSoles + valIgvSoles) '* drFila.Item("Cantidad")
                        ValPrecioVentaTotalDolares = (valVentaDolares + ValIgvDolares) '* drFila.Item("Cantidad")
                        valPrecioVentaTotalSoles = (valVentaSoles + valIgvSoles) '* drFila.Item("Cantidad")

                        'Valores para los marcadores
                        'sumaPresioDolar += ValPrecioVentaTotalDolares
                        'sumaPresiosoles += valPrecioVentaTotalSoles

                        If drFila.Item("TipoProducto") = TipoProducto.ALQUILER Then
                            '------Recorrido de la tabla de Datos Alquileres----------
                            Dim maquinas As String = String.Empty
                            For indFila As Integer = 0 To 8

                                Dim fila As TableRow = tablaPropuesta.Elements(Of TableRow)().ElementAt(indFila)

                                'Casos Por cada Fila
                                Select Case indFila
                                    Case 0 ' Fila: Nombre de Producto

                                        Dim tccelda_Fila0 As TableCell = fila.Elements(Of TableCell)().ElementAt(0)
                                        Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                                        If parag_Fila0 Is Nothing Then
                                            parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                                        End If

                                        Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                                        If run_Fila0 Is Nothing Then
                                            run_Fila0 = parag_Fila0.AppendChild(New Run)
                                        End If
                                        run_Fila0.RemoveAllChildren()
                                        'run_Fila0.AppendChild(New Text(String.Concat(drFila.Item("Descripcion"), " - ", drFila.Item("DescripcionModelo"))))
                                        run_Fila0.AppendChild(New Text(cadena))
                                        run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                        '--Alinear a la Izquierda
                                        Dim paragraphProperties2 As ParagraphProperties = parag_Fila0.Elements(Of ParagraphProperties).FirstOrDefault()
                                        If paragraphProperties2 Is Nothing Then
                                            paragraphProperties2 = run_Fila0.AppendChild(New ParagraphProperties)
                                        End If
                                        Dim justification2 As Justification = New Justification() With {.Val = JustificationValues.Left}
                                        paragraphProperties2.Justification = justification2

                                        Dim runProps1 As RunProperties = RunPropCeldaTablaTexto24.Clone()  'run_Fila0.AppendChild(New RunProperties())
                                        Dim linebreak1 As Break = New Break()
                                        runProps1.AppendChild(linebreak1)
                                        run_Fila0.AppendChild(runProps1)

                                        '-----Escribir los Accesorios-----------------------------------------
                                        Dim dtrAccesorio() As DataRow
                                        Dim intPosicionAccesorio As Integer = 3 '+ cantidadAdicionales
                                        dtrAccesorio = dtbProductoAccesorio.Select("IdProducto = " & drFila("IdProducto").ToString)

                                        For Each dr As DataRow In dtrAccesorio

                                            Dim textoAccesorio = New Text()
                                            Dim sbtextoAccesorio As New StringBuilder

                                            sbtextoAccesorio.Append(" - ")
                                            sbtextoAccesorio.Append(dr("Nombre").ToString.Trim)
                                            sbtextoAccesorio.Append(" ( ")
                                            sbtextoAccesorio.Append(dr("Cantidad").ToString.Trim)
                                            sbtextoAccesorio.Append(" ")
                                            sbtextoAccesorio.Append(dr("UnidadMedida").ToString.Trim)
                                            sbtextoAccesorio.Append(")")

                                            If cantidadAccesorio = 0 Then
                                                run_Fila0.AppendChild(New Text("Accesorio(s): "))
                                                run_Fila0.AppendChild(New RunProperties(New Break()))

                                            End If
                                            textoAccesorio.Text = sbtextoAccesorio.ToString()

                                            run_Fila0.AppendChild(textoAccesorio)
                                            run_Fila0.AppendChild(New RunProperties(New Break()))

                                            cantidadAccesorio += 1

                                        Next
                                        '-----------------------------------------
                                        '-----Construir nombre de los adicionales------- 
                                        For Each dr As DataRow In dtbAdicionalProducto.Select("IdProducto = " & drFila("IdProducto").ToString)
                                            Dim textoProducto = New Text()
                                            textoProducto.Text = String.Concat("- ", dr("Nombre").ToString.Trim)
                                            textoProducto.Text = String.Concat(textoProducto.Text, String.Concat(" (", FormatoMoneda(dr("Cantidad").ToString.Trim)))
                                            textoProducto.Text = String.Concat(textoProducto.Text, String.Concat(" ", dr("UnidadMedida").ToString.Trim))
                                            textoProducto.Text = String.Concat(textoProducto.Text, ")")

                                            If cantidadAdicionales = 0 Then
                                                run_Fila0.AppendChild(New Text("Beneficios Adicionales: "))
                                                run_Fila0.AppendChild(New RunProperties(New Break()))

                                            End If
                                            run_Fila0.AppendChild(textoProducto)

                                            Dim runProps2 As RunProperties = run_Fila0.AppendChild(New RunProperties())
                                            Dim linebreak2 As Break = New Break()

                                            runProps2.AppendChild(linebreak2)
                                            cantidadAdicionales = cantidadAdicionales + 1
                                        Next

                                        'Eliminar el Ultimo Salto de Linea
                                        run_Fila0.Elements(Of RunProperties).Last().Elements(Of Break).Last().Remove()

                                        Exit Select
                                    Case 1 ' Es el Titulo: No se escribe Nada
                                        Exit Select

                                    Case Else
                                        'Columna 0

                                        'Columna 1
                                        Dim tccelda1_Fila As TableCell = fila.Elements(Of TableCell)().ElementAt(1)
                                        Dim parag1_Fila As Paragraph = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                                        If parag1_Fila Is Nothing Then
                                            parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                                        End If

                                        Dim run1_Fila As Run = parag1_Fila.Elements(Of Run).FirstOrDefault

                                        If run1_Fila Is Nothing Then
                                            run1_Fila = parag1_Fila.AppendChild(New Run)
                                        End If

                                        'Columna 2
                                        Dim tccelda2_Fila As TableCell = fila.Elements(Of TableCell)().ElementAt(2)
                                        Dim parag2_Fila As Paragraph = tccelda2_Fila.Elements(Of Paragraph)().FirstOrDefault

                                        If parag2_Fila Is Nothing Then
                                            parag2_Fila = tccelda2_Fila.AppendChild(New Paragraph)
                                        End If

                                        Dim run2_Fila As Run = parag2_Fila.Elements(Of Run).FirstOrDefault

                                        If run2_Fila Is Nothing Then
                                            run2_Fila = parag2_Fila.AppendChild(New Run)
                                        End If

                                        Dim TextoCelda1 As String = String.Empty
                                        Dim TextoCelda2 As String = String.Empty

                                        Select Case indFila

                                            Case 2
                                                TextoCelda1 = FormatoMoneda(valPrecioUnitDolares)
                                                TextoCelda2 = FormatoMoneda(valPrecioUnitSoles)

                                                Exit Select
                                            Case 3
                                                TextoCelda1 = drFila.Item("Cantidad")
                                                TextoCelda2 = drFila.Item("Cantidad")
                                                'Exit Select
                                                'Dim dtrProductoAlquiler() As DataRow
                                                'dtrProductoAlquiler = dtbProductoAlquiler.Select("IdProducto = " & drFila("IdProducto").ToString)
                                                'For Each dr As DataRow In dtrProductoAlquiler
                                                '    TextoCelda1 = dr("DesMesAlquilar").ToString
                                                '    TextoCelda2 = dr("DesMesAlquilar").ToString
                                                '    Exit For
                                                'Next

                                                'TextoCelda1 = dtbProductoAlquiler.Rows(0).Item("DesMesAlquilar")
                                                'TextoCelda1 = dtrProductoAlquiler(0).ItemArray(12)
                                                Exit Select
                                            Case 4
                                                TextoCelda1 = FormatoMoneda(valVentaDolares)
                                                TextoCelda2 = FormatoMoneda(valVentaSoles)
                                                'TextoCelda1 = drFila.Item("Cantidad")
                                                'TextoCelda2 = drFila.Item("Cantidad")
                                                Exit Select
                                            Case 5
                                                If ValIgvDolares <= 0 Or valIgvSoles <= 0 Then
                                                    MostrarGlosaOrvisa = True
                                                End If
                                                TextoCelda1 = FormatoMoneda(ValIgvDolares)
                                                TextoCelda2 = FormatoMoneda(valIgvSoles)
                                                'TextoCelda1 = FormatoMoneda(valPrecioUnitDolares + ValIgvDolares)
                                                'TextoCelda2 = FormatoMoneda(valPrecioUnitSoles + valIgvSoles)
                                                'TextoCelda1 = FormatoMoneda(valVentaDolares)
                                                'TextoCelda2 = FormatoMoneda(valVentaSoles)
                                                Exit Select
                                            Case 6

                                                TextoCelda1 = FormatoMoneda(valVentaDolares + ValIgvDolares)
                                                TextoCelda2 = FormatoMoneda(valVentaSoles + valIgvSoles)
                                                'If ValIgvDolares <= 0 Or valIgvSoles <= 0 Then
                                                '    MostrarGlosaOrvisa = True
                                                'End If
                                                'TextoCelda1 = FormatoMoneda(ValIgvDolares)
                                                'TextoCelda2 = FormatoMoneda(valIgvSoles)
                                                Exit Select
                                            Case 7
                                                Dim dtrProductoAlquiler() As DataRow
                                                dtrProductoAlquiler = dtbProductoAlquiler.Select("IdProducto = " & drFila("IdProducto").ToString)
                                                For Each dr As DataRow In dtrProductoAlquiler
                                                    TextoCelda1 = dr("DesMesAlquilar").ToString
                                                    TextoCelda2 = dr("DesMesAlquilar").ToString
                                                    maquinas = dr("DesMesAlquilar").ToString
                                                    Exit For
                                                Next
                                                'TextoCelda1 = FormatoMoneda(ValPrecioVentaTotalDolares)
                                                'TextoCelda2 = FormatoMoneda(valPrecioVentaTotalSoles)
                                                Exit Select
                                            Case 8
                                                TextoCelda1 = FormatoMoneda(ValPrecioVentaTotalDolares * Val(maquinas))
                                                TextoCelda2 = FormatoMoneda(valPrecioVentaTotalSoles * Val(maquinas))
                                                sumaPresioDolar = sumaPresioDolar + (ValPrecioVentaTotalDolares * Val(maquinas))
                                                sumaPresiosoles = sumaPresiosoles + (valPrecioVentaTotalSoles * Val(maquinas))
                                                Exit Select
                                            Case Else
                                        End Select

                                        run1_Fila.RemoveAllChildren()
                                        run1_Fila.AppendChild(New Text(TextoCelda1))
                                        run1_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                        run2_Fila.RemoveAllChildren()
                                        run2_Fila.AppendChild(New Text(TextoCelda2))
                                        run2_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone()) 'RunPropCeldaTablaCalculos20


                                End Select
                            Next
                        Else
                            '------Recorrido de la tabla de Datos-------------------
                            For indFila As Integer = 0 To 6

                                Dim fila As TableRow = tablaPropuesta.Elements(Of TableRow)().ElementAt(indFila)

                                'Casos Por cada Fila
                                Select Case indFila
                                    Case 0 ' Fila: Nombre de Producto

                                        Dim tccelda_Fila0 As TableCell = fila.Elements(Of TableCell)().ElementAt(0)
                                        Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                                        If parag_Fila0 Is Nothing Then
                                            parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                                        End If

                                        Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                                        If run_Fila0 Is Nothing Then
                                            run_Fila0 = parag_Fila0.AppendChild(New Run)
                                        End If
                                        run_Fila0.RemoveAllChildren()
                                        'run_Fila0.AppendChild(New Text(String.Concat(drFila.Item("Descripcion"), " - ", drFila.Item("DescripcionModelo"))))
                                        run_Fila0.AppendChild(New Text(cadena))
                                        run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                        '--Alinear a la Izquierda
                                        Dim paragraphProperties2 As ParagraphProperties = parag_Fila0.Elements(Of ParagraphProperties).FirstOrDefault()
                                        If paragraphProperties2 Is Nothing Then
                                            paragraphProperties2 = run_Fila0.AppendChild(New ParagraphProperties)
                                        End If
                                        Dim justification2 As Justification = New Justification() With {.Val = JustificationValues.Left}
                                        paragraphProperties2.Justification = justification2

                                        Dim runProps1 As RunProperties = RunPropCeldaTablaTexto24.Clone()  'run_Fila0.AppendChild(New RunProperties())
                                        Dim linebreak1 As Break = New Break()
                                        runProps1.AppendChild(linebreak1)
                                        run_Fila0.AppendChild(runProps1)

                                        '-----Escribir los Accesorios-----------------------------------------
                                        Dim dtrAccesorio() As DataRow
                                        Dim intPosicionAccesorio As Integer = 3 '+ cantidadAdicionales
                                        dtrAccesorio = dtbProductoAccesorio.Select("IdProducto = " & drFila("IdProducto").ToString)

                                        For Each dr As DataRow In dtrAccesorio

                                            Dim textoAccesorio = New Text()
                                            Dim sbtextoAccesorio As New StringBuilder

                                            sbtextoAccesorio.Append(" - ")
                                            sbtextoAccesorio.Append(dr("Nombre").ToString.Trim)
                                            sbtextoAccesorio.Append(" ( ")
                                            sbtextoAccesorio.Append(dr("Cantidad").ToString.Trim)
                                            sbtextoAccesorio.Append(" ")
                                            sbtextoAccesorio.Append(dr("UnidadMedida").ToString.Trim)
                                            sbtextoAccesorio.Append(")")

                                            If cantidadAccesorio = 0 Then
                                                run_Fila0.AppendChild(New Text("Accesorio(s): "))
                                                run_Fila0.AppendChild(New RunProperties(New Break()))

                                            End If
                                            textoAccesorio.Text = sbtextoAccesorio.ToString()

                                            run_Fila0.AppendChild(textoAccesorio)
                                            run_Fila0.AppendChild(New RunProperties(New Break()))

                                            cantidadAccesorio += 1

                                        Next
                                        '-----------------------------------------
                                        '-----Construir nombre de los adicionales------- 
                                        For Each dr As DataRow In dtbAdicionalProducto.Select("IdProducto = " & drFila("IdProducto").ToString)
                                            Dim textoProducto = New Text()
                                            textoProducto.Text = String.Concat("- ", dr("Nombre").ToString.Trim)
                                            textoProducto.Text = String.Concat(textoProducto.Text, String.Concat(" (", FormatoMoneda(dr("Cantidad").ToString.Trim)))
                                            textoProducto.Text = String.Concat(textoProducto.Text, String.Concat(" ", dr("UnidadMedida").ToString.Trim))
                                            textoProducto.Text = String.Concat(textoProducto.Text, ")")

                                            If cantidadAdicionales = 0 Then
                                                run_Fila0.AppendChild(New Text("Beneficios Adicionales: "))
                                                run_Fila0.AppendChild(New RunProperties(New Break()))

                                            End If
                                            run_Fila0.AppendChild(textoProducto)

                                            Dim runProps2 As RunProperties = run_Fila0.AppendChild(New RunProperties())
                                            Dim linebreak2 As Break = New Break()

                                            runProps2.AppendChild(linebreak2)
                                            cantidadAdicionales = cantidadAdicionales + 1
                                        Next

                                        'Eliminar el Ultimo Salto de Linea
                                        run_Fila0.Elements(Of RunProperties).Last().Elements(Of Break).Last().Remove()

                                        Exit Select
                                    Case 1 ' Es el Titulo: No se escribe Nada
                                        Exit Select

                                    Case Else
                                        'Columna 0

                                        'Columna 1
                                        Dim tccelda1_Fila As TableCell = fila.Elements(Of TableCell)().ElementAt(1)
                                        Dim parag1_Fila As Paragraph = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                                        If parag1_Fila Is Nothing Then
                                            parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                                        End If

                                        Dim run1_Fila As Run = parag1_Fila.Elements(Of Run).FirstOrDefault

                                        If run1_Fila Is Nothing Then
                                            run1_Fila = parag1_Fila.AppendChild(New Run)
                                        End If

                                        'Columna 2
                                        Dim tccelda2_Fila As TableCell = fila.Elements(Of TableCell)().ElementAt(2)
                                        Dim parag2_Fila As Paragraph = tccelda2_Fila.Elements(Of Paragraph)().FirstOrDefault

                                        If parag2_Fila Is Nothing Then
                                            parag2_Fila = tccelda2_Fila.AppendChild(New Paragraph)
                                        End If

                                        Dim run2_Fila As Run = parag2_Fila.Elements(Of Run).FirstOrDefault

                                        If run2_Fila Is Nothing Then
                                            run2_Fila = parag2_Fila.AppendChild(New Run)
                                        End If

                                        Dim TextoCelda1 As String = String.Empty
                                        Dim TextoCelda2 As String = String.Empty
                                        Select Case indFila

                                            Case 2
                                                TextoCelda1 = FormatoMoneda(valPrecioUnitDolares)
                                                TextoCelda2 = FormatoMoneda(valPrecioUnitSoles)

                                                Exit Select
                                            Case 3
                                                TextoCelda1 = drFila.Item("Cantidad")
                                                TextoCelda2 = drFila.Item("Cantidad")
                                                'If ValIgvDolares <= 0 Or valIgvSoles <= 0 Then
                                                '    MostrarGlosaOrvisa = True
                                                'End If
                                                'TextoCelda1 = FormatoMoneda(ValIgvDolares)
                                                'TextoCelda2 = FormatoMoneda(valIgvSoles)

                                                Exit Select
                                            Case 4
                                                'TextoCelda1 = FormatoMoneda(valPrecioUnitDolares + ValIgvDolares)
                                                'TextoCelda2 = FormatoMoneda(valPrecioUnitSoles + valIgvSoles)
                                                TextoCelda1 = FormatoMoneda(valVentaDolares)
                                                TextoCelda2 = FormatoMoneda(valVentaSoles)
                                                Exit Select
                                            Case 5
                                                If ValIgvDolares <= 0 Or valIgvSoles <= 0 Then
                                                    MostrarGlosaOrvisa = True
                                                End If
                                                TextoCelda1 = FormatoMoneda(ValIgvDolares)
                                                TextoCelda2 = FormatoMoneda(valIgvSoles)
                                                'TextoCelda1 = drFila.Item("Cantidad")
                                                'TextoCelda2 = drFila.Item("Cantidad")

                                                ''si es alquiler creo una nueva fila solo la primera vez
                                                'If drFila.Item("TipoProducto") = TipoProducto.ALQUILER And sw Then
                                                '    Dim nuevaFila As New TableRow()
                                                '    Dim filaTabla As New TableRow()
                                                '    Dim tccelda_Fila0 As New TableCell
                                                '    Dim tccelda_Fila1 As New TableCell
                                                '    filaTabla = tablaPropuesta.Elements(Of TableRow)().ElementAt(CInt(5))
                                                '    tccelda_Fila0 = fila.Elements(Of TableCell)().ElementAt(CInt(0))
                                                '    tccelda_Fila1 = fila.Elements(Of TableCell)().ElementAt(CInt(1))
                                                '    Dim nroCeldas As Integer = fila.Elements(Of TableCell).Count()
                                                '    For k As Integer = 1 To nroCeldas
                                                '        Select Case k
                                                '            Case 1
                                                '                Dim nuevaCelda As New TableCell(tccelda_Fila0.OuterXml)
                                                '                nuevaFila.Append(nuevaCelda)
                                                '            Case Else
                                                '                Dim nuevaCelda As New TableCell(tccelda_Fila1.OuterXml)
                                                '                nuevaFila.Append(nuevaCelda)
                                                '        End Select
                                                '    Next
                                                '    tablaPropuesta.InsertAt(nuevaFila, 8)
                                                'End If
                                                Exit Select
                                            Case 6
                                                ''si es prod. alquiler lleno los datos en la nueva fila
                                                'If drFila.Item("TipoProducto") = TipoProducto.ALQUILER Then

                                                '    Dim celda0 As TableCell = fila.Elements(Of TableCell)().ElementAt(0)
                                                '    Dim celda1 As TableCell = fila.Elements(Of TableCell)().ElementAt(1)
                                                '    Dim celda2 As TableCell = fila.Elements(Of TableCell)().ElementAt(2)
                                                '    Dim parag0 As Paragraph = celda0.Elements(Of Paragraph)().FirstOrDefault()
                                                '    If parag0 Is Nothing Then
                                                '        parag0 = celda0.AppendChild(New Paragraph)
                                                '    End If
                                                '    Dim parag1 As Paragraph = celda1.Elements(Of Paragraph)().FirstOrDefault()
                                                '    If parag1 Is Nothing Then
                                                '        parag1 = celda1.AppendChild(New Paragraph)
                                                '    End If
                                                '    Dim parag2 As Paragraph = celda2.Elements(Of Paragraph)().FirstOrDefault()
                                                '    If parag2 Is Nothing Then
                                                '        parag2 = celda2.AppendChild(New Paragraph)
                                                '    End If
                                                '    Dim run0 As Run = parag0.Elements(Of Run).FirstOrDefault
                                                '    If run0 Is Nothing Then
                                                '        run0 = parag0.AppendChild(New Run)
                                                '    End If
                                                '    Dim run1 As Run = parag1.Elements(Of Run).FirstOrDefault
                                                '    If run1 Is Nothing Then
                                                '        run1 = parag1.AppendChild(New Run)
                                                '    End If
                                                '    Dim run2 As Run = parag2.Elements(Of Run).FirstOrDefault
                                                '    If run2 Is Nothing Then
                                                '        run2 = parag2.AppendChild(New Run)
                                                '    End If
                                                '    run0.RemoveAllChildren()
                                                '    run0.AppendChild(New Text("Nro. Maquina"))
                                                '    run0.PrependChild(Of RunProperties)(RunPropertiesConfigurado(True, , , , , ).Clone())

                                                '    Dim nroMaq As String = dtbProductoAlquiler.Rows(0).Item("DesMesAlquilar")
                                                '    run1.RemoveAllChildren()
                                                '    run1.AppendChild(New Text(nroMaq))
                                                '    run1.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                                '    run2.RemoveAllChildren()
                                                '    run2.AppendChild(New Text(nroMaq))
                                                '    run2.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                                '    'preparo la ultima fila
                                                '    Dim fila_extra As TableRow = tablaPropuesta.Elements(Of TableRow)().ElementAt(7)
                                                '    tccelda1_Fila = fila_extra.Elements(Of TableCell)().ElementAt(1)
                                                '    parag1_Fila = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                                                '    If parag1_Fila Is Nothing Then
                                                '        parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                                                '    End If

                                                '    run1_Fila = parag1_Fila.Elements(Of Run).FirstOrDefault

                                                '    If run1_Fila Is Nothing Then
                                                '        run1_Fila = parag1_Fila.AppendChild(New Run)
                                                '    End If

                                                '    'Columna 2
                                                '    tccelda2_Fila = fila_extra.Elements(Of TableCell)().ElementAt(2)
                                                '    parag2_Fila = tccelda2_Fila.Elements(Of Paragraph)().FirstOrDefault

                                                '    If parag2_Fila Is Nothing Then
                                                '        parag2_Fila = tccelda2_Fila.AppendChild(New Paragraph)
                                                '    End If

                                                '    run2_Fila = parag2_Fila.Elements(Of Run).FirstOrDefault

                                                '    If run2_Fila Is Nothing Then
                                                '        run2_Fila = parag2_Fila.AppendChild(New Run)
                                                '    End If

                                                '    TextoCelda1 = String.Empty
                                                '    TextoCelda2 = String.Empty
                                                'End If

                                                'TextoCelda1 = FormatoMoneda((valPrecioUnitDolares + ValIgvDolares) * drFila.Item("Cantidad"))
                                                'TextoCelda2 = FormatoMoneda((valPrecioUnitSoles + valIgvSoles) * drFila.Item("Cantidad"))

                                                TextoCelda1 = FormatoMoneda(ValPrecioVentaTotalDolares)
                                                TextoCelda2 = FormatoMoneda(valPrecioVentaTotalSoles)
                                                sumaPresioDolar += ValPrecioVentaTotalDolares
                                                sumaPresiosoles += valPrecioVentaTotalSoles
                                                Exit Select
                                            Case Else
                                        End Select

                                        run1_Fila.RemoveAllChildren()
                                        run1_Fila.AppendChild(New Text(TextoCelda1))
                                        run1_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())

                                        run2_Fila.RemoveAllChildren()
                                        run2_Fila.AppendChild(New Text(TextoCelda2))
                                        run2_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone()) 'RunPropCeldaTablaCalculos20


                                End Select
                            Next
                        End If


                        '----------------------------------------------------------------------
                        ''-----Escribir los Accesorios-----------------------------------------
                        'Dim dtrAccesorio() As DataRow
                        'Dim intPosicionAccesorio As Integer = 3 '+ cantidadAdicionales
                        'dtrAccesorio = dtbProductoAccesorio.Select("IdProducto = " & drFila("IdProducto").ToString)

                        'For Each dr As DataRow In dtrAccesorio

                        '    Dim textoProduct As New StringBuilder
                        '    textoProduct.Append(" + ")
                        '    textoProduct.Append(dr("Nombre").ToString.Trim)
                        '    textoProduct.Append(" ( ")
                        '    textoProduct.Append(dr("Cantidad").ToString.Trim)
                        '    textoProduct.Append(" ")
                        '    textoProduct.Append(dr("UnidadMedida").ToString.Trim)
                        '    textoProduct.Append(")")


                        '    Dim trTemp As TableRow = tablaPropuesta.Elements(Of TableRow)().ElementAt(intPosicionAccesorio - 1).Clone()

                        '    'Columna 0
                        '    Dim tccelda_Fila0 As TableCell = trTemp.Elements(Of TableCell)().ElementAt(0)
                        '    Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                        '    If parag_Fila0 Is Nothing Then
                        '        parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                        '    End If

                        '    Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                        '    If run_Fila0 Is Nothing Then
                        '        run_Fila0 = parag_Fila0.AppendChild(New Run)
                        '    End If
                        '    run_Fila0.RemoveAllChildren()
                        '    run_Fila0.AppendChild(New Text(textoProduct.ToString))
                        '    run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                        '    'Columna 1
                        '    Dim tccelda1_Fila As TableCell = trTemp.Elements(Of TableCell)().ElementAt(1)
                        '    Dim parag1_Fila As Paragraph = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                        '    If parag1_Fila Is Nothing Then
                        '        parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                        '    End If

                        '    Dim run1_Fila As Run = parag1_Fila.Elements(Of Run).FirstOrDefault

                        '    If run1_Fila Is Nothing Then
                        '        run1_Fila = parag1_Fila.AppendChild(New Run)
                        '    End If

                        '    'Columna 2
                        '    Dim tccelda2_Fila As TableCell = trTemp.Elements(Of TableCell)().ElementAt(2)
                        '    Dim parag2_Fila As Paragraph = tccelda2_Fila.Elements(Of Paragraph)().FirstOrDefault

                        '    If parag2_Fila Is Nothing Then
                        '        parag2_Fila = tccelda2_Fila.AppendChild(New Paragraph)
                        '    End If

                        '    Dim run2_Fila As Run = parag2_Fila.Elements(Of Run).FirstOrDefault

                        '    If run2_Fila Is Nothing Then
                        '        run2_Fila = parag2_Fila.AppendChild(New Run)
                        '    End If

                        '    'obtener precio en soloes y dolares
                        '    If EsPrecioSoles = True Then
                        '        valPrecioAccesorioSoles = dr("ValorLista").ToString.Trim
                        '        valPrecioAccesorioDolares = ConvertirATipoCambio(valPrecioAccesorioSoles, valTipoCambioCalculo)
                        '    Else
                        '        valPrecioAccesorioDolares = dr.Item("ValorLista")
                        '        valPrecioAccesorioSoles = ConvertirATipoCambio(valPrecioAccesorioDolares, valTipoCambioCalculo)
                        '    End If

                        '    Dim valPrecioAccesorioDolaresImprim As String = ""
                        '    Dim valPrecioAccesorioSolesImprim As String = ""

                        '    'Validar precio de accesorio que se imprimiran
                        '    If valPrecioAccesorioSoles = 0 Then
                        '        valPrecioAccesorioSolesImprim = ""
                        '    Else
                        '        valPrecioAccesorioSolesImprim = "" 'FormatoMoneda(valPrecioAccesorioSoles)
                        '    End If

                        '    If valPrecioAccesorioDolares = 0 Then
                        '        valPrecioAccesorioDolaresImprim = ""
                        '    Else
                        '        valPrecioAccesorioDolaresImprim = "" 'FormatoMoneda(valPrecioAccesorioDolares)
                        '    End If

                        '    'Escribir en la tabla
                        '    run1_Fila.RemoveAllChildren()
                        '    run1_Fila.AppendChild(New Text(valPrecioAccesorioDolaresImprim))
                        '    run1_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                        '    run2_Fila.RemoveAllChildren()
                        '    run2_Fila.AppendChild(New Text(valPrecioAccesorioSolesImprim))
                        '    run2_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                        '    'tablaPropuesta.InsertAt(trTemp, intPosicionAccesorio)
                        '    tablaPropuesta.InsertAfter(trTemp, tablaPropuesta.Elements(Of TableRow)().ElementAt(intPosicionAccesorio - 1))

                        '    intPosicionAccesorio = intPosicionAccesorio + 1

                        'Next
                        '----------------------------------------------------------------------

                        'Insertar una nueva tabla en el documento si hay mas de un Producto
                        If indFilaTablaWord > 0 Then
                            Dim cantTablas As Integer = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table).Count
                            If cantTablas > 2 Then
                                'Salto de Linea
                                'Dim paragSaltoLinea As New Paragraph(New Run(New Break() With {.Type = BreakValues.TextWrapping}))
                                'Dim paragSaltoLinea As New Paragraph(New Run(New Break()))
                                'Dim TablaLast As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table).Last

                                wordDoc.MainDocumentPart.Document.Body.InsertBefore(tablaPropuesta, wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)().Last)
                            End If
                        End If
                        indFilaTablaWord = +1

                    Next
                End If
                wordDoc.MainDocumentPart.Document.Save()

                listaNombreMarcadores.Add(New EstructuraDatos With {.valor1 = "OficinaResponsable", .valor2 = OficinaResponsable})
                Call EscribirMarcadorDocumento(wordDoc, listaNombreMarcadores)
            Catch ex As Exception
            End Try
        End Using

        '2.- LLENAR DATOS A LOS MARCADORES--------------------------------------------------------
        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)

            For Each oBookmarkStart As BookmarkStart In wordDoc.MainDocumentPart.Document.Body.Descendants(Of BookmarkStart)() 'wordDoc.MainDocumentPart.Document.Body.Elements(Of Paragraph)().Select(Function(o) o.Elements(Of BookmarkStart)())

                Dim valorMarcador As String = String.Empty
                Select Case oBookmarkStart.Name.ToString
                    Case "TipoCambio"
                        valorMarcador = Modulo.FormatoMoneda(valTipoCambioImpresion)
                        Exit Select
                    Case "SUBTD"
                        valorMarcador = Modulo.FormatoMoneda(sumaPresioDolar)
                        Exit Select
                    Case "SUBTS"
                        valorMarcador = Modulo.FormatoMoneda(sumaPresiosoles)
                        Exit Select
                End Select
                If Not String.IsNullOrEmpty(valorMarcador) Then
                    Dim parent = oBookmarkStart.Parent ' bookmark's parent element

                    Dim parag = New Paragraph
                    Dim text = New Text(valorMarcador)
                    Dim run = New Run '(New RunProperties())
                    run.Append(text)


                    If oBookmarkStart.Name.ToString = "SUBTD" Or oBookmarkStart.Name.ToString = "SUBTS" Then
                        Dim rPr As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, "Arial", 24, "FFFFFF", ))
                        run.PrependChild(Of RunProperties)(rPr)
                    Else
                        run.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone)
                    End If

                    parent.InsertBefore(run, oBookmarkStart)
                End If

                wordDoc.MainDocumentPart.Document.Save()
            Next
            'wordDoc.MainDocumentPart.Document.Save()
        End Using
        '-----------------------------------------------------

        'validacion para GLOSA ORVISA---------------------
        Try

            If MostrarGlosaOrvisa = True Then
                obeHomologacion.Tabla = TablaHomologacion.GLOSA_ORVISA    ' tabla
                listaHomologacion.Clear()
                listaHomologacion = ListaTablaHomologacion(obeHomologacion)
                obeHomologacion = listaHomologacion.Where(Function(c) c.ValorSap = CodigoCompania).ToList().FirstOrDefault
                If Not obeHomologacion Is Nothing Then
                    Dim objMemoryStreamGlosa As MemoryStream = Nothing
                    objMemoryStreamGlosa = New MemoryStream

                    Dim NombreArchivo As String = "Otros/GlosaOrvisa.docx"
                    objMemoryStreamGlosa = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
                    If Not objMemoryStreamGlosa Is Nothing Then
                        If objMemoryStreamGlosa.Length > 0 Then
                            Dim ListaByteDocumentoTemp As New List(Of Byte())
                            ListaByteDocumentoTemp.Add(objMemoryStream.ToArray)
                            ListaByteDocumentoTemp.Add(objMemoryStreamGlosa.ToArray)

                            Dim rtemp As Byte() = UnirDocumentos(ListaByteDocumentoTemp)

                            Dim msDocGenerado As New MemoryStream
                            msDocGenerado.Write(rtemp, 0, rtemp.Length)

                            objMemoryStream = Nothing
                            objMemoryStream = msDocGenerado

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            str_mensajeRecorrido.AppendLine("  ex : " & ex.Message.ToString & " - " & ex.StackTrace)
        End Try

        '----------------------------------------
    End Sub

    Private Sub GenerarCondicionesGenerales(ByRef objMemoryStream As MemoryStream, ByVal dtbDatos As DataTable, ByVal dtbAdicional As DataTable, ByVal dtbResumenPropuesta As DataTable, ByVal dtbCondicionesGeneralesAlquiler As DataTable, ByVal session As String)
        Dim NombreArchivo As String = String.Empty
        Dim objMemoryStreamTemp As New MemoryStream
        Dim ListaByteDocumentoTemp As New List(Of Byte())
        Dim swProducLink As Boolean = False
        Dim swVigenciaSC As Boolean = False 'No aparece parrafo
        Dim swCLC As Boolean = True
        Dim obeHomologacion As New beHomologacion
        Dim listaHomologacion As New List(Of beHomologacion)

        str_mensajeRecorrido.AppendLine(" 3- Condiciones generales - " & Now.ToString())

        'validacion marca = CATERPILLAR
        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
            If dtbResumenPropuesta.Rows.Count > 0 Then
                If dtbResumenPropuesta.Rows(0).Item("TipoProducto") = TipoProducto.SOLUCION_COMBINADA Then
                    swProducLink = True

                    obeHomologacion.Tabla = TablaHomologacion.INCLUYE_CUADRO_CONDICIONES_ESPECIFICAS_SC  ' tabla
                    obeHomologacion.ValorSap = dtbResumenPropuesta.Rows(0).Item("CodLinea")
                    listaHomologacion.Clear()
                    listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                    If listaHomologacion.Count = 0 Then
                        swCLC = False
                    End If

                    obeHomologacion.Tabla = TablaHomologacion.INCLUYE_CUADRO_VIGENCIA_SC  ' tabla
                    obeHomologacion.ValorSap = dtbResumenPropuesta.Rows(0).Item("CodLinea")
                    listaHomologacion.Clear()
                    listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)

                    If listaHomologacion.Count > 0 Then
                        swVigenciaSC = True 'Aparece Parrafo
                    End If
                End If
                'For Each drPrime As DataRow In dtbDatos.Rows
                'If drPrime.Item("CodClasificacion") = "01" Then 'Verifica solo Prime-Nuevos

                'For indiFila As Integer = 0 To dtbResumenPropuesta.Rows.Count - 1
                '    Dim drFila As DataRow = dtbResumenPropuesta.Rows(indiFila)
                '    obeHomologacion.ValorSap = drFila.Item("CodLinea")
                '    listaHomologacion.Clear()
                '    listaHomologacion = objImpresionExtender.ListaTablaHomologacionSC(obeHomologacion)
                '    If listaHomologacion.Count > 0 Then

                '        Exit For
                '    End If
                'Next
                'Exit For
                'End If
                'Next

            End If
            If swProducLink Then
                wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1).Remove()
                If Not swVigenciaSC Then
                    wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1).Remove()
                End If
            Else
                If Not swVigenciaSC Then
                    wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(2).Remove()
                End If
            End If

        End Using

        '1.- Adjuntamos el primer Documento
        ListaByteDocumentoTemp.Add(objMemoryStream.ToArray)

        '2.- Verificar si hay algun producto que incluye CLC
        For Each drFila As DataRow In dtbDatos.Rows
            If drFila.Item("IncluyeCLC") = True Or drFila.Item("IncluyeCLC") = "1" Then ' Solo si incluye CLC
                ProductosIncluyenClC = True
                Exit For
            End If
        Next

        '3.- B.2.1. Financiamiento Equipo Nuevo        
        If swCLC AndAlso dtbDatos.Rows.Count > 0 Then ' si hay producto prime            

            If ProductosIncluyenClC = True Then
                '3.- Cargar datos de Parte Fija  
                NombreArchivo = "CLC/CuadroCLCFijo.docx"
                objMemoryStreamTemp = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)

                Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStreamTemp, True)
                    Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                    Dim CantRegTablaWord As Integer = tablaWord.Elements(Of TableRow).Count()
                    For i As Integer = 0 To CantRegTablaWord - 1
                        Dim valCelda2 As String = String.Empty
                        Select Case i
                            Case 0
                                If Not dtbDatos.Rows(0).Item("Cliente") Is DBNull.Value Then valCelda2 = dtbDatos.Rows(0).Item("Cliente")
                                Exit Select
                            Case 1
                                If Not dtbDatos.Rows(0).Item("Vendedor") Is DBNull.Value Then valCelda2 = dtbDatos.Rows(0).Item("Vendedor")
                                Exit Select
                            Case Else
                                Exit Select
                        End Select
                        'Escribe en las celdas de la fila
                        Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                        Dim cantColum = fila.Elements(Of TableCell).Count()
                        For j As Integer = 0 To cantColum - 1
                            Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                            Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                            Dim run As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))

                            Dim TextoCelda As String = String.Empty

                            Select Case j
                                Case 0
                                    'no se escribe nada
                                    Exit Select
                                Case 1
                                    TextoCelda = valCelda2
                                    Exit Select
                                Case Else
                                    TextoCelda = String.Empty
                            End Select

                            run.AppendChild(New Text(TextoCelda))
                            'Dim rPr As RunProperties = New RunProperties(RunPropertiesConfigurado())
                            'run.PrependChild(Of RunProperties)(rPr)
                        Next
                    Next
                End Using
                ListaByteDocumentoTemp.Add(objMemoryStreamTemp.ToArray)


                '4.-  construir el cuadro de Financiamiento
                str_mensajeRecorrido.AppendLine("  ConstruirFinanciamientoEquipo")
                Call ConstruirFinanciamientoEquipo(ListaByteDocumentoTemp, dtbDatos, session)


                '5.- Cargamos los datos de Especifiaciones
                str_mensajeRecorrido.AppendLine("  EspecificacionesCLC")
                objMemoryStreamTemp = Nothing
                NombreArchivo = "CLC/EspecificacionesCLC.docx"
                objMemoryStreamTemp = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
                ListaByteDocumentoTemp.Add(objMemoryStreamTemp.ToArray)
            End If

            '6.- para B.2.2. Condiciones Específicas de Equipo Nuevo
            str_mensajeRecorrido.AppendLine("  ConstruirCondicionEspecifica")
            Call ConstruirCondicionEspecifica(ListaByteDocumentoTemp, dtbDatos, dtbAdicional, session)
        End If

        'Cuadro Condiciones Específicas para Producto Alquiler
        If dtbResumenPropuesta.Rows(0).Item("TipoProducto") = TipoProducto.ALQUILER Then
            str_mensajeRecorrido.AppendLine("  ConstruirCondicionEspecifica")
            Call ConstruirCondicionEspecifica(ListaByteDocumentoTemp, dtbCondicionesGeneralesAlquiler, dtbAdicional, session)
        End If

        Dim rtemp As Byte() = UnirDocumentos(ListaByteDocumentoTemp)

        Dim msDocGenerado As New MemoryStream
        msDocGenerado.Write(rtemp, 0, rtemp.Length)

        objMemoryStream = Nothing
        objMemoryStream = msDocGenerado

    End Sub

    Private Sub ConstruirFinanciamientoEquipo(ByRef listaByteDocumento As List(Of Byte()), ByVal dtbDatos As DataTable, _
                                              ByVal session As String)

        '-- 14/11 VALORES INTERES DINAMICOS
        Dim listabeHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion
        Dim obcHomologacion As New ImpresionExtender
        Dim eCotizacion As New beCotizacion
        Dim swPlazoMeses As Boolean = False
        Dim INICIAL As String = "0"
        Dim IGV As String = "0"
        Dim FINANCE_FREE As String = "0"
        Dim PLAZOMESES1 As String = "0"
        Dim PLAZOMESES2 As String = "0"
        Dim TASA_INTERES1 As String = "0"
        Dim TASA_INTERES2 As String = "0"

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
        Try
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_INICIAL
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    INICIAL = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        INICIAL = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_IGV
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    IGV = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        IGV = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_FINANCE_FREE
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    FINANCE_FREE = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        FINANCE_FREE = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_PLAZOMESES1
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    PLAZOMESES1 = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        PLAZOMESES1 = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_PLAZOMESES2
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    PLAZOMESES2 = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        PLAZOMESES2 = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_TASA_INTERES1
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    TASA_INTERES1 = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        TASA_INTERES1 = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

            listabeHomologacion.Clear()
            obeHomologacion.Tabla = TablaHomologacion.COD_CLCVARIABLE_TASA_INTERES2
            obeHomologacion.ValorSap = eCotizacion.IdCompania.Trim + "@" + eCotizacion.IdSolicitante.Trim
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing Then
                If listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    TASA_INTERES2 = obeHomologacion.ValorCotizador
                Else
                    'default
                    listabeHomologacion.Clear()
                    obeHomologacion.ValorSap = eCotizacion.IdCompania
                    listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                    If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                        obeHomologacion = listabeHomologacion.First
                        TASA_INTERES2 = obeHomologacion.ValorCotizador
                    End If
                End If
            End If

        Catch ex As Exception
        End Try


        Try
            If dtbDatos.Rows.Count > 0 Then
                For Each drFila As DataRow In dtbDatos.Rows

                    If drFila.Item("IncluyeCLC") = True Or drFila.Item("IncluyeCLC") = "1" Then ' Solo si incluye CLC

                        Dim NombreArchivo As String = String.Empty
                        Dim objMemoryStream As New MemoryStream

                        'Obtener la plantilla
                        NombreArchivo = "CLC/CuadroCLCVariable.docx"
                        objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)

                        Dim CantRegTablaWord As Integer = 0
                        Dim cantColum As Integer = 0
                        Dim valTotal1 As Decimal = 0
                        Dim valTotal2 As Decimal = 0

                        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
                            Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                            CantRegTablaWord = tablaWord.Elements(Of TableRow).Count()
                            For i As Integer = 0 To CantRegTablaWord - 1
                                'Obtiene los valores para escribir
                                Dim valCelda1 As String = String.Empty
                                Dim valCelda2 As String = String.Empty
                                Dim valCelda3 As String = String.Empty

                                Dim valInicial1 As Decimal = 0
                                Dim valInicial2 As Decimal = 0
                                Dim valIgv1 As Decimal = 0
                                Dim valIgv2 As Decimal = 0
                                Dim valFinanceFee1 As Decimal = 0
                                Dim valFinanceFee2 As Decimal = 0

                                Dim valValorVenta As Decimal = 0

                                Try
                                    If Not drFila.Item("ValorVenta") Is DBNull.Value Then valValorVenta = drFila.Item("ValorVenta")
                                Catch ex As Exception
                                    valValorVenta = 0
                                End Try

                                Select Case i
                                    Case 0
                                        If Not drFila.Item("FechaHoy") Is DBNull.Value Then valCelda2 = drFila.Item("FechaHoy")
                                        If Not drFila.Item("FechaHoy") Is DBNull.Value Then valCelda3 = drFila.Item("FechaHoy")
                                        Exit Select
                                    Case 1
                                        If Not drFila.Item("Equipo") Is DBNull.Value Then valCelda2 = drFila.Item("Equipo")
                                        If Not drFila.Item("Equipo") Is DBNull.Value Then valCelda3 = drFila.Item("Equipo")
                                        Exit Select
                                    Case 2
                                        valCelda2 = valValorVenta
                                        valCelda3 = valValorVenta
                                        Exit Select
                                    Case 3
                                        valCelda2 = String.Empty
                                        valCelda3 = String.Empty
                                        Exit Select
                                    Case 4

                                        Try
                                            'valInicial1 = (0.15) * valValorVenta
                                            'valInicial2 = (0.15) * valValorVenta
                                            valInicial1 = Val(INICIAL) / 100 * valValorVenta
                                            valInicial2 = Val(INICIAL) / 100 * valValorVenta

                                            valTotal1 += valInicial1
                                            valTotal2 += valInicial2
                                        Catch ex As Exception
                                        End Try
                                        valCelda1 = "Inicial (" + INICIAL + "%)"
                                        valCelda2 = valInicial1
                                        valCelda3 = valInicial2
                                        Exit Select
                                    Case 5
                                        Try
                                            'valIgv1 = (0.18) * valValorVenta
                                            'valIgv2 = (0.18) * valValorVenta
                                            valIgv1 = Val(IGV) / 100 * valValorVenta
                                            valIgv2 = Val(IGV) / 100 * valValorVenta

                                            valTotal1 += valIgv1
                                            valTotal2 += valIgv2
                                        Catch ex As Exception
                                        End Try
                                        valCelda1 = "IGV (" + IGV + "%)"
                                        valCelda2 = valIgv1
                                        valCelda3 = valIgv2
                                        Exit Select
                                    Case 6
                                        Try
                                            'valFinanceFee1 = (0.02) * valValorVenta
                                            'valFinanceFee2 = (0.02) * valValorVenta
                                            valFinanceFee1 = Val(FINANCE_FREE) / 100 * valValorVenta
                                            valFinanceFee2 = Val(FINANCE_FREE) / 100 * valValorVenta
                                            valTotal1 += valFinanceFee1
                                            valTotal2 += valFinanceFee2
                                        Catch ex As Exception
                                        End Try
                                        valCelda1 = "Finance Fee (" + FINANCE_FREE + "%)"
                                        valCelda2 = valFinanceFee1
                                        valCelda3 = valFinanceFee2
                                        Exit Select
                                    Case 7
                                        Try
                                            valCelda2 = valTotal1
                                            valCelda3 = valTotal2
                                        Catch ex As Exception
                                        End Try
                                        Exit Select
                                    Case 8
                                        Try
                                            swPlazoMeses = True
                                            valCelda2 = PLAZOMESES1
                                            valCelda3 = PLAZOMESES2
                                        Catch ex As Exception
                                        End Try
                                        Exit Select
                                    Case 9
                                        Try
                                            valCelda2 = TASA_INTERES1 + "%"
                                            valCelda3 = TASA_INTERES2 + "%"
                                        Catch ex As Exception
                                        End Try
                                        Exit Select
                                    Case 10
                                        'Sumar
                                        Try
                                            Dim i1 As Decimal ' tasa de interes
                                            Dim i2 As Decimal
                                            Dim P1 As Decimal
                                            Dim P2 As Decimal
                                            Dim A1 As Decimal
                                            Dim A2 As Decimal
                                            'Dim n1 As Integer = 24 'numero de Periodos
                                            'Dim n2 As Integer = 36
                                            Dim n1 As Integer = Val(PLAZOMESES1) 'numero de Periodos
                                            Dim n2 As Integer = Val(PLAZOMESES2)

                                            'i1 = (10.5 / 100) / 12
                                            'i2 = (10.5 / 100) / 12
                                            i1 = (Val(TASA_INTERES1) / 100) / 12
                                            i2 = (Val(TASA_INTERES2) / 100) / 12

                                            P1 = valValorVenta - valInicial1
                                            P2 = valValorVenta - valInicial2

                                            A1 = P1 * ((i1 * (1 + i1) ^ n1) / (((1 + i1) ^ n1) - 1))
                                            A2 = P2 * ((i2 * (1 + i2) ^ n2) / (((1 + i2) ^ n2) - 1))

                                            valCelda2 = FormatNumber(A1, 2)
                                            valCelda3 = FormatNumber(A2, 2)

                                        Catch ex As Exception
                                        End Try
                                        Exit Select
                                    Case Else
                                        Exit Select
                                End Select
                                'Escribe en las celdas de la fila
                                Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                                cantColum = fila.Elements(Of TableCell).Count()
                                For j As Integer = 0 To cantColum - 1
                                    Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                                    Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                                    'Dim run As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                                    'RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)
                                    Dim run As Run
                                    Dim TextoCelda As String = String.Empty

                                    Select Case j
                                        Case 0
                                            'TextoCelda = indFilaTablaWord.ToString
                                            '14-11
                                            If valCelda1 <> "" Then
                                                run = para.AppendChild(RunConfigurado(True, , , lenghtLetraTablaCalculo20))
                                                'RunPropertiesConfigurado(True, , , lenghtLetraTablaCalculo20)
                                                TextoCelda = valCelda1
                                            Else
                                                run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                                            End If
                                            Exit Select
                                        Case 1
                                            run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                                            If swPlazoMeses Then
                                                'swPlazoMeses = False
                                            ElseIf IsNumeric(valCelda2) Then
                                                valCelda2 = FormatoMoneda(valCelda2)
                                            End If
                                            TextoCelda = valCelda2
                                            Exit Select
                                        Case 2
                                            run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                                            If swPlazoMeses Then
                                                swPlazoMeses = False
                                            ElseIf IsNumeric(valCelda3) Then
                                                valCelda3 = FormatoMoneda(valCelda3)
                                            End If
                                            TextoCelda = valCelda3
                                            Exit Select
                                        Case Else
                                            TextoCelda = String.Empty
                                    End Select

                                    run.AppendChild(New Text(TextoCelda))
                                Next
                            Next
                            wordDoc.MainDocumentPart.Document.Save()
                        End Using

                        'Agregar a la Lista de Byte
                        listaByteDocumento.Add(objMemoryStream.ToArray)
                    End If 'Fin si incluye CLC
                Next
            End If 'fin dtbDatos.Rows.Count > 0

        Catch ex As Exception
        End Try

    End Sub

    Private Sub ConstruirCondicionEspecifica(ByRef listaByteDocumento As List(Of Byte()), ByVal dtbDatos As DataTable, _
                                             ByVal dtbAdicional As DataTable, ByVal session As String)
        Dim oeHomologacion As New beHomologacion
        Dim listaHomologacion As New List(Of beHomologacion)
        Dim eHomologacion As New beHomologacion
        Dim l_Homologacion As New List(Of beHomologacion)
        'Dim obcHomologacion As New bcHomologacion

        '----------------------------------

        Dim listabeHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion
        Dim obcHomologacion As New ImpresionExtender
        Dim eCotizacion As New beCotizacion
        Dim esCompania As Boolean = False
        Dim esTipoProducto As Boolean = False
        Dim esAccesorio As Boolean = False
        Dim codFamilia As String = ""
        Dim valores() As String

        eCotizacion = CType(HttpContext.Current.Session(String.Concat(Nomb_Cotizacion, session)), beCotizacion)
        listabeHomologacion.Clear()

        Try
            obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_EMPRESA
            obeHomologacion.ValorSap = eCotizacion.IdCompania
            listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
            If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                obeHomologacion = listabeHomologacion.First
                If (obeHomologacion.ValorCotizador = "TRUE") Then
                    esCompania = True
                End If
            End If
        Catch ex As Exception
            esCompania = False
        End Try

        If eCotizacion.ListaProducto.Count > 0 Then

            listabeHomologacion.Clear()

            Try
                obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_TIPO_PRODUCTO
                obeHomologacion.ValorSap = eCotizacion.ListaProducto.Item(0).TipoProducto
                listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    If (obeHomologacion.ValorCotizador = "TRUE") Then
                        esTipoProducto = True
                    End If
                End If
            Catch ex As Exception
                esTipoProducto = False
            End Try


            listabeHomologacion.Clear()

            Try
                obeHomologacion.Tabla = TablaHomologacion.REG_COTIZACION_LISTA_COMPLETA_ACCESORIO
                obeHomologacion.ValorSap = eCotizacion.ListaProducto.Item(0).TipoProducto
                listabeHomologacion = obcHomologacion.ListaTablaHomologacionSC(obeHomologacion)
                If Not listabeHomologacion Is Nothing AndAlso listabeHomologacion.Count > 0 Then
                    obeHomologacion = listabeHomologacion.First
                    valores = Split(obeHomologacion.ValorCotizador, ",")
                    codFamilia = eCotizacion.ListaProducto.Item(0).CodigoFamilia.Substring(0, Val(valores(0)))
                    If (valores.Length > 1 AndAlso codFamilia = valores(1)) Then
                        esAccesorio = True
                    End If
                End If
            Catch ex As Exception
                esAccesorio = False
            End Try

        End If


        '---------------Fin Codigo Nuevo

        Try
            If dtbDatos.Rows.Count > 0 Then
                For indiFila As Integer = 0 To dtbDatos.Rows.Count - 1
                    Dim drFila As DataRow = dtbDatos.Rows(indiFila)
                    Dim sw As Boolean = False
                    Dim NombreArchivo As String = String.Empty
                    Dim objMemoryStream As New MemoryStream

                    'Obtener la plantilla
                    NombreArchivo = "CLC/CuadroCondicEspecificasCLC.docx"
                    objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)

                    Dim CantRegTablaWord As Integer = 0
                    Dim cantColum As Integer = 0

                    Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)

                        'Escribir Titulo si los Productos no Incluye CLC
                        If ProductosIncluyenClC = False Then ' 
                            ''Escribir: B.2.1. Condiciones Específicas de Equipo Nuevo
                            'Dim paraTituloAnexo As New Paragraph
                            'Dim runTituloAnexo As Run = paraTituloAnexo.AppendChild(RunConfigurado())
                            ''runTituloAnexo.AppendChild(New Text("B.2.1. Condiciones Específicas de Equipo Nuevo"))
                            'runTituloAnexo.AppendChild(New Text("Condiciones Específicas de Equipo Nuevo"))
                            'Dim rPrTituloAnexo As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , ))
                            'Dim alineacion As New Justification() With {.Val = JustificationValues.Left}
                            'rPrTituloAnexo.Append(alineacion)

                            'runTituloAnexo.PrependChild(Of RunProperties)(rPrTituloAnexo)
                            'runTituloAnexo.AppendChild(New Break())
                            'wordDoc.MainDocumentPart.Document.Body.InsertAt(paraTituloAnexo, 0) 'Insertar en la Posicion 0 del doc
                        End If

                        '1.- CARGAR DATOS A TABLA ESPECIFICACION DE EQUIPO
                        Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                        CantRegTablaWord = tablaWord.Elements(Of TableRow).Count()

                        For i As Integer = 0 To CantRegTablaWord - 1
                            'Obtiene los valores para escribir 
                            Dim valCelda2 As String = String.Empty

                            Select Case i
                                Case 0
                                    If Not drFila.Item("Equipo") Is DBNull.Value Then valCelda2 = drFila.Item("Equipo")
                                    Exit Select
                                Case 1
                                    If Not drFila.Item("Marca") Is DBNull.Value Then valCelda2 = drFila.Item("Marca")
                                    Exit Select
                                Case 2
                                    If Not drFila.Item("Modelo") Is DBNull.Value Then valCelda2 = drFila.Item("Modelo")
                                    Exit Select
                                Case 3
                                    If Not drFila.Item("Cantidad") Is DBNull.Value Then valCelda2 = drFila.Item("Cantidad")
                                    Exit Select
                                Case 4
                                    'If Not drFila.Item("PlazoEntrega") Is DBNull.Value Then valCelda2 = drFila.Item("PlazoEntrega")

                                    'Solo para Ferreyros y Prime
                                    '24/09


                                    'Homologacion para Plazo de entrega
                                    '28/08
                                    Try
                                        If Not (Not (esCompania AndAlso esTipoProducto) OrElse esAccesorio) Then
                                            eHomologacion.Tabla = TablaHomologacion.PLAZO_ENTREGA_ESTIMADO
                                            eHomologacion.ValorSap = "TRUE"
                                            l_Homologacion.Clear()
                                            l_Homologacion = objImpresionExtender.ListaTablaHomologacionSC(eHomologacion)
                                            eHomologacion = l_Homologacion.ToList().FirstOrDefault
                                            valCelda2 = eHomologacion.ValorCotizador
                                        Else
                                            If Not drFila.Item("PlazoEntrega") Is DBNull.Value Then valCelda2 = drFila.Item("PlazoEntrega")
                                        End If
                                    Catch ex As Exception
                                        If Not drFila.Item("PlazoEntrega") Is DBNull.Value Then valCelda2 = drFila.Item("PlazoEntrega")
                                    End Try
                                    Exit Select
                                Case 5
                                    If Not drFila.Item("DisponibilidadMaquina") Is DBNull.Value Then valCelda2 = drFila.Item("DisponibilidadMaquina")
                                    Exit Select
                                Case 6
                                    If Not drFila.Item("LugarEntrega") Is DBNull.Value Then valCelda2 = drFila.Item("LugarEntrega")
                                    Exit Select
                                Case 7 ' Garantia

                                    ' Buscar si la garantia viene como un Adicional 
                                    oeHomologacion.Tabla = TablaHomologacion.GARANTIA   ' tabla

                                    listaHomologacion = ListaTablaHomologacion(oeHomologacion)
                                    If listaHomologacion.Count > 0 Then
                                        If dtbAdicional.Rows.Count > 0 Then
                                            For Each drReg As DataRow In dtbAdicional.Rows
                                                Dim obeHomologacionBuscar As New beHomologacion
                                                obeHomologacionBuscar = BuscarDatoHomologacion(listaHomologacion, drReg("IdAdicional").ToString)
                                                If Not obeHomologacionBuscar Is Nothing Then
                                                    If Not drReg.Item("Nombre") Is DBNull.Value Then valCelda2 = drReg.Item("Nombre")
                                                    Exit For
                                                End If
                                            Next
                                        Else
                                            If Not drFila.Item("DescripGarantia") Is DBNull.Value Then valCelda2 = drFila.Item("DescripGarantia")
                                        End If
                                    Else
                                        If Not drFila.Item("DescripGarantia") Is DBNull.Value Then valCelda2 = drFila.Item("DescripGarantia")
                                    End If
                                    If valCelda2.Trim = "" Then
                                        sw = True
                                    End If
                                    Exit Select
                                Case 8
                                    If Not drFila.Item("FormaPago") Is DBNull.Value Then valCelda2 = drFila.Item("FormaPago")
                                    Exit Select
                                Case 9
                                    If Not drFila.Item("FechaFinalValidez") Is DBNull.Value Then valCelda2 = drFila.Item("FechaFinalValidez")
                                    Exit Select
                                Case 10
                                    If Not drFila.Item("AnioFabricacion") Is DBNull.Value Then valCelda2 = drFila.Item("AnioFabricacion")
                                    Exit Select
                                Case 11
                                    If Not drFila.Item("AnioModelo") Is DBNull.Value Then valCelda2 = drFila.Item("AnioModelo")
                                    Exit Select
                                Case 12
                                    If Not drFila.Item("Horometro") Is DBNull.Value Then valCelda2 = drFila.Item("Horometro")
                                    Exit Select
                                Case 13
                                    If Not drFila.Item("Serie") Is DBNull.Value Then valCelda2 = drFila.Item("Serie")
                                    Exit Select
                                Case 14
                                    If Not drFila.Item("Orden") Is DBNull.Value Then valCelda2 = drFila.Item("Orden")
                                    Exit Select
                                Case Else
                                    Exit Select
                            End Select

                            'Escribe en las celdas de la fila
                            Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                            cantColum = fila.Elements(Of TableCell).Count()
                            For j As Integer = 0 To cantColum - 1
                                Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                                Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                                Dim run As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))

                                Dim TextoCelda As String = String.Empty

                                Select Case j
                                    Case 0
                                        Exit Select
                                    Case 1
                                        TextoCelda = valCelda2
                                        Exit Select
                                    Case Else
                                        TextoCelda = String.Empty
                                End Select
                                run.AppendChild(New Text(TextoCelda))
                            Next
                        Next

                        'Elimina 5 ultimas filas -- solo se muestran para usados
                        If drFila.Item("CodClasificacion") <> "02" Then
                            Dim cant As Integer = tablaWord.Elements(Of TableRow).Count()
                            For k As Integer = 1 To 5
                                tablaWord.RemoveChild(tablaWord.Elements(Of TableRow)().ElementAt(cant - 5))
                            Next
                        End If

                        'Se elimina garantia cuando no hay informacion
                        If sw Then
                            tablaWord.RemoveChild(tablaWord.Elements(Of TableRow)().ElementAt(7))
                        End If

                        If Not (esCompania AndAlso esTipoProducto) OrElse esAccesorio Then
                            tablaWord.RemoveChild(tablaWord.Elements(Of TableRow)().ElementAt(5))
                        End If
                        'Si es Prime Usados agrego 2 fila
                        'If drFila.Item("CodClasificacion") = "02" Then
                        '    For i As Integer = 1 To 2
                        '        Dim nuevaFila As New TableRow()
                        '        Dim filaTabla As New TableRow()
                        '        Dim tccelda_Fila0 As New TableCell
                        '        filaTabla = tablaWord.Elements(Of TableRow)().ElementAt(CInt(CantRegTablaWord - 2 + i))
                        '        tccelda_Fila0 = filaTabla.Elements(Of TableCell)().ElementAt(CInt(0))
                        '        Dim nroCeldas As Integer = filaTabla.Elements(Of TableCell).Count()
                        '        For k As Integer = 1 To nroCeldas
                        '            Dim nuevaCelda As New TableCell(tccelda_Fila0.OuterXml)
                        '            nuevaFila.Append(nuevaCelda)
                        '        Next
                        '        tablaWord.Append(nuevaFila)

                        '        'LLena la fila isertada
                        '        Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(CantRegTablaWord - 1 + i)
                        '        Dim TextoCelda As String = String.Empty
                        '        For j As Integer = 1 To 2
                        '            Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j - 1)
                        '            Dim para As Paragraph = celda.Elements(Of Paragraph)().First
                        '            para.RemoveAllChildren()
                        '            Dim run As Run
                        '            Select Case i
                        '                Case 1
                        '                    Select Case j
                        '                        Case 1
                        '                            run = para.AppendChild(RunConfigurado(True, , , lenghtLetraTablaCalculo20))
                        '                            TextoCelda = "Año de Fabricación"
                        '                            Exit Select
                        '                        Case Else
                        '                            run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                        '                            If Not drFila.Item("AnioFabricacion") Is DBNull.Value Then TextoCelda = drFila.Item("AnioFabricacion")
                        '                            Exit Select
                        '                    End Select
                        '                Case 2
                        '                    Select Case j
                        '                        Case 1
                        '                            run = para.AppendChild(RunConfigurado(True, , , lenghtLetraTablaCalculo20))
                        '                            TextoCelda = "Año de Modelo"
                        '                            Exit Select
                        '                        Case Else
                        '                            run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))
                        '                            If Not drFila.Item("AnioModelo") Is DBNull.Value Then TextoCelda = drFila.Item("AnioModelo")
                        '                            Exit Select
                        '                    End Select
                        '                Case Else
                        '                    Exit Select
                        '            End Select
                        '            'run.RemoveAllChildren()
                        '            run.AppendChild(New Text(TextoCelda))
                        '        Next
                        '    Next
                        'End If

                        '2.- VERIFICAR TABLA CONTACTO
                        If indiFila < dtbDatos.Rows.Count - 1 Then
                            'Eliminar la tabla de contactos
                            Dim tablaWorContacto As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                            tablaWorContacto.Remove()
                        Else
                            'LLenar la tabla
                            Dim tablaWorContacto As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                            Dim CantRegTablaContacto = tablaWorContacto.Elements(Of TableRow).Count()

                            For i As Integer = 0 To CantRegTablaContacto - 1
                                'Obtiene los valores para escribir 
                                Dim valCelda2 As String = String.Empty

                                Select Case i
                                    Case 0
                                        If Not drFila.Item("Vendedor") Is DBNull.Value Then valCelda2 = drFila.Item("Vendedor")
                                        Exit Select
                                    Case 1
                                        Dim eTelefonoResponsable As New beTelefonoResponsable
                                        Dim listaTelefonoResponsable As New List(Of beTelefonoResponsable)
                                        Dim cTelefonoResponsable As New bcTelefonoResponsable
                                        Dim xListaTelefonoResponsable As New List(Of beTelefonoResponsable)
                                        Dim valorHomologacion As String = ""
                                        Dim arrayValor As String()

                                        eTelefonoResponsable.IdCotizacion = drFila.Item("IdCotizacion")
                                        sw = cTelefonoResponsable.TelefonoResponsableListar(Modulo.strConexionSql, eTelefonoResponsable, listaTelefonoResponsable)
                                        If listaTelefonoResponsable.Count > 0 Then
                                            For Each elemento As beTelefonoResponsable In listaTelefonoResponsable
                                                Dim xTelefonoResponsable As New beTelefonoResponsable
                                                eHomologacion.Tabla = TablaHomologacion.COD_TIPO_TELEFONO_RESPONSABLE  ' tabla
                                                eHomologacion.ValorSap = elemento.CodTipoTelefono
                                                l_Homologacion.Clear()
                                                l_Homologacion = objImpresionExtender.ListaTablaHomologacionSC(eHomologacion)

                                                xTelefonoResponsable.CodTipoTelefono = elemento.CodTipoTelefono
                                                xTelefonoResponsable.Anexo = elemento.Anexo
                                                xTelefonoResponsable.NroTelefono = elemento.NroTelefono

                                                If l_Homologacion.Count > 0 Then
                                                    eHomologacion = l_Homologacion.ToList().FirstOrDefault
                                                    valorHomologacion = eHomologacion.ValorCotizador
                                                    arrayValor = Split(valorHomologacion, "-")

                                                    xTelefonoResponsable.xOrden = arrayValor(0)
                                                    xTelefonoResponsable.xEtiqueta = arrayValor(1)
                                                Else
                                                    valorHomologacion = String.Empty
                                                End If
                                                xListaTelefonoResponsable.Add(xTelefonoResponsable)
                                            Next
                                            xListaTelefonoResponsable = xListaTelefonoResponsable.OrderBy(Function(x) x.xOrden).ToList()
                                        End If

                                        If sw Then
                                            Dim cadena As String = ""
                                            Dim iden As Boolean = True
                                            For Each telefonoResponsable As beTelefonoResponsable In xListaTelefonoResponsable
                                                If iden Then
                                                    iden = False
                                                Else
                                                    cadena = cadena + "                                                                             "
                                                End If
                                                cadena = cadena + telefonoResponsable.xEtiqueta + " "
                                                cadena = cadena + telefonoResponsable.NroTelefono
                                                If telefonoResponsable.Anexo.Trim <> "" Then
                                                    cadena = cadena + " / Anexo: " + telefonoResponsable.Anexo
                                                End If
                                            Next
                                            valCelda2 = cadena
                                        Else
                                            valCelda2 = ""
                                        End If
                                        'If Not drFila.Item("TelefonoVendedor") Is DBNull.Value Then valCelda2 = drFila.Item("TelefonoVendedor")
                                        Exit Select
                                    Case 2
                                        If Not drFila.Item("EmailVendedor") Is DBNull.Value Then valCelda2 = drFila.Item("EmailVendedor")
                                        Exit Select
                                    Case Else
                                        Exit Select
                                End Select
                                'Escribe en las celdas de la fila
                                Dim fila As TableRow = tablaWorContacto.Elements(Of TableRow)().ElementAt(i)
                                Dim cantColumContacto = fila.Elements(Of TableCell).Count()
                                For j As Integer = 0 To cantColumContacto - 1
                                    Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                                    Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                                    Dim run As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))

                                    Dim TextoCelda As String = String.Empty

                                    Select Case j
                                        Case 0
                                            Exit Select
                                        Case 1
                                            TextoCelda = valCelda2
                                            Exit Select
                                        Case Else
                                            TextoCelda = String.Empty
                                    End Select

                                    run.AppendChild(New Text(TextoCelda))
                                Next
                            Next
                        End If

                        wordDoc.MainDocumentPart.Document.Save()
                    End Using
                    'Agregar a la Lista de Byte
                    listaByteDocumento.Add(objMemoryStream.ToArray)
                Next
            End If 'fin dtbDatos.Rows.Count > 0 
        Catch ex As Exception
        End Try

    End Sub

    Private Sub ConstruirAnexos(ByRef objMemoryStream As MemoryStream, ByVal dtbDatos As DataTable)
        Dim NombreArchivo As String = String.Empty
        Dim objMemoryStreamTemp As New MemoryStream
        Dim ListaByteDocumentoTemp As New List(Of Byte())

        Dim cTarifa As bcTarifa = New bcTarifa
        Dim eTarifa As beTarifa = New beTarifa
        Dim l_Tarifa As New List(Of beTarifa)
        Dim eValidacion As New beValidacion

        Dim obeHomologacion As New beHomologacion
        Dim listaHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion


        str_mensajeRecorrido.AppendLine("   3.1- ConstruirAnexos - " & Now.ToString())

        '1.- **** Adjuntamos el primer Documento ************
        ListaByteDocumentoTemp.Add(objMemoryStream.ToArray)

        '2.- **** Cargar datos Anexo 1 *********************
        Dim IdUnidadDuracionProdCsa As String = String.Empty

        If dtbDatos.Rows.Count > 0 Then
            IdUnidadDuracionProdCsa = dtbDatos.Rows(0).Item("IdUnidadDuracion")
            If IdUnidadDuracionProdCsa = "H" Then
                NombreArchivo = "CSA/Plantilla Anexo1 Titulo.docx"
            Else
                NombreArchivo = "CSA/Plantilla Anexo1 TituloFecha.docx"
            End If
        End If



        objMemoryStreamTemp = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)

        If dtbDatos.Rows.Count > 0 Then

            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStreamTemp, True)
                '1.- LLENAR DATOS A LA TABLA DEL WORD
                Try
                    If dtbDatos.Rows.Count > 0 Then
                        Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                        Dim CantRegTablaWord As Integer = 0
                        Dim cantColum As Integer = 0
                        Dim indFilaTablaWord As Integer = 1

                        CantRegTablaWord = tablaWord.Elements(Of TableRow).Count()

                        If CantRegTablaWord > 0 Then
                            For Each drFila As DataRow In dtbDatos.Rows

                                If indFilaTablaWord < CantRegTablaWord Then 'Escribe en las celdas de la fila
                                    Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(indFilaTablaWord)
                                    cantColum = fila.Elements(Of TableCell).Count()
                                    For j As Integer = 0 To cantColum - 1
                                        Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                                        Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                                        Dim run As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))

                                        Dim TextoCelda As String = String.Empty

                                        Select Case j

                                            Case 0
                                                If Not drFila.Item("Familia") Is DBNull.Value Then TextoCelda = drFila.Item("Familia")
                                                Exit Select
                                            Case 1
                                                If Not drFila.Item("ModeloBase") Is DBNull.Value Then TextoCelda = drFila.Item("ModeloBase")
                                                Exit Select
                                            Case 2
                                                If Not drFila.Item("NumeroSerie") Is DBNull.Value Then TextoCelda = drFila.Item("NumeroSerie")
                                                Exit Select
                                            Case 3
                                                If Not drFila.Item("Departamento") Is DBNull.Value Then TextoCelda = drFila.Item("Departamento")
                                                Exit Select
                                            Case 4
                                                If IdUnidadDuracionProdCsa = "H" Then
                                                    If Not drFila.Item("HorometroInicial") Is DBNull.Value Then TextoCelda = drFila.Item("HorometroInicial")
                                                Else
                                                    If Not drFila.Item("FechaHorometroIncial") Is DBNull.Value Then TextoCelda = drFila.Item("FechaHorometroIncial")
                                                End If

                                                Exit Select
                                            Case 5
                                                If IdUnidadDuracionProdCsa = "H" Then
                                                    If Not drFila.Item("HorometroFin") Is DBNull.Value Then TextoCelda = drFila.Item("HorometroFin")
                                                Else
                                                    If Not drFila.Item("FechaHorometroFin") Is DBNull.Value Then TextoCelda = drFila.Item("FechaHorometroFin")
                                                End If

                                                Exit Select
                                            Case Else
                                                TextoCelda = String.Empty
                                        End Select

                                        run.AppendChild(New Text(TextoCelda))

                                    Next
                                Else ' Cuando ya no hay mas filas donde escribir se crean nuevas filas                               

                                    Dim trFila As New TableRow()

                                    Dim valorCelda As String = String.Empty

                                    If Not drFila.Item("Familia") Is DBNull.Value Then valorCelda = drFila.Item("Familia")
                                    Dim tcFamilia As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    valorCelda = String.Empty
                                    If Not drFila.Item("ModeloBase") Is DBNull.Value Then valorCelda = drFila.Item("ModeloBase")
                                    Dim tcModelo As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    valorCelda = String.Empty
                                    If Not drFila.Item("NumeroSerie") Is DBNull.Value Then valorCelda = drFila.Item("NumeroSerie")
                                    Dim tcNumeroSerie As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    valorCelda = String.Empty
                                    If Not drFila.Item("Departamento") Is DBNull.Value Then valorCelda = drFila.Item("Departamento")
                                    Dim tcUbicacion As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    valorCelda = String.Empty

                                    If IdUnidadDuracionProdCsa = "H" Then
                                        If Not drFila.Item("HorometroInicial") Is DBNull.Value Then valorCelda = drFila.Item("HorometroInicial")
                                    Else
                                        If Not drFila.Item("FechaHorometroIncial") Is DBNull.Value Then valorCelda = drFila.Item("FechaHorometroIncial")
                                    End If
                                    'If Not drFila.Item("HorometroInicial") Is DBNull.Value Then valorCelda = drFila.Item("HorometroInicial")
                                    Dim tcHoraInicio As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    valorCelda = String.Empty
                                    If IdUnidadDuracionProdCsa = "H" Then
                                        If Not drFila.Item("HorometroFin") Is DBNull.Value Then valorCelda = drFila.Item("HorometroFin")
                                    Else
                                        If Not drFila.Item("FechaHorometroFin") Is DBNull.Value Then valorCelda = drFila.Item("FechaHorometroFin")
                                    End If

                                    'If Not drFila.Item("HorometroFin") Is DBNull.Value Then valorCelda = drFila.Item("HorometroFin")
                                    Dim tcHoraFin As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado(, , , lenghtLetraTablaCalculo20)), New Text(valorCelda))))

                                    trFila.Append(tcFamilia, tcModelo, tcNumeroSerie, tcUbicacion, tcHoraInicio, tcHoraFin)
                                    tablaWord.AppendChild(trFila)
                                End If
                                'Incrementa el indice de recorido
                                indFilaTablaWord += 1
                            Next
                        End If
                    End If
                    wordDoc.MainDocumentPart.Document.Save()
                Catch ex As Exception
                End Try
            End Using


            ListaByteDocumentoTemp.Add(objMemoryStreamTemp.ToArray)
        End If


        ' 3.- **** GENERACION DE ANEXO 2 *********************************************
        Dim ExisteTituloAnexo2 As Boolean = False

        For Each fila As DataRow In dtbDatos.Rows

            str_mensajeRecorrido.AppendLine("   3.2- Generacion de Anexo 2 - " & Now.ToString())

            Dim TipoCotizacion As String = String.Empty
            If Not IsDBNull(fila.Item("TipoCotizacion")) Then TipoCotizacion = fila.Item("TipoCotizacion")

            Dim EsNueva As Boolean = fila.Item("MaquinaNueva")
            Dim HoraUso As Decimal = fila.Item("HorasPromedioUso")
            Dim Duracion As Decimal = fila.Item("Duracion")
            Dim IdUnidadDuracion As String = fila.Item("IdUnidadDuracion")
            Dim llaveA As String = String.Empty
            Dim llaveB As String = String.Empty
            Dim llaveC As String = String.Empty
            Dim ExisteLlaveA As Boolean = False
            Dim ExisteLlaveB As Boolean = False
            Dim ExisteLlaveC As Boolean = False

            Dim llaveBusqueda As String = String.Empty

            Dim NombrePlantilla As String = String.Empty
            Dim TipoPLantilla As String = String.Empty
            Dim EventoWord As String = String.Empty
            Dim CollectEventosWord As List(Of String)
            Dim CollectEventosTarifaBD As List(Of String)
            Dim TotalPM As Decimal = 0
            Dim flatExiteTarifa As Boolean = False
            Dim SumaTotal As Decimal = 0
            Dim PosicionFilaTotal As Integer = 0
            Dim CantFilaRecorrer As Integer = 0

            'Dim rPrTexto As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , "DERECHA"))

            Select Case TipoCotizacion

                Case TipoCotizacionCSA.PRIME
                    NombrePlantilla = "Plantilla Tarifas CSA Prime.docx"
                    TipoPLantilla = TipoCotizacionCSA.PRIME
                    Exit Select
                Case TipoCotizacionCSA.StandBy
                    NombrePlantilla = "Plantilla Tarifas CSA Stand By.docx"
                    TipoPLantilla = TipoCotizacionCSA.StandBy
                    Exit Select
                Case TipoCotizacionCSA.MONITOREO
                    NombrePlantilla = "Plantilla Tarifas CSA Monitoreo.docx"
                    TipoPLantilla = TipoCotizacionCSA.MONITOREO
                    Exit Select
                Case Else ' TipoCotizacionCSA.Heaving y Otro

                    'Buscar las Llaves
                    llaveA = fila.Item("ModeloBase") & "@" & fila.Item("Prefijo") & "@" & fila.Item("IdPlan") & "@" & "PM 1F"
                    llaveB = fila.Item("ModeloBase") & "@" & fila.Item("Prefijo") & "@" & fila.Item("IdPlan") & "@" & "PM 2F"
                    'Con doble espacio PM  125
                    llaveC = fila.Item("ModeloBase") & "@" & fila.Item("Prefijo") & "@" & fila.Item("IdPlan") & "@" & "PM  125"

                    '----Buscar en la tabla tarifa --------------------------------------------------
                    eValidacion.flag = 1
                    l_Tarifa.Clear()
                    eTarifa.llave = llaveA
                    cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)
                    If l_Tarifa.ToList.Count > 0 Then
                        ExisteLlaveA = True
                    End If

                    l_Tarifa.Clear()
                    eTarifa.llave = llaveB
                    cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)
                    If l_Tarifa.ToList.Count > 0 Then
                        ExisteLlaveB = True
                    End If

                    l_Tarifa.Clear()
                    eTarifa.llave = llaveC
                    cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)
                    If l_Tarifa.ToList.Count > 0 Then
                        ExisteLlaveC = True
                    End If
                    '-------------------------------------------------------------------------------------

                    '*** Generar Tipo Plantilla ********************************
                    TipoPLantilla = String.Empty

                    'LLAVE A
                    If ExisteLlaveA = True Then
                        TipoPLantilla = String.Concat(TipoPLantilla, "1")
                    End If

                    'LLAVE B
                    If ExisteLlaveB = True Then
                        TipoPLantilla = String.Concat(TipoPLantilla, "2")
                    End If

                    'Agregar F
                    If TipoPLantilla.Trim <> String.Empty Then
                        TipoPLantilla = String.Concat(TipoPLantilla, "F")
                    End If

                    'LLAVE C
                    If ExisteLlaveC = True Then
                        TipoPLantilla = String.Concat(TipoPLantilla, "125")
                    End If

                    'Si no se generó ningun tipo de plantilla
                    If TipoPLantilla.Trim = String.Empty Then
                        TipoPLantilla = "CSA"
                    End If

                    '*******************************************

                    If EsNueva Then

                        Select Case TipoPLantilla
                            Case "12F"
                                NombrePlantilla = "Plantilla Tarifas CSA MaqNuevo 12F.docx"
                                Exit Select
                            Case "1F"
                                NombrePlantilla = "Plantilla Tarifas CSA MaqNuevo 1F.docx"
                                Exit Select
                            Case "2F"
                                NombrePlantilla = "Plantilla Tarifas CSA MaqNuevo 2F.docx"
                                Exit Select
                            Case "1F125"
                                NombrePlantilla = "Plantilla Tarifas CSA Maquina 1F125.docx"
                                Exit Select
                            Case "2F125"
                                NombrePlantilla = "Plantilla Tarifas CSA Maquina 2F125.docx"
                                Exit Select
                            Case "12F125"
                                NombrePlantilla = "Plantilla Tarifas CSA Maquina 12F125.docx"
                                Exit Select
                            Case "125"
                                NombrePlantilla = "Plantilla Tarifas CSA Maquina 125.docx"
                                Exit Select
                            Case "CSA"
                                NombrePlantilla = "Plantilla Tarifas CSA.docx"
                                Exit Select
                            Case Else
                                NombrePlantilla = "Plantilla Tarifas CSA.docx"
                                Exit Select
                        End Select

                        '--------------------------------------------------------------------------------
                    Else
                        'NombrePlantilla = "Plantilla Tarifas CSA.docx"
                        'TipoPLantilla = "CSA"

                        'TipoPLantilla se le asigna un valor deacuerdo al nombre de planilla
                        Select Case TipoPLantilla
                            Case "125", "1F125", "2F125", "12F125"
                                NombrePlantilla = "Plantilla Tarifas CSA Maquina 125.docx"
                                TipoPLantilla = "125"
                                Exit Select
                            Case "CSA"
                                NombrePlantilla = "Plantilla Tarifas CSA.docx"
                                TipoPLantilla = "CSA"
                                Exit Select
                            Case Else
                                NombrePlantilla = "Plantilla Tarifas CSA.docx"
                                TipoPLantilla = "CSA"
                                Exit Select
                        End Select
                    End If

            End Select

            ' Trabajar con el MemoryStream de la plantilla seleccionada
            '======================================================
            objMemoryStreamTemp = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, String.Concat("CSA/", NombrePlantilla))


            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStreamTemp, True)

                '----- Escribir Titulo de Tarifa ---------------
                'Dim para3 As New Paragraph
                'Dim run3 As Run = para3.AppendChild(RunConfigurado(True, True, , , , "CENTRO"))
                Dim TituloMaquina As String = String.Empty

                Select Case TipoCotizacion
                    Case TipoCotizacionCSA.PRIME, TipoCotizacionCSA.StandBy

                        If IsNothing(fila.Item("Modelo")) Then
                            TituloMaquina = fila.Item("ModeloBase")
                        Else
                            If String.IsNullOrEmpty(fila.Item("Modelo")) Then
                                TituloMaquina = fila.Item("ModeloBase")
                            Else
                                TituloMaquina = fila.Item("Modelo")
                            End If
                        End If
                        TituloMaquina = String.Concat("TARIFAS ", TituloMaquina)

                    Case Else
                        TituloMaquina = String.Concat("TARIFAS ", " " & fila.Item("Modelo") & " " & fila.Item("NumeroSerie"))
                End Select


                'Dim runBreakTituTarifa As New Run()
                'runBreakTituTarifa.AppendChild(New Break())
                Dim rPrTituloTarifa As RunProperties = New RunProperties(RunPropertiesConfigurado(True, True, , , , ))
                Dim paraTituloTarifa As New Paragraph()

                Dim paragraphPropertiesTituTarifa As New ParagraphProperties()
                Dim justificationTituTarifa As New Justification()
                justificationTituTarifa.Val = JustificationValues.Center
                paragraphPropertiesTituTarifa.Append(justificationTituTarifa)

                Dim runTituloTarifa As New Run(New Text(TituloMaquina))
                runTituloTarifa.PrependChild(Of RunProperties)(rPrTituloTarifa)
                paraTituloTarifa.Append(paragraphPropertiesTituTarifa)
                'paraTituloTarifa.Append(runBreakTituTarifa)
                paraTituloTarifa.Append(runTituloTarifa)
                runTituloTarifa.AppendChild(New Break())
                wordDoc.MainDocumentPart.Document.Body.InsertAt(paraTituloTarifa, 0) 'Insertar en la Posicion 0 del doc 

                'Escribir en la tabla del word
                Select Case TipoCotizacion

                    Case TipoCotizacionCSA.PRIME
                        Dim totalServicios As Decimal = 0
                        Dim tarifaUSDH As Decimal = 0
                        Dim total As Decimal = 0
                        Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)

                        'Reiniciando Variables
                        PosicionFilaTotal = 6
                        CantFilaRecorrer = 4

                        Dim duracionPrime As Decimal = 0 'HoraUso.ToString

                        obeHomologacion.Tabla = TablaHomologacion.DURACION_PLAN_GENERADOR_PRIME  ' tabla
                        listaHomologacion.Clear()
                        listaHomologacion = ListaTablaHomologacion(obeHomologacion)
                        obeHomologacion = listaHomologacion.ToList().FirstOrDefault
                        If Not obeHomologacion Is Nothing Then
                            duracionPrime = obeHomologacion.ValorSap
                        End If

                        'mmmmmmm()
                        '--- Escribir la cantidad de horas ----------------------------------------
                        Dim trFilaTitu As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(1)
                        Dim tcCeldaTitu As TableCell = trFilaTitu.Elements(Of TableCell)().ElementAt(1)
                        Dim runTitulo As Run = tcCeldaTitu.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        Dim NombConstruido As String = " " & duracionPrime.ToString & " Horas"
                        runTitulo.AppendChild(New Text(NombConstruido))
                        runTitulo.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                        '---------------------------------------------------------------------------

                        For i As Integer = 1 To CantFilaRecorrer
                            Dim valIncluyeFluido As String = String.Empty
                            If fila("IncluyeFluidos") Then
                                'valIncluyeFluido = "1"
                                valIncluyeFluido = "Con Fluidos"
                            Else
                                'valIncluyeFluido = "0"
                                valIncluyeFluido = "Sin Fluidos"
                            End If

                            llaveBusqueda = fila("Modelo") & "@" & fila("IdPlan") & "@" & valIncluyeFluido & "@" & "PM " + i.ToString()
                            EventoWord = String.Concat("PM", i.ToString)

                            l_Tarifa.Clear()
                            eValidacion.flag = 2
                            eTarifa.llave = llaveBusqueda
                            cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)
                            If l_Tarifa.ToList.Count > 0 Then
                                flatExiteTarifa = True
                                Dim cantColum As Integer = 0
                                Dim trFila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i + 1)
                                Dim tcCelda1 As TableCell = trFila.Elements(Of TableCell)().ElementAt(0)
                                Dim run1 As Run = tcCelda1.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                '--Calcular Tarifas-------------
                                tarifaUSDH = l_Tarifa.First().tarifaUSDxH
                                Dim totalServicioReg As Decimal
                                Dim servicioContratado As Decimal = l_Tarifa.First().servicioContratado
                                Dim Sos As Decimal = l_Tarifa.First().SOS
                                Dim EventoNueva As Decimal = l_Tarifa.First().eventosNueva

                                totalServicioReg = (servicioContratado + Sos) * EventoNueva
                                totalServicios = totalServicios + totalServicioReg
                                'total = total + (l_Tarifa.First().total * l_Tarifa.First().tarifaUSDxH)
                                'SumaTotal = total
                                '------------------------------
                                'Compara el Evento 
                                If tcCelda1.InnerText = EventoWord Then
                                    For j As Integer = 0 To 1
                                        Dim tcCelda As TableCell = trFila.Elements(Of TableCell)().ElementAt(j)
                                        Dim para As Paragraph = tcCelda.Elements(Of Paragraph)().First()
                                        Dim runCelda As Run = para.AppendChild(RunConfigurado(, , , lenghtLetraTablaCalculo20))

                                        Select Case j
                                            Case 1
                                                runCelda.AppendChild(New Text(l_Tarifa.First().eventosNueva))
                                                Exit Select
                                        End Select
                                    Next
                                End If
                            End If
                        Next


                        'total = (Duracion * tarifaUSDH)
                        total = (duracionPrime * tarifaUSDH)

                        SumaTotal = total

                        '--- Escribir valores de Tarifas -------------------------------------------

                        Dim trFilaTarifa As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(2)
                        Dim tcCelda2 As TableCell = trFilaTarifa.Elements(Of TableCell)().ElementAt(2)
                        Dim run_2 As Run = tcCelda2.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If run_2 Is Nothing Then
                            Dim para As Paragraph = tcCelda2.Elements(Of Paragraph)().First()
                            run_2 = para.AppendChild(New Run)
                        End If

                        run_2.AppendChild(New Text(FormatoMoneda(total - totalServicios)))
                        run_2.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                        Dim tcCelda3 As TableCell = trFilaTarifa.Elements(Of TableCell)().ElementAt(3)
                        Dim run_3 As Run = tcCelda3.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If run_3 Is Nothing Then
                            Dim para As Paragraph = tcCelda3.Elements(Of Paragraph)().First()
                            run_3 = para.AppendChild(New Run)
                        End If
                        run_3.AppendChild(New Text(FormatoMoneda(totalServicios)))
                        run_3.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                        Dim tcCelda4 As TableCell = trFilaTarifa.Elements(Of TableCell)().ElementAt(4)
                        Dim run_4 As Run = tcCelda4.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If run_4 Is Nothing Then
                            Dim para As Paragraph = tcCelda4.Elements(Of Paragraph)().First()
                            run_4 = para.AppendChild(New Run)
                        End If
                        run_4.AppendChild(New Text(FormatoMoneda(total)))
                        run_4.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                        '--- Escribir total en Pie del Cuadro -------------------------------------
                        Dim trFilaTotal As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal)
                        Dim tcCeldaTotal As TableCell = trFilaTotal.Elements(Of TableCell)().ElementAt(2) 'celda para escribir el valor total
                        Dim runTotal As Run = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If runTotal Is Nothing Then
                            Dim para As Paragraph = tcCeldaTotal.Elements(Of Paragraph)().First()
                            runTotal = para.AppendChild(New Run)
                        End If
                        Dim TextoConstruido As String = String.Empty
                        'If HoraUso = 0 Then
                        '    TextoConstruido = FormatoMoneda(Decimal.Round((SumaTotal / 1), 2))
                        'Else
                        '    TextoConstruido = FormatoMoneda(Decimal.Round((SumaTotal / HoraUso), 2))
                        'End If
                        TextoConstruido = FormatoMoneda(tarifaUSDH)

                        runTotal.AppendChild(New Text(TextoConstruido))
                        runTotal.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                        '----------------------------------------------------------------------------
                        '--alinear a la derecha----------------------------
                        Dim paragrCeldaTotal As Paragraph = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault()
                        paragrCeldaTotal.RemoveAllChildren()
                        paragrCeldaTotal.Append(paragraphPropertiesCentro.Clone)
                        paragrCeldaTotal.Append(runTotal)
                        '---------------------------------------------------
                        Exit Select

                    Case TipoCotizacionCSA.StandBy

                        Dim eventos(3) As String
                        eventos(0) = "INSPECCIÓN"
                        eventos(1) = "PRUEBA"
                        eventos(2) = "MANTENIMIENTO"

                        'Reiniciando Variables
                        PosicionFilaTotal = 5
                        CantFilaRecorrer = 3

                        '---- Escribir Titulo Tarifas: Es el segundo paragraph del documento ---------------------------
                        Dim prgTitulo As Paragraph = wordDoc.MainDocumentPart.Document.Body.Elements(Of Paragraph)()(1)
                        Dim runTitulo As Run = prgTitulo.Elements(Of Run)().FirstOrDefault
                        runTitulo.Append(New Text(fila("Modelo")))
                        '------------------------------------------------------------------------------------------------

                        Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)

                        For i As Integer = 1 To CantFilaRecorrer
                            llaveBusqueda = fila("Modelo") & "@" & fila("IdPlan") & "@" & eventos(i - 1)

                            '----- Escribir la cantidad de Eventos ----------------------------------------------------------
                            If i = 1 Then
                                Dim trFilaTituloCantEvento As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                                Dim tcCeldaTituloCantEvento As TableCell = trFilaTituloCantEvento.Elements(Of TableCell)().ElementAt(2)
                                Dim runTituloCantEvento As Run = tcCeldaTituloCantEvento.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                runTituloCantEvento.AppendChild(New Text(" " & Duracion.ToString & " " & Homologacion.ObtenerUnidadDuracion(IdUnidadDuracion)))
                            End If
                            '------------------------------------------------------------------------------------------------

                            l_Tarifa.Clear()
                            eValidacion.flag = 3
                            eTarifa.llave = llaveBusqueda
                            cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)

                            If l_Tarifa.ToList.Count > 0 Then
                                flatExiteTarifa = True

                                Dim cantColum As Integer = 0
                                Dim trFila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i + 1)
                                For j As Integer = 1 To 3
                                    Dim tcCeldaDatos As TableCell = trFila.Elements(Of TableCell)().ElementAt(j)
                                    Dim runDatos As Run = tcCeldaDatos.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                    If IsNothing(runDatos) Then
                                        Dim prgDatos As Paragraph = tcCeldaDatos.Elements(Of Paragraph)().First()
                                        runDatos = prgDatos.AppendChild(New Run)
                                    End If
                                    Select Case j
                                        Case 1
                                            runDatos.AppendChild(New Text(FormatoMoneda(l_Tarifa.First().servicioContratado)))
                                            Exit Select
                                        Case 2
                                            runDatos.AppendChild(New Text(l_Tarifa.First().eventosNueva))
                                            Exit Select
                                        Case 3
                                            SumaTotal += (l_Tarifa.First().servicioContratado * l_Tarifa.First().eventosNueva)

                                            runDatos.AppendChild(New Text(FormatoMoneda(l_Tarifa.First().servicioContratado * l_Tarifa.First().eventosNueva)))
                                            Exit Select
                                    End Select
                                    runDatos.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                Next
                            End If
                        Next
                        '--- Escribir total en Pie del Cuadro ---------------------------------------------------
                        '--Anual -------------------------
                        Dim trFilaTotalAnual As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal)
                        Dim tcCeldaTotalAnual As TableCell = trFilaTotalAnual.Elements(Of TableCell)().ElementAt(2) 'celda para escribir el valor total
                        Dim runTotalAnual As Run = tcCeldaTotalAnual.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If runTotalAnual Is Nothing Then
                            Dim prgTotalAnual As Paragraph = tcCeldaTotalAnual.Elements(Of Paragraph)().First()
                            runTotalAnual = prgTotalAnual.AppendChild(New Run)
                        End If
                        Dim TextoConstruidoAnual As String = FormatoMoneda(Decimal.Round(SumaTotal, 2))
                        runTotalAnual.AppendChild(New Text(TextoConstruidoAnual))
                        runTotalAnual.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                        '--alinear a la derecha----------------------------
                        Dim paragrTotalAnual As Paragraph = tcCeldaTotalAnual.Elements(Of Paragraph).FirstOrDefault()
                        paragrTotalAnual.RemoveAllChildren()
                        paragrTotalAnual.Append(paragraphPropertiesCentro.Clone)
                        paragrTotalAnual.Append(runTotalAnual)
                        '---------------------------------------------------
                        '-----------------------------------------------------------------------

                        '---- Mensual----------------------------------------
                        Dim trFilaTotalMensual As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal + 1)
                        Dim tcCeldaTotalMensual As TableCell = trFilaTotalMensual.Elements(Of TableCell)().ElementAt(2) 'celda para escribir el valor total
                        Dim runTotalMensual As Run = tcCeldaTotalMensual.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                        If runTotalMensual Is Nothing Then
                            Dim prgTotalMensual As Paragraph = tcCeldaTotalMensual.Elements(Of Paragraph)().First()
                            runTotalMensual = prgTotalMensual.AppendChild(New Run)
                        End If
                        Dim TextoConstruidoMensual As String = String.Empty
                        If Duracion = 0 Then
                            TextoConstruidoMensual = FormatoMoneda(Decimal.Round((SumaTotal / 1), 2))
                        Else
                            TextoConstruidoMensual = FormatoMoneda(Decimal.Round((SumaTotal / Duracion), 2))
                        End If

                        runTotalMensual.AppendChild(New Text(TextoConstruidoMensual))
                        runTotalMensual.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                        '--alinear a la derecha----------------------------
                        Dim paragrTotalMensual As Paragraph = tcCeldaTotalMensual.Elements(Of Paragraph).FirstOrDefault()
                        paragrTotalMensual.RemoveAllChildren()
                        paragrTotalMensual.Append(paragraphPropertiesCentro.Clone)
                        paragrTotalMensual.Append(runTotalMensual)
                        '---------------------------------------------------
                        '--------------------------------------------------------------------------------------------------
                        Exit Select

                    Case TipoCotizacionCSA.MONITOREO
                        'Reiniciar variables
                        SumaTotal = 0
                        PosicionFilaTotal = 5
                        CantFilaRecorrer = 4

                        ' asignando Eventos
                        CollectEventosWord = New List(Of String)
                        CollectEventosWord.Add("Inspección 1  (AT1 Plus)")
                        CollectEventosWord.Add("Inspección 2  (AT1 Plus)")
                        CollectEventosWord.Add("Monitoreo de Condiciones")

                        CollectEventosTarifaBD = New List(Of String)
                        CollectEventosTarifaBD.Add("Inspección 1")
                        CollectEventosTarifaBD.Add("Inspección 2")
                        CollectEventosTarifaBD.Add("Monitoreo")

                        ''Linea para q todos los planes de monitoreos se calculen sin fluidos
                        'ProductoCSA.IncluyeFluidos = False

                        'Recorrer segun cantida de fila
                        For i As Integer = 2 To CantFilaRecorrer


                            llaveBusqueda = fila("ModeloBase") & "@" & fila("Prefijo") & "@" & fila("IdPlan") & "@" & CollectEventosTarifaBD(i - 2)
                            EventoWord = CollectEventosWord(i - 2) 'String.Concat("PM", i.ToString)

                            l_Tarifa.Clear()
                            eValidacion.flag = 1
                            eTarifa.llave = llaveBusqueda
                            cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)

                            '------------------------------------------------
                            'verificar si producto incluye fluidos exita tarifas con fluidos
                            If CBool(fila("IncluyeFluidos") = True) Then
                                l_Tarifa = l_Tarifa.Where(Function(c) c.conFluidos.ToString().ToUpper() = "CON FLUIDOS").ToList()
                            Else
                                l_Tarifa = l_Tarifa.Where(Function(c) c.conFluidos.ToString().ToUpper() = "SIN FLUIDOS").ToList()
                            End If
                            '------------------------------------------------


                            If l_Tarifa.Count > 0 Then
                                flatExiteTarifa = True
                                Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                                Dim cantColum As Integer = 0

                                Dim trFila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                                Dim tcCelda1 As TableCell = trFila.Elements(Of TableCell)().ElementAt(0)
                                Dim run1 As Run = tcCelda1.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                'Compara el Evento
                                If tcCelda1.InnerText = EventoWord Then
                                    'Recorrer las columnas de una fila
                                    For j As Integer = 0 To 3
                                        Dim tcCelda As TableCell = trFila.Elements(Of TableCell)().ElementAt(j)
                                        Dim para As Paragraph = tcCelda.Elements(Of Paragraph)().First()
                                        Dim runCelda As Run = para.AppendChild(New Run)
                                        Dim ValorCelda As String = String.Empty
                                        Select Case j
                                            Case 1

                                                'If (IsNumeric(l_Tarifa.First().eventosNueva)) Then
                                                '    ValorCelda = FormatoMoneda(l_Tarifa.First().eventosNueva)
                                                'Else
                                                '    ValorCelda = l_Tarifa.First().eventosNueva
                                                'End If 
                                                ValorCelda = l_Tarifa.First().servicioContratado
                                                runCelda.AppendChild(New Text(ValorCelda))
                                                Exit Select
                                            Case 2
                                                ValorCelda = l_Tarifa.First().eventosNueva
                                                runCelda.AppendChild(New Text(ValorCelda))
                                                Exit Select
                                            Case 3
                                                ValorCelda = l_Tarifa.First().servicioContratadoT
                                                SumaTotal = SumaTotal + l_Tarifa.First().servicioContratadoT
                                                runCelda.AppendChild(New Text(ValorCelda))
                                                Exit Select
                                        End Select
                                        If j <> 0 Then
                                            runCelda.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                        End If
                                    Next
                                End If

                                '--- Escribir en total ---------------------------------------------------------------------------------
                                'Dim rPrTotal As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , "DERECHA"))
                                Dim trFilaTotal As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal)
                                Dim tcCeldaTotal As TableCell = trFilaTotal.Elements(Of TableCell)().ElementAt(2) 'celda para escribir el valor total
                                Dim runTotal As Run = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                Dim total As String = String.Empty

                                If IsNumeric(SumaTotal) Then
                                    total = FormatoMoneda(SumaTotal)
                                Else
                                    total = SumaTotal
                                End If
                                If IsNothing(runTotal) Then
                                    runTotal = New Run
                                    runTotal.AppendChild(New Text(total))
                                    tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault().InsertAt(runTotal, 0)
                                Else
                                    runTotal.RemoveAllChildren()
                                    runTotal.AppendChild(New Text(total))
                                End If
                                runTotal.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                '--alinear a la derecha----------------------------
                                Dim paragrTotal As Paragraph = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault()
                                paragrTotal.RemoveAllChildren()
                                paragrTotal.Append(paragraphPropertiesCentro.Clone)
                                paragrTotal.Append(runTotal)
                                '---------------------------------------------------


                                'Dim rPrTotalxHora As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , "DERECHA"))
                                Dim trFilaTotalxMes As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal + 1)
                                Dim tcCeldaTotalxMes As TableCell = trFilaTotalxMes.Elements(Of TableCell)().ElementAt(2)
                                Dim runTotalxMes As Run = tcCeldaTotalxMes.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                Dim totalPorMes As String = String.Empty
                                Try
                                    'totalPorHoras = SumaTotal / fila("HorometroFin")
                                    totalPorMes = SumaTotal / 12
                                    totalPorMes = CStr(Decimal.Round(CDec(totalPorMes), 4))
                                Catch ex As Exception
                                    totalPorMes = String.Empty
                                End Try

                                If (IsNumeric(totalPorMes)) Then
                                    totalPorMes = FormatoMoneda(totalPorMes)
                                End If

                                If IsNothing(runTotalxMes) Then
                                    runTotalxMes = New Run
                                    runTotalxMes.AppendChild(New Text(totalPorMes))
                                    tcCeldaTotalxMes.Elements(Of Paragraph).FirstOrDefault().InsertAt(runTotalxMes, 0)
                                Else
                                    runTotalxMes.RemoveAllChildren()
                                    runTotalxMes.AppendChild(New Text(totalPorMes))
                                End If
                                runTotalxMes.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                '--alinear a la derecha----------------------------
                                Dim paragrTotalxMes As Paragraph = tcCeldaTotalxMes.Elements(Of Paragraph).FirstOrDefault()
                                paragrTotalxMes.RemoveAllChildren()
                                paragrTotalxMes.Append(paragraphPropertiesCentro.Clone)
                                paragrTotalxMes.Append(runTotalxMes)
                                '---------------------------------------------------
                            End If
                        Next

                    Case Else 'Heaving y Otros

                        'Reiniciar variables
                        SumaTotal = 0
                        PosicionFilaTotal = 0
                        CantFilaRecorrer = 0

                        '--- Cantidad de filas segun el tipo de Plantilla: CantidadFilasCuadro - cantidadFilasCabecera(2) - cantidadFilasTotal(2)----------------------
                        Select Case TipoPLantilla
                            Case "1F"
                                CantFilaRecorrer = 5
                                PosicionFilaTotal = 7

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM 1F")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 1F")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                            Case "2F"
                                CantFilaRecorrer = 5
                                PosicionFilaTotal = 7

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM 2F")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 2F")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                            Case "12F"
                                CantFilaRecorrer = 6
                                PosicionFilaTotal = 8

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM 1F")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM 2F")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 1F")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 2F")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select

                            Case "1F125"
                                CantFilaRecorrer = 6
                                PosicionFilaTotal = 8

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM 125")
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM 1F")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                'Con doble espacio PM  125
                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM  125")
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 1F")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select

                            Case "2F125"
                                CantFilaRecorrer = 6
                                PosicionFilaTotal = 8

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM 125")
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM 2F")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                'Con doble espacio PM  125
                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM  125")
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 2F")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select

                            Case "12F125"
                                CantFilaRecorrer = 7
                                PosicionFilaTotal = 9

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM 125")
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM 1F")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM 2F")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                'Con doble espacio PM  125
                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM  125")
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 1F")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 2F")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                            Case "125"
                                CantFilaRecorrer = 5
                                PosicionFilaTotal = 7

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM 125")
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                'Con doble espacio PM  125
                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM  125")
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                            Case "CSA"
                                CantFilaRecorrer = 4
                                PosicionFilaTotal = 6

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                            Case Else
                                CantFilaRecorrer = 4
                                PosicionFilaTotal = 6

                                CollectEventosWord = New List(Of String)
                                CollectEventosWord.Add("PM1")
                                CollectEventosWord.Add("PM2")
                                CollectEventosWord.Add("PM3")
                                CollectEventosWord.Add("PM4")

                                CollectEventosTarifaBD = New List(Of String)
                                CollectEventosTarifaBD.Add("PM 1")
                                CollectEventosTarifaBD.Add("PM 2")
                                CollectEventosTarifaBD.Add("PM 3")
                                CollectEventosTarifaBD.Add("PM 4")
                                Exit Select
                        End Select
                        '-------------------------------------------------------------------------------
                        'Recorrer segun cantida de fila
                        For i As Integer = 1 To CantFilaRecorrer

                            llaveBusqueda = fila("ModeloBase") & "@" & fila("Prefijo") & "@" & fila("IdPlan") & "@" & CollectEventosTarifaBD(i - 1) ' "PM " + i.ToString()
                            EventoWord = CollectEventosWord(i - 1) 'String.Concat("PM", i.ToString)

                            l_Tarifa.Clear()
                            eValidacion.flag = 1
                            eTarifa.llave = llaveBusqueda
                            cTarifa.ListarModBaseCodPlanPrefPM(strConexionSql, eTarifa, eValidacion, l_Tarifa)

                            If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                l_Tarifa = l_Tarifa.Where(Function(c) c.conFluidos.ToString().ToUpper() = "CON FLUIDOS").ToList()
                            Else
                                l_Tarifa = l_Tarifa.Where(Function(c) c.conFluidos.ToString().ToUpper() = "SIN FLUIDOS").ToList()
                            End If

                            If l_Tarifa.ToList.Count > 0 Then
                                flatExiteTarifa = True

                                Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                                Dim cantColum As Integer = 0

                                '----- Escribir la cantidad de Eventos ----------------------------------------------------------
                                If i = 1 Then
                                    Dim trFilaTituloCantEvento As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i)
                                    'Dim tcCeldaTituloCantEvento As TableCell = trFilaTituloCantEvento.Elements(Of TableCell)().ElementAt(5) ' cuando hay 7 columnas
                                    Dim tcCeldaTituloCantEvento As TableCell = trFilaTituloCantEvento.Elements(Of TableCell)().ElementAt(2) ' cuando hay 4 columnaas
                                    Dim runTituloCantEvento As Run = tcCeldaTituloCantEvento.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                    'runTituloCantEvento.AppendChild(New Text(" " & Duracion.ToString & " " & Homologacion.ObtenerUnidadDuracion(IdUnidadDuracion)))
                                    runTituloCantEvento.AppendChild(New Text(" 2,000 Horas"))
                                End If
                                '------------------------------------------------------------------------------------------------

                                Dim trFila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(i + 1)
                                Dim tcCelda1 As TableCell = trFila.Elements(Of TableCell)().ElementAt(0)
                                Dim run1 As Run = tcCelda1.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                'Compara el Evento
                                If run1.InnerText = EventoWord Then
                                    'Recorrer las columnas de una fila 
                                    For j As Integer = 0 To 3   'Para 4 columnas ..'6 'Para 7 columnas
                                        Dim tcCelda As TableCell = trFila.Elements(Of TableCell)().ElementAt(j)
                                        Dim para As Paragraph = tcCelda.Elements(Of Paragraph)().First()
                                        Dim runCelda As Run = para.AppendChild(New Run)
                                        Dim ValorCelda As String = String.Empty
                                        Select Case j
                                            Case 1
                                                ' ''Kit de Repuestos
                                                ''If (IsNumeric(l_Tarifa.First().kitRepuestos)) Then
                                                ''    ValorCelda = FormatoMoneda(l_Tarifa.First().kitRepuestos)
                                                ''Else
                                                ''    ValorCelda = l_Tarifa.First().kitRepuestos
                                                ''End If

                                                ''runCelda.AppendChild(New Text(ValorCelda))

                                                If (IsNumeric(l_Tarifa.First().total)) Then
                                                    ValorCelda = FormatoMoneda(l_Tarifa.First().total)
                                                Else
                                                    ValorCelda = l_Tarifa.First().total
                                                End If

                                                runCelda.AppendChild(New Text(ValorCelda))

                                                Exit Select
                                            Case 2
                                                ' ''Fluido
                                                ''If Not IsNothing(l_Tarifa.First().fluidos) Then
                                                ''    If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                                ''        If (IsNumeric(l_Tarifa.First().fluidos)) Then
                                                ''            ValorCelda = FormatoMoneda(l_Tarifa.First().fluidos)
                                                ''        Else
                                                ''            ValorCelda = l_Tarifa.First().fluidos
                                                ''        End If
                                                ''        runCelda.AppendChild(New Text(ValorCelda))
                                                ''        Exit Select
                                                ''    Else
                                                ''        runCelda.AppendChild(New Text("0"))
                                                ''    End If
                                                ''Else
                                                ''    runCelda.AppendChild(New Text("0"))
                                                ''End If
                                                If EsNueva Then
                                                    runCelda.AppendChild(New Text(l_Tarifa.First().eventosNueva))
                                                    'TotalPM = l_Tarifa.First().eventosNueva * l_Tarifa.First().total
                                                    '==========================================
                                                    If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                                        If (IsNumeric(l_Tarifa.First().fluidos)) Then
                                                            TotalPM = l_Tarifa.First().eventosNueva * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().fluidos) + CDec(l_Tarifa.First().servicioContratado))
                                                        End If
                                                    Else
                                                        TotalPM = l_Tarifa.First().eventosNueva * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().servicioContratado))

                                                    End If
                                                    '==========================================
                                                Else
                                                    runCelda.AppendChild(New Text(l_Tarifa.First().eventosUsada))
                                                    'TotalPM = l_Tarifa.First().eventosUsada * l_Tarifa.First().total
                                                    '==========================================
                                                    If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                                        If (IsNumeric(l_Tarifa.First().fluidos)) Then
                                                            TotalPM = l_Tarifa.First().eventosUsada * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().fluidos) + CDec(l_Tarifa.First().servicioContratado))
                                                        End If
                                                    Else
                                                        TotalPM = l_Tarifa.First().eventosUsada * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().servicioContratado))

                                                    End If
                                                    '==========================================
                                                End If
                                                Exit Select
                                            Case 3

                                                ' ''Servicio contratado
                                                ''If (IsNumeric(l_Tarifa.First().servicioContratado)) Then
                                                ''    ValorCelda = FormatoMoneda(l_Tarifa.First().servicioContratado)
                                                ''Else
                                                ''    ValorCelda = l_Tarifa.First().servicioContratado
                                                ''End If

                                                ''runCelda.AppendChild(New Text(ValorCelda))

                                                If (IsNumeric(TotalPM)) Then
                                                    ValorCelda = FormatoMoneda(TotalPM)
                                                Else
                                                    ValorCelda = TotalPM
                                                End If

                                                runCelda.AppendChild(New Text(ValorCelda))
                                                SumaTotal += TotalPM
                                                Exit Select
                                                'Case 4
                                                '    If (IsNumeric(l_Tarifa.First().total)) Then
                                                '        ValorCelda = FormatoMoneda(l_Tarifa.First().total)
                                                '    Else
                                                '        ValorCelda = l_Tarifa.First().total
                                                '    End If

                                                '    runCelda.AppendChild(New Text(ValorCelda))
                                                '    Exit Select
                                                'Case 5
                                                '    If EsNueva Then
                                                '        runCelda.AppendChild(New Text(l_Tarifa.First().eventosNueva))
                                                '        'TotalPM = l_Tarifa.First().eventosNueva * l_Tarifa.First().total
                                                '        '==========================================
                                                '        If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                                '            If (IsNumeric(l_Tarifa.First().fluidos)) Then
                                                '                TotalPM = l_Tarifa.First().eventosNueva * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().fluidos) + CDec(l_Tarifa.First().servicioContratado))
                                                '            End If
                                                '        Else
                                                '            TotalPM = l_Tarifa.First().eventosNueva * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().servicioContratado))

                                                '        End If
                                                '        '==========================================
                                                '    Else
                                                '        runCelda.AppendChild(New Text(l_Tarifa.First().eventosUsada))
                                                '        'TotalPM = l_Tarifa.First().eventosUsada * l_Tarifa.First().total
                                                '        '==========================================
                                                '        If fila.Item("IncluyeFluidos") = True Or fila.Item("IncluyeFluidos").ToString() = "1" Or fila.Item("IncluyeFluidos").ToString().ToUpper = "CON FLUIDOS" Then
                                                '            If (IsNumeric(l_Tarifa.First().fluidos)) Then
                                                '                TotalPM = l_Tarifa.First().eventosUsada * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().fluidos) + CDec(l_Tarifa.First().servicioContratado))
                                                '            End If
                                                '        Else
                                                '            TotalPM = l_Tarifa.First().eventosUsada * (CDec(l_Tarifa.First().kitRepuestos) + CDec(l_Tarifa.First().servicioContratado))

                                                '        End If
                                                '        '==========================================
                                                '    End If

                                                '    Exit Select
                                                'Case 6
                                                '    If (IsNumeric(TotalPM)) Then
                                                '        ValorCelda = FormatoMoneda(TotalPM)
                                                '    Else
                                                '        ValorCelda = TotalPM
                                                '    End If

                                                '    runCelda.AppendChild(New Text(ValorCelda))
                                                '    SumaTotal += TotalPM
                                                '    Exit Select
                                        End Select

                                        If j <> 0 Then
                                            runCelda.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                        End If

                                    Next
                                End If

                                '--- Escribir en total ---------------------------------------------------------------------------------
                                'Dim rPrTotal As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , "DERECHA"))
                                Dim trFilaTotal As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal)
                                Dim tcCeldaTotal As TableCell = trFilaTotal.Elements(Of TableCell)().ElementAt(2)
                                Dim runTotal As Run = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                Dim total As String = String.Empty

                                If IsNumeric(SumaTotal) Then
                                    total = FormatoMoneda(SumaTotal)
                                Else
                                    total = SumaTotal
                                End If

                                If IsNothing(runTotal) Then
                                    runTotal = New Run
                                    runTotal.AppendChild(New Text(total))
                                    tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault().InsertAt(runTotal, 0)
                                Else
                                    runTotal.RemoveAllChildren()
                                    runTotal.AppendChild(New Text(total))
                                End If
                                runTotal.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())

                                '--alinear a la derecha----------------------------
                                Dim paragrTotal As Paragraph = tcCeldaTotal.Elements(Of Paragraph).FirstOrDefault()
                                paragrTotal.RemoveAllChildren()
                                paragrTotal.Append(paragraphPropertiesCentro.Clone)
                                paragrTotal.Append(runTotal)
                                '---------------------------------------------------


                                'Dim rPrTotalxHora As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , "DERECHA"))
                                Dim trFilaTotalxHora As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(PosicionFilaTotal + 1)
                                Dim tcCeldaTotalxHora As TableCell = trFilaTotalxHora.Elements(Of TableCell)().ElementAt(2)
                                Dim runTotalxHora As Run = tcCeldaTotalxHora.Elements(Of Paragraph).FirstOrDefault().Elements(Of Run).FirstOrDefault
                                Dim totalPorHoras As String = String.Empty
                                Try
                                    'totalPorHoras = SumaTotal / fila("HorometroFin")
                                    totalPorHoras = SumaTotal / 2000
                                    totalPorHoras = CStr(Decimal.Round(CDec(totalPorHoras), 4))
                                Catch ex As Exception
                                    totalPorHoras = String.Empty
                                End Try

                                If (IsNumeric(totalPorHoras)) Then
                                    totalPorHoras = FormatoMoneda(totalPorHoras)
                                End If

                                If IsNothing(runTotalxHora) Then
                                    runTotalxHora = New Run
                                    runTotalxHora.AppendChild(New Text(totalPorHoras))
                                    tcCeldaTotalxHora.Elements(Of Paragraph).FirstOrDefault().InsertAt(runTotalxHora, 0)
                                Else
                                    runTotalxHora.RemoveAllChildren()
                                    runTotalxHora.AppendChild(New Text(totalPorHoras))
                                End If
                                runTotalxHora.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos20.Clone())
                                '--alinear a la derecha----------------------------
                                Dim paragrTotalxHora As Paragraph = tcCeldaTotalxHora.Elements(Of Paragraph).FirstOrDefault()
                                paragrTotalxHora.RemoveAllChildren()
                                paragrTotalxHora.Append(paragraphPropertiesCentro.Clone)
                                paragrTotalxHora.Append(runTotalxHora)
                                '---------------------------------------------------
                            End If
                        Next
                End Select

                If flatExiteTarifa Then
                    If ExisteTituloAnexo2 = False Then
                        'Escribir Anexo 2 
                        Dim runBreakAnexo As New Run()
                        runBreakAnexo.AppendChild(New Break())
                        runBreakAnexo.AppendChild(New Break())
                        Dim rPrTituloAnexo As RunProperties = New RunProperties(RunPropertiesConfigurado(True, True, , , , ))
                        Dim paraTituloAnexo As New Paragraph()

                        Dim paragraphProperties1 As New ParagraphProperties()
                        Dim justification1 As New Justification()
                        justification1.Val = JustificationValues.Center
                        paragraphProperties1.Append(justification1)

                        Dim runTituloAnexo As New Run(New Text("ANEXO 2"))
                        runTituloAnexo.PrependChild(Of RunProperties)(rPrTituloAnexo)
                        paraTituloAnexo.Append(paragraphProperties1)
                        paraTituloAnexo.Append(runBreakAnexo)
                        paraTituloAnexo.Append(runTituloAnexo)
                        runTituloAnexo.AppendChild(New Break())
                        wordDoc.MainDocumentPart.Document.Body.InsertAt(paraTituloAnexo, 0) 'Insertar en la Posicion 0 del doc

                        ExisteTituloAnexo2 = True
                    End If
                End If
                'Guardar Cambios en el documento
                wordDoc.MainDocumentPart.Document.Save()
                If flatExiteTarifa Then
                    ListaByteDocumentoTemp.Add(objMemoryStreamTemp.ToArray)
                End If
            End Using
            '======================================================
        Next

        Dim rtemp As Byte() = UnirDocumentos(ListaByteDocumentoTemp)

        Dim msDocGenerado As New MemoryStream
        msDocGenerado.Write(rtemp, 0, rtemp.Length)

        objMemoryStream = Nothing
        objMemoryStream = msDocGenerado
    End Sub

    Private Sub ConstruirExcelDetallePartes(ByVal dtbDatos As DataTable, ByVal NombreArchivo As String)
        Try

            Dim objMemoryStream As New MemoryStream
            Dim Contador As Integer = 0
            Dim Generado As Boolean = False

            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoDetallePartes, String.Concat("PlantillaDetalle", ".xlsx"))

            If objMemoryStream.Length < 0 Then
                Exit Sub
            End If

            If dtbDatos.Rows.Count <= 0 Then
                Exit Sub
            End If

            For Each drFila As DataRow In dtbDatos.Rows

                Dim strNombModelo As String = String.Empty

                If drFila("IncluyeDetallePartes") = 0 Or drFila("IncluyeDetallePartes") = False Then
                    Continue For
                End If


                If IsDBNull(drFila.Item("Modelo")) Then
                    strNombModelo = drFila.Item("ModeloBase")
                Else
                    If Not IsDBNull(drFila.Item("Modelo")) Then
                        strNombModelo = drFila.Item("Modelo")
                    Else
                        strNombModelo = ""
                    End If
                End If

                If String.IsNullOrEmpty(strNombModelo) Then
                    strNombModelo = "Modelo Vacio"
                End If
                'strLlave = drFila.Item("Familia") & "@" & drFila.Item("ModeloBase") & "@" & drFila.Item("Prefijo")

                Dim obcDetallePartes As New bcDetallePartes
                Dim obeDetallePartes As New beDetallePartes
                Dim ListaDetallePartes As New List(Of beDetallePartes)
                Dim ebeValidacion As New beValidacion

                obeDetallePartes.Familia = drFila.Item("Familia")
                obeDetallePartes.ModeloBase = drFila.Item("ModeloBase")
                obeDetallePartes.Prefijo = drFila.Item("Prefijo")
                obeDetallePartes.CodPlan = drFila.Item("IdPlan")

                If drFila.Item("IncluyeFluidos") = True Or drFila.Item("IncluyeFluidos") = 1 Then
                    obeDetallePartes.IncluyeFluidos = 1
                Else
                    obeDetallePartes.IncluyeFluidos = 0
                End If


                obcDetallePartes.BuscarLlave(Modulo.strConexionSql, obeDetallePartes, ebeValidacion, ListaDetallePartes)
                If Not ListaDetallePartes.Count > 0 Then
                    obeDetallePartes.CodPlan = ""
                    obcDetallePartes.BuscarLlave(Modulo.strConexionSql, obeDetallePartes, ebeValidacion, ListaDetallePartes)
                End If

                '000000000000000000000000000000000000000000000000000000000000000000000000


                Using documentoExcel As SpreadsheetDocument = SpreadsheetDocument.Open(objMemoryStream, True)

                    'Obtener Data de la plantilla
                    Dim NuevoSheetData As New DocumentFormat.OpenXml.Spreadsheet.SheetData()
                    NuevoSheetData = documentoExcel.WorkbookPart.WorksheetParts.FirstOrDefault.Worksheet.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.SheetData)().Clone()


                    'Escribir Familia, Modelo, Prefijo
                    For ind As Integer = 4 To 6

                        Dim row = NuevoSheetData.Elements(Of Spreadsheet.Row)().Where(Function(r) r.RowIndex.Value = ind).FirstOrDefault()

                        If Not IsNothing(row) Then

                            Dim strValorCelda As String = String.Empty

                            Select Case ind
                                Case 4
                                    strValorCelda = obeDetallePartes.Familia
                                    Exit Select
                                Case 5
                                    strValorCelda = strNombModelo
                                    Exit Select
                                Case 6
                                    strValorCelda = obeDetallePartes.Prefijo
                                    Exit Select
                            End Select

                            If ind >= 4 Then
                                Dim CellB = row.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Cell).Where(Function(r) r.CellReference.Value = String.Concat("B", ind.ToString)).First
                                CellB.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(CStr(strValorCelda))
                                CellB.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.String)
                            Else
                                'Dim MergeCell As DocumentFormat.OpenXml.Spreadsheet.MergeCell = New DocumentFormat.OpenXml.Spreadsheet.MergeCell() With {.Reference = "A3:I3"}
                                'row.InsertAt(MergeCell, 3)
                            End If

                        End If
                    Next


                    'Escribir la data..empieza de la fila 9
                    Dim IndDetallePartes As Integer = 9
                    For Each beDetallePartes In ListaDetallePartes
                        Dim row = NuevoSheetData.Elements(Of Spreadsheet.Row)().Where(Function(r) r.RowIndex.Value = IndDetallePartes).FirstOrDefault()

                        'Eliminamos todas las celda
                        row.RemoveAllChildren()

                        'Dim row = GetRow(NuevoSheetData, 1)
                        If Not IsNothing(row) Then

                            ' Add the cell to the cell table at A1.
                            Dim newCellA As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("A", IndDetallePartes.ToString)}
                            newCellA.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.ServiceCategory)
                            newCellA.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.String)
                            row.InsertAt(newCellA, 0)

                            Dim newCellB As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("B", IndDetallePartes.ToString)}
                            newCellB.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.Rodetail)
                            newCellB.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.String)
                            row.InsertAt(newCellB, 1)

                            Dim newCellC As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("C", IndDetallePartes.ToString)}
                            newCellC.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.FirstInterval)
                            newCellC.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                            row.InsertAt(newCellC, 2)

                            Dim newCellD As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("D", IndDetallePartes.ToString)}
                            newCellD.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.NextInterval)
                            newCellD.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                            row.InsertAt(newCellD, 3)

                            Dim newCellE As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("E", IndDetallePartes.ToString)}
                            newCellE.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.CompQty)
                            newCellE.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                            row.InsertAt(newCellE, 4)

                            Dim newCellF As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("F", IndDetallePartes.ToString)}
                            newCellF.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.JODETAIL)
                            newCellF.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.String)
                            row.InsertAt(newCellF, 5)

                            Dim newCellG As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("G", IndDetallePartes.ToString)}
                            newCellG.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(CStr(beDetallePartes.SOSPartNumber) + " " + CStr(beDetallePartes.SOSDescription))
                            newCellG.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.String)
                            row.InsertAt(newCellG, 6)

                            Dim newCellH As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("H", IndDetallePartes.ToString)}
                            newCellH.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.Quantity)
                            newCellH.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                            row.InsertAt(newCellH, 7)

                            Dim newCellI As New DocumentFormat.OpenXml.Spreadsheet.Cell() With {.CellReference = String.Concat("I", IndDetallePartes.ToString)}
                            newCellI.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(beDetallePartes.Replacement)
                            newCellI.DataType = New EnumValue(Of DocumentFormat.OpenXml.Spreadsheet.CellValues)(DocumentFormat.OpenXml.Spreadsheet.CellValues.Number)
                            row.InsertAt(newCellI, 8)

                        End If
                        'sheetData.Append(row)
                        IndDetallePartes += 1
                    Next


                    ' Agregar un blank WorksheetPart.
                    Dim newWorksheetPart As WorksheetPart = documentoExcel.WorkbookPart.AddNewPart(Of WorksheetPart)()
                    newWorksheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet(NuevoSheetData) 'New DocumentFormat.OpenXml.Spreadsheet.SheetData())
                    'newWorksheetPart.Worksheet.Save()

                    Dim sheets As DocumentFormat.OpenXml.Spreadsheet.Sheets = documentoExcel.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)()
                    Dim relationshipId As String = documentoExcel.WorkbookPart.GetIdOfPart(newWorksheetPart)

                    ' Get a unique ID for the new worksheet.
                    Dim sheetId As UInteger = 1
                    If (sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet).Count > 0) Then
                        sheetId = sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet).Select(Function(s) s.SheetId.Value).Max + 1
                    End If

                    ' Nombre del nuevo worksheet.
                    Dim sheetName As String = strNombModelo '("Sheet" + sheetId.ToString())

                    ' Append the new worksheet and associate it with the workbook.
                    Dim sheet As DocumentFormat.OpenXml.Spreadsheet.Sheet = New DocumentFormat.OpenXml.Spreadsheet.Sheet
                    sheet.Id = relationshipId
                    sheet.SheetId = sheetId
                    sheet.Name = CStr((Contador + 1)) + "." + sheetName
                    sheets.Append(sheet)

                End Using

                '000000000000000000000000000000000000000000000000000000000000000000000
                Generado = True
                Contador += 1
            Next

            If objMemoryStream.Length > 0 Then
                If Generado = True Then
                    '====== Eliminar la Primera Hoja =======================================
                    Using documentoExcelFinal As SpreadsheetDocument = SpreadsheetDocument.Open(objMemoryStream, True)

                        Dim sheets As DocumentFormat.OpenXml.Spreadsheet.Sheets = documentoExcelFinal.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)()
                        If documentoExcelFinal.WorkbookPart.Workbook.Sheets.Count > 1 Then
                            Dim sheet As DocumentFormat.OpenXml.Spreadsheet.Sheet = sheets.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)()
                            If sheet.Name = "Hoja1" Or sheet.Id = "rId1" Then
                                sheet.RemoveAllChildren()
                                sheet.Remove()
                                documentoExcelFinal.WorkbookPart.Workbook.Save()
                            End If

                        End If

                    End Using
                    '===================================================================================================
                    objAdminFTP.SubirArchivo(objMemoryStream, Modulo.strUrlFtpArchivoDetallePartes, String.Concat(NombreArchivo, ".xlsx"))
                    Me.TieneDetallePartes = 1
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub GenerarTerminosCondiciones(ByRef objMemoryStream As MemoryStream, ByVal dtbDatos As DataTable, ByVal obeTablaMaestra As beTablaMaestra)
        Dim NombreArchivo As String = String.Empty
        Dim objMemoryStreamTemp As New MemoryStream
        Dim ListaByteDocumentoTemp As New List(Of Byte())
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        Dim dtbProducto As New DataTable
        Dim ResponsableServicio As String = String.Empty
        Dim SupervisorServicio As String = String.Empty
        Dim TextoDuracion As String = String.Empty

        NombreArchivo = String.Empty

        str_mensajeRecorrido.AppendLine(" 5- Terminos Y condiciones - " & Now.ToString())

        obcArchivoConfiguracion.BuscarArchivoProducto(Modulo.strConexionSql, obeTablaMaestra, dtbProducto)
        For Each drProducto As DataRow In dtbProducto.Rows

            ResponsableServicio = drProducto.Item("NombreResponsableServicio")
            SupervisorServicio = drProducto.Item("NombreSupervisorServicio")

            If drProducto("IdUnidadDuracion") = "H" Then
                TextoDuracion = "hasta cumplir el período establecido (2000 horas)"
            Else
                TextoDuracion = "desde " + drProducto.Item("FechaInicioContrato") + " hasta " + drProducto.Item("FechaFinContrato")
            End If



            If drProducto.Item("TipoProducto") = TipoProducto.CSA Or drProducto.Item("TipoProducto") = TipoProducto.SOLUCION_COMBINADA Then
                NombreArchivo = drProducto.Item("Archivo").ToString

                objMemoryStreamTemp = New MemoryStream

                objMemoryStreamTemp = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)

                If objMemoryStreamTemp.Length > 0 Then
                    '--- Aquí trabajo con marcadores -------------------------------
                    Try
                        Dim xmlDocumento As New System.Xml.XmlDocument
                        Dim xpnDocumento As System.Xml.XPath.XPathNavigator
                        Dim xnmDocumento As System.Xml.XmlNamespaceManager
                        Dim msPackage As IO.Packaging.PackagePart
                        Dim uriPartTarget As Uri
                        Dim strCampo As String
                        Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

                        Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStreamTemp, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                            'Recupera el xml del contenido del documento
                            uriPartTarget = New Uri("/word/document.xml", UriKind.Relative)
                            msPackage = package.GetPart(uriPartTarget)
                            xmlDocumento.Load(msPackage.GetStream)

                            'Se crea el navegador
                            xpnDocumento = xmlDocumento.CreateNavigator()
                            xnmDocumento = New System.Xml.XmlNamespaceManager(xpnDocumento.NameTable)
                            xnmDocumento.AddNamespace("w", strUri)
                            For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                                strCampo = String.Empty
                                If xpnCampo.MoveToChild("name", strUri) Then
                                    If xpnCampo.MoveToAttribute("val", strUri) Then
                                        strCampo = xpnCampo.Value
                                    End If
                                    'Move to w:instrText
                                    If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                        'Check FORMTEXT
                                        If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                            'Move to t
                                            If xpnCampo.MoveToFollowing("t", strUri) Then
                                                'Set value
                                                Select Case strCampo.ToUpper
                                                    Case UCase("ResponsableServicio")
                                                        xpnCampo.SetValue(ResponsableServicio)
                                                        Exit Select
                                                    Case UCase("SupervisorServicio")
                                                        xpnCampo.SetValue(SupervisorServicio)
                                                        Exit Select
                                                    Case UCase("Duracion")
                                                        xpnCampo.SetValue(TextoDuracion)
                                                End Select
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                            'Actualiza y cierra el documento
                            xmlDocumento.Save(msPackage.GetStream(IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))
                            msPackage.GetStream.Flush()
                            msPackage.GetStream.Close()
                        End Using

                    Catch ex As Exception
                    End Try
                    '---------------------------------------------------------------

                    ListaByteDocumentoTemp.Add(objMemoryStreamTemp.ToArray())
                End If
            End If
        Next

        If ListaByteDocumentoTemp.Count > 0 Then
            Dim rtemp As Byte()
            Dim msDocGenerado As New MemoryStream

            If ListaByteDocumentoTemp.Count = 1 Then
                rtemp = ListaByteDocumentoTemp(0)
                msDocGenerado.Write(rtemp, 0, rtemp.Length)
            Else
                rtemp = UnirDocumentos(ListaByteDocumentoTemp)
                msDocGenerado.Write(rtemp, 0, rtemp.Length)
            End If

            If msDocGenerado.Length > 0 Then
                objMemoryStream = Nothing
                objMemoryStream = msDocGenerado
            End If

        End If


    End Sub

    Private Sub GenerarIndiceCartaPresentacion(ByVal listaDocImpreso As List(Of beTablaMaestra), ByRef ListaByteDocumento As List(Of Byte()))

        Try
            Dim Contador As Integer = 0
            Dim indCartaPresentacion As Integer = -1
            Dim mainStream As New MemoryStream()

            If ListaByteDocumento.Count > 0 Then
                'Buscar Indice de Carta Presentacion
                For Each eTablaMaestracDoc As beTablaMaestra In listaDocImpreso
                    If eTablaMaestracDoc.Codigo = CodigoSeccion.CartaPresentacion Then
                        indCartaPresentacion = Contador
                        Exit For
                    End If
                    Contador += 1
                Next
                If indCartaPresentacion <> -1 Then
                    'Dim mainStream As New MemoryStream()
                    mainStream.Write(ListaByteDocumento(indCartaPresentacion), 0, ListaByteDocumento(indCartaPresentacion).Length)
                    mainStream.Position = 0
                    Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)
                        '--- Tabla 2: Secciones de la Cotizacion----------------------------------------------
                        Dim tableSecciones As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                        Dim nfilaSecciones As Integer = tableSecciones.Elements(Of TableRow).Count
                        Dim nfilamaxSecciones As Integer = listaDocImpreso.Count
                        Dim iSecciones As Integer = 1

                        For Each eTablaMaestra As beTablaMaestra In listaDocImpreso

                            If eTablaMaestra.Codigo = CodigoSeccion.CartaPresentacion Then
                                'iSecciones = iSecciones + 1
                                Continue For
                            End If

                            If iSecciones <= nfilaSecciones Then
                                'llenando las filas existentes
                                Dim row As TableRow = tableSecciones.Elements(Of TableRow)().ElementAt(iSecciones - 1)
                                Dim cell As TableCell = row.Elements(Of TableCell)().ElementAt(0)
                                Dim para As Paragraph = cell.Elements(Of Paragraph)().First()
                                Dim run As Run = para.AppendChild(New Run())
                                run.AppendChild(New Text(eTablaMaestra.Nombre))
                                run.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())
                            Else
                                'insertando y llegando las filas faltantes
                                Dim row As New TableRow()
                                Dim cell As New TableCell()
                                cell.Append(New TableCellProperties(New TableCellWidth()))

                                Dim para As Paragraph = cell.AppendChild(New Paragraph)
                                Dim run As Run = para.AppendChild(New Run)
                                run.AppendChild(New Text(eTablaMaestra.Nombre))
                                run.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())
                                row.Append(cell)
                                tableSecciones.Append(row)
                            End If
                            iSecciones = iSecciones + 1

                        Next ' Fin de Tabla Seccion de Cotizacion
                        '--------------------------------------------------------------------------------------
                        wordDoc.MainDocumentPart.Document.Save()

                    End Using
                End If

                If indCartaPresentacion <> -1 Then
                    If mainStream.Length > 0 Then
                        mainStream.Position = 0
                        ListaByteDocumento(indCartaPresentacion) = mainStream.ToArray
                    End If
                End If
            End If
        Catch ex As Exception
            Dim mesajeError = ex.Message
        End Try
    End Sub

    Private Sub AdjuntarEspecificacionAdicionales(ByRef objMemoryStreamEspecificacion As MemoryStream, _
                                                  ByVal drProducto As DataRow, ByVal dtbAdicionalProducto As DataTable, _
                                                  ByVal PosicionProductos As Integer, ByVal dtbAccesorioProducto As DataTable)
        '-----------------------------------------------------------------------------------
        Dim obeTablaMaestra As New beTablaMaestra
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        Dim drAdicionalesArray() As DataRow
        Dim CodigoBusqueda As New StringBuilder
        Dim dtbArchivoAdicionalAccesorio As New DataTable
        Dim ListaByteDocumentoAdicional As New List(Of Byte())

        Dim drAccesoriosArray() As DataRow

        drAdicionalesArray = dtbAdicionalProducto.Select("IdProducto = " + drProducto.Item("IdProducto").ToString.Trim)
        drAccesoriosArray = dtbAccesorioProducto.Select("IdProducto = " + drProducto.Item("IdProducto").ToString.Trim)

        Try
            If drAdicionalesArray Is Nothing Then drAdicionalesArray = (New List(Of DataRow)).ToArray
            If drAccesoriosArray Is Nothing Then drAccesoriosArray = (New List(Of DataRow)).ToArray
        Catch ex As Exception
        End Try

        'memoryStream de Especificacion Tecnica
        ListaByteDocumentoAdicional.Add(objMemoryStreamEspecificacion.ToArray)

        'Codigos de adicional
        For Each drAdicional In drAdicionalesArray
            If drAdicional("flatMostrarEspTecnica").ToString = "1" _
              Or drAdicional("flatMostrarEspTecnica").ToString().ToUpper() = "TRUE" Then
                CodigoBusqueda.Append("'")
                CodigoBusqueda.Append(drAdicional("IdAdicional").ToString)
                CodigoBusqueda.Append("'")
                CodigoBusqueda.Append(",")
            End If

        Next

        'Codigos de accesorio
        For Each drAccesorio In drAccesoriosArray
            If drAccesorio("flatMostrarEspTecnica").ToString = "1" _
              Or drAccesorio("flatMostrarEspTecnica").ToString().ToUpper() = "TRUE" Then
                CodigoBusqueda.Append("'")
                CodigoBusqueda.Append(drAccesorio("IdAccesorio").ToString)
                CodigoBusqueda.Append("'")
                CodigoBusqueda.Append(",")
            End If
        Next

        'Cortar la ultima coma
        If CodigoBusqueda.Length > 1 Then
            CodigoBusqueda.Remove(CodigoBusqueda.Length - 1, 1)
        End If


        If Not String.IsNullOrEmpty(CodigoBusqueda.ToString) Then

            'Buscar Archivos para los adicionales
            obeTablaMaestra.Codigo = CodigoBusqueda.ToString
            obeTablaMaestra.Nombre = "CODIGO"
            obcArchivoConfiguracion.BuscarArchivoProductoGeneral(Modulo.strConexionSql, obeTablaMaestra, dtbArchivoAdicionalAccesorio)

            Dim contadorAdicional As Integer = 1
            For Each drArchivo As DataRow In dtbArchivoAdicionalAccesorio.Rows
                Dim NombreArchivoAdicionalTemp As String = String.Empty
                NombreArchivoAdicionalTemp = drArchivo("Archivo").ToString()
                If Not String.IsNullOrEmpty(NombreArchivoAdicionalTemp) Then
                    Dim objMemoryStreamAdicional As MemoryStream = Nothing
                    objMemoryStreamAdicional = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivoAdicionalTemp)

                    If Not objMemoryStreamAdicional Is Nothing Then
                        If objMemoryStreamAdicional.Length > 0 Then
                            '--Escribir Titulo de Adicional ---
                            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStreamAdicional, True)

                                Dim runBreakAnexo As New Run()
                                runBreakAnexo.AppendChild(New Break())
                                Dim rPrTituloAnexo As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, , , , ))
                                Dim paraTituloAnexo As New Paragraph()

                                Dim NombreAdicionalAccesorio As String = String.Empty
                                'buscar el nombre en adicional
                                For Each drAdicional In drAdicionalesArray
                                    If (drAdicional("IdAdicional").ToString) = drArchivo("Codigo").ToString Then
                                        NombreAdicionalAccesorio = drAdicional("Nombre").ToString
                                        Exit For
                                    End If
                                Next
                                'buscar el nombre en accesorio
                                If NombreAdicionalAccesorio = String.Empty Then
                                    For Each drAccesorio In drAccesoriosArray
                                        If (drAccesorio("IdAccesorio").ToString) = drArchivo("Codigo").ToString Then
                                            NombreAdicionalAccesorio = drAccesorio("Nombre").ToString
                                            Exit For
                                        End If
                                    Next
                                End If


                                'Dim runNombreAdicional As New Run(New Text(NombreAdicional))
                                Dim runNombreAdicional As Run
                                If contadorAdicional = 1 Then
                                    'runNombreAdicional = New Run(New Text(String.Concat(PosicionProductos, ".1", " Beneficios Adicionales / Accesorios")))
                                    runNombreAdicional = New Run(New Text("Beneficios Adicionales / Accesorios"))
                                    runNombreAdicional.AppendChild(New Break())
                                    runNombreAdicional.AppendChild(New Break())
                                    'runNombreAdicional.AppendChild(New Text(String.Concat(PosicionProductos, ".1.", contadorAdicional, " ", NombreAdicionalAccesorio)))
                                    runNombreAdicional.AppendChild(New Text(NombreAdicionalAccesorio))
                                Else
                                    'runNombreAdicional = New Run(New Text(String.Concat(PosicionProductos, ".1.", contadorAdicional, " ", NombreAdicionalAccesorio)))
                                    runNombreAdicional = New Run(New Text(NombreAdicionalAccesorio))
                                End If
                                runNombreAdicional.PrependChild(Of RunProperties)(rPrTituloAnexo)
                                paraTituloAnexo.Append(runBreakAnexo)
                                paraTituloAnexo.Append(runNombreAdicional)
                                runNombreAdicional.AppendChild(New Break())
                                wordDoc.MainDocumentPart.Document.Body.InsertAt(paraTituloAnexo, 0) 'Insertar en la Posicion 0 del doc
                                wordDoc.MainDocumentPart.Document.Save()
                                contadorAdicional += 1
                            End Using

                            '---------------------------------
                            '---Adjuntar Especificaciones de los adicionales
                            ListaByteDocumentoAdicional.Add(objMemoryStreamAdicional.ToArray)
                        End If
                    End If

                End If
            Next

        End If

        'Unir los documentos sin saltos de Lineas
        If ListaByteDocumentoAdicional.Count > 0 Then
            Dim rtemp As Byte() = CombinarDocumentos(ListaByteDocumentoAdicional, False, True)
            Dim msDocGenerado As New MemoryStream
            msDocGenerado.Write(rtemp, 0, rtemp.Length)

            objMemoryStreamEspecificacion = Nothing
            objMemoryStreamEspecificacion = msDocGenerado
        End If

        '-----------------------------------------------------------------------------------------------
    End Sub
#End Region

#Region "Funciones Generales"

    Private Function CombinarDocumentos(ByVal documents As List(Of Byte()), ByVal ConSaltoPagina As Boolean, ByVal ConPiePagina As Boolean) As Byte()

        Dim mainStream As New MemoryStream()
        mainStream.Write(documents(0), 0, documents(0).Length)
        mainStream.Position = 0
        Dim pointer As Integer = 1
        Dim cadenaUnicaTiempo As String = Modulo.GenerarCadenaUnicaTiempo()

        Try
            Using mainDocument As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)

                Dim mainPart As MainDocumentPart = mainDocument.MainDocumentPart

                For pointer = 1 To documents.Count - 1
                    Dim altChunkId As String = "AltChunkId" & "_" & cadenaUnicaTiempo & "_" & pointer
                    Dim chuck As AlternativeFormatImportPart = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId)
                    Dim strm As New MemoryStream(documents(pointer))

                    ' Eliminar los saltos de lineas
                    'ValidarSaltoPagina(strm)

                    chuck.FeedData(strm)

                    Dim altChunk As New AltChunk()
                    altChunk.Id = altChunkId
                    'If pointer <> documents.Count - 1 Then
                    If ConSaltoPagina = True Then
                        mainDocument.MainDocumentPart.Document.Body.Append(New Paragraph(New Run(New Break() With {.Type = BreakValues.Page})))
                    End If
                    mainDocument.MainDocumentPart.Document.Body.InsertAfter(altChunk, mainDocument.MainDocumentPart.Document.Body.LastChild())
                    mainDocument.ExtendedFilePropertiesPart.Properties.Reload()
                    mainDocument.MainDocumentPart.Document.Save()
                Next
            End Using
        Catch

        End Try

        'Agregar Pie de Pagina
        If ConPiePagina = True Then
            Call AddHeaderAndFooter(mainStream, False, True)
        End If

        ' '' Actualizar el Numero de Paginas
        'Dim NumPagina As String = ObtenerNumeroPaginas(mainStream)
        'Using myDocumento As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)
        '    myDocumento.ExtendedFilePropertiesPart.Properties.Pages.Text = NumPagina
        '    myDocumento.MainDocumentPart.Document.Save()
        'End Using
        Return mainStream.ToArray()
    End Function

    Private Function UnirDocumentos(ByVal documents As List(Of Byte())) As Byte()
        Dim cadenaUnicaTiempo As String = Modulo.GenerarCadenaUnicaTiempo()
        Dim mainStream As New MemoryStream()
        mainStream.Write(documents(0), 0, documents(0).Length)
        mainStream.Position = 0
        Dim pointer As Integer = 1

        Try
            Using mainDocument As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)
                Dim mainPart As MainDocumentPart = mainDocument.MainDocumentPart

                For pointer = 1 To documents.Count - 1
                    Dim strm As New MemoryStream()
                    strm.Write(documents(pointer), 0, documents(pointer).Length)
                    strm.Position = 0
                    Using mainDocumentTemp As WordprocessingDocument = WordprocessingDocument.Open(strm, True)
                        For Each XElement As OpenXmlElement In mainDocumentTemp.MainDocumentPart.Document.Body.Elements()
                            If Not XElement.LocalName.ToUpper = UCase("sectPr") Then ' No insertar Propiedades de seccion
                                Dim newXelment As OpenXmlElement
                                newXelment = XElement.Clone

                                mainDocument.MainDocumentPart.Document.Body.InsertAfter(newXelment, mainDocument.MainDocumentPart.Document.Body.LastChild())
                                mainDocument.MainDocumentPart.Document.Save()
                            End If
                        Next
                    End Using
                Next
            End Using
        Catch

        End Try
        'Agregar Pie de Pagina
        Call AddHeaderAndFooter(mainStream, False, True)
        '' Actualizar el Numero de Paginas
        'Dim NumPagina As String = ObtenerNumeroPaginas(mainStream)
        'Using myDocumento As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)
        '    myDocumento.ExtendedFilePropertiesPart.Properties.Pages.Text = NumPagina
        '    myDocumento.MainDocumentPart.Document.Save()
        'End Using
        Return mainStream.ToArray()
    End Function

    Private Function ObtenerNumeroPaginas(ByVal objStream As MemoryStream) As String
        Dim strRetur As String = "0"
        Dim numberOfPages As String = "-1"
        Try
            Dim appWord As New Microsoft.Office.Interop.Word.Application()

            'Dim tmpFile = Path.GetTempFileName()
            Dim strArchivoTemp As String
            strArchivoTemp = HttpContext.Current.Server.MapPath(String.Concat("~/ArchivosCotizador", "\tmpDoc.tmp"))
            'strArchivoTemp = tmpFile
            Dim tmpFileStream = File.OpenWrite(strArchivoTemp)
            tmpFileStream.Write(objStream.ToArray(), 0, objStream.ToArray().Length)
            tmpFileStream.Close()

            Dim document As Microsoft.Office.Interop.Word.Document = appWord.Documents.Open(strArchivoTemp)
            numberOfPages = document.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages, False)

            strRetur = numberOfPages
            document.Close()
            appWord.Quit()

            File.Delete(strArchivoTemp)

        Catch ex As Exception
            'strRetur = "0"
            ' ''========================================
            'Using myDoc As WordprocessingDocument = WordprocessingDocument.Open(objStream, True)
            '    ''// Add a new main document part. 
            '    'Dim mainPart As MainDocumentPart = myDoc.AddMainDocumentPart()
            '    ''//Create Document tree for simple document. 
            '    'mainPart.Document = New Document()
            '    ''//Create Body (this element contains
            '    ''//other elements that we want to include 
            '    'Dim body As Body = New Body()
            '    ''//Create paragraph 
            '    Dim paragraph As New Paragraph()
            '    Dim run_paragraph As New Run()
            '    '// we want to put that text into the output document 
            '    Dim text_paragraph As Text = New Text("Mensaje ERROR: " & ex.Message.ToString & " - numberOfPages: " & numberOfPages)
            '    '//Append elements appropriately. 
            '    run_paragraph.Append(text_paragraph)
            '    paragraph.Append(run_paragraph)
            '    myDoc.MainDocumentPart.Document.Body.Append(paragraph)

            '    '// Save changes to the main document part. 
            '    myDoc.MainDocumentPart.Document.Save()
            'End Using

            'objAdminFTP.SubirArchivoTemporal(objStream, String.Concat(Modulo.strUrlFtpArchivoTemporal, "DocPrueba.docx"))

            ''============================================
        End Try

        Return strRetur
    End Function

    Private Sub GenerarIndice(ByVal listaDocImpreso As List(Of beTablaMaestra), ByRef ListaByteDocumento As List(Of Byte()))

        'Reiniciar las cantidades de paginas
        CantidadPagCartaPresentacion = 0
        CantidadPagPropuestaComercial = 0
        CantidadPagCondicionesGenerales = 0
        CantidadPagEspecificacionesTecnicas = 0
        CantidadPagTerminosCondiciones = 0
        CantidadPagPresentacionFSA = 0
        CantidadPagPresentacionMercado = 0
        CantidadPagRequisitosAprobacionCredito = 0
        CantidadPagRequisitosFormalizacionCredito = 0
        CantidadPagFormatoUCMI = 0

        Try
            Dim ind As Integer = 0
            Dim indCartaPresentacion As Integer = -1

            If ListaByteDocumento.Count > 0 Then
                For Each eTablaMaestra As beTablaMaestra In listaDocImpreso
                    Dim mainStream As New MemoryStream()
                    mainStream.Write(ListaByteDocumento(ind), 0, ListaByteDocumento(ind).Length)
                    mainStream.Position = 0
                    Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)
                        Select Case eTablaMaestra.Codigo
                            Case CodigoSeccion.CartaPresentacion
                                CantidadPagCartaPresentacion += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                indCartaPresentacion = ind
                                eTablaMaestra.PosicionInicial = CantidadPagCartaPresentacion
                                Exit Select
                            Case CodigoSeccion.PropuestaComercial
                                CantidadPagPropuestaComercial += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagPropuestaComercial
                                Exit Select
                            Case CodigoSeccion.CondicionesGenerales
                                CantidadPagCondicionesGenerales += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagCondicionesGenerales
                                Exit Select
                            Case CodigoSeccion.EspecificacionTecnica
                                CantidadPagEspecificacionesTecnicas += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagEspecificacionesTecnicas
                                Exit Select
                            Case CodigoSeccion.TerminosCondiciones
                                CantidadPagTerminosCondiciones += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagTerminosCondiciones
                                Exit Select
                            Case CodigoSeccion.PresentacionFSA
                                CantidadPagPresentacionFSA += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagPresentacionFSA
                                Exit Select
                            Case CodigoSeccion.PresentacionMercado
                                CantidadPagPresentacionMercado += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagPresentacionMercado
                                Exit Select
                            Case CodigoSeccion.RequisitosAprobacionFormalizacionCredito
                                CantidadPagRequisitosAprobacionCredito += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagRequisitosAprobacionCredito
                                Exit Select
                            Case CodigoSeccion.FormatoUCMIAnual
                                CantidadPagRequisitosFormalizacionCredito += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagRequisitosFormalizacionCredito
                                Exit Select
                            Case CodigoSeccion.FormatoUCMIEvento
                                CantidadPagFormatoUCMI += wordDoc.ExtendedFilePropertiesPart.Properties.Pages().Text
                                eTablaMaestra.PosicionInicial = CantidadPagFormatoUCMI
                                Exit Select
                            Case Else
                        End Select
                    End Using
                    listaDocImpreso(ind).PosicionInicial = eTablaMaestra.PosicionInicial
                    ind += 1
                Next

                If indCartaPresentacion <> -1 Then
                    Dim objMemoryStream As New MemoryStream(ListaByteDocumento(indCartaPresentacion), True)
                    objMemoryStream.Position = 0
                    Call EscribirIndice(listaDocImpreso, objMemoryStream)
                    ListaByteDocumento(indCartaPresentacion) = objMemoryStream.ToArray
                End If
            End If
        Catch ex As Exception
            Dim mesajeError = ex.Message
        End Try
    End Sub

    Private Sub AddHeaderAndFooter(ByRef docStream As MemoryStream, ByVal boolAddHead As Boolean, ByVal boolAddFoot As Boolean)
        Try
            ' Replace header in target document with header of source document.
            Using wdDoc As WordprocessingDocument = WordprocessingDocument.Open(docStream, True)
                Dim mainPart As MainDocumentPart = wdDoc.MainDocumentPart

                '1. CABECERA
                If boolAddHead Then
                    ' Delete the existing Header part.
                    mainPart.DeleteParts(mainPart.HeaderParts)
                    ' Create a new Header part.
                    Dim HeaderPart = mainPart.AddNewPart(Of HeaderPart)()
                    ' Get Id of the FooterPart.
                    Dim rId As String = mainPart.GetIdOfPart(HeaderPart)
                    Dim hdr As New Header
                    'HeaderPart.Header = MakeFooter()

                    ' Get SectionProperties and Replace HeaderReference with new Id.
                    Dim sectPrs = mainPart.Document.Body.Elements(Of SectionProperties)()
                    For Each sectPr In sectPrs
                        ' Delete existing references to Headers.
                        sectPr.RemoveAllChildren(Of HeaderReference)()

                        ' Create the new Header reference node.
                        sectPr.PrependChild(Of HeaderReference)(New HeaderReference() With {.Id = rId})
                    Next
                End If

                '2. PIE DE PAGINA
                'If boolAddFoot Then
                If False Then
                    ' Delete the existing Footer part.
                    mainPart.DeleteParts(mainPart.FooterParts)
                    ' Create a new Footer part.
                    Dim footerPart = mainPart.AddNewPart(Of FooterPart)()
                    ' Get Id of the FooterPart.
                    Dim rId As String = mainPart.GetIdOfPart(footerPart)
                    Dim ftr As New Footer
                    footerPart.Footer = MakeFooter()
                    ' Get SectionProperties and Replace FooterReference with new Id.
                    Dim sectPrs = mainPart.Document.Body.Elements(Of SectionProperties)()
                    For Each sectPr In sectPrs
                        ' Delete existing references to Footers.
                        sectPr.RemoveAllChildren(Of FooterReference)()

                        ' Create the new Footer reference node.
                        sectPr.PrependChild(Of FooterReference)(New FooterReference() With {.Id = rId})
                    Next
                End If

                wdDoc.MainDocumentPart.Document.Save()
            End Using

        Catch ex As Exception

        End Try

    End Sub

    Private Function MakeFooter() As Footer
        Dim footer As New Footer()
        Try
            Dim paragraphProperties As New ParagraphProperties(
                                    New ParagraphStyleId() With {.Val = "Footer"},
                                    New Tabs(
                                        New TabStop() With {.Val = TabStopValues.Clear, .Position = 4320},
                                        New TabStop() With {.Val = TabStopValues.Clear, .Position = 8640},
                                        New TabStop() With {.Val = TabStopValues.Center, .Position = 4820},
                                        New TabStop() With {.Val = TabStopValues.Right, .Position = 9639}))

            Dim paragraph As New Paragraph(paragraphProperties,
                            New Run(
                                New FieldChar() With {.FieldCharType = FieldCharValues.Begin}),
                            New Run(
                                New FieldCode(" TITLE   \\* MERGEFORMAT ") With {.Space = SpaceProcessingModeValues.Preserve}
                            ),
                            New Run(
                                New FieldChar() With {.FieldCharType = FieldCharValues.End}),
                            New Run(
                                New PositionalTab() With {.Alignment = AbsolutePositionTabAlignmentValues.Center, .RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, .Leader = AbsolutePositionTabLeaderCharValues.None}
                            ),
                            New Run(
                                New PositionalTab() With {.Alignment = AbsolutePositionTabAlignmentValues.Right, .RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, .Leader = AbsolutePositionTabLeaderCharValues.None}
                            ),
                            New Run(
                                New Text("Página ") With {.Space = SpaceProcessingModeValues.Preserve}
                            ),
                            New SimpleField(
                                New Run(
                                    New RunProperties(
                                        New NoProof()),
                                    New Text("1")
                                )
                            ) With {.Instruction = " PAGE   \\* MERGEFORMAT "}
                        )

            footer.Append(paragraph)

        Catch ex As Exception

        End Try

        Return footer
    End Function

    Private Sub EscribirIndice(ByVal listadocImpreso As List(Of beTablaMaestra), ByRef objMemoryStream As MemoryStream)
        Try
            Dim listaImprimir As New List(Of beTablaMaestra)
            listaImprimir = ListaSinRepeticion(listadocImpreso)
            Dim ind As Integer = 0
            For Each ebeTablaMaestra As beTablaMaestra In listaImprimir
                Select Case ebeTablaMaestra.Codigo
                    Case CodigoSeccion.CartaPresentacion
                        listaImprimir(ind).PosicionInicial = CantidadPagCartaPresentacion
                        Exit Select
                    Case CodigoSeccion.PropuestaComercial
                        listaImprimir(ind).PosicionInicial = CantidadPagPropuestaComercial
                        Exit Select
                    Case CodigoSeccion.CondicionesGenerales
                        listaImprimir(ind).PosicionInicial = CantidadPagCondicionesGenerales
                        Exit Select
                    Case CodigoSeccion.EspecificacionTecnica
                        listaImprimir(ind).PosicionInicial = CantidadPagEspecificacionesTecnicas
                        Exit Select
                    Case CodigoSeccion.TerminosCondiciones
                        listaImprimir(ind).PosicionInicial = CantidadPagTerminosCondiciones
                        Exit Select
                    Case CodigoSeccion.PresentacionFSA
                        listaImprimir(ind).PosicionInicial = CantidadPagPresentacionFSA
                        Exit Select
                    Case CodigoSeccion.PresentacionMercado
                        listaImprimir(ind).PosicionInicial = CantidadPagPresentacionMercado
                        Exit Select
                    Case CodigoSeccion.RequisitosAprobacionFormalizacionCredito
                        listaImprimir(ind).PosicionInicial = CantidadPagRequisitosAprobacionCredito
                        Exit Select
                    Case CodigoSeccion.FormatoUCMIAnual
                        listaImprimir(ind).PosicionInicial = CantidadPagRequisitosFormalizacionCredito
                        Exit Select
                    Case CodigoSeccion.FormatoUCMIEvento
                        listaImprimir(ind).PosicionInicial = CantidadPagFormatoUCMI
                        Exit Select
                    Case Else
                End Select
                ind += 1
            Next
            Dim wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
            Using wordDoc
                Dim tablaWord As Table = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(2)
                Dim CantRegTablaWord As Integer = 0
                Dim cantColum As Integer = 0
                Dim indFilaTablaWord As Integer = 0
                Dim numeroPag As Integer = 1

                CantRegTablaWord = tablaWord.Elements(Of TableRow).Count()

                If CantRegTablaWord > 0 Then
                    For Each ebeTablaMaestra As beTablaMaestra In listaImprimir

                        '===Generar Numero Pagina===
                        If indFilaTablaWord = 0 Then
                            numeroPag = 1
                        Else
                            numeroPag += listaImprimir.Item(indFilaTablaWord - 1).PosicionInicial
                        End If
                        '==========================
                        If indFilaTablaWord < CantRegTablaWord Then 'Escribe en las celdas de la fila
                            Dim fila As TableRow = tablaWord.Elements(Of TableRow)().ElementAt(indFilaTablaWord)
                            cantColum = fila.Elements(Of TableCell).Count()

                            For j As Integer = 0 To cantColum - 1
                                Dim celda As TableCell = fila.Elements(Of TableCell)().ElementAt(j)
                                Dim para As Paragraph = celda.Elements(Of Paragraph)().First()
                                Dim run As Run = para.AppendChild(New Run)

                                Dim TextoCelda As String = String.Empty

                                Select Case j
                                    Case 0
                                        TextoCelda = ebeTablaMaestra.Nombre
                                        Exit Select
                                    Case 1
                                        TextoCelda = numeroPag
                                        Exit Select
                                    Case Else
                                        TextoCelda = String.Empty
                                End Select

                                run.AppendChild(New Text(TextoCelda))
                                Dim rPr As RunProperties = New RunProperties(RunPropertiesConfigurado())
                                run.PrependChild(Of RunProperties)(rPr)

                            Next
                        Else ' Cuando ya no hay mas filas donde escribir se crean nuevas filas                               

                            Dim trFila As New TableRow()
                            Dim tcNombre As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado()), New Text(ebeTablaMaestra.Nombre))))
                            Dim tcNroPagina As TableCell = New TableCell(New Paragraph(New Run(New RunProperties(RunPropertiesConfigurado()), New Text(numeroPag))))

                            trFila.Append(tcNombre, tcNroPagina)
                            tablaWord.AppendChild(trFila)
                        End If
                        'Incrementa el indice de recorido
                        indFilaTablaWord += 1
                    Next
                End If
                wordDoc.MainDocumentPart.Document.Save()
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Private Function ListaSinRepeticion(ByVal listaTablaMaestra As List(Of beTablaMaestra)) As List(Of beTablaMaestra)
        Dim ListaResult As New List(Of beTablaMaestra)
        Dim uniqueStore As New Dictionary(Of Integer, Integer)()
        For Each ebetablaMaestra As beTablaMaestra In listaTablaMaestra
            If Not uniqueStore.ContainsKey(ebetablaMaestra.Codigo) Then
                uniqueStore.Add(ebetablaMaestra.Codigo, 0)
                ListaResult.Add(ebetablaMaestra)
            End If
        Next
        Return ListaResult
    End Function


    Private Function ConvertirATipoCambio(ByVal valMonto As Decimal, ByVal valTipoCambio As Decimal)
        Dim total As Decimal = 0
        Try
            total = valMonto * valTipoCambio
        Catch ex As Exception
            total = 0
        End Try
        Return total
    End Function

    Private Function FormatoMoneda(ByVal valMonto As Decimal) As String
        Dim strResultado As String = String.Empty

        Try
            strResultado = String.Format("{0:#,##0.00}", valMonto)
        Catch ex As Exception
            strResultado = String.Empty
        End Try
        If strResultado = "0" Then strResultado = String.Empty

        Return strResultado
    End Function

    Private Function ObtenerSoloNombDocumento(ByVal NombDocExtension As String)
        Dim strNombre As String = String.Empty

        If Not String.IsNullOrEmpty(NombDocExtension) Then
            Dim Indi As Integer = 0
            Indi = NombDocExtension.LastIndexOf(".")
            If Indi > 0 Then
                strNombre = NombDocExtension.Substring(0, Indi)
            End If
        End If

        Return strNombre
    End Function


    Private Function RunPropertiesConfigurado(Optional ByVal isBold As Boolean = False, Optional ByVal isUnderline As Boolean = False, Optional ByVal tipoLetra As String = "Arial", Optional ByVal fontSize As String = "24", Optional ByVal fontColor As String = "000000", Optional ByVal Alineacion As String = "Izquierda") As RunProperties
        Dim rnProperties As RunProperties = Nothing
        If isBold And isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold(), New Underline() With {.Val = UnderlineValues.Single}, New Color() With {.Val = fontColor})
        ElseIf isBold And Not isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold(), New Color() With {.Val = fontColor})
        ElseIf Not isBold And isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Underline() With {.Val = UnderlineValues.Single}, New Color() With {.Val = fontColor})
        Else
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold() With {.Val = False}, New Underline() With {.Val = UnderlineValues.None}, New Color() With {.Val = fontColor})
        End If

        Dim Alinear As Justification = Nothing
        Select Case Alineacion.ToUpper
            Case "IZQUIERDA"
                Alinear = New Justification() With {.Val = JustificationValues.Left}
                Exit Select
            Case "DERECHA"
                Alinear = New Justification() With {.Val = JustificationValues.Right}
                Exit Select
            Case "CENTRO"
                Alinear = New Justification() With {.Val = JustificationValues.Center}
                Exit Select
            Case Else
                Alinear = New Justification() With {.Val = JustificationValues.Left}
        End Select

        rnProperties.AppendChild(Alinear)
        Return rnProperties
    End Function

    Private Function RunConfigurado(Optional ByVal isBold As Boolean = False, Optional ByVal isUnderline As Boolean = False, Optional ByVal tipoLetra As String = "Arial", Optional ByVal fontSize As String = "24", Optional ByVal fontColor As String = "000000", Optional ByVal Alineacion As String = "Izquierda") As Run
        Dim runReturn As New Run

        Dim rnProperties As RunProperties = Nothing
        If isBold And isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold(), New Underline() With {.Val = UnderlineValues.Single}, New Color() With {.Val = fontColor})
        ElseIf isBold And Not isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold(), New Color() With {.Val = fontColor})
        ElseIf Not isBold And isUnderline Then
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Underline() With {.Val = UnderlineValues.Single}, New Color() With {.Val = fontColor})
        Else
            rnProperties = New RunProperties(New RunFonts() With {.Ascii = tipoLetra, .HighAnsi = tipoLetra, .ComplexScript = tipoLetra}, New FontSize() With {.Val = fontSize}, New Bold() With {.Val = False}, New Underline() With {.Val = UnderlineValues.None}, New Color() With {.Val = fontColor})
        End If

        Dim Alinear As Justification = Nothing
        Select Case Alineacion.ToUpper
            Case "IZQUIERDA"
                Alinear = New Justification() With {.Val = JustificationValues.Left}
                Exit Select
            Case "DERECHA"
                Alinear = New Justification() With {.Val = JustificationValues.Right}
                Exit Select
            Case "CENTRO"
                Alinear = New Justification() With {.Val = JustificationValues.Center}
                Exit Select
            Case Else
                Alinear = New Justification() With {.Val = JustificationValues.Left}
        End Select
        rnProperties.AppendChild(Alinear)

        runReturn.Append(rnProperties)
        Return runReturn
    End Function

    Private Sub EscribirLog(ByVal mensaje As StringBuilder)

        ' Log de tareas
        Dim file As FileInfo
        Dim log As System.IO.StreamWriter
        Dim NombUrlLog As String
        Dim Fecha As String
        Fecha = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString
        NombUrlLog = Hosting.HostingEnvironment.ApplicationPhysicalPath & "\ArchivosCotizador\" & "logImpresion_" & Fecha & ".txt"

        Try
            ' RUTA PRINCIPAL
            file = New FileInfo(NombUrlLog)

            If Not file.Exists Then
                ' CREAR SI NO EXISTE
                log = New System.IO.StreamWriter(NombUrlLog)
            Else
                ' YA EXISTE
                log = New System.IO.StreamWriter(NombUrlLog, True)
            End If

            log.WriteLine(mensaje.ToString)
            log.Close()
            log.Dispose()
            GC.SuppressFinalize(log)
        Catch ex As Exception
            'Notificar el error por email
        End Try

    End Sub

    Private Function ListaTablaHomologacion(ByVal obeHomologacion As beHomologacion) As List(Of beHomologacion)
        Dim listHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion
        obcHomologacion.BuscarTabla(Modulo.strConexionSql, obeHomologacion, listHomologacion)
        Return listHomologacion
    End Function

    Private Function BuscarDatoHomologacion(ByVal listaHomologacion As List(Of beHomologacion), ByVal DatoBuscar As String) As beHomologacion
        Dim obeHomologacion As beHomologacion

        Dim lista As New List(Of beHomologacion)
        lista = listaHomologacion.Where(Function(a) a.ValorSap = DatoBuscar.ToString).ToList()
        If Not lista Is Nothing Then
            If lista.Count > 0 Then
                obeHomologacion = lista.FirstOrDefault
            End If
        End If

        Return obeHomologacion
    End Function


    Private Sub EscribirMarcadorDocumento(ByRef wordDoc As WordprocessingDocument, ByVal listaMarcadores As List(Of EstructuraDatos))

        If wordDoc Is Nothing Then Exit Sub
        Try
            Dim listabookmarks = wordDoc.MainDocumentPart.Document.Descendants(Of BookmarkStart).ToList()

            For Each marcador As BookmarkStart In listabookmarks

                Dim oEstructuraDatos As New EstructuraDatos
                oEstructuraDatos = listaMarcadores.Where(Function(c) c.valor1.ToUpper = marcador.Name.ToString.ToUpper).FirstOrDefault

                If Not oEstructuraDatos Is Nothing Then
                    Dim padre = marcador.Parent ' puede ser paragrah, tableRow

                    If Not padre Is Nothing Then
                        Select Case padre.GetType
                            Case GetType(Paragraph)
                                Dim runEscribibr As New Run(New Text(oEstructuraDatos.valor2))
                                padre.InsertAfter(runEscribibr, marcador)
                                Exit Select
                            Case GetType(TableRow)
                                Exit Select
                        End Select
                    End If
                End If

            Next
            wordDoc.MainDocumentPart.Document.Save()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ValidarSaltoPagina(ByRef mainStream As MemoryStream)

        Using mainDocument As WordprocessingDocument = WordprocessingDocument.Open(mainStream, True)

            Dim listaBreak As New List(Of Break)
            listaBreak = mainDocument.MainDocumentPart.Document.Body.Elements(Of Break)()
            'listaBreak = mainDocument.MainDocumentPart.Document.Body.Elements(Of Paragraph)().ToList(Of Run)()
            'mainDocument.MainDocumentPart.Document.Body.Append(New Paragraph(New Run(New Break() With {.Type = BreakValues.Page})))
            For Each ibreak As Break In listaBreak
                ibreak.RemoveAllChildren()
                ibreak.Remove()
            Next
            mainDocument.MainDocumentPart.Document.Save()

        End Using

    End Sub


#End Region

#Region "Funciones para Excel"
    Public Function LeerExcel(ByRef objMemoryStream As MemoryStream) As DataTable

        Dim dtb As New DataTable
        objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoTemporal, "Excel-Tarifa.xlsx")
        Dim documentExcel As SpreadsheetDocument = Nothing
        Dim wbPart As WorkbookPart = Nothing

        documentExcel = SpreadsheetDocument.Open(objMemoryStream, True)
        wbPart = documentExcel.WorkbookPart

        Dim sheet As Spreadsheet.Sheet = wbPart.Workbook.Descendants(Of Spreadsheet.Sheet)().FirstOrDefault

        Dim ws As Spreadsheet.Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
        Dim sheetData As Spreadsheet.SheetData = ws.GetFirstChild(Of Spreadsheet.SheetData)()

        Dim dataRows = From reg In ws.Descendants(Of Spreadsheet.Row)()
        Dim listaTarifa As New List(Of beTarifa)
        For Each Reg As Spreadsheet.Row In dataRows
            Dim valCelda As String
            Dim refCell As Spreadsheet.Cell = Nothing
            Dim ebeTarifa As New beTarifa

            For Each cell As Spreadsheet.Cell In Reg.Elements(Of Spreadsheet.Cell)()
                valCelda = ""
                Try
                    If CType(Reg.RowIndex.ToString, Integer) = 422 Then
                        If cell.CellReference.ToString = "L422" Then
                            Dim a As String = "jjjjj"
                        End If
                    End If
                    Dim cellValue As Spreadsheet.SharedStringItem = GetSharedStringItemById(documentExcel.WorkbookPart, cell.CellValue.Text)
                    valCelda = cellValue.InnerText
                Catch ex As Exception
                    valCelda = "Sin Datos"
                End Try
                Select Case cell.CellReference.Value
                    Case String.Concat("A", Reg.RowIndex)
                        ebeTarifa.conFluidos = valCelda
                        ebeTarifa.aceites = valCelda
                        Exit Select
                    Case String.Concat("B", Reg.RowIndex)
                        ebeTarifa.plan = valCelda
                        Exit Select
                    Case String.Concat("C", Reg.RowIndex)
                        'ebeTarifa.= valCelda
                        Exit Select
                    Case String.Concat("D", Reg.RowIndex)
                        ebeTarifa.familia = valCelda
                        Exit Select
                    Case String.Concat("E", Reg.RowIndex)
                        ebeTarifa.modelo = valCelda
                        Exit Select
                    Case String.Concat("F", Reg.RowIndex)
                        ebeTarifa.modeloBase = valCelda
                        Exit Select
                    Case String.Concat("G", Reg.RowIndex)
                        ebeTarifa.prefijo = valCelda
                        Exit Select
                    Case String.Concat("H", Reg.RowIndex)
                        'ebeTarifa. = valCelda
                        Exit Select
                    Case String.Concat("I", Reg.RowIndex)
                        'ebeTarifa.ser = valCelda
                        Exit Select
                    Case String.Concat("J", Reg.RowIndex)
                        ebeTarifa.kitRepuestos = valCelda
                        Exit Select
                    Case String.Concat("K", Reg.RowIndex)
                        ebeTarifa.fluidos = valCelda
                        Exit Select
                    Case String.Concat("L", Reg.RowIndex)
                        ebeTarifa.servicioContratado = valCelda
                        Exit Select
                    Case String.Concat("M", Reg.RowIndex)
                        ebeTarifa.SOS = valCelda
                        Exit Select
                    Case String.Concat("N", Reg.RowIndex)
                        ebeTarifa.total = valCelda
                        Exit Select
                    Case String.Concat("O", Reg.RowIndex)
                        ebeTarifa.eventosNueva = valCelda
                        Exit Select
                    Case String.Concat("P", Reg.RowIndex)
                        ebeTarifa.eventosUsada = valCelda
                        Exit Select
                    Case String.Concat("Q", Reg.RowIndex)
                        ebeTarifa.kitRepuestosT = valCelda
                        Exit Select
                    Case String.Concat("R", Reg.RowIndex)
                        ebeTarifa.fluidosT = valCelda
                        Exit Select
                    Case String.Concat("S", Reg.RowIndex)
                        ebeTarifa.servicioContratadoT = valCelda
                        Exit Select
                    Case String.Concat("T", Reg.RowIndex)
                        ebeTarifa.totalT = valCelda
                        Exit Select
                    Case String.Concat("U", Reg.RowIndex)
                        ebeTarifa.tarifaUSDxH = valCelda
                        Exit Select
                End Select
                'If cell.CellReference.Value = DirCelda Then refCell = cell
            Next
            listaTarifa.Add(ebeTarifa)
        Next

        Return dtb
    End Function
    Private Function GetSharedStringItemById(ByVal workbookPart As WorkbookPart, ByVal id As Integer) As Spreadsheet.SharedStringItem
        Dim cantidad As String = String.Empty
        cantidad = workbookPart.SharedStringTablePart.SharedStringTable.Elements(Of Spreadsheet.SharedStringItem)().Count
        Dim lista = workbookPart.SharedStringTablePart.SharedStringTable.Elements(Of Spreadsheet.SharedStringItem)()

        Return workbookPart.SharedStringTablePart.SharedStringTable.Elements(Of Spreadsheet.SharedStringItem).ElementAt(id)
    End Function
    Private Function GetRow(ByVal wsData As Spreadsheet.SheetData, ByVal rowIndex As UInt32) As Spreadsheet.Row
        Dim row = wsData.Elements(Of Spreadsheet.Row)().Where(Function(r) r.RowIndex.Value = rowIndex).FirstOrDefault()
        If row Is Nothing Then
            row = New Spreadsheet.Row()
            row.RowIndex = rowIndex
            wsData.Append(row)
        End If
        Return row
    End Function
#End Region

End Class
