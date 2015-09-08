Public Class objBBDD

    Public Structure StoreProcedure
        'Aprobador --------------------------------------------------
        Public Shared AprobadorBuscar As String = "uspAprobadorBuscar"
        Public Shared AprobadorUsuarioBuscar As String = "uspAprobadorUsuarioBuscar"
        Public Shared AprobadorUsuarioBuscarMatricula As String = "uspAprobadorUsuarioBuscarMatricula"
        Public Shared AprobadorMantenimiento As String = "uspAprobadorMantenimiento"
        Public Shared AprobadorUsuarioInsertar As String = "uspAprobadorUsuarioInsertar"
        Public Shared AprobadorAnular As String = "uspAprobadorAnular"
        'Tarifas --------------------------------------------------
        Public Shared TarifaBuscar As String = "uspTarifaBuscar"
        Public Shared TarifaMantenimiento As String = "uspTarifaMantenimiento"
        Public Shared TarifaCsaModBPlanPrefPM As String = "uspTarifaCsaModBPlanPrefPM"
        Public Shared TarifaModeloPlanEvento As String = "uspTarifaModeloPlanEvento"
        'Detalle Partes --------------------------------------------------
        Public Shared DetallePartesBuscar As String = "uspDetallePartesBuscar"
        Public Shared DetallePartesMantenimiento As String = "uspDetallePartesMantenimiento"
        'TablaMaestra --------------------------------------------------
        Public Shared TablaMaestraBuscarGrupo As String = "uspTablaMaestraBuscarGrupo"
        'Cotizacion  --------------------------------------------------

        Public Shared ProductoActualizar As String = "uspProductoActualizar"
        Public Shared CotizacionActualizar As String = "uspCotizacionActualizar"

        Public Shared CotizacionConsulta As String = "uspCotizacionConsulta"
        Public Shared CotizacionListar As String = "uspCotizacionListar"
        Public Shared MaquinariaListar As String = "uspMaquinariaListar"
        Public Shared ProductoCsaActualizar As String = "uspProductoCsaActualizar"
        Public Shared MaquinariaActualizar As String = "uspMaquinariaActualizar"
        Public Shared CotizacionCsaParametros As String = "uspCotizacionCsaParametros"
        Public Shared CotizacionContactoListar As String = "uspCotizacionContactoListar"
        Public Shared ProductoListar As String = "uspProductoListar"
        Public Shared ProductoCSAListar As String = "uspProductoCSAListar"

        'Seccion     --------------------------------------------------
        Public Shared SeccionesCotizacionListar As String = "uspSeccionesCotizacionListar"
        Public Shared SeccionMantenimiento As String = "uspSeccionMantenimiento"
        Public Shared CriterioMantenimiento As String = "uspCriterioMantenimiento"

        Public Shared SeccionCriterioListar As String = "uspSeccionCriterioListar"
        Public Shared SeccionCriterioInsertar As String = "uspSeccionCriterioInsertar"
        Public Shared ListaSeccionInsertar As String = "uspListaSeccionInsertar"
        Public Shared ListaSeccionLimpiar As String = "uspListaSeccionLimpiar"
        Public Shared ListaSeccionBusqueda As String = "uspListaSeccionBusqueda"

        ' Detalle Partes
        Public Shared uspDetallePartesBuscarLlave As String = "uspDetallePartesBuscarLlave"

        '===================================================================================
        'Cotizacion Prime
        Public Shared uspCotizacionBuscarId As String = "uspCotizacionBuscarId"
        Public Shared uspCotizacionBusqueda As String = "uspCotizacionBusqueda"
        Public Shared uspCotizacionBuscarEnAprobacion As String = "uspCotizacionBuscarEnAprobacion"
        Public Shared uspCotizacionReportePrime As String = "uspCotizacionReportePrime"
        Public Shared uspCotizacionDatosDocumento As String = "uspCotizacionDatosDocumento"

        'Cotizacion Version
        Public Const uspCotizacionVersionInsertar As String = "uspCotizacionVersionInsertar"
        Public Const uspCotizacionVersionBuscarIdCotizacionSap As String = "uspCotizacionVersionBuscarIdCotizacionSap"
        Public Const UspCotizacionVersionCrearVersion As String = "UspCotizacionVersionCrearVersion"
        Public Const uspCotizacionVersionActualizarArchivo As String = "uspCotizacionVersionActualizarArchivo"

        'Cotizacion Contacto
        Public Shared uspCotizacionContactoBuscarIdCotizacion As String = "uspCotizacionContactoBuscarIdCotizacion"

        'Producto
        Public Shared uspProductoBuscarIdCotizacion As String = "uspProductoBuscarIdCotizacion"
        Public Shared UspCotizacionVersionProductoBuscarIdCotVers As String = "UspCotizacionVersionProductoBuscarIdCotVers"
        Public Shared uspProductoBuscarNumeroCotizacion As String = "uspProductoBuscarNumeroCotizacion"

        'Tabla Maestra
        Public Shared uspTablaMaestraBuscarId As String = "uspTablaMaestraBuscarId"
        Public Shared uspTablaMaestraBuscarGrupo As String = "uspTablaMaestraBuscarGrupo"

        'Producto Prime
        Public Shared uspProductoPrimeBuscarId As String = "uspProductoPrimeBuscarId"
        Public Shared uspProductoAdicionalBuscarIdProducto As String = "uspProductoAdicionalBuscarIdProducto"
        Public Shared uspProductoPrimeEliminar As String = "uspProductoPrimeEliminar"
        Public Shared uspProductoAccesorioBuscarIdProducto As String = "uspProductoAccesorioBuscarIdProducto"

        'SeccionCriterio
        Public Shared uspSeccionCriterioBuscarIdSeccion As String = "uspSeccionCriterioBuscarIdSeccion"

        'ArchivoConfiguracion
        Public Shared uspArchivoConfiguracionBuscarPorCriterio As String = "uspArchivoConfiguracionBuscarPorCriterio"
        Public Shared uspArchivoConfiguracionBuscarId As String = "uspArchivoConfiguracionBuscarId"
        Public Shared uspArchivoConfiguracionInsertar As String = "uspArchivoConfiguracionInsertar"
        Public Shared uspArchivoConfiguracionEliminar As String = "uspArchivoConfiguracionEliminar"
        Public Shared uspArchivoConfiguracionActualizar As String = "uspArchivoConfiguracionActualizar"
        Public Shared uspArchivoConfiguracionBuscarArchivo As String = "uspArchivoConfiguracionBuscarArchivo"
        Public Shared uspArchivoConfiguracionBuscarArchivoProd As String = "uspArchivoConfiguracionBuscarArchivoProd"
        Public Shared uspArchivoConfiguracionBuscarGeneral As String = "uspArchivoConfiguracionBuscarGeneral"
        Public Shared uspArchivoConfiguracionBuscarIdSeccionCriterio As String = "uspArchivoConfiguracionBuscarIdSeccionCriterio"
        Public Const uspArchivoConfiguracionBuscarCodigoYSeccion As String = "uspArchivoConfiguracionBuscarCodigoYSeccion"

        'Marcador
        Public Shared uspMarcadorListar As String = "uspMarcadorListar"

        'MarcadorCotizacion
        Public Shared uspMarcadorCotizacionInsertar As String = "uspMarcadorCotizacionInsertar"
        Public Shared uspMarcadorCotizacionActualizar As String = "uspMarcadorCotizacionActualizar"
        Public Shared uspMarcadorCotizacionBuscarIdArchivo As String = "uspMarcadorCotizacionBuscarIdArchivo"

        'Producto Alquiler
        Public Shared uspProductoAlquilerBuscarId As String = "uspProductoAlquilerBuscarId"

        'Producto Alquiler Tarifa
        Public Const uspProductoAlquilerTarifaBuscarIdProducto As String = "uspProductoAlquilerTarifaBuscarIdProducto"

        'Producto Caracteristica
        Public Const uspProductoCaracteristicaBuscarIdProducto As String = "uspProductoCaracteristicaBuscarIdProducto"

        'Producto Solucion Combinada
        Public Const uspProductoSolucionCombinadaBuscarId As String = "uspProductoSolucionCombinadaBuscarId"

        'MaquinariaEnvioSap
        Public Shared uspMaquinariaEnvioSapInsertar As String = "uspMaquinariaEnvioSapInsertar"

        'Llave
        Public Const uspLLaveBuscarCodigoLinea As String = "uspLLaveBuscarCodigoLinea"

        'TarifaRS
        Public Const UspTarifasRSBuscarCombinacionLLave As String = "UspTarifasRSBuscarCombinacionLLave"
        'Homologacion
        Public Shared uspHomologacionBuscarTabla As String = "uspHomologacionBuscarTabla"
        Public Shared uspHomologacionListar As String = "uspHomologacionListar"
        Public Shared uspHomologacionInsertar As String = "uspHomologacionInsertar"
        Public Shared uspHomologacionActualizar As String = "uspHomologacionActualizar"
        Public Shared uspHomologacionEliminar As String = "uspHomologacionEliminar"
        '===================================================================================

        'Querys
        Public Shared uspQueryMantenimiento As String = "uspQueryMantenimiento"

    End Structure

End Class
