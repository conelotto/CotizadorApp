Imports Ferreyros.BECotizador
Public Class JQGridJsonResponse

    Dim lista As List(Of String) = Nothing
    Dim jqItem As JQGridItem = Nothing


    Private _pageCount As Integer
    Public Property PageCount() As Integer
        Get
            Return _pageCount
        End Get
        Set(ByVal value As Integer)
            _pageCount = value
        End Set
    End Property

    Private _currentPage As Integer
    Public Property CurrentPage() As Integer
        Get
            Return _currentPage
        End Get
        Set(ByVal value As Integer)
            _currentPage = value
        End Set
    End Property

    Private _recordCount As Integer
    Public Property RecordCount() As Integer
        Get
            Return _recordCount
        End Get
        Set(ByVal value As Integer)
            _recordCount = value
        End Set
    End Property

    Private _items As List(Of JQGridItem)
    Public Property Items() As List(Of JQGridItem)
        Get
            Return _items
        End Get
        Set(ByVal value As List(Of JQGridItem))
            _items = value
        End Set
    End Property

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
                   ByVal pRecordCount As Integer, ByVal pTipo As String, _
                   ByVal pTarifas As List(Of beTarifa))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        If pTipo = "T" Then
            For Each Rpt In pTarifas
                lista = New List(Of String)
                lista.Add(Rpt.id)
                lista.Add(Rpt.prefijo)
                lista.Add(Rpt.modelo)
                lista.Add(Rpt.modeloBase)
                lista.Add(Rpt.familia)
                lista.Add(Rpt.codigoPlan)
                lista.Add(Rpt.plan)
                lista.Add(Rpt.evento)
                lista.Add(Rpt.conFluidos)
                lista.Add(Rpt.aceites)
                lista.Add(Rpt.kitRepuestos)
                lista.Add(Rpt.fluidos)
                lista.Add(Rpt.servicioContratado)
                lista.Add(Rpt.SOS)
                lista.Add(Rpt.total)
                lista.Add(Rpt.eventosNueva)
                lista.Add(Rpt.eventosUsada)
                lista.Add(Rpt.kitRepuestosT)
                lista.Add(Rpt.fluidosT)
                lista.Add(Rpt.servicioContratadoT)
                lista.Add(Rpt.totalT)
                lista.Add(Rpt.tarifaUSDxH)
                jqItem = New JQGridItem(Rpt.id, lista)
                _items.Add(jqItem)
            Next
        End If

        If pTipo = "D" Then
            For Each Rpt In pTarifas
                lista = New List(Of String)
                lista.Add(Rpt.id)
                lista.Add(Rpt.prefijo)
                lista.Add(Rpt.modelo)
                lista.Add(Rpt.modeloBase)
                lista.Add(Rpt.familia)
                lista.Add(Rpt.serviceCategory)
                lista.Add(Rpt.rodetail)
                lista.Add(Rpt.compQty)
                lista.Add(Rpt.firstInterval)
                lista.Add(Rpt.nextInterval)
                lista.Add(Rpt.jodetail)
                lista.Add(Rpt.SOSPartNumber)
                lista.Add(Rpt.SOSDescription)
                lista.Add(Rpt.quantity)
                lista.Add(Rpt.replacement)
                lista.Add(Rpt.unitPrice)
                lista.Add(Rpt.extendedPrice)
                lista.Add(Rpt.sellEvent)
                lista.Add(Rpt.evento)
                lista.Add(Rpt.sell)
                jqItem = New JQGridItem(Rpt.id, lista)
                _items.Add(jqItem)
            Next
        End If


    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
                  ByVal pRecordCount As Integer, ByVal pPlantilla As List(Of bePlantilla))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pPlantilla
            lista = New List(Of String)
            lista.Add(Rpt.IdPlantilla)
            lista.Add(Rpt.CodigoModelo)
            lista.Add(Rpt.VersionModelo)
            lista.Add(Rpt.Nombre)
            lista.Add(Rpt.Items)
            lista.Add(Rpt.ItemsEmpleados)
            lista.Add(Rpt.Estado)
            jqItem = New JQGridItem(Rpt.IdPlantilla, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
                 ByVal pRecordCount As Integer, ByVal pCotizacion As List(Of beCotizacion))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        'Dim i As Integer = 0
        For Each Rpt In pCotizacion
            'i += 1
            lista = New List(Of String)
            lista.Add(Rpt.IdCotizacion)
            lista.Add(Rpt.IdCotizacionSap)
            lista.Add(Rpt.DescripSolicitante)
            lista.Add(Rpt.DescripResponsable)
            lista.Add(Rpt.FechaInicioValidez)
            lista.Add(Rpt.FechaFinalValidez)
            lista.Add(Rpt.NumeroOportunidad)
            lista.Add(Rpt.ItemOportunidad)
            lista.Add(Rpt.NombreEstado)
            jqItem = New JQGridItem(Rpt.IdCotizacion, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
             ByVal pRecordCount As Integer, ByVal pCotizacionContacto As List(Of beCotizacionContacto))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pCotizacionContacto
            lista = New List(Of String)
            lista.Add(Rpt.IdCotizacionContacto)
            lista.Add(Rpt.Cargo)
            lista.Add(Rpt.Nombres)
            lista.Add(Rpt.Direccion)
            lista.Add(Rpt.Email)
            lista.Add(Rpt.Telefono)
            jqItem = New JQGridItem(Rpt.IdCotizacionContacto, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
        ByVal pRecordCount As Integer, ByVal pProducto As List(Of beProducto))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)
        'Dim i As Integer = 0
        For Each Rpt In pProducto
            'i += 1
            lista = New List(Of String)
            lista.Add(Rpt.IdProducto)
            lista.Add(Rpt.IdPosicion)
            lista.Add(Rpt.IdProductoSap)
            lista.Add(Rpt.TipoProducto)
            lista.Add(Rpt.Descripcion)
            lista.Add(Rpt.ValorUnitario)
            lista.Add(Rpt.IdMonedaValorUnitario)
            lista.Add(Rpt.Cantidad)
            lista.Add(Rpt.Unidad)
            lista.Add(Rpt.ValorNeto)
            lista.Add(Rpt.IdMonedaValorNeto)
            lista.Add(Rpt.NombreEstado)
            jqItem = New JQGridItem(Rpt.IdProducto, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
      ByVal pRecordCount As Integer, ByVal pMaquinaria As List(Of beMaquinaria))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pMaquinaria

            lista = New List(Of String)
            lista.Add(Rpt.codigo)
            lista.Add(Rpt.item)

            If Rpt.familia = "0" Then
                lista.Add(Rpt.familiaOt)
            Else
                lista.Add(Rpt.familia)
            End If

            lista.Add(Rpt.modelo)

            If Rpt.modeloBase = "0" Then
                lista.Add(Rpt.modeloBaseOt)
            Else
                lista.Add(Rpt.modeloBase)
            End If

            If Rpt.prefijo = "0" Then
                lista.Add(Rpt.prefijoOt)
            Else
                lista.Add(Rpt.prefijo)
            End If

            lista.Add(Rpt.condicionMaquinaria)

            If Rpt.numeroSerie = "0" Then
                lista.Add(Rpt.numeroSerieOt)
            Else
                lista.Add(Rpt.numeroSerie)
            End If

            lista.Add(Rpt.horometroInicial)
            lista.Add(Rpt.fechaHorometro)
            lista.Add(Rpt.horasPromedioMensual)
            lista.Add(Rpt.horometroFinal)
            lista.Add(Rpt.descripRenovacion)
            lista.Add(Rpt.descripRenovacionValida)
            lista.Add(Rpt.departamento)
            lista.Add(Rpt.maquinaNueva)
            lista.Add(Rpt.renovacion)
            lista.Add(Rpt.renovacionValida)
            lista.Add(Rpt.familiaOt)
            lista.Add(Rpt.modeloBaseOt)
            lista.Add(Rpt.prefijoOt)
            lista.Add(Rpt.numeroMaquinas)
            lista.Add(Rpt.numeroSerieOt)
            lista.Add(Rpt.codDepartamento)
            lista.Add(Rpt.montoItem)
            jqItem = New JQGridItem(Rpt.item, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
      ByVal pRecordCount As Integer, ByVal pMaquinaria As List(Of beTablaMaestra))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pMaquinaria
            lista = New List(Of String)
            lista.Add(Rpt.IdSeccionCriterio)
            lista.Add(Rpt.IdSubSeccionCriterio)
            lista.Add(Rpt.IdCriterio)
            lista.Add(Rpt.IdSeccion)
            lista.Add(Rpt.Tipo)
            lista.Add(Rpt.PosicionInicial)
            lista.Add(Rpt.Nombre)
            lista.Add(Rpt.Opcional)
            lista.Add(Rpt.CambioPosicion)
            jqItem = New JQGridItem(Rpt.IdSeccion, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
          ByVal pRecordCount As Integer, ByVal pArchivoConfiguracion As List(Of beArchivoConfiguracion))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pArchivoConfiguracion
            lista = New List(Of String)
            lista.Add(Rpt.IdArchivoConfiguracion)
            lista.Add(Rpt.IdSeccionCriterio)
            lista.Add(Rpt.IdSubSeccionCriterio)
            lista.Add(Rpt.Tipo)
            lista.Add(Rpt.Codigo) 
            lista.Add(Rpt.Nombre)
            lista.Add(Rpt.Archivo)
            lista.Add(Rpt.Valor) 
            jqItem = New JQGridItem(Rpt.IdArchivoConfiguracion, lista)
            _items.Add(jqItem)
        Next

    End Sub

    Public Sub New(ByVal pPageCount As Integer, ByVal pCurrentPage As Integer, _
          ByVal pRecordCount As Integer, ByVal pHomologacion As List(Of beHomologacion))

        _pageCount = pPageCount
        _currentPage = pCurrentPage
        _recordCount = pRecordCount
        _items = New List(Of JQGridItem)

        For Each Rpt In pHomologacion
            lista = New List(Of String)
            lista.Add(Rpt.IdHomologacion)
            lista.Add(Rpt.Tabla)
            lista.Add(Rpt.Descripcion)
            lista.Add(Rpt.ValorSap)
            lista.Add(Rpt.ValorCotizador) 
            jqItem = New JQGridItem(Rpt.IdHomologacion, lista)
            _items.Add(jqItem)
        Next

    End Sub
End Class
