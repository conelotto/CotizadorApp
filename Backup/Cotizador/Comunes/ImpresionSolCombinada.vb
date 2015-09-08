Imports Ferreyros.BECotizador
Imports Ferreyros.BCCotizador
Imports System.IO
Imports DocumentFormat.OpenXml.Packaging
Imports Ferreyros.Utiles.Estructuras
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.Reflection

Public Class ImpresionSolCombinada

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

    Dim objAdminFTP As New AdminFTP
    Public Function GenerarEspecificacionSolCombinada(ByVal listaTablaMaestra As List(Of beTablaMaestra), ByVal dtbSeccionArchivo As DataTable, ByVal eCotizacion As beCotizacion, ByRef objMemoryStream As MemoryStream, ByVal idProductoX As String, ByRef ListaByteDocumento As List(Of Byte()))
        Dim ebeValidacion As New beValidacion
        Dim obcArchivoConfiguracion As New bcArchivoConfiguracion
        Dim obcCotizacion As New bcCotizacion
        Dim ListaDocImpreso As New List(Of beTablaMaestra)
        Dim dtsDatosDocumento As New DataSet
        obcCotizacion.DatosDocumento(Modulo.strConexionSql, eCotizacion, dtsDatosDocumento)

        Dim dtbProducto As New DataTable
        Dim obeTablaMaestra As New beTablaMaestra
        Dim dtbAdicionalProducto As New DataTable
        Dim dtbAccesorioProducto As New DataTable

        Dim obcSolComb As New bcProductoSolucionCombinada
        Dim dataMarcadores As New DataTable
        Dim xmlDocumento As New System.Xml.XmlDocument
        Dim xpnDocumento As System.Xml.XPath.XPathNavigator
        Dim xnmDocumento As System.Xml.XmlNamespaceManager
        Dim msPackage As IO.Packaging.PackagePart
        Dim uriPartTarget As Uri
        Dim strUri As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

        For Each eTablaMaestra As beTablaMaestra In listaTablaMaestra
            If eTablaMaestra.Imprimir.ToUpper() = "SI" Then
                'Dim objMemoryStream As MemoryStream = Nothing
                Dim drRegistros() As Data.DataRow
                'buscar segun : eTablaMaestra.IdSeccion
                drRegistros = dtbSeccionArchivo.Select("IdSeccion = " + eTablaMaestra.IdSeccion.Trim)
                If drRegistros.Length > 0 Then
                    Dim drData As DataRow = drRegistros(0)
                    Dim IdArchivoConfig As String = drData.Item("IdArchivoConfig").ToString
                    Dim NombreArchivo As String = drData.Item("NombreArchivo").ToString

                    ' Generar Documento segun el Codigo de seccion
                    Select Case drData.Item("CodigoSeccion").ToString.Trim.ToUpper
                        Case CodigoSeccion.EspecificacionTecnica
                            dtbAdicionalProducto = dtsDatosDocumento.Tables(Entidad.AdicionalProducto)
                            dtbAccesorioProducto = dtsDatosDocumento.Tables(Entidad.AccesorioProducto)

                            obeTablaMaestra.IdSeccionCriterio = drData.Item("IdSeccionCriterio").ToString()
                            obeTablaMaestra.IdTablaMaestra = eCotizacion.IdCotizacion
                            obeTablaMaestra.Nombre = "ESPECIFICACION" ' Para Filtrar productos de especificacion tecnicas con sus archivos
                            obcArchivoConfiguracion.BuscarArchivoProducto(Modulo.strConexionSql, obeTablaMaestra, dtbProducto)
                            Dim PosicionProductos As Integer = 0

                            For Each drProducto As DataRow In dtbProducto.Rows
                                Dim idProducto As String = drProducto.Item("IdProducto").ToString
                                Dim strMarcadores As String = String.Empty
                                PosicionProductos = PosicionProductos + 1
                                NombreArchivo = drProducto.Item("Archivo").ToString
                                objMemoryStream = objAdminFTP.ObtenerArchivo(Modulo.strUrlFtpArchivoPlantillas, NombreArchivo)

                                If Not objMemoryStream Is Nothing Then
                                    If objMemoryStream.Length > 0 Then
                                        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)

                                            Dim nroTablas As Integer = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table).Count
                                            Dim nombreTabla As String = ""
                                            Dim Cond_valor As String = ""
                                            Dim condiciones As String = ""
                                            Dim idTabla(nroTablas) As String
                                            Dim valTabla(nroTablas) As String
                                            'recorre todas las tablas para sacar los nombres
                                            Try
                                                Dim swNT As Boolean = False
                                                For tb As Integer = 0 To nroTablas - 1
                                                    Dim tablaPropuesta As New Table
                                                    tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(tb)

                                                    'capturamos los nombre de cada tabla
                                                    Dim xmlE As String = tablaPropuesta.OuterXml
                                                    Dim dsAux As DataSet
                                                    Try
                                                        Using cadena As New StringReader(xmlE)
                                                            dsAux = New DataSet
                                                            dsAux.ReadXml(cadena)
                                                        End Using
                                                    Catch ex As Exception
                                                        Continue For
                                                    End Try

                                                    If dsAux.Tables.Contains("tblCaption") Then
                                                        If dsAux.Tables("tblCaption").Rows.Count > 0 Then
                                                            If swNT Then
                                                                nombreTabla = nombreTabla + ","
                                                            End If
                                                            nombreTabla = nombreTabla + dsAux.Tables("tblCaption").Rows(0)("val").ToString.Trim
                                                            idTabla(tb) = tb
                                                            valTabla(tb) = dsAux.Tables("tblCaption").Rows(0)("val").ToString.Trim
                                                            swNT = True
                                                        Else
                                                            idTabla(tb) = "-1"
                                                            valTabla(tb) = "-1"
                                                        End If
                                                    Else
                                                        idTabla(tb) = "-1"
                                                        valTabla(tb) = "-1"
                                                    End If

                                                Next
                                            Catch ex As Exception
                                                ebeValidacion.validacion = False
                                                ebeValidacion.mensaje = "Error al capturar las tablas del documento: " + ex.Message.ToString
                                            End Try

                                            'Traemos las condiciones
                                            obcSolComb.DevolverCampos(Modulo.strConexionSql, nombreTabla, condiciones)
                                            Dim arrayCond() As String = Split(condiciones, ",")
                                            For i As Integer = 0 To arrayCond.Length - 1
                                                If i > 0 Then
                                                    Cond_valor = Cond_valor + ","
                                                End If
                                                Cond_valor = Cond_valor + arrayCond(i)
                                                'lleno las codiciones con sus valores
                                                Cond_valor = Cond_valor + "," + buscarCondicion(Cond_valor, eCotizacion, idProducto)
                                            Next

                                            'Obtenemos la data a mostrar en el documento
                                            Dim data As New DataTable
                                            obcSolComb.DevolverData(Modulo.strConexionSql, nombreTabla, Cond_valor, data)

                                            'recorre todas las tablas para llenar tablas
                                            Try
                                                For tb As Integer = 0 To nroTablas - 1
                                                    Dim contador As Integer = 0
                                                    Dim nroCeldas As Integer = 0
                                                    Dim sw As Boolean = True
                                                    Dim nroColumnaAnterior As String = ""
                                                    If idTabla(tb) < 0 Then
                                                        Continue For
                                                    End If
                                                    Dim tablaPropuesta As New Table
                                                    Dim dtTabla() As DataRow
                                                    Dim strSelect As String = "DescripcionTabla = '" & valTabla(tb) & "'"
                                                    tablaPropuesta = wordDoc.MainDocumentPart.Document.Body.Elements(Of Table)()(CInt(idTabla(tb)))
                                                    dtTabla = data.Select(strSelect)
                                                    Try
                                                        For Each registro As DataRow In dtTabla
                                                            Dim repetir As String = registro.Item("Repetir").ToString
                                                            Dim nroFila As String = registro.Item("Fila").ToString
                                                            Dim nroColumna As String = registro.Item("Columna").ToString
                                                            Dim fila As New TableRow
                                                            Dim tccelda_Fila0 As New TableCell

                                                            'logica para crear filas mientras haya mas de una fila una vez por cantidad de columnas
                                                            If contador = 0 Then
                                                                nroColumnaAnterior = nroColumna
                                                            End If
                                                            If Not nroColumnaAnterior = nroColumna And Not nroColumnaAnterior = "" Then
                                                                contador = 0
                                                                sw = False
                                                                nroColumnaAnterior = nroColumna
                                                            End If
                                                            If Not CBool(repetir) Then
                                                                contador = 0
                                                            End If
                                                            If contador > 0 And sw Then
                                                                Dim nuevaFila As New TableRow()
                                                                fila = tablaPropuesta.Elements(Of TableRow)().ElementAt(CInt(0))
                                                                tccelda_Fila0 = fila.Elements(Of TableCell)().ElementAt(CInt(0))
                                                                tccelda_Fila0.ClearAllAttributes()
                                                                nroCeldas = fila.Elements(Of TableCell).Count()
                                                                For k As Integer = 1 To nroCeldas
                                                                    Dim nuevaCelda As New TableCell(tccelda_Fila0.OuterXml)
                                                                    nuevaFila.Append(nuevaCelda)
                                                                Next
                                                                nuevaFila.ClearAllAttributes()
                                                                tablaPropuesta.Append(nuevaFila)
                                                                nroColumnaAnterior = nroColumna
                                                            End If

                                                            'Obtener la fila y celdas de la tabla
                                                            fila = New TableRow
                                                            tccelda_Fila0 = New TableCell
                                                            nroFila = nroFila + contador - 1
                                                            nroColumna = nroColumna - 1
                                                            fila = tablaPropuesta.Elements(Of TableRow)().ElementAt(CInt(nroFila))
                                                            tccelda_Fila0 = fila.Elements(Of TableCell)().ElementAt(CInt(nroColumna))

                                                            'Valores necesarios
                                                            Dim parag_Fila0 As Paragraph = tccelda_Fila0.Elements(Of Paragraph)().First()
                                                            If parag_Fila0 Is Nothing Then
                                                                parag_Fila0 = tccelda_Fila0.AppendChild(New Paragraph)
                                                            End If
                                                            Dim run_Fila0 As Run = parag_Fila0.Elements(Of Run).FirstOrDefault
                                                            If run_Fila0 Is Nothing Then
                                                                run_Fila0 = parag_Fila0.AppendChild(New Run)
                                                            End If
                                                            run_Fila0.RemoveAllChildren()
                                                            run_Fila0.AppendChild(New Text(registro.Item("valorCampo").ToString))
                                                            Dim RunPropCeldaTablaTexto24 As RunProperties
                                                            RunPropCeldaTablaTexto24 = RunPropertiesConfigurado(, , , "20")
                                                            run_Fila0.PrependChild(Of RunProperties)(RunPropCeldaTablaTexto24.Clone())
                                                            Dim paragraphProperties2 As ParagraphProperties = parag_Fila0.Elements(Of ParagraphProperties).FirstOrDefault()
                                                            If paragraphProperties2 Is Nothing Then
                                                                paragraphProperties2 = run_Fila0.AppendChild(New ParagraphProperties)
                                                            End If
                                                            Dim justification2 As Justification = New Justification() With {.Val = JustificationValues.Left}
                                                            paragraphProperties2.Justification = justification2

                                                            Dim runProps1 As RunProperties = RunPropCeldaTablaTexto24.Clone()
                                                            run_Fila0.AppendChild(runProps1)
                                                            contador = contador + 1
                                                        Next

                                                    Catch ex As Exception
                                                        ebeValidacion.validacion = False
                                                        ebeValidacion.mensaje = "Error al generar Especificacion Fila o columna no existen : " + ex.Message.ToString
                                                    End Try
                                                Next

                                                'LLenamos los marcadores
                                                'For Each oBookmarkStart As BookmarkStart In wordDoc.MainDocumentPart.Document.Body.Descendants(Of BookmarkStart)()
                                                '    For Each drFila As DataRow In dataMarcadores.Rows
                                                '        If oBookmarkStart.Name.ToString = drFila.Item("DescripcionTabla") Then
                                                '            Dim parent = oBookmarkStart.Parent ' bookmark's parent element

                                                '            If Not parent Is Nothing Then
                                                '                Select Case parent.GetType
                                                '                    Case GetType(Paragraph)
                                                '                        Dim runEscribibr As New Run(New Text(drFila.Item("ValorCampo")))
                                                '                        Dim rPr As RunProperties = New RunProperties(RunPropertiesConfigurado(True, , , , , ))
                                                '                        runEscribibr.PrependChild(Of RunProperties)(rPr)
                                                '                        parent.InsertBefore(runEscribibr, oBookmarkStart)
                                                '                        wordDoc.MainDocumentPart.Document.Save()
                                                '                        Exit Select
                                                '                    Case GetType(TableRow)
                                                '                        Exit Select
                                                '                End Select
                                                '            End If

                                                '            'Dim parag = New Paragraph
                                                '            'Dim text = New Text()
                                                '            'Dim run = New Run
                                                '            'run.Append(text)
                                                '            'Dim rPr As RunProperties = New RunProperties(RunPropertiesConfigurado(True, False, "Arial", 24, "FFFFFF", ))
                                                '            'run.PrependChild(Of RunProperties)(rPr)
                                                '            'parent.InsertBefore(run, oBookmarkStart)
                                                '            'wordDoc.MainDocumentPart.Document.Save()
                                                '        End If
                                                '    Next
                                                'Next



                                                wordDoc.MainDocumentPart.Document.Save()
                                            Catch ex As Exception
                                                ebeValidacion.validacion = False
                                                ebeValidacion.mensaje = "Error mientras se construia el documento: " + ex.Message.ToString
                                            End Try
                                        End Using


                                        'Marcadores
                                        'Recorremos marcadores
                                        Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(objMemoryStream, True)
                                            For Each oBookmarkStart As BookmarkStart In wordDoc.MainDocumentPart.Document.Body.Descendants(Of BookmarkStart)()
                                                strMarcadores = strMarcadores + oBookmarkStart.Name.ToString() + ","
                                            Next
                                            strMarcadores = strMarcadores.Remove(strMarcadores.Length - 1, 1)

                                            'Logica para traer las condiciones
                                            Dim strCondicionesMarcadores As String = String.Empty
                                            obcSolComb.DevolverCondicionesMarcadores(Modulo.strConexionSql, strMarcadores, strCondicionesMarcadores)
                                            Dim CondicionesMarcadores() As String = Split(strCondicionesMarcadores, ",")
                                            Dim ValorCondicion As String = String.Empty
                                            For i As Integer = 0 To CondicionesMarcadores.Length - 1
                                                If i > 0 Then
                                                    ValorCondicion = ValorCondicion + ","
                                                End If
                                                ValorCondicion = ValorCondicion + CondicionesMarcadores(i)
                                                'lleno las codiciones con sus valores
                                                ValorCondicion = ValorCondicion + "," + buscarCondicion(ValorCondicion, eCotizacion, idProducto)
                                            Next

                                            'Obtenemos la data a marcadores
                                            obcSolComb.DevolverDataMarcadores(Modulo.strConexionSql, strMarcadores, ValorCondicion, dataMarcadores)
                                        End Using

                                        Using package As System.IO.Packaging.Package = IO.Packaging.Package.Open(objMemoryStream, IO.FileMode.Open, IO.FileAccess.ReadWrite)

                                            'Recupera el xml del contenido del documento
                                            uriPartTarget = New Uri("/word/document.xml", UriKind.Relative)
                                            msPackage = package.GetPart(uriPartTarget)
                                            xmlDocumento.Load(msPackage.GetStream)

                                            'Se crea el navegador
                                            xpnDocumento = xmlDocumento.CreateNavigator()
                                            xnmDocumento = New System.Xml.XmlNamespaceManager(xpnDocumento.NameTable)
                                            xnmDocumento.AddNamespace("w", strUri)

                                            If strMarcadores.Length > 0 Then
                                                For Each xpnCampo In xpnDocumento.Select("//w:ffData", xnmDocumento)
                                                    Dim valorCampo As String = String.Empty
                                                    If xpnCampo.MoveToChild("name", strUri) Then
                                                        If xpnCampo.MoveToAttribute("val", strUri) Then
                                                            For Each drFila As DataRow In dataMarcadores.Rows
                                                                If xpnCampo.Value = drFila.Item("DescripcionTabla") Then
                                                                    valorCampo = drFila.Item("ValorCampo")
                                                                End If
                                                            Next
                                                        End If
                                                        If valorCampo <> String.Empty Then
                                                            'Move to w:instrText
                                                            If xpnCampo.MoveToFollowing("instrText", strUri) Then
                                                                'Check FORMTEXT
                                                                If xpnCampo.Value.Trim.Equals("FORMTEXT") Then
                                                                    'Move to t
                                                                    If xpnCampo.MoveToFollowing("t", strUri) Then
                                                                        'Set value
                                                                        xpnCampo.SetValue(valorCampo)
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                Next

                                            End If
                                            'Actualiza y cierra el documento
                                            xmlDocumento.Save(msPackage.GetStream(IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))
                                            msPackage.GetStream.Flush()
                                            msPackage.GetStream.Close()
                                        End Using

                                        ListaByteDocumento.Add(objMemoryStream.ToArray())
                                    End If
                                End If
                            Next
                        Case Else
                    End Select
                End If
            End If
        Next
        Return ListaByteDocumento
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

    Private Function buscarCondicion(ByVal Cond_valor As String, ByVal eCotizacion As beCotizacion, ByVal idProducto As String) As String

        Dim l_Producto As IEnumerable(Of beProducto)
        Dim eProducto As beProducto
        Dim result As String = ""

        Try
            l_Producto = eCotizacion.ListaProducto.Where(Function(c) c.IdProducto = idProducto)
            eProducto = CType(l_Producto.ToList, List(Of beProducto)).First
            result = DataBinder.Eval(eProducto, Cond_valor)

        Catch ex As Exception

        End Try
        
        Return result
    End Function
End Class
