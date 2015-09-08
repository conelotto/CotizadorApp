Imports Ferreyros.ClasServicioCotizador
Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page
    Private strConexionAS400 As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConexionAS400")
    Private strConexionCotizador As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConexionCotizador")
    Private strUrlFtpDescarga As String = System.Configuration.ConfigurationManager.AppSettings.Get("UrlFtpDescarga")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim dato As Integer
        'dato = TryCast(integer, dato)
        'If Not IsDBNull(dato) Then dato = ""
        'Server.MapPath("~")
        'NombUrlLog = Server.MapPath("~\Log") & "\" & "logCuenta_" & Fecha & ".txt"

        Dim Generator As System.Random = New System.Random()
       
        Dim a As String
        a = Generator.Next(1000, 9999)

        ScriptManager.RegisterStartupScript(Page, Page.GetType, "alert", "alert('" + GenerarRandom() + "--" + "')", True)


    End Sub

    Protected Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Dim obeCotizacion As New beCotizacion
        'obeCotizacion.IdCorporacion = "50000066"
        'obeCotizacion.IdCompanhia = "50000068"
        'obeCotizacion.OficinaResponsable = "50000358"


        'obeCotizacion.AnexoTelefonoResponsable

        obeCotizacion.CargoResponsable = "REPRESENTANTE DE VEN"

        'obeCotizacion.CodigoMercadoSolicitante

        'obeCotizacion.DNISolicitante

        obeCotizacion.DescripResponsable = "IVAN HAROLD LAZO RAMOS"

        obeCotizacion.DescripSolicitante = "CAMINOS SA"

        obeCotizacion.EmailResponsable = "ivan.lazo@ferreyros.com.pe"

        obeCotizacion.FechaEstimadaFacturacion = "20130613"

        obeCotizacion.FechaFinalValidez = "20130627"

        obeCotizacion.FechaInicioValidez = "20130613"

        obeCotizacion.FechaPrecio = "20130613"

        obeCotizacion.IdCompanhia = "50000001"

        obeCotizacion.IdCorporacion = "50000000"

        obeCotizacion.IdCotizacion = "0002000749"

        obeCotizacion.IdPersonaResponsable = "100843"

        obeCotizacion.IdSolicitante = "1021657"

        'obeCotizacion.ItemOportunidad 
        Dim lista_Cotizacion(1) As beCotizacionContacto
        Dim eCotizacionContacto As New beCotizacionContacto
        eCotizacionContacto.Direccion = "NRO. 30 INT. 40 URB. PREVI LIMA LOS OLIVOS"
        eCotizacionContacto.Nombres = "ROBERTO ESTEBAN PILCO QUESQUEN"
        eCotizacionContacto.Telefono = "( 01)6238593"

        lista_Cotizacion(0) = eCotizacionContacto


        '    'Dim l_contacto(1) As WcfLocal.beCotizacionContacto
        '    'Dim econtacto As WcfLocal.beCotizacionContacto

        '    'econtacto = New WcfLocal.beCotizacionContacto
        '    'econtacto._Nombres = "Maria Chavez"
        '    'econtacto._Cargo = "jefa de logistica"
        '    'econtacto._Email = "MariaChavez@gmail.com"
        '    'econtacto._Direccion = "lima"
        '    'econtacto._Telefono = "995620012"

        '    'l_contacto(0) = econtacto

        '    'econtacto = New WcfLocal.beCotizacionContacto
        '    'econtacto._Nombres = "Camila Gonzalez"
        '    'econtacto._Cargo = "secretaria de logistica"
        '    'econtacto._Email = "camilagonzalez@gmail.com"
        '    'econtacto._Direccion = "lima"
        '    'econtacto._Telefono = "996581258"

        '    'l_contacto(1) = econtacto

        '    'vCotizacion._ListaCotizacionContacto = l_contacto


        obeCotizacion.ListaCotizacionContacto = lista_Cotizacion


        'obeCotizacion.ListaProducto 

        'obeCotizacion.MonedaValorBruto

        'obeCotizacion.MonedaValorImpuesto

        'obeCotizacion.MonedaValorNeto 

        obeCotizacion.NombreEstado = "Initial"

        obeCotizacion.NumeroOportunidad = "0001001188"

        obeCotizacion.OficinaResponsable = "50002555"

        obeCotizacion.RUCSolicitante = "20123789556"

        obeCotizacion.TelefonoResponsable = "249632"

        obeCotizacion.TotalDescuentoImp = "0.00"

        obeCotizacion.TotalDescuentoPorc = "0.00"

        obeCotizacion.TotalFlete = "0.00"

        obeCotizacion.TotalPrecioVentaFinal = "0.00"

        obeCotizacion.TotalValorImpuesto = "0.00"

        obeCotizacion.TotalValorLista = "0.00"

        obeCotizacion.TotalValorReal = "0.00"

        obeCotizacion.TotalValorVenta = "0.00"

        obeCotizacion.Usuario = "ILAZO"

        'obeCotizacion.ValorBruto 

        'obeCotizacion.ValorImpuesto 

        'obeCotizacion.ValorNeto 

        obeCotizacion.ValorTipoCambio = "3.52"

        obeCotizacion.Version = "0"



        Dim obcCotizacion As New bcCotizacion
        Dim obeValidacion As New beValidacion

        obcCotizacion.UrlServicio = HttpContext.Current.Server.MapPath("").ToString
        obcCotizacion.UrlServicio = Hosting.HostingEnvironment.ApplicationPhysicalPath
        Call obcCotizacion.InsertarCotizacion(strConexionCotizador, strConexionAS400, obeCotizacion, obeValidacion)
        'Call obcCotizacion.HomologarCamposCotizacion(strConexionAS400, obeCotizacion, obeValidacion)

    End Sub

    Protected Sub btnPrueba2_Click(sender As Object, e As EventArgs) Handles btnPrueba2.Click
        'Dim valor As String = Nothing
        'valor = valor.ToString.PadRight(2, Char.Parse(" "))
        'valor = valor & "-Yo"
        'ScriptManager.RegisterStartupScript(Page, Page.GetType, "alert", "alert('" + valor + "')", True)

        Dim tramaXml As String = String.Empty
        tramaXml = "<TIPOREGISTRO>C</TIPOREGISTRO><CORP>001</CORP><CIA>2</CIA><CODCLIENTE></CODCLIENTE><OFICINA>01</OFICINA><INACTIVO>N</INACTIVO><RAZONSOCIAL>D Y M MAQUINARIA Y MINERIA S.A.C.</RAZONSOCIAL><PAIS>604</PAIS><TIPOPERSONA>J</TIPOPERSONA><TIPODOCI></TIPODOCI><NUMDOCI></NUMDOCI><TIPODOCT>R</TIPODOCT><NUMDOCT>20498395458</NUMDOCT><TIPOCLIENTE>P</TIPOCLIENTE><SECTOREC>02</SECTOREC><GIRONEG>CN02</GIRONEG><GRUPOEC></GRUPOEC><CLASIF>B1</CLASIF><CARTERA>A</CARTERA><CUENTA></CUENTA><MONEDA></MONEDA><INDIMPUESTO>S</INDIMPUESTO><DIASVENC></DIASVENC><MOTIVOINAC></MOTIVOINAC><DESCINAC></DESCINAC><CONTADO></CONTADO><CHEQUE></CHEQUE><PREMIUM></PREMIUM><BOLETA></BOLETA><USUARIO>10652</USUARIO><LIBRERIA>LIBT99</LIBRERIA><URL></URL><AREA_RES>02</AREA_RES><INDFV></INDFV><ORIGEN>1</ORIGEN><TERMOD>SAPC</TERMOD><SEDES>LIBT99<SEDE><SECUENCIAL>1</SECUENCIAL><TIPOSEDE>SUCURSAL       0</TIPOSEDE><DESCRIPCION>OFICINA</DESCRIPCION><DIRECCION>JR  ESPINAR MZ 21 LT 5</DIRECCION><REFERENCIA1>URB LOS CHOFERES</REFERENCIA1><REFERENCIA2></REFERENCIA2><REFERENCIA3>11278</REFERENCIA3><DEPARTAMENTO>U00</DEPARTAMENTO><PROVINCIA>UK0</PROVINCIA><DISTRITO>UKA</DISTRITO><CPOSTAL></CPOSTAL><TIPOTELEFONO1>1B</TIPOTELEFONO1><TELEFONO1>051329807</TELEFONO1><TIPOTELEFONO2>1H</TIPOTELEFONO2><TELEFONO2>NO TIENE</TELEFONO2><EXPIRACION>99999999</EXPIRACION><PONORQ></PONORQ><IDCD>CN02</IDCD><EMSIND></EMSIND><PARIND></PARIND><SRVIND></SRVIND></SEDE><SEDE><SECUENCIAL>0</SECUENCIAL><TIPOSEDE>PRINCIPAL      1</TIPOSEDE><DESCRIPCION>OFICINA PRINCIPAL</DESCRIPCION><DIRECCION>CL  SANCHEZ TRUJILLO NRO. 234</DIRECCION><REFERENCIA1>A TRES CUADRA IGLESIA CHAPI CHICO</REFERENCIA1><REFERENCIA2></REFERENCIA2><REFERENCIA3>11279</REFERENCIA3><DEPARTAMENTO>D00</DEPARTAMENTO><PROVINCIA>DA0</PROVINCIA><DISTRITO>DAD</DISTRITO><CPOSTAL></CPOSTAL><TIPOTELEFONO1>1I</TIPOTELEFONO1><TELEFONO1>NO DISPONIBLE</TELEFONO1><TIPOTELEFONO2>1I</TIPOTELEFONO2><TELEFONO2>NO DISPONIBLE</TELEFONO2><EXPIRACION>99999999</EXPIRACION><PONORQ></PONORQ><IDCD>CN02</IDCD><EMSIND></EMSIND><PARIND></PARIND><SRVIND></SRVIND></SEDE><SEDE><SECUENCIAL>3</SECUENCIAL><TIPOSEDE>OBRA           2</TIPOSEDE><DESCRIPCION>TRAMO HURAHUASI</DESCRIPCION><DIRECCION>CA  HURAHUASI</DIRECCION><REFERENCIA1>OLLACHEA-SAN GABAN</REFERENCIA1><REFERENCIA2></REFERENCIA2><REFERENCIA3>11280</REFERENCIA3><DEPARTAMENTO>U00</DEPARTAMENTO><PROVINCIA>UC0</PROVINCIA><DISTRITO>UCH</DISTRITO><CPOSTAL></CPOSTAL><TIPOTELEFONO1>1D</TIPOTELEFONO1><TELEFONO1>959288797</TELEFONO1><TIPOTELEFONO2>1H</TIPOTELEFONO2><TELEFONO2>NO TIENE</TELEFONO2><EXPIRACION>99999999</EXPIRACION><PONORQ></PONORQ><IDCD>CN02</IDCD><EMSIND></EMSIND><PARIND></PARIND><SRVIND></SRVIND></SEDE><SEDE><SECUENCIAL>4</SECUENCIAL><TIPOSEDE>OBRA           3</TIPOSEDE><DESCRIPCION>CANTERA MACUSANI; SAN ANTON ;</DESCRIPCION><DIRECCION>CA  CARRETERA</DIRECCION><REFERENCIA1>CHUANI</REFERENCIA1><REFERENCIA2></REFERENCIA2><REFERENCIA3>11281</REFERENCIA3><DEPARTAMENTO>U00</DEPARTAMENTO><PROVINCIA>UC0</PROVINCIA><DISTRITO>UCA</DISTRITO><CPOSTAL></CPOSTAL><TIPOTELEFONO1>1I</TIPOTELEFONO1><TELEFONO1>NO DISPONIBLE</TELEFONO1><TIPOTELEFONO2>1I</TIPOTELEFONO2><TELEFONO2>NO DISPONIBLE</TELEFONO2><EXPIRACION>99999999</EXPIRACION><PONORQ></PONORQ><IDCD>CN02</IDCD><EMSIND></EMSIND><PARIND></PARIND><SRVIND></SRVIND></SEDE></SEDES><DIRECCIONES><DIRECCION><OFICINA>0</OFICINA><SECTOR></SECTOR><DIRECCION></DIRECCION><LOCALIDAD></LOCALIDAD><DEPARTAMENTO></DEPARTAMENTO><PROVINCIA></PROVINCIA><CPOSTAL></CPOSTAL><TELEFONO>051329807</TELEFONO><ANEXO></ANEXO><FAX>NO TIENE</FAX></DIRECCION></DIRECCIONES>"

        Dim trTrama As StringBuilder = New StringBuilder
        trTrama.Append("<Datos>")
        trTrama.Append("<ACTCLIENTERs>")
        trTrama.Append("<TIPOREGISTRO>C</TIPOREGISTRO>")
        trTrama.Append("<CORP>001</CORP>")
        trTrama.Append("<CIA>2</CIA>")
        trTrama.Append("<CODCLIENTE>5000836</CODCLIENTE>")
        trTrama.Append("<OFICINA>50</OFICINA>")
        trTrama.Append("<INACTIVO>N</INACTIVO>")
        trTrama.Append("<RAZONSOCIAL>CONSTRUCTORA TRITON E.I.R.L.</RAZONSOCIAL>")
        trTrama.Append("<PAIS>604</PAIS>")
        trTrama.Append("<TIPOPERSONA>j</TIPOPERSONA>")
        trTrama.Append("<TIPODOCI/>")
        trTrama.Append("<NUMDOCI/>")
        trTrama.Append("<TIPODOCT>R</TIPODOCT>")
        trTrama.Append("<NUMDOCT>20445669581</NUMDOCT>")
        trTrama.Append("<TIPOCLIENTE>P</TIPOCLIENTE>")
        trTrama.Append("<SECTOREC>02</SECTOREC>")
        trTrama.Append("<GIRONEG>CN08</GIRONEG>")
        trTrama.Append("<GRUPOEC/>")
        trTrama.Append("<CLASIF>B1</CLASIF>")
        trTrama.Append("<CARTERA>A</CARTERA>")
        trTrama.Append("<CUENTA/>")
        trTrama.Append("<MONEDA/>")
        trTrama.Append("<INDIMPUESTO>S</INDIMPUESTO>")
        trTrama.Append("<DIASVENC/>")
        trTrama.Append("<MOTIVOINAC/>")
        trTrama.Append("<DESCINAC/>")
        trTrama.Append("<CONTADO/>")
        trTrama.Append("<CHEQUE/>")
        trTrama.Append("<PREMIUM/>")
        trTrama.Append("<BOLETA/>")
        trTrama.Append("<USUARIO>135159</USUARIO>")
        trTrama.Append("<LIBRERIA>LIBT99</LIBRERIA>")
        trTrama.Append("<URL>WWW.TRITON.COM.PE</URL>")
        trTrama.Append("<AREA_RES>02</AREA_RES>")
        trTrama.Append("<INDFV/>")
        trTrama.Append("<ORIGEN>1</ORIGEN>")
        trTrama.Append("<TERMOD>SAPC</TERMOD>")
        trTrama.Append("<SEDES>")
        trTrama.Append("<SEDE>")
        trTrama.Append("<SECUENCIAL>0.000</SECUENCIAL>")
        trTrama.Append("<TIPOSEDE>PRINCIPAL      0</TIPOSEDE>")
        trTrama.Append("<DESCRIPCION>PRINCIPAL</DESCRIPCION>")
        trTrama.Append("<DIRECCION>URB MZA. L2 LOTE. 3</DIRECCION>")
        trTrama.Append("<REFERENCIA1>ET 2</REFERENCIA1>")
        trTrama.Append("<REFERENCIA2/>")
        trTrama.Append("<REFERENCIA3>135393</REFERENCIA3>")
        trTrama.Append("<DEPARTAMENTO>B00</DEPARTAMENTO>")
        trTrama.Append("<PROVINCIA>BR0</PROVINCIA>")
        trTrama.Append("<DISTRITO>BRI</DISTRITO>")
        trTrama.Append("<CPOSTAL/>")
        trTrama.Append("<TIPOTELEFONO1>1B</TIPOTELEFONO1>")
        trTrama.Append("<TELEFONO1>043324456</TELEFONO1>")
        trTrama.Append("<TIPOTELEFONO2>1H</TIPOTELEFONO2>")
        trTrama.Append("<TELEFONO2>NO DISPONIBLE</TELEFONO2>")
        trTrama.Append("<EXPIRACION>99999999</EXPIRACION>")
        trTrama.Append("<PONORQ/>")
        trTrama.Append("<IDCD>CN08</IDCD>")
        trTrama.Append("<EMSIND/>")
        trTrama.Append("<PARIND/>")
        trTrama.Append("<SRVIND/>")
        trTrama.Append("</SEDE>")
        trTrama.Append("</SEDES>")
        trTrama.Append("<DIRECCIONES>")
        trTrama.Append("<DIRECCION>")
        trTrama.Append("<OFICINA>0</OFICINA>")
        trTrama.Append("<SECTOR/>")
        trTrama.Append("<DIRECCION/>")
        trTrama.Append("<LOCALIDAD/>")
        trTrama.Append("<DEPARTAMENTO/>")
        trTrama.Append("<PROVINCIA/>")
        trTrama.Append("<CPOSTAL/>")
        trTrama.Append("<TELEFONO>043324456</TELEFONO>")
        trTrama.Append("<ANEXO/>")
        trTrama.Append("<FAX>NO TIENE</FAX>")
        trTrama.Append("</DIRECCION>")
        trTrama.Append("</DIRECCIONES>")
        trTrama.Append("<CODCLIEORV></CODCLIEORV>")
        trTrama.Append("</ACTCLIENTERs>")
        trTrama.Append("</Datos>")
        Dim dsAux As DataSet
        Using cadena As New StringReader("<Root>" + tramaXml + "</Root>")
            'Call EscribirLog(Response.IntegRes.Detalle.Datos.InnerXml)
            dsAux = New DataSet
            dsAux.ReadXml(cadena)

        End Using

        Dim CodigoClienteCoorporativo As String = String.Empty
        If dsAux.Tables(0).Rows.Count > 0 Then
            CodigoClienteCoorporativo = dsAux.Tables("Root").Rows(0)("CODCLIENTE").ToString.Trim
        End If

        For Each dato As DataRow In dsAux.Tables("Root").Rows
            Dim a As String
            a = dato.Item("CODCLIENTE").ToString
        Next
        For Each sede As DataRow In dsAux.Tables("SEDE").Rows
            Dim a As String
            a = sede.Item("SECUENCIAL").ToString
            sede.Item("SECUENCIAL") = "-1"
        Next

        For Each sede As DataRow In dsAux.Tables("SEDE").Rows
            Dim a As String
            a = sede.Item("SECUENCIAL").ToString 
        Next
    End Sub

    Protected Sub btnPrueba3_Click(sender As Object, e As EventArgs) Handles btnPrueba3.Click
        'Dim obcCotizacion As New bcCotizacion
        'Dim beCotizacionVersion As New beCotizacionVersion
        'Dim oUtilitario As New cls
        'Try
        '    obcCotizacion.
        'Catch ex As Exception

        'End Try
        Dim oemail As New beEmail
        Dim listaEmailcopia(1) As beEmailDato
        listaEmailcopia(0) = New beEmailDato With {.EmailDireccion = "BackgroundOutsourcing01@ferreyros.com.pe", .Otro = ""}
        listaEmailcopia(1) = New beEmailDato With {.EmailDireccion = "BackgroundOutsourcing01@ferreyros.com.pe", .Otro = ""}
        'Dim l_Maquinaria(0) As WcfLocal.beMaquinaria

        oemail.ListaEmailCopia = listaEmailcopia
        'oemail.EmailResponsableServicio = "BackgroundOutsourcing01@ferreyros.com.pe"
        oemail.RazonSocial = "Envío de prueba"
        oemail.CodigoCotizacionSap = "0002000664"
        oemail.CodigoCliente = "00000"
        oemail.NombreProducto = "Nombre Producto ...."
        oemail.TipoCSA = "Plan"
        oemail.NroItem = "5"

        Dim link(1) As beLink
        link(0) = New beLink With {.Nombre = "link 0", .Url = "#", .Tipo = "Tipo"}
        link(1) = New beLink With {.Nombre = "link 1", .Url = "#", .Tipo = "Tipo"}
        oemail.ListaLink = link

        'Dim oclsUtilitarios As New clsUtilitarios
        'Call oclsUtilitarios.EnvioEmailCotizador(oemail)

        ''Dim objenvio As New wcfEmailCotizador.IwcfEmailCotizadorClient
        ''Dim mensajeError As String = String.Empty
        ''Call objenvio.EnviarEmail(oemail, mensajeError)
    End Sub
 

    Public Function GenerarRandom() As String
        Dim strReturn As String = String.Empty
        strReturn = Now.Date.Year.ToString
        strReturn = String.Concat(strReturn, Now.Date.Month.ToString())
        strReturn = String.Concat(strReturn, Now.Date.Day.ToString())
        strReturn = String.Concat(strReturn, Now.Hour.ToString)
        strReturn = String.Concat(strReturn, Now.Minute.ToString())
        strReturn = String.Concat(strReturn, Now.Second.ToString())
        strReturn = String.Concat(strReturn, Now.Millisecond.ToString())

        Return strReturn
    End Function

    Protected Sub btnPrueba4_Click(sender As Object, e As EventArgs) Handles btnPrueba4.Click
        Dim obeProductoAccesorio As New beProductoAccesorio
        Dim odaProductoAccesorio As New daProductoAccesorio


        'obeProductoAccesorio.IdProductoAccesorio() As String
        obeProductoAccesorio.IdProducto = 1191
        obeProductoAccesorio.IdAccesorio = 300000001
        'obeProductoAccesorio.CodigoProductoAccesorio() As String
        obeProductoAccesorio.NombreProductoAccesorio = "Llanta repuestos A"
        obeProductoAccesorio.Cantidad = 1
        obeProductoAccesorio.UnidadMedida = "ST"
        obeProductoAccesorio.ValorLista = "1000.00"
        obeProductoAccesorio.MonedaValorLista = "USD"
        obeProductoAccesorio.FlatMostrarEspTecnica = "0"
        'obeProductoAccesorio.FlatEliminado() As String = String.Empty
        obeProductoAccesorio.UsuarioCreacion = "RSARMIENTO"
        Using cnnsql As New SqlClient.SqlConnection(strConexionCotizador)
            cnnsql.Open()
            odaProductoAccesorio.Insertar(cnnsql, obeProductoAccesorio, cnnsql.BeginTransaction)
        End Using

        'INFO  2014-02-14 14:10:17,052 644764ms bcCotizacion              LogProductoAccesorio           - 573670702: ,IdProductoAccesorio = ,IdProducto = 1191,
        '    IdAccesorio = 300000001,CodigoProductoAccesorio = ,NombreProductoAccesorio = Llanta repuestos A,Cantidad = 1,UnidadMedida = ST,
        '    ValorLista = 1000.00,MonedaValorLista = USD,FlatMostrarEspTecnica = ,FlatEliminado = ,UsuarioCreacion = RSARMIENTO

    End Sub
End Class