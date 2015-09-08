Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports System.IO
Imports DocumentFormat.OpenXml.Packaging
Imports Ferreyros.Utiles.Estructuras
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Wordprocessing 



Public Class ImpresionExtender
    Private objAdminFTP As New AdminFTP
    Private RunPropCeldaTablaCalculos18 As RunProperties = RunPropertiesConfigurado(, , "Tahoma", "18") '16
    Public Function CotizarAlquilerPrevia(ByVal eCotizacion As beCotizacion, ByVal dtsDatosDocumento As DataSet) As List(Of Byte())

        Dim obcProductoAlquilerTarifa As New bcProductoAlquilerTarifa
        Dim obcProductoCaracteristica As New bcProductoCaracteristica
        Dim obeProductoAlquilerTarifa As New beProductoAlquilerTarifa
        Dim obeProductoCaracteristica As New beProductoCaracteristica
        Dim ListaProductoAlquilerTarifa As New List(Of beProductoAlquilerTarifa)
        Dim ListaProductoCaracteristica As New List(Of beProductoCaracteristica)

        Dim dtbDatosPivot As New DataTable
        Dim ListaByteDocumento As New List(Of Byte())

        Dim listaHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion

        Dim NombreArchivo As String = String.Empty '"Otros/CotizacionAlquiler/PlantillaAlquilerPrevia.docx"
        Dim objMemoryStream As MemoryStream = Nothing
        Dim objMemoryStreamAdicional As MemoryStream = Nothing

        obeHomologacion.Tabla = TablaHomologacion.DIR_COTIZACION_ALQUILER_PREVIA  ' tabla
        obeHomologacion.ValorSap = eCotizacion.IdCompania
        listaHomologacion.Clear()
        listaHomologacion = ListaTablaHomologacionSC(obeHomologacion)

        If listaHomologacion.Count > 0 Then
            obeHomologacion = listaHomologacion.ToList().FirstOrDefault
            NombreArchivo = obeHomologacion.ValorCotizador
        Else
            NombreArchivo = String.Empty
        End If

        If Not String.IsNullOrEmpty(NombreArchivo) Then
            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
            dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.CartaPresentacion)
        End If

        '1.- Escribir en los marcadores
        If dtbDatosPivot.Rows.Count > 0 Then

            Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStream, IO.FileMode.Open, IO.FileAccess.ReadWrite)

                Dim xmlDocumento As New System.Xml.XmlDocument
                Dim xpnDocumento As System.Xml.XPath.XPathNavigator
                Dim xnmDocumento As System.Xml.XmlNamespaceManager
                Dim msPackage As IO.Packaging.PackagePart
                Dim uriPartTarget As Uri
                Dim strCampo As String
                Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

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
                                        Case UCase("Lugar")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Lugar").ToString)
                                            Exit Select
                                        Case UCase("Fecha")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Fecha").ToString)
                                            Exit Select
                                        Case UCase("Cliente")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Cliente").ToString)
                                            Exit Select
                                        Case UCase("Contacto")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Contacto").ToString)
                                        Case UCase("NomVendedor")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("NomVendedor").ToString)
                                            Exit Select
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
        End If


        dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.DetalleProducto)
        Dim indFilaTablaWord As Integer = 0
        For Each drProducto In dtbDatosPivot.Rows

            obeProductoAlquilerTarifa.IdProducto = drProducto("IdProducto").ToString
            obeProductoCaracteristica.IdProducto = drProducto("IdProducto").ToString

            ListaProductoCaracteristica.Clear()
            ListaProductoAlquilerTarifa.Clear()

            'La primera caracteristica siempre es el Modelo del producto
            ListaProductoCaracteristica.Add(New beProductoCaracteristica With {.DescripcionAtributo = "Descripción", .ValorAtributo = drProducto("DescripcionFamilia").ToString})

            ListaProductoCaracteristica.Add(New beProductoCaracteristica With {.DescripcionAtributo = "Marca", .ValorAtributo = drProducto("Marca").ToString})
            ListaProductoCaracteristica.Add(New beProductoCaracteristica With {.DescripcionAtributo = "Modelo", .ValorAtributo = drProducto("Modelo").ToString})

            obcProductoCaracteristica.BuscarIdProducto(Modulo.strConexionSql, obeProductoCaracteristica, ListaProductoCaracteristica)
            obcProductoAlquilerTarifa.BuscarIdProducto(Modulo.strConexionSql, obeProductoAlquilerTarifa, ListaProductoAlquilerTarifa)

            '2.- Escribir en la tabla
            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)

                Dim tablaPropuesta As Table
                If indFilaTablaWord = 0 Then
                    tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)
                Else
                    tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0).Clone()
                End If

                'Solo dejarlo con 1 registro
                Dim posicionFilaPropuesta As Integer = 0
                For Each fila In tablaPropuesta.Elements(Of TableRow)().ToList
                    If posicionFilaPropuesta <> 0 Then
                        tablaPropuesta.RemoveChild(fila)
                    End If
                    posicionFilaPropuesta += 1
                Next

                Dim intPosicionFila = 0

                Dim nuevaListaProductoCaracteristica As New List(Of beProductoCaracteristica)
                For Each oProductoCaracteristica In ListaProductoCaracteristica
                    Dim nuevoValorAtributo As New StringBuilder

                    If oProductoCaracteristica.CodigoUnidadMedida.Trim <> "" Then
                        nuevoValorAtributo.Append(oProductoCaracteristica.ValorAtributo)
                        nuevoValorAtributo.Append(" ")
                        nuevoValorAtributo.Append(oProductoCaracteristica.CodigoUnidadMedida)
                        oProductoCaracteristica.ValorAtributo = nuevoValorAtributo.ToString
                    End If
                    nuevaListaProductoCaracteristica.Add(oProductoCaracteristica)
                Next

                ListaProductoCaracteristica.Clear()
                ListaProductoCaracteristica = nuevaListaProductoCaracteristica

                For Each oProductoAlquilerTarifa In ListaProductoAlquilerTarifa
                    Dim textoDescripcionTarifa As New StringBuilder
                    Dim textoValorTarifa As New StringBuilder

                    textoDescripcionTarifa.Append("Tarifas ")
                    textoDescripcionTarifa.Append(oProductoAlquilerTarifa.ValorEscala)
                    textoDescripcionTarifa.Append(" ")
                    textoDescripcionTarifa.Append(oProductoAlquilerTarifa.UnidadMedidaPrecio)
                    textoDescripcionTarifa.Append(" mínimas")

                    textoValorTarifa.Append(oProductoAlquilerTarifa.Importe)
                    textoValorTarifa.Append(" ")
                    textoValorTarifa.Append(oProductoAlquilerTarifa.Moneda)
                    textoValorTarifa.Append("/")
                    textoValorTarifa.Append(oProductoAlquilerTarifa.CodigoUnidadMedida)

                    ListaProductoCaracteristica.Add(New beProductoCaracteristica With {.DescripcionAtributo = textoDescripcionTarifa.ToString, .ValorAtributo = textoValorTarifa.ToString})
                Next

                For Each oProductoCaracteristica In ListaProductoCaracteristica
                    Dim trTemp As TableRow
                    Dim textoColumna0 As String = String.Empty
                    Dim textoColumna1 As String = String.Empty

                    If intPosicionFila = 0 Then
                        trTemp = tablaPropuesta.Elements(Of TableRow)().ElementAt(0)
                    Else
                        trTemp = tablaPropuesta.Elements(Of TableRow)().ElementAt(0).Clone()
                    End If

                    'Columna 0
                    Dim tccelda_Fila0 As TableCell = trTemp.Elements(Of TableCell)().ElementAt(0)
                    Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                    If parag_Fila0 Is Nothing Then
                        parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                    End If

                    Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                    If run_Fila0 Is Nothing Then
                        run_Fila0 = parag_Fila0.AppendChild(New Run)
                    End If
                    'run_Fila0.RemoveAllChildren()

                    If intPosicionFila <> 0 Then
                        Dim textoCelda As Text = run_Fila0.Elements(Of Text).FirstOrDefault
                        If textoCelda Is Nothing Then
                            textoCelda = run_Fila0.AppendChild(New Text)
                        End If
                        textoCelda.Text = oProductoCaracteristica.DescripcionAtributo + " :"
                        'run1_Fila.AppendChild(New Text(oProductoCaracteristica.ValorAtributo))
                    End If

                    'Columna 1
                    Dim tccelda1_Fila As TableCell = trTemp.Elements(Of TableCell)().ElementAt(1)
                    Dim parag1_Fila As Paragraph = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                    If parag1_Fila Is Nothing Then
                        parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                    End If

                    Dim run1_Fila As Run = parag1_Fila.Elements(Of Run).FirstOrDefault

                    If run1_Fila Is Nothing Then
                        run1_Fila = parag1_Fila.AppendChild(New Run)
                    End If

                    'Escribir en la tabla
                    'run1_Fila.RemoveAllChildren()

                    Dim textoCelda1 As Text = run1_Fila.Elements(Of Text).FirstOrDefault
                    If textoCelda1 Is Nothing Then
                        textoCelda1 = run1_Fila.AppendChild(New Text)
                    End If
                    textoCelda1.Text = oProductoCaracteristica.ValorAtributo

                    run1_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())

                    If intPosicionFila > 0 Then
                        'tablaPropuesta.InsertAt(trTemp, intPosicionFila + 1)
                        tablaPropuesta.InsertAfter(trTemp, tablaPropuesta.Elements(Of TableRow)().Last)
                    End If
                    intPosicionFila = intPosicionFila + 1
                Next
                If indFilaTablaWord > 0 Then
                    wordDoc.MainDocumentPart.Document.Body.Append(tablaPropuesta)
                End If
                Dim paragSaltoLinea As New Paragraph(New Run(New Break()))
                wordDoc.MainDocumentPart.Document.Body.Append(paragSaltoLinea)
                wordDoc.MainDocumentPart.Document.Save()
            End Using

            indFilaTablaWord = indFilaTablaWord + 1
        Next

        If objMemoryStream.Length > 0 Then
            ListaByteDocumento.Add(objMemoryStream.ToArray())
        End If

        '3.- Agregar Informacion adicional

        obeHomologacion.Tabla = TablaHomologacion.DIR_COTIZACION_ALQUILER_PREVIA_INFOADIC  ' tabla
        obeHomologacion.ValorSap = eCotizacion.IdCompania
        listaHomologacion.Clear()
        listaHomologacion = ListaTablaHomologacionSC(obeHomologacion)

        If listaHomologacion.Count > 0 Then
            obeHomologacion = listaHomologacion.ToList().FirstOrDefault
            NombreArchivo = obeHomologacion.ValorCotizador
        Else
            NombreArchivo = String.Empty
        End If

        objMemoryStreamAdicional = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)

        If objMemoryStreamAdicional.Length > 0 Then

            'Datos del vendedor
            dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.CartaPresentacion)

            'Llenamos los marcadores
            Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStreamAdicional, IO.FileMode.Open, IO.FileAccess.ReadWrite)

                Dim xmlDocumento As New System.Xml.XmlDocument
                Dim xpnDocumento As System.Xml.XPath.XPathNavigator
                Dim xnmDocumento As System.Xml.XmlNamespaceManager
                Dim msPackage As IO.Packaging.PackagePart
                Dim uriPartTarget As Uri
                Dim strCampo As String
                Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

                'Recupera el xml del contenido del documento
                uriPartTarget = New Uri("/word/document.xml", UriKind.Relative)
                msPackage = package.GetPart(uriPartTarget)
                xmlDocumento.Load(msPackage.GetStream)

                'Se crea el navegador
                xpnDocumento = xmlDocumento.CreateNavigator()
                xnmDocumento = New System.Xml.XmlNamespaceManager(xpnDocumento.NameTable)
                xnmDocumento.AddNamespace("w", strUri)
                Dim eTelefonoResponsable As New beTelefonoResponsable
                Dim listaTelefonoResponsable As New List(Of beTelefonoResponsable)
                Dim cTelefonoResponsable As New bcTelefonoResponsable
                Dim xListaTelefonoResponsable As New List(Of beTelefonoResponsable)
                Dim sw As Boolean = False
                For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                    strCampo = String.Empty
                    If xpnCampo.MoveToChild("name", strUri) Then
                        If xpnCampo.MoveToAttribute("val", strUri) Then
                            strCampo = xpnCampo.Value
                        End If

                        'MARCADORES DINAMICOS                        
                        If strCampo.ToUpper = "TELEFONO" Then
                            eTelefonoResponsable.IdCotizacion = dtbDatosPivot.Rows(0).Item("IdCotizacion")
                            sw = cTelefonoResponsable.TelefonoResponsableListar(Modulo.strConexionSql, eTelefonoResponsable, listaTelefonoResponsable)
                            If listaTelefonoResponsable.Count > 0 Then
                                Dim eHomologacion As New beHomologacion
                                Dim l_Homologacion As New List(Of beHomologacion)
                                Dim obcHomologacion As New bcHomologacion
                                Dim valorHomologacion As String = ""
                                Dim arrayValor As String()


                                For Each elemento As beTelefonoResponsable In listaTelefonoResponsable
                                    Dim xTelefonoResponsable As New beTelefonoResponsable
                                    eHomologacion.Tabla = TablaHomologacion.COD_TIPO_TELEFONO_RESPONSABLE  ' tabla
                                    eHomologacion.ValorSap = elemento.CodTipoTelefono
                                    l_Homologacion.Clear()
                                    l_Homologacion = ListaTablaHomologacionSC(eHomologacion)

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
                            End If

                        Else

                            'Move to w:instrText
                            If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                'Check FORMTEXT
                                If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                    'Move to t
                                    If xpnCampo.MoveToFollowing("t", strUri) Then
                                        'Set value
                                        Select Case strCampo.ToUpper
                                            Case UCase("Cargo")
                                                xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Cargo").ToString)
                                                Exit Select
                                            Case UCase("Email")
                                                xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Email").ToString)
                                                Exit Select
                                            Case UCase("Telefono")
                                                xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Telefono").ToString)
                                                Exit Select
                                            Case UCase("Vendedor")
                                                xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("NomVendedor").ToString)
                                                Exit Select
                                        End Select
                                    End If
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
            ListaByteDocumento.Add(objMemoryStreamAdicional.ToArray())
        End If

        Return ListaByteDocumento

    End Function

    Public Function CotizarAlquilerLeasing(ByVal eCotizacion As beCotizacion, ByVal dtsDatosDocumento As DataSet) As List(Of Byte())

        Dim obcProductoCaracteristica As New bcProductoCaracteristica
        Dim obeProductoCaracteristica As New beProductoCaracteristica
        Dim ListaProductoCaracteristica As New List(Of beProductoCaracteristica)

        Dim listaHomologacion As New List(Of beHomologacion)
        Dim obeHomologacion As New beHomologacion

        Dim dtbDatosPivot As New DataTable
        Dim ListaByteDocumento As New List(Of Byte())
        Dim textoDatoPivot As String = String.Empty
        Dim textoListaProducto As String = String.Empty

        Dim NombreArchivo As String = String.Empty
        Dim CodigoProductoCaracteristicaPeso As String = String.Empty
        Dim objMemoryStream As MemoryStream = Nothing
        Dim objMemoryStreamAdicional As MemoryStream = Nothing

        'Nombre de marcador
        Dim NombMarcadorMarca As String = String.Empty
        Dim NombMarcadorTiempoContrato As String = String.Empty
        Dim NombMarcadorDescripListaProducto As String = String.Empty

        Dim valorTiempoContrato As String = String.Empty
        Dim valorSumaCantidadUnidadesProducto As Integer = 0
        'Asignacion de valores a variables de marcadores ---------------------

        dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.ProductoAlquiler)
        For Each drDatoPivot In dtbDatosPivot.Rows
            NombMarcadorMarca = drDatoPivot("Marca").ToString
            valorSumaCantidadUnidadesProducto = valorSumaCantidadUnidadesProducto + drDatoPivot("Cantidad").ToString
            'If drDatoPivot("CodigoTipoArrendamiento").ToString = "02" Then
            Dim meses() As String = drDatoPivot("LeasingMeses").ToString.Split(" ")
            NombMarcadorTiempoContrato = meses(0) + " MESES"
            valorTiempoContrato = meses(0)
            'Else
            'NombMarcadorTiempoContrato = drDatoPivot("DesMesAlquilar").ToString + " MESES"
            'valorTiempoContrato = drDatoPivot("DesMesAlquilar").ToString
            'End If

        Next
        '---------------------------------------------------------------------
        'Nombre de Plantilla de cotizacion leasing
        obeHomologacion.Tabla = TablaHomologacion.DIR_COTIZACION_ALQUILER_LEASING  ' tabla
        listaHomologacion.Clear()
        listaHomologacion = ListaTablaHomologacion(obeHomologacion)
        obeHomologacion = listaHomologacion.ToList().FirstOrDefault
        If Not obeHomologacion Is Nothing Then
            NombreArchivo = obeHomologacion.ValorSap
        End If

        'Codigo de caracteristica peso de producto
        obeHomologacion.Tabla = TablaHomologacion.COD_PRODUCTO_CARACTERISTICA_PESO   ' tabla
        listaHomologacion.Clear()
        listaHomologacion = ListaTablaHomologacion(obeHomologacion)
        obeHomologacion = listaHomologacion.ToList().FirstOrDefault
        If Not obeHomologacion Is Nothing Then
            CodigoProductoCaracteristicaPeso = obeHomologacion.ValorSap
        End If


        If Not String.IsNullOrEmpty(NombreArchivo) Then
            objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoAnexos, NombreArchivo)
            'dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.CartaPresentacion)
        End If


        '1.- Escribir en las tablas
        dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.DetalleProducto)
        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)

            Dim tablaEquipos As Table
            Dim tablaPropuestaEconomica As Table
            Dim tablaCronograma As Table
            Dim intPosicionFila As Integer = 0
            Dim CantidadTabla As Integer = 0
            '2.1 Tabla Cantidad por equipo(tabla Nr° 0)--------------------------------
            tablaEquipos = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(0)

            Dim CantidadFilas As Integer = dtbDatosPivot.Rows.Count
            Dim posFila As Integer = 0
            textoListaProducto = ""
            For Each drDatoPivot In dtbDatosPivot.Rows
                Dim trTemp As TableRow
                Dim textoColumna0 As String = String.Empty
                Dim textoColumna1 As String = String.Empty
                Dim textoCaracteristicaPeso As String = String.Empty

                obeProductoCaracteristica = New beProductoCaracteristica
                ListaProductoCaracteristica.Clear()

                obeProductoCaracteristica.IdProducto = drDatoPivot("IdProducto").ToString
                obcProductoCaracteristica.BuscarIdProducto(Modulo.strConexionSql, obeProductoCaracteristica, ListaProductoCaracteristica)

                obeProductoCaracteristica = ListaProductoCaracteristica.Where(Function(a) a.CodigoAtributo = CodigoProductoCaracteristicaPeso).FirstOrDefault
                If Not obeProductoCaracteristica Is Nothing Then
                    textoCaracteristicaPeso = "(" + obeProductoCaracteristica.ValorAtributo + " " + obeProductoCaracteristica.CodigoUnidadMedida + ")"
                End If
                If intPosicionFila = 0 Then
                    trTemp = tablaEquipos.Elements(Of TableRow)().ElementAt(0)
                Else
                    trTemp = tablaEquipos.Elements(Of TableRow)().ElementAt(0).Clone()
                End If

                'Iterar en la columnas
                For i = 0 To 1
                    'Columna 0
                    Dim tccelda_Fila0 As TableCell = trTemp.Elements(Of TableCell)().ElementAt(i)
                    Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                    If parag_Fila0 Is Nothing Then
                        parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                    End If

                    Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                    If run_Fila0 Is Nothing Then
                        run_Fila0 = parag_Fila0.AppendChild(New Run)
                    End If
                    Dim textoCelda As Text = run_Fila0.Elements(Of Text).FirstOrDefault
                    If textoCelda Is Nothing Then
                        textoCelda = run_Fila0.AppendChild(New Text)
                    End If

                    Select Case i
                        Case 0
                            textoCelda.Text = Chr(65 + intPosicionFila) + ")"
                            Exit Select
                        Case 1
                            textoDatoPivot = drDatoPivot("Cantidad").ToString
                            If Not String.IsNullOrEmpty(textoDatoPivot) Then
                                If IsNumeric(textoDatoPivot) Then
                                    If CDec(textoDatoPivot) < 10 Then
                                        textoDatoPivot = String.Concat("0", textoDatoPivot)
                                    End If
                                End If
                            End If
                            'textoDatoPivot = drDatoPivot("Cantidad").ToString + " "
                            textoDatoPivot = textoDatoPivot + " - "
                            textoDatoPivot = textoDatoPivot + drDatoPivot("DescripcionFamilia").ToString + " "
                            textoDatoPivot = textoDatoPivot + drDatoPivot("Marca").ToString + " "
                            textoDatoPivot = textoDatoPivot + "modelo" + " "
                            textoDatoPivot = textoDatoPivot + drDatoPivot("Modelo").ToString + " "
                            textoListaProducto = textoDatoPivot
                            textoDatoPivot = textoDatoPivot + textoCaracteristicaPeso
                            textoCelda.Text = textoDatoPivot

                            If posFila = CantidadFilas - 1 Then
                                If CantidadFilas > 1 Then
                                    NombMarcadorDescripListaProducto = String.Concat(NombMarcadorDescripListaProducto, " y ", textoListaProducto)
                                Else
                                    NombMarcadorDescripListaProducto = textoListaProducto
                                End If
                            Else
                                If posFila = 0 Then
                                    NombMarcadorDescripListaProducto = textoListaProducto
                                Else
                                    NombMarcadorDescripListaProducto = String.Concat(NombMarcadorDescripListaProducto, ", ", textoListaProducto)
                                End If

                            End If
                            Exit Select
                    End Select
                    run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())
                Next

                'Productos Accesorios para leasing
                Dim dtDatosAccesorios As DataTable = dtsDatosDocumento.Tables(Entidad.AccesorioProducto)
                Dim subpos As Integer = 1
                For Each drDatosAccesorios In dtDatosAccesorios.Rows
                    Dim trTemp1 As TableRow
                    trTemp1 = tablaEquipos.Elements(Of TableRow)().ElementAt(0).Clone()

                    'Iterar en la columnas
                    For i = 0 To 1
                        'Columna 0
                        Dim tccelda_Fila0 As TableCell = trTemp1.Elements(Of TableCell)().ElementAt(i)
                        Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                        If parag_Fila0 Is Nothing Then
                            parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                        End If

                        Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                        If run_Fila0 Is Nothing Then
                            run_Fila0 = parag_Fila0.AppendChild(New Run)
                        End If
                        Dim textoCelda As Text = run_Fila0.Elements(Of Text).FirstOrDefault
                        If textoCelda Is Nothing Then
                            textoCelda = run_Fila0.AppendChild(New Text)
                        End If

                        Select Case i
                            Case 0
                                textoCelda.Text = Chr(65 + intPosicionFila) + "." + subpos.ToString + ")"
                                Exit Select
                            Case 1
                                textoCelda.Text = drDatosAccesorios("Nombre").ToString
                                Exit Select
                        End Select
                        run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())
                    Next
                    subpos = subpos + 1
                    tablaEquipos.InsertAfter(trTemp1, tablaEquipos.Elements(Of TableRow)().Last)
                Next

                If intPosicionFila > 0 Then
                    tablaEquipos.InsertAfter(trTemp, tablaEquipos.Elements(Of TableRow)().Last)
                End If
                intPosicionFila = intPosicionFila + 1
                posFila += 1
            Next
            NombMarcadorDescripListaProducto = String.Concat("(", NombMarcadorDescripListaProducto, ")")
            '-----------------------------------------------------------------------------------

            '2.2 Tabla propuesta economica(tabla Nr° 1)--------------------------------

            textoDatoPivot = String.Empty

            dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.ProductoAlquiler)
            For Each drDatoPivot In dtbDatosPivot.Rows
                Dim trTemp As TableRow
                Dim textoColumna0 As String = String.Empty
                Dim textoColumna1 As String = String.Empty

                If CantidadTabla = 0 Then
                    tablaPropuestaEconomica = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1)
                Else
                    tablaPropuestaEconomica = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(1).Clone()
                End If

                textoDatoPivot = "3." + CStr(CantidadTabla + 1) + ".-TARIFA UNITARIA PARA "
                textoDatoPivot = textoDatoPivot + drDatoPivot("DescripcionFamilia").ToString.ToUpper + " "
                textoDatoPivot = textoDatoPivot + drDatoPivot("Marca").ToString.ToUpper + " "
                textoDatoPivot = textoDatoPivot + " MODELO "
                textoDatoPivot = textoDatoPivot + drDatoPivot("Modelo").ToString.ToUpper + " "

                'la tabla tiene solo 3 registros
                For i = 0 To 2
                    trTemp = tablaPropuestaEconomica.Elements(Of TableRow)().ElementAt(i)
                    'Columna 1
                    Dim tccelda1_Fila As TableCell = trTemp.Elements(Of TableCell)().ElementAt(1)
                    Dim parag1_Fila As Paragraph = tccelda1_Fila.Elements(Of Paragraph)().FirstOrDefault()

                    If parag1_Fila Is Nothing Then
                        parag1_Fila = tccelda1_Fila.AppendChild(New Paragraph)
                    End If

                    Dim run1_Fila As Run = parag1_Fila.Elements(Of Run).FirstOrDefault

                    If run1_Fila Is Nothing Then
                        run1_Fila = parag1_Fila.AppendChild(New Run)
                    End If

                    Dim textoCelda1 As Text = run1_Fila.Elements(Of Text).FirstOrDefault
                    If textoCelda1 Is Nothing Then
                        textoCelda1 = run1_Fila.AppendChild(New Text)
                    End If

                    Select Case i
                        Case 0
                            textoCelda1.Text = drDatoPivot("HorasUsoMensual").ToString
                            Exit Select
                        Case 1
                            Dim valorTarifaMensual As String = String.Empty
                            Try
                                'If drDatoPivot("CodigoTipoArrendamiento").ToString = "02" Then
                                valorTarifaMensual = "$ " + Modulo.FormatoMoneda(drDatoPivot("LeasingValorMensual").ToString)
                                'Else
                                'valorTarifaMensual = "$ " + Modulo.FormatoMoneda(drDatoPivot("ValorNeto").ToString / valorTiempoContrato)
                                'End If

                            Catch ex As Exception
                                valorTarifaMensual = String.Empty
                            End Try
                            textoCelda1.Text = valorTarifaMensual
                            Exit Select
                        Case 2
                            textoCelda1.Text = "$ " + Modulo.FormatoMoneda(drDatoPivot("ValorAdicionalHora").ToString)
                            Exit Select
                    End Select

                    run1_Fila.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())
                Next

                Dim paragNombTarifa As New Paragraph(New Run(New Text(textoDatoPivot)))
                Dim paragSaltoLinea As New Paragraph(New Run(New Break()))

                paragNombTarifa.Elements(Of Run).FirstOrDefault().PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())

                If CantidadTabla > 0 Then
                    wordDoc.MainDocumentPart.Document.Body.InsertAfter(tablaPropuestaEconomica, wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(CantidadTabla))
                End If

                wordDoc.MainDocumentPart.Document.Body.InsertBefore(paragSaltoLinea, tablaPropuestaEconomica)
                wordDoc.MainDocumentPart.Document.Body.InsertBefore(paragNombTarifa, tablaPropuestaEconomica)

                CantidadTabla = CantidadTabla + 1
                wordDoc.MainDocumentPart.Document.Save()
            Next

            '2.2 Tabla Cronograma(tabla Nr°  segun la cantidad de tablas propuesta economica)--------------------------------
            textoDatoPivot = String.Empty
            intPosicionFila = 0
            dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.ProductoAlquiler)
            'Mantener el valor CantidadTabla para saber la cantidad de tablas de tarifas que se genero
            tablaCronograma = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(CantidadTabla + 1)

            For Each drDatoPivot In dtbDatosPivot.Rows
                Dim trTemp As TableRow
                Dim textoColumna0 As String = String.Empty
                Dim textoColumna1 As String = String.Empty

                If intPosicionFila = 0 Then
                    trTemp = tablaCronograma.Elements(Of TableRow)().ElementAt(1)
                Else
                    trTemp = tablaCronograma.Elements(Of TableRow)().ElementAt(1).Clone()
                End If

                'Iterar en la columnas
                For i = 0 To 3
                    'Columna 0
                    Dim tccelda_Fila0 As TableCell = trTemp.Elements(Of TableCell)().ElementAt(i)
                    Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()

                    If parag_Fila0 Is Nothing Then
                        parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                    End If

                    Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault

                    If run_Fila0 Is Nothing Then
                        run_Fila0 = parag_Fila0.AppendChild(New Run)
                    End If
                    Dim textoCelda As Text = run_Fila0.Elements(Of Text).FirstOrDefault
                    If textoCelda Is Nothing Then
                        textoCelda = run_Fila0.AppendChild(New Text)
                    End If

                    Select Case i
                        Case 0
                            textoCelda.Text = intPosicionFila + 1
                            Exit Select
                        Case 1
                            textoCelda.Text = drDatoPivot("Cantidad").ToString
                            Exit Select
                        Case 2
                            textoCelda.Text = drDatoPivot("Modelo").ToString + " (" + drDatoPivot("DescripcionFamilia").ToString + ")"
                            Exit Select
                        Case 3
                            textoCelda.Text = drDatoPivot("PlazoEntrega").ToString
                            Exit Select
                    End Select
                    run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaCalculos18.Clone())
                Next

                If intPosicionFila > 0 Then
                    tablaCronograma.InsertAfter(trTemp, tablaCronograma.Elements(Of TableRow)().Last)
                End If
                intPosicionFila = intPosicionFila + 1
                wordDoc.MainDocumentPart.Document.Save()
            Next
            '-----------------------------------------------------------------------------------

        End Using

        dtbDatosPivot = dtsDatosDocumento.Tables(Entidad.CartaPresentacion)
        '2.- Escribir en los marcadores
        If dtbDatosPivot.Rows.Count > 0 Then

            Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStream, IO.FileMode.Open, IO.FileAccess.ReadWrite)

                Dim xmlDocumento As New System.Xml.XmlDocument
                Dim xpnDocumento As System.Xml.XPath.XPathNavigator
                Dim xnmDocumento As System.Xml.XmlNamespaceManager
                Dim msPackage As IO.Packaging.PackagePart
                Dim uriPartTarget As Uri
                Dim strCampo As String
                Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

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
                                        Case UCase("Lugar")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Lugar").ToString)
                                            Exit Select
                                        Case UCase("Fecha")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Fecha").ToString)
                                            Exit Select
                                        Case UCase("Cliente")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Cliente").ToString)
                                            Exit Select
                                        Case UCase("Contacto")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Contacto").ToString)
                                            Exit Select
                                        Case UCase("CargoContacto")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("CargoContacto").ToString)
                                            Exit Select
                                        Case UCase("ResponsableComercial")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("NomVendedor").ToString + " (" + dtbDatosPivot.Rows(0).Item("Telefono").ToString + ")")
                                            Exit Select
                                        Case UCase("NomVendedor")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("NomVendedor").ToString)
                                            Exit Select
                                        Case UCase("Cargo")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Cargo").ToString)
                                            Exit Select
                                        Case UCase("Email")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Email").ToString)
                                            Exit Select
                                        Case UCase("Telefono")
                                            xpnCampo.SetValue(dtbDatosPivot.Rows(0).Item("Telefono").ToString)
                                            Exit Select
                                        Case UCase("Asunto")
                                            xpnCampo.SetValue("PROPUESTA DE ALQUILER A " + valorTiempoContrato + " MESES")
                                            Exit Select
                                        Case UCase("Marca")
                                            xpnCampo.SetValue(NombMarcadorMarca)
                                            Exit Select
                                        Case UCase("TiempoContrato")
                                            xpnCampo.SetValue(NombMarcadorTiempoContrato)
                                            Exit Select
                                        Case UCase("CantidadUnidades")
                                            xpnCampo.SetValue(valorSumaCantidadUnidadesProducto)
                                            Exit Select
                                        Case UCase("CantidadCronograma")
                                            xpnCampo.SetValue(valorSumaCantidadUnidadesProducto)
                                            Exit Select
                                        Case UCase("DescripListaProducto")
                                            xpnCampo.SetValue(NombMarcadorDescripListaProducto)
                                            Exit Select
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
        End If

        If objMemoryStream.Length > 0 Then
            ListaByteDocumento.Add(objMemoryStream.ToArray())
        End If

        Return ListaByteDocumento

    End Function

#Region "Funciones y Metodos Generales"

    Private Function ListaTablaHomologacion(ByVal obeHomologacion As beHomologacion) As List(Of beHomologacion)
        Dim listHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion
        obcHomologacion.BuscarTabla(Modulo.strConexionSql, obeHomologacion, listHomologacion)
        Return listHomologacion
    End Function

    Public Function ListaTablaHomologacionSC(ByVal obeHomologacion As beHomologacion) As List(Of beHomologacion)
        Dim listHomologacion As New List(Of beHomologacion)
        Dim obcHomologacion As New bcHomologacion
        obcHomologacion.BuscarTablaSC(Modulo.strConexionSql, obeHomologacion, listHomologacion)
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

#End Region

    
End Class
