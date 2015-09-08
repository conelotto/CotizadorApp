Imports Ferreyros.BECotizador
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros


Public Class WSAccesoLocal
     
    Private iCountProducto As Integer = 0
    Private iCountMaquinaria As Integer = 0
    Private objCredentialCache As System.Net.CredentialCache = Nothing
    Private uConfig As New Utiles.uConfiguracion



    Public Sub GuardarCambiosSapDEV(ByVal Cotizacion As beCotizacion, ByRef Validacion As beValidacion)
        Dim strRecorrido As String = String.Empty
        strRecorrido = "1"

        Try

            Dim objCotizacion As wsRespuestaSapDEV.ZWS_IN_COT_CSA_BClient = Nothing
            Dim objArrayProducto() As wsRespuestaSapDEV.ZPRODWSCSA = Nothing
            Dim objArrayMaquinaria() As wsRespuestaSapDEV.ZMAQWSCSA = Nothing
            Dim objRequest As wsRespuestaSapDEV.ZWS_RECIBE_COTIZACION_CSA = Nothing
            Dim objResponse As wsRespuestaSapDEV.ZWS_RECIBE_COTIZACION_CSAResponse = Nothing

            strRecorrido = "2"
            objCotizacion = New wsRespuestaSapDEV.ZWS_IN_COT_CSA_BClient
            objRequest = New wsRespuestaSapDEV.ZWS_RECIBE_COTIZACION_CSA
            objResponse = New wsRespuestaSapDEV.ZWS_RECIBE_COTIZACION_CSAResponse

            strRecorrido = "3"
            iCountProducto = Cotizacion.ListaProducto.Count
            ReDim objArrayProducto(iCountProducto - 1)

            strRecorrido = "4"
            For i As Integer = 0 To iCountProducto - 1

                strRecorrido = "5"
                Dim Producto As beProducto = Cotizacion.ListaProducto(i)
                Dim objProducto As New wsRespuestaSapDEV.ZPRODWSCSA

                strRecorrido = "6"
                With objProducto
                    .DETALLE_PARTES = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeDetallePartes)
                    .FECHA_I = uConfig.fc_ConvertirFechaSAP(Producto.ProductoCSA.FechaInicioContrato)
                    .FLUIDOS = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeFluidos)
                    .ITEM = Producto.IdPosicion
                    .MONTO = Producto.ValorNeto
                    If TipoProducto.CSA = Producto.TipoProducto Then
                        iCountMaquinaria = Producto.ProductoCSA.ListaMaquinaria.Count
                        objArrayMaquinaria = Nothing
                        ReDim objArrayMaquinaria(iCountMaquinaria - 1)
                        For j As Integer = 0 To iCountMaquinaria - 1

                            strRecorrido = "7"
                            Dim Maquinaria As beMaquinaria = Producto.ProductoCSA.ListaMaquinaria(j)
                            Dim objMaquinaria As New wsRespuestaSapDEV.ZMAQWSCSA

                            strRecorrido = "8"
                            With objMaquinaria
                                .COD_DPTO = Maquinaria.codDepartamento
                                .DEPARTAMENTO = Maquinaria.departamento
                                .FAMILIA = Maquinaria.familia
                                .FECHA_HOROMETRO = uConfig.fc_ConvertirFechaSAP(Maquinaria.fechaHorometro)
                                .HORAS_PROMEDIO = Maquinaria.horasPromedioMensual
                                .HOROMETRO_F = Maquinaria.horometroFinal
                                .HOROMETRO_I = Maquinaria.horometroInicial
                                .MAQUINA_NUEVA = uConfig.fc_ConvertirBooleanSAP(Maquinaria.maquinaNueva)
                                .MODELO = Maquinaria.modelo
                                .MODELO_BASE = Maquinaria.modeloBase
                                .NUMERO_SERIE = Maquinaria.numeroSerie
                                .PREFIJO = Maquinaria.prefijo
                                .RENOVACION = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacion)
                                .RENOVACION_VAL = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacionValida)
                            End With
                            objArrayMaquinaria(j) = objMaquinaria
                        Next
                        .MAQUINA = objArrayMaquinaria
                    End If
                End With

                strRecorrido = "9"
                objArrayProducto(i) = objProducto
            Next

            strRecorrido = "10"
            objRequest.ID_COTIZACION = Cotizacion.IdCotizacionSap
            objRequest.PRODUCTO = objArrayProducto

            strRecorrido = "11"
            objCotizacion.ClientCredentials.UserName.UserName = Modulo.strUsuarioServSAP
            objCotizacion.ClientCredentials.UserName.Password = Modulo.strPasswordServSAP

            strRecorrido = "12"
            objResponse = objCotizacion.ZWS_RECIBE_COTIZACION_CSA(objRequest)

            strRecorrido = "13"
            Validacion.respuesta = objResponse.E_RESULTADO
            Validacion.mensaje = objResponse.MENSAJE_RESULTADO
            Validacion.validacion = objResponse.E_RESULTADO
            strRecorrido = "15"
        Catch ex As Exception
            Dim mensaje As String = "Error comunicación con Sap: "
            mensaje = String.Concat(mensaje, strRecorrido, " - ", ex.Message, " - ", ex.StackTrace)

            Validacion.mensaje = mensaje
            Validacion.validacion = False
            'Throw New Exception(mensaje)
        End Try

    End Sub
    Public Sub GuardarCambiosSapQA(ByVal Cotizacion As beCotizacion, ByRef Validacion As beValidacion)
        Dim strRecorrido As String = String.Empty
        strRecorrido = "1"

        Try
            Dim objCotizacion As wsRespuestaSapQA.ZWS_IN_COT_CSA_BClient = Nothing
            Dim objArrayProducto() As wsRespuestaSapQA.ZPRODWSCSA = Nothing
            Dim objArrayMaquinaria() As wsRespuestaSapQA.ZMAQWSCSA = Nothing
            Dim objRequest As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSA = Nothing
            Dim objResponse As wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse = Nothing

            strRecorrido = "2"
            objCotizacion = New wsRespuestaSapQA.ZWS_IN_COT_CSA_BClient
            objRequest = New wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSA
            objResponse = New wsRespuestaSapQA.ZWS_RECIBE_COTIZACION_CSAResponse

            strRecorrido = "3"
            iCountProducto = Cotizacion.ListaProducto.Count
            ReDim objArrayProducto(iCountProducto - 1)

            strRecorrido = "4"
            For i As Integer = 0 To iCountProducto - 1

                strRecorrido = "5"
                Dim Producto As beProducto = Cotizacion.ListaProducto(i)
                Dim objProducto As New wsRespuestaSapQA.ZPRODWSCSA

                strRecorrido = "6"
                With objProducto
                    .DETALLE_PARTES = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeDetallePartes)
                    .FECHA_I = uConfig.fc_ConvertirFechaSAP(Producto.ProductoCSA.FechaInicioContrato)
                    .FLUIDOS = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeFluidos)
                    .ITEM = Producto.IdPosicion
                    .MONTO = Producto.ValorUnitario
                    If TipoProducto.CSA = Producto.TipoProducto Then
                        iCountMaquinaria = Producto.ProductoCSA.ListaMaquinaria.Count
                        objArrayMaquinaria = Nothing
                        ReDim objArrayMaquinaria(iCountMaquinaria - 1)
                        For j As Integer = 0 To iCountMaquinaria - 1

                            strRecorrido = "7"
                            Dim Maquinaria As beMaquinaria = Producto.ProductoCSA.ListaMaquinaria(j)
                            Dim objMaquinaria As New wsRespuestaSapQA.ZMAQWSCSA

                            strRecorrido = "8"
                            With objMaquinaria
                                .COD_DPTO = Maquinaria.codDepartamento
                                .DEPARTAMENTO = Maquinaria.departamento
                                .FAMILIA = Maquinaria.familia
                                .FECHA_HOROMETRO = uConfig.fc_ConvertirFechaSAP(Maquinaria.fechaHorometro)
                                .HORAS_PROMEDIO = Maquinaria.horasPromedioMensual
                                .HOROMETRO_F = Maquinaria.horometroFinal
                                .HOROMETRO_I = Maquinaria.horometroInicial
                                .MAQUINA_NUEVA = uConfig.fc_ConvertirBooleanSAP(Maquinaria.maquinaNueva)
                                .MODELO = Maquinaria.modelo
                                .MODELO_BASE = Maquinaria.modeloBase
                                .NUMERO_SERIE = Maquinaria.numeroSerie
                                .PREFIJO = Maquinaria.prefijo
                                .RENOVACION = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacion)
                                .RENOVACION_VAL = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacionValida)
                            End With
                            objArrayMaquinaria(j) = objMaquinaria
                        Next
                        .MAQUINA = objArrayMaquinaria
                    End If
                End With

                strRecorrido = "9"
                objArrayProducto(i) = objProducto
            Next

            strRecorrido = "10"
            objRequest.ID_COTIZACION = Cotizacion.IdCotizacionSap
            objRequest.PRODUCTO = objArrayProducto

            strRecorrido = "11"
            objCotizacion.ClientCredentials.UserName.UserName = Modulo.strUsuarioServSAP
            objCotizacion.ClientCredentials.UserName.Password = Modulo.strPasswordServSAP

            strRecorrido = "12"
            objResponse = objCotizacion.ZWS_RECIBE_COTIZACION_CSA(objRequest)

            strRecorrido = "13"
            Validacion.respuesta = objResponse.E_RESULTADO
            Validacion.mensaje = objResponse.MENSAJE_RESULTADO
            Validacion.validacion = objResponse.E_RESULTADO
            strRecorrido = "15"
        Catch ex As Exception
            Dim mensaje As String = "Error comunicación con Sap: "
            mensaje = String.Concat(mensaje, strRecorrido, " - ", ex.Message, " - ", ex.StackTrace)

            Validacion.mensaje = mensaje
            Validacion.validacion = False
            'Throw New Exception(mensaje)
        End Try

    End Sub
    Public Sub GuardarCambiosSapPRD(ByVal Cotizacion As beCotizacion, ByRef Validacion As beValidacion)
        Dim strRecorrido As String = String.Empty
        strRecorrido = "1"

        Try
            Dim objCotizacion As wsRespuestaSapPRD.ZWS_IN_COT_CSA_BClient = Nothing
            Dim objArrayProducto() As wsRespuestaSapPRD.ZPRODWSCSA = Nothing
            Dim objArrayMaquinaria() As wsRespuestaSapPRD.ZMAQWSCSA = Nothing
            Dim objRequest As wsRespuestaSapPRD.ZWS_RECIBE_COTIZACION_CSA = Nothing
            Dim objResponse As wsRespuestaSapPRD.ZWS_RECIBE_COTIZACION_CSAResponse = Nothing

            strRecorrido = "2"
            objCotizacion = New wsRespuestaSapPRD.ZWS_IN_COT_CSA_BClient
            objRequest = New wsRespuestaSapPRD.ZWS_RECIBE_COTIZACION_CSA
            objResponse = New wsRespuestaSapPRD.ZWS_RECIBE_COTIZACION_CSAResponse

            strRecorrido = "3"
            iCountProducto = Cotizacion.ListaProducto.Count
            ReDim objArrayProducto(iCountProducto - 1)

            strRecorrido = "4"
            For i As Integer = 0 To iCountProducto - 1

                strRecorrido = "5"
                Dim Producto As beProducto = Cotizacion.ListaProducto(i)
                Dim objProducto As New wsRespuestaSapPRD.ZPRODWSCSA

                strRecorrido = "6"
                With objProducto
                    .DETALLE_PARTES = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeDetallePartes)
                    .FECHA_I = uConfig.fc_ConvertirFechaSAP(Producto.ProductoCSA.FechaInicioContrato)
                    .FLUIDOS = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeFluidos)
                    .ITEM = Producto.IdPosicion
                    .MONTO = Producto.ValorNeto
                    If TipoProducto.CSA = Producto.TipoProducto Then
                        iCountMaquinaria = Producto.ProductoCSA.ListaMaquinaria.Count
                        objArrayMaquinaria = Nothing
                        ReDim objArrayMaquinaria(iCountMaquinaria - 1)
                        For j As Integer = 0 To iCountMaquinaria - 1

                            strRecorrido = "7"
                            Dim Maquinaria As beMaquinaria = Producto.ProductoCSA.ListaMaquinaria(j)
                            Dim objMaquinaria As New wsRespuestaSapPRD.ZMAQWSCSA

                            strRecorrido = "8"
                            With objMaquinaria
                                .COD_DPTO = Maquinaria.codDepartamento
                                .DEPARTAMENTO = Maquinaria.departamento
                                .FAMILIA = Maquinaria.familia
                                .FECHA_HOROMETRO = uConfig.fc_ConvertirFechaSAP(Maquinaria.fechaHorometro)
                                .HORAS_PROMEDIO = Maquinaria.horasPromedioMensual
                                .HOROMETRO_F = Maquinaria.horometroFinal
                                .HOROMETRO_I = Maquinaria.horometroInicial
                                .MAQUINA_NUEVA = uConfig.fc_ConvertirBooleanSAP(Maquinaria.maquinaNueva)
                                .MODELO = Maquinaria.modelo
                                .MODELO_BASE = Maquinaria.modeloBase
                                .NUMERO_SERIE = Maquinaria.numeroSerie
                                .PREFIJO = Maquinaria.prefijo
                                .RENOVACION = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacion)
                                .RENOVACION_VAL = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacionValida)
                            End With
                            objArrayMaquinaria(j) = objMaquinaria
                        Next
                        .MAQUINA = objArrayMaquinaria
                    End If
                End With

                strRecorrido = "9"
                objArrayProducto(i) = objProducto
            Next

            strRecorrido = "10"
            objRequest.ID_COTIZACION = Cotizacion.IdCotizacionSap
            objRequest.PRODUCTO = objArrayProducto

            strRecorrido = "11"
            objCotizacion.ClientCredentials.UserName.UserName = Modulo.strUsuarioServSAP
            objCotizacion.ClientCredentials.UserName.Password = Modulo.strPasswordServSAP

            strRecorrido = "12"
            objResponse = objCotizacion.ZWS_RECIBE_COTIZACION_CSA(objRequest)

            strRecorrido = "13"
            Validacion.respuesta = objResponse.E_RESULTADO
            Validacion.mensaje = objResponse.MENSAJE_RESULTADO
            Validacion.validacion = objResponse.E_RESULTADO
            strRecorrido = "15"
        Catch ex As Exception
            Dim mensaje As String = "Error comunicación con Sap: "
            mensaje = String.Concat(mensaje, strRecorrido, " - ", ex.Message, " - ", ex.StackTrace)

            Validacion.mensaje = mensaje
            Validacion.validacion = False
            'Throw New Exception(mensaje)
        End Try

    End Sub
    'Public Sub GuardarCambiosSAP(ByVal Cotizacion As beCotizacion, ByRef Validacion As beValidacion)

    '    Dim objCotizacion As New wsRespuestaSAP.ZWS_IN_COT_CSA_BClient
    '    Dim objRequest As New wsRespuestaSAP.ZWS_RECIBE_COTIZACION_CSA
    '    Dim objResponse As New wsRespuestaSAP.ZWS_RECIBE_COTIZACION_CSAResponse

    '    iCountProducto = Cotizacion.ListaProducto.Count
    '    ReDim objArrayProducto(iCountProducto - 1)

    '    For i As Integer = 0 To iCountProducto - 1
    '        Dim Producto As beProducto = Cotizacion.ListaProducto(i)
    '        Dim objProducto As New wsRespuestaSAP.ZPRODWSCSA
    '        With objProducto
    '            .DETALLE_PARTES = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeDetallePartes)
    '            .FECHA_I = uConfig.fc_ConvertirFechaSAP(Producto.ProductoCSA.FechaInicioContrato)
    '            .FLUIDOS = uConfig.fc_ConvertirBooleanSAP(Producto.ProductoCSA.IncluyeFluidos)
    '            .ITEM = Producto.IdPosicion
    '            .MONTO = Producto.ValorNeto
    '            If TipoProducto.CSA = Producto.TipoProducto Then
    '                iCountMaquinaria = Producto.ProductoCSA.ListaMaquinaria.Count
    '                objArrayMaquinaria = Nothing
    '                ReDim objArrayMaquinaria(iCountMaquinaria - 1)
    '                For j As Integer = 0 To iCountMaquinaria - 1
    '                    Dim Maquinaria As beMaquinaria = Producto.ProductoCSA.ListaMaquinaria(j)
    '                    Dim objMaquinaria As New wsRespuestaSAP.ZMAQWSCSA
    '                    With objMaquinaria
    '                        .COD_DPTO = Maquinaria.codDepartamento
    '                        .DEPARTAMENTO = Maquinaria.departamento
    '                        .FAMILIA = Maquinaria.familia
    '                        .FECHA_HOROMETRO = uConfig.fc_ConvertirFechaSAP(Maquinaria.fechaHorometro)
    '                        .HORAS_PROMEDIO = Maquinaria.horasPromedioMensual
    '                        .HOROMETRO_F = Maquinaria.horometroFinal
    '                        .HOROMETRO_I = Maquinaria.horometroInicial
    '                        .MAQUINA_NUEVA = uConfig.fc_ConvertirBooleanSAP(Maquinaria.maquinaNueva)
    '                        .MODELO = Maquinaria.modelo
    '                        .MODELO_BASE = Maquinaria.modeloBase
    '                        .NUMERO_SERIE = Maquinaria.numeroSerie
    '                        .PREFIJO = Maquinaria.prefijo
    '                        .RENOVACION = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacion)
    '                        .RENOVACION_VAL = uConfig.fc_ConvertirBooleanSAP(Maquinaria.renovacionValida) 
    '                    End With
    '                    objArrayMaquinaria(j) = objMaquinaria
    '                Next
    '                .MAQUINA = objArrayMaquinaria
    '            End If
    '        End With
    '        objArrayProducto(i) = objProducto
    '    Next

    '    objRequest.ID_COTIZACION = Cotizacion.IdCotizacionSap
    '    objRequest.PRODUCTO = objArrayProducto

    '    objCotizacion.ClientCredentials.UserName.UserName = Modulo.strUsuarioServSAP
    '    objCotizacion.ClientCredentials.UserName.Password = Modulo.strPasswordServSAP

    '    'objCotizacion.Url = "http://fsacrmdevqa.dominio.ferreyros.com.pe:8001/sap/bc/srt/wsdl/srvc_5057791F5F1202F0E10080000A4B010D/wsdl11/allinone/ws_policy/document?sap-client=100"
    '    'objCotizacion.Credentials = New System.Net.NetworkCredential("WSFSAA", "WsFerr20")
    '    'objCotizacion.Proxy = New System.Net.WebProxy

    '    objResponse = objCotizacion.ZWS_RECIBE_COTIZACION_CSA(objRequest)

    '    Validacion.respuesta = objResponse.E_RESULTADO
    '    Validacion.mensaje = objResponse.MENSAJE_RESULTADO
    '    Validacion.validacion = objResponse.E_RESULTADO

    'End Sub


End Class
